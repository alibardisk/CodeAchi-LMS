namespace CodeAchi_Library_Management_System
{
    partial class FormSmsSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSmsSetting));
            this.label3 = new System.Windows.Forms.Label();
            this.txtbApi = new System.Windows.Forms.RichTextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnBlocked = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lstbBlockedList = new System.Windows.Forms.ListBox();
            this.txtbId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unblockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtbSenderId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(12, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 17);
            this.label3.TabIndex = 6131;
            this.label3.Text = "SMS API :";
            // 
            // txtbApi
            // 
            this.txtbApi.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbApi.Location = new System.Drawing.Point(15, 27);
            this.txtbApi.Name = "txtbApi";
            this.txtbApi.Size = new System.Drawing.Size(284, 116);
            this.txtbApi.TabIndex = 6132;
            this.txtbApi.Text = "";
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
            this.btnSave.Location = new System.Drawing.Point(204, 278);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 30);
            this.btnSave.TabIndex = 6161;
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
            this.btnTest.Location = new System.Drawing.Point(102, 278);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 30);
            this.btnTest.TabIndex = 6160;
            this.btnTest.Text = "Test SMS";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
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
            this.btnBlocked.Location = new System.Drawing.Point(445, 55);
            this.btnBlocked.Name = "btnBlocked";
            this.btnBlocked.Size = new System.Drawing.Size(80, 30);
            this.btnBlocked.TabIndex = 6169;
            this.btnBlocked.Text = "Block";
            this.btnBlocked.UseVisualStyleBackColor = false;
            this.btnBlocked.Click += new System.EventHandler(this.btnBlocked_Click);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(341, 88);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(184, 17);
            this.label9.TabIndex = 6168;
            this.label9.Text = "Blocked Contact";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lstbBlockedList
            // 
            this.lstbBlockedList.BackColor = System.Drawing.Color.Gainsboro;
            this.lstbBlockedList.FormattingEnabled = true;
            this.lstbBlockedList.ItemHeight = 17;
            this.lstbBlockedList.Location = new System.Drawing.Point(341, 105);
            this.lstbBlockedList.Name = "lstbBlockedList";
            this.lstbBlockedList.Size = new System.Drawing.Size(184, 174);
            this.lstbBlockedList.TabIndex = 6167;
            this.lstbBlockedList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstbBlockedList_MouseDown);
            // 
            // txtbId
            // 
            this.txtbId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbId.ForeColor = System.Drawing.Color.Black;
            this.txtbId.Location = new System.Drawing.Point(325, 27);
            this.txtbId.MaxLength = 500;
            this.txtbId.Name = "txtbId";
            this.txtbId.Size = new System.Drawing.Size(200, 25);
            this.txtbId.TabIndex = 6165;
            this.txtbId.TextChanged += new System.EventHandler(this.txtbId_TextChanged);
            this.txtbId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbId_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(322, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 17);
            this.label8.TabIndex = 6166;
            this.label8.Text = "Contact No to Block :";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Location = new System.Drawing.Point(311, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 305);
            this.panel2.TabIndex = 6164;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(319, 286);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(211, 15);
            this.label19.TabIndex = 6171;
            this.label19.Text = "* To unblock a contact right click on it.";
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
            // txtbSenderId
            // 
            this.txtbSenderId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSenderId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSenderId.ForeColor = System.Drawing.Color.Black;
            this.txtbSenderId.Location = new System.Drawing.Point(15, 247);
            this.txtbSenderId.MaxLength = 500;
            this.txtbSenderId.Name = "txtbSenderId";
            this.txtbSenderId.Size = new System.Drawing.Size(284, 25);
            this.txtbSenderId.TabIndex = 6158;
            this.txtbSenderId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbSenderId_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(12, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 17);
            this.label5.TabIndex = 6159;
            this.label5.Text = "Contact No to Test :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(12, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 45);
            this.label1.TabIndex = 6172;
            this.label1.Text = "Replace [$Message$] for message body and \r\n[$ContactNumber$] for receiver contact" +
    " from\r\nthe api url.";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Location = new System.Drawing.Point(2, 226);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(307, 1);
            this.panel3.TabIndex = 6173;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(3, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(300, 30);
            this.label2.TabIndex = 6175;
            this.label2.Text = "* To copy the keyword please double click on it && press \r\nCtrl+V to paste it.";
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
            this.btnTutorial.Location = new System.Drawing.Point(273, 7);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654673;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // FormSmsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 314);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.btnBlocked);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lstbBlockedList);
            this.Controls.Add(this.txtbId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbSenderId);
            this.Controls.Add(this.txtbApi);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSmsSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SMS Setting";
            this.Load += new System.EventHandler(this.FormSmsSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormSmsSetting_Paint);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox txtbApi;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Button btnBlocked;
        internal System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lstbBlockedList;
        internal System.Windows.Forms.TextBox txtbId;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label label19;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem unblockToolStripMenuItem;
        internal System.Windows.Forms.TextBox txtbSenderId;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
    }
}