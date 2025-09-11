
namespace POS_display.Views.CRM
{
    partial class ClientSearchView
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
            this.gbClientInfo = new System.Windows.Forms.GroupBox();
            this.lblBirthdateValidationNote = new System.Windows.Forms.Label();
            this.mtbBirthdate = new System.Windows.Forms.MaskedTextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblBirthdate = new System.Windows.Forms.Label();
            this.btnFindClient = new System.Windows.Forms.Button();
            this.lblSurename = new System.Windows.Forms.Label();
            this.tbSurename = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbExistingClients = new System.Windows.Forms.GroupBox();
            this.clientDataGridView = new System.Windows.Forms.DataGridView();
            this.colCardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSurename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBirthDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbClientInfo.SuspendLayout();
            this.gbExistingClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // gbClientInfo
            // 
            this.gbClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbClientInfo.Controls.Add(this.lblBirthdateValidationNote);
            this.gbClientInfo.Controls.Add(this.mtbBirthdate);
            this.gbClientInfo.Controls.Add(this.btnConfirm);
            this.gbClientInfo.Controls.Add(this.btnClear);
            this.gbClientInfo.Controls.Add(this.lblBirthdate);
            this.gbClientInfo.Controls.Add(this.btnFindClient);
            this.gbClientInfo.Controls.Add(this.lblSurename);
            this.gbClientInfo.Controls.Add(this.tbSurename);
            this.gbClientInfo.Controls.Add(this.tbEmail);
            this.gbClientInfo.Controls.Add(this.tbPhone);
            this.gbClientInfo.Controls.Add(this.lblPhone);
            this.gbClientInfo.Controls.Add(this.lblEmail);
            this.gbClientInfo.Controls.Add(this.tbName);
            this.gbClientInfo.Controls.Add(this.lblName);
            this.gbClientInfo.Location = new System.Drawing.Point(0, 0);
            this.gbClientInfo.Name = "gbClientInfo";
            this.gbClientInfo.Size = new System.Drawing.Size(1237, 212);
            this.gbClientInfo.TabIndex = 1;
            this.gbClientInfo.TabStop = false;
            this.gbClientInfo.Text = "Kliento informacija";
            // 
            // lblBirthdateValidationNote
            // 
            this.lblBirthdateValidationNote.AutoSize = true;
            this.lblBirthdateValidationNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblBirthdateValidationNote.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBirthdateValidationNote.Location = new System.Drawing.Point(278, 132);
            this.lblBirthdateValidationNote.Name = "lblBirthdateValidationNote";
            this.lblBirthdateValidationNote.Size = new System.Drawing.Size(110, 13);
            this.lblBirthdateValidationNote.TabIndex = 25;
            this.lblBirthdateValidationNote.Text = "Metai/Mėnesis/Diena";
            this.lblBirthdateValidationNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mtbBirthdate
            // 
            this.mtbBirthdate.Location = new System.Drawing.Point(81, 129);
            this.mtbBirthdate.Mask = "0000/00/00";
            this.mtbBirthdate.Name = "mtbBirthdate";
            this.mtbBirthdate.Size = new System.Drawing.Size(191, 20);
            this.mtbBirthdate.TabIndex = 5;
            this.mtbBirthdate.ValidatingType = typeof(System.DateTime);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnConfirm.Location = new System.Drawing.Point(1066, 156);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(159, 39);
            this.btnConfirm.TabIndex = 8;
            this.btnConfirm.Text = "Taikyti kortelę";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnClear.Location = new System.Drawing.Point(171, 156);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(159, 39);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Išvalyti";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblBirthdate
            // 
            this.lblBirthdate.AutoSize = true;
            this.lblBirthdate.Location = new System.Drawing.Point(9, 132);
            this.lblBirthdate.Name = "lblBirthdate";
            this.lblBirthdate.Size = new System.Drawing.Size(68, 13);
            this.lblBirthdate.TabIndex = 20;
            this.lblBirthdate.Text = "Gimimo data:";
            // 
            // btnFindClient
            // 
            this.btnFindClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnFindClient.Location = new System.Drawing.Point(6, 156);
            this.btnFindClient.Name = "btnFindClient";
            this.btnFindClient.Size = new System.Drawing.Size(159, 39);
            this.btnFindClient.TabIndex = 6;
            this.btnFindClient.Text = "Ieškoti";
            this.btnFindClient.UseVisualStyleBackColor = true;
            this.btnFindClient.Click += new System.EventHandler(this.btnFindClient_Click);
            // 
            // lblSurename
            // 
            this.lblSurename.AutoSize = true;
            this.lblSurename.Location = new System.Drawing.Point(9, 54);
            this.lblSurename.Name = "lblSurename";
            this.lblSurename.Size = new System.Drawing.Size(50, 13);
            this.lblSurename.TabIndex = 18;
            this.lblSurename.Text = "Pavardė:";
            // 
            // tbSurename
            // 
            this.tbSurename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSurename.Location = new System.Drawing.Point(81, 51);
            this.tbSurename.MaxLength = 50;
            this.tbSurename.Name = "tbSurename";
            this.tbSurename.Size = new System.Drawing.Size(1144, 20);
            this.tbSurename.TabIndex = 2;
            this.tbSurename.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSurename_KeyPress);
            // 
            // tbEmail
            // 
            this.tbEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEmail.Location = new System.Drawing.Point(81, 103);
            this.tbEmail.MaxLength = 200;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(1144, 20);
            this.tbEmail.TabIndex = 4;
            // 
            // tbPhone
            // 
            this.tbPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPhone.Location = new System.Drawing.Point(81, 77);
            this.tbPhone.MaxLength = 11;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(1144, 20);
            this.tbPhone.TabIndex = 3;
            this.tbPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPhone_KeyPress);
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(9, 80);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(57, 13);
            this.lblPhone.TabIndex = 5;
            this.lblPhone.Text = "Telefonas:";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(9, 106);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(35, 13);
            this.lblEmail.TabIndex = 4;
            this.lblEmail.Text = "Email:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(81, 25);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(1144, 20);
            this.tbName.TabIndex = 1;
            this.tbName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbName_KeyPress);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(43, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Vardas:";
            // 
            // gbExistingClients
            // 
            this.gbExistingClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExistingClients.Controls.Add(this.clientDataGridView);
            this.gbExistingClients.Location = new System.Drawing.Point(0, 201);
            this.gbExistingClients.Name = "gbExistingClients";
            this.gbExistingClients.Size = new System.Drawing.Size(1237, 418);
            this.gbExistingClients.TabIndex = 2;
            this.gbExistingClients.TabStop = false;
            this.gbExistingClients.Text = "Esami klientai:";
            // 
            // clientDataGridView
            // 
            this.clientDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCardNumber,
            this.colName,
            this.colSurename,
            this.colPhone,
            this.colEmail,
            this.colBirthDate,
            this.colAddress,
            this.colCity,
            this.colPostCode,
            this.colCountry});
            this.clientDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientDataGridView.Location = new System.Drawing.Point(3, 16);
            this.clientDataGridView.MultiSelect = false;
            this.clientDataGridView.Name = "clientDataGridView";
            this.clientDataGridView.ReadOnly = true;
            this.clientDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientDataGridView.Size = new System.Drawing.Size(1231, 399);
            this.clientDataGridView.TabIndex = 0;
            this.clientDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientDataGridView_CellDoubleClick);
            this.clientDataGridView.SelectionChanged += new System.EventHandler(this.clientDataGridView_SelectionChanged);
            // 
            // colCardNumber
            // 
            this.colCardNumber.DataPropertyName = "CardNumber";
            this.colCardNumber.HeaderText = "Kortelės Nr.";
            this.colCardNumber.Name = "colCardNumber";
            this.colCardNumber.ReadOnly = true;
            this.colCardNumber.Width = 150;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Vardas";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 120;
            // 
            // colSurename
            // 
            this.colSurename.DataPropertyName = "Surename";
            this.colSurename.HeaderText = "Pavardė";
            this.colSurename.Name = "colSurename";
            this.colSurename.ReadOnly = true;
            this.colSurename.Width = 120;
            // 
            // colPhone
            // 
            this.colPhone.DataPropertyName = "Phone";
            this.colPhone.HeaderText = "Tel. Nr.";
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "Email";
            this.colEmail.HeaderText = "El.paštas";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.Width = 150;
            // 
            // colBirthDate
            // 
            this.colBirthDate.DataPropertyName = "BirthDate";
            this.colBirthDate.HeaderText = "Gimimo data";
            this.colBirthDate.Name = "colBirthDate";
            this.colBirthDate.ReadOnly = true;
            // 
            // colAddress
            // 
            this.colAddress.DataPropertyName = "Address";
            this.colAddress.HeaderText = "Adresas";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Width = 200;
            // 
            // colCity
            // 
            this.colCity.DataPropertyName = "City";
            this.colCity.HeaderText = "Miestas";
            this.colCity.Name = "colCity";
            this.colCity.ReadOnly = true;
            // 
            // colPostCode
            // 
            this.colPostCode.DataPropertyName = "PostCode";
            this.colPostCode.HeaderText = "Pašto kodas";
            this.colPostCode.Name = "colPostCode";
            this.colPostCode.ReadOnly = true;
            this.colPostCode.Width = 90;
            // 
            // colCountry
            // 
            this.colCountry.DataPropertyName = "Country";
            this.colCountry.HeaderText = "Šalis";
            this.colCountry.Name = "colCountry";
            this.colCountry.ReadOnly = true;
            this.colCountry.Width = 50;
            // 
            // ClientSearchView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 621);
            this.Controls.Add(this.gbExistingClients);
            this.Controls.Add(this.gbClientInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ClientSearchView";
            this.Text = "BENU kliento paieška";
            this.gbClientInfo.ResumeLayout(false);
            this.gbClientInfo.PerformLayout();
            this.gbExistingClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbClientInfo;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSurename;
        private System.Windows.Forms.TextBox tbSurename;
        private System.Windows.Forms.Button btnFindClient;
        private System.Windows.Forms.GroupBox gbExistingClients;
        private System.Windows.Forms.DataGridView clientDataGridView;
        private System.Windows.Forms.Label lblBirthdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSurename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBirthDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCountry;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.MaskedTextBox mtbBirthdate;
        private System.Windows.Forms.Label lblBirthdateValidationNote;
    }
}