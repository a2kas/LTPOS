namespace POS_display
{
    partial class DrugPricesView
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
            this.lblActiveSubstance = new System.Windows.Forms.Label();
            this.tbSearchActiveSubstance = new System.Windows.Forms.TextBox();
            this.lblDosageAmount = new System.Windows.Forms.Label();
            this.tbSearchMedicationName = new System.Windows.Forms.TextBox();
            this.lblMedicationName = new System.Windows.Forms.Label();
            this.tbQty = new System.Windows.Forms.NumericUpDown();
            this.tbSearchBarcode = new System.Windows.Forms.TextBox();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.btnSelBarcode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbQty)).BeginInit();
            this.SuspendLayout();
            // 
            // lblActiveSubstance
            // 
            this.lblActiveSubstance.AutoSize = true;
            this.lblActiveSubstance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblActiveSubstance.Location = new System.Drawing.Point(8, 48);
            this.lblActiveSubstance.Name = "lblActiveSubstance";
            this.lblActiveSubstance.Size = new System.Drawing.Size(188, 20);
            this.lblActiveSubstance.TabIndex = 0;
            this.lblActiveSubstance.Text = "Bendrinis pavadinimas";
            // 
            // tbSearchActiveSubstance
            // 
            this.tbSearchActiveSubstance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbSearchActiveSubstance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbSearchActiveSubstance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbSearchActiveSubstance.Location = new System.Drawing.Point(12, 71);
            this.tbSearchActiveSubstance.Name = "tbSearchActiveSubstance";
            this.tbSearchActiveSubstance.Size = new System.Drawing.Size(902, 23);
            this.tbSearchActiveSubstance.TabIndex = 2;
            this.tbSearchActiveSubstance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearchActiveSubstance_KeyDown);
            // 
            // lblDosageAmount
            // 
            this.lblDosageAmount.AutoSize = true;
            this.lblDosageAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblDosageAmount.Location = new System.Drawing.Point(8, 15);
            this.lblDosageAmount.Name = "lblDosageAmount";
            this.lblDosageAmount.Size = new System.Drawing.Size(95, 20);
            this.lblDosageAmount.TabIndex = 11;
            this.lblDosageAmount.Text = "Kiekis doz.";
            // 
            // tbSearchFirm
            // 
            this.tbSearchMedicationName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbSearchMedicationName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbSearchMedicationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbSearchMedicationName.Location = new System.Drawing.Point(12, 130);
            this.tbSearchMedicationName.Name = "tbSearchFirm";
            this.tbSearchMedicationName.Size = new System.Drawing.Size(902, 23);
            this.tbSearchMedicationName.TabIndex = 3;
            this.tbSearchMedicationName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearchMedicationName_KeyDown);
            // 
            // lblMedicationName
            // 
            this.lblMedicationName.AutoSize = true;
            this.lblMedicationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblMedicationName.Location = new System.Drawing.Point(8, 107);
            this.lblMedicationName.Name = "lblMedicationName";
            this.lblMedicationName.Size = new System.Drawing.Size(175, 20);
            this.lblMedicationName.TabIndex = 14;
            this.lblMedicationName.Text = "Firminis pavadinimas";
            // 
            // tbQty
            // 
            this.tbQty.DecimalPlaces = 2;
            this.tbQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbQty.Location = new System.Drawing.Point(109, 12);
            this.tbQty.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.tbQty.Name = "tbQty";
            this.tbQty.Size = new System.Drawing.Size(72, 23);
            this.tbQty.TabIndex = 1;
            this.tbQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbQty.Enter += new System.EventHandler(this.tbQty_Enter);
            // 
            // tbSearchBarcode
            // 
            this.tbSearchBarcode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbSearchBarcode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbSearchBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbSearchBarcode.Location = new System.Drawing.Point(12, 189);
            this.tbSearchBarcode.Name = "tbSearchBarcode";
            this.tbSearchBarcode.Size = new System.Drawing.Size(875, 23);
            this.tbSearchBarcode.TabIndex = 104;
            this.tbSearchBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearchBarcode_KeyDown);
            // 
            // lblBarcode
            // 
            this.lblBarcode.AutoSize = true;
            this.lblBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblBarcode.Location = new System.Drawing.Point(8, 166);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(85, 20);
            this.lblBarcode.TabIndex = 105;
            this.lblBarcode.Text = "Barkodas";
            // 
            // btnSelBarcode
            // 
            this.btnSelBarcode.Location = new System.Drawing.Point(893, 189);
            this.btnSelBarcode.Name = "btnSelBarcode";
            this.btnSelBarcode.Size = new System.Drawing.Size(25, 23);
            this.btnSelBarcode.TabIndex = 107;
            this.btnSelBarcode.Text = "&...";
            this.btnSelBarcode.UseVisualStyleBackColor = true;
            this.btnSelBarcode.Click += new System.EventHandler(this.btnSelBarcode_Click);
            // 
            // SearchFormView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 234);
            this.Controls.Add(this.btnSelBarcode);
            this.Controls.Add(this.tbSearchBarcode);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.tbQty);
            this.Controls.Add(this.tbSearchMedicationName);
            this.Controls.Add(this.lblMedicationName);
            this.Controls.Add(this.lblDosageAmount);
            this.Controls.Add(this.tbSearchActiveSubstance);
            this.Controls.Add(this.lblActiveSubstance);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "SearchFormView";
            this.Text = "Vaistai";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.SearchFormView_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchFormView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.tbQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblActiveSubstance;
        private System.Windows.Forms.TextBox tbSearchActiveSubstance;
        private System.Windows.Forms.Label lblDosageAmount;
        private System.Windows.Forms.TextBox tbSearchMedicationName;
        private System.Windows.Forms.Label lblMedicationName;
        private System.Windows.Forms.NumericUpDown tbQty;
        private System.Windows.Forms.TextBox tbSearchBarcode;
        private System.Windows.Forms.Label lblBarcode;
        private System.Windows.Forms.Button btnSelBarcode;
    }
}