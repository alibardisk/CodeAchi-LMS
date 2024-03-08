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
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(15, 61);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(99, 17);
            this.Label2.TabIndex = 6117;
            this.Label2.Text = "Database Path :";
            // 
            // txtbDatabasePath
            // 
            this.txtbDatabasePath.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDatabasePath.ForeColor = System.Drawing.Color.Black;
            this.txtbDatabasePath.Location = new System.Drawing.Point(18, 82);
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
            this.label1.Location = new System.Drawing.Point(32, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 6119;
            this.label1.Text = "Sever Mechine Ip :";
            // 
            // txtbIp
            // 
            this.txtbIp.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbIp.ForeColor = System.Drawing.Color.Black;
            this.txtbIp.Location = new System.Drawing.Point(18, 26);
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
            this.btnBrowsePath.Location = new System.Drawing.Point(18, 118);
            this.btnBrowsePath.Name = "btnBrowsePath";
            this.btnBrowsePath.Size = new System.Drawing.Size(77, 30);
            this.btnBrowsePath.TabIndex = 6128;
            this.btnBrowsePath.Text = "Browse";
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
            this.btnTest.Location = new System.Drawing.Point(101, 118);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(113, 30);
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
            this.btnUpdate.Location = new System.Drawing.Point(219, 118);
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
            this.chkbServerIp.Location = new System.Drawing.Point(18, 8);
            this.chkbServerIp.Name = "chkbServerIp";
            this.chkbServerIp.Size = new System.Drawing.Size(15, 14);
            this.chkbServerIp.TabIndex = 6130;
            this.chkbServerIp.UseVisualStyleBackColor = true;
            this.chkbServerIp.CheckedChanged += new System.EventHandler(this.chkbServerIp_CheckedChanged);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 155);
            this.Controls.Add(this.chkbServerIp);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnBrowsePath);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbIp);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtbDatabasePath);
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
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}