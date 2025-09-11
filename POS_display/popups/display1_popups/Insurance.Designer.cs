namespace POS_display.Popups.display1_popups
{
    partial class Insurance
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabEPS = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbInsurance = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbIdentity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCardNo = new System.Windows.Forms.TextBox();
            this.tabGjensidige = new System.Windows.Forms.TabPage();
            this.btnWeb = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbOthersSum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVitaminsSum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMedicinesSum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbInsuranceSum = new System.Windows.Forms.TextBox();
            this.btnCalc = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabEPS.SuspendLayout();
            this.tabGjensidige.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabEPS);
            this.tabControl1.Controls.Add(this.tabGjensidige);
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(346, 163);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            // 
            // tabEPS
            // 
            this.tabEPS.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabEPS.Controls.Add(this.label7);
            this.tabEPS.Controls.Add(this.cmbInsurance);
            this.tabEPS.Controls.Add(this.label6);
            this.tabEPS.Controls.Add(this.tbIdentity);
            this.tabEPS.Controls.Add(this.label5);
            this.tabEPS.Controls.Add(this.tbCardNo);
            this.tabEPS.Location = new System.Drawing.Point(4, 22);
            this.tabEPS.Name = "tabEPS";
            this.tabEPS.Padding = new System.Windows.Forms.Padding(3);
            this.tabEPS.Size = new System.Drawing.Size(338, 137);
            this.tabEPS.TabIndex = 0;
            this.tabEPS.Text = "Draudimo atpažinimo duomenys";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 70;
            this.label7.Text = "Draudimo kompanija";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbInsurance
            // 
            this.cmbInsurance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInsurance.FormattingEnabled = true;
            this.cmbInsurance.Location = new System.Drawing.Point(116, 16);
            this.cmbInsurance.Name = "cmbInsurance";
            this.cmbInsurance.Size = new System.Drawing.Size(214, 21);
            this.cmbInsurance.TabIndex = 2;
            this.cmbInsurance.SelectedIndexChanged += new System.EventHandler(this.cmbInsurance_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 68;
            this.label6.Text = "Identifikaciniai sk.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbIdentity
            // 
            this.tbIdentity.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIdentity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbIdentity.Location = new System.Drawing.Point(116, 72);
            this.tbIdentity.Name = "tbIdentity";
            this.tbIdentity.Size = new System.Drawing.Size(214, 23);
            this.tbIdentity.TabIndex = 4;
            this.tbIdentity.TextChanged += new System.EventHandler(this.tbCardNo_TextChanged);
            this.tbIdentity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_keyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Kortelės numeris";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbCardNo
            // 
            this.tbCardNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCardNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCardNo.Location = new System.Drawing.Point(116, 43);
            this.tbCardNo.Name = "tbCardNo";
            this.tbCardNo.Size = new System.Drawing.Size(214, 23);
            this.tbCardNo.TabIndex = 3;
            this.tbCardNo.TextChanged += new System.EventHandler(this.tbCardNo_TextChanged);
            this.tbCardNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_keyPress);
            // 
            // tabGjensidige
            // 
            this.tabGjensidige.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabGjensidige.Controls.Add(this.btnWeb);
            this.tabGjensidige.Controls.Add(this.label4);
            this.tabGjensidige.Controls.Add(this.tbOthersSum);
            this.tabGjensidige.Controls.Add(this.label3);
            this.tabGjensidige.Controls.Add(this.tbVitaminsSum);
            this.tabGjensidige.Controls.Add(this.label2);
            this.tabGjensidige.Controls.Add(this.tbMedicinesSum);
            this.tabGjensidige.Controls.Add(this.label1);
            this.tabGjensidige.Controls.Add(this.tbInsuranceSum);
            this.tabGjensidige.Location = new System.Drawing.Point(4, 22);
            this.tabGjensidige.Name = "tabGjensidige";
            this.tabGjensidige.Padding = new System.Windows.Forms.Padding(3);
            this.tabGjensidige.Size = new System.Drawing.Size(338, 137);
            this.tabGjensidige.TabIndex = 1;
            this.tabGjensidige.Text = "Gjensidige draudimas";
            // 
            // btnWeb
            // 
            this.btnWeb.Location = new System.Drawing.Point(241, 102);
            this.btnWeb.Name = "btnWeb";
            this.btnWeb.Size = new System.Drawing.Size(91, 24);
            this.btnWeb.TabIndex = 8;
            this.btnWeb.Text = "&Apskaičiuoti";
            this.btnWeb.UseVisualStyleBackColor = true;
            this.btnWeb.Click += new System.EventHandler(this.btnWeb_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "Visų rizikų dr.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbOthersSum
            // 
            this.tbOthersSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOthersSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbOthersSum.Location = new System.Drawing.Point(100, 72);
            this.tbOthersSum.Name = "tbOthersSum";
            this.tbOthersSum.ReadOnly = true;
            this.tbOthersSum.Size = new System.Drawing.Size(135, 23);
            this.tbOthersSum.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 69;
            this.label3.Text = "Vitaminai";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbVitaminsSum
            // 
            this.tbVitaminsSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVitaminsSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbVitaminsSum.Location = new System.Drawing.Point(100, 42);
            this.tbVitaminsSum.Name = "tbVitaminsSum";
            this.tbVitaminsSum.ReadOnly = true;
            this.tbVitaminsSum.Size = new System.Drawing.Size(135, 23);
            this.tbVitaminsSum.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 67;
            this.label2.Text = "Vaistai";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbMedicinesSum
            // 
            this.tbMedicinesSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMedicinesSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbMedicinesSum.Location = new System.Drawing.Point(100, 12);
            this.tbMedicinesSum.Name = "tbMedicinesSum";
            this.tbMedicinesSum.ReadOnly = true;
            this.tbMedicinesSum.Size = new System.Drawing.Size(135, 23);
            this.tbMedicinesSum.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "Draudimo suma";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbInsuranceSum
            // 
            this.tbInsuranceSum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInsuranceSum.Enabled = false;
            this.tbInsuranceSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbInsuranceSum.Location = new System.Drawing.Point(100, 102);
            this.tbInsuranceSum.Name = "tbInsuranceSum";
            this.tbInsuranceSum.Size = new System.Drawing.Size(135, 23);
            this.tbInsuranceSum.TabIndex = 9;
            this.tbInsuranceSum.TextChanged += new System.EventHandler(this.tbInsuranceSum_TextChanged);
            this.tbInsuranceSum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPrice_KeyPress);
            // 
            // btnCalc
            // 
            this.btnCalc.Enabled = false;
            this.btnCalc.Location = new System.Drawing.Point(3, 3);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(88, 24);
            this.btnCalc.TabIndex = 1;
            this.btnCalc.Text = "&Patvirtinti";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(248, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 24);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Uždaryti";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Insurance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(346, 193);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "Insurance";
            this.ShowIcon = false;
            this.Text = "Draudimas";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Insurance_Closing);
            this.Load += new System.EventHandler(this.Insurance_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabEPS.ResumeLayout(false);
            this.tabEPS.PerformLayout();
            this.tabGjensidige.ResumeLayout(false);
            this.tabGjensidige.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabEPS;
        private System.Windows.Forms.TabPage tabGjensidige;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbCardNo;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWeb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbOthersSum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVitaminsSum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMedicinesSum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInsuranceSum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbIdentity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbInsurance;
    }
}