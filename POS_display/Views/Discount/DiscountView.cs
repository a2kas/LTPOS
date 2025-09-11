using POS_display.Models.Discount;
using POS_display.Presenters.Discount;
using POS_display.Repository.Discount;
using POS_display.Repository.Loyalty;
using POS_display.Views;
using POS_display.Views.Discount;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace POS_display
{
    public partial class DiscountView : FormBase, IDiscountView, IBaseView
    {
        #region Members
        private readonly DiscountPresenter _discountPresenter;
        private decimal _poshId = 0;
        private decimal _posdId = 0;
        #endregion

        public DiscountView(decimal posh_id, decimal posd_id)
        {
            _poshId = posh_id;
            _posdId = posd_id;

            InitializeComponent();

            _discountPresenter = new DiscountPresenter(this, new DiscountRepository(), new LoyaltyRepository());
        }

        private async void DiscountView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                cbDiscountSum.Select();
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                await _discountPresenter.UpdateSession("Nuolaidų suteikimas  ", 2);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                await _discountPresenter.LoadDiscountCategories();
                await _discountPresenter.LoadDiscountTypes1();

            }, false);
        }

        #region Properties
        public IList<DiscountH> DiscountCategories
        {
            get { return (List<DiscountH>)this.cbDiscountCategory.DataSource; }
            set 
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = value;
                cbDiscountCategory.DataSource = bindingSource.DataSource;
                cbDiscountCategory.DisplayMember = "Name";
                cbDiscountCategory.ValueMember = "Perfix";
            }
        }
        public DiscountH SelectedDiscountCategory
        {
            get { return cbDiscountCategory.SelectedItem as DiscountH; }
            set { cbDiscountCategory.SelectedItem = value; }
        }
        public IList<DiscountType> DiscountTypes1
        {
            get { return (List<DiscountType>)this.cbDiscountType1.DataSource; }
            set
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = value;
                cbDiscountType1.DataSource = bindingSource.DataSource;
                cbDiscountType1.DisplayMember = "DiscType";
                cbDiscountType1.ValueMember = "Type";
            }
        }
        public IList<DiscountType> DiscountTypes2
        {
            get { return (List<DiscountType>)this.cbDiscountType2.DataSource; }
            set
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = value;
                cbDiscountType2.DataSource = bindingSource.DataSource;
                cbDiscountType2.DisplayMember = "DiscType";
                cbDiscountType2.ValueMember = "Type";
            }
        }
        public DiscountType SelectedDiscountType2
        {
            get { return cbDiscountType2.SelectedItem as DiscountType; }
            set { cbDiscountType2.SelectedItem = value; }
        }
        public DiscountType SelectedDiscountType1
        {
            get { return cbDiscountType1.SelectedItem as DiscountType; }
            set { cbDiscountType1.SelectedItem = value; }
        }
        public TextBox CardNoTextBox
        {
            get { return tbCardNo; }
            set { tbCardNo = value; }
        }
        public Button CalcButton
        {
            get { return btnCalc; }
            set { btnCalc = value; }
        }
        public ComboBox DiscountSum
        {
            get { return cbDiscountSum; }
            set { cbDiscountSum = value; }
        }
        #endregion

        #region Actions
        private void DiscountView_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy)
                e.Cancel = true;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private async void cbDiscountCategory_IndexChanged(object sender, EventArgs e)
        {            
            await ExecuteWithWaitAsync(async () =>
            {
                await _discountPresenter.LoadDiscountTypes2();
            },
            false);
        }
        private async void cbDiscountType2_IndexChanged(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async() =>
            {
                await _discountPresenter.LoadDiscounts();
            },
            false);
        }
        private async void btnCalc_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync( async() =>
            {
                bool result = await _discountPresenter.CalculateDiscount(_poshId, _posdId);
                if (result)
                    DialogResult = DialogResult.OK;
            });
        }
        private void tbCardNo_TextChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(()=>
            {
                _discountPresenter.EnableButtons(sender as TextBox);
            });
        }
        #endregion
    }
}
