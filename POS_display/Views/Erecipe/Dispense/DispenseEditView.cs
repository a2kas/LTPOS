using POS_display.Presenters.Erecipe.Dispense;
using POS_display.Repository.Recipe;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace POS_display.Views.Erecipe.Dispense
{
    public partial class DispenseEditView : FormBase, IDispenseEditView, IBaseView
    {
        #region Members
        private IDispenseEditPresenter _dispenseEditPresenter;
        #endregion

        #region Events
        public event EventHandler DayCountChanged;
        public event EventHandler MedicationValidUntilChanged;
        public event EventHandler SalePriceChanged;
        public event EventHandler PatientAmountChanged;
        public event EventHandler ReimbursedAmountChanged;
        #endregion

        #region Constructor
        public DispenseEditView()
        {
            InitializeComponent();
            _dispenseEditPresenter = new DispenseEditPresenter(this, Session.eRecipeUtils, new RecipeRepository());
            SubscribeToTextBoxEvents();
            SetupInputValidation();
        }
        #endregion

        #region Properties
        public string SaleDate
        {
            get => txtSaleDate.Text;
            set => txtSaleDate.Text = value ?? string.Empty;
        }

        public string MedicationValidUntil
        {
            get => txtMedicationValidUntil.Text;
            set => txtMedicationValidUntil.Text = value ?? string.Empty;
        }

        public string DayCount
        {
            get => txtDayCount.Text;
            set => txtDayCount.Text = value ?? string.Empty;
        }

        public string IssuedQuantity
        {
            get => txtIssuedQuantity.Text;
            set => txtIssuedQuantity.Text = value ?? string.Empty;
        }

        public string SalePrice
        {
            get => txtSalePrice.Text;
            set => txtSalePrice.Text = value ?? string.Empty;
        }

        public string ReimbursedAmount
        {
            get => txtReimbursedAmount.Text;
            set => txtReimbursedAmount.Text = value ?? string.Empty;
        }

        public string AdditionalReimbursedAmount
        {
            get => txtAdditionalReimbursedAmount.Text;
            set => txtAdditionalReimbursedAmount.Text = value ?? string.Empty;
        }

        public string PatientAmount
        {
            get => txtPatientAmount.Text;
            set => txtPatientAmount.Text = value ?? string.Empty;
        }

        public string EditReason
        {
            get => txtEditReason.Text;
            set => txtEditReason.Text = value ?? string.Empty;
        }
        #endregion

        #region Public methods
        public async Task Init(string compositionId)
        {
            Text = $"Išdavimo redagavimas ID: {compositionId}";
            await _dispenseEditPresenter.Init(compositionId);
        }
        #endregion

        #region Private methods        
        private void SubscribeToTextBoxEvents()
        {
            txtDayCount.TextChanged += (sender, e) => DayCountChanged?.Invoke(sender, e);
            txtMedicationValidUntil.TextChanged += (sender, e) => MedicationValidUntilChanged?.Invoke(sender, e);

            txtSalePrice.TextChanged += (sender, e) => SalePriceChanged?.Invoke(sender, e);
            txtPatientAmount.TextChanged += (sender, e) => PatientAmountChanged?.Invoke(sender, e);
            txtReimbursedAmount.TextChanged += (sender, e) => ReimbursedAmountChanged?.Invoke(sender, e);
        }

        private void SetupInputValidation()
        {
            txtDayCount.KeyPress += NumericOnly_KeyPress;
            txtIssuedQuantity.KeyPress += NumericOnly_KeyPress;

            txtSalePrice.KeyPress += DecimalOnly_KeyPress;
            txtReimbursedAmount.KeyPress += DecimalOnly_KeyPress;
            txtPatientAmount.KeyPress += DecimalOnly_KeyPress;

            txtMedicationValidUntil.KeyPress += DateOnly_KeyPress;
            txtMedicationValidUntil.Leave += MedicationValidUntil_Leave;

            txtMedicationValidUntil.Enter += MedicationValidUntil_Enter;
            SetDatePlaceholder();
        }

        private void NumericOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void DecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar))
                return;

            if ((e.KeyChar == ',' || e.KeyChar == '.') && !textBox.Text.Contains(",") && !textBox.Text.Contains("."))
                return;

            e.Handled = true;
        }

        private void DateOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;

            if (char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '/' || e.KeyChar == '.')
                return;

            e.Handled = true;
        }

        private void MedicationValidUntil_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "YYYY-MM-DD")
            {
                textBox.Text = "";
                textBox.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void MedicationValidUntil_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetDatePlaceholder();
                return;
            }

            if (TryParseDate(textBox.Text, out DateTime parsedDate))
            {
                textBox.Text = parsedDate.ToString("yyyy-MM-dd");
                textBox.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            else
            {
                MessageBox.Show("Netinkamas datos formatas. Naudokite formatą YYYY-MM-DD arba DD/MM/YYYY.",
                               "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Focus();
            }
        }

        private void SetDatePlaceholder()
        {
            if (string.IsNullOrWhiteSpace(txtMedicationValidUntil.Text))
            {
                txtMedicationValidUntil.Text = "YYYY-MM-DD";
                txtMedicationValidUntil.ForeColor = System.Drawing.SystemColors.GrayText;
            }
        }

        private bool TryParseDate(string dateString, out DateTime date)
        {
            date = default;

            if (string.IsNullOrWhiteSpace(dateString) || dateString == "YYYY-MM-DD")
                return false;

            string[] formats = {
                "yyyy-MM-dd",
                "yyyy/MM/dd",
                "yyyy.MM.dd",
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "dd.MM.yyyy",
                "MM/dd/yyyy",
                "MM-dd-yyyy",
                "MM.dd.yyyy"
            };

            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return true;
                }
            }

            return DateTime.TryParse(dateString, out date);
        }

        private async void btnSave_Click(object sender, System.EventArgs e)
        {
            DialogResult result = DialogResult.None;

            if (!string.IsNullOrWhiteSpace(txtMedicationValidUntil.Text) &&
                txtMedicationValidUntil.Text != "YYYY-MM-DD" &&
                !TryParseDate(txtMedicationValidUntil.Text, out _))
            {
                MessageBox.Show("Prašome įvesti tinkamą datą lauke 'Vaisto pakanka iki'.",
                               "Klaida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMedicationValidUntil.Focus();
                return;
            }

            await ExecuteWithWaitAsync(async () =>
            {
                var errorMessage = _dispenseEditPresenter.Validate();
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    throw new Exception(errorMessage);
                await _dispenseEditPresenter.Save();
                result = DialogResult.OK;
            });
            DialogResult = result;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion
    }
}