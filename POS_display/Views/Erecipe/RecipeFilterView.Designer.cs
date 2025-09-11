namespace POS_display.Views.Erecipe
{
    partial class RecipeFilterView
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
            this.tbActiveSubtance = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblActiveSubstance = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbActiveSubtance
            // 
            this.tbActiveSubtance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbActiveSubtance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbActiveSubtance.Location = new System.Drawing.Point(116, 15);
            this.tbActiveSubtance.Name = "tbActiveSubtance";
            this.tbActiveSubtance.Size = new System.Drawing.Size(357, 20);
            this.tbActiveSubtance.TabIndex = 0;
            this.tbActiveSubtance.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbActiveSubtance_KeyDown);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(12, 56);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Taikyti";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Anuliuoti";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblActiveSubstance
            // 
            this.lblActiveSubstance.AutoSize = true;
            this.lblActiveSubstance.Location = new System.Drawing.Point(13, 18);
            this.lblActiveSubstance.Name = "lblActiveSubstance";
            this.lblActiveSubstance.Size = new System.Drawing.Size(97, 13);
            this.lblActiveSubstance.TabIndex = 3;
            this.lblActiveSubstance.Text = "Aktyvioji medžiaga:";
            // 
            // RecipeFilterView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 91);
            this.Controls.Add(this.lblActiveSubstance);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbActiveSubtance);
            this.Name = "RecipeFilterView";
            this.ShowIcon = false;
            this.Text = "Filtras";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbActiveSubtance;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblActiveSubstance;
    }
}