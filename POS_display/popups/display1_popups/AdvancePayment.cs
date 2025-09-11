using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace POS_display
{
    public partial class AdvancePayment : FormBase
    {
        private decimal poshId = 0;

        #region Callbacks
        #endregion
        public AdvancePayment(decimal id)
        {
            poshId = id;
            InitializeComponent();
        }

        private void AdvancePayment_Load(object sender, EventArgs e)
        {
            ExecuteWithWait(() =>
            {
                //hardcode fill AdvancePaymentType combobox
                Dictionary<String, String> comboSource = new Dictionary<String, String>();
                comboSource.Add("ESHOP", "Elektroninės parduotuvės užsakymas");
                comboBox_AdvancePaymentType.DataSource = new BindingSource(comboSource, null);
                comboBox_AdvancePaymentType.DisplayMember = "Value";
                comboBox_AdvancePaymentType.SelectedIndex = 0;
                ExecuteAsyncAction(async () =>
                {
                    await DB.POS.UpdateSession("Priemokos dengimas ", 2);
                });
                ValidateButtons();
                tbOrderNumber.Select();
            });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            if (tbAdvanceSum.Text.ToDecimal() > 5000)
            {
                helpers.alert(Enumerator.alert.error, "Pasitikrinkite ar teisingai įvesta avanso suma.");
                return;
            }
            await ExecuteWithWaitAsync(async () =>
            {
                decimal advancePaymentId = await DB.POS.CreateAdvancePayment(poshId, ((KeyValuePair<string, string>)comboBox_AdvancePaymentType.SelectedItem).Key, tbOrderNumber.Text, tbAdvanceSum.Text.ToDecimal());
                if (advancePaymentId > 0)
                    this.DialogResult = DialogResult.OK;
                else
                    helpers.alert(Enumerator.alert.error, "Klaida atliekant avansinį mokėjimą");
            });


        }

        private void tbAdvanceSum_TextChanged(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void tbAdvanceSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private void ValidateButtons()
        {
            btnOK.Enabled = comboBox_AdvancePaymentType.SelectedItem.ToString() != String.Empty && tbAdvanceSum.Text.ToDecimal() > 0 && tbOrderNumber.Text != String.Empty;
        }

        private void tbOrderNumber_TextChanged(object sender, EventArgs e)
        {
            ValidateButtons();
        }
    }
}
