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
    public partial class Params : Form
    {
        private bool formWaiting = false;

        public Params(string system, string par)
        {
            InitializeComponent();
            SystemTxt = system;
            ParTxt = par;
        }

        private void Params_Load(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DB.POS.UpdateSession("Params", 2);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ValueTxt = Session.Params.FirstOrDefault(x => x.system == SystemTxt && x.par == ParTxt).value;
        }

        private void Params_Closing(object sender, FormClosingEventArgs e)
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
            await DB.Settings.updateParams(SystemTxt, ParTxt, ValueTxt);
            this.DialogResult = DialogResult.OK;
        }

        #region Variables
        private string SystemTxt
        {
            get
            {
                return tbSystem.Text;
            }
            set
            {
                tbSystem.Text = value;
            }
        }
        private string ParTxt
        {
            get
            {
                return tbPar.Text;
            }
            set
            {
                tbPar.Text = value;
            }
        }
        private string ValueTxt
        {
            get
            {
                return tbValue.Text;
            }
            set
            {
                tbValue.Text = value;
            }
        }
        #endregion
    }
}
