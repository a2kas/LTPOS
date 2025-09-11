namespace POS_display.Views.CRM
{
    partial class LoyaltyCardTypeView
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
            this.rbLoyaltyCard = new System.Windows.Forms.RadioButton();
            this.rbB2BCard = new System.Windows.Forms.RadioButton();
            this.btnCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbLoyaltyCard
            // 
            this.rbLoyaltyCard.AutoSize = true;
            this.rbLoyaltyCard.Checked = true;
            this.rbLoyaltyCard.Location = new System.Drawing.Point(12, 12);
            this.rbLoyaltyCard.Name = "rbLoyaltyCard";
            this.rbLoyaltyCard.Size = new System.Drawing.Size(102, 17);
            this.rbLoyaltyCard.TabIndex = 0;
            this.rbLoyaltyCard.TabStop = true;
            this.rbLoyaltyCard.Text = "Lojalumo kortelė";
            this.rbLoyaltyCard.UseVisualStyleBackColor = true;
            // 
            // rbB2BCard
            // 
            this.rbB2BCard.AutoSize = true;
            this.rbB2BCard.Location = new System.Drawing.Point(12, 35);
            this.rbB2BCard.Name = "rbB2BCard";
            this.rbB2BCard.Size = new System.Drawing.Size(84, 17);
            this.rbB2BCard.TabIndex = 1;
            this.rbB2BCard.Text = "B2B klientas";
            this.rbB2BCard.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(12, 120);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(146, 23);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Kurti klientą";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // LoyaltyCardTypeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 155);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.rbB2BCard);
            this.Controls.Add(this.rbLoyaltyCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoyaltyCardTypeView";
            this.ShowIcon = false;
            this.Text = "Naujo kliento kūrimas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbLoyaltyCard;
        private System.Windows.Forms.RadioButton rbB2BCard;
        private System.Windows.Forms.Button btnCreate;
    }
}