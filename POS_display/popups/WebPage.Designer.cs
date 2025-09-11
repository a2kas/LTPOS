namespace POS_display
{
    partial class WebPage
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
            this.webBrowserPOS = new POS_display.WebBrowserPOS();
            this.SuspendLayout();
            // 
            // webBrowserPOS
            // 
            this.webBrowserPOS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserPOS.Location = new System.Drawing.Point(0, 0);
            this.webBrowserPOS.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserPOS.Name = "webBrowserPOS";
            this.webBrowserPOS.Size = new System.Drawing.Size(900, 626);
            this.webBrowserPOS.TabIndex = 0;
            this.webBrowserPOS.Closing += new ClosingEventHandler(this.webBrowserPOS_Closing);
            // 
            // WebPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 626);
            this.Controls.Add(this.webBrowserPOS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WebPage";
            this.Text = "WebPage";
            this.ResumeLayout(false);

        }

        #endregion

        private WebBrowserPOS webBrowserPOS;

    }
}