using POS_display.Helpers;
using POS_display.Presenters.ERecipe;
using POS_display.Repository.Recipe;
using POS_display.Views.ERecipe;
using System;
using System.Linq;
using TamroUtilities.HL7.Models;

namespace POS_display.Popups.display1_popups.ERecipe
{
    public partial class VaccineSellUserControlV2 : UserControlBase, IVaccineSellUserV2
    {
        #region Members
        private readonly VaccineSellUserPresenterV2 _vaccineSellUserPresenter;
        private PatientDto _patient;
        private LowIncomeDto _lowIncome;
        private MedicationDto _medication;
        private Items.posd _posd;
        #endregion

        #region Consturctor
        public VaccineSellUserControlV2(PatientDto patient, LowIncomeDto lowIncome)
        {          
            InitializeComponent();
            _patient = patient;
            _lowIncome = lowIncome;
            _vaccineSellUserPresenter = new VaccineSellUserPresenterV2(this, this, new RecipeRepository());

            cbInfectiousDisease.Items.AddRange(Session.VaccinationInfectiousDiseases.Keys.ToArray());
            cbInfectiousDisease.SelectedIndex = 0;
        }
        #endregion

        #region Properties
        public PatientDto Patient 
        {
            get { return _patient; }
            set { _patient = value; }
        }

        public LowIncomeDto LowIncome
        {
            get { return _lowIncome; }
            set { _lowIncome = value; }
        }

        public MedicationDto Medication
        {
            get { return _medication; }
            set { _medication = value; }
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

        public void Close() 
        {
            RaiseCloseEvent(null, Enumerator.VaccineOrderEvent.Closed);
        }

        public void FillVaccineData(Items.posd posd, MedicationDto medication)
        {
            LblVaccinationItemName.Text = medication.ProprietaryName;
            _medication = medication;
            _posd = posd;
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
                    Program.Display1.PoshItem.PosdItems.FirstOrDefault());
            }
            else
                Close();
        }
        #endregion
    }
}
