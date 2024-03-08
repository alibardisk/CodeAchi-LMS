namespace CodeAchi_LMS_Search
{
    partial class FormUserCreation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUserCreation));
            this.label2 = new System.Windows.Forms.Label();
            this.txtbBrrId = new System.Windows.Forms.TextBox();
            this.txtbBrrMail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtbUserId = new System.Windows.Forms.TextBox();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtbConfirm = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(9, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 6141;
            this.label2.Text = "Borrower Id :";
            // 
            // txtbBrrId
            // 
            this.txtbBrrId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbBrrId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrId.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrId.Location = new System.Drawing.Point(132, 8);
            this.txtbBrrId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbBrrId.Name = "txtbBrrId";
            this.txtbBrrId.Size = new System.Drawing.Size(250, 25);
            this.txtbBrrId.TabIndex = 0;
            this.txtbBrrId.TextChanged += new System.EventHandler(this.txtbBrrId_TextChanged);
            this.txtbBrrId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbBrrId_KeyDown);
            // 
            // txtbBrrMail
            // 
            this.txtbBrrMail.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrMail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrMail.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrMail.Location = new System.Drawing.Point(132, 72);
            this.txtbBrrMail.MaxLength = 500;
            this.txtbBrrMail.Name = "txtbBrrMail";
            this.txtbBrrMail.Size = new System.Drawing.Size(250, 25);
            this.txtbBrrMail.TabIndex = 1;
            this.txtbBrrMail.Enter += new System.EventHandler(this.txtbBrrMail_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(9, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 6150;
            this.label1.Text = "Email Id :";
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label17.ForeColor = System.Drawing.Color.Red;
            this.Label17.Location = new System.Drawing.Point(0, 75);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(14, 17);
            this.Label17.TabIndex = 6149;
            this.Label17.Text = "*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(9, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 17);
            this.label5.TabIndex = 6153;
            this.label5.Text = "User Id :";
            // 
            // txtbUserId
            // 
            this.txtbUserId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbUserId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbUserId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbUserId.ForeColor = System.Drawing.Color.Black;
            this.txtbUserId.Location = new System.Drawing.Point(132, 41);
            this.txtbUserId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbUserId.Name = "txtbUserId";
            this.txtbUserId.ReadOnly = true;
            this.txtbUserId.Size = new System.Drawing.Size(250, 25);
            this.txtbUserId.TabIndex = 6152;
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(132, 105);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(250, 25);
            this.txtbPassword.TabIndex = 2;
            this.txtbPassword.TextChanged += new System.EventHandler(this.txtbPassword_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(9, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 17);
            this.label7.TabIndex = 6155;
            this.label7.Text = "Password :";
            // 
            // txtbConfirm
            // 
            this.txtbConfirm.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbConfirm.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbConfirm.ForeColor = System.Drawing.Color.Black;
            this.txtbConfirm.Location = new System.Drawing.Point(132, 137);
            this.txtbConfirm.MaxLength = 500;
            this.txtbConfirm.Name = "txtbConfirm";
            this.txtbConfirm.Size = new System.Drawing.Size(250, 25);
            this.txtbConfirm.TabIndex = 3;
            this.txtbConfirm.TextChanged += new System.EventHandler(this.txtbConfirm_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(9, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 17);
            this.label8.TabIndex = 6157;
            this.label8.Text = "Confirm Password :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(0, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 17);
            this.label9.TabIndex = 6158;
            this.label9.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(0, 139);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 17);
            this.label10.TabIndex = 6159;
            this.label10.Text = "*";
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnLogin.FlatAppearance.BorderSize = 2;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(282, 169);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 33);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Save";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // FormUserCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 207);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtbConfirm);
            this.Controls.Add(this.txtbPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbUserId);
            this.Controls.Add(this.txtbBrrMail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label17);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbBrrId);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserCreation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Password";
            this.Load += new System.EventHandler(this.FormUserCreation_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormUserCreation_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtbBrrId;
        internal System.Windows.Forms.TextBox txtbBrrMail;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label Label17;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtbUserId;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtbConfirm;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Button btnLogin;
    }
}