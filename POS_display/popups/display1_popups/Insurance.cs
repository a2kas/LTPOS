using POS_display.Helpers;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TamroUtilities.HL7.Models;

namespace POS_display.Popups.display1_popups
{
    public partial class Insurance : Form
    {
        private bool formWaiting = false;
        private Items.posh poshItem;
        private const int AdultAge = 18;

        public Insurance(Items.posh posh_item, string tab)
        {
            poshItem = posh_item;
            InitializeComponent();
            switch (tab)
            {
                case "EPS":
                    tabControl1.TabPages.Remove(tabGjensidige);
                    break;
                case "GJN":
                    tabControl1.TabPages.Remove(tabEPS);
                    break;
            }
        }

        private void Insurance_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Draudimas", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            cmbInsurance.DataSource = Session.Params.Where(x => x.system == "INSURANCE").ToList();
            cmbInsurance.DisplayMember = "value";
            cmbInsurance.ValueMember = "par";
        }

        private void Insurance_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnCalc_Click(object sender, EventArgs e)
        {
            if (formWaiting)
                return;
            if (tabControl1.SelectedTab == tabGjensidige && InsuranceSum != 0)
                DialogResult = DialogResult.OK;
            else if ((cmbInsurance.SelectedItem as Items.Params).par == "ERG" && Identity.Length != 11)
            {
                helpers.alert(Enumerator.alert.warning, "Taikant ERGO draudimą privaloma nurodyti pilną asmens kodą!");
                return;
            }
            else if ((cmbInsurance.SelectedItem as Items.Params).par == "SAM") 
            {
                if (Session.getParam("SAM", "ON") == "0")
                {
                    helpers.alert(Enumerator.alert.warning, "Ukrainos piliečių draudimo galimybė išjungta!");
                    return;
                }
                
                if (!IsValidESINumber(Identity))
                {
                    helpers.alert(Enumerator.alert.warning, "Neteisingai įvestas ESI numerio formatas. Privalo būti 'patient-XXXXXX-XXXXX'");
                    return;
                }

                ESIIdentityDto eSIIdentityDto = await Session.eRecipeUtils.CheckESI<ESIIdentityDto>(Identity);
                if (eSIIdentityDto == null || eSIIdentityDto.Exists == 0)
                {
                    helpers.alert(Enumerator.alert.warning, "Neregistruotas. Asmeniui nepriklauso kompensacija");
                    return;
                }
                
                var patientDto = await Session.eRecipeUtils.GetPatient<PatientDto>(Identity, string.Empty);
                if (string.IsNullOrEmpty(patientDto?.PersonalCode))
                {
                    DialogResult = DialogResult.OK;
                    return;
                }

                var hasLowIncomeResponse = await Session.eRecipeUtils.GetLowIncome<LowIncomeDto>(patientDto?.PersonalCode);
                if (hasLowIncomeResponse == null || (hasLowIncomeResponse.PatientInsured && hasLowIncomeResponse.PatientAge > AdultAge))
                {
                    helpers.alert(Enumerator.alert.warning, "Asmuo draustas PSD, taikyti kompensacijos negalima!");
                    return;
                }
                
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.OK;
        }
        private bool IsValidESINumber(string esi)
        {
            if (esi.Length != 20) return false;
            return new Regex("([patient])-([0-9]{6})-([0-9]{5})").IsMatch(esi);
        }

        private void tbInsuranceSum_TextChanged(object sender, EventArgs e)
        {
            if (InsuranceSum > 0 && InsuranceSum <= OthersSum)
                btnCalc.Enabled = true;
            else
                btnCalc.Enabled = false;
        }

        private void tbCardNo_TextChanged(object sender, EventArgs e)
        {
            bool enabled = true;
            if (Identity == "")
                enabled = false;
            if (InsuranceType != "ERG" && InsuranceType != "SAM" && CardNo == "")
                enabled = false;
            btnCalc.Enabled = enabled;
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            tbInsuranceSum.Enabled = true;
            tbInsuranceSum.Select();
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ProcessStartInfo sInfo = new ProcessStartInfo(@"https://sales.gjensidige.lt/");
                    Process.Start(sInfo);
                }
            ));
            backgroundThread.Start();
        }

        private void cmbInsurance_SelectedValueChanged(object sender, EventArgs e)
        {
            CardNo = "";
            Identity = "";
            tbCardNo.Enabled = true;
            tbCardNo.Focus();
            if ((cmbInsurance.SelectedItem as Items.Params).par == "SAM")
            {
                tbCardNo.Enabled = false;
                Identity = "patient-";
                tbIdentity.SelectionStart = 8;
                tbIdentity.SelectionLength = 0;
                tbIdentity.Focus();
            }
        }

        private void tb_keyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (InsuranceType == "IF" && CardNo.Contains(" "))
                {
                    Identity = CardNo.Substring(0, CardNo.IndexOf(' '));
                    CardNo = CardNo.Substring(CardNo.IndexOf(' ') + 1);
                }
                tbCardNo_TextChanged(sender, new EventArgs());
                if (btnCalc.Enabled == true)
                    btnCalc_Click(sender, new EventArgs());
                else
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                    SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
        }

        #region Variables
        public string InsuranceType
        {
            get
            {
                return cmbInsurance.SelectedValue.ToString();
            }
            set
            {
                cmbInsurance.SelectedValue = value;
            }
        }

        public string CardNo
        {
            get
            {
                return tbCardNo.Text.Trim();
            }
            set
            {
                tbCardNo.Text = value;
            }
        }
        public string Identity
        {
            get
            {
                return tbIdentity.Text;
            }
            set
            {
                tbIdentity.Text = value;
            }
        }
        public decimal MedicinesSum
        {
            get
            {
                return tbMedicinesSum.Text.ToDecimal();
            }
            set
            {
                tbMedicinesSum.Text = value.ToString().Replace('.', ',');
            }
        }
        public decimal VitaminsSum
        {
            get
            {
                return tbVitaminsSum.Text.ToDecimal();
            }
            set
            {
                tbVitaminsSum.Text = value.ToString().Replace('.', ',');
            }
        }
        public decimal OthersSum
        {
            get
            {
                return tbOthersSum.Text.ToDecimal();
            }
            set
            {
                tbOthersSum.Text = value.ToString().Replace('.', ',');
            }
        }
        public decimal InsuranceSum
        {
            get
            {
                return tbInsuranceSum.Text.ToDecimal();
            }
            set
            {
                tbInsuranceSum.Text = value.ToString().Replace('.', ',');
            }
        }

        #endregion
    }
}
