
namespace POS_display.Views.SalesOrder
{
    partial class SalesOrderView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesOrderView));
            this.btnConfirm = new System.Windows.Forms.Button();
            this.gbClientInfo = new System.Windows.Forms.GroupBox();
            this.btnSendToTerminal = new System.Windows.Forms.Button();
            this.lblComment = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnPrintAgreement = new System.Windows.Forms.Button();
            this.btnEditClientData = new System.Windows.Forms.Button();
            this.btnFindClient = new System.Windows.Forms.Button();
            this.lblSurename = new System.Windows.Forms.Label();
            this.tbSurename = new System.Windows.Forms.TextBox();
            this.cbCountry = new System.Windows.Forms.ComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.lblPostCode = new System.Windows.Forms.Label();
            this.tbPostCode = new System.Windows.Forms.TextBox();
            this.tbCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.gbExistingClients = new System.Windows.Forms.GroupBox();
            this.clientDataGridView = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSurename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPostCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSaveClientData = new System.Windows.Forms.Button();
            this.gbClientInfo.SuspendLayout();
            this.gbExistingClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnConfirm.Location = new System.Drawing.Point(12, 272);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(154, 39);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "Patvirtinti užsakymą";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // gbClientInfo
            // 
            this.gbClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbClientInfo.Controls.Add(this.btnSaveClientData);
            this.gbClientInfo.Controls.Add(this.btnSendToTerminal);
            this.gbClientInfo.Controls.Add(this.lblComment);
            this.gbClientInfo.Controls.Add(this.tbComment);
            this.gbClientInfo.Controls.Add(this.btnClear);
            this.gbClientInfo.Controls.Add(this.btnPrintAgreement);
            this.gbClientInfo.Controls.Add(this.btnEditClientData);
            this.gbClientInfo.Controls.Add(this.btnFindClient);
            this.gbClientInfo.Controls.Add(this.lblSurename);
            this.gbClientInfo.Controls.Add(this.tbSurename);
            this.gbClientInfo.Controls.Add(this.cbCountry);
            this.gbClientInfo.Controls.Add(this.lblCountry);
            this.gbClientInfo.Controls.Add(this.lblPostCode);
            this.gbClientInfo.Controls.Add(this.tbPostCode);
            this.gbClientInfo.Controls.Add(this.tbCity);
            this.gbClientInfo.Controls.Add(this.lblCity);
            this.gbClientInfo.Controls.Add(this.tbAddress);
            this.gbClientInfo.Controls.Add(this.tbEmail);
            this.gbClientInfo.Controls.Add(this.tbPhone);
            this.gbClientInfo.Controls.Add(this.lblPhone);
            this.gbClientInfo.Controls.Add(this.lblEmail);
            this.gbClientInfo.Controls.Add(this.lblAddress);
            this.gbClientInfo.Controls.Add(this.tbName);
            this.gbClientInfo.Controls.Add(this.lblName);
            this.gbClientInfo.Controls.Add(this.btnConfirm);
            this.gbClientInfo.Location = new System.Drawing.Point(0, 0);
            this.gbClientInfo.Name = "gbClientInfo";
            this.gbClientInfo.Size = new System.Drawing.Size(1233, 320);
            this.gbClientInfo.TabIndex = 1;
            this.gbClientInfo.TabStop = false;
            this.gbClientInfo.Text = "Kliento informacija";
            // 
            // btnSendToTerminal
            // 
            this.btnSendToTerminal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnSendToTerminal.Location = new System.Drawing.Point(332, 272);
            this.btnSendToTerminal.Name = "btnSendToTerminal";
            this.btnSendToTerminal.Size = new System.Drawing.Size(136, 39);
            this.btnSendToTerminal.TabIndex = 25;
            this.btnSendToTerminal.Text = "Siųsti į terminalą";
            this.btnSendToTerminal.UseVisualStyleBackColor = true;
            this.btnSendToTerminal.Click += new System.EventHandler(this.btnSendToTerminal_Click);
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(9, 237);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(66, 13);
            this.lblComment.TabIndex = 24;
            this.lblComment.Text = "Komentaras:";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(81, 234);
            this.tbComment.MaxLength = 500;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(1140, 20);
            this.tbComment.TabIndex = 23;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1173, 272);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(48, 39);
            this.btnClear.TabIndex = 22;
            this.btnClear.Text = "Išvalyti";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPrintAgreement
            // 
            this.btnPrintAgreement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnPrintAgreement.Location = new System.Drawing.Point(172, 272);
            this.btnPrintAgreement.Name = "btnPrintAgreement";
            this.btnPrintAgreement.Size = new System.Drawing.Size(154, 39);
            this.btnPrintAgreement.TabIndex = 21;
            this.btnPrintAgreement.Text = "Spausdinti sutikimą";
            this.btnPrintAgreement.UseVisualStyleBackColor = true;
            this.btnPrintAgreement.Click += new System.EventHandler(this.btnPrintAgreement_Click);
            // 
            // btnEditClientData
            // 
            this.btnEditClientData.Location = new System.Drawing.Point(474, 272);
            this.btnEditClientData.Name = "btnEditClientData";
            this.btnEditClientData.Size = new System.Drawing.Size(152, 39);
            this.btnEditClientData.TabIndex = 20;
            this.btnEditClientData.Text = "Redaguoti kliento informaciją";
            this.btnEditClientData.UseVisualStyleBackColor = true;
            this.btnEditClientData.Click += new System.EventHandler(this.btnEditClientData_Click);
            // 
            // btnFindClient
            // 
            this.btnFindClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnFindClient.Location = new System.Drawing.Point(1008, 272);
            this.btnFindClient.Name = "btnFindClient";
            this.btnFindClient.Size = new System.Drawing.Size(159, 39);
            this.btnFindClient.TabIndex = 19;
            this.btnFindClient.Text = "Ieškoti kliento";
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
            this.tbSurename.Location = new System.Drawing.Point(81, 51);
            this.tbSurename.MaxLength = 50;
            this.tbSurename.Name = "tbSurename";
            this.tbSurename.Size = new System.Drawing.Size(1140, 20);
            this.tbSurename.TabIndex = 17;
            this.tbSurename.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSurename_KeyPress);
            // 
            // cbCountry
            // 
            this.cbCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCountry.FormattingEnabled = true;
            this.cbCountry.Items.AddRange(new object[] {
            "Lietuva"});
            this.cbCountry.Location = new System.Drawing.Point(81, 207);
            this.cbCountry.Name = "cbCountry";
            this.cbCountry.Size = new System.Drawing.Size(1140, 21);
            this.cbCountry.TabIndex = 15;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(9, 210);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(32, 13);
            this.lblCountry.TabIndex = 14;
            this.lblCountry.Text = "Šalis:";
            // 
            // lblPostCode
            // 
            this.lblPostCode.AutoSize = true;
            this.lblPostCode.Location = new System.Drawing.Point(9, 184);
            this.lblPostCode.Name = "lblPostCode";
            this.lblPostCode.Size = new System.Drawing.Size(69, 13);
            this.lblPostCode.TabIndex = 12;
            this.lblPostCode.Text = "Pašto kodas:";
            // 
            // tbPostCode
            // 
            this.tbPostCode.Location = new System.Drawing.Point(81, 181);
            this.tbPostCode.MaxLength = 10;
            this.tbPostCode.Name = "tbPostCode";
            this.tbPostCode.Size = new System.Drawing.Size(1140, 20);
            this.tbPostCode.TabIndex = 11;
            // 
            // tbCity
            // 
            this.tbCity.Location = new System.Drawing.Point(81, 155);
            this.tbCity.MaxLength = 50;
            this.tbCity.Name = "tbCity";
            this.tbCity.Size = new System.Drawing.Size(1140, 20);
            this.tbCity.TabIndex = 10;
            this.tbCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCity_KeyPress);
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(9, 158);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(46, 13);
            this.lblCity.TabIndex = 9;
            this.lblCity.Text = "Miestas:";
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(81, 129);
            this.tbAddress.MaxLength = 200;
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(1140, 20);
            this.tbAddress.TabIndex = 8;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(81, 103);
            this.tbEmail.MaxLength = 200;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(1140, 20);
            this.tbEmail.TabIndex = 7;
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(81, 77);
            this.tbPhone.MaxLength = 12;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(1140, 20);
            this.tbPhone.TabIndex = 6;
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
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(9, 132);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Adresas:";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(81, 25);
            this.tbName.MaxLength = 50;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(1140, 20);
            this.tbName.TabIndex = 2;
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
            this.gbExistingClients.Location = new System.Drawing.Point(0, 317);
            this.gbExistingClients.Name = "gbExistingClients";
            this.gbExistingClients.Size = new System.Drawing.Size(1233, 336);
            this.gbExistingClients.TabIndex = 2;
            this.gbExistingClients.TabStop = false;
            this.gbExistingClients.Text = "Esami klientai:";
            // 
            // clientDataGridView
            // 
            this.clientDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colSurename,
            this.colPhone,
            this.colEmail,
            this.colAddress,
            this.colCity,
            this.colPostCode,
            this.colCountry,
            this.colComment});
            this.clientDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientDataGridView.Location = new System.Drawing.Point(3, 16);
            this.clientDataGridView.Name = "clientDataGridView";
            this.clientDataGridView.ReadOnly = true;
            this.clientDataGridView.Size = new System.Drawing.Size(1227, 317);
            this.clientDataGridView.TabIndex = 0;
            this.clientDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientDataGridView_CellDoubleClick);
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
            // colComment
            // 
            this.colComment.DataPropertyName = "Comment";
            this.colComment.HeaderText = "Komentaras";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.Width = 250;
            // 
            // btnSaveClientData
            // 
            this.btnSaveClientData.Location = new System.Drawing.Point(632, 272);
            this.btnSaveClientData.Name = "btnSaveClientData";
            this.btnSaveClientData.Size = new System.Drawing.Size(152, 39);
            this.btnSaveClientData.TabIndex = 26;
            this.btnSaveClientData.Text = "Išsaugoti kliento informaciją";
            this.btnSaveClientData.UseVisualStyleBackColor = true;
            this.btnSaveClientData.Click += new System.EventHandler(this.btnSaveClientData_Click);
            // 
            // SalesOrderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 655);
            this.Controls.Add(this.gbExistingClients);
            this.Controls.Add(this.gbClientInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SalesOrderView";
            this.Text = "Prekės pardavimas į namus";
            this.gbClientInfo.ResumeLayout(false);
            this.gbClientInfo.PerformLayout();
            this.gbExistingClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox gbClientInfo;
        private System.Windows.Forms.Label lblPostCode;
        private System.Windows.Forms.TextBox tbPostCode;
        private System.Windows.Forms.TextBox tbCity;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.Label lblSurename;
        private System.Windows.Forms.TextBox tbSurename;
        private System.Windows.Forms.Button btnFindClient;
        private System.Windows.Forms.GroupBox gbExistingClients;
        private System.Windows.Forms.DataGridView clientDataGridView;
        private System.Windows.Forms.Button btnEditClientData;
        private System.Windows.Forms.Button btnPrintAgreement;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSurename;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPostCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCountry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.Button btnSendToTerminal;
        private System.Windows.Forms.Button btnSaveClientData;
    }
}