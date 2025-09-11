using System.Windows.Forms;

namespace POS_display.Helpers
{
    public partial class FormWait : Form
    {
        private bool _allowClose = false;

        public FormWait()
        {
            InitializeComponent();
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Text = "Prašome palaukti..";
            UseWaitCursor = true;
            Cursor = Cursors.WaitCursor;
            loaderUserControl.SpinnerLength = 60;
            loaderUserControl.IsLoading = true;
            StartPosition = FormStartPosition.CenterParent;
            TopMost = true;
        }

        public string Notification
        {
            get { return lblNotification.Text; }
            set { lblNotification.Text = value; }
        }

        public void Start()
        {
            _allowClose = false;
            Show();
            BringToFront();
        }

        public void Stop()
        {
            _allowClose = true;
            if (!IsDisposed)
            {
                Close();
            }
        }

        private void FormWait_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_allowClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }
    }
}
