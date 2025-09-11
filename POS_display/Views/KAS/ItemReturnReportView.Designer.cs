using POS_display.Properties;

namespace POS_display.Views.KAS
{
    partial class ItemReturnReportView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblPharmacyNo = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAddress = new System.Windows.Forms.Label();
            this.gbReturningItems = new System.Windows.Forms.GroupBox();
            this.dgvReturningItems = new System.Windows.Forms.DataGridView();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.tbReturnSum = new System.Windows.Forms.TextBox();
            this.tbBuyer = new System.Windows.Forms.TextBox();
            this.tbCashier = new System.Windows.Forms.TextBox();
            this.lblReportDate = new System.Windows.Forms.Label();
            this.lblReturningItem = new System.Windows.Forms.Label();
            this.lblBuyer = new System.Windows.Forms.Label();
            this.lblCashier = new System.Windows.Forms.Label();
            this.tbInsuranceCompany = new System.Windows.Forms.TextBox();
            this.tbChequeNr = new System.Windows.Forms.TextBox();
            this.tbCashDeskNr = new System.Windows.Forms.TextBox();
            this.lblChequeNr = new System.Windows.Forms.Label();
            this.lblInsuranceCompany = new System.Windows.Forms.Label();
            this.lblCashDeskNr = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCompensation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompensationOther = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVAT5Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVAT21Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaidSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbReturningItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturningItems)).BeginInit();
            this.gbInfo.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPharmacyNo
            // 
            this.lblPharmacyNo.AutoSize = true;
            this.lblPharmacyNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblPharmacyNo.Location = new System.Drawing.Point(15, 30);
            this.lblPharmacyNo.Name = "lblPharmacyNo";
            this.lblPharmacyNo.Size = new System.Drawing.Size(73, 13);
            this.lblPharmacyNo.TabIndex = 0;
            this.lblPharmacyNo.Text = "BENU 7120";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(6, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Spausdinti";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1010, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(15, 55);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(48, 13);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Adresas:";
            // 
            // gbReturningItems
            // 
            this.gbReturningItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbReturningItems.Controls.Add(this.dgvReturningItems);
            this.gbReturningItems.Location = new System.Drawing.Point(0, 357);
            this.gbReturningItems.Name = "gbReturningItems";
            this.gbReturningItems.Size = new System.Drawing.Size(1091, 423);
            this.gbReturningItems.TabIndex = 5;
            this.gbReturningItems.TabStop = false;
            this.gbReturningItems.Text = "Grąžinamos prekės";
            // 
            // dgvReturningItems
            // 
            this.dgvReturningItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReturningItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colQty,
            this.colDiscount,
            this.colPrice,
            this.colCompensation,
            this.CompensationOther,
            this.colVAT5Value,
            this.colVAT21Value,
            this.colPaidSum,
            this.colTotal});
            this.dgvReturningItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReturningItems.Location = new System.Drawing.Point(3, 16);
            this.dgvReturningItems.Name = "dgvReturningItems";
            this.dgvReturningItems.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvReturningItems.RowHeadersVisible = false;
            this.dgvReturningItems.Size = new System.Drawing.Size(1085, 404);
            this.dgvReturningItems.TabIndex = 0;
            this.dgvReturningItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturningItems_CellEndEdit);
            this.dgvReturningItems.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvReturningItems_CellValidating);
            // 
            // gbInfo
            // 
            this.gbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbInfo.Controls.Add(this.tbDate);
            this.gbInfo.Controls.Add(this.tbReturnSum);
            this.gbInfo.Controls.Add(this.tbBuyer);
            this.gbInfo.Controls.Add(this.tbCashier);
            this.gbInfo.Controls.Add(this.lblReportDate);
            this.gbInfo.Controls.Add(this.lblReturningItem);
            this.gbInfo.Controls.Add(this.lblBuyer);
            this.gbInfo.Controls.Add(this.lblCashier);
            this.gbInfo.Controls.Add(this.tbInsuranceCompany);
            this.gbInfo.Controls.Add(this.tbChequeNr);
            this.gbInfo.Controls.Add(this.tbCashDeskNr);
            this.gbInfo.Controls.Add(this.lblChequeNr);
            this.gbInfo.Controls.Add(this.lblInsuranceCompany);
            this.gbInfo.Controls.Add(this.lblCashDeskNr);
            this.gbInfo.Controls.Add(this.tbAddress);
            this.gbInfo.Controls.Add(this.lblPharmacyNo);
            this.gbInfo.Controls.Add(this.lblAddress);
            this.gbInfo.Location = new System.Drawing.Point(0, 49);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(1091, 302);
            this.gbInfo.TabIndex = 6;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Akto informacija:";
            // 
            // tbDate
            // 
            this.tbDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDate.Location = new System.Drawing.Point(105, 260);
            this.tbDate.Name = "tbDate";
            this.tbDate.Size = new System.Drawing.Size(974, 20);
            this.tbDate.TabIndex = 18;
            // 
            // tbReturnSum
            // 
            this.tbReturnSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReturnSum.Location = new System.Drawing.Point(105, 230);
            this.tbReturnSum.Name = "tbReturnSum";
            this.tbReturnSum.Size = new System.Drawing.Size(974, 20);
            this.tbReturnSum.TabIndex = 17;
            // 
            // tbBuyer
            // 
            this.tbBuyer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBuyer.Location = new System.Drawing.Point(105, 200);
            this.tbBuyer.Name = "tbBuyer";
            this.tbBuyer.Size = new System.Drawing.Size(974, 20);
            this.tbBuyer.TabIndex = 16;
            // 
            // tbCashier
            // 
            this.tbCashier.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCashier.Location = new System.Drawing.Point(105, 170);
            this.tbCashier.Name = "tbCashier";
            this.tbCashier.Size = new System.Drawing.Size(974, 20);
            this.tbCashier.TabIndex = 15;
            // 
            // lblReportDate
            // 
            this.lblReportDate.AutoSize = true;
            this.lblReportDate.Location = new System.Drawing.Point(15, 265);
            this.lblReportDate.Name = "lblReportDate";
            this.lblReportDate.Size = new System.Drawing.Size(56, 13);
            this.lblReportDate.TabIndex = 14;
            this.lblReportDate.Text = "Akto data:";
            // 
            // lblReturningItem
            // 
            this.lblReturningItem.AutoSize = true;
            this.lblReturningItem.Location = new System.Drawing.Point(15, 235);
            this.lblReturningItem.Name = "lblReturningItem";
            this.lblReturningItem.Size = new System.Drawing.Size(88, 13);
            this.lblReturningItem.TabIndex = 13;
            this.lblReturningItem.Text = "Grąžinama suma:";
            // 
            // lblBuyer
            // 
            this.lblBuyer.AutoSize = true;
            this.lblBuyer.Location = new System.Drawing.Point(15, 205);
            this.lblBuyer.Name = "lblBuyer";
            this.lblBuyer.Size = new System.Drawing.Size(62, 13);
            this.lblBuyer.TabIndex = 12;
            this.lblBuyer.Text = "Pirkėjas(-a):";
            // 
            // lblCashier
            // 
            this.lblCashier.AutoSize = true;
            this.lblCashier.Location = new System.Drawing.Point(15, 175);
            this.lblCashier.Name = "lblCashier";
            this.lblCashier.Size = new System.Drawing.Size(76, 13);
            this.lblCashier.TabIndex = 11;
            this.lblCashier.Text = "Kasininkas(-ė):";
            // 
            // tbInsuranceCompany
            // 
            this.tbInsuranceCompany.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInsuranceCompany.Location = new System.Drawing.Point(105, 140);
            this.tbInsuranceCompany.Name = "tbInsuranceCompany";
            this.tbInsuranceCompany.Size = new System.Drawing.Size(974, 20);
            this.tbInsuranceCompany.TabIndex = 10;
            // 
            // tbChequeNr
            // 
            this.tbChequeNr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChequeNr.Location = new System.Drawing.Point(105, 110);
            this.tbChequeNr.Name = "tbChequeNr";
            this.tbChequeNr.Size = new System.Drawing.Size(974, 20);
            this.tbChequeNr.TabIndex = 9;
            // 
            // tbCashDeskNr
            // 
            this.tbCashDeskNr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCashDeskNr.Location = new System.Drawing.Point(105, 80);
            this.tbCashDeskNr.Name = "tbCashDeskNr";
            this.tbCashDeskNr.Size = new System.Drawing.Size(974, 20);
            this.tbCashDeskNr.TabIndex = 8;
            // 
            // lblChequeNr
            // 
            this.lblChequeNr.AutoSize = true;
            this.lblChequeNr.Location = new System.Drawing.Point(15, 115);
            this.lblChequeNr.Name = "lblChequeNr";
            this.lblChequeNr.Size = new System.Drawing.Size(48, 13);
            this.lblChequeNr.TabIndex = 7;
            this.lblChequeNr.Text = "Kvito Nr:";
            // 
            // lblInsuranceCompany
            // 
            this.lblInsuranceCompany.AutoSize = true;
            this.lblInsuranceCompany.Location = new System.Drawing.Point(15, 145);
            this.lblInsuranceCompany.Name = "lblInsuranceCompany";
            this.lblInsuranceCompany.Size = new System.Drawing.Size(87, 13);
            this.lblInsuranceCompany.TabIndex = 6;
            this.lblInsuranceCompany.Text = "Draudimo komp.:";
            // 
            // lblCashDeskNr
            // 
            this.lblCashDeskNr.AutoSize = true;
            this.lblCashDeskNr.Location = new System.Drawing.Point(15, 85);
            this.lblCashDeskNr.Name = "lblCashDeskNr";
            this.lblCashDeskNr.Size = new System.Drawing.Size(71, 13);
            this.lblCashDeskNr.TabIndex = 5;
            this.lblCashDeskNr.Text = "Kasos ap. Nr:";
            // 
            // tbAddress
            // 
            this.tbAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddress.Location = new System.Drawing.Point(105, 50);
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(974, 20);
            this.tbAddress.TabIndex = 4;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnPrint);
            this.gbActions.Controls.Add(this.btnClose);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbActions.Location = new System.Drawing.Point(0, 0);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(1091, 47);
            this.gbActions.TabIndex = 7;
            this.gbActions.TabStop = false;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Pavadinimas";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 330;
            // 
            // colQty
            // 
            this.colQty.DataPropertyName = "Qty";
            this.colQty.HeaderText = "Kiekis";
            this.colQty.Name = "colQty";
            this.colQty.Width = 50;
            // 
            // colDiscount
            // 
            this.colDiscount.DataPropertyName = "DiscountSum";
            this.colDiscount.HeaderText = "Nuolaida";
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.Width = 60;
            // 
            // colPrice
            // 
            this.colPrice.DataPropertyName = "PriceWithVAT";
            this.colPrice.HeaderText = "Kaina prieš nuolaidą";
            this.colPrice.Name = "colPrice";
            // 
            // colCompensation
            // 
            this.colCompensation.DataPropertyName = "TotalCompensation";
            this.colCompensation.HeaderText = "Kompensacija TLK";
            this.colCompensation.Name = "colCompensation";
            // 
            // CompensationOther
            // 
            this.CompensationOther.DataPropertyName = "InsuranceSum";
            this.CompensationOther.HeaderText = "Kompencacija kiti";
            this.CompensationOther.Name = "CompensationOther";
            // 
            // colVAT5Value
            // 
            this.colVAT5Value.DataPropertyName = "VAT5Value";
            this.colVAT5Value.HeaderText = "PVM 5%";
            this.colVAT5Value.Name = "colVAT5Value";
            this.colVAT5Value.Width = 80;
            // 
            // colVAT21Value
            // 
            this.colVAT21Value.DataPropertyName = "VAT21Value";
            this.colVAT21Value.HeaderText = "PVM 21%";
            this.colVAT21Value.Name = "colVAT21Value";
            this.colVAT21Value.Width = 80;
            // 
            // colPaidSum
            // 
            this.colPaidSum.DataPropertyName = "SumWithVAT";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.colPaidSum.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPaidSum.HeaderText = "Sumokėta grynais arba kortele";
            this.colPaidSum.Name = "colPaidSum";
            this.colPaidSum.Width = 130;
            // 
            // colTotal
            // 
            this.colTotal.DataPropertyName = "TotalSum";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            this.colTotal.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTotal.HeaderText = "Viso";
            this.colTotal.Name = "colTotal";
            this.colTotal.Width = 50;
            // 
            // ItemReturnReportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 721);
            this.Controls.Add(this.gbActions);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.gbReturningItems);
            this.Name = "ItemReturnReportView";
            this.ShowIcon = false;
            this.Text = "Prekių grąžinimo aktas";
            this.gbReturningItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturningItems)).EndInit();
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPharmacyNo;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.GroupBox gbReturningItems;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.TextBox tbInsuranceCompany;
        private System.Windows.Forms.TextBox tbChequeNr;
        private System.Windows.Forms.TextBox tbCashDeskNr;
        private System.Windows.Forms.Label lblChequeNr;
        private System.Windows.Forms.Label lblInsuranceCompany;
        private System.Windows.Forms.Label lblCashDeskNr;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.TextBox tbReturnSum;
        private System.Windows.Forms.TextBox tbBuyer;
        private System.Windows.Forms.TextBox tbCashier;
        private System.Windows.Forms.Label lblReportDate;
        private System.Windows.Forms.Label lblReturningItem;
        private System.Windows.Forms.Label lblBuyer;
        private System.Windows.Forms.Label lblCashier;
        private System.Windows.Forms.DataGridView dgvReturningItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompensation;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompensationOther;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVAT5Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVAT21Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaidSum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
    }
}