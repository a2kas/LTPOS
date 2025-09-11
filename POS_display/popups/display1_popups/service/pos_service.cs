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
    public partial class pos_service : Form
    {
        private bool formWaiting = false;
        private string serviceId = "";
        private decimal poshId = 0;

        public pos_service(decimal posh_id)
        {
            InitializeComponent();
            poshId = posh_id;
        }

        private void pos_service_Load(object sender, EventArgs e)
        {
            tbSum.Select();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Paslaugų pardavimas  ", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void pos_service_Closing(object sender, FormClosingEventArgs e)
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
            decimal result = await DB.POS.create_posd_service(poshId, serviceId, 2, tbSum.Text.Replace('.', ',').ToDecimal());
            form_wait(false);
            if (result > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                helpers.alert(Enumerator.alert.warning, "Nepavyko pridėti paslaugos!");
            }
            form_wait(false);
        }

        private void tbSum_TextChanged(object sender, EventArgs e)
        {
            if (/*tbSum.Text.ToDecimal() > 0 &&*/ !tbService.Text.Equals(""))
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }

        private void tbSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            helpers.tb_KeyPress(sender, e);
        }

        private void btnSelService_Click(object sender, EventArgs e)
        {
            service dlg = new service();
            dlg.Location = helpers.middleScreen(this, dlg);
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK)
            {
                tbService.Text = dlg.serviceName;
                serviceId = dlg.serviceId;
                if (tbSum.Text.ToDecimal() > 0 && !tbService.Text.Equals(""))
                    btnSave.Enabled = true;
            }
            tbSum.Select();
            dlg.Dispose();
            dlg = null;
        }
    }
}
