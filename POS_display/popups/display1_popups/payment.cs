using Microsoft.Extensions.DependencyInjection;
using POS_display.Helpers;
using POS_display.Models;
using POS_display.Models.Pos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamroutilities.Client;

namespace POS_display
{
    public partial class PaymentView : FormBase, Views.Ipayment
    {
        private readonly Presenters.PaymentPresenter presenter;
        private bool _isAdvancePayment;
        private bool _isRoundingEnabled;
        private CashPaymentRoundingResponse _cashRoundingCalculation;
        private bool _inProgress = false;

        public PaymentView(Items.posh posh_item)
        {
            InitializeComponent();
            presenter = new Presenters.PaymentPresenter(this, Program.ServiceProvider.GetRequiredService<ITamroClient>());
            PoshItem = posh_item;
            _isAdvancePayment = PoshItem.PosdItems.Any(val => val.Type == "ADVANCEPAYMENT");
            _isRoundingEnabled = Session.IsRoundingEnabled;
        }

        #region Properties
        public Button CashButton
        {
            get { return btnCash; }
        }

        public Button CardButton
        {
            get { return btnCard; }
        }

        public bool IsRoundingEnabled
        {
            get { return _isRoundingEnabled; }
        }

        public CashPaymentRoundingResponse CashRoundingCalculation
        {
            get { return _cashRoundingCalculation; }
            set { _cashRoundingCalculation = value; }
        }
        #endregion

        private async void payment_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Atsiskaitymas", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            await presenter.RefreshGrid();
            BindGrid();
            await presenter.RecalculateLeftPayValues(_isRoundingEnabled);
            PaySum = PoshItem.TotalSum;
            gvPayment.Select();
            gvPayment.CurrentCell = gvPayment[gvPayment.Columns["amount"].Index, 0];
            await presenter.CheckChequePaper();
            btnPay.Enabled = true;
        }

