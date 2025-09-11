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
    public partial class WebPage : Form
    {
        private bool formWaiting = false;

        public WebPage(string url)
        {
            form_wait(true);
            InitializeComponent();
            webBrowserPOS.ScriptErrorsSuppressed = false;
            webBrowserPOS.AllowWebBrowserDrop = false;
            webBrowserPOS.IsWebBrowserContextMenuEnabled = false;
            webBrowserPOS.WebBrowserShortcutsEnabled = false;
            webBrowserPOS.ObjectForScripting = this;
            webBrowserPOS.Navigate(url);
            form_wait(false);
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

        public void webBrowserPOS_Closing(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
