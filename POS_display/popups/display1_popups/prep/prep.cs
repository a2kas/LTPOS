using POS_display.SR_Privat;
using POS_display.Views.Partners;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace POS_display
{
    public partial class prep : Form
    {
        private bool formWaiting = false;
        private decimal creditorId = 0;
        private Items.posh _PoshItem = null;

        #region Callbacks
        #endregion
        public prep(Items.posh PoshItem)
        {
            InitializeComponent();
            _PoshItem = PoshItem;
        }

        private void prep_Load(object sender, EventArgs e)
        {
            form_wait(true);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Priemokos dengimas ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            tbDiscountSum.Select();
            form_wait(false);
        }

        private void prep_Closing(object sender, FormClosingEventArgs e)
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);

            foreach (var posd in _PoshItem.PosdItems)
            {
                bool success = await DB.POS.asyncCreatePrepTrans(posd.id, creditorId);

                if (!success)
                {
                    helpers.alert(Enumerator.alert.error, "Klaida suteikiant priemoką!");

                    break;
                }
            }

            form_wait(false);

            this.DialogResult = DialogResult.OK;
        }


        private void tbDiscountSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private void ddService_Changed(object sender, EventArgs e)
        {
            tbDiscountSum.Select();
        }

        private void btnSelPartner_Click(object sender, EventArgs e)
        {
            using (PartnersView dlg = new PartnersView()) 
            {
                dlg.Location = helpers.middleScreen(this, dlg);
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    tbDebtorEcode.Text = dlg.FocusedPartner.ECode;
                    tbDebtorName.Text = dlg.FocusedPartner.TCode;
                    creditorId = dlg.FocusedPartner.Id;
                    if (!tbDebtorEcode.Text.Equals(""))
                        btnSave.Enabled = true;
                }
                tbDiscountSum.Select();
            }
        }
    }
}
