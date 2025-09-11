using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_display.Popups
{
    public partial class wpf_dlg : FormBase
    {
        private readonly System.Windows.Controls.UserControl _wpf_uc;
        private bool _isClosable = true;

        public bool IsClosable 
        {
            get { return _isClosable; }
            set { _isClosable = value; }
        }

        public wpf_dlg(System.Windows.Controls.UserControl wpf_uc, string name = "", int width = -1, int height = -1)
        {
            InitializeComponent();
            this.Width = width > 0 ? width : (int)wpf_uc.Width;
            this.Height = height > 0 ? height : (int)wpf_uc.Height;
            this.Text = name;
            _wpf_uc = wpf_uc;
        }

        private void wpf_dlg_Load(object sender, EventArgs e)
        {
            var vm = _wpf_uc.DataContext as wpf.ViewModel.BaseViewModel;
            vm.CloseEventHandler += CloseDialogFromControl;
            elementHost1.Child = _wpf_uc;
        }

        public void CloseDialogFromControl(object sender, EventArgs e)
        {
            IsBusy = false;
            var closeResult = (DialogResult)(sender ?? DialogResult.Cancel);
            this.DialogResult = closeResult;
        }

        public void HideControlBox()
        {
            ControlBox = false;
        }

        public void SetNotResizeable()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void wpf_dlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_isClosable && (e.CloseReason == CloseReason.UserClosing);
        }
    }
}
