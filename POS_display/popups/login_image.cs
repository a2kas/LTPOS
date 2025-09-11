using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POS_display
{
    public partial class login_image : Form
    {
        private int ImageInterval = 0;

        public login_image()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.ImageInterval++;

            if (this.ImageInterval >= 8)
            {
                this.LoadAnotherImage();
                this.ImageInterval = 0;
            }
        }

        public void LoadAnotherImage()
        {
            try
            {
                if (Session.ImagesPOS == null)
                    return;
                var img = helpers.GetRandomFromList(Session.ImagesPOS);
                if (img != null)
                {
                    using (MemoryStream stream = new MemoryStream(img))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                    pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.display2, ex.Message);
            }
        }

        public void perform_step(int step, string status)
        {
            statusLabel.Text = status;
            switch (step)
            {
                case 0:
                    progressBar.Value = step;
                    break;
                case 7:
                    progressBar.Value = step;
                    break;
                default:
                    for (int i = 0; i < step; i++)
                        progressBar.PerformStep();
                    break;
            }
        }
    }
}
