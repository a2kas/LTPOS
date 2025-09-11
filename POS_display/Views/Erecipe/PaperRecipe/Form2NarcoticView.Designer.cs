using System.Drawing;
using System.Windows.Forms;

namespace POS_display.Views.Erecipe.PaperRecipe
{
    partial class Form2NarcoticView
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
            this.Text = "2 Formos receptas (narkotinių vaistų)";

            this.tbRecipeSerial = new System.Windows.Forms.TextBox();
            this.tbRecipeNumber = new System.Windows.Forms.TextBox();
            this.cbCompensationCode = new System.Windows.Forms.ComboBox();
            this.lblRecipeSerial = new System.Windows.Forms.Label();
            this.lblRecipeNumber = new System.Windows.Forms.Label();
            this.lblCompensationCode = new System.Windows.Forms.Label();

            this.tbRecipeSerial.Dock = DockStyle.Fill;
            this.tbRecipeNumber.Dock = DockStyle.Fill;
            this.cbCompensationCode.Dock = DockStyle.Fill;

            this.lblRecipeSerial.Text = "Recepto serija*:";
            this.lblRecipeSerial.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRecipeSerial.Dock = DockStyle.Fill;
            this.lblRecipeSerial.AutoSize = true;

            this.lblRecipeNumber.Text = "Recepto numeris*:";
            this.lblRecipeNumber.TextAlign = ContentAlignment.MiddleCenter;
            this.lblRecipeNumber.Dock = DockStyle.Fill;
            this.lblRecipeNumber.AutoSize = true;

            this.lblCompensationCode.Text = "Kompensavimo rūšies kodas*:";
            this.lblCompensationCode.TextAlign = ContentAlignment.MiddleCenter;
            this.lblCompensationCode.Dock = DockStyle.Fill;
            this.lblCompensationCode.AutoSize = true;

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

            tlpPaperRecipeData.Controls.Add(lblRecipeSerial, 0, 0);
            tlpPaperRecipeData.Controls.Add(tbRecipeSerial, 1, 0);

            tlpPaperRecipeData.Controls.Add(lblRecipeNumber, 0, 1);
            tlpPaperRecipeData.Controls.Add(tbRecipeNumber, 1, 1);

            tlpPaperRecipeData.Controls.Add(lblCompensationCode, 0, 2);
            tlpPaperRecipeData.Controls.Add(cbCompensationCode, 1, 2);

            tlpPaperRecipeData.ResumeLayout(false);
            tlpPaperRecipeData.PerformLayout();
        }

        private System.Windows.Forms.TextBox tbRecipeSerial;
        private System.Windows.Forms.TextBox tbRecipeNumber;
        private System.Windows.Forms.ComboBox cbCompensationCode;
        private System.Windows.Forms.Label lblRecipeSerial;
        private System.Windows.Forms.Label lblRecipeNumber;
        private System.Windows.Forms.Label lblCompensationCode;
        #endregion
    }
}