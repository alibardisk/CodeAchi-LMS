
namespace CodeAchi_Library_Management_System
{
    partial class FormServerConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServerConnection));
            this.label11 = new System.Windows.Forms.Label();
            this.txtbSchema = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbHostIp = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(179, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 17);
            this.label11.TabIndex = 6145;
            this.label11.Text = "Schema Name :";
            // 
            // txtbSchema
            // 
            this.txtbSchema.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSchema.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSchema.ForeColor = System.Drawing.Color.Black;
            this.txtbSchema.Location = new System.Drawing.Point(182, 77);
            this.txtbSchema.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbSchema.MaxLength = 500;
            this.txtbSchema.Name = "txtbSchema";
            this.txtbSchema.Size = new System.Drawing.Size(156, 25);
            this.txtbSchema.TabIndex = 6144;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(5, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 17);
            this.label8.TabIndex = 6143;
            this.label8.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(8, 77);
            this.txtbPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(156, 25);
            this.txtbPassword.TabIndex = 6142;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(179, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 6141;
            this.label7.Text = "User Name :";
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
            this.btnTest.Location = new System.Drawing.Point(101, 110);
            this.btnTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(139, 34);
            this.btnTest.TabIndex = 6136;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.FlatAppearance.BorderSize = 2;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.Location = new System.Drawing.Point(248, 110);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 34);
            this.btnUpdate.TabIndex = 6137;
            this.btnUpdate.Text = "Save";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtbUserName
            // 
            this.txtbUserName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbUserName.ForeColor = System.Drawing.Color.Black;
            this.txtbUserName.Location = new System.Drawing.Point(182, 26);
            this.txtbUserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbUserName.MaxLength = 500;
            this.txtbUserName.Name = "txtbUserName";
            this.txtbUserName.Size = new System.Drawing.Size(156, 25);
            this.txtbUserName.TabIndex = 6140;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(5, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 6139;
            this.label4.Text = "Server :";
            // 
            // txtbHostIp
            // 
            this.txtbHostIp.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbHostIp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbHostIp.ForeColor = System.Drawing.Color.Black;
            this.txtbHostIp.Location = new System.Drawing.Point(8, 26);
            this.txtbHostIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbHostIp.MaxLength = 500;
            this.txtbHostIp.Name = "txtbHostIp";
            this.txtbHostIp.Size = new System.Drawing.Size(156, 25);
            this.txtbHostIp.TabIndex = 6138;
            // 
            // FormServerConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 148);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtbSchema);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtbPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtbUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbHostIp);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServerConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormServerConnection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServerConnection_FormClosing);
            this.Load += new System.EventHandler(this.FormServerConnection_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormServerConnection_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox txtbSchema;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Button btnUpdate;
        internal System.Windows.Forms.TextBox txtbUserName;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbHostIp;
    }
}