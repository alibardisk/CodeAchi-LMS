namespace CodeAchi_Library_Management_System
{
    partial class FormReportSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportSetting));
            this.btnSave = new System.Windows.Forms.Button();
            this.chkbMail = new System.Windows.Forms.CheckBox();
            this.chkbWebsite = new System.Windows.Forms.CheckBox();
            this.chkbContact = new System.Windows.Forms.CheckBox();
            this.chkbAddress = new System.Windows.Forms.CheckBox();
            this.rdbShortName = new System.Windows.Forms.RadioButton();
            this.rdbFullName = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
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
            this.btnSave.Location = new System.Drawing.Point(224, 105);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 6086;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkbMail
            // 
            this.chkbMail.AutoSize = true;
            this.chkbMail.Location = new System.Drawing.Point(24, 114);
            this.chkbMail.Name = "chkbMail";
            this.chkbMail.Size = new System.Drawing.Size(101, 21);
            this.chkbMail.TabIndex = 6085;
            this.chkbMail.Text = "Institute Mail";
            this.chkbMail.UseVisualStyleBackColor = true;
            // 
            // chkbWebsite
            // 
            this.chkbWebsite.AutoSize = true;
            this.chkbWebsite.Location = new System.Drawing.Point(24, 87);
            this.chkbWebsite.Name = "chkbWebsite";
            this.chkbWebsite.Size = new System.Drawing.Size(122, 21);
            this.chkbWebsite.TabIndex = 6084;
            this.chkbWebsite.Text = "Institute Website";
            this.chkbWebsite.UseVisualStyleBackColor = true;
            // 
            // chkbContact
            // 
            this.chkbContact.AutoSize = true;
            this.chkbContact.Location = new System.Drawing.Point(24, 60);
            this.chkbContact.Name = "chkbContact";
            this.chkbContact.Size = new System.Drawing.Size(120, 21);
            this.chkbContact.TabIndex = 6083;
            this.chkbContact.Text = "Institute Contact";
            this.chkbContact.UseVisualStyleBackColor = true;
            // 
            // chkbAddress
            // 
            this.chkbAddress.AutoSize = true;
            this.chkbAddress.Location = new System.Drawing.Point(24, 33);
            this.chkbAddress.Name = "chkbAddress";
            this.chkbAddress.Size = new System.Drawing.Size(124, 21);
            this.chkbAddress.TabIndex = 6081;
            this.chkbAddress.Text = "Institute Address";
            this.chkbAddress.UseVisualStyleBackColor = true;
            // 
            // rdbShortName
            // 
            this.rdbShortName.AutoSize = true;
            this.rdbShortName.Location = new System.Drawing.Point(163, 6);
            this.rdbShortName.Name = "rdbShortName";
            this.rdbShortName.Size = new System.Drawing.Size(145, 21);
            this.rdbShortName.TabIndex = 6082;
            this.rdbShortName.Text = "Institute Short Name";
            this.rdbShortName.UseVisualStyleBackColor = true;
            // 
            // rdbFullName
            // 
            this.rdbFullName.AutoSize = true;
            this.rdbFullName.Checked = true;
            this.rdbFullName.Location = new System.Drawing.Point(24, 6);
            this.rdbFullName.Name = "rdbFullName";
            this.rdbFullName.Size = new System.Drawing.Size(133, 21);
            this.rdbFullName.TabIndex = 6081;
            this.rdbFullName.TabStop = true;
            this.rdbFullName.Text = "Institute Full Name";
            this.rdbFullName.UseVisualStyleBackColor = true;
            // 
            // FormReportSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 143);
            this.Controls.Add(this.chkbMail);
            this.Controls.Add(this.chkbWebsite);
            this.Controls.Add(this.rdbFullName);
            this.Controls.Add(this.chkbContact);
            this.Controls.Add(this.rdbShortName);
            this.Controls.Add(this.chkbAddress);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormReportSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Setting";
            this.Load += new System.EventHandler(this.FormReportSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormReportSetting_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rdbShortName;
        private System.Windows.Forms.RadioButton rdbFullName;
        private System.Windows.Forms.CheckBox chkbMail;
        private System.Windows.Forms.CheckBox chkbWebsite;
        private System.Windows.Forms.CheckBox chkbContact;
        private System.Windows.Forms.CheckBox chkbAddress;
        internal System.Windows.Forms.Button btnSave;
    }
}