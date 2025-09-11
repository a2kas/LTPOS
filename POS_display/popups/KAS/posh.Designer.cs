namespace POS_display
{
    partial class posh
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnInvoice = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblRecordsStatus = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.horizontalLine2 = new HorizontalLine();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.horizontalLine1 = new HorizontalLine();
            this.label1 = new System.Windows.Forms.Label();
            this.ddFilter = new System.Windows.Forms.ComboBox();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.gvFilter = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.documentdate2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.debtorname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalsum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sumincvat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCheque = new System.Windows.Forms.Button();
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kopijuotiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGlobalBlue = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvFilter)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.btnInvoice, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLast, 11, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnNext, 9, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblRecordsStatus, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnPrevious, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnFirst, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnFilter, 9, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 11, 0);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ddFilter, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbFilter, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.gvFilter, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnCheque, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGlobalBlue, 2, 0);
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(804, 571);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnInvoice
            // 
            this.btnInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInvoice.Location = new System.Drawing.Point(70, 3);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(61, 24);
            this.btnInvoice.TabIndex = 85;
            this.btnInvoice.Text = "Sąskaita";
            this.btnInvoice.UseVisualStyleBackColor = true;
            this.btnInvoice.Click += new System.EventHandler(this.btnInvoice_Click);
            // 
            // btnLast
            // 
            this.btnLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLast.Location = new System.Drawing.Point(740, 544);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(61, 24);
            this.btnLast.TabIndex = 83;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // btnNext
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnNext, 2);
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Location = new System.Drawing.Point(606, 544);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(128, 24);
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
            this.lblRecordsStatus.Location = new System.Drawing.Point(338, 541);
            this.lblRecordsStatus.Name = "lblRecordsStatus";
            this.lblRecordsStatus.Size = new System.Drawing.Size(128, 30);
            this.lblRecordsStatus.TabIndex = 81;
            this.lblRecordsStatus.Text = "1 / 1";
            this.lblRecordsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnPrevious, 2);
            this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrevious.Location = new System.Drawing.Point(70, 544);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(128, 24);
            this.btnPrevious.TabIndex = 80;
            this.btnPrevious.Text = "Ankstesnis";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirst.Location = new System.Drawing.Point(3, 544);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(61, 24);
            this.btnFirst.TabIndex = 79;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // horizontalLine2
            // 
            this.horizontalLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.horizontalLine2, 17);
            this.horizontalLine2.Location = new System.Drawing.Point(3, 65);
            this.horizontalLine2.Name = "horizontalLine2";
            this.horizontalLine2.Size = new System.Drawing.Size(798, 2);
            this.horizontalLine2.TabIndex = 55;
            // 
            // btnFilter
            // 
            this.btnFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFilter.Location = new System.Drawing.Point(606, 38);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(61, 24);
            this.btnFilter.TabIndex = 54;
            this.btnFilter.Text = "&Ieškoti";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(740, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(61, 24);
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
            this.horizontalLine1.Size = new System.Drawing.Size(798, 2);
            this.horizontalLine1.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 30);
            this.label1.TabIndex = 51;
            this.label1.Text = "Paieška pagal:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ddFilter
            // 
            this.ddFilter.AccessibleDescription = "";
            this.ddFilter.AccessibleName = "";
            this.ddFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.ddFilter, 4);
            this.ddFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddFilter.FormattingEnabled = true;
            this.ddFilter.Location = new System.Drawing.Point(70, 38);
            this.ddFilter.Name = "ddFilter";
            this.ddFilter.Size = new System.Drawing.Size(262, 21);
            this.ddFilter.TabIndex = 52;
            this.ddFilter.SelectedIndexChanged += new System.EventHandler(this.ddFilter_Changed);
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.tbFilter, 4);
            this.tbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbFilter.Location = new System.Drawing.Point(338, 38);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(262, 23);
            this.tbFilter.TabIndex = 53;
            this.tbFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter_KeyDown);
            // 
            // gvFilter
            // 
            this.gvFilter.AllowUserToAddRows = false;
            this.gvFilter.AllowUserToDeleteRows = false;
            this.gvFilter.AllowUserToResizeColumns = false;
            this.gvFilter.AllowUserToResizeRows = false;
            this.gvFilter.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvFilter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvFilter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.deviceno,
            this.checkno,
            this.documentdate2,
            this.debtorname,
            this.totalsum,
            this.vat,
            this.sumincvat,
            this.type,
            this.status,
            this.sf});
            this.tableLayoutPanel1.SetColumnSpan(this.gvFilter, 12);
            this.gvFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvFilter.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvFilter.Location = new System.Drawing.Point(3, 73);
            this.gvFilter.MultiSelect = false;
            this.gvFilter.Name = "gvFilter";
            this.gvFilter.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvFilter.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvFilter.RowHeadersVisible = false;
            this.gvFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFilter.Size = new System.Drawing.Size(798, 465);
            this.gvFilter.StandardTab = true;
            this.gvFilter.TabIndex = 78;
            this.gvFilter.VirtualMode = true;
            this.gvFilter.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvFilter_DoubleClick);
            this.gvFilter.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvFilter_CellMouseDown);
            this.gvFilter.CurrentCellChanged += new System.EventHandler(this.gvFilter_CurrentCellChanged);
            this.gvFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvFilter_KeyDown);
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "Oper. nr.";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 73;
            // 
            // deviceno
            // 
            this.deviceno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.deviceno.DataPropertyName = "deviceno";
            this.deviceno.HeaderText = "Kasa";
            this.deviceno.Name = "deviceno";
            this.deviceno.ReadOnly = true;
            this.deviceno.Width = 56;
            // 
            // checkno
            // 
            this.checkno.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.checkno.DataPropertyName = "checkno";
            this.checkno.HeaderText = "Kvitas";
            this.checkno.Name = "checkno";
            this.checkno.ReadOnly = true;
            this.checkno.Width = 61;
            // 
            // documentdate2
            // 
            this.documentdate2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.documentdate2.DataPropertyName = "documentdate2";
            this.documentdate2.HeaderText = "Data";
            this.documentdate2.Name = "documentdate2";
            this.documentdate2.ReadOnly = true;
            this.documentdate2.Width = 55;
            // 
            // debtorname
            // 
            this.debtorname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.debtorname.DataPropertyName = "debtorname";
            this.debtorname.HeaderText = "Pirkėjas";
            this.debtorname.Name = "debtorname";
            this.debtorname.ReadOnly = true;
            // 
            // totalsum
            // 
            this.totalsum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.totalsum.DataPropertyName = "totalsum";
            this.totalsum.HeaderText = "Suma";
            this.totalsum.Name = "totalsum";
            this.totalsum.ReadOnly = true;
            this.totalsum.Width = 59;
            // 
            // vat
            // 
            this.vat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.vat.DataPropertyName = "vat";
            this.vat.HeaderText = "PVM";
            this.vat.Name = "vat";
            this.vat.ReadOnly = true;
            this.vat.Width = 55;
            // 
            // sumincvat
            // 
            this.sumincvat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.sumincvat.DataPropertyName = "sumincvat";
            this.sumincvat.HeaderText = "Viso";
            this.sumincvat.Name = "sumincvat";
            this.sumincvat.ReadOnly = true;
            this.sumincvat.Width = 52;
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.type.DataPropertyName = "type";
            this.type.HeaderText = "Tipas";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Width = 58;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.status.DataPropertyName = "status";
            this.status.HeaderText = "Statusas";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 73;
            // 
            // sf
            // 
            this.sf.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.sf.DataPropertyName = "sf";
            this.sf.HeaderText = "SF";
            this.sf.Name = "sf";
            this.sf.ReadOnly = true;
            this.sf.Width = 45;
            // 
            // btnCheque
            // 
            this.btnCheque.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCheque.Location = new System.Drawing.Point(3, 3);
            this.btnCheque.Name = "btnCheque";
            this.btnCheque.Size = new System.Drawing.Size(61, 24);
            this.btnCheque.TabIndex = 84;
            this.btnCheque.Text = "Kvitas";
            this.btnCheque.UseVisualStyleBackColor = true;
            this.btnCheque.Click += new System.EventHandler(this.btnCheque_Click);
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
            this.kopijuotiToolStripMenuItem.Click += new System.EventHandler(this.kopijuotiToolStripMenuItem_Click);
            // 
            // btnGlobalBlue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnGlobalBlue, 2);
            this.btnGlobalBlue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGlobalBlue.Location = new System.Drawing.Point(137, 3);
            this.btnGlobalBlue.Name = "btnGlobalBlue";
            this.btnGlobalBlue.Size = new System.Drawing.Size(128, 24);
            this.btnGlobalBlue.TabIndex = 86;
            this.btnGlobalBlue.Text = "Global Blue kvitas";
            this.btnGlobalBlue.UseVisualStyleBackColor = true;
            this.btnGlobalBlue.Click += new System.EventHandler(this.btnGlobalBlue_Click);
            // 
            // posh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(804, 571);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "posh";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Kasos operacijų lentelė";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.posh_Closing);
            this.Load += new System.EventHandler(this.posh_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvFilter)).EndInit();
            this.dgvContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private HorizontalLine horizontalLine1;
        private HorizontalLine horizontalLine2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox tbFilter;
        private System.Windows.Forms.DataGridView gvFilter;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Label lblRecordsStatus;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.ContextMenuStrip dgvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem kopijuotiToolStripMenuItem;
        private System.Windows.Forms.Button btnCheque;
        private System.Windows.Forms.Button btnInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceno;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkno;
        private System.Windows.Forms.DataGridViewTextBoxColumn documentdate2;
        private System.Windows.Forms.DataGridViewTextBoxColumn debtorname;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalsum;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn sumincvat;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn sf;
        private System.Windows.Forms.Button btnGlobalBlue;
    }
}