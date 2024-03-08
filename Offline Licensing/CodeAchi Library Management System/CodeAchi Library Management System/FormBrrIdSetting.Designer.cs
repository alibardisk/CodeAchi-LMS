namespace CodeAchi_Library_Management_System
{
    partial class FormBrrIdSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBrrIdSetting));
            this.grpbAuto = new System.Windows.Forms.GroupBox();
            this.txtbJoinChar = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.txtbPrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbPrefixText = new System.Windows.Forms.RadioButton();
            this.rdbCategory = new System.Windows.Forms.RadioButton();
            this.rdbManual = new System.Windows.Forms.RadioButton();
            this.rdbAuto = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.grpbAuto.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpbAuto
            // 
            this.grpbAuto.Controls.Add(this.txtbJoinChar);
            this.grpbAuto.Controls.Add(this.label2);
            this.grpbAuto.Controls.Add(this.lblNotification);
            this.grpbAuto.Controls.Add(this.txtbPrefix);
            this.grpbAuto.Controls.Add(this.label1);
            this.grpbAuto.Controls.Add(this.rdbPrefixText);
            this.grpbAuto.Controls.Add(this.rdbCategory);
            this.grpbAuto.Location = new System.Drawing.Point(12, 37);
            this.grpbAuto.Name = "grpbAuto";
            this.grpbAuto.Size = new System.Drawing.Size(396, 143);
            this.grpbAuto.TabIndex = 6079;
            this.grpbAuto.TabStop = false;
            // 
            // txtbJoinChar
            // 
            this.txtbJoinChar.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbJoinChar.Location = new System.Drawing.Point(154, 85);
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
            this.label2.Location = new System.Drawing.Point(162, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Only { /, \\, -, _ } and space are allowed";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(150, 112);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(16, 20);
            this.lblNotification.TabIndex = 7;
            this.lblNotification.Text = "*";
            // 
            // txtbPrefix
            // 
            this.txtbPrefix.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPrefix.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbPrefix.Location = new System.Drawing.Point(154, 49);
            this.txtbPrefix.Name = "txtbPrefix";
            this.txtbPrefix.Size = new System.Drawing.Size(150, 25);
            this.txtbPrefix.TabIndex = 5;
            this.txtbPrefix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbPrefix_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Joining Character :";
            // 
            // rdbPrefixText
            // 
            this.rdbPrefixText.AutoSize = true;
            this.rdbPrefixText.Location = new System.Drawing.Point(29, 50);
            this.rdbPrefixText.Name = "rdbPrefixText";
            this.rdbPrefixText.Size = new System.Drawing.Size(65, 21);
            this.rdbPrefixText.TabIndex = 3;
            this.rdbPrefixText.TabStop = true;
            this.rdbPrefixText.Text = "Prefix :";
            this.rdbPrefixText.UseVisualStyleBackColor = true;
            // 
            // rdbCategory
            // 
            this.rdbCategory.AutoSize = true;
            this.rdbCategory.Location = new System.Drawing.Point(29, 21);
            this.rdbCategory.Name = "rdbCategory";
            this.rdbCategory.Size = new System.Drawing.Size(298, 21);
            this.rdbCategory.TabIndex = 2;
            this.rdbCategory.TabStop = true;
            this.rdbCategory.Text = "Prefix - Borrower category first three character";
            this.rdbCategory.UseVisualStyleBackColor = true;
            this.rdbCategory.CheckedChanged += new System.EventHandler(this.rdbCategory_CheckedChanged);
            // 
            // rdbManual
            // 
            this.rdbManual.AutoSize = true;
            this.rdbManual.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbManual.Location = new System.Drawing.Point(21, 7);
            this.rdbManual.Name = "rdbManual";
            this.rdbManual.Size = new System.Drawing.Size(87, 21);
            this.rdbManual.TabIndex = 6078;
            this.rdbManual.TabStop = true;
            this.rdbManual.Text = "Manual Id";
            this.rdbManual.UseVisualStyleBackColor = true;
            // 
            // rdbAuto
            // 
            this.rdbAuto.AutoSize = true;
            this.rdbAuto.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAuto.Location = new System.Drawing.Point(21, 34);
            this.rdbAuto.Name = "rdbAuto";
            this.rdbAuto.Size = new System.Drawing.Size(115, 21);
            this.rdbAuto.TabIndex = 6080;
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
            this.btnSave.Location = new System.Drawing.Point(160, 186);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 6081;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormBrrIdSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 221);
            this.Controls.Add(this.rdbAuto);
            this.Controls.Add(this.grpbAuto);
            this.Controls.Add(this.rdbManual);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBrrIdSetting";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Borrower Id Setting";
            this.Load += new System.EventHandler(this.FormBrrIdSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormBrrIdSetting_Paint);
            this.grpbAuto.ResumeLayout(false);
            this.grpbAuto.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox grpbAuto;
        private System.Windows.Forms.TextBox txtbJoinChar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.TextBox txtbPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbPrefixText;
        private System.Windows.Forms.RadioButton rdbCategory;
        private System.Windows.Forms.RadioButton rdbManual;
        internal System.Windows.Forms.RadioButton rdbAuto;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Timer timer1;
    }
}