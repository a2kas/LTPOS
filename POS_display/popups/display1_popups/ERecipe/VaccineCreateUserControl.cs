using Microsoft.Extensions.DependencyInjection;
using POS_display.Helpers;
using POS_display.Presenters.ERecipe;
using POS_display.Views.ERecipe;
using System;
using System.Linq;
using System.Windows.Forms;
using Tamroutilities.Client;
using TamroUtilities.HL7.Models;

namespace POS_display.Popups.display1_popups.ERecipe
{
    public partial class VaccineCreateUserControl : UserControlBase, IVaccineCreateUser
    {
        #region Members
        private readonly VaccineCreateUserPresenter _vaccineCreateUserPresenter;
        private string _npakid7 = string.Empty;
        private PatientDto _patient;
        private LowIncomeDto _lowIncome;
        private Models.Barcode _barcodeModel;
        #endregion

        #region Construct
        public VaccineCreateUserControl(PatientDto patient, LowIncomeDto lowIncome)
        {
            InitializeComponent();
          
            _vaccineCreateUserPresenter = new VaccineCreateUserPresenter(this, this);
            _patient = patient;
            _lowIncome = lowIncome;

            cbInfectiousDisease.Items.AddRange(Session.VaccinationInfectiousDiseases.Keys.ToArray());
            cbInfectiousDisease.SelectedIndex = 0;
        }

        private void VaccineCreateUserControl_Load(object sender, EventArgs e)
        {
            TbBarcode.Select();
        }
        #endregion

        #region Properties
        public string NPAKID7
        {
            get => _npakid7;
            set => _npakid7 = value;
        }

        public string InfectiousDiseaseCode
        {
            get
            {
                return Session
                    .VaccinationInfectiousDiseases
                    .FirstOrDefault(e => e.Key == InfectiousDisease)
                    .Value;
            }
        }

        public string InfectiousDisease
        {
            get => cbInfectiousDisease.Text;
            set => cbInfectiousDisease.Text = value;
        }

        public string Barcode
        {
            get => TbBarcode.Text;
            set => TbBarcode.Text = value;
        }

        public string Medicine
        {
            get => TbMedicine.Text;
            set => TbMedicine.Text = value;
        }

        public string DoseNumber
        {
            get => TbDoseSerialNumber.Text;
            set => TbDoseSerialNumber.Text = value;
        }

        public string Notes
        {
            get => RtbNote.Text;
            set => RtbNote.Text = value;
        }

        public PatientDto Patient
        {
            get => _patient;
            set => _patient = value;
        }

        public LowIncomeDto LowIncome
        {
            get => _lowIncome;
            set => _lowIncome = value;
        }

        public Models.Barcode BarcodeModel 
        {
            get => _barcodeModel;
        }
        #endregion

        #region Delegate/Events
        public delegate void CloseEventHandler(object sender, object data, Enumerator.VaccineOrderEvent evt);
        public event CloseEventHandler CloseEvent;
        #endregion

        public void RaiseCloseEvent(object data, Enumerator.VaccineOrderEvent voe)
        {
            CloseEvent?.Invoke(this, data, voe);
        }

        #region Actions
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            await _vaccineCreateUserPresenter.CreateVaccinePescription();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Closed);
        }
        #endregion

        private async void TbBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((int)e.KeyChar)
            {
                case 29:
                    TextBox tb = sender as TextBox;
                    int i = tb.SelectionStart;
                    tb.Text = tb.Text.Insert(tb.SelectionStart, Session.BarcodeParser.groupSeperator.ToString());
                    tb.SelectionStart = i + 5;
                    e.Handled = true;
                    break;
                case (char)Keys.Enter:
                    _barcodeModel = new Models.Barcode() { BarcodeStr = Barcode };
                    Presenters.BarcodePresenter BCPresenter = new Presenters.BarcodePresenter(null, _barcodeModel);
                    await BCPresenter.GetDataFromBarcode();
                    if (_barcodeModel.BarcodeID == 0m)
                    {
                        helpers.alert(Enumerator.alert.warning, "Prekės barkodas nerastas!");
                        return;
                    }
                    _barcodeModel.NpakId7 = await Session.SamasUtils.GetNpakid7ByProductId(_barcodeModel.ProductId);
                    if (_barcodeModel.NpakId7 <= 0)
                    {
                        helpers.alert(Enumerator.alert.warning, "Prekės NpakId7 nerastas!");
                        return;
                    }
                    _vaccineCreateUserPresenter.FindMedicine(_barcodeModel.NpakId7.ToString());
                    break;
                case ',':
                    e.KeyChar = '.';
                    break;
            }
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void TbDoseSerialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
