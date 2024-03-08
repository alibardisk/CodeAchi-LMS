namespace CodeAchi_LMS_Search
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblCompany1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.txtbMailId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDesignation = new System.Windows.Forms.Label();
            this.lblRefresh = new System.Windows.Forms.LinkLabel();
            this.pcbCompany = new System.Windows.Forms.PictureBox();
            this.pcbUsrImage = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbUsrImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnLogin.FlatAppearance.BorderSize = 2;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(46, 286);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 32);
            this.btnLogin.TabIndex = 6094;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblCompany1
            // 
            this.lblCompany1.AutoSize = true;
            this.lblCompany1.BackColor = System.Drawing.Color.Black;
            this.lblCompany1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblCompany1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCompany1.Location = new System.Drawing.Point(-3, 329);
            this.lblCompany1.Name = "lblCompany1";
            this.lblCompany1.Size = new System.Drawing.Size(316, 13);
            this.lblCompany1.TabIndex = 6093;
            this.lblCompany1.Text = "© 2015-2019 | Developed by CodeAchi Technologies Pvt. Ltd.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(43, 231);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 17);
            this.label6.TabIndex = 6092;
            this.label6.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(46, 251);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(220, 25);
            this.txtbPassword.TabIndex = 6090;
            this.txtbPassword.TextChanged += new System.EventHandler(this.txtbPassword_TextChanged);
            this.txtbPassword.Enter += new System.EventHandler(this.txtbPassword_Enter);
            this.txtbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbPassword_KeyDown);
            this.txtbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbPassword_KeyPress);
            // 
            // txtbMailId
            // 
            this.txtbMailId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbMailId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbMailId.ForeColor = System.Drawing.Color.Black;
            this.txtbMailId.Location = new System.Drawing.Point(46, 200);
            this.txtbMailId.MaxLength = 500;
            this.txtbMailId.Name = "txtbMailId";
            this.txtbMailId.Size = new System.Drawing.Size(220, 25);
            this.txtbMailId.TabIndex = 6089;
            this.txtbMailId.TextChanged += new System.EventHandler(this.txtbMailId_TextChanged);
            this.txtbMailId.Enter += new System.EventHandler(this.txtbMailId_Enter);
            this.txtbMailId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbMailId_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(43, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 6091;
            this.label3.Text = "User Id :";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDesignation);
            this.panel1.Location = new System.Drawing.Point(-3, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 54);
            this.panel1.TabIndex = 6088;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblDesignation
            // 
            this.lblDesignation.AutoSize = true;
            this.lblDesignation.Location = new System.Drawing.Point(3, 0);
            this.lblDesignation.Name = "lblDesignation";
            this.lblDesignation.Size = new System.Drawing.Size(43, 17);
            this.lblDesignation.TabIndex = 6144;
            this.lblDesignation.Text = "label1";
            // 
            // lblRefresh
            // 
            this.lblRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefresh.AutoSize = true;
            this.lblRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefresh.Location = new System.Drawing.Point(185, 299);
            this.lblRefresh.Name = "lblRefresh";
            this.lblRefresh.Size = new System.Drawing.Size(85, 17);
            this.lblRefresh.TabIndex = 6143;
            this.lblRefresh.TabStop = true;
            this.lblRefresh.Text = "Set Password";
            this.lblRefresh.Visible = false;
            this.lblRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRefresh_LinkClicked);
            // 
            // pcbCompany
            // 
            this.pcbCompany.Image = ((System.Drawing.Image)(resources.GetObject("pcbCompany.Image")));
            this.pcbCompany.Location = new System.Drawing.Point(-2, 0);
            this.pcbCompany.Name = "pcbCompany";
            this.pcbCompany.Size = new System.Drawing.Size(315, 71);
            this.pcbCompany.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcbCompany.TabIndex = 6086;
            this.pcbCompany.TabStop = false;
            // 
            // pcbUsrImage
            // 
            this.pcbUsrImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pcbUsrImage.BackColor = System.Drawing.Color.Transparent;
            this.pcbUsrImage.Image = ((System.Drawing.Image)(resources.GetObject("pcbUsrImage.Image")));
            this.pcbUsrImage.Location = new System.Drawing.Point(110, 78);
            this.pcbUsrImage.Name = "pcbUsrImage";
            this.pcbUsrImage.Size = new System.Drawing.Size(89, 92);
            this.pcbUsrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbUsrImage.TabIndex = 6087;
            this.pcbUsrImage.TabStop = false;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 343);
            this.Controls.Add(this.lblRefresh);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblCompany1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtbPassword);
            this.Controls.Add(this.txtbMailId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pcbCompany);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pcbUsrImage);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormLogin_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbUsrImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblCompany1;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.TextBox txtbMailId;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.PictureBox pcbCompany;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.PictureBox pcbUsrImage;
        internal System.Windows.Forms.LinkLabel lblRefresh;
        internal System.Windows.Forms.Label lblDesignation;
    }
}