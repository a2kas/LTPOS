using POS_display.Models.SalesOrder;
using POS_display.Repository.SalesOrder;
using POS_display.Views.SalesOrder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using POS_display.Models.General;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Utils.Email;
using System.Linq;
using System.Windows.Documents;
using System.Windows;
using Microsoft.AspNetCore.Mvc;

namespace POS_display.Presenters.SalesOrder
{
    public class SalesOrderPresenter : BasePresenter, ISalesOrderPresenter
    {
        #region Members
        private readonly ISalesOrderView _view;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private const int IdLength = 11;
        private const string _emailSubject = "Pranešimas apie pardavimą į namus";
        private readonly string _sendEmail;
        private TransferResult _transferResult;
        #endregion

        #region Constructor
        public SalesOrderPresenter(ISalesOrderView view, ISalesOrderRepository salesOrderRepository)
        {
            _view = view ?? throw new ArgumentNullException();
            _salesOrderRepository = salesOrderRepository ?? throw new ArgumentNullException();
            _sendEmail = Session.Develop ? Session.getParam("EMAIL_TEST", "SALESORDER") : Session.getParam("EMAIL", "SALESORDER");
            _view.Country.SelectedIndex = 0;
        }
        #endregion

        #region Public methods
        public void EnableControls() 
        {
            _view.EditClientData.Enabled = _view.FocusedClient != null;
            _view.PrintAgreement.Enabled = _view.FocusedClient != null;
            _view.SendToTerminal.Enabled = _view.FocusedClient != null;
        }
        public async Task LoadClientData(string jsonData)
        {
            _view.Clients = await Session.RemotePharmacyGateway.PostCallApi<List<ClientData>>(jsonData);
        }

        public async Task TransferFromRemotePharmacy()
        {
            var transferResult = new TransferResult
            {
                ClientId = 0,
                ExternalID = 0,
                InternalID = 0
            };

            try
            {
                var result = await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
                {
                    method = "exporttopharmacy",
                    kasclientid = Session.SystemData.kas_client_id.ToString(CultureInfo.InvariantCulture),
                    productid = _view.BarcodeModel.ProductId.ToString(CultureInfo.InvariantCulture),
                    qty = _view.QtyToTransfer.ToString(CultureInfo.InvariantCulture)
                }.ToJsonString());
                transferResult.ExternalID = result != null ? result.export_to_pharmacy : 0;
                if (transferResult.ExternalID.ToString(CultureInfo.InvariantCulture).Length != IdLength)
                {
                    throw new Exception(GetMessageText((Enumerator.SalesOrderResult)transferResult.ExternalID.ToInt()));
                }

                result = await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
                {
                    method = "getkasclientid"
                }.ToJsonString());

                string kasClientId = result != null ? result.kas_client_id.ToString() : string.Empty;
                if (string.IsNullOrEmpty(kasClientId))
                    throw new Exception("Nutolusi vaistinė nepasiekiama! užsakymo atlikti negalima.");

                var importDocHid = await _salesOrderRepository
                    .ImportToPharmacy(_view.BarcodeModel.ProductId, _view.QtyToTransfer, kasClientId.ToDecimal());
                transferResult.InternalID = importDocHid;

                if (importDocHid.ToString(CultureInfo.InvariantCulture).Length != IdLength)
                {
                    throw new Exception(GetMessageText((Enumerator.SalesOrderResult)importDocHid.ToInt()));
                }

                var clientId = await SaveClientData();
                if (clientId != "0")
                {
                    await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
                    {
                        method = "saveclientdoc",
                        hid = transferResult.ExternalID,
                        externalhid = transferResult.InternalID,
                        poshid = _view.PoshItem.Id,
                        posdid = _view.BarcodeModel.PosdId,
                        clientid = clientId
                    }.ToJsonString());
                }
                else
                    throw new Exception(GetMessageText(Enumerator.SalesOrderResult.SAVE_CLIENT_ERROR));

                _transferResult = transferResult;

