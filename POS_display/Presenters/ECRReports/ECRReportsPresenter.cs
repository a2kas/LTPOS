using AutoMapper;
using POS_display.Models.ECRReports;
using POS_display.Repository.ECRReports;
using POS_display.Repository.Pos;
using POS_display.Utils.Logging;
using POS_display.Views.ECRReports;
using POS_display.Views.NegativeSales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace POS_display.Presenters.ECRReports
{
    public class ECRReportsPresenter : BasePresenter, IECRReportsPresenter
    {
        #region Members
        private readonly IECRReportsView _view;
        private readonly IPosRepository _posRepository;
        private readonly IECRReportsRepository _ecrReportsRepository;
        private readonly IMapper _mapper;
        private const int AspaSerialNumberKey = 32;
        #endregion

        #region Constructor
        public ECRReportsPresenter(
            IECRReportsView view,
            IPosRepository posRepository,
            IECRReportsRepository ecrReportsRepository,
            IMapper mapper)
        {
            _view = view ?? throw new ArgumentNullException();
            _posRepository = posRepository ?? throw new ArgumentNullException();
            _ecrReportsRepository = ecrReportsRepository ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }
        #endregion

        #region Public methods
        public async Task InitOperationsData()
        {
            var bindingSource = new BindingSource();
            IList<ECRReport> commands = await _ecrReportsRepository.Get();
            bindingSource.DataSource = commands;
            _view.Report.DataSource = bindingSource.DataSource;
            _view.Report.DisplayMember = "Command";
            _view.Report.ValueMember = "Id";
            _view.Report.Focus();
        }

        public async Task<decimal> PerformExecute(string selectedIndex)
        {
            decimal poshId = 0;
            _view.Calc.Enabled = false;
  
            if (selectedIndex == "4")//jei Z ataskaita visu pirma i DB tik tada POS
            {
                poshId = await _posRepository.GetECRReports(selectedIndex.ToDecimal(), _view.Change.Text.ToDecimal());
                await ExecuteReportZ();
            }
            else
            {
                if (await DoReportPOS(selectedIndex))
                    poshId = await _posRepository.GetECRReports(selectedIndex.ToDecimal(), _view.Change.Text.ToDecimal());
            }
            return poshId;
        }

        public void EnableControls(string selectedIndex)
        {
            if (selectedIndex == "1") //Atidaryti stalčių
            {
                _view.DateFrom.Enabled = false;
                _view.DateTo.Enabled = false;
                _view.SetDate.Enabled = false;
                _view.SetTime.Enabled = false;
                _view.Change.Enabled = false;
                _view.Calc.Enabled = true;
            }

            if (selectedIndex == "3") //X ataskaita
            {
                _view.DateFrom.Enabled = false;
                _view.DateTo.Enabled = false;
                _view.SetDate.Enabled = false;
                _view.SetTime.Enabled = false;
                _view.Change.Enabled = false;
                _view.Calc.Enabled = true;
            }

            if (selectedIndex == "5" || selectedIndex == "6" || selectedIndex == "13" || selectedIndex == "14") //Pinigų įdėjimas, Pinigų išėmimas, Avanso įdėjimas
            {
                _view.DateFrom.Enabled = false;
                _view.DateTo.Enabled = false;
                _view.SetDate.Enabled = false;
                _view.SetTime.Enabled = false;
                _view.Change.Enabled = true;
                _view.Change.Select();
                _view.Calc.Enabled = false;
            }

            if (selectedIndex == "7" || selectedIndex == "8") //Suminė periodinė ataskaita, Detali periodinė ataskaita
            {
                _view.DateFrom.Enabled = true;
                _view.DateFrom.Select();
                _view.DateTo.Enabled = true;
                _view.SetDate.Enabled = false;
                _view.SetTime.Enabled = false;
                _view.Change.Enabled = false;
                _view.Calc.Enabled = true;
            }

            if (selectedIndex == "11") //Nustatyti datą ir laiką
            {
                _view.DateFrom.Enabled = false;
                _view.DateTo.Enabled = false;
                _view.SetDate.Enabled = false;
                _view.SetTime.Enabled = true;
                _view.SetTime.Select();
                _view.Change.Enabled = false;
                _view.Calc.Enabled = true;
            }

            if (selectedIndex == "12") //Paskutinio kvito kopija
            {
                _view.DateFrom.Enabled = false;
                _view.DateTo.Enabled = false;
                _view.SetDate.Enabled = true;
                _view.SetDate.Select();
                _view.SetTime.Enabled = false;
                _view.Change.Enabled = false;
                _view.Calc.Enabled = true;
            }
        }
        public void ChangeTextChanged(object sender)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.ToDecimal() > 0)
                _view.Calc.Enabled = true;
            else
                _view.Calc.Enabled = false;
        }
        #endregion

        #region Private methods
        private async Task ExecuteReportZ()
        {
            if (Session.Devices.fiscal == 1 && await Session.FP550.PrintReportZ())
            {
                if (Session.getParam("FISCALDATA", "SQLITE") == "0")
                {
                    var list = GetCashDeskEntries();
                    if (list.Any())
                    {
                        var pozLine = list.Last();
                        pozLine.Created = DateTime.Now;
                        await _posRepository.CreatePosZ(pozLine);
                    }
                }
                else
                {
                    var pozLine = await GetLastCashDeskEntry();
                    if (pozLine != null)
                    {
                        pozLine.Created = DateTime.Now;
                        await _posRepository.CreatePosZ(pozLine);
                    }
                }

                if (!await CheckNegativeSales())
                {
                    await _posRepository.TransferChequesFromPOS(
                        Session.SystemData.code,
                        Session.Devices.debtorid,
                        DateTime.Now.ToString("yyyy.MM.dd"));
                }
                _view.FormDialogResult = DialogResult.OK;
            }
        }

        private async Task<bool> CheckNegativeSales()
        {
           var lackOfSales = await _posRepository.GetLackOfSales();
            if (lackOfSales.Any()) 
            {
                using (var lackOfSalesView = new LackOfSalesView()) 
                {
                    lackOfSalesView.LackOfSales = lackOfSales;
                    lackOfSalesView.ShowDialog();
                }
                return true;
            }
            return false;
        }

        private async Task<bool> DoReportPOS(string selectedIndex)
        {
            bool result = true;
            if (Session.Devices.fiscal != 1 || selectedIndex == "0")
                return result;

            switch (selectedIndex)
            {
                case "1": // Atidaryti stalciu
                    result = await Session.FP550.OpenDrawer();
                    break;
                case "3": // X ataskaita
                    result = await Session.FP550.PrintReportX();
                    break;
                case "5": // Pinigų įdėjimas
                    if (_view.Change.Text.ToDecimal() != 0)
                        result = await Session.FP550.MoneyIn(Math.Abs(_view.Change.Text.ToDecimal()));
                    break;
                case "6": // Pinigų išėmimas
                    if (_view.Change.Text.ToDecimal() != 0)
                        result = await Session.FP550.MoneyOut(Math.Abs(_view.Change.Text.ToDecimal()));
                    break;
                case "7": // Suminė periodinė ataskaita
                    result = await Session.FP550.PrintReportSum(_view.DateFrom.Text, _view.DateTo.Text);
                    break;
                case "8": // Detali periodinė ataskaita
                    result = await Session.FP550.PrintReportDetail(_view.DateFrom.Text, _view.DateTo.Text);
                    break;
                case "11": // Nustatyti datą ir laiką
                    if (helpers.alert(Enumerator.alert.warning, "Ar prieš nustant datą ir laiką atlikote operaciją - 'Z ataskaita' ? Kitu atveju negalėsite nustayti datos ir laiko", true))
                    {
                        result = await Session.FP550.SetDateTime(_view.SetDate.Text, _view.SetTime.Text);
                    }
                    break;
                case "12": // Paskutinio kvito kopija
                    result = await Session.FP550.PrintReceiptCopy();
                    break;
            }

            return result;
        }

        private async Task<PosZLine> GetLastCashDeskEntry()
        {
            PosZLine posZLine = null;

            var lastEKJEntry = await _posRepository.GetLastEKJEntryByType(Enumerator.EKJType.ZReport);
            var deviceSettingValue = await _posRepository.GetDeviceSetttingValueByKey(AspaSerialNumberKey);

            if (lastEKJEntry != null && deviceSettingValue != null) 
            {
                var ZReportEntry = await _posRepository.GetZReportEntryByEkjId(lastEKJEntry.Id);

                posZLine = _mapper.Map<PosZLine>((lastEKJEntry, ZReportEntry, deviceSettingValue));
            }

            return posZLine;
        }

        private List<PosZLine> GetCashDeskEntries()
        {
            List<PosZLine> entries = new List<PosZLine>();
            string filePath = Session.getParam("CASHDESKDATA", "FILEPATH");
            if (!Directory.Exists(filePath))
            {
                Serilogger.GetLogger().Error($"Cash desk data folder: {filePath} does not exist");
                return entries;
            }

            string[] files = Directory.GetFiles(filePath, "*.mem");
            if (files.Length == 0)
            {
                Serilogger.GetLogger().Error($"Did not find any cash desk data file in {filePath} folder with .MEM extension");
                return entries;
            }


            if (!Regex.Match(Session.Devices.deviceno, @"IN\d+").Success)
            {
                Serilogger.GetLogger().Error("Wrong device no, it does not fit format 'IN{number}'");
                return entries;
            }

            var plainDeviceNo = Regex.Match(Session.Devices.deviceno, @"\d+").Value;
            var matches = Directory.GetFiles(filePath).Where(path => Regex.Match(path.ToLowerInvariant(), $"fiscal{plainDeviceNo}.mem").Success);
            if (!matches.Any())
            {
                Serilogger.GetLogger().Error($"Did not find cash desk data file 'Fiscal{plainDeviceNo}.mem' in the '{filePath}' folder.");
                return entries;
            }

            string fullFileName = matches.FirstOrDefault();
            string fileNameWithouExt = Path.GetFileNameWithoutExtension(fullFileName);

            return LoadCashDeskData(fullFileName).Lines.ToList();
        }

        private PosZData LoadCashDeskData(string filePath) 
        {
            XDocument doc = XDocument.Load(filePath);
            XmlSerializer deserializer = new XmlSerializer(typeof(PosZData));
            using (var reader = doc.Root.CreateReader())
            {
                return (PosZData)deserializer.Deserialize(reader);
            }
        }
        #endregion
    }
}
