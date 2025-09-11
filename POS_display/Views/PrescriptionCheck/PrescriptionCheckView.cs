using POS_display.Presenters.PrescriptionCheck;
using POS_display.Repository.Recipe;
using POS_display.Views;
using POS_display.Views.PrescriptionCheck;
using System;
using System.Windows.Forms;

namespace POS_display
{
    public partial class PrescriptionCheckView : FormBase, IPrescriptionCheckView, IBaseView
    {
        #region Members
        private Items.posd _posDModel = null;
        private readonly PrescriptionCheckPresenter _prescriptionCheckPresenter;
        #endregion

        #region Constructor
        public PrescriptionCheckView(Items.posd model)
        {
            _posDModel = model;
            InitializeComponent();

            _prescriptionCheckPresenter = new PrescriptionCheckPresenter(this, new RecipeRepository());
        }
        #endregion

        #region Properties
        public TextBox Doses
        {
            get { return tbDoses; }
            set { tbDoses = value; }
        }

        public TextBox QtyDay
        {
            get { return tbQtyDay; }
            set { tbQtyDay = value; }
        }

        public TextBox CountDay
        {
            get { return tbCountDay; }
            set { tbCountDay = value; }
        }

        public TextBox TillDate
        {
            get { return tbTillDate; }
            set { tbTillDate = value; }
        }

        public TextBox ValidFrom
        {
            get { return tbValidFrom; }
            set { tbValidFrom = value; }
        }

        public DateTimePicker TillDateValue
        {
            get { return dtpTillDate; }
            set { dtpTillDate = value; }
        }

        public DateTimePicker ValidFromValue
        {
            get { return dtpValidFrom; }
            set { dtpValidFrom = value; }
        }

        public Items.posd PosDModel
        {
            get { return _posDModel; }
            set { _posDModel = value; }
        }
        #endregion

        #region Actions
        private void AppendEvents()
        {
			tbDoses.TextChanged += tbDoses_TextChanged;
			tbDoses.KeyPress += tbDoses_KeyPress;
			tbQtyDay.TextChanged += tbQtyDay_TextChanged;
			tbQtyDay.KeyPress += tbQtyDay_KeyPress;
			tbCountDay.TextChanged += tbCountDay_TextChanged;
			tbCountDay.KeyPress += tbCountDay_KeyPress;
			tbValidFrom.TextChanged += tbValidFrom_TextChanged;
			dtpValidFrom.ValueChanged += dtpValidFrom_ValueChanged;
			dtpTillDate.ValueChanged += dtpTillDate_ValueChanged;
		}

		private async void PrescriptionCheckView_Load(object sender, EventArgs e)
		{
			await ExecuteWithWaitAsync(async () => await _prescriptionCheckPresenter.UpdateSession("Recepto čekis", 2));
			await ExecuteWithWaitAsync(async () => await _prescriptionCheckPresenter.Init());
			ExecuteWithWait(() => AppendEvents());
		}

		private void PrescriptionCheckView_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy)
                e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                var recipeId = await _prescriptionCheckPresenter.Save();
                if(recipeId > 0)
				    await Program.Display1.RefreshPosh();
            });
			DialogResult = DialogResult.OK;
		}

		private void dtpTillDate_ValueChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() =>
			{
				DateTimePicker dtp = sender as DateTimePicker;
				tbTillDate.Text = dtp.Value.ToString("yyyy-MM-dd");
			}, false);
		}

		private void dtpValidFrom_ValueChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.SetValidFromDisplay(sender), false);
		}

		private void tbValidFrom_TextChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.ValueChangesCalculation(sender), false);
		}

		private void tbDoses_TextChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() =>_prescriptionCheckPresenter.ValueChangesCalculation(sender));
		}

		private void tbQtyDay_TextChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.ValueChangesCalculation(sender));
		}

		private void tbCountDay_TextChanged(object sender, EventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.ValueChangesCalculation(sender));
		}

		private void tbDoses_KeyPress(object sender, KeyPressEventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.QuantityKeyPress(sender, e));
		}

		private void tbQtyDay_KeyPress(object sender, KeyPressEventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.QuantityKeyPress(sender,e));
		}

		private void tbCountDay_KeyPress(object sender, KeyPressEventArgs e)
		{
			ExecuteWithWait(() => _prescriptionCheckPresenter.NumberKeyPress(sender,e));
		}
		#endregion
	}
}
