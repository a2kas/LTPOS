using POS_display.Models.KAS;
using POS_display.Models.Pos;
using POS_display.Repository.Pos;
using POS_display.Utils;
using POS_display.Views.KAS;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace POS_display.Presenters.KAS
{
    public class ItemReturnReportPresenter : IItemReturnReportPresenter
    {
        #region Members
        private readonly IItemReturnReportView _view;
        private ReturningItemsData _returningItemsData;
        private readonly IPOSUtils _posUtils;
        private readonly IPosRepository _posRepository;
        private const string FiscalCreditValue = "N";
        private const string FiscalCashValue = "P";
        private const string FiscalInsuranceValue = "J";
        private const string FiscalPresentCardValue = "K";
        private const string FiscalWoltValue = "L";
        private const char VAT5TypeValue = 'C';
        private const char VAT21TypeValue = 'A';
        private CashPaymentRoundingResponse _cashPaymentRoundingResponse;
        #endregion

        #region Constructor
        public ItemReturnReportPresenter(IItemReturnReportView view,
            IPosRepository posRepository,
            IPOSUtils posUtils)
        {
            _view = view ?? throw new ArgumentNullException();
            _posUtils = posUtils ?? throw new ArgumentNullException();
            _posRepository = posRepository ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public async Task Init(ReturningItemsData returningItemsData)
        {
            _returningItemsData = returningItemsData;

            var isWoltPayment = await IsWoltPayment(returningItemsData.PosHeaderId);
            var returnSum = CalculateReturnSum();

            _returningItemsData.ReturnSum = isWoltPayment ? returnSum : await ApplyRounding(returnSum);
            _view.PharmacyNo.Text = returningItemsData.PharmacyNo;
            _view.Address.Text = returningItemsData.Address;
            _view.CashDeskNr.Text = returningItemsData.CashDeskNr;
            _view.ChequeNr.Text = returningItemsData.ChequeNr;
            _view.Cashier.Text = returningItemsData.Cashier;
            _view.ReturnSum.Text = _returningItemsData.ReturnSum.ToString();
            _view.Date.Text = returningItemsData.Date.ToString("yyyy/MM/dd");
            _view.ReturningItem.DataSource = returningItemsData.ReturningItems;
        }

        public async Task RefreshData()
        {
            var isWoltPayment = await IsWoltPayment(_returningItemsData.PosHeaderId);
            var returnSum = CalculateReturnSum();

            _view.ReturningItem.Refresh();
            _returningItemsData.ReturnSum = isWoltPayment ? returnSum : await ApplyRounding(returnSum);
            _view.ReturnSum.Text = _returningItemsData.ReturnSum.ToString();
        }

        public Task PerformPrint()
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


        public async Task<decimal> CalculateRoundingValue()
        {
            if (Session.Devices.fiscal != 1 || Session.getParam("EKA", "NEW") != "1")
            {
                return 0;
            }

            var totalReturnSum = 0.0m;
            var totalCompensationSum = 0.0m;
            var totalPresentCardSum = await GetPresentCardSum(_returningItemsData.PosHeaderId);
            var totalWoltSum = await IsWoltPayment(_returningItemsData.PosHeaderId)
                ? _returningItemsData.ReturnSum
                : 0;

            var allItems = _returningItemsData.ReturningItems;
            if (!allItems.Any())
            {
                return 0;
            }

            foreach (var returningItem in allItems)
            {
                if (returningItem.SumWithVAT > 0)
                {
                    totalReturnSum += returningItem.SumWithVAT;
                }
                if (returningItem.TotalCompensation > 0)
                {
                    totalCompensationSum += returningItem.TotalCompensation;
                }
            }

            totalReturnSum -= (totalPresentCardSum + totalWoltSum);
            await ApplyRounding(totalReturnSum);
            return _cashPaymentRoundingResponse.RoundingValue;
        }

        public async Task SendItemsReturnToCashRegister() 
        {
            if (Session.Devices.fiscal == 1 && Session.getParam("EKA", "NEW") == "1")
            {
                var totalReturnSum = 0.0m;
                var totalCompensationSum = 0.0m;
                var totalInsuranceSum = 0.0m;
                var totalPresentCardSum = await GetPresentCardSum(_returningItemsData.PosHeaderId);
                var totalWoltSum = await IsWoltPayment(_returningItemsData.PosHeaderId) ? _returningItemsData.ReturnSum : 0;

                var allItems = _returningItemsData.ReturningItems;
                if (allItems.Any())
                {
                    await _posUtils.FiscalReturnOpen(_returningItemsData.DocumentNo);
                    foreach (var returningItem in allItems)
                    {
                        if (returningItem.SumWithVAT > 0)
                        {
                            await _posUtils.FiscalReturnItem(
                                returningItem.Name,
                                returningItem.VAT5Value == 0 ? VAT21TypeValue : VAT5TypeValue,
                                returningItem.SumWithVAT);
                            totalReturnSum += returningItem.SumWithVAT;
                        }

                        if (returningItem.TotalCompensation > 0)
                        {
                            await _posUtils.FiscalReturnItem(
                                returningItem.Name,
                                returningItem.VAT5Value == 0 ? VAT21TypeValue : VAT5TypeValue,
                                returningItem.TotalCompensation);
                            totalCompensationSum += returningItem.TotalCompensation;
                        }

                        if (returningItem.InsuranceSum > 0)
                        {
                            await _posUtils.FiscalReturnItem(
                                returningItem.Name,
                                returningItem.VAT5Value == 0 ? VAT21TypeValue : VAT5TypeValue,
                                returningItem.InsuranceSum);
                            totalInsuranceSum += returningItem.InsuranceSum;
                        }
                    }

                    totalReturnSum -= (totalPresentCardSum + totalWoltSum);
                    totalReturnSum = await ApplyRounding(totalReturnSum);
                    await PerformReturnPayment(totalReturnSum, totalCompensationSum, totalInsuranceSum, totalPresentCardSum, totalWoltSum);
                    await _posUtils.FiscalSaleClose();
                }
            }
        }
        #endregion

        #region Private methods
        private async Task PerformReturnPayment(decimal totalReturnSum, decimal compensationSum, decimal totalInsuranceSum, decimal totalPresentCardSum, decimal totalWoltSum)
        {
            if (compensationSum > 0)
                await _posUtils.FiscalReturnPay(FiscalCreditValue, Math.Round(compensationSum * (-1), 2));

            if (totalReturnSum > 0)
                await _posUtils.FiscalReturnPay(FiscalCashValue, Math.Round(totalReturnSum * (-1), 2));

            if (totalInsuranceSum > 0)
                await _posUtils.FiscalReturnPay(FiscalInsuranceValue, Math.Round(totalInsuranceSum * (-1), 2));

            if (totalPresentCardSum > 0)
                await _posUtils.FiscalReturnPay(FiscalPresentCardValue, Math.Round(totalPresentCardSum * (-1), 2));

            if (totalWoltSum > 0)
                await _posUtils.FiscalReturnPay(FiscalWoltValue, Math.Round(totalWoltSum * (-1), 2));
        }

        private async Task<decimal> GetPresentCardSum(decimal posHeaderId) 
        {
            var presentCardSum = 0.0m;
            var posPayments = await _posRepository.GetPosPayment(posHeaderId, true);
            foreach (var posPayment in posPayments)
            {
                if (posPayment.PaymentType == PosPaymentType.PRESENTCARD) 
                {
                    presentCardSum = posPayment.Amount;
                    return presentCardSum;
                }
            }
            return presentCardSum;
        }

        private async Task<bool> IsWoltPayment(decimal posHeaderId)
        {
            var posPayments = await _posRepository.GetPosPayment(posHeaderId, true);
            return posPayments?.Any(e => e.PaymentType == PosPaymentType.WOLT) ?? false;  
        }

        private void HandleButtonsEnabling(bool apply)
        {
            _view.PrintButton.Enabled = apply;
        }

        private decimal CalculateReturnSum() 
        {
            decimal totalSum = 0m;
            if (_returningItemsData == null)
                return totalSum;

            foreach (var returningItem in _returningItemsData.ReturningItems)
                totalSum += returningItem.SumWithVAT;

            return Math.Round(totalSum,2);
        }

        private async Task<decimal> ApplyRounding(decimal sum)
        {
            if (!Session.IsRoundingEnabled)
                return sum;

            _cashPaymentRoundingResponse = await Session.FP550.CashPaymentRoundingSimulation(sum, sum, 0, false);
            sum += _cashPaymentRoundingResponse.RoundingValue;
            return sum;            
        }

        private FlowDocument FormatFlowDocument()
        {
            FlowDocument doc = new FlowDocument();

            Paragraph p = new Paragraph(new Run(_view.PharmacyNo.Text));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Adresas: {_view.Address.Text}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Kasos ap. Nr: {_view.CashDeskNr.Text}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Kvito Nr: {_view.ChequeNr.Text}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Kasininkas(ė): {_view.Cashier.Text}"));
            p.FontSize = 15;
            doc.Blocks.Add(p);

            if (!string.IsNullOrEmpty(_view.InsuranceCompany.Text))
            {
                p = new Paragraph(new Run($"Draudimo komp.: {_view.InsuranceCompany.Text}"));
                p.FontSize = 15;
                doc.Blocks.Add(p);
            }

            p = new Paragraph(new Run("Klaidų taisymo - pinigų grąžinimo aktas"));
            p.FontSize = 20;
            p.TextAlignment = TextAlignment.Center;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run($"Data: {_view.Date.Text}"));
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
            row.Cells.Add(CreateTableCell("PVM 5%"));
            row.Cells.Add(CreateTableCell("PVM 21%"));
            row.Cells.Add(CreateTableCell("Sumokėta grynais arba kortele, EUR"));
            row.Cells.Add(CreateTableCell("Kompensuojama kiti., EUR"));
            row.Cells.Add(CreateTableCell("Kompensuojama suma TLK., EUR"));
            row.Cells.Add(CreateTableCell("Viso, EUR"));

            decimal totalPriceWithVAT = 0;
            decimal totalDiscount = 0;
            decimal totalVAT5 = 0;
            decimal totalVAT21 = 0;
            decimal totalSumWithVAT = 0;
            decimal totalInsuranceSum = 0;
            decimal totalCompensation = 0;
            decimal totalSum = 0;

            foreach (var returningItem in _returningItemsData.ReturningItems)
            {
                row = new TableRow();
                table.RowGroups[0].Rows.Add(row);
                row.Cells.Add(CreateTableCell(returningItem.Name));
                row.Cells.Add(CreateTableCell(returningItem.Qty.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.PriceWithVAT.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.DiscountSum.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.VAT5Value.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.VAT21Value.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.SumWithVAT.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.InsuranceSum.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.TotalCompensation.ToString()));
                row.Cells.Add(CreateTableCell(returningItem.TotalSum.ToString()));

                totalPriceWithVAT += returningItem.PriceWithVAT;
                totalDiscount += returningItem.DiscountSum;
                totalVAT5 += returningItem.VAT5Value;
                totalVAT21 += returningItem.VAT21Value;
                totalSumWithVAT += returningItem.SumWithVAT;
                totalInsuranceSum += returningItem.InsuranceSum;
                totalCompensation += returningItem.TotalCompensation;
                totalSum += returningItem.TotalSum;
            }

            row = new TableRow();
            table.RowGroups[0].Rows.Add(row);

            row.Cells.Add(CreateBoldTableCell("Suma, iš viso"));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateBoldTableCell(totalPriceWithVAT.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalDiscount.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalVAT5.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalVAT21.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalSumWithVAT.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalInsuranceSum.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalCompensation.ToString("0.00")));
            row.Cells.Add(CreateBoldTableCell(totalSum.ToString("0.00")));


            row = new TableRow();
            table.RowGroups[0].Rows.Add(row);

            row.Cells.Add(CreateBoldTableCell("Apvalinimas"));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateBoldTableCell(_cashPaymentRoundingResponse?.RoundingValue.ToString("0.00")));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));

            row = new TableRow();
            table.RowGroups[0].Rows.Add(row);

            row.Cells.Add(CreateBoldTableCell("Iš viso mokėtina suma"));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateBoldTableCell((totalSumWithVAT + (_cashPaymentRoundingResponse?.RoundingValue ?? 0)).ToString("0.00")));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));
            row.Cells.Add(CreateTableCell(""));

            doc.Blocks.Add(table);

            p = new Paragraph(new Run($"Pardavimas anuliuojamas. Priežastis: {_returningItemsData.MistakeType}"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Kasininko parašas: ____________________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            string returnSumField = string.IsNullOrEmpty(_view.ReturnSum.Text) ?
                "__________________________________" :
                Math.Round(totalSumWithVAT + (_cashPaymentRoundingResponse?.RoundingValue ?? 0),2).ToString("0.00");

            p = new Paragraph(new Run($"Pinigus gavau: Suma (skaičiais): {returnSumField}"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            string buyerField = string.IsNullOrEmpty(_view.Buyer.Text) ?
                "_______________________________________" :
                _view.Buyer.Text;

            p = new Paragraph(new Run($"Pirkėjo vardas ir pavardė: {buyerField}"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run());
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            p = new Paragraph(new Run("Pirkėjo parašas: ______________________________________________"));
            p.TextAlignment = TextAlignment.Left;
            p.FontSize = 12;
            doc.Blocks.Add(p);

            return doc;
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

        private TableCell CreateBoldTableCell(string text)
        {
            Run run = new Run(text);
            run.FontWeight = FontWeights.Bold;

            Paragraph p = new Paragraph(run) { FontSize = 10 };
            return new TableCell(p)
            {
                BorderBrush = System.Windows.Media.Brushes.Black,
                Background = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(1)
            };
        }
        #endregion
    }
}
