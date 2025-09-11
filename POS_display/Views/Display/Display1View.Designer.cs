using System.Windows.Forms;
namespace POS_display
{
    partial class Display1View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display1View));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.kopijuotiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblForecast = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPurpose = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSaveable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslUserPrioritiesRatio = new System.Windows.Forms.ToolStripStatusLabel();
            this.poswrap = new System.Windows.Forms.Panel();
            this.tableLayout_poswrap = new System.Windows.Forms.TableLayoutPanel();
            this.LblMarketingConsent = new System.Windows.Forms.Label();
            this.btnEshop = new System.Windows.Forms.Button();
            this.btnDonation = new System.Windows.Forms.Button();
            this.btnCreateBENUclient = new System.Windows.Forms.Button();
            this.btnFMD = new System.Windows.Forms.Button();
            this.lblRestSum = new System.Windows.Forms.Label();
            this.tbRestSum = new System.Windows.Forms.TextBox();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnCheckBalance = new System.Windows.Forms.Button();
            this.btnInsurance = new System.Windows.Forms.Button();
            this.btnKAS = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogOff = new System.Windows.Forms.Button();
            this.btnX = new System.Windows.Forms.Button();
            this.btnCard = new System.Windows.Forms.Button();
            this.pos = new System.Windows.Forms.Panel();
            this.tableLayout_pos = new System.Windows.Forms.TableLayoutPanel();
            this.tbInsuranceSum = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.horizontalLine4 = new HorizontalLine();
            this.horizontalLine3 = new HorizontalLine();
            this.horizontalLine1 = new HorizontalLine();
            this.horizontalLine2 = new HorizontalLine();
            this.btnLast = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblInitName = new System.Windows.Forms.Label();
            this.tbVatSize = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.tbDiscPercent = new System.Windows.Forms.TextBox();
            this.tbPrice = new System.Windows.Forms.TextBox();
            this.tbQty = new System.Windows.Forms.TextBox();
            this.tbSum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbRecipeNo = new System.Windows.Forms.TextBox();
            this.panel2Pos = new System.Windows.Forms.Panel();
            this.btnLabel = new System.Windows.Forms.Button();
            this.btnSelBarcode = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnPrescriptionCheck = new System.Windows.Forms.Button();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.btnCheque = new System.Windows.Forms.Button();
            this.btnPrep = new System.Windows.Forms.Button();
            this.btnPrice = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.edtBarcode = new System.Windows.Forms.TextBox();
            this.gvPosd = new System.Windows.Forms.DataGridView();
            this.btnDeleteLine = new System.Windows.Forms.DataGridViewImageColumn();
            this.fmd_link = new System.Windows.Forms.DataGridViewLinkColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apply_insurance = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.have_recipe = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Saveable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.fmd_is_valid_for_sale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vatsize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcodename_info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pricediscounted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cheque_sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cheque_sum_insurance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcodename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recipeid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.compensationsum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.erecipe_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.erecipe_active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbChequeSum = new System.Windows.Forms.TextBox();
            this.tbTotalSum = new System.Windows.Forms.TextBox();
            this.lblRecordsStatus = new System.Windows.Forms.Label();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnVouchers = new System.Windows.Forms.Button();
            this.btnStockInfo = new System.Windows.Forms.Button();
            this.chb2display = new System.Windows.Forms.CheckBox();
            this.btnDrugPrice = new System.Windows.Forms.Button();
            this.btnERecipe = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.tbCompCode = new System.Windows.Forms.TextBox();
            this.tbCompPercent = new System.Windows.Forms.TextBox();
            this.tbRecCompSum = new System.Windows.Forms.TextBox();
            this.tbEndSum = new System.Windows.Forms.TextBox();
            this.tbPaySum = new System.Windows.Forms.TextBox();
            this.tbRecTotalSum = new System.Windows.Forms.TextBox();
            this.tbCheckValue = new System.Windows.Forms.TextBox();
            this.gbRecommendations = new System.Windows.Forms.GroupBox();
            this.loaderUserControl = new POS_display.Helpers.LoaderUserControl();
            this.gvRecommandations = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblCRMDataLoadStatus = new System.Windows.Forms.Label();
            this.btnCRMDataReload = new System.Windows.Forms.Button();
            this.lblDisplay2 = new System.Windows.Forms.Label();
            this.lblOnHandQty = new System.Windows.Forms.Label();
            this.tbOnHandQty = new System.Windows.Forms.TextBox();
            this.btnResetPos = new System.Windows.Forms.Button();
            this.btnEcrRep = new System.Windows.Forms.Button();
            this.chbFiscal = new System.Windows.Forms.CheckBox();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnPersonalPharmacist = new System.Windows.Forms.Button();
            this.btnClientSearch = new System.Windows.Forms.Button();
            this.btnHomeMode = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnAdvancePayment = new System.Windows.Forms.Button();
            this.btnWoltMode = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.dgvContextMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.poswrap.SuspendLayout();
            this.tableLayout_poswrap.SuspendLayout();
            this.pos.SuspendLayout();
            this.tableLayout_pos.SuspendLayout();
            this.panel2Pos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPosd)).BeginInit();
            this.panelNavigation.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbRecommendations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvRecommandations)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.updateToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 92);
            this.contextMenuStrip1.Text = "Baigti";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.adminToolStripMenuItem.Text = "Login Admin";
            this.adminToolStripMenuItem.Click += new System.EventHandler(this.adminToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.btnLogOff_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "tamro.POS";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dgvContextMenu
            // 
            this.dgvContextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.dgvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kopijuotiToolStripMenuItem});
            this.dgvContextMenu.Name = "dgvContextMenu";
            this.dgvContextMenu.Size = new System.Drawing.Size(123, 26);
            // 
            // kopijuotiToolStripMenuItem
            // 
            this.kopijuotiToolStripMenuItem.Name = "kopijuotiToolStripMenuItem";
            this.kopijuotiToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.kopijuotiToolStripMenuItem.Text = "Kopijuoti";
            this.kopijuotiToolStripMenuItem.Click += new System.EventHandler(this.kopijuotiToolStripMenuItem_Click);
            // 
            // systemTimer
            // 
            this.systemTimer.Interval = 1000;
            this.systemTimer.Tick += new System.EventHandler(this.systemTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel1,
            this.lblForecast,
            this.toolStripStatusLabel5,
            this.lblPurpose,
            this.toolStripStatusLabel4,
            this.tsslSaveable,
            this.tsslUserPrioritiesRatio});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(5, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1250, 24);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.LimeGreen;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(41, 19);
            this.toolStripStatusLabel2.Text = "******";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel3.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(130, 19);
            this.toolStripStatusLabel3.Text = "- Užpildytas receptas ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 19);
            this.toolStripStatusLabel1.Text = "R ! - Receptas privalomas";
            // 
            // lblForecast
            // 
            this.lblForecast.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblForecast.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblForecast.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblForecast.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblForecast.Name = "lblForecast";
            this.lblForecast.Size = new System.Drawing.Size(4, 19);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(66, 19);
            this.toolStripStatusLabel5.Text = "Prognozė: ";
            // 
            // lblPurpose
            // 
            this.lblPurpose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblPurpose.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPurpose.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblPurpose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPurpose.Name = "lblPurpose";
            this.lblPurpose.Size = new System.Drawing.Size(4, 19);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripStatusLabel4.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel4.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(218, 19);
            this.toolStripStatusLabel4.Text = "Vaistinės pardavimo tikslo vykdymas: ";
            // 
            // tsslSaveable
            // 
            this.tsslSaveable.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslSaveable.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslSaveable.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tsslSaveable.Name = "tsslSaveable";
            this.tsslSaveable.Size = new System.Drawing.Size(137, 19);
            this.tsslSaveable.Text = "S - Receptas saugomas";
            // 
            // tsslUserPrioritiesRatio
            // 
            this.tsslUserPrioritiesRatio.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslUserPrioritiesRatio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tsslUserPrioritiesRatio.Name = "tsslUserPrioritiesRatio";
            this.tsslUserPrioritiesRatio.Size = new System.Drawing.Size(343, 19);
            this.tsslUserPrioritiesRatio.Text = "Jūsų šio mėnesio prioritetinių prekių pardavimo santykis: 0 %";
            // 
            // poswrap
            // 
            this.poswrap.AutoSize = true;
            this.poswrap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.poswrap.Controls.Add(this.tableLayout_poswrap);
            this.poswrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poswrap.Location = new System.Drawing.Point(5, 5);
            this.poswrap.Name = "poswrap";
            this.poswrap.Size = new System.Drawing.Size(1250, 677);
            this.poswrap.TabIndex = 1;
            // 
            // tableLayout_poswrap
            // 
            this.tableLayout_poswrap.AutoSize = true;
            this.tableLayout_poswrap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout_poswrap.ColumnCount = 11;
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout_poswrap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayout_poswrap.Controls.Add(this.LblMarketingConsent, 6, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnEshop, 2, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnDonation, 2, 0);
            this.tableLayout_poswrap.Controls.Add(this.btnCreateBENUclient, 3, 0);
            this.tableLayout_poswrap.Controls.Add(this.btnFMD, 3, 1);
            this.tableLayout_poswrap.Controls.Add(this.lblRestSum, 1, 0);
            this.tableLayout_poswrap.Controls.Add(this.tbRestSum, 1, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnPayment, 0, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnCheckBalance, 2, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnInsurance, 2, 2);
            this.tableLayout_poswrap.Controls.Add(this.btnKAS, 3, 2);
            this.tableLayout_poswrap.Controls.Add(this.btnSettings, 9, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnLogOff, 10, 0);
            this.tableLayout_poswrap.Controls.Add(this.btnX, 9, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnCard, 10, 2);
            this.tableLayout_poswrap.Controls.Add(this.pos, 0, 4);
            this.tableLayout_poswrap.Controls.Add(this.btnResetPos, 10, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnEcrRep, 8, 1);
            this.tableLayout_poswrap.Controls.Add(this.chbFiscal, 7, 1);
            this.tableLayout_poswrap.Controls.Add(this.lblInfo2, 6, 2);
            this.tableLayout_poswrap.Controls.Add(this.btnHelp, 10, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnPersonalPharmacist, 3, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnClientSearch, 4, 3);
            this.tableLayout_poswrap.Controls.Add(this.btnHomeMode, 4, 2);
            this.tableLayout_poswrap.Controls.Add(this.lblUser, 6, 0);
            this.tableLayout_poswrap.Controls.Add(this.btnAdvancePayment, 4, 1);
            this.tableLayout_poswrap.Controls.Add(this.btnWoltMode, 4, 0);
            this.tableLayout_poswrap.Controls.Add(this.btnTest, 9, 2);
            this.tableLayout_poswrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_poswrap.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_poswrap.Name = "tableLayout_poswrap";
            this.tableLayout_poswrap.RowCount = 7;
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout_poswrap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout_poswrap.Size = new System.Drawing.Size(1250, 677);
            this.tableLayout_poswrap.TabIndex = 10;
            // 
            // LblMarketingConsent
            // 
            this.LblMarketingConsent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMarketingConsent.AutoSize = true;
            this.tableLayout_poswrap.SetColumnSpan(this.LblMarketingConsent, 3);
            this.LblMarketingConsent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.LblMarketingConsent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(38)))), ((int)(((byte)(100)))));
            this.LblMarketingConsent.Location = new System.Drawing.Point(753, 90);
            this.LblMarketingConsent.Name = "LblMarketingConsent";
            this.LblMarketingConsent.Size = new System.Drawing.Size(314, 30);
            this.LblMarketingConsent.TabIndex = 60;
            this.LblMarketingConsent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEshop
            // 
            this.btnEshop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEshop.Location = new System.Drawing.Point(313, 93);
            this.btnEshop.Name = "btnEshop";
            this.btnEshop.Size = new System.Drawing.Size(149, 24);
            this.btnEshop.TabIndex = 59;
            this.btnEshop.Text = "E - užsakymai";
            this.btnEshop.UseVisualStyleBackColor = true;
            this.btnEshop.Click += new System.EventHandler(this.btnEshop_Click);
            // 
            // btnDonation
            // 
            this.btnDonation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDonation.Location = new System.Drawing.Point(313, 3);
            this.btnDonation.Name = "btnDonation";
            this.btnDonation.Size = new System.Drawing.Size(149, 24);
            this.btnDonation.TabIndex = 58;
            this.btnDonation.Text = "Taškų aukojimas";
            this.btnDonation.UseVisualStyleBackColor = true;
            this.btnDonation.Visible = false;
            this.btnDonation.Click += new System.EventHandler(this.btnDonation_Click);
            // 
            // btnCreateBENUclient
            // 
            this.btnCreateBENUclient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateBENUclient.Location = new System.Drawing.Point(468, 3);
            this.btnCreateBENUclient.Name = "btnCreateBENUclient";
            this.btnCreateBENUclient.Size = new System.Drawing.Size(149, 24);
            this.btnCreateBENUclient.TabIndex = 57;
            this.btnCreateBENUclient.Text = "Sukurti klientą";
            this.btnCreateBENUclient.UseVisualStyleBackColor = true;
            this.btnCreateBENUclient.Click += new System.EventHandler(this.btnCreateBENUclient_Click);
            // 
            // btnFMD
            // 
            this.btnFMD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFMD.Location = new System.Drawing.Point(468, 33);
            this.btnFMD.Name = "btnFMD";
            this.btnFMD.Size = new System.Drawing.Size(149, 24);
            this.btnFMD.TabIndex = 56;
            this.btnFMD.Text = "&FMD";
            this.btnFMD.UseVisualStyleBackColor = true;
            this.btnFMD.Click += new System.EventHandler(this.btnFMD_Click);
            // 
            // lblRestSum
            // 
            this.lblRestSum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRestSum.AutoSize = true;
            this.lblRestSum.Location = new System.Drawing.Point(158, 0);
            this.lblRestSum.Name = "lblRestSum";
            this.lblRestSum.Size = new System.Drawing.Size(35, 30);
            this.lblRestSum.TabIndex = 42;
            this.lblRestSum.Text = "Grąža";
            this.lblRestSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRestSum
            // 
            this.tbRestSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRestSum.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tbRestSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbRestSum.ForeColor = System.Drawing.Color.Red;
            this.tbRestSum.Location = new System.Drawing.Point(158, 33);
            this.tbRestSum.Name = "tbRestSum";
            this.tbRestSum.ReadOnly = true;
            this.tableLayout_poswrap.SetRowSpan(this.tbRestSum, 2);
            this.tbRestSum.Size = new System.Drawing.Size(149, 53);
            this.tbRestSum.TabIndex = 41;
            this.tbRestSum.TabStop = false;
            this.tbRestSum.Text = "0,00";
            // 
            // btnPayment
            // 
            this.btnPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnPayment.Location = new System.Drawing.Point(3, 33);
            this.btnPayment.Name = "btnPayment";
            this.tableLayout_poswrap.SetRowSpan(this.btnPayment, 2);
            this.btnPayment.Size = new System.Drawing.Size(149, 54);
            this.btnPayment.TabIndex = 38;
            this.btnPayment.Text = "ATSISKAITYMAS (F4)";
            this.btnPayment.UseVisualStyleBackColor = true;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnCheckBalance
            // 
            this.btnCheckBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCheckBalance.Location = new System.Drawing.Point(313, 33);
            this.btnCheckBalance.Name = "btnCheckBalance";
            this.btnCheckBalance.Size = new System.Drawing.Size(149, 24);
            this.btnCheckBalance.TabIndex = 37;
            this.btnCheckBalance.Text = "Patikrinti li&kutį";
            this.btnCheckBalance.UseVisualStyleBackColor = true;
            this.btnCheckBalance.Click += new System.EventHandler(this.btnCheckBalance_Click);
            // 
            // btnInsurance
            // 
            this.btnInsurance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsurance.Location = new System.Drawing.Point(313, 63);
            this.btnInsurance.Name = "btnInsurance";
            this.btnInsurance.Size = new System.Drawing.Size(149, 24);
            this.btnInsurance.TabIndex = 54;
            this.btnInsurance.Text = "Draudimo ko&mp.";
            this.btnInsurance.UseVisualStyleBackColor = true;
            this.btnInsurance.Click += new System.EventHandler(this.btnInsurance_Click);
            // 
            // btnKAS
            // 
            this.btnKAS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKAS.Location = new System.Drawing.Point(468, 63);
            this.btnKAS.Name = "btnKAS";
            this.btnKAS.Size = new System.Drawing.Size(149, 24);
            this.btnKAS.TabIndex = 55;
            this.btnKAS.Text = "K.&A.S.";
            this.btnKAS.UseVisualStyleBackColor = true;
            this.btnKAS.Click += new System.EventHandler(this.btnKAS_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(1073, 93);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(84, 24);
            this.btnSettings.TabIndex = 36;
            this.btnSettings.Text = "N&ustatymai";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnLogOff
            // 
            this.btnLogOff.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOff.Location = new System.Drawing.Point(1163, 3);
            this.btnLogOff.Name = "btnLogOff";
            this.btnLogOff.Size = new System.Drawing.Size(84, 24);
            this.btnLogOff.TabIndex = 14;
            this.btnLogOff.Text = "Už&daryti";
            this.btnLogOff.UseVisualStyleBackColor = true;
            this.btnLogOff.Click += new System.EventHandler(this.btnLogOff_Click);
            // 
            // btnX
            // 
            this.btnX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnX.Location = new System.Drawing.Point(1073, 33);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(84, 24);
            this.btnX.TabIndex = 16;
            this.btnX.Text = "&X";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // btnCard
            // 
            this.btnCard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCard.Location = new System.Drawing.Point(1163, 63);
            this.btnCard.Name = "btnCard";
            this.btnCard.Size = new System.Drawing.Size(84, 24);
            this.btnCard.TabIndex = 17;
            this.btnCard.Text = "Nuo&l. kortelė";
            this.btnCard.UseVisualStyleBackColor = true;
            this.btnCard.Click += new System.EventHandler(this.btnCard_Click);
            // 
            // pos
            // 
            this.pos.AutoScroll = true;
            this.pos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayout_poswrap.SetColumnSpan(this.pos, 11);
            this.pos.Controls.Add(this.tableLayout_pos);
            this.pos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pos.Location = new System.Drawing.Point(3, 123);
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(1244, 511);
            this.pos.TabIndex = 31;
            // 
            // tableLayout_pos
            // 
            this.tableLayout_pos.AutoSize = true;
            this.tableLayout_pos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayout_pos.ColumnCount = 16;
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.19512F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.439025F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.097562F));
            this.tableLayout_pos.Controls.Add(this.tbInsuranceSum, 15, 10);
            this.tableLayout_pos.Controls.Add(this.label16, 10, 10);
            this.tableLayout_pos.Controls.Add(this.btnNext, 13, 12);
            this.tableLayout_pos.Controls.Add(this.btnPrevious, 1, 12);
            this.tableLayout_pos.Controls.Add(this.btnFirst, 0, 12);
            this.tableLayout_pos.Controls.Add(this.horizontalLine4, 0, 11);
            this.tableLayout_pos.Controls.Add(this.horizontalLine3, 0, 6);
            this.tableLayout_pos.Controls.Add(this.horizontalLine1, 0, 1);
            this.tableLayout_pos.Controls.Add(this.horizontalLine2, 0, 3);
            this.tableLayout_pos.Controls.Add(this.btnLast, 15, 12);
            this.tableLayout_pos.Controls.Add(this.label8, 12, 10);
            this.tableLayout_pos.Controls.Add(this.label20, 11, 7);
            this.tableLayout_pos.Controls.Add(this.label19, 14, 7);
            this.tableLayout_pos.Controls.Add(this.label18, 9, 7);
            this.tableLayout_pos.Controls.Add(this.label10, 2, 7);
            this.tableLayout_pos.Controls.Add(this.lblInitName, 0, 5);
            this.tableLayout_pos.Controls.Add(this.tbVatSize, 15, 4);
            this.tableLayout_pos.Controls.Add(this.label15, 14, 5);
            this.tableLayout_pos.Controls.Add(this.label14, 12, 5);
            this.tableLayout_pos.Controls.Add(this.label13, 10, 5);
            this.tableLayout_pos.Controls.Add(this.label12, 14, 4);
            this.tableLayout_pos.Controls.Add(this.label11, 12, 4);
            this.tableLayout_pos.Controls.Add(this.lblProductName, 0, 4);
            this.tableLayout_pos.Controls.Add(this.tbDiscPercent, 13, 4);
            this.tableLayout_pos.Controls.Add(this.tbPrice, 13, 5);
            this.tableLayout_pos.Controls.Add(this.tbQty, 11, 5);
            this.tableLayout_pos.Controls.Add(this.tbSum, 15, 5);
            this.tableLayout_pos.Controls.Add(this.label9, 0, 7);
            this.tableLayout_pos.Controls.Add(this.tbRecipeNo, 1, 7);
            this.tableLayout_pos.Controls.Add(this.panel2Pos, 0, 2);
            this.tableLayout_pos.Controls.Add(this.gvPosd, 0, 8);
            this.tableLayout_pos.Controls.Add(this.tbChequeSum, 14, 10);
            this.tableLayout_pos.Controls.Add(this.tbTotalSum, 13, 10);
            this.tableLayout_pos.Controls.Add(this.lblRecordsStatus, 7, 12);
            this.tableLayout_pos.Controls.Add(this.panelNavigation, 0, 0);
            this.tableLayout_pos.Controls.Add(this.tbCompCode, 4, 7);
            this.tableLayout_pos.Controls.Add(this.tbCompPercent, 5, 7);
            this.tableLayout_pos.Controls.Add(this.tbRecCompSum, 15, 7);
            this.tableLayout_pos.Controls.Add(this.tbEndSum, 13, 7);
            this.tableLayout_pos.Controls.Add(this.tbPaySum, 12, 7);
            this.tableLayout_pos.Controls.Add(this.tbRecTotalSum, 10, 7);
            this.tableLayout_pos.Controls.Add(this.tbCheckValue, 11, 10);
            this.tableLayout_pos.Controls.Add(this.gbRecommendations, 0, 9);
            this.tableLayout_pos.Controls.Add(this.lblCRMDataLoadStatus, 1, 10);
            this.tableLayout_pos.Controls.Add(this.btnCRMDataReload, 0, 10);
            this.tableLayout_pos.Controls.Add(this.lblDisplay2, 3, 10);
            this.tableLayout_pos.Controls.Add(this.lblOnHandQty, 10, 4);
            this.tableLayout_pos.Controls.Add(this.tbOnHandQty, 11, 4);
            this.tableLayout_pos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_pos.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_pos.Name = "tableLayout_pos";
            this.tableLayout_pos.RowCount = 13;
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayout_pos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayout_pos.Size = new System.Drawing.Size(1240, 507);
            this.tableLayout_pos.TabIndex = 0;
            // 
            // tbInsuranceSum
            // 
            this.tbInsuranceSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInsuranceSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbInsuranceSum.Location = new System.Drawing.Point(1159, 444);
            this.tbInsuranceSum.Name = "tbInsuranceSum";
            this.tbInsuranceSum.ReadOnly = true;
            this.tbInsuranceSum.Size = new System.Drawing.Size(78, 23);
            this.tbInsuranceSum.TabIndex = 55;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(784, 441);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(69, 30);
            this.label16.TabIndex = 53;
            this.label16.Text = "Kvito vertė:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnNext
            // 
            this.tableLayout_pos.SetColumnSpan(this.btnNext, 2);
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Location = new System.Drawing.Point(1009, 479);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(144, 25);
            this.btnNext.TabIndex = 30;
            this.btnNext.Text = "Sekantis";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrevious.Location = new System.Drawing.Point(78, 479);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(145, 25);
            this.btnPrevious.TabIndex = 29;
            this.btnPrevious.Text = "Ankstesnis";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirst.Location = new System.Drawing.Point(3, 479);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(69, 25);
            this.btnFirst.TabIndex = 28;
            this.btnFirst.Text = "<<";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // horizontalLine4
            // 
            this.horizontalLine4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayout_pos.SetColumnSpan(this.horizontalLine4, 17);
            this.horizontalLine4.Location = new System.Drawing.Point(3, 471);
            this.horizontalLine4.Name = "horizontalLine4";
            this.horizontalLine4.Size = new System.Drawing.Size(1234, 2);
            this.horizontalLine4.TabIndex = 50;
            // 
            // horizontalLine3
            // 
            this.horizontalLine3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayout_pos.SetColumnSpan(this.horizontalLine3, 17);
            this.horizontalLine3.Location = new System.Drawing.Point(3, 130);
            this.horizontalLine3.Name = "horizontalLine3";
            this.horizontalLine3.Size = new System.Drawing.Size(1234, 2);
            this.horizontalLine3.TabIndex = 21;
            // 
            // horizontalLine1
            // 
            this.horizontalLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayout_pos.SetColumnSpan(this.horizontalLine1, 17);
            this.horizontalLine1.Location = new System.Drawing.Point(3, 29);
            this.horizontalLine1.Name = "horizontalLine1";
            this.horizontalLine1.Size = new System.Drawing.Size(1234, 2);
            this.horizontalLine1.TabIndex = 19;
            // 
            // horizontalLine2
            // 
            this.horizontalLine2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayout_pos.SetColumnSpan(this.horizontalLine2, 17);
            this.horizontalLine2.Location = new System.Drawing.Point(3, 65);
            this.horizontalLine2.Name = "horizontalLine2";
            this.horizontalLine2.Size = new System.Drawing.Size(1234, 2);
            this.horizontalLine2.TabIndex = 20;
            // 
            // btnLast
            // 
            this.btnLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLast.Location = new System.Drawing.Point(1159, 479);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(78, 25);
            this.btnLast.TabIndex = 31;
            this.btnLast.Text = ">>";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(934, 441);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 30);
            this.label8.TabIndex = 49;
            this.label8.Text = "Viso:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(859, 135);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 30);
            this.label20.TabIndex = 45;
            this.label20.Text = "Priemoka:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(1084, 135);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(69, 30);
            this.label19.TabIndex = 44;
            this.label19.Text = "Komp. suma:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(709, 135);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 30);
            this.label18.TabIndex = 43;
            this.label18.Text = "Rec. suma:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.tableLayout_pos.SetColumnSpan(this.label10, 2);
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(229, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 30);
            this.label10.TabIndex = 42;
            this.label10.Text = "Kompensacija:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblInitName
            // 
            this.lblInitName.AutoSize = true;
            this.tableLayout_pos.SetColumnSpan(this.lblInitName, 10);
            this.lblInitName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblInitName.Location = new System.Drawing.Point(3, 100);
            this.lblInitName.Name = "lblInitName";
            this.lblInitName.Size = new System.Drawing.Size(775, 30);
            this.lblInitName.TabIndex = 33;
            this.lblInitName.Text = "lblInitName";
            this.lblInitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbVatSize
            // 
            this.tbVatSize.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVatSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbVatSize.Location = new System.Drawing.Point(1159, 73);
            this.tbVatSize.Name = "tbVatSize";
            this.tbVatSize.ReadOnly = true;
            this.tbVatSize.Size = new System.Drawing.Size(78, 23);
            this.tbVatSize.TabIndex = 14;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(1084, 100);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 30);
            this.label15.TabIndex = 26;
            this.label15.Text = "Moka";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(934, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 30);
            this.label14.TabIndex = 25;
            this.label14.Text = "Kaina";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(784, 100);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 30);
            this.label13.TabIndex = 24;
            this.label13.Text = "Kiekis";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(1084, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(69, 30);
            this.label12.TabIndex = 23;
            this.label12.Text = "PVM %";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(934, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 30);
            this.label11.TabIndex = 22;
            this.label11.Text = "Nuolaida %";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.tableLayout_pos.SetColumnSpan(this.lblProductName, 10);
            this.lblProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblProductName.Location = new System.Drawing.Point(3, 70);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(775, 30);
            this.lblProductName.TabIndex = 32;
            this.lblProductName.Text = "lblProductName";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDiscPercent
            // 
            this.tbDiscPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDiscPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbDiscPercent.Location = new System.Drawing.Point(1009, 73);
            this.tbDiscPercent.Name = "tbDiscPercent";
            this.tbDiscPercent.ReadOnly = true;
            this.tbDiscPercent.Size = new System.Drawing.Size(69, 23);
            this.tbDiscPercent.TabIndex = 13;
            // 
            // tbPrice
            // 
            this.tbPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbPrice.Location = new System.Drawing.Point(1009, 103);
            this.tbPrice.Name = "tbPrice";
            this.tbPrice.ReadOnly = true;
            this.tbPrice.Size = new System.Drawing.Size(69, 23);
            this.tbPrice.TabIndex = 16;
            // 
            // tbQty
            // 
            this.tbQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbQty.Location = new System.Drawing.Point(859, 103);
            this.tbQty.Name = "tbQty";
            this.tbQty.ReadOnly = true;
            this.tbQty.Size = new System.Drawing.Size(69, 23);
            this.tbQty.TabIndex = 15;
            // 
            // tbSum
            // 
            this.tbSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbSum.Location = new System.Drawing.Point(1159, 103);
            this.tbSum.Name = "tbSum";
            this.tbSum.ReadOnly = true;
            this.tbSum.Size = new System.Drawing.Size(78, 23);
            this.tbSum.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 30);
            this.label9.TabIndex = 34;
            this.label9.Text = "Recepto nr.:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbRecipeNo
            // 
            this.tbRecipeNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecipeNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbRecipeNo.Location = new System.Drawing.Point(78, 138);
            this.tbRecipeNo.Name = "tbRecipeNo";
            this.tbRecipeNo.ReadOnly = true;
            this.tbRecipeNo.Size = new System.Drawing.Size(145, 23);
            this.tbRecipeNo.TabIndex = 18;
            // 
            // panel2Pos
            // 
            this.tableLayout_pos.SetColumnSpan(this.panel2Pos, 16);
            this.panel2Pos.Controls.Add(this.btnLabel);
            this.panel2Pos.Controls.Add(this.btnSelBarcode);
            this.panel2Pos.Controls.Add(this.btnRecipe);
            this.panel2Pos.Controls.Add(this.btnPrescriptionCheck);
            this.panel2Pos.Controls.Add(this.btnDiscount);
            this.panel2Pos.Controls.Add(this.btnCheque);
            this.panel2Pos.Controls.Add(this.btnPrep);
            this.panel2Pos.Controls.Add(this.btnPrice);
            this.panel2Pos.Controls.Add(this.btnService);
            this.panel2Pos.Controls.Add(this.btnCancel);
            this.panel2Pos.Controls.Add(this.edtBarcode);
            this.panel2Pos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2Pos.Location = new System.Drawing.Point(3, 37);
            this.panel2Pos.Name = "panel2Pos";
            this.panel2Pos.Size = new System.Drawing.Size(1234, 25);
            this.panel2Pos.TabIndex = 34;
            // 
            // btnLabel
            // 
            this.btnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLabel.Location = new System.Drawing.Point(631, 0);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(69, 24);
            this.btnLabel.TabIndex = 7;
            this.btnLabel.Text = "Inf. &Lapelis";
            this.btnLabel.UseVisualStyleBackColor = true;
            this.btnLabel.Click += new System.EventHandler(this.btnLabel_Click);
            // 
            // btnSelBarcode
            // 
            this.btnSelBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelBarcode.Location = new System.Drawing.Point(150, 0);
            this.btnSelBarcode.Name = "btnSelBarcode";
            this.btnSelBarcode.Size = new System.Drawing.Size(25, 24);
            this.btnSelBarcode.TabIndex = 2;
            this.btnSelBarcode.Text = "&...";
            this.btnSelBarcode.UseVisualStyleBackColor = true;
            this.btnSelBarcode.Click += new System.EventHandler(this.btnSelBarcode_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRecipe.Location = new System.Drawing.Point(256, 0);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(69, 24);
            this.btnRecipe.TabIndex = 4;
            this.btnRecipe.Text = "&Receptas";
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // btnPrescriptionCheck
            // 
            this.btnPrescriptionCheck.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrescriptionCheck.Location = new System.Drawing.Point(331, 0);
            this.btnPrescriptionCheck.Name = "btnPrescriptionCheck";
            this.btnPrescriptionCheck.Size = new System.Drawing.Size(69, 24);
            this.btnPrescriptionCheck.TabIndex = 8;
            this.btnPrescriptionCheck.Text = "Re&c. čekis";
            this.btnPrescriptionCheck.UseVisualStyleBackColor = true;
            this.btnPrescriptionCheck.Click += new System.EventHandler(this.btnPrescriptionCheck_Click);
            // 
            // btnDiscount
            // 
            this.btnDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDiscount.Location = new System.Drawing.Point(481, 0);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(69, 24);
            this.btnDiscount.TabIndex = 5;
            this.btnDiscount.Text = "&Nuolaida";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // btnCheque
            // 
            this.btnCheque.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheque.Location = new System.Drawing.Point(556, 0);
            this.btnCheque.Name = "btnCheque";
            this.btnCheque.Size = new System.Drawing.Size(69, 24);
            this.btnCheque.TabIndex = 6;
            this.btnCheque.Text = "&GSK čekis";
            this.btnCheque.UseVisualStyleBackColor = true;
            this.btnCheque.Click += new System.EventHandler(this.btnCheque_Click);
            // 
            // btnPrep
            // 
            this.btnPrep.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrep.Location = new System.Drawing.Point(406, 0);
            this.btnPrep.Name = "btnPrep";
            this.btnPrep.Size = new System.Drawing.Size(69, 24);
            this.btnPrep.TabIndex = 7;
            this.btnPrep.Text = "&Priemoka";
            this.btnPrep.UseVisualStyleBackColor = true;
            this.btnPrep.Click += new System.EventHandler(this.btnPrep_Click);
            // 
            // btnPrice
            // 
            this.btnPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrice.Location = new System.Drawing.Point(781, 0);
            this.btnPrice.Name = "btnPrice";
            this.btnPrice.Size = new System.Drawing.Size(69, 24);
            this.btnPrice.TabIndex = 9;
            this.btnPrice.Text = "Ka&ina";
            this.btnPrice.UseVisualStyleBackColor = true;
            this.btnPrice.Click += new System.EventHandler(this.btnPrice_Click);
            // 
            // btnService
            // 
            this.btnService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnService.Location = new System.Drawing.Point(181, 0);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(69, 24);
            this.btnService.TabIndex = 9;
            this.btnService.Text = "Pa&slauga";
            this.btnService.UseVisualStyleBackColor = true;
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(1162, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Trinti kvitą";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // edtBarcode
            // 
            this.edtBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.edtBarcode.BackColor = System.Drawing.SystemColors.HighlightText;
            this.edtBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.edtBarcode.Location = new System.Drawing.Point(4, 1);
            this.edtBarcode.Name = "edtBarcode";
            this.edtBarcode.Size = new System.Drawing.Size(140, 23);
            this.edtBarcode.TabIndex = 1;
            this.edtBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edtBarcode_KeyPress);
            // 
            // gvPosd
            // 
            this.gvPosd.AllowUserToAddRows = false;
            this.gvPosd.AllowUserToResizeColumns = false;
            this.gvPosd.AllowUserToResizeRows = false;
            this.gvPosd.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvPosd.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvPosd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvPosd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnDeleteLine,
            this.fmd_link,
            this.id,
            this.apply_insurance,
            this.have_recipe,
            this.symbol,
            this.Saveable,
            this.fmd_is_valid_for_sale,
            this.vatsize,
            this.barcode,
            this.barcodename_info,
            this.qty,
            this.price,
            this.discount,
            this.pricediscounted,
            this.sum,
            this.cheque_sum,
            this.cheque_sum_insurance,
            this.barcodename,
            this.recipeid,
            this.compensationsum,
            this.erecipe_no,
            this.erecipe_active});
            this.tableLayout_pos.SetColumnSpan(this.gvPosd, 16);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvPosd.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvPosd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvPosd.Location = new System.Drawing.Point(3, 168);
            this.gvPosd.MultiSelect = false;
            this.gvPosd.Name = "gvPosd";
            this.gvPosd.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvPosd.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvPosd.RowHeadersVisible = false;
            this.gvPosd.RowHeadersWidth = 62;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvPosd.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gvPosd.RowTemplate.Height = 45;
            this.gvPosd.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvPosd.Size = new System.Drawing.Size(1234, 201);
            this.gvPosd.StandardTab = true;
            this.gvPosd.TabIndex = 25;
            this.gvPosd.VirtualMode = true;
            this.gvPosd.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPosd_CellContentClick);
            this.gvPosd.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvPosd_CellMouseDown);
            this.gvPosd.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.gvPosd_ColumnAdded);
            this.gvPosd.CurrentCellChanged += new System.EventHandler(this.gvPosd_CurrentCellChanged);
            this.gvPosd.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvDelete_Click);
            this.gvPosd.MouseLeave += new System.EventHandler(this.gvPosd_MouseLeave);
            // 
            // btnDeleteLine
            // 
            this.btnDeleteLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.btnDeleteLine.DataPropertyName = "btnDeleteLine";
            this.btnDeleteLine.HeaderText = "";
            this.btnDeleteLine.Image = global::POS_display.Properties.Resources.delete;
            this.btnDeleteLine.MinimumWidth = 8;
            this.btnDeleteLine.Name = "btnDeleteLine";
            this.btnDeleteLine.ReadOnly = true;
            this.btnDeleteLine.Width = 8;
            // 
            // fmd_link
            // 
            this.fmd_link.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.fmd_link.DataPropertyName = "fmd_link";
            this.fmd_link.HeaderText = "FMD";
            this.fmd_link.MinimumWidth = 8;
            this.fmd_link.Name = "fmd_link";
            this.fmd_link.ReadOnly = true;
            this.fmd_link.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.fmd_link.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.fmd_link.Width = 58;
            // 
            // id
            // 
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.MinimumWidth = 8;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // apply_insurance
            // 
            this.apply_insurance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.apply_insurance.DataPropertyName = "apply_insurance";
            this.apply_insurance.FalseValue = "0";
            this.apply_insurance.HeaderText = "Taikyti draudimą";
            this.apply_insurance.MinimumWidth = 8;
            this.apply_insurance.Name = "apply_insurance";
            this.apply_insurance.ReadOnly = true;
            this.apply_insurance.TrueValue = "1";
            this.apply_insurance.Width = 96;
            // 
            // have_recipe
            // 
            this.have_recipe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.have_recipe.DataPropertyName = "have_recipe";
            this.have_recipe.FalseValue = "0";
            this.have_recipe.HeaderText = "Su receptu";
            this.have_recipe.MinimumWidth = 8;
            this.have_recipe.Name = "have_recipe";
            this.have_recipe.ReadOnly = true;
            this.have_recipe.TrueValue = "1";
            this.have_recipe.Width = 68;
            // 
            // symbol
            // 
            this.symbol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.symbol.DataPropertyName = "symbol";
            this.symbol.HeaderText = "R";
            this.symbol.MinimumWidth = 8;
            this.symbol.Name = "symbol";
            this.symbol.ReadOnly = true;
            this.symbol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.symbol.Width = 22;
            // 
            // Saveable
            // 
            this.Saveable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Saveable.DataPropertyName = "Saveable";
            this.Saveable.FalseValue = "false";
            this.Saveable.HeaderText = "S";
            this.Saveable.MinimumWidth = 8;
            this.Saveable.Name = "Saveable";
            this.Saveable.ReadOnly = true;
            this.Saveable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Saveable.TrueValue = "true";
            this.Saveable.Width = 21;
            // 
            // fmd_is_valid_for_sale
            // 
            this.fmd_is_valid_for_sale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.fmd_is_valid_for_sale.DataPropertyName = "fmd_is_valid_for_sale";
            this.fmd_is_valid_for_sale.HeaderText = "FMD";
            this.fmd_is_valid_for_sale.MinimumWidth = 8;
            this.fmd_is_valid_for_sale.Name = "fmd_is_valid_for_sale";
            this.fmd_is_valid_for_sale.ReadOnly = true;
            this.fmd_is_valid_for_sale.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.fmd_is_valid_for_sale.Width = 58;
            // 
            // vatsize
            // 
            this.vatsize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.vatsize.DataPropertyName = "vatsize";
            this.vatsize.HeaderText = "PVM";
            this.vatsize.MinimumWidth = 8;
            this.vatsize.Name = "vatsize";
            this.vatsize.ReadOnly = true;
            this.vatsize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.vatsize.Width = 39;
            // 
            // barcode
            // 
            this.barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.barcode.DataPropertyName = "barcode";
            this.barcode.HeaderText = "Barkodas";
            this.barcode.MinimumWidth = 8;
            this.barcode.Name = "barcode";
            this.barcode.ReadOnly = true;
            this.barcode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.barcode.Width = 66;
            // 
            // barcodename_info
            // 
            this.barcodename_info.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.barcodename_info.DataPropertyName = "barcodename_info";
            this.barcodename_info.HeaderText = "Prekės pavadinimas";
            this.barcodename_info.MinimumWidth = 8;
            this.barcodename_info.Name = "barcodename_info";
            this.barcodename_info.ReadOnly = true;
            this.barcodename_info.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.qty.DataPropertyName = "qty";
            this.qty.HeaderText = "Kiekis";
            this.qty.MinimumWidth = 8;
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.qty.Width = 47;
            // 
            // price
            // 
            this.price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.price.DataPropertyName = "price";
            this.price.HeaderText = "Kaina";
            this.price.MinimumWidth = 8;
            this.price.Name = "price";
            this.price.ReadOnly = true;
            this.price.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.price.Width = 45;
            // 
            // discount
            // 
            this.discount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.discount.DataPropertyName = "discount";
            this.discount.HeaderText = "Nuolaida";
            this.discount.MinimumWidth = 8;
            this.discount.Name = "discount";
            this.discount.ReadOnly = true;
            this.discount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.discount.Width = 63;
            // 
            // pricediscounted
            // 
            this.pricediscounted.DataPropertyName = "pricediscounted";
            this.pricediscounted.HeaderText = "Kaina su nuolaida";
            this.pricediscounted.MinimumWidth = 8;
            this.pricediscounted.Name = "pricediscounted";
            this.pricediscounted.ReadOnly = true;
            this.pricediscounted.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.pricediscounted.Width = 63;
            // 
            // sum
            // 
            this.sum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.sum.DataPropertyName = "sum";
            this.sum.HeaderText = "Suma";
            this.sum.MinimumWidth = 8;
            this.sum.Name = "sum";
            this.sum.ReadOnly = true;
            this.sum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sum.Width = 44;
            // 
            // cheque_sum
            // 
            this.cheque_sum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cheque_sum.DataPropertyName = "cheque_sum";
            this.cheque_sum.HeaderText = "GSK suma";
            this.cheque_sum.MinimumWidth = 8;
            this.cheque_sum.Name = "cheque_sum";
            this.cheque_sum.ReadOnly = true;
            this.cheque_sum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cheque_sum.Visible = false;
            // 
            // cheque_sum_insurance
            // 
            this.cheque_sum_insurance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.cheque_sum_insurance.DataPropertyName = "cheque_sum_insurance";
            this.cheque_sum_insurance.HeaderText = "Draudimo suma";
            this.cheque_sum_insurance.MinimumWidth = 8;
            this.cheque_sum_insurance.Name = "cheque_sum_insurance";
            this.cheque_sum_insurance.ReadOnly = true;
            this.cheque_sum_insurance.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cheque_sum_insurance.Width = 89;
            // 
            // barcodename
            // 
            this.barcodename.DataPropertyName = "barcodename";
            this.barcodename.HeaderText = "barcodename";
            this.barcodename.MinimumWidth = 8;
            this.barcodename.Name = "barcodename";
            this.barcodename.ReadOnly = true;
            this.barcodename.Visible = false;
            this.barcodename.Width = 150;
            // 
            // recipeid
            // 
            this.recipeid.DataPropertyName = "recipeid";
            this.recipeid.HeaderText = "recipeid";
            this.recipeid.MinimumWidth = 8;
            this.recipeid.Name = "recipeid";
            this.recipeid.ReadOnly = true;
            this.recipeid.Visible = false;
            this.recipeid.Width = 150;
            // 
            // compensationsum
            // 
            this.compensationsum.DataPropertyName = "compensationsum";
            this.compensationsum.HeaderText = "compensationsum";
            this.compensationsum.MinimumWidth = 8;
            this.compensationsum.Name = "compensationsum";
            this.compensationsum.ReadOnly = true;
            this.compensationsum.Visible = false;
            this.compensationsum.Width = 150;
            // 
            // erecipe_no
            // 
            this.erecipe_no.DataPropertyName = "erecipe_no";
            this.erecipe_no.HeaderText = "erecipe_no";
            this.erecipe_no.MinimumWidth = 8;
            this.erecipe_no.Name = "erecipe_no";
            this.erecipe_no.ReadOnly = true;
            this.erecipe_no.Visible = false;
            this.erecipe_no.Width = 150;
            // 
            // erecipe_active
            // 
            this.erecipe_active.DataPropertyName = "erecipe_active";
            this.erecipe_active.HeaderText = "erecipe_active";
            this.erecipe_active.MinimumWidth = 8;
            this.erecipe_active.Name = "erecipe_active";
            this.erecipe_active.ReadOnly = true;
            this.erecipe_active.Visible = false;
            this.erecipe_active.Width = 150;
            // 
            // tbChequeSum
            // 
            this.tbChequeSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChequeSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbChequeSum.Location = new System.Drawing.Point(1084, 444);
            this.tbChequeSum.Name = "tbChequeSum";
            this.tbChequeSum.ReadOnly = true;
            this.tbChequeSum.Size = new System.Drawing.Size(69, 23);
            this.tbChequeSum.TabIndex = 27;
            // 
            // tbTotalSum
            // 
            this.tbTotalSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTotalSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbTotalSum.Location = new System.Drawing.Point(1009, 444);
            this.tbTotalSum.Name = "tbTotalSum";
            this.tbTotalSum.ReadOnly = true;
            this.tbTotalSum.Size = new System.Drawing.Size(69, 23);
            this.tbTotalSum.TabIndex = 26;
            // 
            // lblRecordsStatus
            // 
            this.lblRecordsStatus.AutoSize = true;
            this.tableLayout_pos.SetColumnSpan(this.lblRecordsStatus, 2);
            this.lblRecordsStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordsStatus.Location = new System.Drawing.Point(559, 476);
            this.lblRecordsStatus.Name = "lblRecordsStatus";
            this.lblRecordsStatus.Size = new System.Drawing.Size(144, 31);
            this.lblRecordsStatus.TabIndex = 51;
            this.lblRecordsStatus.Text = "1 / 1";
            this.lblRecordsStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelNavigation
            // 
            this.tableLayout_pos.SetColumnSpan(this.panelNavigation, 16);
            this.panelNavigation.Controls.Add(this.panel1);
            this.panelNavigation.Controls.Add(this.lblInfo);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavigation.Location = new System.Drawing.Point(3, 3);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(1234, 23);
            this.panelNavigation.TabIndex = 52;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnVouchers);
            this.panel1.Controls.Add(this.btnStockInfo);
            this.panel1.Controls.Add(this.chb2display);
            this.panel1.Controls.Add(this.btnDrugPrice);
            this.panel1.Controls.Add(this.btnERecipe);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(749, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(485, 23);
            this.panel1.TabIndex = 57;
            // 
            // btnVouchers
            // 
            this.btnVouchers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVouchers.Location = new System.Drawing.Point(5, 0);
            this.btnVouchers.Name = "btnVouchers";
            this.btnVouchers.Size = new System.Drawing.Size(92, 23);
            this.btnVouchers.TabIndex = 56;
            this.btnVouchers.Text = "Akcijos";
            this.btnVouchers.UseVisualStyleBackColor = true;
            this.btnVouchers.Visible = false;
            this.btnVouchers.Click += new System.EventHandler(this.btnVouchers_Click);
            // 
            // btnStockInfo
            // 
            this.btnStockInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockInfo.Location = new System.Drawing.Point(205, 0);
            this.btnStockInfo.Name = "btnStockInfo";
            this.btnStockInfo.Size = new System.Drawing.Size(92, 23);
            this.btnStockInfo.TabIndex = 12;
            this.btnStockInfo.Text = "Pr&ekių info";
            this.btnStockInfo.UseVisualStyleBackColor = true;
            this.btnStockInfo.Click += new System.EventHandler(this.btnStockInfo_Click);
            // 
            // chb2display
            // 
            this.chb2display.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chb2display.AutoSize = true;
            this.chb2display.Location = new System.Drawing.Point(404, 3);
            this.chb2display.Name = "chb2display";
            this.chb2display.Size = new System.Drawing.Size(74, 17);
            this.chb2display.TabIndex = 15;
            this.chb2display.Text = "Ekranas 2";
            this.chb2display.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chb2display.UseVisualStyleBackColor = true;
            this.chb2display.CheckedChanged += new System.EventHandler(this.chb2display_CheckedChanged);
            // 
            // btnDrugPrice
            // 
            this.btnDrugPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrugPrice.Location = new System.Drawing.Point(305, 0);
            this.btnDrugPrice.Name = "btnDrugPrice";
            this.btnDrugPrice.Size = new System.Drawing.Size(92, 23);
            this.btnDrugPrice.TabIndex = 8;
            this.btnDrugPrice.Text = "&Vaistų kaina";
            this.btnDrugPrice.UseVisualStyleBackColor = true;
            this.btnDrugPrice.Click += new System.EventHandler(this.btnDrugPrice_Click);
            // 
            // btnERecipe
            // 
            this.btnERecipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnERecipe.Enabled = false;
            this.btnERecipe.Location = new System.Drawing.Point(105, 0);
            this.btnERecipe.Name = "btnERecipe";
            this.btnERecipe.Size = new System.Drawing.Size(92, 23);
            this.btnERecipe.TabIndex = 53;
            this.btnERecipe.Text = "E - sveika&ta";
            this.btnERecipe.UseVisualStyleBackColor = true;
            this.btnERecipe.Click += new System.EventHandler(this.btnERecipe_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.BackColor = System.Drawing.SystemColors.Control;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(748, 23);
            this.lblInfo.TabIndex = 18;
            this.lblInfo.Text = "lblInfo";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo.TextChanged += new System.EventHandler(this.lblInfo_TextChanged);
            // 
            // tbCompCode
            // 
            this.tbCompCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCompCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCompCode.Location = new System.Drawing.Point(334, 138);
            this.tbCompCode.Name = "tbCompCode";
            this.tbCompCode.ReadOnly = true;
            this.tbCompCode.Size = new System.Drawing.Size(69, 23);
            this.tbCompCode.TabIndex = 19;
            // 
            // tbCompPercent
            // 
            this.tbCompPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCompPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCompPercent.Location = new System.Drawing.Point(409, 138);
            this.tbCompPercent.Name = "tbCompPercent";
            this.tbCompPercent.ReadOnly = true;
            this.tbCompPercent.Size = new System.Drawing.Size(69, 23);
            this.tbCompPercent.TabIndex = 20;
            // 
            // tbRecCompSum
            // 
            this.tbRecCompSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecCompSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbRecCompSum.Location = new System.Drawing.Point(1159, 138);
            this.tbRecCompSum.Name = "tbRecCompSum";
            this.tbRecCompSum.ReadOnly = true;
            this.tbRecCompSum.Size = new System.Drawing.Size(78, 23);
            this.tbRecCompSum.TabIndex = 22;
            // 
            // tbEndSum
            // 
            this.tbEndSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEndSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbEndSum.Location = new System.Drawing.Point(1009, 138);
            this.tbEndSum.Name = "tbEndSum";
            this.tbEndSum.ReadOnly = true;
            this.tbEndSum.Size = new System.Drawing.Size(69, 23);
            this.tbEndSum.TabIndex = 24;
            // 
            // tbPaySum
            // 
            this.tbPaySum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPaySum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbPaySum.Location = new System.Drawing.Point(934, 138);
            this.tbPaySum.Name = "tbPaySum";
            this.tbPaySum.ReadOnly = true;
            this.tbPaySum.Size = new System.Drawing.Size(69, 23);
            this.tbPaySum.TabIndex = 23;
            // 
            // tbRecTotalSum
            // 
            this.tbRecTotalSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecTotalSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbRecTotalSum.Location = new System.Drawing.Point(784, 138);
            this.tbRecTotalSum.Name = "tbRecTotalSum";
            this.tbRecTotalSum.ReadOnly = true;
            this.tbRecTotalSum.Size = new System.Drawing.Size(69, 23);
            this.tbRecTotalSum.TabIndex = 21;
            // 
            // tbCheckValue
            // 
            this.tbCheckValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCheckValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCheckValue.Location = new System.Drawing.Point(859, 444);
            this.tbCheckValue.Name = "tbCheckValue";
            this.tbCheckValue.ReadOnly = true;
            this.tbCheckValue.Size = new System.Drawing.Size(69, 23);
            this.tbCheckValue.TabIndex = 54;
            // 
            // gbRecommendations
            // 
            this.tableLayout_pos.SetColumnSpan(this.gbRecommendations, 16);
            this.gbRecommendations.Controls.Add(this.loaderUserControl);
            this.gbRecommendations.Controls.Add(this.gvRecommandations);
            this.gbRecommendations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRecommendations.Location = new System.Drawing.Point(3, 375);
            this.gbRecommendations.Name = "gbRecommendations";
            this.gbRecommendations.Size = new System.Drawing.Size(1234, 63);
            this.gbRecommendations.TabIndex = 56;
            this.gbRecommendations.TabStop = false;
            this.gbRecommendations.Text = "Prekių rekomendacijos klientui";
            // 
            // loaderUserControl
            // 
            this.loaderUserControl.BackColor = System.Drawing.Color.Transparent;
            this.loaderUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loaderUserControl.IsLoading = false;
            this.loaderUserControl.Location = new System.Drawing.Point(3, 16);
            this.loaderUserControl.Name = "loaderUserControl";
            this.loaderUserControl.Size = new System.Drawing.Size(1228, 44);
            this.loaderUserControl.SpinnerLength = 80;
            this.loaderUserControl.TabIndex = 1;
            // 
            // gvRecommandations
            // 
            this.gvRecommandations.AllowUserToAddRows = false;
            this.gvRecommandations.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvRecommandations.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.gvRecommandations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvRecommandations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colPrice,
            this.colDiscount});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvRecommandations.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvRecommandations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvRecommandations.Location = new System.Drawing.Point(3, 16);
            this.gvRecommandations.Name = "gvRecommandations";
            this.gvRecommandations.ReadOnly = true;
            this.gvRecommandations.RowHeadersVisible = false;
            this.gvRecommandations.RowHeadersWidth = 62;
            this.gvRecommandations.Size = new System.Drawing.Size(1228, 44);
            this.gvRecommandations.TabIndex = 0;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.FillWeight = 60F;
            this.colName.HeaderText = "Prekės pavadinimas";
            this.colName.MinimumWidth = 8;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colPrice
            // 
            this.colPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPrice.DataPropertyName = "Price";
            this.colPrice.FillWeight = 20F;
            this.colPrice.HeaderText = "Galutinė kaina (EUR)";
            this.colPrice.MinimumWidth = 8;
            this.colPrice.Name = "colPrice";
            this.colPrice.ReadOnly = true;
            // 
            // colDiscount
            // 
            this.colDiscount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDiscount.DataPropertyName = "Discount";
            this.colDiscount.FillWeight = 20F;
            this.colDiscount.HeaderText = "Nuolaida (%)";
            this.colDiscount.MinimumWidth = 8;
            this.colDiscount.Name = "colDiscount";
            this.colDiscount.ReadOnly = true;
            // 
            // lblCRMDataLoadStatus
            // 
            this.tableLayout_pos.SetColumnSpan(this.lblCRMDataLoadStatus, 2);
            this.lblCRMDataLoadStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCRMDataLoadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblCRMDataLoadStatus.Location = new System.Drawing.Point(78, 441);
            this.lblCRMDataLoadStatus.Name = "lblCRMDataLoadStatus";
            this.lblCRMDataLoadStatus.Size = new System.Drawing.Size(175, 30);
            this.lblCRMDataLoadStatus.TabIndex = 57;
            this.lblCRMDataLoadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCRMDataReload
            // 
            this.btnCRMDataReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCRMDataReload.Location = new System.Drawing.Point(3, 444);
            this.btnCRMDataReload.Name = "btnCRMDataReload";
            this.btnCRMDataReload.Size = new System.Drawing.Size(69, 24);
            this.btnCRMDataReload.TabIndex = 58;
            this.btnCRMDataReload.Text = "Perkrauti";
            this.btnCRMDataReload.UseVisualStyleBackColor = true;
            this.btnCRMDataReload.Click += new System.EventHandler(this.btnCRMDataReload_Click);
            // 
            // lblDisplay2
            // 
            this.lblDisplay2.AutoSize = true;
            this.tableLayout_pos.SetColumnSpan(this.lblDisplay2, 4);
            this.lblDisplay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDisplay2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay2.ForeColor = System.Drawing.Color.Red;
            this.lblDisplay2.Location = new System.Drawing.Point(259, 441);
            this.lblDisplay2.Name = "lblDisplay2";
            this.lblDisplay2.Size = new System.Drawing.Size(294, 30);
            this.lblDisplay2.TabIndex = 59;
            this.lblDisplay2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOnHandQty
            // 
            this.lblOnHandQty.AutoSize = true;
            this.lblOnHandQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOnHandQty.Location = new System.Drawing.Point(784, 70);
            this.lblOnHandQty.Name = "lblOnHandQty";
            this.lblOnHandQty.Size = new System.Drawing.Size(69, 30);
            this.lblOnHandQty.TabIndex = 60;
            this.lblOnHandQty.Text = "Likutis vaist.";
            this.lblOnHandQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOnHandQty
            // 
            this.tbOnHandQty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOnHandQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tbOnHandQty.Location = new System.Drawing.Point(859, 73);
            this.tbOnHandQty.Name = "tbOnHandQty";
            this.tbOnHandQty.ReadOnly = true;
            this.tbOnHandQty.Size = new System.Drawing.Size(69, 23);
            this.tbOnHandQty.TabIndex = 61;
            // 
            // btnResetPos
            // 
            this.btnResetPos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetPos.Location = new System.Drawing.Point(1163, 33);
            this.btnResetPos.Name = "btnResetPos";
            this.btnResetPos.Size = new System.Drawing.Size(84, 24);
            this.btnResetPos.TabIndex = 11;
            this.btnResetPos.Text = "Anuliuoti k.";
            this.btnResetPos.UseVisualStyleBackColor = true;
            this.btnResetPos.Click += new System.EventHandler(this.btnResetPos_Click);
            // 
            // btnEcrRep
            // 
            this.btnEcrRep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEcrRep.Location = new System.Drawing.Point(983, 33);
            this.btnEcrRep.Name = "btnEcrRep";
            this.btnEcrRep.Size = new System.Drawing.Size(84, 24);
            this.btnEcrRep.TabIndex = 10;
            this.btnEcrRep.Text = "Kasos &op.";
            this.btnEcrRep.UseVisualStyleBackColor = true;
            this.btnEcrRep.Click += new System.EventHandler(this.btnEcrRep_Click);
            // 
            // chbFiscal
            // 
            this.chbFiscal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chbFiscal.AutoSize = true;
            this.chbFiscal.Location = new System.Drawing.Point(945, 33);
            this.chbFiscal.Name = "chbFiscal";
            this.chbFiscal.Size = new System.Drawing.Size(32, 24);
            this.chbFiscal.TabIndex = 9;
            this.chbFiscal.Text = "F";
            this.chbFiscal.UseVisualStyleBackColor = true;
            this.chbFiscal.CheckedChanged += new System.EventHandler(this.chbFiscal_CheckedChanged);
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.AutoSize = true;
            this.tableLayout_poswrap.SetColumnSpan(this.lblInfo2, 3);
            this.lblInfo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblInfo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(38)))), ((int)(((byte)(100)))));
            this.lblInfo2.Location = new System.Drawing.Point(753, 60);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(314, 30);
            this.lblInfo2.TabIndex = 33;
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnHelp
            // 
            this.btnHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHelp.Location = new System.Drawing.Point(1163, 93);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(84, 24);
            this.btnHelp.TabIndex = 35;
            this.btnHelp.Text = "Pagal&ba";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnPersonalPharmacist
            // 
            this.btnPersonalPharmacist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPersonalPharmacist.Location = new System.Drawing.Point(468, 93);
            this.btnPersonalPharmacist.Name = "btnPersonalPharmacist";
            this.btnPersonalPharmacist.Size = new System.Drawing.Size(149, 24);
            this.btnPersonalPharmacist.TabIndex = 61;
            this.btnPersonalPharmacist.Text = "Asm. Vaistininkas";
            this.btnPersonalPharmacist.UseVisualStyleBackColor = true;
            this.btnPersonalPharmacist.Visible = false;
            this.btnPersonalPharmacist.Click += new System.EventHandler(this.btnPersonalPharmacist_Click);
            // 
            // btnClientSearch
            // 
            this.tableLayout_poswrap.SetColumnSpan(this.btnClientSearch, 2);
            this.btnClientSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClientSearch.Location = new System.Drawing.Point(623, 93);
            this.btnClientSearch.Name = "btnClientSearch";
            this.btnClientSearch.Size = new System.Drawing.Size(124, 24);
            this.btnClientSearch.TabIndex = 62;
            this.btnClientSearch.Text = "Klientų paieška";
            this.btnClientSearch.UseVisualStyleBackColor = true;
            this.btnClientSearch.Click += new System.EventHandler(this.btnClientSearch_Click);
            // 
            // btnHomeMode
            // 
            this.tableLayout_poswrap.SetColumnSpan(this.btnHomeMode, 2);
            this.btnHomeMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHomeMode.Location = new System.Drawing.Point(623, 63);
            this.btnHomeMode.Name = "btnHomeMode";
            this.btnHomeMode.Size = new System.Drawing.Size(124, 24);
            this.btnHomeMode.TabIndex = 63;
            this.btnHomeMode.Text = "Užsak. į namus";
            this.btnHomeMode.UseVisualStyleBackColor = true;
            this.btnHomeMode.Click += new System.EventHandler(this.btnHomeMode_Click);
            // 
            // lblUser
            // 
            this.lblUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUser.AutoSize = true;
            this.tableLayout_poswrap.SetColumnSpan(this.lblUser, 4);
            this.lblUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblUser.Location = new System.Drawing.Point(753, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(404, 30);
            this.lblUser.TabIndex = 27;
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnAdvancePayment
            // 
            this.btnAdvancePayment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvancePayment.Location = new System.Drawing.Point(623, 33);
            this.btnAdvancePayment.Name = "btnAdvancePayment";
            this.btnAdvancePayment.Size = new System.Drawing.Size(84, 24);
            this.btnAdvancePayment.TabIndex = 34;
            this.btnAdvancePayment.Text = "Avansas";
            this.btnAdvancePayment.UseVisualStyleBackColor = true;
            this.btnAdvancePayment.Click += new System.EventHandler(this.btnAdvancePayment_Click);
            // 
            // btnWoltMode
            // 
            this.btnWoltMode.Location = new System.Drawing.Point(623, 3);
            this.btnWoltMode.Name = "btnWoltMode";
            this.btnWoltMode.Size = new System.Drawing.Size(75, 23);
            this.btnWoltMode.TabIndex = 64;
            this.btnWoltMode.Text = "Wolt";
            this.btnWoltMode.UseVisualStyleBackColor = true;
            this.btnWoltMode.Click += new System.EventHandler(this.btnWolt_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(1073, 63);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(84, 24);
            this.btnTest.TabIndex = 34;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // Display1View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1260, 687);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.poswrap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Display1View";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.display1_Closing);
            this.Load += new System.EventHandler(this.display1_Load);
            this.Shown += new System.EventHandler(this.Display1View_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.display1_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.dgvContextMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.poswrap.ResumeLayout(false);
            this.poswrap.PerformLayout();
            this.tableLayout_poswrap.ResumeLayout(false);
            this.tableLayout_poswrap.PerformLayout();
            this.pos.ResumeLayout(false);
            this.pos.PerformLayout();
            this.tableLayout_pos.ResumeLayout(false);
            this.tableLayout_pos.PerformLayout();
            this.panel2Pos.ResumeLayout(false);
            this.panel2Pos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvPosd)).EndInit();
            this.panelNavigation.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbRecommendations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvRecommandations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel poswrap;
        private System.Windows.Forms.TableLayoutPanel tableLayout_poswrap;
        private System.Windows.Forms.Button btnLogOff;
        private System.Windows.Forms.Button btnDrugPrice;
        private System.Windows.Forms.CheckBox chbFiscal;
        private System.Windows.Forms.Button btnEcrRep;
        private System.Windows.Forms.Button btnResetPos;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Button btnCard;
        private CheckBox chb2display;
        private Label lblUser;
        private Panel pos;
        private TableLayoutPanel tableLayout_pos;
        private Button btnSelBarcode;
        private Button btnStockInfo;
        private Button btnCancel;
        private Button btnService;
        private Button btnPrice;
        private Button btnPrep;
        private Button btnCheque;
        private Button btnDiscount;
        private Button btnRecipe;
        private TextBox edtBarcode;
        private Label lblInfo;
        private HorizontalLine horizontalLine1;
        private HorizontalLine horizontalLine2;
        private HorizontalLine horizontalLine3;
        private TextBox tbDiscPercent;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;
        private TextBox tbQty;
        private TextBox tbPrice;
        private TextBox tbSum;
        private TextBox tbVatSize;
        private Label lblInitName;
        private Label lblProductName;
        private Label label9;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label10;
        private TextBox tbRecipeNo;
        private TextBox tbEndSum;
        private TextBox tbPaySum;
        private TextBox tbRecTotalSum;
        private TextBox tbCompPercent;
        private TextBox tbCompCode;
        private TextBox tbRecCompSum;
        private DataGridView gvPosd;
        private DataGridView gvRecommandations;
        private Label label8;
        private TextBox tbChequeSum;
        private TextBox tbTotalSum;
        private Label lblRecordsStatus;
        private Button btnNext;
        private Button btnPrevious;
        private Button btnFirst;
        private Button btnLast;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Label lblInfo2;
        private Button btnPrescriptionCheck;
        private Panel panel2Pos;
        private Panel panelNavigation;
        private HorizontalLine horizontalLine4;
        private Button btnERecipe;
        private Label label16;
        private TextBox tbCheckValue;
        private Button btnAdvancePayment;
        private Button btnTest;
        private ContextMenuStrip dgvContextMenu;
        private ToolStripMenuItem kopijuotiToolStripMenuItem;
        private Button btnHelp;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private ToolStripStatusLabel lblPurpose;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripStatusLabel lblForecast;
        private Timer systemTimer;
        private Button btnSettings;
        private ToolStripMenuItem adminToolStripMenuItem;
        private TextBox tbInsuranceSum;
        private Button btnInsurance;
        private Button btnKAS;
        private Button btnVouchers;
        private Button btnPayment;
        private Button btnCheckBalance;
        private Label lblRestSum;
        private TextBox tbRestSum;
        private Button btnLabel;
        private Button btnFMD;
        private Button btnCreateBENUclient;
        private Button btnDonation;
        private Button btnEshop;
        private Label LblMarketingConsent;
        private Panel panel1;
        private GroupBox gbRecommendations;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colDiscount;
        private Helpers.LoaderUserControl loaderUserControl;
        private Button btnPersonalPharmacist;
        private Label lblCRMDataLoadStatus;
        private Button btnCRMDataReload;
        private Button btnClientSearch;
        private ToolStripStatusLabel tsslUserPrioritiesRatio;
		private ToolStripStatusLabel tsslSaveable;
		private DataGridViewImageColumn btnDeleteLine;
		private DataGridViewLinkColumn fmd_link;
		private DataGridViewTextBoxColumn id;
		private DataGridViewCheckBoxColumn apply_insurance;
		private DataGridViewCheckBoxColumn have_recipe;
		private DataGridViewTextBoxColumn symbol;
		private DataGridViewCheckBoxColumn Saveable;
		private DataGridViewTextBoxColumn fmd_is_valid_for_sale;
		private DataGridViewTextBoxColumn vatsize;
		private DataGridViewTextBoxColumn barcode;
		private DataGridViewTextBoxColumn barcodename_info;
		private DataGridViewTextBoxColumn qty;
		private DataGridViewTextBoxColumn price;
		private DataGridViewTextBoxColumn discount;
		private DataGridViewTextBoxColumn pricediscounted;
		private DataGridViewTextBoxColumn sum;
		private DataGridViewTextBoxColumn cheque_sum;
		private DataGridViewTextBoxColumn cheque_sum_insurance;
		private DataGridViewTextBoxColumn barcodename;
		private DataGridViewTextBoxColumn recipeid;
		private DataGridViewTextBoxColumn compensationsum;
		private DataGridViewTextBoxColumn erecipe_no;
		private DataGridViewTextBoxColumn erecipe_active;
        private Label lblDisplay2;
        private Label lblOnHandQty;
        private TextBox tbOnHandQty;
        private Button btnHomeMode;
        private Button btnWoltMode;
    }
}