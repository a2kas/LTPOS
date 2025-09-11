
namespace POS_display.Popups.display1_popups.ERecipe
{
    partial class VaccineCreateUserControl
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
            this.Load += new System.EventHandler(this.VaccineCreateUserControl_Load);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.LblBarcode = new System.Windows.Forms.Label();
            this.TbBarcode = new System.Windows.Forms.TextBox();
            this.LblCreateVaccine = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.LblVaccineExpiryDate = new System.Windows.Forms.Label();
            this.LblMedicine = new System.Windows.Forms.Label();
            this.LblDoseSerialNumber = new System.Windows.Forms.Label();
            this.LblNote = new System.Windows.Forms.Label();
            this.TbMedicine = new System.Windows.Forms.TextBox();
            this.TbDoseSerialNumber = new System.Windows.Forms.TextBox();
            this.RtbNote = new System.Windows.Forms.RichTextBox();
            this.cbInfectiousDisease = new System.Windows.Forms.ComboBox();
            this.BtnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.LblBarcode, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.TbBarcode, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.LblCreateVaccine, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.BtnClose, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.LblVaccineExpiryDate, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LblMedicine, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.LblDoseSerialNumber, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.LblNote, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.TbMedicine, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.TbDoseSerialNumber, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.RtbNote, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbInfectiousDisease, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnSave, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(583, 680);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LblBarcode
            // 
            this.LblBarcode.AutoSize = true;
            this.LblBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblBarcode.Location = new System.Drawing.Point(3, 60);
            this.LblBarcode.Name = "LblBarcode";
            this.LblBarcode.Size = new System.Drawing.Size(144, 30);
            this.LblBarcode.TabIndex = 148;
            this.LblBarcode.Text = "Barkodas";
            this.LblBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TbBarcode
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.TbBarcode, 2);
            this.TbBarcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbBarcode.Location = new System.Drawing.Point(153, 63);
            this.TbBarcode.MaxLength = 20;
            this.TbBarcode.Name = "TbBarcode";
            this.TbBarcode.Size = new System.Drawing.Size(174, 23);
            this.TbBarcode.TabIndex = 149;
            this.TbBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbBarcode_KeyPress);
            // 
            // LblCreateVaccine
            // 
            this.LblCreateVaccine.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.LblCreateVaccine, 3);
            this.LblCreateVaccine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblCreateVaccine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.LblCreateVaccine.Location = new System.Drawing.Point(3, 0);
            this.LblCreateVaccine.Name = "LblCreateVaccine";
            this.LblCreateVaccine.Size = new System.Drawing.Size(324, 30);
            this.LblCreateVaccine.TabIndex = 131;
            this.LblCreateVaccine.Text = "Skiepo skyrimas";
            this.LblCreateVaccine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnClose
            // 
            this.BtnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnClose.Location = new System.Drawing.Point(486, 3);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(84, 24);
            this.BtnClose.TabIndex = 1;
            this.BtnClose.Text = "Atgal";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // LblVaccineExpiryDate
            // 
            this.LblVaccineExpiryDate.AutoSize = true;
            this.LblVaccineExpiryDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblVaccineExpiryDate.Location = new System.Drawing.Point(3, 30);
            this.LblVaccineExpiryDate.Name = "LblVaccineExpiryDate";
            this.LblVaccineExpiryDate.Size = new System.Drawing.Size(144, 30);
            this.LblVaccineExpiryDate.TabIndex = 132;
            this.LblVaccineExpiryDate.Text = "Užkrečiamosios ligos";
            this.LblVaccineExpiryDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblMedicine
            // 
            this.LblMedicine.AutoSize = true;
            this.LblMedicine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblMedicine.Location = new System.Drawing.Point(3, 90);
            this.LblMedicine.Name = "LblMedicine";
            this.LblMedicine.Size = new System.Drawing.Size(144, 30);
            this.LblMedicine.TabIndex = 133;
            this.LblMedicine.Text = "Imuninis vaistinis preparatas";
            this.LblMedicine.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblDoseSerialNumber
            // 
            this.LblDoseSerialNumber.AutoSize = true;
            this.LblDoseSerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblDoseSerialNumber.Location = new System.Drawing.Point(3, 120);
            this.LblDoseSerialNumber.Name = "LblDoseSerialNumber";
            this.LblDoseSerialNumber.Size = new System.Drawing.Size(144, 30);
            this.LblDoseSerialNumber.TabIndex = 134;
            this.LblDoseSerialNumber.Text = "Dozinės eilės numeris";
            this.LblDoseSerialNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LblNote
            // 
            this.LblNote.AutoSize = true;
            this.LblNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblNote.Location = new System.Drawing.Point(3, 150);
            this.LblNote.Name = "LblNote";
            this.LblNote.Size = new System.Drawing.Size(144, 30);
            this.LblNote.TabIndex = 135;
            this.LblNote.Text = "Pastabos";
            this.LblNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TbMedicine
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.TbMedicine, 3);
            this.TbMedicine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbMedicine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbMedicine.Location = new System.Drawing.Point(153, 93);
            this.TbMedicine.MaxLength = 20;
            this.TbMedicine.Name = "TbMedicine";
            this.TbMedicine.ReadOnly = true;
            this.TbMedicine.Size = new System.Drawing.Size(327, 23);
            this.TbMedicine.TabIndex = 143;
            // 
            // TbDoseSerialNumber
            // 
            this.TbDoseSerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbDoseSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.TbDoseSerialNumber.Location = new System.Drawing.Point(153, 123);
            this.TbDoseSerialNumber.MaxLength = 5;
            this.TbDoseSerialNumber.Name = "TbDoseSerialNumber";
            this.TbDoseSerialNumber.Size = new System.Drawing.Size(84, 23);
            this.TbDoseSerialNumber.TabIndex = 141;
            this.TbDoseSerialNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbDoseSerialNumber_KeyPress);
            // 
            // RtbNote
            // 
            this.RtbNote.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.RtbNote, 3);
            this.RtbNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtbNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.RtbNote.Location = new System.Drawing.Point(153, 153);
            this.RtbNote.Name = "RtbNote";
            this.tableLayoutPanel1.SetRowSpan(this.RtbNote, 2);
            this.RtbNote.Size = new System.Drawing.Size(327, 54);
            this.RtbNote.TabIndex = 146;
            this.RtbNote.Text = "";
            // 
            // cbInfectiousDisease
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbInfectiousDisease, 2);
            this.cbInfectiousDisease.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbInfectiousDisease.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInfectiousDisease.FormattingEnabled = true;
            this.cbInfectiousDisease.Location = new System.Drawing.Point(153, 33);
            this.cbInfectiousDisease.Name = "cbInfectiousDisease";
            this.cbInfectiousDisease.Size = new System.Drawing.Size(174, 21);
            this.cbInfectiousDisease.TabIndex = 147;
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(3, 213);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(144, 24);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "Išsaugoti";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // VaccineCreateUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "VaccineCreateUserControl";
            this.Size = new System.Drawing.Size(583, 680);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox TbDoseSerialNumber;
        private System.Windows.Forms.Label LblNote;
        private System.Windows.Forms.Label LblDoseSerialNumber;
        private System.Windows.Forms.Label LblMedicine;
        private System.Windows.Forms.Label LblVaccineExpiryDate;
        private System.Windows.Forms.Label LblCreateVaccine;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.TextBox TbMedicine;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.RichTextBox RtbNote;
        private System.Windows.Forms.ComboBox cbInfectiousDisease;
        private System.Windows.Forms.Label LblBarcode;
        private System.Windows.Forms.TextBox TbBarcode;
    }
}
