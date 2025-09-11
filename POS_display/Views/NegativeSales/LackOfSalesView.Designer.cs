namespace POS_display.Views.NegativeSales
{
    partial class LackOfSalesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LackOfSalesView));
            this.lackOfSalesDataGridView = new System.Windows.Forms.DataGridView();
            this.colBarcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtyLack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lackOfSalesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // lackOfSalesDataGridView
            // 
            this.lackOfSalesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lackOfSalesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBarcode,
            this.colName,
            this.colQty,
            this.colQtyLeft,
            this.colQtyLack});
            this.lackOfSalesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lackOfSalesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.lackOfSalesDataGridView.Name = "lackOfSalesDataGridView";
            this.lackOfSalesDataGridView.ReadOnly = true;
            this.lackOfSalesDataGridView.RowHeadersVisible = false;
            this.lackOfSalesDataGridView.Size = new System.Drawing.Size(708, 430);
            this.lackOfSalesDataGridView.TabIndex = 0;
            // 
            // colBarcode
            // 
            this.colBarcode.DataPropertyName = "Barcode";
            this.colBarcode.HeaderText = "Barkodas";
            this.colBarcode.Name = "colBarcode";
            this.colBarcode.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Pavadinimas";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 300;
            // 
            // colQty
            // 
            this.colQty.DataPropertyName = "Qty";
            this.colQty.HeaderText = "Kiekis";
            this.colQty.Name = "colQty";
            this.colQty.ReadOnly = true;
            // 
            // colQtyLeft
            // 
            this.colQtyLeft.DataPropertyName = "QtyLeft";
            this.colQtyLeft.HeaderText = "Likutis";
            this.colQtyLeft.Name = "colQtyLeft";
            this.colQtyLeft.ReadOnly = true;
            // 
            // colQtyLack
            // 
            this.colQtyLack.DataPropertyName = "QtyLack";
            this.colQtyLack.HeaderText = "Trūksta";
            this.colQtyLack.Name = "colQtyLack";
            this.colQtyLack.ReadOnly = true;
            // 
            // LackOfSalesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 430);
            this.Controls.Add(this.lackOfSalesDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LackOfSalesView";
            this.Text = "Prekių, kurių negalima perkelti sąrašas";
            ((System.ComponentModel.ISupportInitialize)(this.lackOfSalesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView lackOfSalesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBarcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtyLack;
    }
}