namespace POS_display
{
    partial class PrescriptionCheckView
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
			this.tlpLayout = new System.Windows.Forms.TableLayoutPanel();
			this.lblValidFrom = new System.Windows.Forms.Label();
			this.tbValidFrom = new System.Windows.Forms.TextBox();
			this.tbTillDate = new System.Windows.Forms.TextBox();
			this.tbCountDay = new System.Windows.Forms.TextBox();
			this.lblTillDate = new System.Windows.Forms.Label();
			this.lblaysAmount = new System.Windows.Forms.Label();
			this.lblAmountInDays = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblDoseAmount = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.horizontalLine1 = new HorizontalLine();
			this.tbDoses = new System.Windows.Forms.TextBox();
			this.tbQtyDay = new System.Windows.Forms.TextBox();
			this.dtpValidFrom = new System.Windows.Forms.DateTimePicker();
			this.dtpTillDate = new System.Windows.Forms.DateTimePicker();
			this.tlpLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlpLayout
			// 
			this.tlpLayout.ColumnCount = 5;
			this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.4086F));
			this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.62007F));
			this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.36917F));
			this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.602151F));
			this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
			this.tlpLayout.Controls.Add(this.lblValidFrom, 1, 1);
			this.tlpLayout.Controls.Add(this.tbValidFrom, 1, 2);
			this.tlpLayout.Controls.Add(this.tbTillDate, 1, 6);
			this.tlpLayout.Controls.Add(this.tbCountDay, 1, 5);
			this.tlpLayout.Controls.Add(this.lblTillDate, 0, 6);
			this.tlpLayout.Controls.Add(this.lblaysAmount, 0, 5);
			this.tlpLayout.Controls.Add(this.lblAmountInDays, 0, 4);
			this.tlpLayout.Controls.Add(this.btnSave, 0, 0);
			this.tlpLayout.Controls.Add(this.lblDoseAmount, 0, 3);
			this.tlpLayout.Controls.Add(this.btnClose, 4, 0);
			this.tlpLayout.Controls.Add(this.horizontalLine1, 0, 1);
			this.tlpLayout.Controls.Add(this.tbDoses, 1, 3);
			this.tlpLayout.Controls.Add(this.tbQtyDay, 1, 4);
			this.tlpLayout.Controls.Add(this.dtpValidFrom, 3, 2);
			this.tlpLayout.Controls.Add(this.dtpTillDate, 3, 6);
			this.tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpLayout.Location = new System.Drawing.Point(0, 0);
			this.tlpLayout.Name = "tlpLayout";
			this.tlpLayout.RowCount = 8;
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpLayout.Size = new System.Drawing.Size(346, 193);
			this.tlpLayout.TabIndex = 0;
			// 
			// lblValidFrom
			// 
			this.lblValidFrom.AutoSize = true;
			this.lblValidFrom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblValidFrom.Location = new System.Drawing.Point(3, 35);
			this.lblValidFrom.Name = "lblValidFrom";
			this.lblValidFrom.Size = new System.Drawing.Size(90, 30);
			this.lblValidFrom.TabIndex = 65;
			this.lblValidFrom.Text = "Įsigalioja nuo:";
			this.lblValidFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbValidFrom
			// 
			this.tbValidFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpLayout.SetColumnSpan(this.tbValidFrom, 2);
			this.tbValidFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
			this.tbValidFrom.Location = new System.Drawing.Point(99, 38);
			this.tbValidFrom.Name = "tbValidFrom";
			this.tbValidFrom.ReadOnly = true;
			this.tbValidFrom.Size = new System.Drawing.Size(153, 23);
			this.tbValidFrom.TabIndex = 64;
			// 
			// tbTillDate
			// 
			this.tbTillDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpLayout.SetColumnSpan(this.tbTillDate, 2);
			this.tbTillDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
			this.tbTillDate.Location = new System.Drawing.Point(99, 158);
			this.tbTillDate.Name = "tbTillDate";
			this.tbTillDate.ReadOnly = true;
			this.tbTillDate.Size = new System.Drawing.Size(153, 23);
			this.tbTillDate.TabIndex = 62;
			// 
			// tbCountDay
			// 
			this.tbCountDay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpLayout.SetColumnSpan(this.tbCountDay, 3);
			this.tbCountDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
			this.tbCountDay.Location = new System.Drawing.Point(99, 128);
			this.tbCountDay.MaxLength = 9;
			this.tbCountDay.Name = "tbCountDay";
			this.tbCountDay.Size = new System.Drawing.Size(177, 23);
			this.tbCountDay.TabIndex = 61;
			// 
			// lblTillDate
			// 
			this.lblTillDate.AutoSize = true;
			this.lblTillDate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTillDate.Location = new System.Drawing.Point(3, 155);
			this.lblTillDate.Name = "lblTillDate";
			this.lblTillDate.Size = new System.Drawing.Size(90, 30);
			this.lblTillDate.TabIndex = 60;
			this.lblTillDate.Text = "Pakanka iki:";
			this.lblTillDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblaysAmount
			// 
			this.lblaysAmount.AutoSize = true;
			this.lblaysAmount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblaysAmount.Location = new System.Drawing.Point(3, 125);
			this.lblaysAmount.Name = "lblaysAmount";
			this.lblaysAmount.Size = new System.Drawing.Size(90, 30);
			this.lblaysAmount.TabIndex = 59;
			this.lblaysAmount.Text = "Dienų skaičius:";
			this.lblaysAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAmountInDays
			// 
			this.lblAmountInDays.AutoSize = true;
			this.lblAmountInDays.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAmountInDays.Location = new System.Drawing.Point(3, 95);
			this.lblAmountInDays.Name = "lblAmountInDays";
			this.lblAmountInDays.Size = new System.Drawing.Size(90, 30);
			this.lblAmountInDays.TabIndex = 58;
			this.lblAmountInDays.Text = "Kiekis per dieną:";
			this.lblAmountInDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSave
			// 
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSave.Location = new System.Drawing.Point(3, 3);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(90, 24);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "&Išsaugoti";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lblDoseAmount
			// 
			this.lblDoseAmount.AutoSize = true;
			this.lblDoseAmount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDoseAmount.Location = new System.Drawing.Point(3, 65);
			this.lblDoseAmount.Name = "lblDoseAmount";
			this.lblDoseAmount.Size = new System.Drawing.Size(90, 30);
			this.lblDoseAmount.TabIndex = 56;
			this.lblDoseAmount.Text = "Dozių kiekis:";
			this.lblDoseAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnClose.Location = new System.Drawing.Point(282, 3);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(61, 24);
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
			this.tlpLayout.SetColumnSpan(this.horizontalLine1, 17);
			this.horizontalLine1.Location = new System.Drawing.Point(3, 30);
			this.horizontalLine1.Name = "horizontalLine1";
			this.horizontalLine1.Size = new System.Drawing.Size(340, 2);
			this.horizontalLine1.TabIndex = 50;
			// 
			// tbDoses
			// 
			this.tbDoses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpLayout.SetColumnSpan(this.tbDoses, 3);
			this.tbDoses.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
			this.tbDoses.Location = new System.Drawing.Point(99, 68);
			this.tbDoses.MaxLength = 9;
			this.tbDoses.Name = "tbDoses";
			this.tbDoses.Size = new System.Drawing.Size(177, 23);
			this.tbDoses.TabIndex = 7;
			// 
			// tbQtyDay
			// 
			this.tbQtyDay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpLayout.SetColumnSpan(this.tbQtyDay, 3);
			this.tbQtyDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
			this.tbQtyDay.Location = new System.Drawing.Point(99, 98);
			this.tbQtyDay.MaxLength = 9;
			this.tbQtyDay.Name = "tbQtyDay";
			this.tbQtyDay.Size = new System.Drawing.Size(177, 23);
			this.tbQtyDay.TabIndex = 57;
			// 
			// dtpValidFrom
			// 
			this.dtpValidFrom.Location = new System.Drawing.Point(255, 40);
			this.dtpValidFrom.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.dtpValidFrom.Name = "dtpValidFrom";
			this.dtpValidFrom.Size = new System.Drawing.Size(15, 20);
			this.dtpValidFrom.TabIndex = 63;
			// 
			// dtpTillDate
			// 
			this.dtpTillDate.Location = new System.Drawing.Point(255, 160);
			this.dtpTillDate.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.dtpTillDate.Name = "dtpTillDate";
			this.dtpTillDate.Size = new System.Drawing.Size(15, 20);
			this.dtpTillDate.TabIndex = 66;
			// 
			// PrescriptionCheckView
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(346, 193);
			this.Controls.Add(this.tlpLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MinimizeBox = false;
			this.Name = "PrescriptionCheckView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Recepto čekis - papildoma informacija";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrescriptionCheckView_Closing);
			this.Load += new System.EventHandler(this.PrescriptionCheckView_Load);
			this.tlpLayout.ResumeLayout(false);
			this.tlpLayout.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private System.Windows.Forms.Button btnClose;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Label lblDoseAmount;
        private System.Windows.Forms.TextBox tbDoses;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbTillDate;
        private System.Windows.Forms.TextBox tbCountDay;
        private System.Windows.Forms.Label lblTillDate;
        private System.Windows.Forms.Label lblaysAmount;
        private System.Windows.Forms.Label lblAmountInDays;
        private System.Windows.Forms.TextBox tbQtyDay;
        private System.Windows.Forms.DateTimePicker dtpValidFrom;
        private System.Windows.Forms.TextBox tbValidFrom;
        private System.Windows.Forms.Label lblValidFrom;
        private System.Windows.Forms.DateTimePicker dtpTillDate;
    }
}