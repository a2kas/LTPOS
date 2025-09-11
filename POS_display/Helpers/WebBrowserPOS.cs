using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public delegate void ClosingEventHandler(object sender, EventArgs e);

    public class WebBrowserPOS : WebBrowser
    {
        // Define constants from winuser.h
        private const int WM_PARENTNOTIFY = 0x210;
        private const int WM_DESTROY = 2;

        public event ClosingEventHandler Closing;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PARENTNOTIFY:
                    if (!DesignMode)
                    {
                        if (m.WParam.ToInt32() == WM_DESTROY)
                        {
                            Closing(this, EventArgs.Empty);
                        }
                    }
                    DefWndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
