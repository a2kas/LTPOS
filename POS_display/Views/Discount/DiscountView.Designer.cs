namespace POS_display
{
    partial class DiscountView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCalc = new System.Windows.Forms.Button();
            this.lblCardNr = new System.Windows.Forms.Label();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblDiscountType = new System.Windows.Forms.Label();
            this.lblDiscountType2 = new System.Windows.Forms.Label();
            this.lblDiscountCategory = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.horizontalLine1 = new HorizontalLine();
            this.tbCardNo = new System.Windows.Forms.TextBox();
            this.cbDiscountCategory = new System.Windows.Forms.ComboBox();
            this.cbDiscountType1 = new System.Windows.Forms.ComboBox();
            this.cbDiscountType2 = new System.Windows.Forms.ComboBox();
            this.cbDiscountSum = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnCalc, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCardNr, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lblDiscount, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblDiscountType, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblDiscountType2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblDiscountCategory, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbCardNo, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbDiscountCategory, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbDiscountType1, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbDiscountType2, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbDiscountSum, 2, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(346, 193);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCalc
            // 
            this.btnCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalc.Location = new System.Drawing.Point(3, 3);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(92, 24);
            this.btnCalc.TabIndex = 1;
            this.btnCalc.Text = "&Skaičiuoti";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // lblCardNr
            // 
            this.lblCardNr.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCardNr, 2);
            this.lblCardNr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCardNr.Location = new System.Drawing.Point(3, 155);
            this.lblCardNr.Name = "lblCardNr";
            this.lblCardNr.Size = new System.Drawing.Size(141, 30);
            this.lblCardNr.TabIndex = 60;
            this.lblCardNr.Text = "Kortelės nr.:";
            this.lblCardNr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDiscount, 2);
            this.lblDiscount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscount.Location = new System.Drawing.Point(3, 125);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(141, 30);
            this.lblDiscount.TabIndex = 59;
            this.lblDiscount.Text = "Nuolaida:";
            this.lblDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscountType
            // 
            this.lblDiscountType.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDiscountType, 2);
            this.lblDiscountType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountType.Location = new System.Drawing.Point(3, 95);
            this.lblDiscountType.Name = "lblDiscountType";
            this.lblDiscountType.Size = new System.Drawing.Size(141, 30);
            this.lblDiscountType.TabIndex = 58;
            this.lblDiscountType.Text = "Nuolaidos tipas:";
            this.lblDiscountType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscountType2
            // 
            this.lblDiscountType2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDiscountType2, 2);
            this.lblDiscountType2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountType2.Location = new System.Drawing.Point(3, 65);
            this.lblDiscountType2.Name = "lblDiscountType2";
            this.lblDiscountType2.Size = new System.Drawing.Size(141, 30);
            this.lblDiscountType2.TabIndex = 57;
            this.lblDiscountType2.Text = "Nuolaida taikoma:";
            this.lblDiscountType2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiscountCategory
            // 
            this.lblDiscountCategory.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblDiscountCategory, 2);
            this.lblDiscountCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDiscountCategory.Location = new System.Drawing.Point(3, 35);
            this.lblDiscountCategory.Name = "lblDiscountCategory";
            this.lblDiscountCategory.Size = new System.Drawing.Size(141, 30);
            this.lblDiscountCategory.TabIndex = 56;
            this.lblDiscountCategory.Text = "Nuolaidos kategorija:";
            this.lblDiscountCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(248, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.horizontalLine1, 17);
            this.horizontalLine1.Location = new System.Drawing.Point(3, 30);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(340, 2);
            this.horizontalLine1.TabIndex = 50;
            // 
            // tbCardNo
            // 
            this.tbCardNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbCardNo, 2);
            this.tbCardNo.Enabled = false;
            this.tbCardNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCardNo.Location = new System.Drawing.Point(150, 158);
            this.tbCardNo.Name = "tbCardNo";
            this.tbCardNo.Size = new System.Drawing.Size(152, 23);
            this.tbCardNo.TabIndex = 7;
            this.tbCardNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbCardNo.TextChanged += new System.EventHandler(this.tbCardNo_TextChanged);
            // 
            // cbDiscountCategory
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbDiscountCategory, 2);
            this.cbDiscountCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiscountCategory.FormattingEnabled = true;
            this.cbDiscountCategory.Location = new System.Drawing.Point(150, 38);
            this.cbDiscountCategory.Name = "cbDiscountCategory";
            this.cbDiscountCategory.Size = new System.Drawing.Size(152, 21);
            this.cbDiscountCategory.TabIndex = 3;
            this.cbDiscountCategory.SelectedIndexChanged += new System.EventHandler(this.cbDiscountCategory_IndexChanged);
            // 
            // cbDiscountType1
            // 
            this.cbDiscountType1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiscountType1.FormattingEnabled = true;
            this.cbDiscountType1.Location = new System.Drawing.Point(150, 68);
            this.cbDiscountType1.Name = "cbDiscountType1";
            this.cbDiscountType1.Size = new System.Drawing.Size(92, 21);
            this.cbDiscountType1.TabIndex = 4;
            // 
            // cbDiscountType2
            // 
            this.cbDiscountType2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDiscountType2.FormattingEnabled = true;
            this.cbDiscountType2.Location = new System.Drawing.Point(150, 98);
            this.cbDiscountType2.Name = "cbDiscountType2";
            this.cbDiscountType2.Size = new System.Drawing.Size(92, 21);
            this.cbDiscountType2.TabIndex = 5;
            this.cbDiscountType2.SelectedIndexChanged += new System.EventHandler(this.cbDiscountType2_IndexChanged);
            // 
            // ddDiscountSum
            // 
            this.cbDiscountSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDiscountSum.FormattingEnabled = true;
            this.cbDiscountSum.Location = new System.Drawing.Point(150, 128);
            this.cbDiscountSum.Name = "ddDiscountSum";
            this.cbDiscountSum.Size = new System.Drawing.Size(92, 21);
            this.cbDiscountSum.TabIndex = 61;
            // 
            // DiscountView
            // 
            this.AcceptButton = this.btnCalc;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(346, 193);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "DiscountView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Nuolaidų suteikimas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiscountView_Closing);
            this.Load += new System.EventHandler(this.DiscountView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Label lblCardNr;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblDiscountType;
        private System.Windows.Forms.Label lblDiscountType2;
        private System.Windows.Forms.Label lblDiscountCategory;
        private System.Windows.Forms.TextBox tbCardNo;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.ComboBox cbDiscountCategory;
        private System.Windows.Forms.ComboBox cbDiscountType1;
        private System.Windows.Forms.ComboBox cbDiscountType2;
        private System.Windows.Forms.ComboBox cbDiscountSum;
    }
}