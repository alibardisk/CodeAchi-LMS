namespace CodeAchi_Library_Management_System
{
    partial class FormAccnSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccnSetting));
            this.rdbManual = new System.Windows.Forms.RadioButton();
            this.grpbAuto = new System.Windows.Forms.GroupBox();
            this.rdbnoPrefix = new System.Windows.Forms.RadioButton();
            this.txtbJoinChar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.txtbPrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbPrefixText = new System.Windows.Forms.RadioButton();
            this.rdbSubcategory = new System.Windows.Forms.RadioButton();
            this.rdbAuto = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.grpbAuto.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdbManual
            // 
            this.rdbManual.AutoSize = true;
            this.rdbManual.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbManual.Location = new System.Drawing.Point(18, 8);
            this.rdbManual.Name = "rdbManual";
            this.rdbManual.Size = new System.Drawing.Size(134, 21);
            this.rdbManual.TabIndex = 0;
            this.rdbManual.TabStop = true;
            this.rdbManual.Text = "Manual Accession";
            this.rdbManual.UseVisualStyleBackColor = true;
            // 
            // grpbAuto
            // 
            this.grpbAuto.Controls.Add(this.rdbnoPrefix);
            this.grpbAuto.Controls.Add(this.txtbJoinChar);
            this.grpbAuto.Controls.Add(this.label2);
            this.grpbAuto.Controls.Add(this.lblNotification);
            this.grpbAuto.Controls.Add(this.txtbPrefix);
            this.grpbAuto.Controls.Add(this.label1);
            this.grpbAuto.Controls.Add(this.rdbPrefixText);
            this.grpbAuto.Controls.Add(this.rdbSubcategory);
            this.grpbAuto.Location = new System.Drawing.Point(12, 38);
            this.grpbAuto.Name = "grpbAuto";
            this.grpbAuto.Size = new System.Drawing.Size(396, 172);
            this.grpbAuto.TabIndex = 1;
            this.grpbAuto.TabStop = false;
            // 
            // rdbnoPrefix
            // 
            this.rdbnoPrefix.AutoSize = true;
            this.rdbnoPrefix.Location = new System.Drawing.Point(23, 24);
            this.rdbnoPrefix.Name = "rdbnoPrefix";
            this.rdbnoPrefix.Size = new System.Drawing.Size(80, 21);
            this.rdbnoPrefix.TabIndex = 9;
            this.rdbnoPrefix.TabStop = true;
            this.rdbnoPrefix.Text = "No Prefix";
            this.rdbnoPrefix.UseVisualStyleBackColor = true;
            this.rdbnoPrefix.CheckedChanged += new System.EventHandler(this.rdbnoPrefix_CheckedChanged);
            // 
            // txtbJoinChar
            // 
            this.txtbJoinChar.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbJoinChar.Location = new System.Drawing.Point(148, 116);
            this.txtbJoinChar.Name = "txtbJoinChar";
            this.txtbJoinChar.Size = new System.Drawing.Size(150, 25);
            this.txtbJoinChar.TabIndex = 6;
            this.txtbJoinChar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbJoinChar_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(156, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Only { /, \\, -, _, ~, . } and space are allowed.";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(144, 143);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(16, 20);
            this.lblNotification.TabIndex = 7;
            this.lblNotification.Text = "*";
            // 
            // txtbPrefix
            // 
            this.txtbPrefix.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPrefix.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbPrefix.Location = new System.Drawing.Point(148, 80);
            this.txtbPrefix.Name = "txtbPrefix";
            this.txtbPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtbPrefix.Size = new System.Drawing.Size(150, 25);
            this.txtbPrefix.TabIndex = 5;
            //this.txtbPrefix.Enter += new System.EventHandler(this.txtbPrefix_Enter);
            this.txtbPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbPrefix_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Joining Character :";
            // 
            // rdbPrefixText
            // 
            this.rdbPrefixText.AutoSize = true;
            this.rdbPrefixText.Location = new System.Drawing.Point(23, 81);
            this.rdbPrefixText.Name = "rdbPrefixText";
            this.rdbPrefixText.Size = new System.Drawing.Size(65, 21);
            this.rdbPrefixText.TabIndex = 3;
            this.rdbPrefixText.TabStop = true;
            this.rdbPrefixText.Text = "Prefix :";
            this.rdbPrefixText.UseVisualStyleBackColor = true;
            // 
            // rdbSubcategory
            // 
            this.rdbSubcategory.AutoSize = true;
            this.rdbSubcategory.Location = new System.Drawing.Point(23, 51);
            this.rdbSubcategory.Name = "rdbSubcategory";
            this.rdbSubcategory.Size = new System.Drawing.Size(294, 21);
            this.rdbSubcategory.TabIndex = 2;
            this.rdbSubcategory.TabStop = true;
            this.rdbSubcategory.Text = "Prefix - Book && Item Subcategory Short Name";
            this.rdbSubcategory.UseVisualStyleBackColor = true;
            this.rdbSubcategory.CheckedChanged += new System.EventHandler(this.rdbSubcategory_CheckedChanged);
            // 
            // rdbAuto
            // 
            this.rdbAuto.AutoSize = true;
            this.rdbAuto.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAuto.Location = new System.Drawing.Point(19, 35);
            this.rdbAuto.Name = "rdbAuto";
            this.rdbAuto.Size = new System.Drawing.Size(115, 21);
            this.rdbAuto.TabIndex = 1;
            this.rdbAuto.TabStop = true;
            this.rdbAuto.Text = "Auto Generate";
            this.rdbAuto.UseVisualStyleBackColor = true;
            this.rdbAuto.CheckedChanged += new System.EventHandler(this.rdbAuto_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(160, 216);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 6076;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(245, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(166, 17);
            this.linkLabel1.TabIndex = 6123;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "What is Accession Setting ?";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // FormAccnSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 252);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.rdbAuto);
            this.Controls.Add(this.grpbAuto);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rdbManual);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAccnSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accession Setting";
            this.Load += new System.EventHandler(this.FormAccnSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormAccnSetting_Paint);
            this.grpbAuto.ResumeLayout(false);
            this.grpbAuto.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbManual;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.TextBox txtbJoinChar;
        private System.Windows.Forms.TextBox txtbPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbPrefixText;
        private System.Windows.Forms.RadioButton rdbSubcategory;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.GroupBox grpbAuto;
        internal System.Windows.Forms.RadioButton rdbAuto;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.RadioButton rdbnoPrefix;
    }
}