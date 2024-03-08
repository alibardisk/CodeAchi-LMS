namespace CodeAchi_Library_Management_System
{
    partial class FormDatabaseSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseSetting));
            this.Label2 = new System.Windows.Forms.Label();
            this.txtbDatabasePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbBackupPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numHour = new System.Windows.Forms.NumericUpDown();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnBrowsePath = new System.Windows.Forms.Button();
            this.btnBrowseBackupLocation = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtbHostIp = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkbIp = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtbSchema = new System.Windows.Forms.TextBox();
            this.btnMigrate = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbMysql = new System.Windows.Forms.RadioButton();
            this.rdbSqlite = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtbBackupFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseBackupFile = new System.Windows.Forms.Button();
            this.panelMigration = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTable = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bWorkerBackup = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelMigration.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(36, 44);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(99, 17);
            this.Label2.TabIndex = 6054;
            this.Label2.Text = "Database Path :";
            // 
            // txtbDatabasePath
            // 
            this.txtbDatabasePath.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbDatabasePath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDatabasePath.ForeColor = System.Drawing.Color.Black;
            this.txtbDatabasePath.Location = new System.Drawing.Point(39, 64);
            this.txtbDatabasePath.MaxLength = 500;
            this.txtbDatabasePath.Name = "txtbDatabasePath";
            this.txtbDatabasePath.ReadOnly = true;
            this.txtbDatabasePath.Size = new System.Drawing.Size(389, 25);
            this.txtbDatabasePath.TabIndex = 6053;
            this.txtbDatabasePath.TextChanged += new System.EventHandler(this.txtbDatabasePath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(13, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 6056;
            this.label1.Text = "Backup Location :";
            // 
            // txtbBackupPath
            // 
            this.txtbBackupPath.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBackupPath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBackupPath.ForeColor = System.Drawing.Color.Black;
            this.txtbBackupPath.Location = new System.Drawing.Point(131, 261);
            this.txtbBackupPath.MaxLength = 500;
            this.txtbBackupPath.Name = "txtbBackupPath";
            this.txtbBackupPath.ReadOnly = true;
            this.txtbBackupPath.Size = new System.Drawing.Size(324, 25);
            this.txtbBackupPath.TabIndex = 6055;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 6058;
            this.label3.Text = "Backup per :";
            // 
            // numHour
            // 
            this.numHour.BackColor = System.Drawing.Color.Gainsboro;
            this.numHour.Location = new System.Drawing.Point(126, 65);
            this.numHour.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numHour.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numHour.Name = "numHour";
            this.numHour.Size = new System.Drawing.Size(96, 25);
            this.numHour.TabIndex = 6059;
            this.numHour.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            this.btnTest.Location = new System.Drawing.Point(207, 182);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(119, 30);
            this.btnTest.TabIndex = 6098;
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
            this.btnUpdate.Location = new System.Drawing.Point(333, 182);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(77, 30);
            this.btnUpdate.TabIndex = 6099;
            this.btnUpdate.Text = "Save";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.EnabledChanged += new System.EventHandler(this.btnUpdate_EnabledChanged);
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnBrowsePath
            // 
            this.btnBrowsePath.BackColor = System.Drawing.Color.Silver;
            this.btnBrowsePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowsePath.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBrowsePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowsePath.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowsePath.Location = new System.Drawing.Point(457, 61);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(77, 30);
            this.btnBrowsePath.TabIndex = 6109;
            this.btnBrowsePath.Text = "Browse";
            this.btnBrowsePath.UseVisualStyleBackColor = false;
            this.btnBrowsePath.Click += new System.EventHandler(this.btnBrowsePath_Click);
            // 
            // btnBrowseBackupLocation
            // 
            this.btnBrowseBackupLocation.BackColor = System.Drawing.Color.Silver;
            this.btnBrowseBackupLocation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseBackupLocation.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBrowseBackupLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseBackupLocation.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBackupLocation.Location = new System.Drawing.Point(457, 23);
            this.btnBrowseBackupLocation.Name = "btnBrowseBackupLocation";
            this.btnBrowseBackupLocation.Size = new System.Drawing.Size(77, 30);
            this.btnBrowseBackupLocation.TabIndex = 6110;
            this.btnBrowseBackupLocation.Text = "Browse";
            this.btnBrowseBackupLocation.UseVisualStyleBackColor = false;
            this.btnBrowseBackupLocation.Click += new System.EventHandler(this.btnBrowseBackupLocation_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnRestore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestore.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRestore.FlatAppearance.BorderSize = 2;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(405, 62);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(129, 30);
            this.btnRestore.TabIndex = 6111;
            this.btnRestore.Text = "Restore  Database";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(235, 302);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 17);
            this.label5.TabIndex = 6115;
            this.label5.Text = "Minutes";
            // 
            // txtbHostIp
            // 
            this.txtbHostIp.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbHostIp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbHostIp.ForeColor = System.Drawing.Color.Black;
            this.txtbHostIp.Location = new System.Drawing.Point(39, 136);
            this.txtbHostIp.MaxLength = 500;
            this.txtbHostIp.Name = "txtbHostIp";
            this.txtbHostIp.Size = new System.Drawing.Size(160, 25);
            this.txtbHostIp.TabIndex = 6125;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(104, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 15);
            this.label9.TabIndex = 6126;
            this.label9.Text = "(Use for LAN Connection)";
            this.label9.Visible = false;
            // 
            // chkbIp
            // 
            this.chkbIp.AutoSize = true;
            this.chkbIp.Location = new System.Drawing.Point(7, 68);
            this.chkbIp.Name = "chkbIp";
            this.chkbIp.Size = new System.Drawing.Size(76, 21);
            this.chkbIp.TabIndex = 6127;
            this.chkbIp.Text = "Host Ip :";
            this.chkbIp.UseVisualStyleBackColor = true;
            this.chkbIp.Visible = false;
            this.chkbIp.CheckedChanged += new System.EventHandler(this.chkbIp_CheckedChanged);
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
            this.btnSave.Location = new System.Drawing.Point(457, 63);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 30);
            this.btnSave.TabIndex = 6128;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtbSchema);
            this.groupBox1.Controls.Add(this.btnMigrate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtbPassword);
            this.groupBox1.Controls.Add(this.btnBrowsePath);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnUpdate);
            this.groupBox1.Controls.Add(this.txtbUserName);
            this.groupBox1.Controls.Add(this.txtbDatabasePath);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.rdbMysql);
            this.groupBox1.Controls.Add(this.txtbHostIp);
            this.groupBox1.Controls.Add(this.rdbSqlite);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 228);
            this.groupBox1.TabIndex = 6129;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Database";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(36, 166);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 17);
            this.label11.TabIndex = 6135;
            this.label11.Text = "Schema Name :";
            // 
            // txtbSchema
            // 
            this.txtbSchema.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSchema.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSchema.ForeColor = System.Drawing.Color.Black;
            this.txtbSchema.Location = new System.Drawing.Point(39, 186);
            this.txtbSchema.MaxLength = 500;
            this.txtbSchema.Name = "txtbSchema";
            this.txtbSchema.Size = new System.Drawing.Size(160, 25);
            this.txtbSchema.TabIndex = 6134;
            // 
            // btnMigrate
            // 
            this.btnMigrate.BackColor = System.Drawing.Color.Silver;
            this.btnMigrate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMigrate.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnMigrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMigrate.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMigrate.Location = new System.Drawing.Point(417, 182);
            this.btnMigrate.Name = "btnMigrate";
            this.btnMigrate.Size = new System.Drawing.Size(117, 30);
            this.btnMigrate.TabIndex = 6133;
            this.btnMigrate.Text = "Migrate Database";
            this.btnMigrate.UseVisualStyleBackColor = false;
            this.btnMigrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(371, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 17);
            this.label8.TabIndex = 6132;
            this.label8.Text = "Password :";
            // 
            // txtbPassword
            // 
            this.txtbPassword.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPassword.ForeColor = System.Drawing.Color.Black;
            this.txtbPassword.Location = new System.Drawing.Point(374, 136);
            this.txtbPassword.MaxLength = 500;
            this.txtbPassword.Name = "txtbPassword";
            this.txtbPassword.Size = new System.Drawing.Size(160, 25);
            this.txtbPassword.TabIndex = 6131;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(204, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 17);
            this.label7.TabIndex = 6130;
            this.label7.Text = "User Name :";
            // 
            // txtbUserName
            // 
            this.txtbUserName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbUserName.ForeColor = System.Drawing.Color.Black;
            this.txtbUserName.Location = new System.Drawing.Point(207, 136);
            this.txtbUserName.MaxLength = 500;
            this.txtbUserName.Name = "txtbUserName";
            this.txtbUserName.Size = new System.Drawing.Size(160, 25);
            this.txtbUserName.TabIndex = 6129;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(36, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 17);
            this.label4.TabIndex = 6128;
            this.label4.Text = "Server :";
            // 
            // rdbMysql
            // 
            this.rdbMysql.AutoSize = true;
            this.rdbMysql.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMysql.Location = new System.Drawing.Point(16, 92);
            this.rdbMysql.Name = "rdbMysql";
            this.rdbMysql.Size = new System.Drawing.Size(63, 21);
            this.rdbMysql.TabIndex = 1;
            this.rdbMysql.TabStop = true;
            this.rdbMysql.Text = "MySql";
            this.rdbMysql.UseVisualStyleBackColor = true;
            this.rdbMysql.CheckedChanged += new System.EventHandler(this.rdbMysql_CheckedChanged);
            // 
            // rdbSqlite
            // 
            this.rdbSqlite.AutoSize = true;
            this.rdbSqlite.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSqlite.Location = new System.Drawing.Point(16, 20);
            this.rdbSqlite.Name = "rdbSqlite";
            this.rdbSqlite.Size = new System.Drawing.Size(64, 21);
            this.rdbSqlite.TabIndex = 0;
            this.rdbSqlite.TabStop = true;
            this.rdbSqlite.Text = "SQLite";
            this.rdbSqlite.UseVisualStyleBackColor = true;
            this.rdbSqlite.CheckedChanged += new System.EventHandler(this.rdbSqlite_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnBrowseBackupLocation);
            this.groupBox2.Controls.Add(this.numHour);
            this.groupBox2.Location = new System.Drawing.Point(5, 235);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(544, 111);
            this.groupBox2.TabIndex = 6130;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Backup Database";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtbBackupFile);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btnBrowseBackupFile);
            this.groupBox3.Controls.Add(this.btnRestore);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chkbIp);
            this.groupBox3.Location = new System.Drawing.Point(5, 352);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(544, 101);
            this.groupBox3.TabIndex = 6131;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Restore Database";
            // 
            // txtbBackupFile
            // 
            this.txtbBackupFile.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBackupFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBackupFile.ForeColor = System.Drawing.Color.Black;
            this.txtbBackupFile.Location = new System.Drawing.Point(98, 29);
            this.txtbBackupFile.MaxLength = 500;
            this.txtbBackupFile.Name = "txtbBackupFile";
            this.txtbBackupFile.ReadOnly = true;
            this.txtbBackupFile.Size = new System.Drawing.Size(352, 25);
            this.txtbBackupFile.TabIndex = 6129;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(13, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 17);
            this.label6.TabIndex = 6056;
            this.label6.Text = "Backup File :";
            // 
            // btnBrowseBackupFile
            // 
            this.btnBrowseBackupFile.BackColor = System.Drawing.Color.Silver;
            this.btnBrowseBackupFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowseBackupFile.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBrowseBackupFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseBackupFile.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseBackupFile.Location = new System.Drawing.Point(457, 26);
            this.btnBrowseBackupFile.Name = "btnBrowseBackupFile";
            this.btnBrowseBackupFile.Size = new System.Drawing.Size(77, 30);
            this.btnBrowseBackupFile.TabIndex = 6110;
            this.btnBrowseBackupFile.Text = "Browse";
            this.btnBrowseBackupFile.UseVisualStyleBackColor = false;
            this.btnBrowseBackupFile.Click += new System.EventHandler(this.btnBrowseBackupFile_Click);
            // 
            // panelMigration
            // 
            this.panelMigration.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMigration.Controls.Add(this.lblCount);
            this.panelMigration.Controls.Add(this.label10);
            this.panelMigration.Controls.Add(this.lblTable);
            this.panelMigration.Controls.Add(this.progressBar1);
            this.panelMigration.Location = new System.Drawing.Point(4, 3);
            this.panelMigration.Name = "panelMigration";
            this.panelMigration.Size = new System.Drawing.Size(545, 450);
            this.panelMigration.TabIndex = 6134;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(106, 168);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(15, 17);
            this.lblCount.TabIndex = 3;
            this.lblCount.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 168);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 17);
            this.label10.TabIndex = 2;
            this.label10.Text = "Record Count :";
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTable.Location = new System.Drawing.Point(5, 99);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(0, 21);
            this.lblTable.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 136);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(531, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // bWorkerBackup
            // 
            this.bWorkerBackup.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorkerBackup_DoWork);
            // 
            // FormDatabaseSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 458);
            this.Controls.Add(this.panelMigration);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbBackupPath);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDatabaseSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Setting";
            this.Load += new System.EventHandler(this.FormDatabaseSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormDatabaseSetting_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelMigration.ResumeLayout(false);
            this.panelMigration.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtbDatabasePath;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbBackupPath;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numHour;
        internal System.Windows.Forms.Button btnTest;
        internal System.Windows.Forms.Button btnUpdate;
        internal System.Windows.Forms.Button btnBrowsePath;
        internal System.Windows.Forms.Button btnBrowseBackupLocation;
        internal System.Windows.Forms.Button btnRestore;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtbHostIp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkbIp;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        internal System.Windows.Forms.TextBox txtbBackupFile;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button btnBrowseBackupFile;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.TextBox txtbPassword;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtbUserName;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbMysql;
        private System.Windows.Forms.RadioButton rdbSqlite;
        internal System.Windows.Forms.Button btnMigrate;
        private System.Windows.Forms.Panel panelMigration;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTable;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox txtbSchema;
        private System.ComponentModel.BackgroundWorker bWorkerBackup;
    }
}