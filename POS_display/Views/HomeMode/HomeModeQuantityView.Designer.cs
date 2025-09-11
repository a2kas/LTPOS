namespace POS_display.Views.HomeMode
{
    partial class HomeModeQuantityView
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblHomeQty = new System.Windows.Forms.Label();
            this.tbHomeQty = new System.Windows.Forms.TextBox();
            this.tbRealQty = new System.Windows.Forms.TextBox();
            this.lblRealQty = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.54217F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.45783F));
            this.tlpMain.Controls.Add(this.lblHomeQty, 0, 1);
            this.tlpMain.Controls.Add(this.tbHomeQty, 1, 1);
            this.tlpMain.Controls.Add(this.tbRealQty, 1, 3);
            this.tlpMain.Controls.Add(this.lblRealQty, 0, 3);
            this.tlpMain.Controls.Add(this.btnSubmit, 1, 5);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 6;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.90654F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.09346F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpMain.Size = new System.Drawing.Size(388, 122);
            this.tlpMain.TabIndex = 0;
            // 
            // lblHomeQty
            // 
            this.lblHomeQty.AutoSize = true;
            this.lblHomeQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHomeQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblHomeQty.Location = new System.Drawing.Point(3, 8);
            this.lblHomeQty.Name = "lblHomeQty";
            this.lblHomeQty.Size = new System.Drawing.Size(228, 21);
            this.lblHomeQty.TabIndex = 0;
            this.lblHomeQty.Text = "Į namus pristatomas kiekis:";
            // 
            // tbHomeQty
            // 
            this.tbHomeQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbHomeQty.Location = new System.Drawing.Point(237, 11);
            this.tbHomeQty.Name = "tbHomeQty";
            this.tbHomeQty.Size = new System.Drawing.Size(148, 20);
            this.tbHomeQty.TabIndex = 2;
            // 
            // tbRealQty
            // 
            this.tbRealQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRealQty.Location = new System.Drawing.Point(237, 44);
            this.tbRealQty.Name = "tbRealQty";
            this.tbRealQty.Size = new System.Drawing.Size(148, 20);
            this.tbRealQty.TabIndex = 3;
            // 
            // lblRealQty
            // 
            this.lblRealQty.AutoSize = true;
            this.lblRealQty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRealQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblRealQty.Location = new System.Drawing.Point(3, 41);
            this.lblRealQty.Name = "lblRealQty";
            this.lblRealQty.Size = new System.Drawing.Size(228, 28);
            this.lblRealQty.TabIndex = 1;
            this.lblRealQty.Text = "Vaistinėje atiduodamas kiekis:";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSubmit.Location = new System.Drawing.Point(237, 95);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(148, 24);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Patvirtinti";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // HomeModeQuantityView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 122);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HomeModeQuantityView";
            this.ShowIcon = false;
            this.Text = "Kiekis pristatyti į namus";
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblHomeQty;
        private System.Windows.Forms.Label lblRealQty;
        private System.Windows.Forms.TextBox tbHomeQty;
        private System.Windows.Forms.TextBox tbRealQty;
        private System.Windows.Forms.Button btnSubmit;
    }
}