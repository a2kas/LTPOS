namespace POS_display.Views.Erecipe.PaperRecipe
{
    partial class PaperRecipeSelectionView
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
            this.cbRecipeForms = new System.Windows.Forms.ComboBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblRecipeForms = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbRecipeForms
            // 
            this.cbRecipeForms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRecipeForms.FormattingEnabled = true;
            this.cbRecipeForms.Location = new System.Drawing.Point(103, 12);
            this.cbRecipeForms.Name = "cbRecipeForms";
            this.cbRecipeForms.Size = new System.Drawing.Size(445, 21);
            this.cbRecipeForms.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(422, 44);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(126, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Pildyti";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblRecipeForms
            // 
            this.lblRecipeForms.AutoSize = true;
            this.lblRecipeForms.Location = new System.Drawing.Point(12, 15);
            this.lblRecipeForms.Name = "lblRecipeForms";
            this.lblRecipeForms.Size = new System.Drawing.Size(85, 13);
            this.lblRecipeForms.TabIndex = 2;
            this.lblRecipeForms.Text = "Recepto formos:";
            // 
            // PaperRecipeSelectionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 79);
            this.Controls.Add(this.lblRecipeForms);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cbRecipeForms);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PaperRecipeSelectionView";
            this.ShowIcon = false;
            this.Text = "Recepto forma";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbRecipeForms;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblRecipeForms;
    }
}