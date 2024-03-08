namespace CodeAchi_AllinOne
{
    partial class FormTechnicalSupport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTechnicalSupport));
            this.pcbTwitter = new System.Windows.Forms.PictureBox();
            this.pcbYoutube = new System.Windows.Forms.PictureBox();
            this.pcbLinkedIn = new System.Windows.Forms.PictureBox();
            this.pcbFacebook = new System.Windows.Forms.PictureBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblCode = new System.Windows.Forms.Label();
            this.cmbHour = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            this.rdbPhone = new System.Windows.Forms.RadioButton();
            this.rdbSkype = new System.Windows.Forms.RadioButton();
            this.rdbWhatsApp = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbDetails = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcbTwitter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLinkedIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbFacebook)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbTwitter
            // 
            this.pcbTwitter.Cursor = System.Windows.Forms.Cursors.Default;
            this.pcbTwitter.Image = ((System.Drawing.Image)(resources.GetObject("pcbTwitter.Image")));
            this.pcbTwitter.Location = new System.Drawing.Point(340, 217);
            this.pcbTwitter.Name = "pcbTwitter";
            this.pcbTwitter.Size = new System.Drawing.Size(36, 36);
            this.pcbTwitter.TabIndex = 6172;
            this.pcbTwitter.TabStop = false;
            // 
            // pcbYoutube
            // 
            this.pcbYoutube.Cursor = System.Windows.Forms.Cursors.Default;
            this.pcbYoutube.Image = ((System.Drawing.Image)(resources.GetObject("pcbYoutube.Image")));
            this.pcbYoutube.Location = new System.Drawing.Point(376, 217);
            this.pcbYoutube.Name = "pcbYoutube";
            this.pcbYoutube.Size = new System.Drawing.Size(36, 36);
            this.pcbYoutube.TabIndex = 6171;
            this.pcbYoutube.TabStop = false;
            // 
            // pcbLinkedIn
            // 
            this.pcbLinkedIn.Cursor = System.Windows.Forms.Cursors.Default;
            this.pcbLinkedIn.Image = ((System.Drawing.Image)(resources.GetObject("pcbLinkedIn.Image")));
            this.pcbLinkedIn.Location = new System.Drawing.Point(412, 217);
            this.pcbLinkedIn.Name = "pcbLinkedIn";
            this.pcbLinkedIn.Size = new System.Drawing.Size(36, 36);
            this.pcbLinkedIn.TabIndex = 6170;
            this.pcbLinkedIn.TabStop = false;
            // 
            // pcbFacebook
            // 
            this.pcbFacebook.Cursor = System.Windows.Forms.Cursors.Default;
            this.pcbFacebook.Image = ((System.Drawing.Image)(resources.GetObject("pcbFacebook.Image")));
            this.pcbFacebook.Location = new System.Drawing.Point(304, 217);
            this.pcbFacebook.Name = "pcbFacebook";
            this.pcbFacebook.Size = new System.Drawing.Size(36, 36);
            this.pcbFacebook.TabIndex = 6169;
            this.pcbFacebook.TabStop = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.BackColor = System.Drawing.Color.DimGray;
            this.btnSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSubmit.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSubmit.FlatAppearance.BorderSize = 2;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(346, 177);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 30);
            this.btnSubmit.TabIndex = 6168;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCode.ForeColor = System.Drawing.Color.DimGray;
            this.lblCode.Location = new System.Drawing.Point(8, 161);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(131, 15);
            this.lblCode.TabIndex = 6167;
            this.lblCode.Text = "(Include Country Code)";
            // 
            // cmbHour
            // 
            this.cmbHour.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHour.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbHour.FormattingEnabled = true;
            this.cmbHour.Items.AddRange(new object[] {
            "Please select your time...",
            "Next One Hour",
            "Next Two Hours",
            "Next Four Hours",
            "Next Eight Hours"});
            this.cmbHour.Location = new System.Drawing.Point(152, 181);
            this.cmbHour.Name = "cmbHour";
            this.cmbHour.Size = new System.Drawing.Size(188, 25);
            this.cmbHour.TabIndex = 6166;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 17);
            this.label4.TabIndex = 6165;
            this.label4.Text = "Available Time :";
            // 
            // txtbId
            // 
            this.txtbId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbId.Location = new System.Drawing.Point(152, 142);
            this.txtbId.Name = "txtbId";
            this.txtbId.Size = new System.Drawing.Size(294, 25);
            this.txtbId.TabIndex = 6164;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblId.Location = new System.Drawing.Point(8, 145);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(81, 17);
            this.lblId.TabIndex = 6163;
            this.lblId.Text = "Contact No :";
            // 
            // rdbPhone
            // 
            this.rdbPhone.AutoSize = true;
            this.rdbPhone.Location = new System.Drawing.Point(384, 107);
            this.rdbPhone.Name = "rdbPhone";
            this.rdbPhone.Size = new System.Drawing.Size(65, 21);
            this.rdbPhone.TabIndex = 6162;
            this.rdbPhone.TabStop = true;
            this.rdbPhone.Text = "Phone";
            this.rdbPhone.UseVisualStyleBackColor = true;
            this.rdbPhone.CheckedChanged += new System.EventHandler(this.rdbPhone_CheckedChanged);
            // 
            // rdbSkype
            // 
            this.rdbSkype.AutoSize = true;
            this.rdbSkype.Location = new System.Drawing.Point(273, 106);
            this.rdbSkype.Name = "rdbSkype";
            this.rdbSkype.Size = new System.Drawing.Size(62, 21);
            this.rdbSkype.TabIndex = 6161;
            this.rdbSkype.TabStop = true;
            this.rdbSkype.Text = "Skype";
            this.rdbSkype.UseVisualStyleBackColor = true;
            this.rdbSkype.CheckedChanged += new System.EventHandler(this.rdbSkype_CheckedChanged);
            // 
            // rdbWhatsApp
            // 
            this.rdbWhatsApp.AutoSize = true;
            this.rdbWhatsApp.Location = new System.Drawing.Point(152, 106);
            this.rdbWhatsApp.Name = "rdbWhatsApp";
            this.rdbWhatsApp.Size = new System.Drawing.Size(94, 21);
            this.rdbWhatsApp.TabIndex = 6160;
            this.rdbWhatsApp.TabStop = true;
            this.rdbWhatsApp.Text = "Whats App";
            this.rdbWhatsApp.UseVisualStyleBackColor = true;
            this.rdbWhatsApp.CheckedChanged += new System.EventHandler(this.rdbWhatsApp_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 17);
            this.label2.TabIndex = 6159;
            this.label2.Text = "Provide Your Contact :";
            // 
            // txtbDetails
            // 
            this.txtbDetails.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbDetails.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDetails.Location = new System.Drawing.Point(11, 28);
            this.txtbDetails.Name = "txtbDetails";
            this.txtbDetails.Size = new System.Drawing.Size(435, 63);
            this.txtbDetails.TabIndex = 6158;
            this.txtbDetails.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 17);
            this.label1.TabIndex = 6157;
            this.label1.Text = "Please Describe Your Problem :";
            // 
            // FormTechnicalSupport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(457, 261);
            this.Controls.Add(this.pcbTwitter);
            this.Controls.Add(this.pcbYoutube);
            this.Controls.Add(this.pcbLinkedIn);
            this.Controls.Add(this.pcbFacebook);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.cmbHour);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbId);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.rdbPhone);
            this.Controls.Add(this.rdbSkype);
            this.Controls.Add(this.rdbWhatsApp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbDetails);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTechnicalSupport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Need Technical Support";
            this.Load += new System.EventHandler(this.FormTechnicalSupport_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormTechnicalSupport_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pcbTwitter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLinkedIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbFacebook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pcbTwitter;
        private System.Windows.Forms.PictureBox pcbYoutube;
        private System.Windows.Forms.PictureBox pcbLinkedIn;
        private System.Windows.Forms.PictureBox pcbFacebook;
        internal System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.ComboBox cmbHour;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtbId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.RadioButton rdbPhone;
        private System.Windows.Forms.RadioButton rdbSkype;
        private System.Windows.Forms.RadioButton rdbWhatsApp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtbDetails;
        private System.Windows.Forms.Label label1;
    }
}