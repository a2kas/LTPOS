namespace POS_display.Views.Erecipe.Prepayment
{
    partial class PrepaymentView
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
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.rtbContent = new POS_display.Helpers.RichTextBoxEx();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbContent.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbContent
            // 
            this.gbContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbContent.Controls.Add(this.rtbContent);
            this.gbContent.Location = new System.Drawing.Point(0, 47);
            this.gbContent.Name = "gbContent";
            this.gbContent.Size = new System.Drawing.Size(508, 170);
            this.gbContent.TabIndex = 0;
            this.gbContent.TabStop = false;
            // 
            // rtbContent
            // 
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.rtbContent.Location = new System.Drawing.Point(3, 16);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.ReadOnly = true;
            this.rtbContent.Size = new System.Drawing.Size(502, 151);
            this.rtbContent.TabIndex = 0;
            this.rtbContent.Text = "";
            // 
            // gbActions
            // 
            this.gbActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbActions.Controls.Add(this.btnClose);
            this.gbActions.Location = new System.Drawing.Point(3, 1);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(502, 47);
            this.gbActions.TabIndex = 1;
            this.gbActions.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(6, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 29);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PrepaymentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 217);
            this.Controls.Add(this.gbActions);
            this.Controls.Add(this.gbContent);
            this.Name = "PrepaymentView";
            this.ShowIcon = false;
            this.Text = "Paciento priemokos informacija";
            this.gbContent.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbContent;
        private Helpers.RichTextBoxEx rtbContent;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnClose;
    }
}