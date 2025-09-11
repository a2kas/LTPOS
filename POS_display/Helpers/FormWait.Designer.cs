namespace POS_display.Helpers
{
    partial class FormWait
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
            this.loaderUserControl = new POS_display.Helpers.LoaderUserControl();
            this.lblNotification = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loaderUserControl
            // 
            this.loaderUserControl.BackColor = System.Drawing.Color.Transparent;
            this.loaderUserControl.IsLoading = false;
            this.loaderUserControl.Location = new System.Drawing.Point(12, 12);
            this.loaderUserControl.Name = "loaderUserControl";
            this.loaderUserControl.Size = new System.Drawing.Size(87, 87);
            this.loaderUserControl.SpinnerLength = 80;
            this.loaderUserControl.TabIndex = 0;
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblNotification.Location = new System.Drawing.Point(109, 9);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(140, 16);
            this.lblNotification.TabIndex = 1;
            this.lblNotification.Text = "Prašome palaukti...";
            // 
            // FormWait
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 117);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.loaderUserControl);
            this.Name = "FormWait";
            this.Text = "FormWait";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWait_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LoaderUserControl loaderUserControl;
        private System.Windows.Forms.Label lblNotification;
    }
}