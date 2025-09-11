
namespace POS_display.Views.NarcoticAlert
{
    partial class NarcoticAlertView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NarcoticAlertView));
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.rtbNotification = new POS_display.Helpers.RichTextBoxEx();
            this.dataGridView_drugMaterials = new System.Windows.Forms.DataGridView();
            this.colActiveSubstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitOfMeasurement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_drugMaterials)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblHeader.Location = new System.Drawing.Point(93, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(543, 31);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Jūs parduodate narkotiką / psichotropą !!!";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.08882F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.91118F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel2.Controls.Add(this.lblHeader, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.rtbNotification, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dataGridView_drugMaterials, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 113F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.17647F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.82353F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(733, 314);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(642, 284);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 27);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // rtbNotification
            // 
            this.rtbNotification.BackColor = System.Drawing.SystemColors.Menu;
            this.rtbNotification.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNotification.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.rtbNotification.Location = new System.Drawing.Point(93, 34);
            this.rtbNotification.Name = "rtbNotification";
            this.rtbNotification.Size = new System.Drawing.Size(543, 107);
            this.rtbNotification.TabIndex = 4;
            this.rtbNotification.Text = "";
            this.rtbNotification.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.RtbNotification_LinkClicked);
            // 
            // dataGridView_drugMaterials
            // 
            this.dataGridView_drugMaterials.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_drugMaterials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colActiveSubstance,
            this.colUnitOfMeasurement,
            this.colAmount});
            this.dataGridView_drugMaterials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_drugMaterials.Location = new System.Drawing.Point(93, 147);
            this.dataGridView_drugMaterials.Name = "dataGridView_drugMaterials";
            this.dataGridView_drugMaterials.ReadOnly = true;
            this.dataGridView_drugMaterials.Size = new System.Drawing.Size(543, 131);
            this.dataGridView_drugMaterials.TabIndex = 5;
            // 
            // colActiveSubstance
            // 
            this.colActiveSubstance.DataPropertyName = "ActiveSubstance";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.colActiveSubstance.DefaultCellStyle = dataGridViewCellStyle1;
            this.colActiveSubstance.HeaderText = "Veiklioji medžiaga ar veikliųjų medžiagų grupė";
            this.colActiveSubstance.Name = "colActiveSubstance";
            this.colActiveSubstance.ReadOnly = true;
            this.colActiveSubstance.Width = 250;
            // 
            // colUnitOfMeasurement
            // 
            this.colUnitOfMeasurement.DataPropertyName = "UnitOfMeasurement";
            this.colUnitOfMeasurement.HeaderText = "Mato vienetas/ Dozuotė";
            this.colUnitOfMeasurement.Name = "colUnitOfMeasurement";
            this.colUnitOfMeasurement.ReadOnly = true;
            this.colUnitOfMeasurement.Width = 150;
            // 
            // colAmount
            // 
            this.colAmount.DataPropertyName = "Amount";
            this.colAmount.HeaderText = "Kiekis";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // NarcoticAlertView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 314);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NarcoticAlertView";
            this.Text = "Narkotinių vaistų pardavimas";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_drugMaterials)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnClose;
        private POS_display.Helpers.RichTextBoxEx rtbNotification;
        private System.Windows.Forms.DataGridView dataGridView_drugMaterials;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActiveSubstance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitOfMeasurement;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
    }
}