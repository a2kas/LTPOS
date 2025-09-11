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
    public partial class RecipeSettings : Form
    {
        private bool formWaiting = false;

        #region Callbacks

        private void dbRecipeParams_cb(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (success && t.Rows.Count > 0)
                {
                    try
                    {
                        form_wait(false);
                        AccountId = t.Rows[0]["sales_accountid"].ToDecimal();
                        tbAccountCode.Text = t.Rows[0]["account_code"].ToString();
                        tbAccountName.Text = t.Rows[0]["account_name"].ToString();
                        OffsetAccountId = t.Rows[0]["offset_accountid"].ToDecimal();
                        tbOffsetAccountCode.Text = t.Rows[0]["offset_account_code"].ToString();
                        tbOffsetAccountName.Text = t.Rows[0]["offset_account_name"].ToString();
                        MyOS = t.Rows[0]["my_os"].ToString();
                        MyEmail = t.Rows[0]["my_email"].ToString();
                        tbMyServer.Text = t.Rows[0]["my_server"].ToString();
                        MyProtocol = t.Rows[0]["my_protocol"].ToString();
                        MyLogin = t.Rows[0]["my_login"].ToString();
                        MyPassword = t.Rows[0]["my_pasword"].ToString();
                        TLKEmail = t.Rows[0]["tlk_email"].ToString();
                        tbTLKID.Text = t.Rows[0]["tlk_id"].ToString();
                        cmbCommitFromPos.SelectedValue = int.Parse(t.Rows[0]["commit_from_pos"].ToString());
                        cmbPrintOnSave.SelectedValue = int.Parse(t.Rows[0]["print_on_save"].ToString());
                        cmbCheck.SelectedValue = int.Parse(t.Rows[0]["check_"].ToString());
                        tbTimeout.Text = t.Rows[0]["timeout"].ToString();
                        cmbSend.SelectedValue = int.Parse(t.Rows[0]["send_to_tlk"].ToString());
                    }
                    catch (Exception e)
                    {
                        helpers.alert(Enumerator.alert.error, e.Message);
                    }
                }
            }));
        }
        #endregion

        public RecipeSettings()
        {
            InitializeComponent();
        }

        private void RecipeSettings_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Receptų nustatymai", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            tbAccountCode.Select();
            form_wait(true);
            Dictionary<int, string> test = new Dictionary<int, string>();
            test.Add(1, "Taip");
            test.Add(0, "Ne");

            cmbCommitFromPos.DataSource = new BindingSource(test, null);
            cmbCommitFromPos.DisplayMember = "Value";
            cmbCommitFromPos.ValueMember = "Key";

            cmbPrintOnSave.DataSource = new BindingSource(test, null);
            cmbPrintOnSave.DisplayMember = "Value";
            cmbPrintOnSave.ValueMember = "Key";
            
            cmbCheck.DataSource = new BindingSource(test, null);
            cmbCheck.DisplayMember = "Value";
            cmbCheck.ValueMember = "Key";

            test = new Dictionary<int, string>();
            test.Add(0, "Nesiųsti");
            //test.Add(1, "Siųsti visus receptus");//RFC-229
            test.Add(2, "Siųsti tik popierinius receptus");
            //test.Add(3, "Siųsti tik elektroninius receptus");//RFC-229

            cmbSend.DataSource = new BindingSource(test, null);
            cmbSend.DisplayMember = "Value";
            cmbSend.ValueMember = "Key";
            DB.Settings.asyncRecipeParams(dbRecipeParams_cb);
        }

        private void RecipeSettings_Closing(object sender, FormClosingEventArgs e)
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
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            bool success = await DB.Settings.asyncUpdateRecipeParams(AccountId, OffsetAccountId, MyOS, MyEmail, tbMyServer.Text, MyProtocol, MyLogin, MyPassword, TLKEmail, tbTLKID.Text,
                ((KeyValuePair<int, string>)cmbCommitFromPos.SelectedItem).Key,
                ((KeyValuePair<int, string>)cmbPrintOnSave.SelectedItem).Key,
                ((KeyValuePair<int, string>)cmbCheck.SelectedItem).Key,
                int.Parse(tbTimeout.Text),
                ((KeyValuePair<int, string>)cmbSend.SelectedItem).Key);
                if (success)
                    this.DialogResult = DialogResult.OK;
        }
        
        private void btnSelAccount_Click(object sender, EventArgs e)
        {
            ledger dlg = new ledger();
            dlg.Location = helpers.middleScreen(this, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                tbAccountCode.Text = dlg.ledgerCode;
                tbAccountName.Text = dlg.ledgerName;
                AccountId = dlg.ledgerId;
            }
            this.DialogResult = new DialogResult();
            dlg.Dispose();
            dlg = null;
        }

        private void btnSeltbOffsetAccount_Click(object sender, EventArgs e)
        {
            ledger dlg = new ledger();
            dlg.Location = helpers.middleScreen(this, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                tbOffsetAccountCode.Text = dlg.ledgerCode;
                tbOffsetAccountName.Text = dlg.ledgerName;
                OffsetAccountId = dlg.ledgerId;
            }
            this.DialogResult = new DialogResult();
            dlg.Dispose();
            dlg = null;
        }

        private decimal AccountId { get; set; }
        private decimal OffsetAccountId { get; set; }
        private string MyOS { get; set; }
        private string MyEmail { get; set; }
        private string MyProtocol { get; set; }
        private string MyLogin { get; set; }
        private string MyPassword { get; set; }
        private string TLKEmail { get; set; }
    }
}
