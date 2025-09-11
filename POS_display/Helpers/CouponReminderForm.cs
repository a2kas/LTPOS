using System;
using System.Drawing;
using System.Windows.Forms;

namespace POS_display.Helpers
{
    public partial class CouponReminderForm : Form
    {
        private Label messageLabel;
        private LinkLabel linkLabel;
        private Button okButton;

        public CouponReminderForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Priminimas";
            Size = new Size(400, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;

            messageLabel = new Label();
            messageLabel.Text = "Nepamirškite panaudoti kuponą:";
            messageLabel.Location = new Point(20, 30);
            messageLabel.Size = new Size(350, 20);
            messageLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

            linkLabel = new LinkLabel();
            linkLabel.Text = "https://partner.geradovana.lt/login";
            linkLabel.Location = new Point(20, 60);
            linkLabel.Size = new Size(350, 20);
            linkLabel.Font = new Font("Microsoft Sans Serif", 9F);
            linkLabel.LinkClicked += LinkLabel_LinkClicked;

            okButton = new Button();
            okButton.Text = "Supratau";
            okButton.Location = new Point(150, 110);
            okButton.Size = new Size(100, 30);
            okButton.DialogResult = DialogResult.OK;
            okButton.UseVisualStyleBackColor = true;

            Controls.Add(messageLabel);
            Controls.Add(linkLabel);
            Controls.Add(okButton);

            AcceptButton = okButton;
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = linkLabel.Text,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepavyko atidaryti nuorodos: {ex.Message}", "Klaida",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
