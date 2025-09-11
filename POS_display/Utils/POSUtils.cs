using POS_display.Models.Pos;
using POS_display.Utils.Logging;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace POS_display.Utils
{
    public interface IPOSUtils
    {
        Task<bool> Init();
        Task<bool> PrintReportZ();
        Task<bool> Close();
        Task<string> GetVersion();
        Task<bool> OpenDrawer();
        Task<bool> OpenNonFiscal();
        Task<bool> PrintNonFiscal(string line);
        Task<bool> CloseNonFiscal();
        Task<bool> FiscalSaleOpen();
        Task<bool> FiscalSaleText(string line);
        Task<bool> FiscalSaleItem(string productName, string name2, int tax, decimal price, decimal qty, decimal discount, char discType);
        Task<bool> FiscalDiscount(decimal amount);
        Task<bool> FiscalSalePay(string line1, string line2, string type, decimal amount);
        Task<bool> FiscalAdvancePay(string line, string type, decimal amount);
        Task<string> GetNextCheckNo();
        Task<bool> FiscalSaleClose();
        Task<bool> CutLine();
        Task<bool> PrintReportMiniX();
        Task<bool> FiscalSaleReset();
        Task<string> ReadCard();
        Task<bool> PrintReportX();
        Task<bool> MoneyIn(decimal amount);
        Task<bool> MoneyOut(decimal amount);
        Task<bool> PrintReportSum(string from, string to);
        Task<bool> PrintReportDetail(string from, string to);
        Task<bool> SetDateTime(string date, string time);
        Task<bool> PrintReceiptCopy();
        Task<bool> PrintBarcode(string barcode, decimal type);
        Task<bool> FiscalReturnOpen(string documentNo);
        Task<bool> FiscalReturnItem(string productName, char taxTypeValue, decimal priceSum);
        Task<bool> FiscalReturnPay(string type, decimal amount);
        Task<decimal> GetCashAmount();
        Task<string> GetDeviceInfo(bool includeSymbolCount = false);
        Task<int> GetMaxSymbolsPerLine();
        Task<CashPaymentRoundingResponse> CashPaymentRoundingSimulation(decimal reciptSum, decimal cashSum, decimal creditSum, bool creditLast = true);
    }

    public enum Status
    {
        UNKNOWN                          = 0,
        SYNTAX_ERROR                     = 10,
        WRONG_COMMAND                    = 11,
        TIME_AND_DATE_NOT_SET            = 12,
        THERE_IS_NO_DISPLAY              = 13,
        PRINTER_MECHANISM                = 14,
        GENERIC_ERROR                    = 15,
        RESERVED_1                       = 16,
        RESERVED_2                       = 17,
        MEMORY_OVERFLOW                  = 20,
        ANOTHER_MODE                     = 21,
        MEMORY_RESET                     = 22,
        RESERVED_3                       = 23,
        RAM_ERROR                        = 24,
        COVER_REMOVED                    = 25,
        RESERVED_4                       = 26,
        RESERVED_5                       = 27,
        THERE_IS_NO_CHEQUE_PAPER         = 30,
        CHEQUE_PAPER_IS_ENDING           = 31,
        THERE_IS_NOT_SPACE_IN_EKJ_MEMORY = 32,
        FISCAL_CHEQUE                    = 33,
        EKJ_MEMORY_IS_ENDING             = 34,
        NON_FISCAL_CHEQUE                = 35,
        EKJ_LOW_SPACE                    = 36,
        RESERVED_6                       = 37,
        AUTO_SCISSORS                    = 40,
        VIEW_IN_DISPALY                  = 41,
        RESERVED_7                       = 42,
        RESERVED_8                       = 43,
        RESERVED_9                       = 44,
        INTERFACE_SPEED_1                = 45,
        INTERFACE_SPEED_2                = 46,
        INTERFACE_SPEED_3                = 47,
        RESERVED_10                      = 50,
        WRITE_TO_FA_ERROR                = 51,
        SERIAL_NUMBER_IS_SET             = 52,
        LEFT_LESS_THAN_50_REPORT         = 53,
        FILLED_FA                        = 54,
        GENERIC_FA_ERROR                 = 55,
        RESERVED_11                      = 56,
        RESERVED_12                      = 57,
        FA_READ_ONLY                     = 60,
        PVM_CODE_INPUTTED                = 61,
        RECORD_IS_CORRECT                = 62,
        FISCALIZED                       = 63,
        PVM_TARIFFS_MORE_THAN_50         = 64,
        FRAME_NUMBER_IS_SET              = 65,
        THERE_IS_NO_FA                   = 66,
        RESERVED_13                      = 67
    }


    public class POSUtils : IPOSUtils
    {
        private Type COMType;
        private object POS;
        private bool OPENED;
        private int MaxSymbolsPerLine = 0;

        public POSUtils()
        { }
        //Async calls
        public async Task<bool> Init()
        {
            COMType = Type.GetTypeFromProgID("fp550.dll");
            if (COMType == null)
            {
                Session.Devices.fiscal = -1;
                helpers.alert(Enumerator.alert.warning, "Nepavyksta rasti fp550.dll!");
            }
            else
                POS = Activator.CreateInstance(COMType);

            string command = "";
            object[] mParam = null;
            switch (Session.Devices.postype)
            {
                case "DATECSFP1000":
                    command = "Open";
                    mParam = new object[] { 1, "9600,n,8,1" };
                    break;
                default:
                    Session.Devices.fiscal = -1;
                    helpers.alert(Enumerator.alert.warning, "Šis kasos aparato modelis " + Session.Devices.postype + " yra nesuderinamas su programa! Galimas tik nefiskalinis pardavimas.");
                    break;
            }

            OPENED = true;
            var result = await ExecuteRequestAsync(command, mParam);
            var success = result.Length >= 6 && result.Substring(0, 6) != "000000";
            if (!success)
                helpers.alert(Enumerator.alert.warning, "Nepavyksta prisijungti prie kasos aparato. Patikrinkite kasos aparato nustatymus!");
            Session.FP550.OPENED = success;
            Session.Devices.fiscal = success ? 1 : -1;

            return success;
        }

        public async Task<bool> PrintReportZ()
        {
            object[] mParam = new object[] { 69, "1" };
            var result = await ExecuteRequestAsync("cmdline", mParam);

            return EnsureSuccessResult(result);
        }

        protected async Task<string> ExecuteRequestAsync(string command, object[] mParam)
        {
            string result = "";
            if (!OPENED)
                return result;
            try
            {
                if (Session.Devices.fiscal != 1)
                    throw new Exception("");
                await Task.Run(() =>
                {
                    result = (string)COMType.InvokeMember(command, BindingFlags.InvokeMethod, null, POS, mParam);
                });
            }
            catch (Exception ex)
            {
                if (ex.Message != "")
                    throw new Exception("Kasos aparato klaida! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> Close()
        {
            object[] mParam = new object[] { };
            var result = await ExecuteRequestAsync("Close", mParam);
            var success = !result.Equals("0");
            if (success)
                OPENED = false;
            else
                throw new Exception("Nepavyksta uždaryti kasos aparato! \nFunkcija: Close(). \nKlaida: " + result);

            return success;
        }

        public async Task<string> GetVersion()
        {
            object[] mParam = new object[] { };
            var result = await ExecuteRequestAsync("Versija", mParam);

            return result;
        }

        public async Task<bool> OpenDrawer()
        {
            object[] mParam = new object[] { 106, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atidaryti stalčiaus! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> OpenNonFiscal()
        {
            object[] mParam = new object[] { 38, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atidaryti nefiskalinio kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<string> GetDeviceInfo(bool includeSymbolCount = false)
        {
            string parameter = includeSymbolCount ? "A" : "";
            object[] mParam = new object[] { 90, parameter };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);

            if (!success)
                throw new Exception("Nepavyksta gauti įrenginio informacijos! \nFunkcija: cmdline(90, \"" + parameter + "\"). \nKlaida: " + result);

            return result;
        }

        public async Task<int> GetMaxSymbolsPerLine()
        {
            try
            {
                var result = await GetDeviceInfo(true);
                if (result.StartsWith("OK"))
                {
                    var parts = result.Split(',');
                    if (parts.Length >= 7)
                    {
                        var symbolsPart = parts[parts.Length - 1];
                        if (int.TryParse(symbolsPart, out int symbolCount))
                        {
                            return symbolCount;
                        }
                    }
                }
                return 42;
            }
            catch
            {
                return 42;
            }
        }

        public async Task<bool> PrintNonFiscal(string line)
        {
            bool success = false;
            string result = "";
            int start = 0;
            int end = 42;
            decimal line_length = line.Length;
            decimal lines = Math.Ceiling(line_length / end);
            for (int i = 1; i <= lines; i++)
            {
                if (line_length < end)
                    end = (int)line_length;
                object[] mParam = new object[] { 42, line.Substring(start, end) };
                result = await ExecuteRequestAsync("cmdline", mParam);
                success = EnsureSuccessResult(result);
                start = end * i;
                line_length -= end;
            }
            if (!success)
                throw new Exception("Nepavyksta atspausdinti nefiskalinės eilutės! \nFunkcija: cmdline(42, \"" + line + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintNonFiscalCentered(string line)
        {
            if (MaxSymbolsPerLine == 0)
                MaxSymbolsPerLine = await GetMaxSymbolsPerLine();

            bool success = false;
            string result = "";
            decimal line_length = line.Length;
            decimal lines = Math.Ceiling(line_length / MaxSymbolsPerLine);

            for (int i = 0; i < lines; i++)
            {
                int start = i * MaxSymbolsPerLine;
                int length = Math.Min(MaxSymbolsPerLine, (int)line_length - start);
                string segment = line.Substring(start, length);
                string centeredText = CenterText(segment, MaxSymbolsPerLine);
                object[] mParam = new object[] { 42, centeredText };
                result = await ExecuteRequestAsync("cmdline", mParam);
                success = EnsureSuccessResult(result);

                if (!success)
                    break;
            }

            if (!success)
                throw new Exception("Nepavyksta atspausdinti nefiskalinės eilutės! \nFunkcija: cmdline(42, \"" + line + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> CloseNonFiscal()
        {
            object[] mParam = new object[] { 39, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta uždaryti nefiskalinio kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalSaleOpen()
        {
            string Kasininkas = "1";
            string KasininkoNr = "0000";
            string KasosNr = "0001";
            object[] mParam = new object[] { 48, Kasininkas + "," + KasininkoNr + "," + KasosNr + ",I " };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atidaryti fiskalinio kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalSaleText(string line)
        {
            bool success = false;
            string result = "";
            int start = 0;
            int end = 42;
            decimal line_length = line.Length;
            decimal lines = Math.Ceiling(line_length / end);
            for (int i = 1; i <= lines; i++)
            {
                if (line_length < end)
                    end = (int)line_length;
                object[] mParam = new object[] { 54, line.Substring(start, end) };
                result = await ExecuteRequestAsync("cmdline", mParam);
                success = EnsureSuccessResult(result);
                start = end * i;
                line_length -= end;
            }
            if (!success)
                throw new Exception("Nepavyksta atspausdinti teksto! \nFunkcija: cmdline(54, \"" + line + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalSaleItem(string productName, string name2, int tax, decimal price, decimal qty, decimal discount, char discType)//disc_type ; - sumine , - procentine
        {
            int length = productName.Length;
            if (length > 42)
                length = 42;
            string discountText = "";
            if (discount != 0)
                discountText = discType + discount.ToString().Replace(',', '.');
            var totalAmount = Math.Round(qty * price, 2, MidpointRounding.AwayFromZero);
            var productNameFull = productName.Substring(0, length);
            if (!string.IsNullOrWhiteSpace(name2))
                productNameFull += Convert.ToChar(10) + name2;
            object[] mParam = new object[] 
            { 
                49, 
                 productNameFull + Convert.ToChar(9) + Convert.ToChar(65 + tax) + totalAmount.ToString().Replace(',', '.') + discountText 
            };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta įvykdyti fiskalinio pardavimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalDiscount(decimal amount)
        {
            object[] mParam = new object[] { 51, "00;-" + amount.ToString().Replace(',', '.') };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);

            if (!success)
                throw new Exception("Nepavyksta pritaikyti nuolaidos kvitui! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalSalePay(string line1, string line2, string type, decimal amount)
        {
            object[] mParam = new object[] { 53, line1 + Convert.ToChar(10) + line2 + Convert.ToChar(9) + type + amount.ToString().Replace(',', '.') };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);

            if (!success)
                throw new Exception("Nepavyksta įvykdyti fiskalinio pardavimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalAdvancePay(string line, string type, decimal amount)
        {
            object[] mParam = new object[] { 70, $"{type}+{amount.ToString().Replace(',', '.')},{line}" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atlikti avanso operacijos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<string> GetNextCheckNo()
        {
            object[] mParam = new object[] { 113, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta gauti paskutinio kvito numerio! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return (result.Substring(3, 8).ToDecimal() + 1).ToString();
        }

        public async Task<bool> FiscalSaleClose()
        {
            object[] mParam = new object[] { 56, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta uždaryti fiskalinio kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> CutLine()
        {
            object[] mParam = new object[] { 45, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta nukirpti juostos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintReportMiniX()
        {
            object[] mParam = new object[] { 69, "2" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti Mini X ataskaitos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalSaleReset()
        {
            object[] mParam = new object[] { 57, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta anuliuoti fiskalinio kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<string> ReadCard()
        {
            object[] mParam = new object[] { "9" };
            var result = await ExecuteRequestAsync("BankasCardRead", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyko nuskaityti kortelės.");

            return result.Substring(3);
        }

        public async Task<bool> PrintReportX()
        {
            object[] mParam = new object[] { 69, "3" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti X ataskaitos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> MoneyIn(decimal amount)
        {
            object[] mParam = new object[] { 70, "+" + amount.ToString().Replace(',', '.') };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atlikti pinigų įdėjimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> MoneyOut(decimal amount)
        {
            object[] mParam = new object[] { 70, "-" + amount.ToString().Replace(',', '.') };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atlikti pinigų išėmimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintReportSum(string from, string to)
        {
            //if (from.Length == 10 && to.Length == 10)
            object[] mParam = new object[] { 79, from.Substring(8, 2) + from.Substring(5, 2) + from.Substring(2, 2) + "," + to.Substring(8, 2) + to.Substring(5, 2) + to.Substring(2, 2) };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti periodinės suminės ataskaitos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintReportDetail(string from, string to)
        {
            //if (from.Length == 10 && to.Length == 10)
            object[] mParam = new object[] { 94, from.Substring(8, 2) + from.Substring(5, 2) + from.Substring(2, 2) + "," + to.Substring(8, 2) + to.Substring(5, 2) + to.Substring(2, 2) };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti periodinės ataskaitos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> SetDateTime(string date, string time)
        {
            //if (date.Length == 10 && time.Length == 8)
            object[] mParam = new object[] { 61, date.Substring(8, 2) + "-" + date.Substring(5, 2) + "-" + date.Substring(2, 2) + " " + time };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta nustatyti datos ir laiko! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintReceiptCopy()
        {
            object[] mParam = new object[] { 109, "1" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti kvito kopijos! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> PrintBarcode(string barcode, decimal type)
        {
            object[] mParam = new object[] { 84, type + ", " + barcode };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atspausdinti barkodo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<string> CheckDateTime()
        {
            object[] mParam = new object[] { 62, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                return string.Empty;
            else
                return result.Substring(3);
        }

        public async Task<string> CheckStatus(int status)
        {
            object[] mParam = new object[] { status };
            return await ExecuteRequestAsync("Status", mParam);
        }

        public async Task<bool> FiscalReturnOpen(string documentNo)
        {
            object[] mParam = new object[] { 48, $"G,{documentNo}" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta atidaryti fiskalinio grąžinimo kvito! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalReturnItem(string productName, char taxTypeValue, decimal priceSum)
        {
            int length = productName.Length;
            if (length > 42)
                length = 42;

            var totalAmount = Math.Round(priceSum, 2, MidpointRounding.AwayFromZero);
            var productNameFull = productName.Substring(0, length);

            object[] mParam = new object[]
            {
                49,
                 productNameFull + Convert.ToChar(9) + taxTypeValue + totalAmount.ToString().Replace(',', '.')
            };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("Nepavyksta įvykdyti fiskalinio grąžinimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<bool> FiscalReturnPay(string type, decimal amount)
        {
            object[] mParam = new object[] { 53, Convert.ToChar(9) + type + amount.ToString().Replace(',', '.') };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);

            if (!success)
                throw new Exception("Nepavyksta įvykdyti fiskalinio grąžinimo apmokėjimo! \nFunkcija: cmdline(" + mParam.GetValue(0).ToString() + ", \"" + mParam.GetValue(1).ToString() + "\"). \nKlaida: " + result);

            return success;
        }

        public async Task<decimal> GetCashAmount()
        {
            object[] mParam = new object[] { 70, "" };
            var result = await ExecuteRequestAsync("cmdline", mParam);
            var success = EnsureSuccessResult(result);
            if (!success)
                throw new Exception("");
            return ParseCashAmount(result);
        }

        public async Task<CashPaymentRoundingResponse> CashPaymentRoundingSimulation(decimal reciptSum, decimal cashSum, decimal creditSum, bool creditLast = true)
        {
            if (!Session.FP550.OPENED || Session.Devices.fiscal != 1) 
            {
                return new CashPaymentRoundingResponse();
            }

            var creditLastValue = creditLast ? "1" : "0";
            object[] mParam = new object[]
            {
                177,
                $"{reciptSum.ToString().Replace(',', '.')}," +
                $"{cashSum.ToString().Replace(',', '.')}," +
                $"{creditSum.ToString().Replace(',', '.')}," +
                $"{creditLastValue}"
            };

            var result = await ExecuteRequestAsync("cmdline", mParam);

            if (result.StartsWith("OK"))
            {
                var parts = result.Split(',');
                if (parts.Length == 6)
                {
                    return new CashPaymentRoundingResponse
                    {
                        ResponseStatus = GetResponseStatus(result),
                        IsValid = parts[1] == "1",
                        IsPaid = parts[2] == "1",
                        CashAmountToFinish = decimal.Parse(parts[3], CultureInfo.InvariantCulture),
                        CreditAmountToFinish = decimal.Parse(parts[4], CultureInfo.InvariantCulture),
                        RoundingValue = decimal.Parse(parts[5], CultureInfo.InvariantCulture)
                    };
                }
            }
            else if (result.StartsWith("NO") || result.StartsWith("ER"))
            {
                return new CashPaymentRoundingResponse
                {
                    ResponseStatus = GetResponseStatus(result),
                    IsValid = false,
                    IsPaid = false,
                    CashAmountToFinish = 0,
                    CreditAmountToFinish = 0,
                    RoundingValue = 0
                };
            }

            throw new Exception("Unexpected response format while attempting to simulate rounding: " + result);
        }

        private decimal ParseCashAmount(string result)
        {
            const int ExpectedArgCount = 5;
            string[] args = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length != ExpectedArgCount)
            {
                return 0.0m;
            }
            if (decimal.TryParse(args[2], out decimal cashAmount))
            {
                return Math.Round(cashAmount / 100, 2);
            }
            return 0.0m;
        }

        private bool EnsureSuccessResult(string result)
        {
            return result.Length >= 2 && result.Substring(0, 2) == "OK";
        }

        private string CenterText(string text, int width)
        {
            if (text.Length >= width)
                return text.Substring(0, width);

            int padding = width - text.Length;
            int leftPadding = padding / 2;
            int rightPadding = padding - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }

        private Enumerator.PosResponseStatus GetResponseStatus(string result)
        {
            if (result.Length >= 2)
            {
                switch (result.Substring(0, 2))
                {
                    case "OK":
                        return Enumerator.PosResponseStatus.OK;
                    case "NO":
                        return Enumerator.PosResponseStatus.NO;
                    case "ER":
                        return Enumerator.PosResponseStatus.ER;
                }
            }
            return Enumerator.PosResponseStatus.Unknown;
        }
    }
}