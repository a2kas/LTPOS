using POS_display.Views;
using System.Windows.Forms;

namespace POS_display.Helpers
{
    public class UserControlBase: UserControl, IBusy
    {
        private bool _isBusy;
        public virtual bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                this.UseWaitCursor = value;
                this.Cursor = _isBusy ? Cursors.WaitCursor : Cursors.Default;
            }
        }
    }
}
