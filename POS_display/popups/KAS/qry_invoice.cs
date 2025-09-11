using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public partial class qry_invoice : Form
    {
        private bool formWaiting = false;
        private Items.KAS.posh posh_data;

        public qry_invoice(Items.KAS.posh posh_item)
        {
            posh_data = posh_item;
            InitializeComponent();
        }

        private void qry_invoice_Load(object sender, EventArgs e)
        {
            //form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Sąskaita ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            CheckNo = posh_data.checkno;
            CheckDate = posh_data.documentdate.Date.ToString("yyyy.MM.dd");
            DocumentDate = DateTime.Now.Date.ToString("yyyy.MM.dd");
            DataTable dt_BENUM = DB.KAS.getBENUMtransaction(posh_data.id, 7419);
            if (dt_BENUM.Rows.Count > 0)
            {
                DataTable dt_partners = DB.partners.getPartnerByScala(dt_BENUM.Rows[0]["buyer"].ToString());
                if (dt_partners.Rows.Count > 0)
                {
                    tbDebtorEcode.ReadOnly = true;
                    btnSelDebtor.Enabled = false;
                    tbDebtorEcode.Text = dt_partners.Rows[0]["ecode"].ToString();
                    tbDebtorName.Text = dt_partners.Rows[0]["name"].ToString();
                    creditorId = dt_partners.Rows[0]["id"].ToDecimal();
                }
            }
            checkValues();
        }

        private void qry_invoice_Closing(object sender, FormClosingEventArgs e)
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
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            if (DocumentNo == "" || creditorId == 0)
                helpers.alert(Enumerator.alert.error, "Neįvesti duomenys!");
            else
            {
                DataTable sfh = DB.KAS.checkSFH(DocumentNo);
                if (sfh.Rows.Count > 0)
                    helpers.alert(Enumerator.alert.error, "Toks sąskaitos faktūros nr. jau egzistuoja!");
                else
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
        
        private void btnSelPartner_Click(object sender, EventArgs e)
        {
            partners dlg = new partners();
            dlg.Location = helpers.middleScreen(this, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                tbDebtorEcode.Text = dlg.debtorEcode;
                tbDebtorName.Text = dlg.debtorName;
                creditorId = dlg.debtorId.ToDecimal();
                checkValues();
            }
            tbDocumentNo.Select();
            dlg.Dispose();
            dlg = null;
        }

        private void tbDebtorEcode_Leave(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            DataTable dt_partners = DB.partners.getPartner(tbDebtorEcode.Text);
            if (dt_partners.Rows.Count > 0)
            {
                tbDebtorEcode.Text = dt_partners.Rows[0]["ecode"].ToString();
                tbDebtorName.Text = dt_partners.Rows[0]["name"].ToString();
                creditorId = dt_partners.Rows[0]["id"].ToDecimal();
            }
            else
                btnSelPartner_Click(new object(), new EventArgs());
            checkValues();
        }

        private void tbDocumentNo_TextChanged(object sender, EventArgs e)
        {
            checkValues();
        }

        private void tbDocumentDate_TextChanged(object sender, EventArgs e)
        {
            checkValues();
        }

        private void checkValues()
        {
            if (DocumentNo.Replace('.', ',').ToDecimal() > 0 && !tbDebtorEcode.Text.Equals(""))
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        #region Variables
        public decimal creditorId { get; set; }

        public string CheckNo
        {
            get { return tbCheckNo.Text; }
            set { tbCheckNo.Text = value; }
        }

        public string CheckDate
        {
            get { return tbCheckDate.Text; }
            set { tbCheckDate.Text = value; }
        }

        public string DocumentDate
        {
            get { return tbDocumentDate.Text; }
            set { tbDocumentDate.Text = value; }
        }

        public string DocumentNo
        {
            get { return tbDocumentNo.Text; }
            set { tbDocumentNo.Text = value; }
        }
        #endregion
    }
}
