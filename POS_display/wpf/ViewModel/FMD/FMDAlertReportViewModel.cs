using Microsoft.Extensions.DependencyInjection;
using POS_display.Utils.Email;
using POS_display.wpf.Model;
using POS_display.wpf.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class FMDAlertReportViewModel : BaseViewModel
    {
        #region Variables
        private fmd _fmdModel;
        private string _sendReportEmail;
        private const string _fmdAlertEmailSubject = "FMD Pranešimas";
        private const string _description = "FMD Pranešimas";
        private List<MistakeType> _mistakeTypeEntries = null;
        private bool _isCloseEnabled = false;
        private bool _isSendEnabled = true;
        private bool _isPrintEnabled = true;
        private bool _sendingHasBeenPerformed = false;
        private readonly IFMDAlertReportView _view;
        #endregion

        #region Properties
        public List<MistakeType> MistakeTypeEntries 
        {
            get { return _mistakeTypeEntries; }
            set { _mistakeTypeEntries = value; }
        }

        public FlowDocument ReportDocument { get; set; }

        public fmd FMDModel
        {
            get { return _fmdModel; }
            set { _fmdModel = value; }
        }
        public int SelectedMistakeTypeID { get; set; }

        public string SelectedMistakeType 
        {
            get { return MistakeTypeEntries[SelectedMistakeTypeID].Value; }
        }

        public string Comment { get; set; }

        public bool IsCloseEnabled 
        {
            get 
            { 
                return _isCloseEnabled; 
            }
            set 
            { 
                _isCloseEnabled = value; 
                NotifyPropertyChanged(nameof(IsCloseEnabled));
            } 
        }

        public bool IsSendEnabled 
        {
            get { return _isSendEnabled; }
            set 
            { 
                _isSendEnabled = value;
                NotifyPropertyChanged(nameof(IsSendEnabled));
            }
        }

        public bool IsPrintEnabled
        {
            get { return _isPrintEnabled; }
            set 
            {
                _isPrintEnabled = value;
                NotifyPropertyChanged(nameof(IsPrintEnabled));
            }
        }
        #endregion

        #region Commands
        public BaseAsyncCommand PerformPrintCommand { get; set; }
        public BaseAsyncCommand SendEmailCommand { get; set; }
        #endregion

        #region Constructor
        public FMDAlertReportViewModel(IFMDAlertReportView view, fmd FMDModel)
        {
            _view = view ?? throw new ArgumentNullException();
            _fmdModel = FMDModel ?? throw new ArgumentNullException();

            _sendReportEmail = Session.Develop ? Session.getParam("FMD", "TEST_REPORTEMAIL") : Session.getParam("FMD", "REPORTEMAIL");
            _mistakeTypeEntries = new List<MistakeType>
            {
                new MistakeType { Id = 0, Value = "Techninė klaida (pvz. skenerio klaida)" },
                new MistakeType { Id = 1, Value = "Procedūrinė klaida (pvz. nuskenuota du kartus)" },
                new MistakeType { Id = 2, Value = "Pakuotė buvo grąžinta" },
                new MistakeType { Id = 3, Value = "Kita" }
            };

            _view.DocumentScrollViewer.Document = FormatFlowDocument(_fmdModel);
            _view.DocumentScrollViewer.Document.Name = _description.Replace(" ", "");

            PerformPrintCommand = new BaseAsyncCommand(PerformPrint);
            SendEmailCommand = new BaseAsyncCommand(SendEmail);
        }
        #endregion

        #region Command definitions

        private Task PerformPrint()
        {
            try
            {
                HandleButtonsEnabling(false);
                using (PrintDialog printDialog = new PrintDialog())
                {
                    using (var printDocument = new PrintDocument()) 
                    {
                        printDocument.DocumentName = _description;
                        printDocument.PrintPage += PrintDoc_PrintPage;
                        printDialog.Document = printDocument;
                        if (DialogResult.OK == printDialog.ShowDialog())
                            printDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, $"Nepavyko atspausdinti!\n" +
                    $"Nusirašykite pranešimo informaciją ir išsisaugokite.\n" +
                    $"Spausdinimo klaidos pranšimas: {ex.Message}");
            }
            finally 
            {
                HandleButtonsEnabling(true);
            }

            return Task.CompletedTask;
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            float x = 50.0F;
            float y = 50.0F;

            e.Graphics.DrawString($"Product ID: {_fmdModel.posDetail?.productid ?? 0}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Product Code: {_fmdModel.productCode}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Serial: {_fmdModel.serialNumber}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Batch ID: {_fmdModel.batchId}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Expriy date: {_fmdModel.expiryDate}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Warning: {_fmdModel.warning}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Alert ID: {_fmdModel.alertId}", drawFont, drawBrush, x, y);
            y += 40;

            e.Graphics.DrawString($"Data: {DateTime.Now}", drawFont, drawBrush, x, y);
           
        }


        private async Task SendEmail()
        {
            try 
            {
                HandleButtonsEnabling(false);
                var emailUtils = Program.ServiceProvider.GetRequiredService<IEmailUtils>();
                await emailUtils?.SendEmail(_fmdAlertEmailSubject, FormatEmailBody(), _sendReportEmail, Session.SystemData.InternalEmail);
                if(helpers.alert(Enumerator.alert.info, "Pranešimas buvo sėkmingai nusiųstas.\n" +
                    "Ar norite atspausdinti pranešimo ataskaitą?", true))
                    await PerformPrint();
            } 
            catch (Exception ex) 
            {
                helpers.alert(Enumerator.alert.error, "Nepavyko išsiųsti!\n" +
                    $"Išiųskite informaciją rankiniu būdu į {_sendReportEmail}.\nSiuntimo klaidos pranešimas: {ex.Message}");
            } 
            finally 
            {
                _sendingHasBeenPerformed = true;
                HandleButtonsEnabling(true);
            }
        }
        #endregion

        #region Private methods
        private void HandleButtonsEnabling(bool apply)
        {
            Mouse.OverrideCursor = apply ? System.Windows.Input.Cursors.Arrow :
                System.Windows.Input.Cursors.Wait;
            IsCloseEnabled = _sendingHasBeenPerformed ? apply : false;
            IsSendEnabled = _sendingHasBeenPerformed ? false : apply;
            IsPrintEnabled = apply;
        }

        private string FormatEmailBody()
        {
            string body = "<b>Vaistinės informacija:</b>";
            body += $"<br/> KAS ID: {Session.SystemData.kas_client_id}";
            body += $"<br/> Pavadinimas: {Session.SystemData.name}";
            body += $"<br/> Adresas: {Session.SystemData.address}";
            body += $"<br/> Tel.nr: {Session.SystemData.phone}";
            body += "<br/><br/><b>Darbuotojo informacija:</b>";
            body += $"<br/> Vardas: {Session.User.DisplayName}";
            body += $"<br/> Stamp ID: {Session.User.Stamp}";
            body += "<br/><br/><b>FMD pranešimo informacija:</b>";
            body += $"<br/> Product ID: {_fmdModel.posDetail?.productid ?? 0}";
            body += $"<br/> Product Code: {_fmdModel.productCode}";
            body += $"<br/> Serial: {_fmdModel.serialNumber}";
            body += $"<br/> Batch ID: {_fmdModel.batchId}";
            body += $"<br/> Expriy date: {_fmdModel.expiryDate}";
            body += $"<br/> Warning: {_fmdModel.warning}";
            body += $"<br/> Alert ID: {_fmdModel.alertId}";
            body += $"<br/><b>Klaidos tipas:</b> {SelectedMistakeType}<br/>";
            body += !string.IsNullOrEmpty(Comment) ? $"<b>Komentaras:</b> {Comment}<br/>" : string.Empty;
            return body;
        }

        private FlowDocument FormatFlowDocument(fmd model)
        {
            FlowDocument doc = new FlowDocument();

            Paragraph p = new Paragraph(new Run($"Product ID: {model.posDetail?.productid ?? 0}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Product Code: {model.productCode}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Serial: {model.serialNumber}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Batch ID: {model.batchId}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Expriy date: {model.expiryDate}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Warning: {model.warning}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Alert ID: {model.alertId}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Data: {DateTime.Now}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);
            
            return doc;
        }
        #endregion

        public class MistakeType
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
    }


}
