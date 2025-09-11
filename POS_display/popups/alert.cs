using POS_display.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POS_display
{
    public partial class alert : Form
    {
        public alert(Enumerator.alert alert_type, string msg, string source_txt, bool confirm = false)
        {
            InitializeComponent();
            lblInfo.Text = msg;
            this.Text = source_txt;
            switch (alert_type)
            {
                case Enumerator.alert.error:
                    pictureBox1.Image = SystemIcons.Error.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case Enumerator.alert.info:
                    pictureBox1.Image = SystemIcons.Information.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case Enumerator.alert.warning:
                    pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case Enumerator.alert.display2:
                    this.Text = this.Text.Replace(Assembly.GetEntryAssembly().GetName().Name, Assembly.GetEntryAssembly().GetName().Name + " 2 ekranas - ");
                    pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case Enumerator.alert.confirm:
                    pictureBox1.Image = SystemIcons.Question.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case Enumerator.alert.donations:
                    pictureBox1.Image = Resources.donations.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                default:
                    pictureBox1.Image = SystemIcons.Warning.ToBitmap();
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
            }
            if (!confirm)
            {
                btnOK.Text = "Gerai";
                btnNo.Visible = false;
                tableLayoutPanel2.ColumnStyles[2].Width = 0;
            }
            //set window size
            int tmp = lblInfo.Text.Length / 25;
            var line_count = lblInfo.Text.Split('\n').Length;
            this.Height += (tmp * 5) + (tmp * line_count);
            this.Width += tmp * 20;
            lblInfo.Height = this.Height;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void alert_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(@"http://lithuania.phoenix.loc/kas-instrukcijos/kas-instrukcijos");
            Process.Start(sInfo);
        }
    }
}
