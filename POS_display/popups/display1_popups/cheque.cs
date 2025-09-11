using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace POS_display
{
    public partial class cheque : Form
    {
        private bool formWaiting = false;
        private decimal posdId = 0;
        private DataTable mList = null;
        private string CheckedOption = "";
        private decimal amount = 0;
        private string cheque_code = "";
        private decimal qty = 0;
        private string from = "";

        #region Callbacks
        private void dbCheque(bool success, DataTable t)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (success)
                {
                    mList = t;
                    changeFixedSum();
                }
                form_wait(false);
            }));
        }

        #endregion

        public cheque(decimal ID)
        {
            posdId = ID;
            InitializeComponent();
        }

        private async void cheque_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Cekiai suteikimas  ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dictionary<string, string> test = new Dictionary<string, string>();
            test.Add("GLAXO", "Glaxo Smith...");

            ddfixedCheque.DataSource = new BindingSource(test, null);
            ddfixedCheque.DisplayMember = "Value";
            ddfixedCheque.ValueMember = "Key";

            decimal result = await DB.cheque.asyncChequeTrans(posdId);
            if (result > 0)
            {
                form_wait(false);
                helpers.alert(Enumerator.alert.warning, "GSK čekis jau suteiktas!");
                this.Close();
            }
            else
                DB.cheque.asyncCheque(posdId, dbCheque);
            tbCardNo.SelectionStart = tbCardNo.Text.Length;
            tbCardNo.Select();
        }

        private void cheque_Closing(object sender, FormClosingEventArgs e)
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

        private void changeFixedSum()
        {
            if (mList != null)
            {
                from = ((KeyValuePair<string, string>)ddfixedCheque.SelectedItem).Key;
                for (int i = 0; i < mList.Rows.Count; i++)
                {
                    if (mList.Rows[i]["cheque_from"].ToString().Equals(from))
                    {
                        decimal.TryParse(mList.Rows[i]["cheq"].ToString(), out amount);
                        decimal.TryParse(mList.Rows[i]["qty"].ToString(), out qty);
                        cheque_code = mList.Rows[i]["cheque_code"].ToString();
                    }
                }
                tbChequeFixedAmount.Text = amount.ToString();
                tbChequeQty.Text = qty.ToString();
                tbChequeCode.Text = cheque_code;
            }
        }

        private async void setChequeFixed()
        {
            if (tbCardNo.Text.Length > 0)
            {
                if (tbCardNo.Text.Length != 19 || !validatecard(tbCardNo.Text))
                {
                    helpers.alert(Enumerator.alert.warning, "Blogas kortelės numeris!");
                    tbCardNo.Select();
                }
                else
                {
                    if (await DB.cheque.CreateChequeTrans(posdId, amount, from, "APMOKEJIMAS CEKIU.", tbCardNo.Text, cheque_code, 0,""))
                        this.DialogResult = DialogResult.OK;
                    else
                        helpers.alert(Enumerator.alert.warning, "Nepavyko suteikti GSK čekio!");
                }
            }
        }

        private bool validatecard(string number)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string cardnumber = digitsOnly.Replace(number, "");
            int cardlength = cardnumber.Length;
            int parity = cardlength % 2;
            int sum = 0;
            for (int i = 0; i < cardlength; i++)
            {
                int digit = int.Parse(cardnumber[i].ToString());
                if (i % 2 == parity)
                    digit = digit * 2;
                if (digit > 9)
                    digit = digit - 9;
                sum = sum + digit;
            }
            if (sum % 10 == 0)
                return true;
            else
                return false;
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
            if (rbfixedCheque.Checked == true)
                CheckedOption = "FIXED";

            string dch = doCheck();
            if (dch != "" || dch == "-1")
            {
                if (dch.ToDecimal() != 0)
                {
                    helpers.alert(Enumerator.alert.warning, "Bloga kortelė!");
                    return;
                }
            }
            else
            {
                helpers.alert(Enumerator.alert.error, "Nėra ryšio su autorizavimo centru, pasitikrinkite interneto ryšį!");
                return;
            }

            changeFixedSum();
            switch(CheckedOption)
            {
                case "FIXED":
                    if (helpers.alert(Enumerator.alert.confirm, "Ar tvirtinti apmokėjimą f. čekiu?", true) && tbChequeFixedAmount.Text.ToDecimal() > 0)
                        setChequeFixed();
                    if (tbChequeFixedAmount.Text.ToDecimal() == 0)
                        helpers.alert(Enumerator.alert.warning, "Čekio suma 0!");
                    break;
            }
            this.DialogResult = DialogResult.OK;
        }

        private string doCheck()
        {
            var responseString = "";
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.22.0.6/websrv/discounts/disp.php");
                var postData = "system=gsk&subsystem=checkcard";
                postData += "&value=" + tbCardNo.Text;
                var data = Encoding.ASCII.GetBytes(postData);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                webRequest.ContentLength = data.Length;

                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)webRequest.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }

            return responseString;
        }

        private void tbCardNo_TextChanged(object sender, EventArgs e)
        {
            if (tbChequeFixedAmount.Text.ToDecimal() > 0 && tbChequeQty.Text.ToDecimal() > 0 && tbChequeCode.Text.ToDecimal() > 0 && tbCardNo.Text.ToDecimal() > 0)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        private void ddfixedCheque_Changed(object sender, EventArgs e)
        {
            changeFixedSum();
        }
        
    }
}