                await SendEmailToRemotePharmacy(transferResult);
            }
            catch (Exception)
            {
                await CancelTransfer(transferResult);
                _transferResult = null;
                throw;
            }
        }

        public string ValidateClientData()
        {
            return new ClientData
            {
                Name = _view.ClientName.Text,
                Surename = _view.Surename.Text,
                Phone = _view.Phone.Text,
                Email = _view.Email.Text,
                Address = _view.Address.Text,
                City = _view.City.Text,
                PostCode = _view.PostCode.Text,
                Country = _view.Country.SelectedItem?.ToString() ?? string.Empty
            }.Validate();
        }

        public void ClearCientDataForm()
        {
            _view.FocusedClient = null;
            _view.ClientName.Text = string.Empty;
            _view.Surename.Text = string.Empty;
            _view.Phone.Text = string.Empty;
            _view.Email.Text = string.Empty;
            _view.Address.Text = string.Empty;
            _view.City.Text = string.Empty;
            _view.PostCode.Text = string.Empty;
            _view.Country.SelectedIndex = 0;
            _view.Comment.Text = string.Empty;
            _view.CustomerSignature = string.Empty;
        }

        public void EnableInputFields(bool apply)
        {
            _view.ClientName.Enabled = apply;
            _view.Surename.Enabled = apply;
            _view.Phone.Enabled = apply;
            _view.Email.Enabled = apply;
            _view.Address.Enabled = apply;
            _view.City.Enabled = apply;
            _view.PostCode.Enabled = apply;
            _view.Country.Enabled = apply;
            _view.Comment.Enabled = apply;
        }

        public FlowDocument CreateAgreementDocument()
        {
            var doc = new FlowDocument();

            Paragraph p = new Paragraph(new Run("Kliento sutikimas"));
            p.FontSize = 24;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Vardas: {_view.ClientName.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Pavardė: {_view.Surename.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Telefonas: {_view.Phone.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"El.paštas: {_view.Email.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Adresas: {_view.Address.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Miestas: {_view.City.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Pašto kodas: {_view.PostCode.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Šalis: {_view.Country.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Komentaras: {_view.Comment.Text}"));
            p.FontSize = 14;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Sutinku, kad receptinė prekė būtų pristatyta nurodytu adresu per 2-4 d.d."));
            p.FontSize = 14;
            p.FontStyle = FontStyles.Oblique;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Patvirtinu, kad nurodyta informacija yra teisinga"));
            p.FontSize = 14;
            p.FontStyle = FontStyles.Oblique;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);
            return doc;
        }

        public async Task UpdatePosDetailID(long posDetailID)
        {
            if (_transferResult != null)
            {
                await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
                {
                    method = "updateposdetail",
                    hid = _transferResult.ExternalID,
                    externalhid = _transferResult.InternalID,
                    poshid = _view.PoshItem.Id,
                    posdid = posDetailID
                }.ToJsonString());
            }        
        }

        public async Task<string> SaveClientData()
        {
            var clientId = _view.FocusedClient?.ID ?? string.Empty;
            var result = await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
            {
                method = "saveclientdata",
                clientid = clientId,
                name = _view.ClientName.Text,
                surename = _view.Surename.Text,
                phone = _view.Phone.Text,
                email = _view.Email.Text,
                address = _view.Address.Text,
                city = _view.City.Text,
                postcode = _view.PostCode.Text,
                country = _view.Country.SelectedItem?.ToString() ?? string.Empty,
                comment = _view.Comment.Text,
                signature = _view.CustomerSignature
            }.ToJsonString());
            return result != null ? result.id.ToString() : "0";
        }

        public void SetFocusedClient(ClientData client)
        {
            _view.FocusedClient = client;
        }
        #endregion

        #region Private methods
        private async Task SendEmailToRemotePharmacy(TransferResult tr)
        {
            var emailUtils = Program.ServiceProvider.GetRequiredService<IEmailUtils>();
            await emailUtils?.SendEmail(_emailSubject, await FormatEmailBody(tr), _sendEmail);
        }

        private async Task<string> FormatEmailBody(TransferResult tr)
        {
            var productName = await _salesOrderRepository.GetProductName(_view.BarcodeModel.ProductId.ToLong());

            string body = "<b>Vaistinės informacija:</b>";
            body += $"<br/> KAS ID: {Session.SystemData.kas_client_id}";
            body += $"<br/> Pavadinimas: {Session.SystemData.name}";
            body += $"<br/> Adresas: {Session.SystemData.address}";
            body += $"<br/> Tel.nr: {Session.SystemData.phone}";
            body += "<br/><br/><b>Darbuotojo informacija:</b>";
            body += $"<br/> Vardas: {Session.User.DisplayName}";
            body += $"<br/> Stamp ID: {Session.User.Stamp}";
            body += "<br/><br/><b>Užsakymo informacija:</b>";
            body += $"<br/> Kliento adresas: {_view.Address.Text}";
            body += $"<br/> Prekės pavadinimas: {productName}";
            body += $"<br/> Prekių kiekis siuntimui: {_view.QtyToTransfer}";
            body += "<br/><br/><b>Perkėlimo dokumentas:</b>";
            body += $"<br/> {await GetDocumentName(tr.ExternalID)}";
            return body;
        }

        private async Task<string> GetDocumentName(decimal hid) 
        {
            var result = await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
            {
                method = "getdocname", hid = hid.ToString(CultureInfo.InvariantCulture)
            }.ToJsonString());
            return result.name;
        }

        private async Task CancelTransfer(TransferResult tr)
        {
            await Session.RemotePharmacyGateway.PostCallApi<dynamic>(new
            {
                method = "invokedoctransfer",
                hid = tr.ExternalID.ToString(CultureInfo.InvariantCulture)
            }.ToJsonString());
            await _salesOrderRepository.DeleteTransferData(tr.InternalID);
        }

        private string GetMessageText(Enumerator.SalesOrderResult result)
        {
            switch (result)
            {
                case Enumerator.SalesOrderResult.THERE_IS_NO_BARCODE:
                    return "Tokios prėkes barkodas nerastas!";
                case Enumerator.SalesOrderResult.INSUFFICIENT_QUANTITY:
                    return "Nepakankamas prekės likutis vaistineje iš kurios norima perkelti prekę!";
                case Enumerator.SalesOrderResult.WRONG_KAS_CLIENT:
                    return "Nerasta vaistinė iš kurios norima perkelti prekę!";
                case Enumerator.SalesOrderResult.CREATE_STOCKH_ERROR:
                case Enumerator.SalesOrderResult.CREATE_STOCKD_ERROR:
                    return "Klaida kuriant perkelimo dokumentą!";
                case Enumerator.SalesOrderResult.SAVE_CLIENT_ERROR:
                    return "Nepavyko išsaugoti kliento informacijos, pardavimas negalimas!";
                case Enumerator.SalesOrderResult.UNKNOWN:
                    return "Nenumatyta klaida! Kreiptis į IT skyrių";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
