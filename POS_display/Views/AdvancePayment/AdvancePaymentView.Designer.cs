namespace POS_display.Views.AdvancePayment
{
    partial class AdvancePaymentView
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
            this.lblOrderNumber = new System.Windows.Forms.Label();
            this.lblAdvanceSum = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.horizontalLine1 = new HorizontalLine();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbAdvanceSum = new System.Windows.Forms.TextBox();
            this.tbOrderNumber = new System.Windows.Forms.TextBox();
            this.cbAdvancePaymentType = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.03601F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.28086F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.64712F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.03601F));
            this.tableLayoutPanel1.Controls.Add(this.lblOrderNumber, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblAdvanceSum, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbAdvanceSum, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbOrderNumber, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbAdvancePaymentType, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 173);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.AutoSize = true;
            this.lblOrderNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOrderNumber.Location = new System.Drawing.Point(3, 65);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Size = new System.Drawing.Size(116, 30);
            this.lblOrderNumber.TabIndex = 61;
            this.lblOrderNumber.Text = "Numeris:";
            this.lblOrderNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAdvanceSum
            // 
            this.lblAdvanceSum.AutoSize = true;
            this.lblAdvanceSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAdvanceSum.Location = new System.Drawing.Point(3, 95);
            this.lblAdvanceSum.Name = "lblAdvanceSum";
            this.lblAdvanceSum.Size = new System.Drawing.Size(116, 30);
            this.lblAdvanceSum.TabIndex = 57;
            this.lblAdvanceSum.Text = "Avanso suma:";
            this.lblAdvanceSum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Location = new System.Drawing.Point(3, 35);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(116, 30);
            this.lblType.TabIndex = 56;
            this.lblType.Text = "Tipas:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.horizontalLine1, 17);
            this.horizontalLine1.Location = new System.Drawing.Point(3, 30);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(484, 1);
            this.horizontalLine1.TabIndex = 50;
            // 
            // btnOK
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(377, 128);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 42);
            this.btnClose.TabIndex = 40;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbAdvanceSum
            // 
            this.tbAdvanceSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbAdvanceSum, 2);
            this.tbAdvanceSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbAdvanceSum.Location = new System.Drawing.Point(125, 98);
            this.tbAdvanceSum.MaxLength = 7;
            this.tbAdvanceSum.Name = "tbAdvanceSum";
            this.tbAdvanceSum.Size = new System.Drawing.Size(362, 23);
            this.tbAdvanceSum.TabIndex = 30;
            // 
            // tbOrderNumber
            // 
            this.tbOrderNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbOrderNumber, 2);
            this.tbOrderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbOrderNumber.Location = new System.Drawing.Point(125, 68);
            this.tbOrderNumber.MaxLength = 50;
            this.tbOrderNumber.Name = "tbOrderNumber";
            this.tbOrderNumber.Size = new System.Drawing.Size(362, 23);
            this.tbOrderNumber.TabIndex = 20;
            this.tbOrderNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbOrderNumber_KeyDown);
            // 
            // cbAdvancePaymentType
            // 
            this.cbAdvancePaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbAdvancePaymentType, 2);
            this.cbAdvancePaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdvancePaymentType.FormattingEnabled = true;
            this.cbAdvancePaymentType.Location = new System.Drawing.Point(125, 38);
            this.cbAdvancePaymentType.Name = "cbAdvancePaymentType";
            this.cbAdvancePaymentType.Size = new System.Drawing.Size(362, 21);
            this.cbAdvancePaymentType.TabIndex = 10;
            this.cbAdvancePaymentType.SelectedIndexChanged += new System.EventHandler(this.cbAdvancePaymentType_SelectedIndexChanged);
            // 
            // AdvancePaymentView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 173);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AdvancePaymentView";
            this.ShowIcon = false;
            this.Text = "Avansas";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblOrderNumber;
        private System.Windows.Forms.Label lblAdvanceSum;
        private System.Windows.Forms.Label lblType;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbAdvanceSum;
        private System.Windows.Forms.TextBox tbOrderNumber;
        private System.Windows.Forms.ComboBox cbAdvancePaymentType;
    }
}