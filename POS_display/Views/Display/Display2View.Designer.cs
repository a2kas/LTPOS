namespace POS_display
{
    partial class Display2View
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlDisplay2 = new System.Windows.Forms.Panel();
            this.ehMarquee = new System.Windows.Forms.Integration.ElementHost();
            this.ehDisplay2 = new System.Windows.Forms.Integration.ElementHost();
            this.pricesFromTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlDisplay2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDisplay2
            // 
            this.pnlDisplay2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlDisplay2.Controls.Add(this.ehMarquee);
            this.pnlDisplay2.Controls.Add(this.ehDisplay2);
            this.pnlDisplay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDisplay2.Location = new System.Drawing.Point(0, 0);
            this.pnlDisplay2.Name = "pnlDisplay2";
            this.pnlDisplay2.Size = new System.Drawing.Size(1280, 1024);
            this.pnlDisplay2.TabIndex = 0;
            // 
            // ehMarquee
            // 
            this.ehMarquee.BackColor = System.Drawing.Color.White;
            this.ehMarquee.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ehMarquee.ForeColor = System.Drawing.Color.White;
            this.ehMarquee.Location = new System.Drawing.Point(0, 958);
            this.ehMarquee.Name = "ehMarquee";
            this.ehMarquee.Size = new System.Drawing.Size(1280, 66);
            this.ehMarquee.TabIndex = 1;
            this.ehMarquee.Child = null;
            // 
            // ehDisplay2
            // 
            this.ehDisplay2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ehDisplay2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ehDisplay2.Location = new System.Drawing.Point(0, 0);
            this.ehDisplay2.Name = "ehDisplay2";
            this.ehDisplay2.Size = new System.Drawing.Size(1280, 955);
            this.ehDisplay2.TabIndex = 0;
            this.ehDisplay2.Child = null;
            // 
            // Display2View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 1024);
            this.ControlBox = false;
            this.Controls.Add(this.pnlDisplay2);
            this.Enabled = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimizeBox = false;
            this.Name = "Display2View";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.display2_Load);
            this.pnlDisplay2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDisplay2;
        public System.Windows.Forms.Integration.ElementHost ehDisplay2;
        private System.Windows.Forms.Integration.ElementHost ehMarquee;
        private System.Windows.Forms.Timer pricesFromTimer;
    }
}