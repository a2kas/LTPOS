using POS_display.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public partial class BallanceDialog : Form
    {
        private bool formWaiting = false;

        public BallanceDialog(string in_cardno)
        {
            InitializeComponent();
            CardNo = in_cardno;
        }

        private void BallanceDialog_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Kortelės balansas ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            List<CardType> card_type = new List<CardType>();
            CardType ct = new CardType();
            ct.Value = "LOJALUMAS";
            ct.Display = "Lojalumo kortelė";
            card_type.Add(ct);
            
            foreach (var insurance in Session.Params.Where(x => x.system == "INSURANCE").ToList())
            {
                ct = new CardType();
                ct.Value = insurance.par;
                ct.Display = insurance.value;
                card_type.Add(ct);
            }

            cmbCardType.DataSource = card_type;
            cmbCardType.ValueMember = "Value";
            cmbCardType.DisplayMember = "Display";
            tbCardNo.Select();
            if (CardNo != "")
                btnCheck.PerformClick();
        }

        private void BallanceDialog_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
        }

        private void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
                this.clear_form();
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
            }
        }

        private void clear_form()
        {
            Result = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            Result = "";
            btnUse.Enabled = false;
            try
            {
                if (selectedCard.Value == "LOJALUMAS")
                {
                    form_wait(true);
                    btnUse.Enabled = true;
                    Result += CardNo;
                    Result += "\nLojalumo taškai " + await Session.CRMRestUtils.GetCustomerPoints(CardNo);
                }
                else if (Session.Params.Where(x => x.system == "INSURANCE").Where(p => p.par == selectedCard.Value).Count() > 0)
                {
                    ProgressDialog dlg = new ProgressDialog("", "Įveskite draudimo kortelės identifikacinį kodą", this);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {
                        form_wait(true);
                        Items.Insurance insurance_item = new Items.Insurance(selectedCard.Value, CardNo + dlg.Result);
                        List<Items.ComboBox<decimal>> cardLimits = insurance_item.Utils.GetCardBalance(insurance_item);
                        CardNo2 = dlg.Result;
                        btnUse.Enabled = true;
                        foreach (var cardLimit in cardLimits)
                        {
                            Result += cardLimit.DisplayMember + "\nLikutis: " + cardLimit.ValueMember;
                            Result += "\n------------------------------------------------------------------";
                        }
                        if (Result == "")
                            Result = "Draudimas neteikia informacijos apie klientų likučius";
                    }
                    dlg.Dispose();
                    dlg = null;
                }
                else
                    throw new Exception("Kortelė neatpažinta.");
            }
            catch (TimeoutException ex)
            {
                helpers.alert(Enumerator.alert.error, "Serveris neatsako. Bandykite vėliau.");
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                helpers.alert(Enumerator.alert.error, "Serveris neatsako. Bandykite vėliau.");
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
            form_wait(false);
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            this.DialogResult = DialogResult.OK;
        }

        private void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCardNo.Focus();
            tbCardNo.SelectAll();
        }

        #region Variable
        public string CardNo
        {
            get { return tbCardNo.Text; }
            set { tbCardNo.Text = value; }
        }

        public string CardNo2 { get; set; }

        public string Result
        {
            get { return rtbResult.Text; }
            set 
            {
                rtbResult.Text = value;
            }
        }

        public CardType selectedCard 
        {
            get
            {
                return (CardType)cmbCardType.SelectedItem;
            }
            set
            {
                cmbCardType.SelectedItem = value;
            }
        }

        public class CardType
        {
            public string Display { get; set; }
            public string Value { get; set; }
        }
        #endregion
    }
    
}
