namespace POS_display
{
    partial class ECRReportsView
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
            this.dtpSetTime = new System.Windows.Forms.DateTimePicker();
            this.dtpSetDate = new System.Windows.Forms.DateTimePicker();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.horizontalLine1 = new HorizontalLine();
            this.cmbReport = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbChange = new System.Windows.Forms.TextBox();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.dtpSetTime, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtpSetDate, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.dtpDateTo, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnCalc, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbReport, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbChange, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.dtpDateFrom, 1, 3);
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
            // dtpSetTime
            // 
            this.dtpSetTime.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dtpSetTime.CustomFormat = "HH:mm:ss";
            this.dtpSetTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpSetTime.Enabled = false;
            this.dtpSetTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSetTime.Location = new System.Drawing.Point(225, 98);
            this.dtpSetTime.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpSetTime.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpSetTime.Name = "dtpSetTime";
            this.dtpSetTime.ShowUpDown = true;
            this.dtpSetTime.Size = new System.Drawing.Size(118, 20);
            this.dtpSetTime.TabIndex = 7;
            // 
            // dtpSetDate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.dtpSetDate, 2);
            this.dtpSetDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpSetDate.Enabled = false;
            this.dtpSetDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSetDate.Location = new System.Drawing.Point(54, 98);
            this.dtpSetDate.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpSetDate.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpSetDate.Name = "dtpSetDate";
            this.dtpSetDate.Size = new System.Drawing.Size(114, 20);
            this.dtpSetDate.TabIndex = 6;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDateTo.Enabled = false;
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(225, 68);
            this.dtpDateTo.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpDateTo.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(118, 20);
            this.dtpDateTo.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(174, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 30);
            this.label6.TabIndex = 61;
            this.label6.Text = "Laikas";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(174, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 30);
            this.label5.TabIndex = 60;
            this.label5.Text = "Iki";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 3);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 30);
            this.label4.TabIndex = 59;
            this.label4.Text = "Pinigų įdėjimas / išėmimas";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 30);
            this.label3.TabIndex = 58;
            this.label3.Text = "Data";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 30);
            this.label2.TabIndex = 57;
            this.label2.Text = "Nuo";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCalc
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnCalc, 2);
            this.btnCalc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCalc.Enabled = false;
            this.btnCalc.Location = new System.Drawing.Point(3, 3);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(79, 24);
            this.btnCalc.TabIndex = 1;
            this.btnCalc.Text = "&Vykdyti";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 30);
            this.label1.TabIndex = 56;
            this.label1.Text = "Operacijos tipas";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // cmbReport
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cmbReport, 3);
            this.cmbReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReport.FormattingEnabled = true;
            this.cmbReport.Location = new System.Drawing.Point(88, 38);
            this.cmbReport.Name = "cmbReport";
            this.cmbReport.Size = new System.Drawing.Size(255, 21);
            this.cmbReport.TabIndex = 3;
            this.cmbReport.SelectedIndexChanged += new System.EventHandler(this.cmbReport_Changed);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(264, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbChange
            // 
            this.tbChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbChange, 2);
            this.tbChange.Enabled = false;
            this.tbChange.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbChange.Location = new System.Drawing.Point(174, 128);
            this.tbChange.Name = "tbChange";
            this.tbChange.Size = new System.Drawing.Size(169, 23);
            this.tbChange.TabIndex = 8;
            this.tbChange.TextChanged += new System.EventHandler(this.tbChange_TextChanged);
            this.tbChange.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbChange_KeyPress);
            // 
            // dtpDateFrom
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.dtpDateFrom, 2);
            this.dtpDateFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDateFrom.Enabled = false;
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(54, 68);
            this.dtpDateFrom.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpDateFrom.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(114, 20);
            this.dtpDateFrom.TabIndex = 4;
            // 
            // ECRReportsView
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
            this.Name = "ECRReportsView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Kasos aparato ataskaitos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ECRReports_Closing);
            this.Load += new System.EventHandler(this.ECRReportsView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbChange;
        private System.Windows.Forms.ComboBox cmbReport;
        private System.Windows.Forms.DateTimePicker dtpDateFrom;
        private System.Windows.Forms.DateTimePicker dtpSetTime;
        private System.Windows.Forms.DateTimePicker dtpSetDate;
        private System.Windows.Forms.DateTimePicker dtpDateTo;
    }
}