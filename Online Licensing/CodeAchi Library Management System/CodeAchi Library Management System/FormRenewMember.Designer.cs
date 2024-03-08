namespace CodeAchi_Library_Management_System
{
    partial class FormRenewMember
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRenewMember));
            this.lblBrName = new System.Windows.Forms.Label();
            this.txtbBrrId = new System.Windows.Forms.TextBox();
            this.txtbBrrName = new System.Windows.Forms.TextBox();
            this.lblBrId = new System.Windows.Forms.Label();
            this.txtbCategory = new System.Windows.Forms.TextBox();
            this.lblBrCategory = new System.Windows.Forms.Label();
            this.txtbDuration = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.pcbBrrImage = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbFees = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtbReference = new System.Windows.Forms.TextBox();
            this.cmbPaymentMode = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.txtbFreqncy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpIssue = new System.Windows.Forms.DateTimePicker();
            this.cmbPlan = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkLvlRctSetting = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBrrImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBrName
            // 
            this.lblBrName.AutoSize = true;
            this.lblBrName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrName.ForeColor = System.Drawing.Color.Black;
            this.lblBrName.Location = new System.Drawing.Point(5, 58);
            this.lblBrName.Name = "lblBrName";
            this.lblBrName.Size = new System.Drawing.Size(108, 17);
            this.lblBrName.TabIndex = 6057;
            this.lblBrName.Text = "Borrower Name :";
            // 
            // txtbBrrId
            // 
            this.txtbBrrId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbBrrId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrId.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrId.Location = new System.Drawing.Point(8, 27);
            this.txtbBrrId.MaxLength = 500;
            this.txtbBrrId.Name = "txtbBrrId";
            this.txtbBrrId.Size = new System.Drawing.Size(223, 25);
            this.txtbBrrId.TabIndex = 0;
            this.txtbBrrId.TextChanged += new System.EventHandler(this.txtbBrrId_TextChanged);
            // 
            // txtbBrrName
            // 
            this.txtbBrrName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrName.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrName.Location = new System.Drawing.Point(8, 78);
            this.txtbBrrName.MaxLength = 500;
            this.txtbBrrName.Name = "txtbBrrName";
            this.txtbBrrName.ReadOnly = true;
            this.txtbBrrName.Size = new System.Drawing.Size(223, 25);
            this.txtbBrrName.TabIndex = 1;
            // 
            // lblBrId
            // 
            this.lblBrId.AutoSize = true;
            this.lblBrId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrId.ForeColor = System.Drawing.Color.Black;
            this.lblBrId.Location = new System.Drawing.Point(5, 7);
            this.lblBrId.Name = "lblBrId";
            this.lblBrId.Size = new System.Drawing.Size(84, 17);
            this.lblBrId.TabIndex = 6056;
            this.lblBrId.Text = "Borrower Id :";
            // 
            // txtbCategory
            // 
            this.txtbCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbCategory.ForeColor = System.Drawing.Color.Black;
            this.txtbCategory.Location = new System.Drawing.Point(8, 129);
            this.txtbCategory.MaxLength = 500;
            this.txtbCategory.Name = "txtbCategory";
            this.txtbCategory.ReadOnly = true;
            this.txtbCategory.Size = new System.Drawing.Size(223, 25);
            this.txtbCategory.TabIndex = 2;
            // 
            // lblBrCategory
            // 
            this.lblBrCategory.AutoSize = true;
            this.lblBrCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrCategory.ForeColor = System.Drawing.Color.Black;
            this.lblBrCategory.Location = new System.Drawing.Point(5, 109);
            this.lblBrCategory.Name = "lblBrCategory";
            this.lblBrCategory.Size = new System.Drawing.Size(68, 17);
            this.lblBrCategory.TabIndex = 6077;
            this.lblBrCategory.Text = "Category :";
            // 
            // txtbDuration
            // 
            this.txtbDuration.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbDuration.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDuration.ForeColor = System.Drawing.Color.Black;
            this.txtbDuration.Location = new System.Drawing.Point(8, 227);
            this.txtbDuration.MaxLength = 500;
            this.txtbDuration.Name = "txtbDuration";
            this.txtbDuration.ReadOnly = true;
            this.txtbDuration.Size = new System.Drawing.Size(98, 25);
            this.txtbDuration.TabIndex = 5;
            this.txtbDuration.Enter += new System.EventHandler(this.txtbDuration_Enter);
            this.txtbDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbDuration_KeyPress);
            this.txtbDuration.Leave += new System.EventHandler(this.txtbDuration_Leave);
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.Color.Black;
            this.Label10.Location = new System.Drawing.Point(5, 207);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(56, 17);
            this.Label10.TabIndex = 6079;
            this.Label10.Text = "Validity :";
            // 
            // pcbBrrImage
            // 
            this.pcbBrrImage.BackColor = System.Drawing.Color.Transparent;
            this.pcbBrrImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbBrrImage.Image = ((System.Drawing.Image)(resources.GetObject("pcbBrrImage.Image")));
            this.pcbBrrImage.Location = new System.Drawing.Point(262, 129);
            this.pcbBrrImage.Name = "pcbBrrImage";
            this.pcbBrrImage.Size = new System.Drawing.Size(103, 102);
            this.pcbBrrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbBrrImage.TabIndex = 6080;
            this.pcbBrrImage.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(5, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 17);
            this.label4.TabIndex = 6105;
            this.label4.Text = "Renewal Fees :";
            // 
            // txtbFees
            // 
            this.txtbFees.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFees.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbFees.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbFees.ForeColor = System.Drawing.Color.Black;
            this.txtbFees.Location = new System.Drawing.Point(22, 281);
            this.txtbFees.MaxLength = 500;
            this.txtbFees.Name = "txtbFees";
            this.txtbFees.ReadOnly = true;
            this.txtbFees.Size = new System.Drawing.Size(209, 18);
            this.txtbFees.TabIndex = 6104;
            this.txtbFees.WordWrap = false;
            this.txtbFees.Enter += new System.EventHandler(this.txtbFees_Enter);
            this.txtbFees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbFees_KeyPress);
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
            this.btnSave.Location = new System.Drawing.Point(262, 243);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Renew Now";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtbReference
            // 
            this.txtbReference.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbReference.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbReference.ForeColor = System.Drawing.Color.Black;
            this.txtbReference.Location = new System.Drawing.Point(260, 78);
            this.txtbReference.MaxLength = 500;
            this.txtbReference.Name = "txtbReference";
            this.txtbReference.Size = new System.Drawing.Size(199, 25);
            this.txtbReference.TabIndex = 8;
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaymentMode.ForeColor = System.Drawing.Color.Black;
            this.cmbPaymentMode.FormattingEnabled = true;
            this.cmbPaymentMode.Items.AddRange(new object[] {
            "Please select a payment mode...",
            "Cash",
            "Card",
            "Cheque"});
            this.cmbPaymentMode.Location = new System.Drawing.Point(262, 27);
            this.cmbPaymentMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            this.cmbPaymentMode.Size = new System.Drawing.Size(197, 25);
            this.cmbPaymentMode.TabIndex = 7;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(259, 58);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(73, 17);
            this.label16.TabIndex = 6127;
            this.label16.Text = "Reference :";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(257, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 17);
            this.label15.TabIndex = 6126;
            this.label15.Text = "Payment Mode :";
            // 
            // lblCurrency
            // 
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrency.ForeColor = System.Drawing.Color.Black;
            this.lblCurrency.Location = new System.Drawing.Point(7, 281);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(15, 17);
            this.lblCurrency.TabIndex = 6128;
            this.lblCurrency.Text = "$";
            // 
            // txtbFreqncy
            // 
            this.txtbFreqncy.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFreqncy.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbFreqncy.ForeColor = System.Drawing.Color.Black;
            this.txtbFreqncy.Location = new System.Drawing.Point(140, 176);
            this.txtbFreqncy.MaxLength = 500;
            this.txtbFreqncy.Name = "txtbFreqncy";
            this.txtbFreqncy.Size = new System.Drawing.Size(91, 25);
            this.txtbFreqncy.TabIndex = 4;
            this.txtbFreqncy.Text = "1";
            this.txtbFreqncy.TextChanged += new System.EventHandler(this.txtbFreqncy_TextChanged);
            this.txtbFreqncy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbFreqncy_KeyPress);
            this.txtbFreqncy.Leave += new System.EventHandler(this.txtbFreqncy_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(137, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 456480;
            this.label1.Text = "Duration :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label14.Location = new System.Drawing.Point(117, 207);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 18);
            this.label14.TabIndex = 456479;
            this.label14.Text = "Renew Date :";
            // 
            // dtpIssue
            // 
            this.dtpIssue.CustomFormat = "dd/MM/yyyy";
            this.dtpIssue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpIssue.Location = new System.Drawing.Point(119, 227);
            this.dtpIssue.Name = "dtpIssue";
            this.dtpIssue.Size = new System.Drawing.Size(112, 25);
            this.dtpIssue.TabIndex = 6;
            // 
            // cmbPlan
            // 
            this.cmbPlan.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlan.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPlan.FormattingEnabled = true;
            this.cmbPlan.Location = new System.Drawing.Point(8, 176);
            this.cmbPlan.Name = "cmbPlan";
            this.cmbPlan.Size = new System.Drawing.Size(114, 25);
            this.cmbPlan.TabIndex = 3;
            this.cmbPlan.SelectedIndexChanged += new System.EventHandler(this.cmbPlan_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(5, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 17);
            this.label3.TabIndex = 456478;
            this.label3.Text = "Membership Plan :";
            // 
            // lnkLvlRctSetting
            // 
            this.lnkLvlRctSetting.AutoSize = true;
            this.lnkLvlRctSetting.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLvlRctSetting.Location = new System.Drawing.Point(259, 277);
            this.lnkLvlRctSetting.Name = "lnkLvlRctSetting";
            this.lnkLvlRctSetting.Size = new System.Drawing.Size(99, 17);
            this.lnkLvlRctSetting.TabIndex = 5654664;
            this.lnkLvlRctSetting.TabStop = true;
            this.lnkLvlRctSetting.Text = "Reciept Setting";
            this.lnkLvlRctSetting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLvlRctSetting_LinkClicked);
            // 
            // FormRenewMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 305);
            this.Controls.Add(this.lnkLvlRctSetting);
            this.Controls.Add(this.txtbFreqncy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dtpIssue);
            this.Controls.Add(this.cmbPlan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCurrency);
            this.Controls.Add(this.txtbReference);
            this.Controls.Add(this.cmbPaymentMode);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbFees);
            this.Controls.Add(this.pcbBrrImage);
            this.Controls.Add(this.txtbDuration);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.txtbCategory);
            this.Controls.Add(this.lblBrCategory);
            this.Controls.Add(this.lblBrName);
            this.Controls.Add(this.txtbBrrId);
            this.Controls.Add(this.txtbBrrName);
            this.Controls.Add(this.lblBrId);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRenewMember";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renew Membership";
            this.Load += new System.EventHandler(this.FormRenew_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormRenew_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pcbBrrImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblBrName;
        internal System.Windows.Forms.TextBox txtbBrrId;
        internal System.Windows.Forms.TextBox txtbBrrName;
        internal System.Windows.Forms.Label lblBrId;
        internal System.Windows.Forms.TextBox txtbCategory;
        internal System.Windows.Forms.Label lblBrCategory;
        internal System.Windows.Forms.TextBox txtbDuration;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.PictureBox pcbBrrImage;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbFees;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.TextBox txtbReference;
        internal System.Windows.Forms.ComboBox cmbPaymentMode;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label lblCurrency;
        internal System.Windows.Forms.TextBox txtbFreqncy;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpIssue;
        internal System.Windows.Forms.ComboBox cmbPlan;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.LinkLabel lnkLvlRctSetting;
    }
}