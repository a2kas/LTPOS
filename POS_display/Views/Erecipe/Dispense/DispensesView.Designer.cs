namespace POS_display.Views.Erecipe.Dispense
{
    partial class DispensesView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDispenses = new System.Windows.Forms.DataGridView();
            this.colCompositionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProprietaryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantityValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDispenseInfo = new System.Windows.Forms.Label();
            this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDispenses)).BeginInit();
            this.tblLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDispenses
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDispenses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDispenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDispenses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCompositionId,
            this.colProprietaryName,
            this.colDueDate,
            this.colQuantityValue});
            this.dgvDispenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDispenses.Location = new System.Drawing.Point(3, 26);
            this.dgvDispenses.Name = "dgvDispenses";
            this.dgvDispenses.RowHeadersVisible = false;
            this.dgvDispenses.Size = new System.Drawing.Size(701, 423);
            this.dgvDispenses.TabIndex = 0;
            // 
            // colCompositionId
            // 
            this.colCompositionId.DataPropertyName = "CompositionId";
            this.colCompositionId.Frozen = true;
            this.colCompositionId.HeaderText = "Išdavimo Nr.";
            this.colCompositionId.Name = "colCompositionId";
            this.colCompositionId.ReadOnly = true;
            this.colCompositionId.Width = 120;
            // 
            // colProprietaryName
            // 
            this.colProprietaryName.DataPropertyName = "ProprietaryName";
            this.colProprietaryName.HeaderText = "Firminis pav.";
            this.colProprietaryName.Name = "colProprietaryName";
            this.colProprietaryName.ReadOnly = true;
            this.colProprietaryName.Width = 330;
            // 
            // colDueDate
            // 
            this.colDueDate.DataPropertyName = "DateDueDate";
            dataGridViewCellStyle6.Format = "yyyy.MM.dd";
            this.colDueDate.DefaultCellStyle = dataGridViewCellStyle6;
            this.colDueDate.HeaderText = "Pakanka iki";
            this.colDueDate.Name = "colDueDate";
            this.colDueDate.ReadOnly = true;
            // 
            // colQuantityValue
            // 
            this.colQuantityValue.DataPropertyName = "QuantityValue";
            this.colQuantityValue.HeaderText = "Kiekis";
            this.colQuantityValue.Name = "colQuantityValue";
            this.colQuantityValue.ReadOnly = true;
            // 
            // lblDispenseInfo
            // 
            this.lblDispenseInfo.AutoSize = true;
            this.lblDispenseInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDispenseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblDispenseInfo.Location = new System.Drawing.Point(3, 0);
            this.lblDispenseInfo.Name = "lblDispenseInfo";
            this.lblDispenseInfo.Size = new System.Drawing.Size(701, 23);
            this.lblDispenseInfo.TabIndex = 0;
            this.lblDispenseInfo.Text = "Liko išduoti: 0";
            // 
            // tblLayout
            // 
            this.tblLayout.ColumnCount = 1;
            this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayout.Controls.Add(this.dgvDispenses, 0, 1);
            this.tblLayout.Controls.Add(this.lblDispenseInfo, 0, 0);
            this.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayout.Location = new System.Drawing.Point(0, 0);
            this.tblLayout.Name = "tblLayout";
            this.tblLayout.RowCount = 2;
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblLayout.Size = new System.Drawing.Size(707, 452);
            this.tblLayout.TabIndex = 2;
            // 
            // DispensesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 452);
            this.Controls.Add(this.tblLayout);
            this.Name = "DispensesView";
            this.ShowIcon = false;
            this.Text = "Recepto išdavimai";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDispenses)).EndInit();
            this.tblLayout.ResumeLayout(false);
            this.tblLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDispenses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompositionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProprietaryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantityValue;
        private System.Windows.Forms.Label lblDispenseInfo;
        private System.Windows.Forms.TableLayoutPanel tblLayout;
    }
}