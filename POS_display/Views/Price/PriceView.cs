using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POS_display.Models;
using POS_display.Presenters;

using POS_display.Views.Price;
using POS_display.Presenters.Price;
using POS_display.Repository.Price;
using POS_display.Repository.Pos;
using POS_display.Models.Price;

namespace POS_display
{
    public partial class PriceView : FormBase, IPriceView
    {
        #region Members
        private decimal _posdId = 0;
        private readonly PricePresenter _pricePresenter;
        #endregion

        #region Properties
        public TextBox Price
        {
            get { return tbPrice; }
            set { tbPrice = value; }
        }
        public Button CalcButton
        {
            get { return btnCalc; }
            set { btnCalc = value; }
        }
        #endregion

        #region Constructor
        public PriceView(decimal posdId)
        {
            _posdId = posdId;
            InitializeComponent();
            _pricePresenter = new PricePresenter(this, new PriceRepository());
            tbPrice.Select();
        }
        #endregion

        #region Actions
        private async void PriceView_Load(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                await _pricePresenter.UpdateSession("Kainos keitimas", 2);
            });
        }

        private void PriceView_Closing(object sender, FormClosingEventArgs e)
        {
            if (IsBusy)
                e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () =>
            {
                bool result = await _pricePresenter.ChangePosDPrice(new PosDPrice() { PosdId = _posdId, Price = Price.Text.ToDecimal() });
                if (result)
                    DialogResult = DialogResult.OK;
                else
                    helpers.alert(Enumerator.alert.warning, "Nepavyko pakeisti kainos!");
            });
        }

        private void tbPrice_TextChanged(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                _pricePresenter.EnableButtons();
            });
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }
        #endregion
    }
}
