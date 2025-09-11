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
    public partial class SettingsDialog : Form
    {
        private bool formWaiting = false;

        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void SettingsDialog_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Nustatymai", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            cmbChoose.Select();
            cmbChoose.SelectedIndex = 0;
        }

        private void SettingsDialog_Closing(object sender, FormClosingEventArgs e)
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

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            Form dlg;
            switch (cmbChoose.SelectedItem.ToString())
            {
                case "Receptų":
                    dlg = new RecipeSettings();
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                        Session.RecipeParm = DB.POS.getRecipeParm<Items.RecipeParm>();
                    dlg.Dispose();
                    dlg = null;
                    break;
                /*case "Lojalumas":
                    dlg = new Params("CRM", "ENABLED");
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    //if (dlg.DialogResult == DialogResult.OK)
                    dlg.Dispose();
                    dlg = null;
                    break;*/
                case "Lojalumas":
                    dlg = new CRM();
                    dlg.Location = helpers.middleScreen(this, dlg);
                    dlg.ShowDialog();
                    if (dlg.DialogResult == DialogResult.OK)
                    {

                    }
                    dlg.Dispose();
                    dlg = null;
                    break;
            }
            form_wait(false);
        }
    }
}
