using POS_display.Models.KAS;
using POS_display.wpf.View.KAS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace POS_display.wpf.ViewModel
{
    public class ItemReturnReportViewModel : BaseViewModel
    {
        #region Variables
        private const string _description = "Grąžinimo aktas";
        private readonly IItemReturnReportView _view;
        private bool _isPrintEnabled;
        private PosHeader _posHeader;
        private List<Items.KAS.posd> _posDetails;
        private string _reason;
        #endregion

        #region Properties
        public FlowDocument ReportDocument { get; set; }

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
        #endregion

        #region Constructor
        public ItemReturnReportViewModel(IItemReturnReportView view, PosHeader posHeader, List<Items.KAS.posd> posDetails, string reason)
        {
            
            _view = view ?? throw new ArgumentNullException();
            _posHeader = posHeader ?? throw new ArgumentNullException();
            _posDetails = posDetails ?? throw new ArgumentNullException();

            _reason = reason;

            _view.DocumentScrollViewer.Document = FormatFlowDocument();
            _view.DocumentScrollViewer.Document.Name = _description.Replace(" ", "");

            _isPrintEnabled = true;

            PerformPrintCommand = new BaseAsyncCommand(PerformPrint);
        }
        #endregion

        #region Command definitions

        private Task PerformPrint()
        {
            try
            {
                HandleButtonsEnabling(false);

                System.Windows.Controls.PrintDialog printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    var document = FormatFlowDocument();
                    document.ColumnWidth = 700;
                    IDocumentPaginatorSource source = document;
                    printDialog.PrintDocument(source.DocumentPaginator, "Grąžinimo aktas");
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, $"Nepavyko atspausdinti!\n" +
                    $"Spausdinimo klaidos pranešimas: {ex.Message}");
            }
            finally
            {
                HandleButtonsEnabling(true);
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Private methods
        private void HandleButtonsEnabling(bool apply)
        {
            Mouse.OverrideCursor = apply ? System.Windows.Input.Cursors.Arrow :
                System.Windows.Input.Cursors.Wait;
            IsPrintEnabled = apply;
        }

        private string GetPharmacyNo()
        {
            if (Session.SystemData.prodcustid == 1 || Session.SystemData.prodcustid == 2)
               return $"800{Session.SystemData.prodcustid}";
            else
               return $"7{Session.SystemData.prodcustid}";
        }

        private TableCell CreateTableCell(string text) 
        {
            Paragraph p = new Paragraph(new Run(text)) { FontSize = 10 };
            return new TableCell(p)
            {
                BorderBrush = System.Windows.Media.Brushes.Black,
                Background = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(1)
            };
        }

        private FlowDocument FormatFlowDocument()
        {
            FlowDocument doc = new FlowDocument();

            Paragraph p = new Paragraph(new Run($"BENU {GetPharmacyNo()}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Adresas: {Session.SystemData.address}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Kasos ap. Nr: {Session.Devices.deviceno}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Kvito Nr: {_posHeader.CheckNo}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Kasininkas(ė): _____________________________________"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("PINIGŲ GRĄŽINIMO AKTAS"));
            p.FontSize = 20;
            p.TextAlignment = TextAlignment.Center;           
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Data: {DateTime.Now:yyyy-MM-dd}"));
            p.FontSize = 15;
            p.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Pirkėjui nutraukus pirkimo-pardavimo sutartį arba grąžinus įsigytas prekes:"));
            p.FontSize = 15;
            p.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(p);

            Table table = new Table();
            table.CellSpacing = 0;
            table.BorderBrush = System.Windows.Media.Brushes.Black;
            table.BorderThickness = new Thickness(1);
            table.Background = System.Windows.Media.Brushes.White;

            TableRow row = new TableRow();
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(row);

            row = new TableRow();
            table.RowGroups[0].Rows.Add(row);
            row.Cells.Add(CreateTableCell("Pavadinimas"));
            row.Cells.Add(CreateTableCell("Kiekis"));
            row.Cells.Add(CreateTableCell("Grynais arba kortele prieš nuolaidą"));
            row.Cells.Add(CreateTableCell("Nuolaida"));
            row.Cells.Add(CreateTableCell("Sumokėta grynais arba kortele, EUR"));
            row.Cells.Add(CreateTableCell("Kompensuojama suma TLK., EUR"));
            row.Cells.Add(CreateTableCell("Viso, EUR"));
            row.Cells.Add(CreateTableCell("21 % PVM, EUR"));
            row.Cells.Add(CreateTableCell("5 % PVM, EUR"));

            foreach (var posDetail in _posDetails) 
            {
                row = new TableRow();
                table.RowGroups[0].Rows.Add(row);
                row.Cells.Add(CreateTableCell(posDetail.barcodename));
                row.Cells.Add(CreateTableCell(posDetail.qty.ToString()));
                row.Cells.Add(CreateTableCell(posDetail.price.ToString()));
                row.Cells.Add(CreateTableCell((posDetail.price - posDetail.pricediscounted).ToString()));
                row.Cells.Add(CreateTableCell(posDetail.pricediscounted.ToString()));
                row.Cells.Add(CreateTableCell(posDetail.prepayment_compensation.ToString()));
                row.Cells.Add(CreateTableCell(posDetail.sum.ToString()));
                row.Cells.Add(CreateTableCell(posDetail.vatsize == 21 ? posDetail.vat.ToString() : 0.ToString()));
                row.Cells.Add(CreateTableCell(posDetail.vatsize == 5 ? posDetail.vat.ToString() : 0.ToString()));
            }

            doc.Blocks.Add(table);

            p = new Paragraph(new Run($"Pardavimas anuliuojamas. Priežastis: {_reason}"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Kasininko parašas: ____________________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Atsakingo asmens parašas: ______________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Pinigus gavau: Suma (skaičiais): __________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Pirkėjo vardas ir pavardė: _______________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Pirkėjo parašas: ______________________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            return doc;
        }
        #endregion

    }

}
