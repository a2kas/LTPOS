using POS_display.Properties;
using POS_display.WR_KVAP;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace POS_display.Helpers
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
            InitializeComboBox();
            tbCommand.Text = "1.02,0,0,0";
        }

        private void InitializeComboBox()
        {
            cbCommand.Items.Clear();
            cbCommand.Items.Add("177");
            cbCommand.SelectedIndex = 0;
        }

        private void btnGetSPSTPLDetails_Click(object sender, EventArgs e)
        {
            rtbLogs.Text = "";

            try
            {
                var res = Session.KVAP.GetSPSTPLDetails(tbSPSTPL.Text);
                if (res is SPSTPL_DETAILSROW)
                {
                    var SPSTPLDetailsRow = (res as SPSTPL_DETAILSROW);
                    string txt = $"SPAUDO_NUMERIS: {SPSTPLDetailsRow.SPAUDO_NUMERIS}\n";
                    txt += $"PAVARDE: {SPSTPLDetailsRow.PAVARDE}\n";
                    txt += $"VARDAS: {SPSTPLDetailsRow.VARDAS}\n";
                    txt += $"SVEIDRA_ID: {SPSTPLDetailsRow.SVEIDRA_ID}\n";
                    txt += $"JAR_KODAS: {SPSTPLDetailsRow.JAR_KODAS}\n";
                    txt += $"ASPI_PAVADINIMAS: {SPSTPLDetailsRow.ASPI_PAVADINIMAS}\n";

                    rtbLogs.Text = txt;
                }
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if ((string)cbCommand.SelectedItem == "177")
            {
                var args = tbCommand.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length != 4) 
                {
                    helpers.alert(Enumerator.alert.error, "Neteisingai nurodyti komandos argumentai!");
                } 
                else 
                {
                    decimal recipetSum = decimal.Parse(args[0], CultureInfo.InvariantCulture);
                    decimal cashSum = decimal.Parse(args[1], CultureInfo.InvariantCulture);
                    decimal creditSum = decimal.Parse(args[2], CultureInfo.InvariantCulture);
                    bool creditLast = decimal.Parse(args[3], CultureInfo.InvariantCulture) == 1m ? true : false;
                    var response = await Session.FP550.CashPaymentRoundingSimulation(recipetSum, cashSum, creditSum, creditLast);
                    rtbResponses.Text += DateTime.Now +": "+response.ToString() + "\n";
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbMsgToDecrypt.Text))
                rtbDecryptedMsg.Text = "";
            else
                rtbDecryptedMsg.Text = helpers.Decrypt(rtbMsgToDecrypt.Text, Settings.Default.SecretKey);
        }

        private void btnEcrypt_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbMsgToEncrypt.Text))
                rtbMsgToEncrypt.Text = "";
            else
                rtbEncryptedMsg.Text = helpers.Encrypt(rtbMsgToEncrypt.Text, Settings.Default.SecretKey);
        }
    }
}
