namespace CodeAchi_Library_Management_System
{
    partial class FormEmailSetting
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailSetting));
            this.label6 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.txtbMailId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rdbGmail = new System.Windows.Forms.RadioButton();
            this.rdbManual = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbPort = new System.Windows.Forms.TextBox();
            this.txtbHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSsl = new System.Windows.Forms.ComboBox();
            this.rdbOutlook = new System.Windows.Forms.RadioButton();
            this.rdbYmail = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtbSenderId = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtbId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lstbBlockedList = new System.Windows.Forms.ListBox();
            this.btnBlocked = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unblockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(11, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 17);
            this.label6.TabIndex = 6130;
            this.label6.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(88, 83);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(219, 25);
            this.txtbPassword.TabIndex = 6128;
            this.txtbPassword.TextChanged += new System.EventHandler(this.txtbPassword_TextChanged);
            // 
            // txtbMailId
            // 
            this.txtbMailId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbMailId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbMailId.ForeColor = System.Drawing.Color.Black;
            this.txtbMailId.Location = new System.Drawing.Point(88, 50);
            this.txtbMailId.MaxLength = 500;
            this.txtbMailId.Name = "txtbMailId";
            this.txtbMailId.Size = new System.Drawing.Size(219, 25);
            this.txtbMailId.TabIndex = 6127;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(11, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 17);
            this.label3.TabIndex = 6129;
            this.label3.Text = "E-mail Id :";
            // 
            // rdbGmail
            // 
            this.rdbGmail.AutoSize = true;
            this.rdbGmail.Location = new System.Drawing.Point(14, 7);
            this.rdbGmail.Name = "rdbGmail";
            this.rdbGmail.Size = new System.Drawing.Size(59, 21);
            this.rdbGmail.TabIndex = 6132;
            this.rdbGmail.TabStop = true;
            this.rdbGmail.Text = "Gmail";
            this.rdbGmail.UseVisualStyleBackColor = true;
            this.rdbGmail.CheckedChanged += new System.EventHandler(this.rdbGmail_CheckedChanged);
            // 
            // rdbManual
            // 
            this.rdbManual.AutoSize = true;
            this.rdbManual.Location = new System.Drawing.Point(238, 7);
            this.rdbManual.Name = "rdbManual";
            this.rdbManual.Size = new System.Drawing.Size(69, 21);
            this.rdbManual.TabIndex = 6131;
            this.rdbManual.TabStop = true;
            this.rdbManual.Text = "Manual";
            this.rdbManual.UseVisualStyleBackColor = true;
            this.rdbManual.CheckedChanged += new System.EventHandler(this.rdbManual_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(11, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 17);
            this.label1.TabIndex = 6136;
            this.label1.Text = "Port :";
            // 
            // txtbPort
            // 
            this.txtbPort.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPort.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPort.ForeColor = System.Drawing.Color.Black;
            this.txtbPort.Location = new System.Drawing.Point(88, 150);
            this.txtbPort.MaxLength = 500;
            this.txtbPort.Name = "txtbPort";
            this.txtbPort.Size = new System.Drawing.Size(86, 25);
            this.txtbPort.TabIndex = 6134;
            this.txtbPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbPort_KeyPress);
            // 
            // txtbHost
            // 
            this.txtbHost.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbHost.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbHost.ForeColor = System.Drawing.Color.Black;
            this.txtbHost.Location = new System.Drawing.Point(88, 116);
            this.txtbHost.MaxLength = 500;
            this.txtbHost.Name = "txtbHost";
            this.txtbHost.Size = new System.Drawing.Size(219, 25);
            this.txtbHost.TabIndex = 6133;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 6135;
            this.label2.Text = "Host :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(186, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 17);
            this.label7.TabIndex = 6138;
            this.label7.Text = "SSL :";
            // 
            // cmbSsl
            // 
            this.cmbSsl.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSsl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSsl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSsl.FormattingEnabled = true;
            this.cmbSsl.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cmbSsl.Location = new System.Drawing.Point(223, 150);
            this.cmbSsl.Name = "cmbSsl";
            this.cmbSsl.Size = new System.Drawing.Size(84, 25);
            this.cmbSsl.TabIndex = 6137;
            // 
            // rdbOutlook
            // 
            this.rdbOutlook.AutoSize = true;
            this.rdbOutlook.Location = new System.Drawing.Point(160, 7);
            this.rdbOutlook.Name = "rdbOutlook";
            this.rdbOutlook.Size = new System.Drawing.Size(72, 21);
            this.rdbOutlook.TabIndex = 6140;
            this.rdbOutlook.TabStop = true;
            this.rdbOutlook.Text = "Outlook";
            this.rdbOutlook.UseVisualStyleBackColor = true;
            this.rdbOutlook.CheckedChanged += new System.EventHandler(this.rdbOutlook_CheckedChanged);
            // 
            // rdbYmail
            // 
            this.rdbYmail.AutoSize = true;
            this.rdbYmail.Location = new System.Drawing.Point(85, 7);
            this.rdbYmail.Name = "rdbYmail";
            this.rdbYmail.Size = new System.Drawing.Size(56, 21);
            this.rdbYmail.TabIndex = 6139;
            this.rdbYmail.TabStop = true;
            this.rdbYmail.Text = "Ymail";
            this.rdbYmail.UseVisualStyleBackColor = true;
            this.rdbYmail.CheckedChanged += new System.EventHandler(this.rdbYmail_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(11, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 17);
            this.label5.TabIndex = 6142;
            this.label5.Text = "Email Id :";
            // 
            // txtbSenderId
            // 
            this.txtbSenderId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSenderId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSenderId.ForeColor = System.Drawing.Color.Black;
            this.txtbSenderId.Location = new System.Drawing.Point(88, 194);
            this.txtbSenderId.MaxLength = 500;
            this.txtbSenderId.Name = "txtbSenderId";
            this.txtbSenderId.Size = new System.Drawing.Size(219, 25);
            this.txtbSenderId.TabIndex = 6141;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Location = new System.Drawing.Point(5, 184);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(307, 1);
            this.panel3.TabIndex = 6143;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(212, 225);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 30);
            this.btnSave.TabIndex = 6145;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTest.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTest.FlatAppearance.BorderSize = 2;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(110, 225);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 30);
            this.btnTest.TabIndex = 6144;
            this.btnTest.Text = "Test Email";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblStatus.Location = new System.Drawing.Point(88, 263);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(219, 40);
            this.lblStatus.TabIndex = 6157;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Location = new System.Drawing.Point(316, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 300);
            this.panel2.TabIndex = 6158;
            // 
            // txtbId
            // 
            this.txtbId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbId.ForeColor = System.Drawing.Color.Black;
            this.txtbId.Location = new System.Drawing.Point(327, 37);
            this.txtbId.MaxLength = 500;
            this.txtbId.Name = "txtbId";
            this.txtbId.Size = new System.Drawing.Size(209, 25);
            this.txtbId.TabIndex = 6159;
            this.txtbId.TextChanged += new System.EventHandler(this.txtbId_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(324, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 17);
            this.label8.TabIndex = 6160;
            this.label8.Text = "Email Id to Block :";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(324, 106);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(212, 17);
            this.label9.TabIndex = 6162;
            this.label9.Text = "Blocked Mail";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lstbBlockedList
            // 
            this.lstbBlockedList.BackColor = System.Drawing.Color.Gainsboro;
            this.lstbBlockedList.FormattingEnabled = true;
            this.lstbBlockedList.ItemHeight = 17;
            this.lstbBlockedList.Location = new System.Drawing.Point(324, 123);
            this.lstbBlockedList.Name = "lstbBlockedList";
            this.lstbBlockedList.Size = new System.Drawing.Size(212, 157);
            this.lstbBlockedList.TabIndex = 6161;
            this.lstbBlockedList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstbBlockedList_MouseDown);
            // 
            // btnBlocked
            // 
            this.btnBlocked.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnBlocked.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBlocked.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBlocked.FlatAppearance.BorderSize = 2;
            this.btnBlocked.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlocked.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBlocked.ForeColor = System.Drawing.Color.White;
            this.btnBlocked.Location = new System.Drawing.Point(447, 68);
            this.btnBlocked.Name = "btnBlocked";
            this.btnBlocked.Size = new System.Drawing.Size(89, 30);
            this.btnBlocked.TabIndex = 6163;
            this.btnBlocked.Text = "Block";
            this.btnBlocked.UseVisualStyleBackColor = false;
            this.btnBlocked.Click += new System.EventHandler(this.btnBlocked_Click);
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(319, 284);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(207, 15);
            this.label19.TabIndex = 6165;
            this.label19.Text = "* To unblock an email right click on it.";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unblockToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // unblockToolStripMenuItem
            // 
            this.unblockToolStripMenuItem.Name = "unblockToolStripMenuItem";
            this.unblockToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.unblockToolStripMenuItem.Text = "Unblock";
            this.unblockToolStripMenuItem.Click += new System.EventHandler(this.unblockToolStripMenuItem_Click);
            // 
            // btnToolTip
            // 
            this.btnToolTip.AutoPopDelay = 5000;
            this.btnToolTip.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnToolTip.InitialDelay = 50;
            this.btnToolTip.ReshowDelay = 0;
            // 
            // btnTutorial
            // 
            this.btnTutorial.BackColor = System.Drawing.Color.Transparent;
            this.btnTutorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTutorial.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnTutorial.FlatAppearance.BorderSize = 0;
            this.btnTutorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTutorial.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTutorial.Image = ((System.Drawing.Image)(resources.GetObject("btnTutorial.Image")));
            this.btnTutorial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTutorial.Location = new System.Drawing.Point(281, 29);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654673;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // FormEmailSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 309);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.btnBlocked);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lstbBlockedList);
            this.Controls.Add(this.txtbId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbSenderId);
            this.Controls.Add(this.rdbOutlook);
            this.Controls.Add(this.rdbYmail);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbSsl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbPort);
            this.Controls.Add(this.txtbHost);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdbGmail);
            this.Controls.Add(this.rdbManual);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtbPassword);
            this.Controls.Add(this.txtbMailId);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEmailSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email Setting";
            this.Load += new System.EventHandler(this.FormEmailSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormEmailSetting_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.TextBox txtbMailId;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdbGmail;
        private System.Windows.Forms.RadioButton rdbManual;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbPort;
        internal System.Windows.Forms.TextBox txtbHost;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ComboBox cmbSsl;
        private System.Windows.Forms.RadioButton rdbOutlook;
        private System.Windows.Forms.RadioButton rdbYmail;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtbSenderId;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.TextBox txtbId;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lstbBlockedList;
        internal System.Windows.Forms.Button btnBlocked;
        internal System.Windows.Forms.Label label19;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem unblockToolStripMenuItem;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
    }
}