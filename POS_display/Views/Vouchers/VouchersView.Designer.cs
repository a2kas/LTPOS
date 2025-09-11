using System.Windows.Forms;

namespace POS_display.Views.Vouchers
{
    partial class VouchersView
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblRecordsStatus = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.horizontalLine1 = new HorizontalLine();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbListType = new System.Windows.Forms.GroupBox();
            this.rbCheque = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cbInstanceDiscount = new System.Windows.Forms.CheckBox();
            this.gvVouchers = new System.Windows.Forms.DataGridView();
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kopijuotiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbListType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvVouchers)).BeginInit();
            this.dgvContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.btnLast, 11, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnNext, 9, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblRecordsStatus, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnPrevious, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnFirst, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbListType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbInstanceDiscount, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.gvVouchers, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(621, 336);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnLast
            // 
            this.btnLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLast.Location = new System.Drawing.Point(564, 309);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(54, 24);
            this.btnLast.TabIndex = 83;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnNext, 2);
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Location = new System.Drawing.Point(462, 309);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(96, 24);
            this.btnNext.TabIndex = 82;
            this.btnNext.Text = "Sekantis";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblRecordsStatus
            // 
            this.lblRecordsStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblRecordsStatus, 2);
            this.lblRecordsStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordsStatus.Location = new System.Drawing.Point(258, 306);
            this.lblRecordsStatus.Name = "lblRecordsStatus";
            this.lblRecordsStatus.Size = new System.Drawing.Size(96, 30);
            this.lblRecordsStatus.TabIndex = 81;
            this.lblRecordsStatus.Text = "1 / 1";
            this.lblRecordsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnPrevious, 2);
            this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrevious.Location = new System.Drawing.Point(54, 309);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(96, 24);
            this.btnPrevious.TabIndex = 80;
            this.btnPrevious.Text = "Ankstesnis";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirst.Location = new System.Drawing.Point(3, 309);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(45, 24);
            this.btnFirst.TabIndex = 79;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnClose
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnClose, 2);
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(513, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 24);
            this.btnClose.TabIndex = 0;
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
            this.horizontalLine1.Size = new System.Drawing.Size(615, 1);
            this.horizontalLine1.TabIndex = 50;
            // 
            // btnOK
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnOK, 2);
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 24);
            this.btnOK.TabIndex = 85;
            this.btnOK.Text = "Vykdyti";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbListType
            // 
            this.gbListType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.SetColumnSpan(this.gbListType, 3);
            this.gbListType.Controls.Add(this.rbCheque);
            this.gbListType.Controls.Add(this.rbAll);
            this.gbListType.Location = new System.Drawing.Point(105, 3);
            this.gbListType.Name = "gbListType";
            this.gbListType.Size = new System.Drawing.Size(106, 24);
            this.gbListType.TabIndex = 88;
            this.gbListType.TabStop = false;
            // 
            // rbCheque
            // 
            this.rbCheque.AutoSize = true;
            this.rbCheque.Checked = true;
            this.rbCheque.Location = new System.Drawing.Point(53, 4);
            this.rbCheque.Name = "rbCheque";
            this.rbCheque.Size = new System.Drawing.Size(51, 17);
            this.rbCheque.TabIndex = 86;
            this.rbCheque.TabStop = true;
            this.rbCheque.Text = "Kvitui";
            this.rbCheque.UseVisualStyleBackColor = true;
            this.rbCheque.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(6, 4);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(41, 17);
            this.rbAll.TabIndex = 87;
            this.rbAll.Text = "Visi";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // cbInstanceDiscount
            // 
            this.cbInstanceDiscount.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.cbInstanceDiscount, 3);
            this.cbInstanceDiscount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbInstanceDiscount.Location = new System.Drawing.Point(258, 3);
            this.cbInstanceDiscount.Name = "cbInstanceDiscount";
            this.cbInstanceDiscount.Size = new System.Drawing.Size(147, 24);
            this.cbInstanceDiscount.TabIndex = 89;
            this.cbInstanceDiscount.Text = "Momentinė nuolaida";
            this.cbInstanceDiscount.UseVisualStyleBackColor = true;
            // 
            // gvVouchers
            // 
            this.gvVouchers.AllowUserToAddRows = false;
            this.gvVouchers.AllowUserToDeleteRows = false;
            this.gvVouchers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvVouchers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colCode,
            this.colName,
            this.colPriority,
            this.colMaxCount,
            this.colQty});
            this.tableLayoutPanel1.SetColumnSpan(this.gvVouchers, 12);
            this.gvVouchers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvVouchers.Location = new System.Drawing.Point(3, 38);
            this.gvVouchers.Name = "gvVouchers";
            this.gvVouchers.RowHeadersVisible = false;
            this.tableLayoutPanel1.SetRowSpan(this.gvVouchers, 3);
            this.gvVouchers.Size = new System.Drawing.Size(615, 265);
            this.gvVouchers.TabIndex = 90;
            this.gvVouchers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.gvVouchers.AllowUserToResizeRows = false;
            this.gvVouchers.VirtualMode = true;
            this.gvVouchers.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvVouchers_CellEndEdit);
            this.gvVouchers.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvVouchers_CellMouseDown);
            this.gvVouchers.CurrentCellChanged += new System.EventHandler(this.gvVouchers_CurrentCellChanged);
            // 
            // dgvContextMenu
            // 
            this.dgvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopijuotiToolStripMenuItem});
            this.dgvContextMenu.Name = "contextMenuStrip1";
            this.dgvContextMenu.Size = new System.Drawing.Size(123, 26);
            // 
            // kopijuotiToolStripMenuItem
            // 
            this.kopijuotiToolStripMenuItem.Name = "kopijuotiToolStripMenuItem";
            this.kopijuotiToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.kopijuotiToolStripMenuItem.Text = "Kopijuoti";
            this.kopijuotiToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            // 
            // colSelected
            // 
            this.colSelected.DataPropertyName = "Selected";
            this.colSelected.HeaderText = "Naudoti";
            this.colSelected.Name = "colSelected";
            this.colSelected.Width = 50;
            // 
            // colCode
            // 
            this.colCode.DataPropertyName = "Code";
            this.colCode.HeaderText = "Kodas";
            this.colCode.Name = "colCode";
            this.colCode.Width = 90;
            this.colCode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Pavadinimas";
            this.colName.Name = "colName";
            this.colName.Width = 220;
            this.colName.ReadOnly = true;
            // 
            // colPriority
            // 
            this.colPriority.DataPropertyName = "RewardPriority";
            this.colPriority.HeaderText = "Prioritetas";
            this.colPriority.Name = "colPriority";
            this.colPriority.Width = 70;
            this.colPriority.ReadOnly = true;
            // 
            // colMaxCount
            // 
            this.colMaxCount.DataPropertyName = "MaxCount";
            this.colMaxCount.HeaderText = "Daugiausiai kvite";
            this.colMaxCount.Name = "colMaxCount";
            this.colMaxCount.Width = 120;
            this.colMaxCount.ReadOnly = true;
            // 
            // colQty
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.colQty.DefaultCellStyle = dataGridViewCellStyle1;
            this.colQty.DataPropertyName = "Qty";
            this.colQty.HeaderText = "Kiekis";
            this.colQty.Name = "colQty";
            this.colQty.Width = 50;
            this.colQty.ReadOnly = true;
            // 
            // VouchersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(621, 336);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "VouchersView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Akcijos";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VouchersView_Closing);
            this.Load += new System.EventHandler(this.VouchersView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbListType.ResumeLayout(false);
            this.gbListType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvVouchers)).EndInit();
            this.dgvContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Label lblRecordsStatus;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.ContextMenuStrip dgvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem kopijuotiToolStripMenuItem;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbListType;
        private System.Windows.Forms.RadioButton rbCheque;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.CheckBox cbInstanceDiscount;
        private System.Windows.Forms.DataGridView gvVouchers;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
    }
}