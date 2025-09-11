using System;
using System.Windows.Forms;
using POS_display.Presenters.PersonalPharmacist;
using POS_display.Repository.PersonalPharmacist;

namespace POS_display.Views.PersonalPharmacist
{
    public partial class PersonalPharmacistView : FormBase, IPersonalPharmacistView, IBaseView
    {
        #region Members
        private readonly PersonalPharmacistPresenter _personalPharmacistPresenter;
        #endregion

        #region Constructor
        public PersonalPharmacistView()
        {
            InitializeComponent();
            _personalPharmacistPresenter = new PersonalPharmacistPresenter(this, new PersonalPharmacistRepository());
            _personalPharmacistPresenter.Init();
        }
        #endregion

        #region Properties
        public TextBox ClientPersonalCode
        {
            get => tbClientPersonalCode;
            set => tbClientPersonalCode = value;
        }
        public DateTime DueDate
        {
            get => dtpDueDate.Value;
            set => dtpDueDate.Value = value;
        }
        public TextBox DoctorID 
        {
            get => tbDoctorID;
            set => tbDoctorID = value;
        }
        public TextBox DoctorName
        {
            get => tbDoctorName;
            set => tbDoctorName = value;
        }
        public TextBox DoctorSurename
        {
            get => tbDoctorSurename;
            set => tbDoctorSurename = value;
        }
        public ComboBox Hospital
        {
            get => cbHospital; 
            set => cbHospital = value;
        }
        public Button Apply
        {
            get => btnApply;
            set => btnApply = value;
        }
        public Button Cancel
        {
            get => btnCancel;
            set => btnCancel = value;
        }

        #endregion

        #region Actions
        private async void btnCancel_Click(object sender, System.EventArgs e)
        {
            await _personalPharmacistPresenter.DeletePersonalPharmacistData(Program
                .Display1
                .PoshItem
                .Id
                .ToLong());           
            DialogResult = DialogResult.Abort;
        }

        private async void btnApply_Click(object sender, System.EventArgs e)
        {
            if (!_personalPharmacistPresenter.CheckCardAvailability())
            {
                helpers.alert(Enumerator.alert.warning, "Privalo būti įvesta BENU lojalumo kortelė!");
                return;
            }
            await _personalPharmacistPresenter.SavePersonalPharmacistData(Program
                .Display1
                .PoshItem
                .Id
                .ToLong());
            DialogResult = DialogResult.OK;
        }
        #endregion

        #region Private methods
        private void tbClientPersonalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleDigit(e);
        }

        private static void HandleDigit(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)) return;
            e.Handled = true;
        }
        #endregion
    }
}
