namespace CodeAchi_Library_Management_System
{
    partial class FormOpacSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpacSetting));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMainUrl = new System.Windows.Forms.Label();
            this.txtbSubUrl = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.pcbIcon = new System.Windows.Forms.PictureBox();
            this.pcbClose = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.lblNotification = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.btnSavePass = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Set OPAC Login Url :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(294, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Set Default OPAC Login Password for Member :";
            // 
            // lblMainUrl
            // 
            this.lblMainUrl.AutoSize = true;
            this.lblMainUrl.ForeColor = System.Drawing.Color.MediumBlue;
            this.lblMainUrl.Location = new System.Drawing.Point(18, 55);
            this.lblMainUrl.Name = "lblMainUrl";
            this.lblMainUrl.Size = new System.Drawing.Size(177, 17);
            this.lblMainUrl.TabIndex = 7;
            this.lblMainUrl.Text = "www.codeachilms.com/opac/";
            // 
            // txtbSubUrl
            // 
            this.txtbSubUrl.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSubUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbSubUrl.ForeColor = System.Drawing.Color.MediumBlue;
            this.txtbSubUrl.Location = new System.Drawing.Point(197, 57);
            this.txtbSubUrl.Name = "txtbSubUrl";
            this.txtbSubUrl.Size = new System.Drawing.Size(238, 18);
            this.txtbSubUrl.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(0, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 1);
            this.panel1.TabIndex = 6117;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Silver;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(1, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(416, 18);
            this.label14.TabIndex = 6116;
            this.label14.Text = "OPAC Setting";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pcbIcon
            // 
            this.pcbIcon.BackColor = System.Drawing.Color.Silver;
            this.pcbIcon.Image = ((System.Drawing.Image)(resources.GetObject("pcbIcon.Image")));
            this.pcbIcon.Location = new System.Drawing.Point(5, 2);
            this.pcbIcon.Name = "pcbIcon";
            this.pcbIcon.Size = new System.Drawing.Size(16, 16);
            this.pcbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbIcon.TabIndex = 6144;
            this.pcbIcon.TabStop = false;
            // 
            // pcbClose
            // 
            this.pcbClose.BackColor = System.Drawing.Color.DimGray;
            this.pcbClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcbClose.ErrorImage = null;
            this.pcbClose.Image = ((System.Drawing.Image)(resources.GetObject("pcbClose.Image")));
            this.pcbClose.Location = new System.Drawing.Point(418, 0);
            this.pcbClose.Name = "pcbClose";
            this.pcbClose.Size = new System.Drawing.Size(33, 20);
            this.pcbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbClose.TabIndex = 6145;
            this.pcbClose.TabStop = false;
            this.pcbClose.Click += new System.EventHandler(this.pcbClose_Click);
            this.pcbClose.MouseEnter += new System.EventHandler(this.pcbClose_MouseEnter);
            this.pcbClose.MouseLeave += new System.EventHandler(this.pcbClose_MouseLeave);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(174)))), ((int)(((byte)(30)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Coral;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(335, 81);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 6147;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCheck
            // 
            this.btnCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(75)))), ((int)(((byte)(73)))));
            this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCheck.FlatAppearance.BorderSize = 0;
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheck.ForeColor = System.Drawing.Color.White;
            this.btnCheck.Location = new System.Drawing.Point(197, 81);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(131, 30);
            this.btnCheck.TabIndex = 6146;
            this.btnCheck.Text = "Check Availability";
            this.btnCheck.UseVisualStyleBackColor = false;
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Location = new System.Drawing.Point(194, 114);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(43, 17);
            this.lblNotification.TabIndex = 6148;
            this.lblNotification.Text = "label4";
            // 
            // txtbPassword
            // 
            this.txtbPassword.Location = new System.Drawing.Point(307, 141);
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(128, 25);
            this.txtbPassword.TabIndex = 6149;
            // 
            // btnSavePass
            // 
            this.btnSavePass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(174)))), ((int)(((byte)(30)))));
            this.btnSavePass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSavePass.FlatAppearance.BorderColor = System.Drawing.Color.Coral;
            this.btnSavePass.FlatAppearance.BorderSize = 0;
            this.btnSavePass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSavePass.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSavePass.ForeColor = System.Drawing.Color.White;
            this.btnSavePass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSavePass.Location = new System.Drawing.Point(335, 179);
            this.btnSavePass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSavePass.Name = "btnSavePass";
            this.btnSavePass.Size = new System.Drawing.Size(100, 30);
            this.btnSavePass.TabIndex = 6150;
            this.btnSavePass.Text = "Save";
            this.btnSavePass.UseVisualStyleBackColor = false;
            // 
            // FormOpacSetting
            // 
            this.ClientSize = new System.Drawing.Size(451, 229);
            this.Controls.Add(this.btnSavePass);
            this.Controls.Add(this.txtbPassword);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.pcbClose);
            this.Controls.Add(this.pcbIcon);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtbSubUrl);
            this.Controls.Add(this.lblMainUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormOpacSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormOpacSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormOpacSetting_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMainUrl;
        private System.Windows.Forms.TextBox txtbSubUrl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pcbIcon;
        internal System.Windows.Forms.PictureBox pcbClose;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.Button btnSavePass;
    }
}