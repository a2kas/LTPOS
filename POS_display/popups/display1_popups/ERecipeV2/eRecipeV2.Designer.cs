using POS_display.Models.Recipe;
using System.Collections.Generic;
using System.Windows.Forms;
namespace POS_display
{
    partial class eRecipeV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eRecipeV2));
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblPractitioner = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.tabPanel1 = new System.Windows.Forms.TabControl();
            this.tabPatient = new System.Windows.Forms.TabPage();
            this.tlpPatient = new System.Windows.Forms.TableLayoutPanel();
            this.ehRecipeList = new System.Windows.Forms.Integration.ElementHost();
            this.ehDispenseListPatient = new System.Windows.Forms.Integration.ElementHost();
            this.lblRepresented = new System.Windows.Forms.Label();
            this.tbAge = new System.Windows.Forms.TextBox();
            this.tbSurname = new System.Windows.Forms.TextBox();
            this.tbBirthDate = new System.Windows.Forms.TextBox();
            this.tbPersonalCode = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.cmbRelated = new System.Windows.Forms.ComboBox();
            this.cmbRepresented = new System.Windows.Forms.ComboBox();
            this.tbPatientName = new System.Windows.Forms.TextBox();
            this.gbType = new System.Windows.Forms.GroupBox();
            this.rbActive = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rtbAllergies = new System.Windows.Forms.RichTextBox();
            this.ehRecipeListNavigation = new System.Windows.Forms.Integration.ElementHost();
            this.lblCompensationApplies = new System.Windows.Forms.Label();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.btnGetRecipePdf = new System.Windows.Forms.Button();
            this.btnGetDispensePdf = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.btnSell = new System.Windows.Forms.Button();
            this.btnPrepaymentInfo = new System.Windows.Forms.Button();
            this.btnDispensesInfo = new System.Windows.Forms.Button();
            this.tabRecipes = new System.Windows.Forms.TabPage();
            this.tlpRecipes = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDateTo = new System.Windows.Forms.DateTimePicker();
            this.dtpDateFrom = new System.Windows.Forms.DateTimePicker();
            this.btnFindObsolete = new System.Windows.Forms.Button();
            this.label26 = new System.Windows.Forms.Label();
            this.cmbConfirmed = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.cmbDocStatus = new System.Windows.Forms.ComboBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.btnDispFindDispense = new System.Windows.Forms.Button();
            this.ehDispenseList = new System.Windows.Forms.Integration.ElementHost();
            this.cbNarc = new System.Windows.Forms.CheckBox();
            this.tabVaccination = new System.Windows.Forms.TabPage();
            this.vaccineUserControl = new POS_display.Popups.display1_popups.ERecipe.VaccineUserControlV2();
            this.tabVaccinationSelection = new System.Windows.Forms.TabPage();
            this.tlVaccines = new System.Windows.Forms.TableLayoutPanel();
            this.ehVaccineList = new System.Windows.Forms.Integration.ElementHost();
            this.cmbVaccineStatus = new System.Windows.Forms.ComboBox();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.lblDocs = new System.Windows.Forms.Label();
            this.lblDocType = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblDocStatus = new System.Windows.Forms.Label();
            this.btnFindVaccine = new System.Windows.Forms.Button();
            this.lblVaccineFrom = new System.Windows.Forms.Label();
            this.lblVaccineTo = new System.Windows.Forms.Label();
            this.cmbVaccineDoc = new System.Windows.Forms.ComboBox();
            this.cmbVaccineDocStatus = new System.Windows.Forms.ComboBox();
            this.dtpVaccineFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpVaccineTo = new System.Windows.Forms.DateTimePicker();
            this.ehRecipe = new System.Windows.Forms.Integration.ElementHost();
            this.btnCancelTask = new System.Windows.Forms.Button();
            this.btnPaperRecipe = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.tabPanel1.SuspendLayout();
            this.tabPatient.SuspendLayout();
            this.tlpPatient.SuspendLayout();
            this.gbType.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.tabRecipes.SuspendLayout();
            this.tlpRecipes.SuspendLayout();
            this.tabVaccination.SuspendLayout();
            this.tabVaccinationSelection.SuspendLayout();
            this.tlVaccines.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(1734, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(142, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4});
            this.statusStrip1.Location = new System.Drawing.Point(8, 1126);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1880, 32);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Green;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(75, 25);
            this.toolStripStatusLabel1.Text = "Aktyvus";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 25);
            this.toolStripStatusLabel2.Text = "Išduotas";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.Blue;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(107, 25);
            this.toolStripStatusLabel3.Text = "Rezervuotas";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(213, 25);
            this.toolStripStatusLabel4.Text = "Sustabdytas / Panaikintas";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 255F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.lblSystem, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblPractitioner, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ehRecipe, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCancelTask, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1880, 1148);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblSystem
            // 
            this.lblSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSystem.AutoSize = true;
            this.lblSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblSystem.Location = new System.Drawing.Point(4, 0);
            this.lblSystem.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(1258, 46);
            this.lblSystem.TabIndex = 145;
            this.lblSystem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPractitioner
            // 
            this.lblPractitioner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPractitioner.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPractitioner, 2);
            this.lblPractitioner.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblPractitioner.Location = new System.Drawing.Point(1270, 0);
            this.lblPractitioner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPractitioner.Name = "lblPractitioner";
            this.lblPractitioner.Size = new System.Drawing.Size(411, 46);
            this.lblPractitioner.TabIndex = 58;
            this.lblPractitioner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pnlSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 59);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1258, 1046);
            this.panel1.TabIndex = 92;
            this.panel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.ucRecipeEdit_ControlRemoved);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.tabPanel1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1254, 1042);
            this.pnlSearch.TabIndex = 139;
            // 
            // tabPanel1
            // 
            this.tabPanel1.Controls.Add(this.tabPatient);
            this.tabPanel1.Controls.Add(this.tabRecipes);
            this.tabPanel1.Controls.Add(this.tabVaccination);
            this.tabPanel1.Controls.Add(this.tabVaccinationSelection);
            this.tabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPanel1.Location = new System.Drawing.Point(0, 0);
            this.tabPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPanel1.Name = "tabPanel1";
            this.tabPanel1.SelectedIndex = 0;
            this.tabPanel1.Size = new System.Drawing.Size(1254, 1042);
            this.tabPanel1.TabIndex = 140;
            this.tabPanel1.Tag = "";
            this.tabPanel1.SelectedIndexChanged += new System.EventHandler(this.tabPanel1_SelectedIndexChanged);
            // 
            // tabPatient
            // 
            this.tabPatient.Controls.Add(this.tlpPatient);
            this.tabPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tabPatient.Location = new System.Drawing.Point(4, 29);
            this.tabPatient.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPatient.Name = "tabPatient";
            this.tabPatient.Size = new System.Drawing.Size(1246, 1009);
            this.tabPatient.TabIndex = 0;
            this.tabPatient.Text = "E - receptų išdavimas";
            this.tabPatient.UseVisualStyleBackColor = true;
            // 
            // tlpPatient
            // 
            this.tlpPatient.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tlpPatient.ColumnCount = 7;
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpPatient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPatient.Controls.Add(this.ehRecipeList, 0, 5);
            this.tlpPatient.Controls.Add(this.ehDispenseListPatient, 0, 7);
            this.tlpPatient.Controls.Add(this.lblRepresented, 4, 0);
            this.tlpPatient.Controls.Add(this.tbAge, 1, 3);
            this.tlpPatient.Controls.Add(this.tbSurname, 0, 2);
            this.tlpPatient.Controls.Add(this.tbBirthDate, 0, 3);
            this.tlpPatient.Controls.Add(this.tbPersonalCode, 0, 0);
            this.tlpPatient.Controls.Add(this.btnFind, 2, 0);
            this.tlpPatient.Controls.Add(this.cmbRelated, 4, 1);
            this.tlpPatient.Controls.Add(this.cmbRepresented, 4, 2);
            this.tlpPatient.Controls.Add(this.tbPatientName, 0, 1);
            this.tlpPatient.Controls.Add(this.gbType, 5, 3);
            this.tlpPatient.Controls.Add(this.rtbAllergies, 2, 1);
            this.tlpPatient.Controls.Add(this.ehRecipeListNavigation, 0, 6);
            this.tlpPatient.Controls.Add(this.lblCompensationApplies, 2, 3);
            this.tlpPatient.Controls.Add(this.panelNavigation, 0, 4);
            this.tlpPatient.Controls.Add(this.btnPrepaymentInfo, 3, 0);
            this.tlpPatient.Controls.Add(this.btnDispensesInfo, 4, 3);
            this.tlpPatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPatient.Location = new System.Drawing.Point(0, 0);
            this.tlpPatient.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPatient.Name = "tlpPatient";
            this.tlpPatient.RowCount = 8;
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpPatient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpPatient.Size = new System.Drawing.Size(1246, 1009);
            this.tlpPatient.TabIndex = 92;
            // 
            // ehRecipeList
            // 
            this.tlpPatient.SetColumnSpan(this.ehRecipeList, 7);
            this.ehRecipeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehRecipeList.Location = new System.Drawing.Point(0, 230);
            this.ehRecipeList.Margin = new System.Windows.Forms.Padding(0);
            this.ehRecipeList.Name = "ehRecipeList";
            this.ehRecipeList.Size = new System.Drawing.Size(1246, 326);
            this.ehRecipeList.TabIndex = 151;
            this.ehRecipeList.Text = "elementHost1";
            this.ehRecipeList.Child = null;
            // 
            // ehDispenseListPatient
            // 
            this.tlpPatient.SetColumnSpan(this.ehDispenseListPatient, 7);
            this.ehDispenseListPatient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehDispenseListPatient.Location = new System.Drawing.Point(0, 610);
            this.ehDispenseListPatient.Margin = new System.Windows.Forms.Padding(0);
            this.ehDispenseListPatient.Name = "ehDispenseListPatient";
            this.ehDispenseListPatient.Size = new System.Drawing.Size(1246, 399);
            this.ehDispenseListPatient.TabIndex = 144;
            this.ehDispenseListPatient.Text = "elementHost1";
            this.ehDispenseListPatient.Child = null;
            // 
            // lblRepresented
            // 
            this.lblRepresented.AutoSize = true;
            this.tlpPatient.SetColumnSpan(this.lblRepresented, 3);
            this.lblRepresented.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRepresented.Location = new System.Drawing.Point(528, 0);
            this.lblRepresented.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRepresented.Name = "lblRepresented";
            this.lblRepresented.Size = new System.Drawing.Size(714, 46);
            this.lblRepresented.TabIndex = 129;
            this.lblRepresented.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbAge
            // 
            this.tbAge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbAge.Location = new System.Drawing.Point(154, 143);
            this.tbAge.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbAge.MaxLength = 11;
            this.tbAge.Name = "tbAge";
            this.tbAge.ReadOnly = true;
            this.tbAge.Size = new System.Drawing.Size(52, 30);
            this.tbAge.TabIndex = 124;
            // 
            // tbSurname
            // 
            this.tlpPatient.SetColumnSpan(this.tbSurname, 2);
            this.tbSurname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSurname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbSurname.Location = new System.Drawing.Point(4, 97);
            this.tbSurname.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSurname.MaxLength = 11;
            this.tbSurname.Name = "tbSurname";
            this.tbSurname.ReadOnly = true;
            this.tbSurname.Size = new System.Drawing.Size(202, 30);
            this.tbSurname.TabIndex = 119;
            // 
            // tbBirthDate
            // 
            this.tbBirthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbBirthDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbBirthDate.Location = new System.Drawing.Point(4, 143);
            this.tbBirthDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbBirthDate.MaxLength = 11;
            this.tbBirthDate.Name = "tbBirthDate";
            this.tbBirthDate.ReadOnly = true;
            this.tbBirthDate.Size = new System.Drawing.Size(142, 30);
            this.tbBirthDate.TabIndex = 120;
            // 
            // tbPersonalCode
            // 
            this.tbPersonalCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpPatient.SetColumnSpan(this.tbPersonalCode, 2);
            this.tbPersonalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbPersonalCode.Location = new System.Drawing.Point(4, 5);
            this.tbPersonalCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPersonalCode.MaxLength = 20;
            this.tbPersonalCode.Name = "tbPersonalCode";
            this.tbPersonalCode.Size = new System.Drawing.Size(202, 30);
            this.tbPersonalCode.TabIndex = 7;
            this.tbPersonalCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPersonalCode_KeyPress);
            // 
            // btnFind
            // 
            this.btnFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFind.Location = new System.Drawing.Point(214, 5);
            this.btnFind.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(142, 36);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "&Ieškoti";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // cmbRelated
            // 
            this.tlpPatient.SetColumnSpan(this.cmbRelated, 3);
            this.cmbRelated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRelated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelated.FormattingEnabled = true;
            this.cmbRelated.Location = new System.Drawing.Point(528, 51);
            this.cmbRelated.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbRelated.Name = "cmbRelated";
            this.cmbRelated.Size = new System.Drawing.Size(714, 28);
            this.cmbRelated.TabIndex = 93;
            this.cmbRelated.SelectedIndexChanged += new System.EventHandler(this.cmbRelated_SelectedIndexChanged);
            // 
            // cmbRepresented
            // 
            this.tlpPatient.SetColumnSpan(this.cmbRepresented, 3);
            this.cmbRepresented.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRepresented.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRepresented.FormattingEnabled = true;
            this.cmbRepresented.Location = new System.Drawing.Point(528, 97);
            this.cmbRepresented.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbRepresented.Name = "cmbRepresented";
            this.cmbRepresented.Size = new System.Drawing.Size(714, 28);
            this.cmbRepresented.TabIndex = 94;
            this.cmbRepresented.SelectedIndexChanged += new System.EventHandler(this.cmbRepresented_SelectedIndexChanged);
            // 
            // tbPatientName
            // 
            this.tlpPatient.SetColumnSpan(this.tbPatientName, 2);
            this.tbPatientName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPatientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbPatientName.Location = new System.Drawing.Point(4, 51);
            this.tbPatientName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPatientName.MaxLength = 11;
            this.tbPatientName.Name = "tbPatientName";
            this.tbPatientName.ReadOnly = true;
            this.tbPatientName.Size = new System.Drawing.Size(202, 30);
            this.tbPatientName.TabIndex = 112;
            // 
            // gbType
            // 
            this.tlpPatient.SetColumnSpan(this.gbType, 2);
            this.gbType.Controls.Add(this.rbActive);
            this.gbType.Controls.Add(this.rbAll);
            this.gbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbType.Enabled = false;
            this.gbType.Location = new System.Drawing.Point(680, 143);
            this.gbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbType.Name = "gbType";
            this.gbType.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gbType.Size = new System.Drawing.Size(562, 36);
            this.gbType.TabIndex = 132;
            this.gbType.TabStop = false;
            // 
            // rbActive
            // 
            this.rbActive.AutoSize = true;
            this.rbActive.Checked = true;
            this.rbActive.ForeColor = System.Drawing.Color.Green;
            this.rbActive.Location = new System.Drawing.Point(82, 9);
            this.rbActive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbActive.Name = "rbActive";
            this.rbActive.Size = new System.Drawing.Size(92, 24);
            this.rbActive.TabIndex = 1;
            this.rbActive.TabStop = true;
            this.rbActive.Text = "Aktyvūs";
            this.rbActive.UseVisualStyleBackColor = true;
            this.rbActive.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(9, 9);
            this.rbAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(62, 24);
            this.rbAll.TabIndex = 0;
            this.rbAll.Text = "Visi";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rb_CheckedChanged);
            // 
            // rtbAllergies
            // 
            this.rtbAllergies.BackColor = System.Drawing.SystemColors.Control;
            this.tlpPatient.SetColumnSpan(this.rtbAllergies, 2);
            this.rtbAllergies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAllergies.Location = new System.Drawing.Point(214, 51);
            this.rtbAllergies.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbAllergies.Name = "rtbAllergies";
            this.rtbAllergies.ReadOnly = true;
            this.tlpPatient.SetRowSpan(this.rtbAllergies, 2);
            this.rtbAllergies.Size = new System.Drawing.Size(306, 82);
            this.rtbAllergies.TabIndex = 143;
            this.rtbAllergies.Text = "";
            // 
            // ehRecipeListNavigation
            // 
            this.tlpPatient.SetColumnSpan(this.ehRecipeListNavigation, 7);
            this.ehRecipeListNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehRecipeListNavigation.Location = new System.Drawing.Point(4, 561);
            this.ehRecipeListNavigation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ehRecipeListNavigation.Name = "ehRecipeListNavigation";
            this.ehRecipeListNavigation.Size = new System.Drawing.Size(1238, 44);
            this.ehRecipeListNavigation.TabIndex = 150;
            this.ehRecipeListNavigation.Child = null;
            // 
            // lblCompensationApplies
            // 
            this.lblCompensationApplies.AutoSize = true;
            this.tlpPatient.SetColumnSpan(this.lblCompensationApplies, 2);
            this.lblCompensationApplies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCompensationApplies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblCompensationApplies.ForeColor = System.Drawing.Color.Green;
            this.lblCompensationApplies.Location = new System.Drawing.Point(214, 138);
            this.lblCompensationApplies.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompensationApplies.Name = "lblCompensationApplies";
            this.lblCompensationApplies.Size = new System.Drawing.Size(306, 46);
            this.lblCompensationApplies.TabIndex = 153;
            this.lblCompensationApplies.Text = "Priklauso priemokos kompensacija";
            this.lblCompensationApplies.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCompensationApplies.Visible = false;
            // 
            // panelNavigation
            // 
            this.tlpPatient.SetColumnSpan(this.panelNavigation, 7);
            this.panelNavigation.Controls.Add(this.btnPaperRecipe);
            this.panelNavigation.Controls.Add(this.btnFilter);
            this.panelNavigation.Controls.Add(this.btnUnlock);
            this.panelNavigation.Controls.Add(this.btnGetRecipePdf);
            this.panelNavigation.Controls.Add(this.btnGetDispensePdf);
            this.panelNavigation.Controls.Add(this.btnSuspend);
            this.panelNavigation.Controls.Add(this.btnReserve);
            this.panelNavigation.Controls.Add(this.btnSell);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavigation.Location = new System.Drawing.Point(4, 189);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(1238, 36);
            this.panelNavigation.TabIndex = 154;
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnFilter.BackgroundImage = global::POS_display.Properties.Resources.filter;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFilter.Location = new System.Drawing.Point(1116, 2);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(51, 35);
            this.btnFilter.TabIndex = 133;
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.BackColor = System.Drawing.Color.Transparent;
            this.btnUnlock.BackgroundImage = global::POS_display.Properties.Resources.unlock;
            this.btnUnlock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUnlock.Enabled = false;
            this.btnUnlock.ForeColor = System.Drawing.Color.Transparent;
            this.btnUnlock.Location = new System.Drawing.Point(1176, 2);
            this.btnUnlock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(51, 35);
            this.btnUnlock.TabIndex = 132;
            this.btnUnlock.UseVisualStyleBackColor = false;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnGetRecipePdf
            // 
            this.btnGetRecipePdf.Location = new System.Drawing.Point(630, 0);
            this.btnGetRecipePdf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetRecipePdf.Name = "btnGetRecipePdf";
            this.btnGetRecipePdf.Size = new System.Drawing.Size(150, 37);
            this.btnGetRecipePdf.TabIndex = 130;
            this.btnGetRecipePdf.Text = "Parsisiųsti rec.";
            this.btnGetRecipePdf.UseVisualStyleBackColor = true;
            this.btnGetRecipePdf.Click += new System.EventHandler(this.btnGetRecipePdf_Click);
            // 
            // btnGetDispensePdf
            // 
            this.btnGetDispensePdf.Location = new System.Drawing.Point(472, 0);
            this.btnGetDispensePdf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetDispensePdf.Name = "btnGetDispensePdf";
            this.btnGetDispensePdf.Size = new System.Drawing.Size(150, 37);
            this.btnGetDispensePdf.TabIndex = 131;
            this.btnGetDispensePdf.Text = "Parsisiųsti išdav.";
            this.btnGetDispensePdf.UseVisualStyleBackColor = true;
            this.btnGetDispensePdf.Click += new System.EventHandler(this.btnGetDispensePdf_Click);
            // 
            // btnSuspend
            // 
            this.btnSuspend.Location = new System.Drawing.Point(315, 0);
            this.btnSuspend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(150, 37);
            this.btnSuspend.TabIndex = 128;
            this.btnSuspend.Text = "Sustabdyti";
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            // 
            // btnReserve
            // 
            this.btnReserve.Location = new System.Drawing.Point(158, 0);
            this.btnReserve.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(150, 37);
            this.btnReserve.TabIndex = 125;
            this.btnReserve.Text = "Rezervuoti";
            this.btnReserve.UseVisualStyleBackColor = true;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(0, 0);
            this.btnSell.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(150, 37);
            this.btnSell.TabIndex = 91;
            this.btnSell.Text = "Išduoti vaistus";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // btnPrepaymentInfo
            // 
            this.btnPrepaymentInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrepaymentInfo.Enabled = false;
            this.btnPrepaymentInfo.Location = new System.Drawing.Point(364, 5);
            this.btnPrepaymentInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrepaymentInfo.Name = "btnPrepaymentInfo";
            this.btnPrepaymentInfo.Size = new System.Drawing.Size(156, 36);
            this.btnPrepaymentInfo.TabIndex = 155;
            this.btnPrepaymentInfo.Text = "Priemokos info";
            this.btnPrepaymentInfo.UseVisualStyleBackColor = true;
            this.btnPrepaymentInfo.Click += new System.EventHandler(this.btnPrepaymentInfo_Click);
            // 
            // btnDispensesInfo
            // 
            this.btnDispensesInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDispensesInfo.Enabled = false;
            this.btnDispensesInfo.Location = new System.Drawing.Point(528, 143);
            this.btnDispensesInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDispensesInfo.Name = "btnDispensesInfo";
            this.btnDispensesInfo.Size = new System.Drawing.Size(144, 36);
            this.btnDispensesInfo.TabIndex = 156;
            this.btnDispensesInfo.Text = "Išdavimų Info";
            this.btnDispensesInfo.UseVisualStyleBackColor = true;
            this.btnDispensesInfo.Click += new System.EventHandler(this.btnDispensesInfo_Click);
            // 
            // tabRecipes
            // 
            this.tabRecipes.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabRecipes.Controls.Add(this.tlpRecipes);
            this.tabRecipes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tabRecipes.Location = new System.Drawing.Point(4, 29);
            this.tabRecipes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabRecipes.Name = "tabRecipes";
            this.tabRecipes.Size = new System.Drawing.Size(1244, 1007);
            this.tabRecipes.TabIndex = 1;
            this.tabRecipes.Text = "E - receptų pasirinkimas";
            // 
            // tlpRecipes
            // 
            this.tlpRecipes.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tlpRecipes.ColumnCount = 7;
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRecipes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpRecipes.Controls.Add(this.label2, 1, 2);
            this.tlpRecipes.Controls.Add(this.label1, 0, 2);
            this.tlpRecipes.Controls.Add(this.dtpDateTo, 1, 3);
            this.tlpRecipes.Controls.Add(this.dtpDateFrom, 0, 3);
            this.tlpRecipes.Controls.Add(this.btnFindObsolete, 5, 3);
            this.tlpRecipes.Controls.Add(this.label26, 3, 0);
            this.tlpRecipes.Controls.Add(this.cmbConfirmed, 3, 1);
            this.tlpRecipes.Controls.Add(this.cmbStatus, 1, 1);
            this.tlpRecipes.Controls.Add(this.label30, 2, 0);
            this.tlpRecipes.Controls.Add(this.label29, 1, 0);
            this.tlpRecipes.Controls.Add(this.label28, 0, 0);
            this.tlpRecipes.Controls.Add(this.cmbDocStatus, 2, 1);
            this.tlpRecipes.Controls.Add(this.cmbFilter, 0, 1);
            this.tlpRecipes.Controls.Add(this.btnDispFindDispense, 5, 1);
            this.tlpRecipes.Controls.Add(this.ehDispenseList, 0, 4);
            this.tlpRecipes.Controls.Add(this.cbNarc, 2, 3);
            this.tlpRecipes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRecipes.Location = new System.Drawing.Point(0, 0);
            this.tlpRecipes.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRecipes.Name = "tlpRecipes";
            this.tlpRecipes.RowCount = 5;
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRecipes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpRecipes.Size = new System.Drawing.Size(1244, 1007);
            this.tlpRecipes.TabIndex = 92;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(154, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 46);
            this.label2.TabIndex = 154;
            this.label2.Text = "Iki";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(4, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 46);
            this.label1.TabIndex = 153;
            this.label1.Text = "Nuo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpDateTo
            // 
            this.dtpDateTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateTo.Location = new System.Drawing.Point(154, 143);
            this.dtpDateTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpDateTo.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpDateTo.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpDateTo.Name = "dtpDateTo";
            this.dtpDateTo.Size = new System.Drawing.Size(142, 26);
            this.dtpDateTo.TabIndex = 152;
            // 
            // dtpDateFrom
            // 
            this.dtpDateFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateFrom.Location = new System.Drawing.Point(4, 143);
            this.dtpDateFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpDateFrom.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpDateFrom.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpDateFrom.Name = "dtpDateFrom";
            this.dtpDateFrom.Size = new System.Drawing.Size(142, 26);
            this.dtpDateFrom.TabIndex = 151;
            // 
            // btnFindObsolete
            // 
            this.tlpRecipes.SetColumnSpan(this.btnFindObsolete, 2);
            this.btnFindObsolete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindObsolete.Location = new System.Drawing.Point(656, 143);
            this.btnFindObsolete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFindObsolete.Name = "btnFindObsolete";
            this.btnFindObsolete.Size = new System.Drawing.Size(584, 36);
            this.btnFindObsolete.TabIndex = 150;
            this.btnFindObsolete.Text = "Ieškoti OLD";
            this.btnFindObsolete.UseVisualStyleBackColor = true;
            this.btnFindObsolete.Visible = false;
            this.btnFindObsolete.Click += new System.EventHandler(this.btnFindObsolete_Click);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Location = new System.Drawing.Point(454, 0);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(142, 46);
            this.label26.TabIndex = 141;
            this.label26.Text = "Ar patvirtintas";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbConfirmed
            // 
            this.cmbConfirmed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbConfirmed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConfirmed.FormattingEnabled = true;
            this.cmbConfirmed.Location = new System.Drawing.Point(454, 51);
            this.cmbConfirmed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbConfirmed.Name = "cmbConfirmed";
            this.cmbConfirmed.Size = new System.Drawing.Size(142, 28);
            this.cmbConfirmed.TabIndex = 140;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(154, 51);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(142, 28);
            this.cmbStatus.TabIndex = 138;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Location = new System.Drawing.Point(304, 0);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(142, 46);
            this.label30.TabIndex = 137;
            this.label30.Text = "Dokumento statusas";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Location = new System.Drawing.Point(154, 0);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(142, 46);
            this.label29.TabIndex = 135;
            this.label29.Text = "Statusas";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Location = new System.Drawing.Point(4, 0);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(142, 46);
            this.label28.TabIndex = 134;
            this.label28.Text = "Receptai";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDocStatus
            // 
            this.cmbDocStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDocStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocStatus.FormattingEnabled = true;
            this.cmbDocStatus.Location = new System.Drawing.Point(304, 51);
            this.cmbDocStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDocStatus.Name = "cmbDocStatus";
            this.cmbDocStatus.Size = new System.Drawing.Size(142, 28);
            this.cmbDocStatus.TabIndex = 133;
            // 
            // cmbFilter
            // 
            this.cmbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(4, 51);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(142, 28);
            this.cmbFilter.TabIndex = 132;
            // 
            // btnDispFindDispense
            // 
            this.tlpRecipes.SetColumnSpan(this.btnDispFindDispense, 2);
            this.btnDispFindDispense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDispFindDispense.Location = new System.Drawing.Point(656, 51);
            this.btnDispFindDispense.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDispFindDispense.Name = "btnDispFindDispense";
            this.btnDispFindDispense.Size = new System.Drawing.Size(584, 36);
            this.btnDispFindDispense.TabIndex = 129;
            this.btnDispFindDispense.Text = "&Ieškoti";
            this.btnDispFindDispense.UseVisualStyleBackColor = true;
            this.btnDispFindDispense.Click += new System.EventHandler(this.btnDispFindDispense_Click);
            // 
            // ehDispenseList
            // 
            this.tlpRecipes.SetColumnSpan(this.ehDispenseList, 7);
            this.ehDispenseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehDispenseList.Location = new System.Drawing.Point(0, 184);
            this.ehDispenseList.Margin = new System.Windows.Forms.Padding(0);
            this.ehDispenseList.Name = "ehDispenseList";
            this.ehDispenseList.Size = new System.Drawing.Size(1244, 823);
            this.ehDispenseList.TabIndex = 148;
            this.ehDispenseList.Text = "elementHost1";
            this.ehDispenseList.Child = null;
            // 
            // cbNarc
            // 
            this.cbNarc.AutoSize = true;
            this.cbNarc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbNarc.Location = new System.Drawing.Point(304, 143);
            this.cbNarc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbNarc.Name = "cbNarc";
            this.cbNarc.Size = new System.Drawing.Size(142, 36);
            this.cbNarc.TabIndex = 155;
            this.cbNarc.Text = "Tik narkotiniai";
            this.cbNarc.UseVisualStyleBackColor = true;
            // 
            // tabVaccination
            // 
            this.tabVaccination.Controls.Add(this.vaccineUserControl);
            this.tabVaccination.Location = new System.Drawing.Point(4, 29);
            this.tabVaccination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabVaccination.Name = "tabVaccination";
            this.tabVaccination.Size = new System.Drawing.Size(1244, 1007);
            this.tabVaccination.TabIndex = 2;
            this.tabVaccination.Text = "Vakcinacija";
            // 
            // vaccineUserControl
            // 
            this.vaccineUserControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.vaccineUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vaccineUserControl.IsActiveCancelVaccine = false;
            this.vaccineUserControl.IsActiveSellVaccine = false;
            this.vaccineUserControl.IsBusy = false;
            this.vaccineUserControl.Location = new System.Drawing.Point(0, 0);
            this.vaccineUserControl.LowIncome = null;
            this.vaccineUserControl.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.vaccineUserControl.Name = "vaccineUserControl";
            this.vaccineUserControl.Patient = null;
            this.vaccineUserControl.PatientBirthDate = "";
            this.vaccineUserControl.PatientName = "";
            this.vaccineUserControl.PatientSurname = "";
            this.vaccineUserControl.PersonalCode = "";
            this.vaccineUserControl.SelectedVaccineEntry = null;
            this.vaccineUserControl.Size = new System.Drawing.Size(1244, 1007);
            this.vaccineUserControl.TabIndex = 0;
            this.vaccineUserControl.Total = 0;
            this.vaccineUserControl.VaccineEntries = null;
            // 
            // tabVaccinationSelection
            // 
            this.tabVaccinationSelection.BackColor = System.Drawing.SystemColors.Control;
            this.tabVaccinationSelection.Controls.Add(this.tlVaccines);
            this.tabVaccinationSelection.Location = new System.Drawing.Point(4, 29);
            this.tabVaccinationSelection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabVaccinationSelection.Name = "tabVaccinationSelection";
            this.tabVaccinationSelection.Size = new System.Drawing.Size(1244, 1007);
            this.tabVaccinationSelection.TabIndex = 3;
            this.tabVaccinationSelection.Text = "Vakcinacijų pasirinkimas";
            // 
            // tlVaccines
            // 
            this.tlVaccines.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tlVaccines.ColumnCount = 7;
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlVaccines.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlVaccines.Controls.Add(this.ehVaccineList, 0, 4);
            this.tlVaccines.Controls.Add(this.cmbVaccineStatus, 2, 1);
            this.tlVaccines.Controls.Add(this.cmbDocType, 1, 1);
            this.tlVaccines.Controls.Add(this.lblDocs, 0, 0);
            this.tlVaccines.Controls.Add(this.lblDocType, 1, 0);
            this.tlVaccines.Controls.Add(this.lblStatus, 2, 0);
            this.tlVaccines.Controls.Add(this.lblDocStatus, 3, 0);
            this.tlVaccines.Controls.Add(this.btnFindVaccine, 5, 1);
            this.tlVaccines.Controls.Add(this.lblVaccineFrom, 0, 2);
            this.tlVaccines.Controls.Add(this.lblVaccineTo, 1, 2);
            this.tlVaccines.Controls.Add(this.cmbVaccineDoc, 0, 1);
            this.tlVaccines.Controls.Add(this.cmbVaccineDocStatus, 3, 1);
            this.tlVaccines.Controls.Add(this.dtpVaccineFrom, 0, 3);
            this.tlVaccines.Controls.Add(this.dtpVaccineTo, 1, 3);
            this.tlVaccines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlVaccines.Location = new System.Drawing.Point(0, 0);
            this.tlVaccines.Margin = new System.Windows.Forms.Padding(0);
            this.tlVaccines.Name = "tlVaccines";
            this.tlVaccines.RowCount = 5;
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlVaccines.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlVaccines.Size = new System.Drawing.Size(1244, 1007);
            this.tlVaccines.TabIndex = 0;
            // 
            // ehVaccineList
            // 
            this.tlVaccines.SetColumnSpan(this.ehVaccineList, 7);
            this.ehVaccineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehVaccineList.Location = new System.Drawing.Point(0, 184);
            this.ehVaccineList.Margin = new System.Windows.Forms.Padding(0);
            this.ehVaccineList.Name = "ehVaccineList";
            this.ehVaccineList.Size = new System.Drawing.Size(1244, 823);
            this.ehVaccineList.TabIndex = 162;
            this.ehVaccineList.Text = "elementHost1";
            this.ehVaccineList.Child = null;
            // 
            // cmbVaccineStatus
            // 
            this.cmbVaccineStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVaccineStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVaccineStatus.FormattingEnabled = true;
            this.cmbVaccineStatus.Location = new System.Drawing.Point(304, 51);
            this.cmbVaccineStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbVaccineStatus.Name = "cmbVaccineStatus";
            this.cmbVaccineStatus.Size = new System.Drawing.Size(142, 28);
            this.cmbVaccineStatus.TabIndex = 158;
            // 
            // cmbDocType
            // 
            this.cmbDocType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Location = new System.Drawing.Point(154, 51);
            this.cmbDocType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(142, 28);
            this.cmbDocType.TabIndex = 157;
            // 
            // lblDocs
            // 
            this.lblDocs.AutoSize = true;
            this.lblDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocs.Location = new System.Drawing.Point(4, 0);
            this.lblDocs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDocs.Name = "lblDocs";
            this.lblDocs.Size = new System.Drawing.Size(142, 46);
            this.lblDocs.TabIndex = 135;
            this.lblDocs.Text = "Dokumentai";
            this.lblDocs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocType
            // 
            this.lblDocType.AutoSize = true;
            this.lblDocType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocType.Location = new System.Drawing.Point(154, 0);
            this.lblDocType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDocType.Name = "lblDocType";
            this.lblDocType.Size = new System.Drawing.Size(142, 46);
            this.lblDocType.TabIndex = 136;
            this.lblDocType.Text = "Dok. Tipas";
            this.lblDocType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(304, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(142, 46);
            this.lblStatus.TabIndex = 142;
            this.lblStatus.Text = "Statusas";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocStatus
            // 
            this.lblDocStatus.AutoSize = true;
            this.lblDocStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDocStatus.Location = new System.Drawing.Point(454, 0);
            this.lblDocStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDocStatus.Name = "lblDocStatus";
            this.lblDocStatus.Size = new System.Drawing.Size(142, 46);
            this.lblDocStatus.TabIndex = 138;
            this.lblDocStatus.Text = "Dokumento statusas";
            this.lblDocStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFindVaccine
            // 
            this.tlVaccines.SetColumnSpan(this.btnFindVaccine, 2);
            this.btnFindVaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindVaccine.Location = new System.Drawing.Point(656, 51);
            this.btnFindVaccine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFindVaccine.Name = "btnFindVaccine";
            this.btnFindVaccine.Size = new System.Drawing.Size(584, 36);
            this.btnFindVaccine.TabIndex = 143;
            this.btnFindVaccine.Text = "&Ieškoti";
            this.btnFindVaccine.UseVisualStyleBackColor = true;
            this.btnFindVaccine.Click += new System.EventHandler(this.btnFindVaccine_Click);
            // 
            // lblVaccineFrom
            // 
            this.lblVaccineFrom.AutoSize = true;
            this.lblVaccineFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVaccineFrom.Location = new System.Drawing.Point(4, 92);
            this.lblVaccineFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVaccineFrom.Name = "lblVaccineFrom";
            this.lblVaccineFrom.Size = new System.Drawing.Size(142, 46);
            this.lblVaccineFrom.TabIndex = 154;
            this.lblVaccineFrom.Text = "Nuo";
            this.lblVaccineFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVaccineTo
            // 
            this.lblVaccineTo.AutoSize = true;
            this.lblVaccineTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVaccineTo.Location = new System.Drawing.Point(154, 92);
            this.lblVaccineTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVaccineTo.Name = "lblVaccineTo";
            this.lblVaccineTo.Size = new System.Drawing.Size(142, 46);
            this.lblVaccineTo.TabIndex = 155;
            this.lblVaccineTo.Text = "Iki";
            this.lblVaccineTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbVaccineDoc
            // 
            this.cmbVaccineDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVaccineDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVaccineDoc.FormattingEnabled = true;
            this.cmbVaccineDoc.Location = new System.Drawing.Point(4, 51);
            this.cmbVaccineDoc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbVaccineDoc.Name = "cmbVaccineDoc";
            this.cmbVaccineDoc.Size = new System.Drawing.Size(142, 28);
            this.cmbVaccineDoc.TabIndex = 156;
            // 
            // cmbVaccineDocStatus
            // 
            this.cmbVaccineDocStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbVaccineDocStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVaccineDocStatus.FormattingEnabled = true;
            this.cmbVaccineDocStatus.Location = new System.Drawing.Point(454, 51);
            this.cmbVaccineDocStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbVaccineDocStatus.Name = "cmbVaccineDocStatus";
            this.cmbVaccineDocStatus.Size = new System.Drawing.Size(142, 28);
            this.cmbVaccineDocStatus.TabIndex = 159;
            // 
            // dtpVaccineFrom
            // 
            this.dtpVaccineFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpVaccineFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVaccineFrom.Location = new System.Drawing.Point(4, 143);
            this.dtpVaccineFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpVaccineFrom.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpVaccineFrom.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpVaccineFrom.Name = "dtpVaccineFrom";
            this.dtpVaccineFrom.Size = new System.Drawing.Size(142, 26);
            this.dtpVaccineFrom.TabIndex = 161;
            // 
            // dtpVaccineTo
            // 
            this.dtpVaccineTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpVaccineTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVaccineTo.Location = new System.Drawing.Point(154, 143);
            this.dtpVaccineTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpVaccineTo.MaxDate = new System.DateTime(2098, 12, 31, 0, 0, 0, 0);
            this.dtpVaccineTo.MinDate = new System.DateTime(1999, 1, 1, 0, 0, 0, 0);
            this.dtpVaccineTo.Name = "dtpVaccineTo";
            this.dtpVaccineTo.Size = new System.Drawing.Size(142, 26);
            this.dtpVaccineTo.TabIndex = 160;
            // 
            // ehRecipe
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ehRecipe, 4);
            this.ehRecipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehRecipe.Location = new System.Drawing.Point(1270, 59);
            this.ehRecipe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ehRecipe.Name = "ehRecipe";
            this.ehRecipe.Size = new System.Drawing.Size(606, 1046);
            this.ehRecipe.TabIndex = 147;
            this.ehRecipe.Text = "elementHost1";
            this.ehRecipe.Child = null;
            // 
            // btnCancelTask
            // 
            this.btnCancelTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelTask.Image = global::POS_display.Properties.Resources.stop;
            this.btnCancelTask.Location = new System.Drawing.Point(1689, 5);
            this.btnCancelTask.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelTask.Name = "btnCancelTask";
            this.btnCancelTask.Size = new System.Drawing.Size(37, 36);
            this.btnCancelTask.TabIndex = 153;
            this.btnCancelTask.UseVisualStyleBackColor = true;
            this.btnCancelTask.Click += new System.EventHandler(this.btnCancelTask_Click);
            // 
            // btnPaperRecipe
            // 
            this.btnPaperRecipe.Location = new System.Drawing.Point(787, -1);
            this.btnPaperRecipe.Name = "btnPaperRecipe";
            this.btnPaperRecipe.Size = new System.Drawing.Size(150, 38);
            this.btnPaperRecipe.TabIndex = 134;
            this.btnPaperRecipe.Text = "Popierinis rec.";
            this.btnPaperRecipe.UseVisualStyleBackColor = true;
            this.btnPaperRecipe.Click += new System.EventHandler(this.btnPaperRecipe_Click);
            // 
            // eRecipeV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1896, 1166);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "eRecipeV2";
            this.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.ShowInTaskbar = false;
            this.Text = "Elektroninis receptas V2";
            this.Load += new System.EventHandler(this.eRecipe_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.tabPanel1.ResumeLayout(false);
            this.tabPatient.ResumeLayout(false);
            this.tlpPatient.ResumeLayout(false);
            this.tlpPatient.PerformLayout();
            this.gbType.ResumeLayout(false);
            this.gbType.PerformLayout();
            this.panelNavigation.ResumeLayout(false);
            this.tabRecipes.ResumeLayout(false);
            this.tlpRecipes.ResumeLayout(false);
            this.tlpRecipes.PerformLayout();
            this.tabVaccination.ResumeLayout(false);
            this.tabVaccinationSelection.ResumeLayout(false);
            this.tlVaccines.ResumeLayout(false);
            this.tlVaccines.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HorizontalLine horizontalLine1;
        private System.Windows.Forms.Integration.ElementHost ehRecipe;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblSystem;
        private Label lblPractitioner;
        private Button btnClose;
        private Panel panel1;
        private Panel pnlSearch;
        private TabControl tabPanel1;
        private TabPage tabPatient;
        private TableLayoutPanel tlpPatient;
        private Button btnGetDispensePdf;
        private Button btnGetRecipePdf;
        private Label lblRepresented;
        private Button btnSuspend;
        private TextBox tbAge;
        private TextBox tbSurname;
        private Button btnSell;
        private TextBox tbBirthDate;
        private TextBox tbPersonalCode;
        private Button btnFind;
        private ComboBox cmbRelated;
        private ComboBox cmbRepresented;
        private TextBox tbPatientName;
        private Button btnReserve;
        private TabPage tabRecipes;
        private TableLayoutPanel tlpRecipes;
        private Label label26;
        private ComboBox cmbConfirmed;
        private ComboBox cmbStatus;
        private Label label30;
        private Label label29;
        private Label label28;
        private ComboBox cmbDocStatus;
        private ComboBox cmbFilter;
        private Button btnDispFindDispense;
        private GroupBox gbType;
        private RadioButton rbActive;
        private RadioButton rbAll;
        private RichTextBox rtbAllergies;
        private System.Windows.Forms.Integration.ElementHost ehRecipeListNavigation;
        private System.Windows.Forms.Integration.ElementHost ehRecipeList;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.Integration.ElementHost ehDispenseListPatient;
        private System.Windows.Forms.Integration.ElementHost ehDispenseList;
        private Button btnCancelTask;
        private Button btnFindObsolete;
        private DateTimePicker dtpDateFrom;
        private Label label2;
        private Label label1;
        private DateTimePicker dtpDateTo;
        private CheckBox cbNarc;
        private Label lblCompensationApplies;
        private TabPage tabVaccination;
        private Panel panelNavigation;
        private Popups.display1_popups.ERecipe.VaccineUserControlV2 vaccineUserControl;
        private TabPage tabVaccinationSelection;
        private Label lblStatus;
        private Label lblDocStatus;
        private Label lblDocType;
        private Label lblDocs;
        private TableLayoutPanel tlVaccines;
        private Button btnFindVaccine;
        private Label lblVaccineTo;
        private Label lblVaccineFrom;
        private ComboBox cmbVaccineDoc;
        private ComboBox cmbDocType;
        private ComboBox cmbVaccineDocStatus;
        private ComboBox cmbVaccineStatus;
        private DateTimePicker dtpVaccineFrom;
        private DateTimePicker dtpVaccineTo;
        private System.Windows.Forms.Integration.ElementHost ehVaccineList;
        private Button btnPrepaymentInfo;
        private Button btnDispensesInfo;
        private Button btnUnlock;
        private Button btnFilter;
        private Button btnPaperRecipe;
    }
}