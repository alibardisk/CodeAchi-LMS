namespace CodeAchi_LMS_Search
{
    partial class FormSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.Label2 = new System.Windows.Forms.Label();
            this.txtbDatabasePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbIp = new System.Windows.Forms.TextBox();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkbServerIp = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbSqlite = new System.Windows.Forms.RadioButton();
            this.rdbMysql = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtbSchema = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbHostIp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(14, 73);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(99, 17);
            this.Label2.TabIndex = 6117;
            this.Label2.Text = "Database Path :";
            // 
            // txtbDatabasePath
            // 
            this.txtbDatabasePath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDatabasePath.ForeColor = System.Drawing.Color.Black;
            this.txtbDatabasePath.Location = new System.Drawing.Point(17, 94);
            this.txtbDatabasePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbDatabasePath.MaxLength = 500;
            this.txtbDatabasePath.Name = "txtbDatabasePath";
            this.txtbDatabasePath.ReadOnly = true;
            this.txtbDatabasePath.Size = new System.Drawing.Size(278, 25);
            this.txtbDatabasePath.TabIndex = 6116;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 6119;
            this.label1.Text = "Sever Mechine Ip :";
            // 
            // txtbIp
            // 
            this.txtbIp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbIp.ForeColor = System.Drawing.Color.Black;
            this.txtbIp.Location = new System.Drawing.Point(17, 42);
            this.txtbIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtbIp.MaxLength = 500;
            this.txtbIp.Name = "txtbIp";
            this.txtbIp.Size = new System.Drawing.Size(278, 25);
            this.txtbIp.TabIndex = 6118;
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.BackColor = System.Drawing.Color.Silver;
            this.btnBrowsePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowsePath.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBrowsePath.FlatAppearance.BorderSize = 2;
            this.btnBrowsePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowsePath.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowsePath.Location = new System.Drawing.Point(301, 91);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(38, 30);
            this.btnBrowsePath.TabIndex = 6128;
            this.btnBrowsePath.Text = "...";
            this.btnBrowsePath.UseVisualStyleBackColor = false;
            this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(75)))), ((int)(((byte)(73)))));
            this.btnTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTest.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTest.FlatAppearance.BorderSize = 2;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.Location = new System.Drawing.Point(120, 271);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(140, 30);
            this.btnTest.TabIndex = 6127;
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
            this.btnUpdate.Location = new System.Drawing.Point(265, 271);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(77, 30);
            this.btnUpdate.TabIndex = 6129;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.EnabledChanged += new System.EventHandler(this.btnUpdate_EnabledChanged);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // chkbServerIp
            // 
            this.chkbServerIp.AutoSize = true;
            this.chkbServerIp.Location = new System.Drawing.Point(17, 24);
            this.chkbServerIp.Name = "chkbServerIp";
            this.chkbServerIp.Size = new System.Drawing.Size(15, 14);
            this.chkbServerIp.TabIndex = 6130;
            this.chkbServerIp.UseVisualStyleBackColor = true;
            this.chkbServerIp.CheckedChanged += new System.EventHandler(this.chkbServerIp_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbSqlite);
            this.groupBox1.Controls.Add(this.chkbServerIp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtbDatabasePath);
            this.groupBox1.Controls.Add(this.btnBrowsePath);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.txtbIp);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 131);
            this.groupBox1.TabIndex = 6131;
            this.groupBox1.TabStop = false;
            // 
            // rdbSqlite
            // 
            this.rdbSqlite.AutoSize = true;
            this.rdbSqlite.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSqlite.Location = new System.Drawing.Point(6, -2);
            this.rdbSqlite.Name = "rdbSqlite";
            this.rdbSqlite.Size = new System.Drawing.Size(64, 21);
            this.rdbSqlite.TabIndex = 0;
            this.rdbSqlite.TabStop = true;
            this.rdbSqlite.Text = "SQLite";
            this.rdbSqlite.UseVisualStyleBackColor = true;
            this.rdbSqlite.CheckedChanged += new System.EventHandler(this.rdbSqlite_CheckedChanged);
            // 
            // rdbMysql
            // 
            this.rdbMysql.AutoSize = true;
            this.rdbMysql.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMysql.Location = new System.Drawing.Point(6, -2);
            this.rdbMysql.Name = "rdbMysql";
            this.rdbMysql.Size = new System.Drawing.Size(63, 21);
            this.rdbMysql.TabIndex = 0;
            this.rdbMysql.TabStop = true;
            this.rdbMysql.Text = "MySql";
            this.rdbMysql.UseVisualStyleBackColor = true;
            this.rdbMysql.CheckedChanged += new System.EventHandler(this.rdbMysql_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtbSchema);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtbPassword);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtbUserName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtbHostIp);
            this.groupBox2.Controls.Add(this.rdbMysql);
            this.groupBox2.Location = new System.Drawing.Point(3, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(347, 126);
            this.groupBox2.TabIndex = 6132;
            this.groupBox2.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(176, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 17);
            this.label11.TabIndex = 6138;
            this.label11.Text = "Schema Name :";
            // 
            // txtbSchema
            // 
            this.txtbSchema.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSchema.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSchema.ForeColor = System.Drawing.Color.Black;
            this.txtbSchema.Location = new System.Drawing.Point(179, 93);
            this.txtbSchema.MaxLength = 500;
            this.txtbSchema.Name = "txtbSchema";
            this.txtbSchema.Size = new System.Drawing.Size(160, 25);
            this.txtbSchema.TabIndex = 6137;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(8, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 17);
            this.label8.TabIndex = 6136;
            this.label8.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(11, 93);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(160, 25);
            this.txtbPassword.TabIndex = 6135;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(176, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 6134;
            this.label7.Text = "User Name :";
            // 
            // txtbUserName
            // 
            this.txtbUserName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbUserName.ForeColor = System.Drawing.Color.Black;
            this.txtbUserName.Location = new System.Drawing.Point(179, 41);
            this.txtbUserName.MaxLength = 500;
            this.txtbUserName.Name = "txtbUserName";
            this.txtbUserName.Size = new System.Drawing.Size(160, 25);
            this.txtbUserName.TabIndex = 6133;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(8, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 6132;
            this.label4.Text = "Server :";
            // 
            // txtbHostIp
            // 
            this.txtbHostIp.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbHostIp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbHostIp.ForeColor = System.Drawing.Color.Black;
            this.txtbHostIp.Location = new System.Drawing.Point(11, 41);
            this.txtbHostIp.MaxLength = 500;
            this.txtbHostIp.Name = "txtbHostIp";
            this.txtbHostIp.Size = new System.Drawing.Size(160, 25);
            this.txtbHostIp.TabIndex = 6131;
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 308);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSetting_FormClosing);
            this.Load += new System.EventHandler(this.FormSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormSetting_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtbDatabasePath;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbIp;
        internal System.Windows.Forms.Button btnBrowsePath;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkbServerIp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbSqlite;
        private System.Windows.Forms.RadioButton rdbMysql;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox txtbSchema;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtbUserName;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbHostIp;
    }
}