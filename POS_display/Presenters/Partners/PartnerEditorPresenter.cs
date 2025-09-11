using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using POS_display.Exceptions;
using POS_display.Models.General;
using POS_display.Models.Partner;
using POS_display.Repository.Partners;
using POS_display.Utils;
using POS_display.Utils.Logging;
using POS_display.Views.Partners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display.Presenters.Partners
{
    public class PartnerEditorPresenter : BasePresenter, IPartnerEditorPresenter
    {
        #region Members
        private readonly IPartnerEditorView _view;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ITamroClient _tamroClient;
        private const string _supplierTypeModule = "T";
        private const string _buyerTypeModule = "P";
        private const string _creditorType = "C";
        private const string _debitorType = "D";
        private const string _creditorAndDebitorType = "CD";
        private const string _emptyCompanyCode = "ND";
        private const string _emptyVatCode = "--";
        #endregion

        public PartnerEditorPresenter(
            IPartnerEditorView view,
            IPartnerRepository partnerRepository,
            ITamroClient tamroClient)
        {
            _view = view ?? throw new ArgumentNullException();
            _partnerRepository = partnerRepository ?? throw new ArgumentNullException();
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
        }

        #region Public methods
        public async Task Init()
        {
            Dictionary<string, string> types = new Dictionary<string, string>
            {
                {"D", "Debitorius"},
                {"CD", "Kreditorius/Debitorius"}
            };

            _view.Type.DataSource = new BindingSource(types, null);
            _view.Type.DisplayMember = "Value";
            _view.Type.ValueMember = "Key";
            _view.Type.SelectedValue = _debitorType;

            SortedDictionary<long, string> supplierTypesData = new SortedDictionary<long, string>
            {
                { 0, string.Empty }
            };

            SortedDictionary<long, string> buyerTypesData = new SortedDictionary<long, string> {};

            var supplierTypes = await _partnerRepository.GetPartnerByModule(_supplierTypeModule);
            foreach (var supplierType in supplierTypes)
            {
                supplierTypesData.Add(supplierType.Id, supplierType.Name);
            }
            _view.SupplierType.DataSource = new BindingSource(supplierTypesData, null);
            _view.SupplierType.DisplayMember = "Value";
            _view.SupplierType.ValueMember = "Key";

            var buyerTypes = await _partnerRepository.GetPartnerByModule(_buyerTypeModule);
            foreach (var buyerType in buyerTypes)
            {
                buyerTypesData.Add(buyerType.Id, buyerType.Name);
            }
            _view.BuyerType.DataSource = new BindingSource(buyerTypesData, null);
            _view.BuyerType.DisplayMember = "Value";
            _view.BuyerType.ValueMember = "Key";
            _view.City.ReadOnly = _view.PartnerEditConfig?.CityValueReadOnly ?? true;
            SetValues();
        }

        public async Task Save()
        {
            var errorMessage = await GetErrorMessage();
            if (!string.IsNullOrEmpty(errorMessage))            
                throw new PartnerException(errorMessage);

            var partner = GetPartnerEntity();

            if (partner.Id != 0)
            {
                await _tamroClient.PatchAsync<dynamic>(Session.CKasV1PatchPartnerUpdate, 
                    JObject.Parse(JsonConvert.SerializeObject(partner)));

                await _partnerRepository.UpdatePartner(partner);

                Serilogger.GetLogger().Information($"Partner ID: {partner.Id} has been updated by pharamacy." +
                    $" Partner data: {partner.ToJsonString()}");
            }
            else
            {
                var partnerId = await _tamroClient.PostAsync<decimal>(Session.CKasV1PostPartnerInsert,
                    JObject.Parse(JsonConvert.SerializeObject(partner)));
                partner.Id = partnerId.ToLong();

                await _partnerRepository.InsertPartner(partner);

                var _ = _tamroClient.PostAsync<dynamic>(Session.CKasV1PostPharmacyMarkCommand,
                    JObject.Parse(JsonConvert.SerializeObject(new MarkCommandRequest()
                    {
                        ClientId = Session.SystemData.kas_client_id,
                        CommandPrefix = $"insert into partners values ({partner.Id}",
                        FromDate = DateTime.Now
                    })));

                Serilogger.GetLogger().Information($"Partner ID: {partner.Id} has been created by pharamacy." +
                    $" Partner data: {partner.ToJsonString()}");
            }
            _view.Id = partner.Id;
        }

        public async Task Load(decimal partnerId)
        {
            var partner = await _partnerRepository.GetDebtorById(partnerId);
            if (partner != null)
            {
                _view.Id = partnerId.ToLong();
                _view.PartnerName.Text = partner.Name;
                _view.CompanyCode.Text = partner.ECode;
                _view.VatCode.Text = partner.TCode;
                _view.Email.Text = partner.Email;
                _view.Phone.Text = partner.Phone;
                _view.Address.Text = partner.Address;
                _view.City.Text = partner.City;
                _view.PostIndex.Text = partner.PostIndex;
                _view.Comment.Text = partner.Descrip;
                _view.CountryCode.Text = partner.Agent;
                _view.Fax.Text = partner.Fax;
                _view.BuyerType.SelectedIndex = GetBuyerComboxIndexByBuyerId(partner.DebTypeId);
                _view.Type.SelectedValue = partner.Type;
            }
            else
                Clear();

            EnableControls();
        }

        public void SetValues() 
        {
            if (_view.Type.SelectedValue.ToString() == _creditorType) 
            {
                _view.BuyerType.SelectedIndex = 0;
            }
            else if (_view.Type.SelectedValue.ToString() == _debitorType) 
            {
                _view.SupplierType.SelectedIndex = 0;
            } 
            else
            {
                _view.SupplierType.SelectedIndex = 0;
                _view.BuyerType.SelectedIndex = 0;
            }

            _view.CompanyCode.Text = "ND";
            _view.VatCode.Text = "--";

            EnableControls();
        }
        #endregion

        #region Private methods
        private async Task<string> GetErrorMessage() 
        {
            string message = string.Empty;

            if (AnyInputContainsNotAllowedChar())
                return "Įvestas neleidžiamas simbolis ' ";

            if (string.IsNullOrWhiteSpace(_view.PartnerName.Text))
                return "Pavadinimas privalo būti užpildytas!";

            if (string.IsNullOrWhiteSpace(_view.CompanyCode.Text))
                return "Įmonės kodas privalo būti užpildytas! Jeigu nėra tada įrašykite 'ND'";

            if (string.IsNullOrWhiteSpace(_view.VatCode.Text))
                return "PVM mokėtojo privalo būti užpildytas! Jeigu nėra tada įrašykite '--'";

            if (string.IsNullOrWhiteSpace(_view.Address.Text))
                return "Adresas privalo būti užpildytas!";

            if (string.IsNullOrWhiteSpace(_view.PostIndex.Text))
                return "Pašto kodas privalo būti užpildytas!";

            if (string.IsNullOrWhiteSpace(_view.CountryCode.Text))
                return "Šalies kodas privalo būti užpildytas!";

            if (!_view.CompanyCode.Text.All(char.IsDigit) && _view.CompanyCode.Text != "ND")
                return "Įmonės kodas gali būti sudarytas tik iš skaitmenų! Jeigu nėra įmonės kodo tada įrašykite 'ND'";

            if (!Regex.IsMatch(_view.VatCode.Text, "^[A-Za-z]{2}[0-9]*$") && _view.VatCode.Text != "--")
                return "Blogas PVM mokėtojo kodo formatas!\n" +
                    " Kodas turi prasidėti šalies kodu ir sekti skaičiai pvz.: LT123456 \n" +
                    " Jeigu nėra PVM mokėtojo kodo tada įrašykite '--'";

            if (_view.Id == 0)
            {
                var partners = await _tamroClient.GetAsync<List<Partner>>($"api/v1/ckas/partners?ECode={_view.CompanyCode.Text}");
                if (partners != null && partners.Count > 0 && _view.CompanyCode.Text != _emptyCompanyCode)
                    return $"Partneris su įmonės kodu: '{_view.CompanyCode.Text}' jau egzistuoja centrinėje duomenų bazėje.\n" +
                        $" Jeigu norite naudoti šį partnerį atlikite KAS naujinimus.";

                partners = await _tamroClient.GetAsync<List<Partner>>($"api/v1/ckas/partners?TCode={_view.VatCode.Text}");
                if (partners != null && partners.Count > 0 && _view.VatCode.Text != _emptyVatCode)
                    return $"Partneris su mokėtojo kodu: '{_view.VatCode.Text}' jau egzistuoja centrinėje duomenų bazėje.\n" +
                        $" Jeigu norite naudoti šį partnerį atlikite KAS naujinimus.";
            }

            return message;
        }

        private bool AnyInputContainsNotAllowedChar()
        {
            if (_view.PartnerName.Text.Contains("'") ||
                _view.CompanyCode.Text.Contains("'") ||
                _view.VatCode.Text.Contains("'") ||
                _view.Address.Text.Contains("'") ||
                _view.PostIndex.Text.Contains("'") ||
                _view.CountryCode.Text.Contains("'"))
            {
                return true;
            }

            return false;
        }

        private void EnableControls()
        {
            _view.SupplierType.Enabled = _view.Type.SelectedValue.ToString() == _creditorType || _view.Type.SelectedValue.ToString() == _creditorAndDebitorType;
            _view.BuyerType.Enabled = _view.Type.SelectedValue.ToString() == _debitorType || _view.Type.SelectedValue.ToString() == _creditorAndDebitorType;
        }

        private void Clear()
        {
            _view.PartnerName.Text = string.Empty;
            _view.CompanyCode.Text = string.Empty;
            _view.VatCode.Text = string.Empty;
            _view.Email.Text = string.Empty;
            _view.Phone.Text = string.Empty;
            _view.Address.Text = string.Empty;
            _view.City.Text = string.Empty;
            _view.PostIndex.Text = string.Empty;
            _view.Comment.Text = string.Empty;
            _view.CountryCode.Text = string.Empty;
            _view.Fax.Text = string.Empty;
            _view.SupplierType.SelectedIndex = 0;
            _view.BuyerType.SelectedIndex = 0;
        }

        private Partner GetPartnerEntity() 
        {
            return new Partner()
            {
                Id = _view.Id,
                Name = _view.PartnerName.Text,
                Type = _view.Type.SelectedValue?.ToString() ?? string.Empty,
                ECode = _view.CompanyCode.Text,
                TCode = _view.VatCode.Text,
                Address = _view.Address.Text,
                City = _view.City.Text,
                PostIndex = _view.PostIndex.Text,
                Email = _view.Email.Text,
                Agent = _view.CountryCode.Text,
                Phone = _view.Phone.Text,
                Fax = _view.Fax.Text,
                CredTypeId = _view.SupplierType.SelectedValue?.ToString() ?? string.Empty,
                DebTypeId = _view.BuyerType.SelectedValue?.ToString() ?? string.Empty,
                Descrip = _view.Comment.Text
            };
        }

        private int GetBuyerComboxIndexByBuyerId(string id)
        {
            int index = 0;
            if (string.IsNullOrWhiteSpace(id))
                return index;

            foreach (var item in _view.BuyerType.Items)
            {
                if(((KeyValuePair<long, string>)item).Key == id.ToLong())
                    return index;
                index++;
            }
            return index;
        }
        #endregion
    }
}
