namespace POS_display.Helpers
{
    partial class FormTest
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageKVAP = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblGetSPSTPLDetails = new System.Windows.Forms.Label();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.tbSPSTPL = new System.Windows.Forms.TextBox();
            this.lblSPSTPL = new System.Windows.Forms.Label();
            this.btnGetSPSTPLDetails = new System.Windows.Forms.Button();
            this.tabPagePOS = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gbLogs = new System.Windows.Forms.GroupBox();
            this.rtbResponses = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbCommand = new System.Windows.Forms.TextBox();
            this.lblArgs = new System.Windows.Forms.Label();
            this.lblCommand = new System.Windows.Forms.Label();
            this.cbCommand = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tblEncryption = new System.Windows.Forms.TableLayoutPanel();
            this.gbEncryptedMsg = new System.Windows.Forms.GroupBox();
            this.rtbEncryptedMsg = new System.Windows.Forms.RichTextBox();
            this.gbMsgDecrypt = new System.Windows.Forms.GroupBox();
            this.rtbMsgToDecrypt = new System.Windows.Forms.RichTextBox();
            this.gbDecryptedMsg = new System.Windows.Forms.GroupBox();
            this.rtbDecryptedMsg = new System.Windows.Forms.RichTextBox();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.btnEcrypt = new System.Windows.Forms.Button();
            this.gbMsgToEncrypt = new System.Windows.Forms.GroupBox();
            this.rtbMsgToEncrypt = new System.Windows.Forms.RichTextBox();
            this.tabControl.SuspendLayout();
            this.tabPageKVAP.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.tabPagePOS.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbLogs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tblEncryption.SuspendLayout();
            this.gbEncryptedMsg.SuspendLayout();
            this.gbMsgDecrypt.SuspendLayout();
            this.gbDecryptedMsg.SuspendLayout();
            this.gbMsgToEncrypt.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageKVAP);
            this.tabControl.Controls.Add(this.tabPagePOS);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1223, 775);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageKVAP
            // 
            this.tabPageKVAP.Controls.Add(this.tableLayoutPanel);
            this.tabPageKVAP.Location = new System.Drawing.Point(4, 22);
            this.tabPageKVAP.Name = "tabPageKVAP";
            this.tabPageKVAP.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKVAP.Size = new System.Drawing.Size(1215, 749);
            this.tabPageKVAP.TabIndex = 0;
            this.tabPageKVAP.Text = "KVAP";
            this.tabPageKVAP.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 11;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 174F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 513F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.lblGetSPSTPLDetails, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.rtbLogs, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tbSPSTPL, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.lblSPSTPL, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.btnGetSPSTPLDetails, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.86598F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.13402F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 645F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1209, 743);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // lblGetSPSTPLDetails
            // 
            this.lblGetSPSTPLDetails.AutoSize = true;
            this.lblGetSPSTPLDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGetSPSTPLDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblGetSPSTPLDetails.Location = new System.Drawing.Point(3, 0);
            this.lblGetSPSTPLDetails.Name = "lblGetSPSTPLDetails";
            this.lblGetSPSTPLDetails.Size = new System.Drawing.Size(297, 28);
            this.lblGetSPSTPLDetails.TabIndex = 1;
            this.lblGetSPSTPLDetails.Text = "GetSPSTPLDetails";
            this.lblGetSPSTPLDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbLogs
            // 
            this.rtbLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel.SetColumnSpan(this.rtbLogs, 11);
            this.rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLogs.Location = new System.Drawing.Point(3, 100);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(1203, 640);
            this.rtbLogs.TabIndex = 0;
            this.rtbLogs.Text = "";
            // 
            // tbSPSTPL
            // 
            this.tbSPSTPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSPSTPL.Location = new System.Drawing.Point(385, 3);
            this.tbSPSTPL.Name = "tbSPSTPL";
            this.tbSPSTPL.Size = new System.Drawing.Size(168, 20);
            this.tbSPSTPL.TabIndex = 1;
            // 
            // lblSPSTPL
            // 
            this.lblSPSTPL.AutoSize = true;
            this.lblSPSTPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSPSTPL.Location = new System.Drawing.Point(306, 0);
            this.lblSPSTPL.Name = "lblSPSTPL";
            this.lblSPSTPL.Size = new System.Drawing.Size(73, 28);
            this.lblSPSTPL.TabIndex = 2;
            this.lblSPSTPL.Text = "SPSTPL:";
            this.lblSPSTPL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGetSPSTPLDetails
            // 
            this.btnGetSPSTPLDetails.Location = new System.Drawing.Point(559, 3);
            this.btnGetSPSTPLDetails.Name = "btnGetSPSTPLDetails";
            this.btnGetSPSTPLDetails.Size = new System.Drawing.Size(75, 19);
            this.btnGetSPSTPLDetails.TabIndex = 3;
            this.btnGetSPSTPLDetails.Text = "Call";
            this.btnGetSPSTPLDetails.UseVisualStyleBackColor = true;
            this.btnGetSPSTPLDetails.Click += new System.EventHandler(this.btnGetSPSTPLDetails_Click);
            // 
            // tabPagePOS
            // 
            this.tabPagePOS.Controls.Add(this.tableLayoutPanel1);
            this.tabPagePOS.Location = new System.Drawing.Point(4, 22);
            this.tabPagePOS.Name = "tabPagePOS";
            this.tabPagePOS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePOS.Size = new System.Drawing.Size(1215, 749);
            this.tabPagePOS.TabIndex = 1;
            this.tabPagePOS.Text = "POS";
            this.tabPagePOS.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.57028F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.42972F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 569F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel1.Controls.Add(this.gbLogs, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSend, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbCommand, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblArgs, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCommand, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbCommand, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44.15585F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.84415F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 665F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1209, 743);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gbLogs
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbLogs, 5);
            this.gbLogs.Controls.Add(this.rtbResponses);
            this.gbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogs.Location = new System.Drawing.Point(3, 37);
            this.gbLogs.Name = "gbLogs";
            this.tableLayoutPanel1.SetRowSpan(this.gbLogs, 2);
            this.gbLogs.Size = new System.Drawing.Size(1203, 703);
            this.gbLogs.TabIndex = 3;
            this.gbLogs.TabStop = false;
            this.gbLogs.Text = "Responses:";
            // 
            // rtbResponses
            // 
            this.rtbResponses.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbResponses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbResponses.Location = new System.Drawing.Point(3, 16);
            this.rtbResponses.Name = "rtbResponses";
            this.rtbResponses.Size = new System.Drawing.Size(1197, 684);
            this.rtbResponses.TabIndex = 0;
            this.rtbResponses.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSend.Location = new System.Drawing.Point(1138, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(68, 28);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbCommand
            // 
            this.tbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.tbCommand.Location = new System.Drawing.Point(569, 3);
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.Size = new System.Drawing.Size(563, 26);
            this.tbCommand.TabIndex = 0;
            // 
            // lblArgs
            // 
            this.lblArgs.AutoSize = true;
            this.lblArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblArgs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblArgs.Location = new System.Drawing.Point(381, 0);
            this.lblArgs.Name = "lblArgs";
            this.lblArgs.Size = new System.Drawing.Size(182, 34);
            this.lblArgs.TabIndex = 1;
            this.lblArgs.Text = "Arguments:";
            this.lblArgs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.lblCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.lblCommand.Location = new System.Drawing.Point(3, 0);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(155, 34);
            this.lblCommand.TabIndex = 4;
            this.lblCommand.Text = "Command:";
            this.lblCommand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCommand
            // 
            this.cbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.cbCommand.FormattingEnabled = true;
            this.cbCommand.Items.AddRange(new object[] {
            "177"});
            this.cbCommand.Location = new System.Drawing.Point(164, 3);
            this.cbCommand.Name = "cbCommand";
            this.cbCommand.Size = new System.Drawing.Size(211, 28);
            this.cbCommand.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tblEncryption);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1215, 749);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Encryption";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tblEncryption
            // 
            this.tblEncryption.ColumnCount = 2;
            this.tblEncryption.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblEncryption.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblEncryption.Controls.Add(this.gbEncryptedMsg, 1, 2);
            this.tblEncryption.Controls.Add(this.gbMsgDecrypt, 0, 1);
            this.tblEncryption.Controls.Add(this.gbDecryptedMsg, 0, 2);
            this.tblEncryption.Controls.Add(this.btnDecrypt, 0, 0);
            this.tblEncryption.Controls.Add(this.btnEcrypt, 1, 0);
            this.tblEncryption.Controls.Add(this.gbMsgToEncrypt, 1, 1);
            this.tblEncryption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblEncryption.Location = new System.Drawing.Point(3, 3);
            this.tblEncryption.Name = "tblEncryption";
            this.tblEncryption.RowCount = 3;
            this.tblEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tblEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.tblEncryption.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47F));
            this.tblEncryption.Size = new System.Drawing.Size(1209, 743);
            this.tblEncryption.TabIndex = 0;
            // 
            // gbEncryptedMsg
            // 
            this.gbEncryptedMsg.Controls.Add(this.rtbEncryptedMsg);
            this.gbEncryptedMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbEncryptedMsg.Location = new System.Drawing.Point(607, 396);
            this.gbEncryptedMsg.Name = "gbEncryptedMsg";
            this.gbEncryptedMsg.Size = new System.Drawing.Size(599, 344);
            this.gbEncryptedMsg.TabIndex = 5;
            this.gbEncryptedMsg.TabStop = false;
            this.gbEncryptedMsg.Text = "Ecrypted message";
            // 
            // rtbEncryptedMsg
            // 
            this.rtbEncryptedMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbEncryptedMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbEncryptedMsg.Location = new System.Drawing.Point(3, 16);
            this.rtbEncryptedMsg.Name = "rtbEncryptedMsg";
            this.rtbEncryptedMsg.Size = new System.Drawing.Size(593, 325);
            this.rtbEncryptedMsg.TabIndex = 0;
            this.rtbEncryptedMsg.Text = "";
            // 
            // gbMsgDecrypt
            // 
            this.gbMsgDecrypt.Controls.Add(this.rtbMsgToDecrypt);
            this.gbMsgDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMsgDecrypt.Location = new System.Drawing.Point(3, 47);
            this.gbMsgDecrypt.Name = "gbMsgDecrypt";
            this.gbMsgDecrypt.Size = new System.Drawing.Size(598, 343);
            this.gbMsgDecrypt.TabIndex = 0;
            this.gbMsgDecrypt.TabStop = false;
            this.gbMsgDecrypt.Text = "Message to decrypt";
            // 
            // rtbMsgToDecrypt
            // 
            this.rtbMsgToDecrypt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMsgToDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMsgToDecrypt.Location = new System.Drawing.Point(3, 16);
            this.rtbMsgToDecrypt.Name = "rtbMsgToDecrypt";
            this.rtbMsgToDecrypt.Size = new System.Drawing.Size(592, 324);
            this.rtbMsgToDecrypt.TabIndex = 0;
            this.rtbMsgToDecrypt.Text = "";
            // 
            // gbDecryptedMsg
            // 
            this.gbDecryptedMsg.Controls.Add(this.rtbDecryptedMsg);
            this.gbDecryptedMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDecryptedMsg.Location = new System.Drawing.Point(3, 396);
            this.gbDecryptedMsg.Name = "gbDecryptedMsg";
            this.gbDecryptedMsg.Size = new System.Drawing.Size(598, 344);
            this.gbDecryptedMsg.TabIndex = 1;
            this.gbDecryptedMsg.TabStop = false;
            this.gbDecryptedMsg.Text = "Decrypted message";
            // 
            // rtbDecryptedMsg
            // 
            this.rtbDecryptedMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbDecryptedMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDecryptedMsg.Location = new System.Drawing.Point(3, 16);
            this.rtbDecryptedMsg.Name = "rtbDecryptedMsg";
            this.rtbDecryptedMsg.Size = new System.Drawing.Size(592, 325);
            this.rtbDecryptedMsg.TabIndex = 0;
            this.rtbDecryptedMsg.Text = "";
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDecrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnDecrypt.Location = new System.Drawing.Point(3, 3);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(598, 38);
            this.btnDecrypt.TabIndex = 2;
            this.btnDecrypt.Text = "Decrypt";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // btnEcrypt
            // 
            this.btnEcrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEcrypt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.btnEcrypt.Location = new System.Drawing.Point(607, 3);
            this.btnEcrypt.Name = "btnEcrypt";
            this.btnEcrypt.Size = new System.Drawing.Size(599, 38);
            this.btnEcrypt.TabIndex = 3;
            this.btnEcrypt.Text = "Encrypt";
            this.btnEcrypt.UseVisualStyleBackColor = true;
            this.btnEcrypt.Click += new System.EventHandler(this.btnEcrypt_Click);
            // 
            // gbMsgToEncrypt
            // 
            this.gbMsgToEncrypt.Controls.Add(this.rtbMsgToEncrypt);
            this.gbMsgToEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMsgToEncrypt.Location = new System.Drawing.Point(607, 47);
            this.gbMsgToEncrypt.Name = "gbMsgToEncrypt";
            this.gbMsgToEncrypt.Size = new System.Drawing.Size(599, 343);
            this.gbMsgToEncrypt.TabIndex = 4;
            this.gbMsgToEncrypt.TabStop = false;
            this.gbMsgToEncrypt.Text = "Message to encrypt";
            // 
            // rtbMsgToEncrypt
            // 
            this.rtbMsgToEncrypt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMsgToEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbMsgToEncrypt.Location = new System.Drawing.Point(3, 16);
            this.rtbMsgToEncrypt.Name = "rtbMsgToEncrypt";
            this.rtbMsgToEncrypt.Size = new System.Drawing.Size(593, 324);
            this.rtbMsgToEncrypt.TabIndex = 0;
            this.rtbMsgToEncrypt.Text = "";
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 775);
            this.Controls.Add(this.tabControl);
            this.Name = "FormTest";
            this.ShowIcon = false;
            this.Text = "TEST ENV";
            this.tabControl.ResumeLayout(false);
            this.tabPageKVAP.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tabPagePOS.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbLogs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tblEncryption.ResumeLayout(false);
            this.gbEncryptedMsg.ResumeLayout(false);
            this.gbMsgDecrypt.ResumeLayout(false);
            this.gbDecryptedMsg.ResumeLayout(false);
            this.gbMsgToEncrypt.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageKVAP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Label lblGetSPSTPLDetails;
        private System.Windows.Forms.TextBox tbSPSTPL;
        private System.Windows.Forms.Label lblSPSTPL;
        private System.Windows.Forms.Button btnGetSPSTPLDetails;
        private System.Windows.Forms.TabPage tabPagePOS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbCommand;
        private System.Windows.Forms.Label lblArgs;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox gbLogs;
        private System.Windows.Forms.RichTextBox rtbResponses;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.ComboBox cbCommand;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tblEncryption;
        private System.Windows.Forms.GroupBox gbMsgDecrypt;
        private System.Windows.Forms.GroupBox gbDecryptedMsg;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.RichTextBox rtbMsgToDecrypt;
        private System.Windows.Forms.RichTextBox rtbDecryptedMsg;
        private System.Windows.Forms.GroupBox gbEncryptedMsg;
        private System.Windows.Forms.Button btnEcrypt;
        private System.Windows.Forms.GroupBox gbMsgToEncrypt;
        private System.Windows.Forms.RichTextBox rtbEncryptedMsg;
        private System.Windows.Forms.RichTextBox rtbMsgToEncrypt;
    }
}