        private bool _IsBusy;
        internal override bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                if (_IsBusy == true)
                {
                    this.UseWaitCursor = true;
                    this.Cursor = Cursors.WaitCursor;
                    this.gvPayment.Cursor = Cursors.WaitCursor;
                    btnPay.Enabled = false;
                }
                else
                {
                    this.UseWaitCursor = false;
                    this.Cursor = Cursors.Default;
                    this.gvPayment.Cursor = Cursors.Hand;
                    btnPay.Enabled = true;
                }
            }
        }

        public void BindGrid()
        {
            if (payment_list?.Count() > 0)
            {
                GridView_order();
            }
        }

        private void GridView_order()
        {
            //show columns
            if (!_isRoundingEnabled)
            {
                gvPayment.Columns["name"].DisplayIndex = 0;
                gvPayment.Columns["amount"].DisplayIndex = 1;
                gvPayment.Columns["code"].DisplayIndex = 2;
                gvPayment.Columns["leftPay"].Visible = false;
            }
            else
            {
                gvPayment.Columns["name"].DisplayIndex = 0;
                gvPayment.Columns["leftPay"].DisplayIndex = 1;
                gvPayment.Columns["amount"].DisplayIndex = 2;
                gvPayment.Columns["code"].DisplayIndex = 3;
            }
            //hide columns
            gvPayment.Columns["rank"].Visible = false;
            gvPayment.Columns["fiscal_rank"].Visible = false;
            gvPayment.Columns["id"].Visible = false;
            gvPayment.Columns["fiscal"].Visible = false;
            gvPayment.Columns["enabled"].Visible = false;
            gvPayment.Columns["code_required"].Visible = false;
            gvPayment.Columns["return_allowed"].Visible = false;
            gvPayment.Columns["read_only"].Visible = false;
            gvPayment.Columns["Buyer"].Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Program.Display2.UpdateData(PoshItem, false, 0);
            DialogResult = DialogResult.Cancel;
        }

        private void gvPayment_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var relativeMousePosition = dgv.PointToClient(Cursor.Position);
                dgvContextMenu.Show(dgv, relativeMousePosition);
            }
        }

        private async void gvPayment_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Models.payment payment = (Models.payment)gvPayment.Rows[e.RowIndex].DataBoundItem;
            if (e.ColumnIndex == gvPayment.Rows[e.RowIndex].Cells["amount"].ColumnIndex)
            {
                decimal sum_upto = payment_list.Where(pl => pl.fiscal_rank <= payment.fiscal_rank).Sum(pl => pl.amount);
                decimal sum_remain = payment_list.Where(pl => pl.fiscal_rank > payment.fiscal_rank).Sum(pl => pl.amount);
                if (sum_upto >= PaySum && sum_remain > 0)
                {
                    helpers.alert(Enumerator.alert.error, "Per didelė suma");
                    payment.amount = 0;
                }
                if (!payment.return_allowed && DebtorSum > PaySum)
                    payment.amount = PaySum - (DebtorSum - payment.amount);
                if (payment.amount < 0)
                    payment.amount = 0;
                if (payment.id == 10000109134)//lojalumo taškai
                {
                    IsBusy = true;
                    PoshItem.CRMItem.AcceptedPaymentResponse = await Session.CRMRestUtils.AcceptPayment(PoshItem, (float)payment.amount);
                    decimal payment_value = payment.amount;
                    if (payment.amount != (decimal)PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPoints)
                        payment.amount = (decimal)PoshItem.CRMItem.AcceptedPaymentResponse.Data.CreditPoints;
                    if (payment_value != payment.amount)
                        helpers.alert(Enumerator.alert.warning, "Lojalumo sistema leidžia panaudoti tik " + payment.amount + " lojalumo taškų");
                    IsBusy = false;
                }
            }
            if (e.ColumnIndex == gvPayment.Rows[e.RowIndex].Cells["code"].ColumnIndex)
            {                
                payment.amount = 0;
                if (payment.id == 4098)//Benu čekis
                {
                    IsBusy = true;
                    string[] cards = payment.code?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                    foreach (var card in helpers.GetDistinctList(cards.ToList())) 
                    {
                        var cardCode = card.Trim();
                        var presentCardValidationData = await presenter.ValidatePresentCard(cardCode);
                        if (string.IsNullOrWhiteSpace(presentCardValidationData.ValidationMessage))
                        {
                            payment.amount += Math.Round(presentCardValidationData.Amount, 2);
                            payment.ExternalIds.Add(presentCardValidationData.PresentCardId);
                        }
                        else
                        {
                            helpers.alert(Enumerator.alert.warning, presentCardValidationData.ValidationMessage);
                            payment.code = "";
                            payment.amount = 0;
                            payment.ExternalIds.Clear();
                        }
                    }
                    IsBusy = false;
                }
            }

            await presenter.RecalculateLeftPayValues(_isRoundingEnabled);
            gvPayment.Refresh();
            DebtorSum = payment_list.Sum(pl => pl.amount);          
        }

        private void gvPayment_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView gv = sender as DataGridView;
            gv.Focus();
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                int iColumn = gv.CurrentCell.ColumnIndex;
                int iRow = gv.CurrentCell.RowIndex;
                if (iColumn == gvPayment.Rows[iRow].Cells["code"].ColumnIndex)
                    gv.CurrentCell = gv[iColumn + 1, iRow];
                else if (iColumn == gvPayment.Rows[iRow].Cells["amount"].ColumnIndex)
                {
                    if (gv.Rows.Count == iRow + 1)
                        iRow = -1;
                    gv.CurrentCell = gv[iColumn - 1, iRow + 1];
                }
                else
                    gv.CurrentCell = gv[iColumn + 1, iRow];
                e.Handled = true;
            }
        }

        private void gvPayment_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Amount_KeyPress);
            e.Control.KeyUp -= new KeyEventHandler(Amount_KeyUp);
            if (gvPayment.CurrentCell.ColumnIndex == gvPayment.Columns["amount"].Index) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Amount_KeyPress);
                    tb.KeyUp += new KeyEventHandler(Amount_KeyUp);
                }
            }
        }

        private void Amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private async void Amount_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _inProgress = true;
                 TextBox tb = sender as TextBox;
                gvPayment.Rows[gvPayment.CurrentRow.Index].Cells[gvPayment.CurrentCell.ColumnIndex].Value = tb.Text.ToDecimal();
                await presenter.RecalculateLeftPayValues(_isRoundingEnabled);
                Amount_KeyUp_Event(sender, e);
            }
            finally
            {
                _inProgress = false;
            }
        }

        private void kopijuotiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gvPayment.CurrentRow != null)
            {
                var current_row = gvPayment.CurrentRow.Index;
                var current_column = gvPayment.CurrentCell.ColumnIndex;
                helpers.CopyRowCell(gvPayment, current_row, current_column);
            }
        }

        private async void btnPay_Click(object sender, EventArgs e)
        {
            if (_inProgress)
                return;

            if (presenter.IsGoodPresentPayment())
            {
                using (CouponReminderForm reminderForm = new CouponReminderForm())
                {
                    if (reminderForm.ShowDialog() != DialogResult.OK)
                        return;
                }
            }

            btnPay.Enabled = false;
            await ExecuteWithWaitAsync(async () =>
            {
                string tmp_rest_sum = edtRestSum.Text;
                await presenter.Pay();
                EnableControls(false);
                edtRestSum.Text = tmp_rest_sum;
                edtDebtorSum.Text = "";
                Program.Display1.IsBusy = true;
                await Program.Display1.RefreshPosh();
                await Task.Delay(TimeSpan.FromSeconds(3));
                DialogResult = DialogResult.OK;
            });
        }

        private async void btnCard_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => { await SelectFullAmount("C"); }, false);
            await ExecuteWithWaitAsync(async () => { await presenter.RecalculateLeftPayValues(_isRoundingEnabled); }, false);
        }

        private async void btnCash_Click(object sender, EventArgs e)
        {
            await ExecuteWithWaitAsync(async () => { await SelectFullAmount("P"); }, false);
            await ExecuteWithWaitAsync(async () => { await presenter.RecalculateLeftPayValues(_isRoundingEnabled); }, false);
        }

        private void gvPayment_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (_isAdvancePayment && HasAnotherWaySetAmount(e.RowIndex))
                e.Cancel = true;
        }

        private bool HasAnotherWaySetAmount(int currentRowIndex)
        {
            for (int i = 0; i < gvPayment.Rows.Count; i++)
            {
                if (currentRowIndex == i) continue;
                if ((gvPayment.Rows[i].DataBoundItem as payment)?.amount > 0)
                    return true;
            }
            return false;
        }

        private async Task SelectFullAmount(string type)
        {
            //TM:  || pl.id == 10000109134 dadedam, nes kitaip nusimuša lojalumo taškai
            decimal manual_sum = PaySum - payment_list.Where(pl => pl.read_only == true || pl.id == 10000109134).Sum(pl => pl.amount);
            foreach (var p in payment_list.Where(w => w.fiscal != type && w.read_only == false && w.id != 10000109134))
            {
                if (p.code_required)
                    p.code = "";
                p.amount = 0;
            }

            if (_isRoundingEnabled && type == "P")
            {
                _cashRoundingCalculation = await Session.FP550.CashPaymentRoundingSimulation(PaySum, manual_sum, PaySum - manual_sum, type != "P");
                if (_cashRoundingCalculation.ResponseStatus == Enumerator.PosResponseStatus.OK)
                    manual_sum += _cashRoundingCalculation.RoundingValue;
            }

            payment_list.FirstOrDefault(f => f.fiscal == type).amount = manual_sum;
            gvPayment.Refresh();
            DebtorSum = payment_list.Sum(pl => pl.amount);
        }

        private void RecalcRestSum()
        {
            decimal chsum = payment_list.Where(pl => pl.fiscal == "I").Sum(pl => pl.amount);
            decimal cashSum = payment_list.Where(pl => pl.fiscal == "P").Sum(pl => pl.amount);

            if (chsum >= PaySum)
                RestSum = 0;
            else
                RestSum = _isRoundingEnabled && !Session.WoltMode && cashSum > 0 ?
                    ((PaySum - DebtorSum) + _cashRoundingCalculation.RoundingValue) * (-1) : 
                    (PaySum - DebtorSum) * (-1);

            btnPay.Enabled = chsum > 0 ? RestSum >= 0 : RestSum >= 0 && edtDebtorSum.Text.ToDecimal() >= 0;

            if (PoshItem.PosdItems != null && PoshItem.PosdItems.Count == 0)
                btnPay.Enabled = false;

            foreach (DataGridViewRow dr in gvPayment.Rows)
            {
                var payment = (Models.payment)dr.DataBoundItem;
                dr.Cells["code"].ReadOnly = !payment.code_required;
                dr.Cells["amount"].ReadOnly = payment.read_only;
                if (payment.code_required)
                    dr.Cells["amount"].ReadOnly = string.IsNullOrWhiteSpace(payment.code);
                if (payment.read_only == false)
                {
                    if (payment.amount == 0 && RestSum >= 0)
                    {
                        dr.Cells["code"].ReadOnly = true;
                        dr.Cells["amount"].ReadOnly = true;
                    }
                }
            }

            if (PoshItem.IsEOrder)
            {
                if (gvPayment.Rows.Cast<DataGridViewRow>().Any(e => e.Cells["amount"].Value.ToDecimal() != 0m))
                {
                    foreach (DataGridViewRow dr in gvPayment.Rows)
                    {
                        var payment = (Models.payment)dr.DataBoundItem;
                        if (!payment.read_only)
                        {
                            dr.Cells["amount"].ReadOnly = dr.Cells["amount"].Value.ToDecimal() == 0;
                        }
                    }
                }
            }
        }
        private void payment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnPay.Select();
                btnCard_Click(sender, new EventArgs());
            }
            if (e.KeyCode == Keys.F3)
            {
                btnPay.Select();
                btnCash_Click(sender, new EventArgs());
            }
            if ((e.KeyCode == Keys.F4 || e.KeyCode == Keys.K) && btnPay.Enabled == true)
            {
                btnPay.Select();
                btnPay_Click(sender, new EventArgs());
            }
        }
        private void gvPayment_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private async void PaymentView_Shown(object sender, EventArgs e)
        {
            if (Session.WoltMode)
            {
                await ExecuteWithWaitAsync(async () => { await SelectFullAmount("L"); });
            }
        }

        private void EnableControls(bool apply)
        {
            btnPay.Enabled = apply;
            btnCash.Enabled = apply;
            btnCard.Enabled = apply;
            btnClose.Enabled = apply;
            gvPayment.ReadOnly = !apply;
        }

        public async Task VoidPayment()
        {
            await presenter.VoidPayment();
        }

        #region InterfaceParameters
        public DataGridView PaymentViewGrid
        {
            get { return gvPayment; }
        }
        private List<Models.payment> _payment_list;
        public List<Models.payment> payment_list
        {
            get
            {
                if (_payment_list == null)
                    _payment_list = new List<Models.payment>();
                _payment_list = (List<Models.payment>)gvPayment.DataSource;
                return _payment_list;
            }

            set
            {
                _payment_list = value;
                gvPayment.DataSource = _payment_list;
                DebtorSum = payment_list.Sum(pl => pl.amount);
            }
        }
        private Items.posh _PoshItem;
        public Items.posh PoshItem
        {
            get
            {
                return _PoshItem;
            }

            set
            {
                _PoshItem = value;
            }
        }
        public decimal PaySum
        {
            get
            {
                return edtPaySum.Text.ToDecimal();
            }
            set
            {
                edtPaySum.Text = value.ToString().Replace('.', ',');
                RecalcRestSum();
            }
        }
        public decimal DebtorSum
        {
            get
            {
                //return helpers.getDecimal(edtDebtorSum.Text);
                return payment_list.Sum(pl => pl.amount);
            }
            set
            {
                edtDebtorSum.Text = value.ToString().Replace('.', ',');
                RecalcRestSum();
            }
        }
        public decimal RestSum
        {
            get
            {
                return edtRestSum.Text.ToDecimal();
            }
            set
            {
                edtRestSum.Text = helpers.doFormat(Math.Round(value, 2).ToString());
                Program.Display1.RestSum = edtRestSum.Text;
            }
        }

        private string _lastCheckNo;

        public event EventHandler<KeyEventArgs> Amount_KeyUp_Event;

        public string lastCheckNo
        {
            get
            {
                return _lastCheckNo;
            }

            set
            {
                _lastCheckNo = value;
            }
        }
        public string Info 
        {
            get { return lblInfo.Text; }
            set { lblInfo.Text = value; }
        }
        #endregion
    }
}
