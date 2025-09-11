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
    public partial class CRM : Form
    {
        private bool formWaiting = false;

        public CRM()
        {
            InitializeComponent();
        }

        private void CRM_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("CRM", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            rbFalse.Checked = !Session.CRM;
            rbTrue.Checked = Session.CRM;
        }

        private void CRM_Closing(object sender, FormClosingEventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            Session.CRM = rbTrue.Checked;
            this.DialogResult = DialogResult.OK;
        }
    }
}
