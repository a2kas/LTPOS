using POS_display.Helpers;
using POS_display.Models.Recipe;
using POS_display.Presenters.ERecipe;
using POS_display.Repository.Recipe;
using POS_display.Views.ERecipe;
using System;
using System.Linq;
using TamroUtilities.HL7.Models;

namespace POS_display.Popups.display1_popups.ERecipe
{
    public partial class VaccineSellUserControl : UserControlBase, IVaccineSellUser
    {
        #region Members
        private readonly VaccineSellUserPresenter _vaccineSellUserPresenter;
        private PatientDto _patient;
        private VaccinationEntry _vaccinationEntry;
        private Items.posd _posd;
        #endregion

        #region Consturctor
        public VaccineSellUserControl(PatientDto patient, VaccinationEntry vaccineEntry)
        {          
            InitializeComponent();
            _patient = patient;
            _vaccinationEntry = vaccineEntry;
            _vaccineSellUserPresenter = new VaccineSellUserPresenter(this, this, new RecipeRepository());
        }
        #endregion

        #region Properties
        public PatientDto Patient 
        {
            get { return _patient; }
            set { _patient = value; }
        }
        public string VaccineFullName
        {
            get => LblVaccinationItemName.Text;
            set => LblVaccinationItemName.Text = value;
        }

        public string SerialNumber
        {
            get => TbSerialNumber.Text;
            set => TbSerialNumber.Text = value;
        }

        public DateTime VaccinationDate
        {
            get => DtpVaccinationDate.Value;
            set
            {
                if (value < DtpVaccinationDate.MinDate)
                    DtpVaccinationDate.Value = DtpVaccinationDate.MaxDate;
                else if (value > DtpVaccineExpiryDate.MaxDate)
                    DtpVaccinationDate.Value = DtpVaccinationDate.MaxDate;
                else
                    DtpVaccinationDate.Value = value;
            }
        }

        public string VaccinatedDose
        {
            get => TbVaccinatedDose.Text;
            set => TbVaccinatedDose.Text = value;
        }

        public DateTime VaccineExpiryDate
        {
            get => DtpVaccineExpiryDate.Value;
            set
            {
                if (value < DtpVaccineExpiryDate.MinDate)
                    DtpVaccineExpiryDate.Value = DtpVaccineExpiryDate.MaxDate;
                else if (value > DtpVaccineExpiryDate.MaxDate)
                    DtpVaccineExpiryDate.Value = DtpVaccineExpiryDate.MaxDate;
                else
                    DtpVaccineExpiryDate.Value = value;
            }            
        }
        public int VaccinationTypeindex
        {
            get => cmbVaccinationType.SelectedIndex;
            set => cmbVaccinationType.SelectedIndex = value;
        }
        public object VaccinationType
        {
            get => cmbVaccinationType.SelectedItem;
            set => cmbVaccinationType.SelectedItem = value;
        }
        #endregion

        #region Event/Delegates
        public delegate void CloseEventHandler(object sender, object data, Enumerator.VaccineOrderEvent evt);
        public event CloseEventHandler CloseEvent;
        #endregion

        #region Public methods
        public void RaiseCloseEvent(object data, Enumerator.VaccineOrderEvent voe)
        {
            CloseEvent?.Invoke(this, data, voe);
        }

        public void FillVaccineData(Items.posd posd, string serialNumber, string doseNumber = "") 
        {
            VaccineFullName = _vaccinationEntry?.Prescription?.ImmunizationRecommendation?.VaccineName ?? string.Empty;
            SerialNumber = serialNumber;
            VaccinatedDose = doseNumber;
            _posd = posd;
        }

        public void Close() 
        {
            RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Closed);
        }
        #endregion

        #region Actions
        private void BtnClose_Click(object sender, EventArgs e)
        {
            RaiseCloseEvent(_posd, Enumerator.VaccineOrderEvent.Aborted);
        }

        public async void BtnSave_Click(object sender, EventArgs e)
        {
            if (Program.Display1.PoshItem.PosdItems != null ||
                Program.Display1.PoshItem.PosdItems.Count > 0)
            {
                await _vaccineSellUserPresenter.CreateVaccineDispensation(
                    Program.Display1.PoshItem.PosdItems.FirstOrDefault(),
                    _vaccinationEntry.Prescription);
            }
            else
                Close();
        }
        #endregion
    }
}
