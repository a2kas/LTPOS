using System.Drawing;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    partial class Form3CompensatedView
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
            this.Text = "3 Formos kompensuojamas receptas";

            this.tbDiseaseCode = new System.Windows.Forms.TextBox();
            this.tbCardNo = new System.Windows.Forms.TextBox();
            this.cbCompensationCode = new System.Windows.Forms.ComboBox();
            this.tbCompensatedSum = new System.Windows.Forms.TextBox();
            this.tbPrepaymentCompensatedSum = new System.Windows.Forms.TextBox();
            this.lblDiseaseCode = new System.Windows.Forms.Label();
            this.lblCardNo = new System.Windows.Forms.Label();
            this.lblCompensationCode = new System.Windows.Forms.Label();
            this.lblCompensatedSum = new System.Windows.Forms.Label();
            this.lblPrepaymentCompensatedSum = new System.Windows.Forms.Label();

            this.tbDiseaseCode.Dock = DockStyle.Fill;
            this.tbDiseaseCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbDiseaseCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;

            this.tbCardNo.Dock = DockStyle.Fill;
            this.cbCompensationCode.Dock = DockStyle.Fill;
            this.tbCompensatedSum.Dock = DockStyle.Fill;
            this.tbPrepaymentCompensatedSum.Dock = DockStyle.Fill;

            this.lblDiseaseCode.Text = "Ligos kodas*:";
            this.lblDiseaseCode.TextAlign = ContentAlignment.MiddleCenter;
            this.lblDiseaseCode.Dock = DockStyle.Fill;
            this.lblDiseaseCode.AutoSize = true;

            this.lblCardNo.Text = "AAGA arba ISAS kortelės Nr.*:";
            this.lblCardNo.TextAlign = ContentAlignment.MiddleCenter;
            this.lblCardNo.Dock = DockStyle.Fill;
            this.lblCardNo.AutoSize = true;

            this.lblCompensationCode.Text = "Kompensavimo rūšies kodas*:";
            this.lblCompensationCode.TextAlign = ContentAlignment.MiddleCenter;
            this.lblCompensationCode.Dock = DockStyle.Fill;
            this.lblCompensationCode.AutoSize = true;

            this.lblCompensatedSum.Text = "Kompensuojama suma*:";
            this.lblCompensatedSum.TextAlign = ContentAlignment.MiddleCenter;
            this.lblCompensatedSum.Dock = DockStyle.Fill;
            this.lblCompensatedSum.AutoSize = true;

            this.lblPrepaymentCompensatedSum.Text = "Priemokos komp. suma*:";
            this.lblPrepaymentCompensatedSum.TextAlign = ContentAlignment.MiddleCenter;
            this.lblPrepaymentCompensatedSum.Dock = DockStyle.Fill;
            this.lblPrepaymentCompensatedSum.AutoSize = true;

            tlpPaperRecipeData.SuspendLayout();
            tlpPaperRecipeData.RowCount += 3;

            for (int i = 0; i < 3; i++)
            {
                tlpPaperRecipeData.RowStyles.Insert(i, new RowStyle(SizeType.Absolute, 25));
            }

            foreach (Control control in tlpPaperRecipeData.Controls)
            {
                int row = tlpPaperRecipeData.GetRow(control);
                tlpPaperRecipeData.SetRow(control, row + 3);
            }

            tlpPaperRecipeData.Controls.Add(lblDiseaseCode, 0, 0);
            tlpPaperRecipeData.Controls.Add(tbDiseaseCode, 1, 0);

            tlpPaperRecipeData.Controls.Add(lblCardNo, 0, 1);
            tlpPaperRecipeData.Controls.Add(tbCardNo, 1, 1);

            tlpPaperRecipeData.Controls.Add(lblCompensationCode, 0, 2);
            tlpPaperRecipeData.Controls.Add(cbCompensationCode, 1, 2);

            tlpPaperRecipeData.ResumeLayout(false);
            tlpPaperRecipeData.PerformLayout();

            tlpDispenseInfo.SuspendLayout();

            tlpDispenseInfo.Controls.Add(lblCompensatedSum, 0, 8);
            tlpDispenseInfo.Controls.Add(tbCompensatedSum, 1, 8);

            tlpDispenseInfo.Controls.Add(lblPrepaymentCompensatedSum, 0, 9);
            tlpDispenseInfo.Controls.Add(tbPrepaymentCompensatedSum, 1, 9);

            tlpDispenseInfo.ResumeLayout(false);
            tlpDispenseInfo.PerformLayout();

        }

        private System.Windows.Forms.TextBox tbDiseaseCode;
        private System.Windows.Forms.TextBox tbCardNo;
        private System.Windows.Forms.ComboBox cbCompensationCode;
        private System.Windows.Forms.TextBox tbCompensatedSum;
        private System.Windows.Forms.TextBox tbPrepaymentCompensatedSum;
        private System.Windows.Forms.Label lblDiseaseCode;
        private System.Windows.Forms.Label lblCardNo;
        private System.Windows.Forms.Label lblCompensationCode;
        private System.Windows.Forms.Label lblCompensatedSum;
        private System.Windows.Forms.Label lblPrepaymentCompensatedSum;

        #endregion
    }
}