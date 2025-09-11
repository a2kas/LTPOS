namespace POS_display.Views.SalesOrder
{
    partial class SalesOrderFeedbackView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesOrderFeedbackView));
            this.lblInfoField = new System.Windows.Forms.Label();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ucLoader = new POS_display.Helpers.LoaderUserControl();
            this.SuspendLayout();
            // 
            // lblInfoField
            // 
            this.lblInfoField.AutoSize = true;
            this.lblInfoField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblInfoField.Location = new System.Drawing.Point(12, 9);
            this.lblInfoField.Name = "lblInfoField";
            this.lblInfoField.Size = new System.Drawing.Size(234, 16);
            this.lblInfoField.TabIndex = 0;
            this.lblInfoField.Text = "Laukiama kliento sutikimas terminale...";
            // 
            // btnApprove
            // 
            this.btnApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnApprove.Location = new System.Drawing.Point(15, 44);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(86, 71);
            this.btnApprove.TabIndex = 1;
            this.btnApprove.Text = "Patvirtinti";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnCancel.Location = new System.Drawing.Point(107, 44);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 71);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Atšaukti";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ucLoader
            // 
            this.ucLoader.BackColor = System.Drawing.Color.Transparent;
            this.ucLoader.IsLoading = false;
            this.ucLoader.Location = new System.Drawing.Point(200, 28);
            this.ucLoader.Name = "ucLoader";
            this.ucLoader.Size = new System.Drawing.Size(100, 100);
            this.ucLoader.TabIndex = 3;
            // 
            // SalesOrderFeedbackView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 130);
            this.Controls.Add(this.ucLoader);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApprove);
            this.Controls.Add(this.lblInfoField);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SalesOrderFeedbackView";
            this.Text = "Kliento sutikimas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SalesOrderFeedbackView_FormClosing);
            this.Shown += new System.EventHandler(this.SalesOrderFeedbackView_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfoField;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Button btnCancel;
        private Helpers.LoaderUserControl ucLoader;
    }
}