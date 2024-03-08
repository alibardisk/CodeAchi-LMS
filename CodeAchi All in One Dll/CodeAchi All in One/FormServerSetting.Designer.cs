namespace CodeAchi_AllinOne
{
    partial class FormServerSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormServerSetting));
            this.Label7 = new System.Windows.Forms.Label();
            this.btnblankDb = new System.Windows.Forms.Button();
            this.cmbAuthentication = new System.Windows.Forms.ComboBox();
            this.lnkLblSearch = new System.Windows.Forms.LinkLabel();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.lblSet = new System.Windows.Forms.Label();
            this.panelDownload = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panelDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(10, 63);
            this.Label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(92, 15);
            this.Label7.TabIndex = 31;
            this.Label7.Text = "Authentication :";
            // 
            // btnblankDb
            // 
            this.btnblankDb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnblankDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnblankDb.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnblankDb.FlatAppearance.BorderSize = 2;
            this.btnblankDb.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnblankDb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnblankDb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnblankDb.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnblankDb.ForeColor = System.Drawing.Color.Red;
            this.btnblankDb.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnblankDb.Location = new System.Drawing.Point(227, 162);
            this.btnblankDb.Margin = new System.Windows.Forms.Padding(4);
            this.btnblankDb.Name = "btnblankDb";
            this.btnblankDb.Size = new System.Drawing.Size(186, 29);
            this.btnblankDb.TabIndex = 33;
            this.btnblankDb.Text = "Create Database && Proceed";
            this.btnblankDb.UseVisualStyleBackColor = false;
            this.btnblankDb.Click += new System.EventHandler(this.btnblankDb_Click);
            // 
            // cmbAuthentication
            // 
            this.cmbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthentication.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAuthentication.FormattingEnabled = true;
            this.cmbAuthentication.Items.AddRange(new object[] {
            "Select an authentication...",
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cmbAuthentication.Location = new System.Drawing.Point(150, 58);
            this.cmbAuthentication.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAuthentication.Name = "cmbAuthentication";
            this.cmbAuthentication.Size = new System.Drawing.Size(262, 25);
            this.cmbAuthentication.TabIndex = 24;
            this.cmbAuthentication.SelectedIndexChanged += new System.EventHandler(this.cmbAuthentication_SelectedIndexChanged);
            // 
            // lnkLblSearch
            // 
            this.lnkLblSearch.AutoSize = true;
            this.lnkLblSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLblSearch.LinkColor = System.Drawing.Color.Red;
            this.lnkLblSearch.Location = new System.Drawing.Point(315, 37);
            this.lnkLblSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkLblSearch.Name = "lnkLblSearch";
            this.lnkLblSearch.Size = new System.Drawing.Size(97, 17);
            this.lnkLblSearch.TabIndex = 23;
            this.lnkLblSearch.TabStop = true;
            this.lnkLblSearch.Text = "Search Servers";
            this.lnkLblSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblSearch_LinkClicked);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnTestConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestConnection.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTestConnection.FlatAppearance.BorderSize = 2;
            this.btnTestConnection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnTestConnection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestConnection.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnTestConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestConnection.Location = new System.Drawing.Point(81, 162);
            this.btnTestConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(138, 29);
            this.btnTestConnection.TabIndex = 32;
            this.btnTestConnection.Text = "Test DB Connection";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // cmbServerName
            // 
            this.cmbServerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServerName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(150, 8);
            this.cmbServerName.Margin = new System.Windows.Forms.Padding(4);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(262, 25);
            this.cmbServerName.TabIndex = 22;
            this.cmbServerName.SelectedIndexChanged += new System.EventHandler(this.cmbServerName_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Red;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(13, 151);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(21, 36);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "Create Demo DB && Proceed";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(150, 127);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(262, 25);
            this.txtPassword.TabIndex = 26;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(150, 93);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(262, 25);
            this.txtUserName.TabIndex = 25;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(10, 13);
            this.Label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(106, 15);
            this.Label5.TabIndex = 30;
            this.Label5.Text = "SQL Server Name :";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(10, 98);
            this.Label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(132, 15);
            this.Label3.TabIndex = 29;
            this.Label3.Text = "SQL Server User Name :";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(10, 132);
            this.Label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(114, 15);
            this.Label2.TabIndex = 27;
            this.Label2.Text = "SQL User Password :";
            // 
            // Timer2
            // 
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // lblSet
            // 
            this.lblSet.AutoSize = true;
            this.lblSet.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblSet.Location = new System.Drawing.Point(42, 162);
            this.lblSet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(27, 17);
            this.lblSet.TabIndex = 34;
            this.lblSet.Text = "Set";
            this.lblSet.Visible = false;
            // 
            // panelDownload
            // 
            this.panelDownload.Controls.Add(this.lblProgress);
            this.panelDownload.Controls.Add(this.progressBar1);
            this.panelDownload.Location = new System.Drawing.Point(4, 7);
            this.panelDownload.Name = "panelDownload";
            this.panelDownload.Size = new System.Drawing.Size(410, 186);
            this.panelDownload.TabIndex = 58;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(0, 80);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(129, 21);
            this.lblProgress.TabIndex = 6130;
            this.lblProgress.Text = "Downloaded 0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 63);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(403, 15);
            this.progressBar1.TabIndex = 6129;
            // 
            // FormServerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(418, 195);
            this.Controls.Add(this.panelDownload);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.btnblankDb);
            this.Controls.Add(this.cmbAuthentication);
            this.Controls.Add(this.lnkLblSearch);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.cmbServerName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lblSet);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServerSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormServerSetting_FormClosing);
            this.Load += new System.EventHandler(this.FormServerSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormServerSetting_Paint);
            this.panelDownload.ResumeLayout(false);
            this.panelDownload.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Button btnblankDb;
        internal System.Windows.Forms.ComboBox cmbAuthentication;
        internal System.Windows.Forms.LinkLabel lnkLblSearch;
        internal System.Windows.Forms.Button btnTestConnection;
        internal System.Windows.Forms.ComboBox cmbServerName;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.TextBox txtPassword;
        internal System.Windows.Forms.TextBox txtUserName;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Timer Timer2;
        internal System.Windows.Forms.Label lblSet;
        private System.Windows.Forms.Panel panelDownload;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}