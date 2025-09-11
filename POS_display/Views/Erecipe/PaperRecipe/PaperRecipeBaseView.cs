using POS_display.Items.eRecipe;
using POS_display.Items.Prices;
using POS_display.Models.Recipe;
using POS_display.Presenters.Erecipe.PaperRecipe;
using POS_display.Repository.Recipe;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    public partial class PaperRecipeBaseView : FormBase, IPaperRecipeBaseView
    {
        #region Members
        private readonly IBasePaperRecipePresenter _paperRecipePresenter;
        private GenericItem _genericItem;
        private bool _isFieldValueChanging;
        #endregion

        #region Constructor
        public PaperRecipeBaseView()
        {
            InitializeComponent();
            _paperRecipePresenter = CreatePresenterInstance();
        }
        #endregion

        #region Virtual methods
        public virtual IBasePaperRecipePresenter CreatePresenterInstance() 
        {
            return new BasePaperRecipePresenter(this, Session.KVAP, Session.eRecipeUtils, new RecipeRepository());
        }
        #endregion

        #region Properties
        public bool IsFieldValueChanging 
        {
            get => _isFieldValueChanging;
            set => _isFieldValueChanging = value;
        }

        public DateTimePicker CreationDate
        {
            get => dtpCreationDate;
            set => dtpCreationDate = value;
        }

        public DateTimePicker ExpiryStart
        {
            get => dtpExpiryStart;
            set => dtpExpiryStart = value;
        }

        public TextBox ValidityPeriod
        {
            get => tbValidityPeriod;
            set => tbValidityPeriod = value;
        }

        public DateTimePicker ExpiryEnd
        {
            get => dtpExpiryEnd;
            set => dtpExpiryEnd = value;
        }

        public TextBox DoctorCode
        {
            get => tbDoctorCode;
            set => tbDoctorCode = value;
        }

        public Label DoctorCodeLabel
        {
            get => lblDoctorCode;
            set => lblDoctorCode = value;
        }

        public TextBox Stamp
        {
            get => tbStamp;
            set => tbStamp = value;
        }

        public Label StampLabel
        {
            get => lblStamp;
            set => lblStamp = value;
        }

        public TextBox SveidraID
        {
            get => tbSveidraID;
            set => tbSveidraID = value;
        }

        public Label SveidraIDLabel
        {
            get => lblSveidraID;
            set => lblSveidraID = value;
        }

        public TextBox InfoForSpecialist
        {
            get => tbInfoForSpecialist;
            set => tbInfoForSpecialist = value;
        }

        public CheckBox SpecialPrescription
        {
            get => cbSpecialPrescription;
            set => cbSpecialPrescription = value;
        }

        public CheckBox SpecialistDecision
        {
            get => cbSpecialistDecision;
            set => cbSpecialistDecision = value;
        }

        public CheckBox GKDecision
        {
            get => cbGKDecision;
            set => cbGKDecision = value;
        }

        public CheckBox CertainName
        {
            get => cbCertainName;
            set => cbCertainName = value;
        }

        public CheckBox BrandName
        {
            get => cbBrandName;
            set => cbBrandName = value;
        }

        public CheckBox LabelingExemption
        {
            get => chbLabelingExemption;
            set => chbLabelingExemption = value;
        }


        public ComboBox NominalConfirm
        {
            get => cbNominalConfirm;
            set => cbNominalConfirm = value;
        }

        public DateTimePicker NominalDeclarationValid
        {
            get => dtpNominalDeclarationValid;
            set => dtpNominalDeclarationValid = value;
        }

        // TAB 2
        public TextBox Medication
        {
            get => tbMedication;
            set => tbMedication = value;
        }

        public TextBox PrescriptionDosesAmount
        {
            get => tbPrescriptionDosesAmount;
            set => tbPrescriptionDosesAmount = value;
        }

        public ComboBox PharmaceuticalForm
        {
            get => cbPharmaceuticalForm;
            set => cbPharmaceuticalForm = value;
        }

        public ComboBox Route
        {
            get => cbRoute;
            set => cbRoute = value;
        }

        public TextBox OneTimeDose
        {
            get => tbOneTimeDose;
            set => tbOneTimeDose = value;
        }

        public ComboBox PharmaceuticalFormMeasureUnit 
        {
            get => cbPharmaceuticalFormMeasureUnit;
            set => cbPharmaceuticalFormMeasureUnit = value;
        }

        public TextBox TimesPer
        {
            get => tbTimesPer;
            set => tbTimesPer = value;
        }

        public ComboBox TimesPerSelection
        {
            get => cbTimesPerSelection;
            set => cbTimesPerSelection = value;
        }

        public CheckBox InMorning
        {
            get => chbInMorning;
            set => chbInMorning = value;
        }
        public CheckBox DuringLunch
        {
            get => chbDuringLunch;
            set => chbDuringLunch = value;
        }

        public CheckBox InEvening
        {
            get => chbInEvening;
            set => chbInEvening = value;
        }

        public CheckBox AsNeeded
        {
            get => chbAsNeeded;
            set => chbAsNeeded = value;
        }

        public TextBox DailyDose
        {
            get => tbDailyDose;
            set => tbDailyDose = value;
        }

        public CheckBox BeforeEating
        {
            get => chbBeforeEating;
            set => chbBeforeEating = value;
        }

        public CheckBox AfterEating
        {
            get => chbAfterEating;
            set => chbAfterEating = value;
        }

        public CheckBox DuringEating
        {
            get => chbDuringEating;
            set => chbDuringEating = value;
        }

        public CheckBox RegardlessEating
        {
            get => chbRegardlessEating;
            set => chbRegardlessEating = value;
        }

        public CheckBox BeforeSleeping
        {
            get => chbBeforeSleeping;
            set => chbBeforeSleeping = value;
        }

        // TAB 3

        public DateTimePicker SalesDate
        {
            get => dtpSalesDate;
            set => dtpSalesDate = value;
        }

        public TextBox DispensedMedication
        {
            get => tbDispensedMedication;
            set => tbDispensedMedication = value;
        }

        public TextBox DipsenseDosesAmount
        {
            get => tbDipsenseDosesAmount;
            set => tbDipsenseDosesAmount = value;
        }

        public TextBox AmountPerDay
        {
            get => tbAmountPerDay;
            set => tbAmountPerDay = value;
        }

        public TextBox AmountOfDays
        {
            get => tbAmountOfDays;
            set => tbAmountOfDays = value;
        }

        public DateTimePicker EnoughUntil
        {
            get => dtpEnoughUntil;
            set => dtpEnoughUntil = value;
        }

        public TextBox Price
        {
            get => tbPrice;
            set => tbPrice = value;
        }

        public Button NextFirst
        {
            get => btnNextFirst;
            set => btnNextFirst = value;
        }

        public Button NextSecond
        {
            get => btnNextSecond;
            set => btnNextSecond = value;
        }

        public GenericItem MedicationItem { get; set; }
        public Recipe eRecipeItem { get; set; }
        public Items.posd PosDetail { get; set; }
        #endregion

        #region Private methods
        private void tbDoctorCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExecuteWithWait(() =>
                {
                    _paperRecipePresenter.RetriveDoctorDataByDoctorCode(tbDoctorCode.Text);
                });
            }
        }
        #endregion

        private void PaperRecipeBaseView_Load(object sender, System.EventArgs e)
        {
            if (DesignMode) return;
            ExecuteWithWait(() =>
            {
                _paperRecipePresenter.Init();
            });
        }

        private async void btnApply_Click(object sender, System.EventArgs e)
        {
            PaperRecipeRequestResponse res = new PaperRecipeRequestResponse() 
            { 
                CompositionId = string.Empty 
            };

            await ExecuteWithWaitAsync(async () =>
            {
                _paperRecipePresenter.Validate();
                res = await _paperRecipePresenter.Save();
            });

            if (!string.IsNullOrWhiteSpace(res.CompositionId)) 
            {
                helpers.alert(Enumerator.alert.info, $"Receptas sėkmingai išsaugotas E-Sveikatoje\nComposition ID: {res.CompositionId}");
                Close();
            }
        }

        private void btnNextFirst_Click(object sender, System.EventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void btnNextSecond_Click(object sender, System.EventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void dtpExpiryEnd_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.ValidateValues(); });
            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesByExpiryEnd(); });
        }

        private void tbValidityPeriod_TextChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesByValidPeriod(); });
        }

        private void dtpExpiryStart_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.ValidateValues(); });
            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesByExpiryStart(); });
        }

        private void dtpSalesDate_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesBySalesDate(); });
        }

        private void dtpEnoughUntil_ValueChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesByEnoughUntil(); });
        }

        private void tbAmountOfDays_TextChanged(object sender, System.EventArgs e)
        {
            if (_isFieldValueChanging)
                return;

            ExecuteWithWait(() => { _paperRecipePresenter.RecalculateValuesByAmountOfDays(); });
        }

        private void tbValidityPeriod_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbDoctorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbPrescriptionDosesAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbOneTimeDose_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbTimesPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbDailyDose_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbDipsenseDosesAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbAmountOfDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyInteger(e);
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowInputOnlyDecimal(e, sender as TextBox);
        }

        private void AllowInputOnlyInteger(KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void AllowInputOnlyDecimal(KeyPressEventArgs e, TextBox textBox)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true; 
            }

            if (e.KeyChar == ',' && textBox.Text.Contains(","))
            {
                e.Handled = true;
            }
        }
    }
}
