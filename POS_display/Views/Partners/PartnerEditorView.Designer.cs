namespace POS_display.Views.Partners
{
    partial class PartnerEditorView
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
            this.btnSave = new System.Windows.Forms.Button();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lblBuyerType = new System.Windows.Forms.Label();
            this.lblSupplierType = new System.Windows.Forms.Label();
            this.cbBuyerType = new System.Windows.Forms.ComboBox();
            this.cbSupplierType = new System.Windows.Forms.ComboBox();
            this.lblFax = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.tbFax = new System.Windows.Forms.TextBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.lblCountryCode = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPostIndex = new System.Windows.Forms.Label();
            this.tbPostIndex = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.tbCountryCode = new System.Windows.Forms.TextBox();
            this.tbVatCode = new System.Windows.Forms.TextBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbCompanyCode = new System.Windows.Forms.TextBox();
            this.lblVatCode = new System.Windows.Forms.Label();
            this.lblCompanyCode = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.tbCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.gbInfo.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Išsaugoti";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbInfo
            // 
            this.gbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbInfo.Controls.Add(this.lblCity);
            this.gbInfo.Controls.Add(this.tbCity);
            this.gbInfo.Controls.Add(this.lblAddress);
            this.gbInfo.Controls.Add(this.tbAddress);
            this.gbInfo.Controls.Add(this.lblComment);
            this.gbInfo.Controls.Add(this.tbComment);
            this.gbInfo.Controls.Add(this.lblBuyerType);
            this.gbInfo.Controls.Add(this.lblSupplierType);
            this.gbInfo.Controls.Add(this.cbBuyerType);
            this.gbInfo.Controls.Add(this.cbSupplierType);
            this.gbInfo.Controls.Add(this.lblFax);
            this.gbInfo.Controls.Add(this.lblPhone);
            this.gbInfo.Controls.Add(this.tbFax);
            this.gbInfo.Controls.Add(this.tbPhone);
            this.gbInfo.Controls.Add(this.lblCountryCode);
            this.gbInfo.Controls.Add(this.lblEmail);
            this.gbInfo.Controls.Add(this.lblPostIndex);
            this.gbInfo.Controls.Add(this.tbPostIndex);
            this.gbInfo.Controls.Add(this.tbEmail);
            this.gbInfo.Controls.Add(this.tbCountryCode);
            this.gbInfo.Controls.Add(this.tbVatCode);
            this.gbInfo.Controls.Add(this.cbType);
            this.gbInfo.Controls.Add(this.tbCompanyCode);
            this.gbInfo.Controls.Add(this.lblVatCode);
            this.gbInfo.Controls.Add(this.lblCompanyCode);
            this.gbInfo.Controls.Add(this.lblType);
            this.gbInfo.Controls.Add(this.tbName);
            this.gbInfo.Controls.Add(this.lblName);
            this.gbInfo.Location = new System.Drawing.Point(13, 70);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(473, 397);
            this.gbInfo.TabIndex = 1;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Duomenys";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(5, 129);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 32;
            this.lblAddress.Text = "Adresas:";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(126, 126);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(341, 20);
            this.tbAddress.TabIndex = 5;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(5, 363);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(66, 13);
            this.lblComment.TabIndex = 30;
            this.lblComment.Text = "Komentaras:";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(126, 360);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(341, 20);
            this.tbComment.TabIndex = 13;
            // 
            // lblBuyerType
            // 
            this.lblBuyerType.AutoSize = true;
            this.lblBuyerType.Location = new System.Drawing.Point(5, 336);
            this.lblBuyerType.Name = "lblBuyerType";
            this.lblBuyerType.Size = new System.Drawing.Size(67, 13);
            this.lblBuyerType.TabIndex = 25;
            this.lblBuyerType.Text = "Pirkėjo tipas:";
            // 
            // lblSupplierType
            // 
            this.lblSupplierType.AutoSize = true;
            this.lblSupplierType.Location = new System.Drawing.Point(5, 309);
            this.lblSupplierType.Name = "lblSupplierType";
            this.lblSupplierType.Size = new System.Drawing.Size(70, 13);
            this.lblSupplierType.TabIndex = 24;
            this.lblSupplierType.Text = "Tiekėjo tipas:";
            // 
            // cbBuyerType
            // 
            this.cbBuyerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuyerType.FormattingEnabled = true;
            this.cbBuyerType.Location = new System.Drawing.Point(126, 333);
            this.cbBuyerType.Name = "cbBuyerType";
            this.cbBuyerType.Size = new System.Drawing.Size(342, 21);
            this.cbBuyerType.TabIndex = 12;
            // 
            // cbSupplierType
            // 
            this.cbSupplierType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSupplierType.FormattingEnabled = true;
            this.cbSupplierType.Location = new System.Drawing.Point(126, 306);
            this.cbSupplierType.Name = "cbSupplierType";
            this.cbSupplierType.Size = new System.Drawing.Size(342, 21);
            this.cbSupplierType.TabIndex = 11;
            // 
            // lblFax
            // 
            this.lblFax.AutoSize = true;
            this.lblFax.Location = new System.Drawing.Point(5, 283);
            this.lblFax.Name = "lblFax";
            this.lblFax.Size = new System.Drawing.Size(44, 13);
            this.lblFax.TabIndex = 18;
            this.lblFax.Text = "Faksas:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(5, 257);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(57, 13);
            this.lblPhone.TabIndex = 17;
            this.lblPhone.Text = "Telefonas:";
            // 
            // tbFax
            // 
            this.tbFax.Location = new System.Drawing.Point(126, 280);
            this.tbFax.Name = "tbFax";
            this.tbFax.Size = new System.Drawing.Size(342, 20);
            this.tbFax.TabIndex = 10;
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(126, 254);
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(342, 20);
            this.tbPhone.TabIndex = 9;
            // 
            // lblCountryCode
            // 
            this.lblCountryCode.AutoSize = true;
            this.lblCountryCode.Location = new System.Drawing.Point(5, 231);
            this.lblCountryCode.Name = "lblCountryCode";
            this.lblCountryCode.Size = new System.Drawing.Size(70, 13);
            this.lblCountryCode.TabIndex = 14;
            this.lblCountryCode.Text = "Šalies kodas:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(5, 206);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(56, 13);
            this.lblEmail.TabIndex = 13;
            this.lblEmail.Text = "El. paštas:";
            // 
            // lblPostIndex
            // 
            this.lblPostIndex.AutoSize = true;
            this.lblPostIndex.Location = new System.Drawing.Point(5, 181);
            this.lblPostIndex.Name = "lblPostIndex";
            this.lblPostIndex.Size = new System.Drawing.Size(69, 13);
            this.lblPostIndex.TabIndex = 12;
            this.lblPostIndex.Text = "Pašto kodas:";
            // 
            // tbPostIndex
            // 
            this.tbPostIndex.Location = new System.Drawing.Point(125, 178);
            this.tbPostIndex.Name = "tbPostIndex";
            this.tbPostIndex.Size = new System.Drawing.Size(342, 20);
            this.tbPostIndex.TabIndex = 6;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(126, 203);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(342, 20);
            this.tbEmail.TabIndex = 7;
            // 
            // tbCountryCode
            // 
            this.tbCountryCode.Location = new System.Drawing.Point(126, 228);
            this.tbCountryCode.Name = "tbCountryCode";
            this.tbCountryCode.Size = new System.Drawing.Size(342, 20);
            this.tbCountryCode.TabIndex = 8;
            // 
            // tbVatCode
            // 
            this.tbVatCode.Location = new System.Drawing.Point(126, 100);
            this.tbVatCode.Name = "tbVatCode";
            this.tbVatCode.Size = new System.Drawing.Size(342, 20);
            this.tbVatCode.TabIndex = 4;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(126, 50);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(342, 21);
            this.cbType.TabIndex = 2;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // tbCompanyCode
            // 
            this.tbCompanyCode.Location = new System.Drawing.Point(126, 75);
            this.tbCompanyCode.Name = "tbCompanyCode";
            this.tbCompanyCode.Size = new System.Drawing.Size(342, 20);
            this.tbCompanyCode.TabIndex = 3;
            // 
            // lblVatCode
            // 
            this.lblVatCode.AutoSize = true;
            this.lblVatCode.Location = new System.Drawing.Point(5, 103);
            this.lblVatCode.Name = "lblVatCode";
            this.lblVatCode.Size = new System.Drawing.Size(66, 13);
            this.lblVatCode.TabIndex = 4;
            this.lblVatCode.Text = "PVM Kodas:";
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.AutoSize = true;
            this.lblCompanyCode.Location = new System.Drawing.Point(5, 78);
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Size = new System.Drawing.Size(76, 13);
            this.lblCompanyCode.TabIndex = 3;
            this.lblCompanyCode.Text = "Įmonės kodas:";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(5, 53);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 13);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Tipas:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(126, 25);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(342, 20);
            this.tbName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(5, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(70, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Pavadinimas:";
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnSave);
            this.gbActions.Location = new System.Drawing.Point(13, 12);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(473, 52);
            this.gbActions.TabIndex = 2;
            this.gbActions.TabStop = false;
            // 
            // tbCity
            // 
            this.tbCity.Location = new System.Drawing.Point(125, 152);
            this.tbCity.Name = "tbCity";
            this.tbCity.Size = new System.Drawing.Size(341, 20);
            this.tbCity.TabIndex = 33;
            // 
            // label1
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(6, 155);
            this.lblCity.Name = "label1";
            this.lblCity.Size = new System.Drawing.Size(46, 13);
            this.lblCity.TabIndex = 34;
            this.lblCity.Text = "Miestas:";
            // 
            // PartnerEditorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 479);
            this.Controls.Add(this.gbActions);
            this.Controls.Add(this.gbInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PartnerEditorView";
            this.ShowIcon = false;
            this.Text = "Partneris";
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.ComboBox cbBuyerType;
        private System.Windows.Forms.ComboBox cbSupplierType;
        private System.Windows.Forms.Label lblFax;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox tbFax;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label lblCountryCode;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPostIndex;
        private System.Windows.Forms.TextBox tbPostIndex;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.TextBox tbCountryCode;
        private System.Windows.Forms.TextBox tbVatCode;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbCompanyCode;
        private System.Windows.Forms.Label lblVatCode;
        private System.Windows.Forms.Label lblCompanyCode;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label lblBuyerType;
        private System.Windows.Forms.Label lblSupplierType;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox tbCity;
    }
}