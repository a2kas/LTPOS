using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS_display.Exceptions;
using POS_display.Models.General;
using POS_display.Models.Partner;
using POS_display.Repository.HomeMode;
using POS_display.Repository.Partners;
using POS_display.Views.HomeMode;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using Tamroutilities.Client;

namespace POS_display.Presenters.HomeMode
{
    public class HomeModePresenter : IHomeModePresenter
    {
        #region Members
        private readonly IHomeModeAcitvateView _view;
        private readonly IHomeModeRepository _homeModeRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ITamroClient _tamroClient;
        private PartnerViewData _partner;
        private string _base64ClientSignature;
        private string _pathToClientSignature;
        private decimal _deliveryOrderId;
        #endregion

        #region Constructor
        public HomeModePresenter(
            IHomeModeAcitvateView view,
            IHomeModeRepository homeModeRepository,
            IPartnerRepository partnerRepository,
            ITamroClient tamroClient)
        {
            _view = view ?? throw new ArgumentNullException();
            _homeModeRepository = homeModeRepository ?? throw new ArgumentNullException();
            _partnerRepository =  partnerRepository ?? throw new ArgumentNullException();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();

            Reset();
        }
        #endregion

        #region Public methods
        public void EnableButtons()
        {
            _view.StartProcess.Enabled = _partner != null && !string.IsNullOrEmpty(_base64ClientSignature) && _deliveryOrderId != 0m;
            _view.SendToTerminal.Enabled = _partner != null && string.IsNullOrEmpty(_base64ClientSignature);
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(_view.Address.Text))
                throw new HomeModeException("Adresas negali būti tuščias!\nRedaguokite kliento duomenis");

            if (string.IsNullOrWhiteSpace(_view.City.Text))
                throw new HomeModeException("Miestas negali būti tuščias!\nRedaguokite kliento duomenis");

            if (string.IsNullOrWhiteSpace(_view.CountryCode.Text))
                throw new HomeModeException("Šalies kodas negali būti tuščias!\nRedaguokite kliento duomenis");

            if (string.IsNullOrWhiteSpace(_view.PostIndex.Text))
                throw new HomeModeException("Pašto kodas negali būti tuščias!\nRedaguokite kliento duomenis");

            if (helpers.ParseNumberFromString(_view.PostIndex.Text).Length != 5)
                throw new HomeModeException("Blogas pašto kodas\nPašto kodas turi sudaryti 5 skaičiai!\nRedaguokite kliento duomenis");

            if (string.IsNullOrWhiteSpace(_view.PhoneNumber.Text) && string.IsNullOrWhiteSpace(_view.Email.Text))
                throw new HomeModeException("Tel.nr arba El. paštas privalo būti nurodytas!\nRedaguokite kliento duomenis");
        }

        public async Task SetPartner(PartnerViewData partner) 
        {
            if (_partner?.Id != partner.Id)
                await CancelHomeDeliverOrder();

            _partner = partner;
            _view.BuyerName.Text = partner.Name;
            _view.Address.Text = partner.Address;
            _view.PhoneNumber.Text = partner.Phone;
            _view.Email.Text = partner.Email;
            _view.PostIndex.Text = partner.PostIndex;
            _view.CountryCode.Text = partner.Agent;
            _view.City.Text = partner.City;
        }

        public PartnerViewData GetPartner()
        {
            return _partner;
        }

        public void SetSignature(string signature)
        {
            _base64ClientSignature = signature;
        }

        public async Task CancelHomeDeliverOrder() 
        {
            await _homeModeRepository.DeleteHomeDeliveryOrder(Program.Display1.PoshItem.Id);
            Reset();
        }

        public async Task CreateHomeDeliverOrder()
        {
            if (_partner != null && !string.IsNullOrEmpty(_base64ClientSignature) && _deliveryOrderId == 0m)
            {
                if (string.IsNullOrEmpty(_pathToClientSignature)) 
                {
                    var response = await _tamroClient.PostAsync<UploadSignatureResponse>(Session.CKasV1PostPartnerUploadSignature,
                        JObject.Parse(JsonConvert.SerializeObject(new UploadSignatureRequest()
                        {
                            PartnerId = _partner.Id,
                            Base64Signature = _base64ClientSignature
                        })));

                    _pathToClientSignature = response.PathToSignature;
                }

                _deliveryOrderId = await _homeModeRepository.CreateHomeDeliveryOrder(
                    Program.Display1.PoshItem.Id,
                    _partner.Id,
                    _pathToClientSignature);
            }
        }

        public async Task SetPartnerAgreementToSaveData()
        {
            if (_partner != null && !string.IsNullOrEmpty(_base64ClientSignature))
            {
                var hasAgreementToSaveData = await _partnerRepository.HasPartnerAgreement(_partner.Id);
                if (!hasAgreementToSaveData)
                {
                    // Upload signature to central DB
                    var response = await Session.TamroGateway.PostAsync<UploadSignatureResponse>(Session.CKasV1PostPartnerUploadSignature,
                        JObject.Parse(JsonConvert.SerializeObject(new UploadSignatureRequest()
                        {
                            PartnerId = _partner.Id,
                            Base64Signature = _base64ClientSignature
                        })));

                    _pathToClientSignature = response.PathToSignature;

                    // Set partner agreement in central DB
                    await _tamroClient.PostAsync(Session.CKasV1PostPartnerSetAgreement,
                       JObject.Parse(JsonConvert.SerializeObject(new SetAgreementRequest()
                       {
                           PartnerId = _partner.Id,
                           PathToSignature = _pathToClientSignature
                       })));

                    // Set partner agreement in local DB
                    await _partnerRepository.SetPartnerAgreement(_partner.Id, _pathToClientSignature);

                    // Set mark update command in central DB
                    var _ = _tamroClient.PostAsync(Session.CKasV1PostPharmacyMarkCommand,
                                  JObject.Parse(JsonConvert.SerializeObject(new MarkCommandRequest()
                                  {
                                      ClientId = Session.SystemData.kas_client_id,
                                      CommandPrefix = $"insert into partner_agreement values ({_partner.Id}",
                                      FromDate = DateTime.Now
                                  })));
                }
            }
        }

        public FlowDocument CreateAgreementDocument()
        {
            var doc = new FlowDocument();

            Paragraph p = new Paragraph(new Run("Kliento informacija"));
            p.FontSize = 24;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Vardas: {_partner.Name}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Telefonas: {_partner.Phone}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"El.pašto adresas: {_partner.Email}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Adresas: {_partner.Address}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Miestas: {_partner.City}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Pašto kodas: {_partner.PostIndex}"));
            p.FontSize = 18;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            return doc;
        }
        #endregion

        #region Private methods
        private void Reset() 
        {
            _base64ClientSignature = string.Empty;
            _pathToClientSignature = string.Empty;
            _deliveryOrderId = 0m;
        }
        #endregion
    }
}
