
using System;

namespace POS_display.Popups.display1_popups.ERecipe
{
    partial class VaccineUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PnlVaccine = new System.Windows.Forms.Panel();
            this.Vaccine = new System.Windows.Forms.TableLayoutPanel();
            this.lblInitName = new System.Windows.Forms.Label();
            this.TbAge = new System.Windows.Forms.TextBox();
            this.TbSurname = new System.Windows.Forms.TextBox();
            this.TbBirthDate = new System.Windows.Forms.TextBox();
            this.TbPersonalCode = new System.Windows.Forms.TextBox();
            this.BtnFind = new System.Windows.Forms.Button();
            this.TbPatientName = new System.Windows.Forms.TextBox();
            this.rtbAllergiesForVaccine = new System.Windows.Forms.RichTextBox();
            this.BtnCreateVaccine = new System.Windows.Forms.Button();
            this.BtnSellVaccine = new System.Windows.Forms.Button();
            this.BtnVaccineCancel = new System.Windows.Forms.Button();
            this.ehVaccineOrderList = new System.Windows.Forms.Integration.ElementHost();
            this.ehOrderListNavigation = new System.Windows.Forms.Integration.ElementHost();
            this.PnlVaccine.SuspendLayout();
            this.Vaccine.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlVaccine
            // 
            this.PnlVaccine.Controls.Add(this.Vaccine);
            this.PnlVaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlVaccine.Location = new System.Drawing.Point(0, 0);
            this.PnlVaccine.Name = "PnlVaccine";
            this.PnlVaccine.Size = new System.Drawing.Size(583, 680);
            this.PnlVaccine.TabIndex = 0;
            // 
            // Vaccine
            // 
            this.Vaccine.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Vaccine.ColumnCount = 7;
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Vaccine.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.Vaccine.Controls.Add(this.lblInitName, 0, 5);
            this.Vaccine.Controls.Add(this.TbAge, 1, 3);
            this.Vaccine.Controls.Add(this.TbSurname, 0, 2);
            this.Vaccine.Controls.Add(this.TbBirthDate, 0, 3);
            this.Vaccine.Controls.Add(this.TbPersonalCode, 0, 0);
            this.Vaccine.Controls.Add(this.BtnFind, 2, 0);
            this.Vaccine.Controls.Add(this.TbPatientName, 0, 1);
            this.Vaccine.Controls.Add(this.rtbAllergiesForVaccine, 2, 1);
            this.Vaccine.Controls.Add(this.BtnCreateVaccine, 0, 4);
            this.Vaccine.Controls.Add(this.BtnSellVaccine, 0, 6);
            this.Vaccine.Controls.Add(this.BtnVaccineCancel, 2, 6);
            this.Vaccine.Controls.Add(this.ehVaccineOrderList, 4, 6);
            this.Vaccine.Controls.Add(this.ehOrderListNavigation, 4, 7);
            this.Vaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Vaccine.Location = new System.Drawing.Point(0, 0);
            this.Vaccine.Margin = new System.Windows.Forms.Padding(0);
            this.Vaccine.Name = "Vaccine";
            this.Vaccine.RowCount = 9;
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 413F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.Vaccine.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Vaccine.Size = new System.Drawing.Size(583, 680);
            this.Vaccine.TabIndex = 95;
            // 
            // lblInitName
            // 
            this.lblInitName.AutoSize = true;
            this.Vaccine.SetColumnSpan(this.lblInitName, 3);
            this.lblInitName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInitName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblInitName.Location = new System.Drawing.Point(3, 150);
            this.lblInitName.Name = "lblInitName";
            this.lblInitName.Size = new System.Drawing.Size(234, 20);
            this.lblInitName.TabIndex = 145;
            this.lblInitName.Text = "Neišduoti skiepai";
            this.lblInitName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TbAge
            // 
            this.TbAge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbAge.Location = new System.Drawing.Point(103, 93);
            this.TbAge.MaxLength = 11;
            this.TbAge.Name = "TbAge";
            this.TbAge.ReadOnly = true;
            this.TbAge.Size = new System.Drawing.Size(34, 23);
            this.TbAge.TabIndex = 124;
            // 
            // TbSurname
            // 
            this.Vaccine.SetColumnSpan(this.TbSurname, 2);
            this.TbSurname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbSurname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbSurname.Location = new System.Drawing.Point(3, 63);
            this.TbSurname.MaxLength = 11;
            this.TbSurname.Name = "TbSurname";
            this.TbSurname.ReadOnly = true;
            this.TbSurname.Size = new System.Drawing.Size(134, 23);
            this.TbSurname.TabIndex = 119;
            // 
            // TbBirthDate
            // 
            this.TbBirthDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbBirthDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbBirthDate.Location = new System.Drawing.Point(3, 93);
            this.TbBirthDate.MaxLength = 11;
            this.TbBirthDate.Name = "TbBirthDate";
            this.TbBirthDate.ReadOnly = true;
            this.TbBirthDate.Size = new System.Drawing.Size(94, 23);
            this.TbBirthDate.TabIndex = 120;
            // 
            // TbPersonalCode
            // 
            this.TbPersonalCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Vaccine.SetColumnSpan(this.TbPersonalCode, 2);
            this.TbPersonalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbPersonalCode.Location = new System.Drawing.Point(3, 3);
            this.TbPersonalCode.MaxLength = 20;
            this.TbPersonalCode.Name = "TbPersonalCode";
            this.TbPersonalCode.Size = new System.Drawing.Size(134, 23);
            this.TbPersonalCode.TabIndex = 7;
            this.TbPersonalCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbPersonalCode_KeyPress);
            // 
            // BtnFind
            // 
            this.BtnFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnFind.Location = new System.Drawing.Point(143, 3);
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(94, 24);
            this.BtnFind.TabIndex = 1;
            this.BtnFind.Text = "&Ieškoti";
            this.BtnFind.UseVisualStyleBackColor = true;
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // TbPatientName
            // 
            this.Vaccine.SetColumnSpan(this.TbPatientName, 2);
            this.TbPatientName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbPatientName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbPatientName.Location = new System.Drawing.Point(3, 33);
            this.TbPatientName.MaxLength = 11;
            this.TbPatientName.Name = "TbPatientName";
            this.TbPatientName.ReadOnly = true;
            this.TbPatientName.Size = new System.Drawing.Size(134, 23);
            this.TbPatientName.TabIndex = 112;
            // 
            // rtbAllergiesForVaccine
            // 
            this.rtbAllergiesForVaccine.BackColor = System.Drawing.SystemColors.Control;
            this.Vaccine.SetColumnSpan(this.rtbAllergiesForVaccine, 2);
            this.rtbAllergiesForVaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbAllergiesForVaccine.Location = new System.Drawing.Point(143, 33);
            this.rtbAllergiesForVaccine.Name = "rtbAllergiesForVaccine";
            this.rtbAllergiesForVaccine.ReadOnly = true;
            this.Vaccine.SetRowSpan(this.rtbAllergiesForVaccine, 3);
            this.rtbAllergiesForVaccine.Size = new System.Drawing.Size(203, 84);
            this.rtbAllergiesForVaccine.TabIndex = 143;
            this.rtbAllergiesForVaccine.Text = "";
            // 
            // BtnCreateVaccine
            // 
            this.Vaccine.SetColumnSpan(this.BtnCreateVaccine, 2);
            this.BtnCreateVaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnCreateVaccine.Enabled = false;
            this.BtnCreateVaccine.Location = new System.Drawing.Point(3, 123);
            this.BtnCreateVaccine.Name = "BtnCreateVaccine";
            this.BtnCreateVaccine.Size = new System.Drawing.Size(134, 24);
            this.BtnCreateVaccine.TabIndex = 91;
            this.BtnCreateVaccine.Text = "&Skiepų skyrimas";
            this.BtnCreateVaccine.UseVisualStyleBackColor = true;
            this.BtnCreateVaccine.Click += new System.EventHandler(this.BtnCreateVaccine_Click);
            // 
            // BtnSellVaccine
            // 
            this.Vaccine.SetColumnSpan(this.BtnSellVaccine, 2);
            this.BtnSellVaccine.Enabled = false;
            this.BtnSellVaccine.Location = new System.Drawing.Point(3, 173);
            this.BtnSellVaccine.Name = "BtnSellVaccine";
            this.BtnSellVaccine.Size = new System.Drawing.Size(134, 24);
            this.BtnSellVaccine.TabIndex = 125;
            this.BtnSellVaccine.Text = "Skiepų išdavimas";
            this.BtnSellVaccine.UseVisualStyleBackColor = true;
            this.BtnSellVaccine.Click += new System.EventHandler(this.BtnSellVaccine_Click);
            // 
            // BtnVaccineCancel
            // 
            this.Vaccine.SetColumnSpan(this.BtnVaccineCancel, 2);
            this.BtnVaccineCancel.Location = new System.Drawing.Point(143, 173);
            this.BtnVaccineCancel.Name = "BtnVaccineCancel";
            this.BtnVaccineCancel.Size = new System.Drawing.Size(142, 23);
            this.BtnVaccineCancel.TabIndex = 147;
            this.BtnVaccineCancel.Text = "Skiepo panaikinimas";
            this.BtnVaccineCancel.UseVisualStyleBackColor = true;
            this.BtnVaccineCancel.Click += new System.EventHandler(this.BtnVaccineCancel_Click);
            // 
            // ehVaccineOrderList
            // 
            this.Vaccine.SetColumnSpan(this.ehVaccineOrderList, 7);
            this.ehVaccineOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ehVaccineOrderList.Location = new System.Drawing.Point(0, 200);
            this.ehVaccineOrderList.Margin = new System.Windows.Forms.Padding(0);
            this.ehVaccineOrderList.Name = "ehVaccineOrderList";
            this.ehVaccineOrderList.Size = new System.Drawing.Size(583, 413);
            this.ehVaccineOrderList.TabIndex = 163;
            this.ehVaccineOrderList.Text = "elementHost1";
            this.ehVaccineOrderList.Child = null;
            // 
            // ehOrderListNavigation
            // 
            this.Vaccine.SetColumnSpan(this.ehOrderListNavigation, 7);
            this.ehOrderListNavigation.Dock = System.Windows.Forms.DockStyle.Top;
            this.ehOrderListNavigation.Location = new System.Drawing.Point(3, 616);
            this.ehOrderListNavigation.Name = "ehOrderListNavigation";
            this.ehOrderListNavigation.Size = new System.Drawing.Size(577, 29);
            this.ehOrderListNavigation.TabIndex = 164;
            this.ehOrderListNavigation.Child = null;
            // 
            // VaccineUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PnlVaccine);
            this.Name = "VaccineUserControl";
            this.Size = new System.Drawing.Size(583, 680);
            this.Load += new System.EventHandler(this.Vaccine_Load);
            this.PnlVaccine.ResumeLayout(false);
            this.Vaccine.ResumeLayout(false);
            this.Vaccine.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlVaccine;
        private System.Windows.Forms.TableLayoutPanel Vaccine;
        private System.Windows.Forms.Label lblInitName;
        private System.Windows.Forms.TextBox TbAge;
        private System.Windows.Forms.TextBox TbSurname;
        private System.Windows.Forms.Button BtnCreateVaccine;
        private System.Windows.Forms.TextBox TbBirthDate;
        private System.Windows.Forms.TextBox TbPersonalCode;
        private System.Windows.Forms.Button BtnFind;
        private System.Windows.Forms.TextBox TbPatientName;
        private System.Windows.Forms.Button BtnSellVaccine;
        private System.Windows.Forms.RichTextBox rtbAllergiesForVaccine;
        private Helpers.UserControlBase userControlBase1;
        private System.Windows.Forms.Button BtnVaccineCancel;
        private System.Windows.Forms.Integration.ElementHost ehVaccineOrderList;
        private System.Windows.Forms.Integration.ElementHost ehOrderListNavigation;
    }
}
