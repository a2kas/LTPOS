
namespace POS_display.Views.PersonalPharmacist
{
    partial class PersonalPharmacistView
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
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbPersonalPhramacist = new System.Windows.Forms.GroupBox();
            this.tbDoctorSurename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDueDate = new System.Windows.Forms.Label();
            this.dtpDueDate = new System.Windows.Forms.DateTimePicker();
            this.tbDoctorName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDoctorID = new System.Windows.Forms.TextBox();
            this.lblDoctorID = new System.Windows.Forms.Label();
            this.lblHospital = new System.Windows.Forms.Label();
            this.lblClientPersonalCode = new System.Windows.Forms.Label();
            this.tbClientPersonalCode = new System.Windows.Forms.TextBox();
            this.cbHospital = new System.Windows.Forms.ComboBox();
            this.gbPersonalPhramacist.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(6, 191);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(130, 33);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Pritaikyti";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(142, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(129, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Atšaukti";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbPersonalPhramacist
            // 
            this.gbPersonalPhramacist.Controls.Add(this.tbDoctorSurename);
            this.gbPersonalPhramacist.Controls.Add(this.label2);
            this.gbPersonalPhramacist.Controls.Add(this.lblDueDate);
            this.gbPersonalPhramacist.Controls.Add(this.dtpDueDate);
            this.gbPersonalPhramacist.Controls.Add(this.tbDoctorName);
            this.gbPersonalPhramacist.Controls.Add(this.label1);
            this.gbPersonalPhramacist.Controls.Add(this.tbDoctorID);
            this.gbPersonalPhramacist.Controls.Add(this.lblDoctorID);
            this.gbPersonalPhramacist.Controls.Add(this.lblHospital);
            this.gbPersonalPhramacist.Controls.Add(this.lblClientPersonalCode);
            this.gbPersonalPhramacist.Controls.Add(this.tbClientPersonalCode);
            this.gbPersonalPhramacist.Controls.Add(this.cbHospital);
            this.gbPersonalPhramacist.Controls.Add(this.btnApply);
            this.gbPersonalPhramacist.Controls.Add(this.btnCancel);
            this.gbPersonalPhramacist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPersonalPhramacist.Location = new System.Drawing.Point(0, 0);
            this.gbPersonalPhramacist.Name = "gbPersonalPhramacist";
            this.gbPersonalPhramacist.Size = new System.Drawing.Size(407, 236);
            this.gbPersonalPhramacist.TabIndex = 2;
            this.gbPersonalPhramacist.TabStop = false;
            this.gbPersonalPhramacist.Text = "Informacija:";
            // 
            // tbDoctorSurename
            // 
            this.tbDoctorSurename.Location = new System.Drawing.Point(128, 123);
            this.tbDoctorSurename.MaxLength = 100;
            this.tbDoctorSurename.Name = "tbDoctorSurename";
            this.tbDoctorSurename.Size = new System.Drawing.Size(267, 20);
            this.tbDoctorSurename.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Gydytojas pavardė:";
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoSize = true;
            this.lblDueDate.Location = new System.Drawing.Point(15, 49);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Size = new System.Drawing.Size(97, 13);
            this.lblDueDate.TabIndex = 11;
            this.lblDueDate.Text = "Vaisto pakanka iki:";
            // 
            // dtpDueDate
            // 
            this.dtpDueDate.Location = new System.Drawing.Point(128, 45);
            this.dtpDueDate.Name = "dtpDueDate";
            this.dtpDueDate.Size = new System.Drawing.Size(267, 20);
            this.dtpDueDate.TabIndex = 10;
            // 
            // tbDoctorName
            // 
            this.tbDoctorName.Location = new System.Drawing.Point(128, 97);
            this.tbDoctorName.MaxLength = 100;
            this.tbDoctorName.Name = "tbDoctorName";
            this.tbDoctorName.Size = new System.Drawing.Size(267, 20);
            this.tbDoctorName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Gydytojas vardas:";
            // 
            // tbDoctorID
            // 
            this.tbDoctorID.Location = new System.Drawing.Point(128, 71);
            this.tbDoctorID.MaxLength = 10;
            this.tbDoctorID.Name = "tbDoctorID";
            this.tbDoctorID.Size = new System.Drawing.Size(267, 20);
            this.tbDoctorID.TabIndex = 7;
            // 
            // lblDoctorID
            // 
            this.lblDoctorID.AutoSize = true;
            this.lblDoctorID.Location = new System.Drawing.Point(15, 74);
            this.lblDoctorID.Name = "lblDoctorID";
            this.lblDoctorID.Size = new System.Drawing.Size(103, 13);
            this.lblDoctorID.TabIndex = 6;
            this.lblDoctorID.Text = "Gydytojo spaudo ID:";
            // 
            // lblHospital
            // 
            this.lblHospital.AutoSize = true;
            this.lblHospital.Location = new System.Drawing.Point(15, 152);
            this.lblHospital.Name = "lblHospital";
            this.lblHospital.Size = new System.Drawing.Size(50, 13);
            this.lblHospital.TabIndex = 5;
            this.lblHospital.Text = "Ligoninė:";
            // 
            // lblClientPersonalCode
            // 
            this.lblClientPersonalCode.AutoSize = true;
            this.lblClientPersonalCode.Location = new System.Drawing.Point(15, 22);
            this.lblClientPersonalCode.Name = "lblClientPersonalCode";
            this.lblClientPersonalCode.Size = new System.Drawing.Size(65, 13);
            this.lblClientPersonalCode.TabIndex = 4;
            this.lblClientPersonalCode.Text = "Kliento A.K.:";
            // 
            // tbClientPersonalCode
            // 
            this.tbClientPersonalCode.Location = new System.Drawing.Point(128, 19);
            this.tbClientPersonalCode.MaxLength = 11;
            this.tbClientPersonalCode.Name = "tbClientPersonalCode";
            this.tbClientPersonalCode.Size = new System.Drawing.Size(267, 20);
            this.tbClientPersonalCode.TabIndex = 3;
            this.tbClientPersonalCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbClientPersonalCode_KeyPress);
            // 
            // cbHospital
            // 
            this.cbHospital.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHospital.FormattingEnabled = true;
            this.cbHospital.Location = new System.Drawing.Point(128, 149);
            this.cbHospital.Name = "cbHospital";
            this.cbHospital.Size = new System.Drawing.Size(267, 21);
            this.cbHospital.TabIndex = 2;
            // 
            // PersonalPharmacistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 236);
            this.Controls.Add(this.gbPersonalPhramacist);
            this.Name = "PersonalPharmacistView";
            this.Text = "Asmeninis vaistininkas";
            this.gbPersonalPhramacist.ResumeLayout(false);
            this.gbPersonalPhramacist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbPersonalPhramacist;
        private System.Windows.Forms.Label lblHospital;
        private System.Windows.Forms.Label lblClientPersonalCode;
        private System.Windows.Forms.TextBox tbClientPersonalCode;
        private System.Windows.Forms.ComboBox cbHospital;
        private System.Windows.Forms.TextBox tbDoctorSurename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDueDate;
        private System.Windows.Forms.DateTimePicker dtpDueDate;
        private System.Windows.Forms.TextBox tbDoctorName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDoctorID;
        private System.Windows.Forms.Label lblDoctorID;
    }
}