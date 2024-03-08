namespace CodeAchi_Library_Management_System
{
    partial class FormNotification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNotification));
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.pcbCompany = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lnklblIdChat = new System.Windows.Forms.LinkLabel();
            this.lnklblBuy = new System.Windows.Forms.LinkLabel();
            this.lnklblQuotation = new System.Windows.Forms.LinkLabel();
            this.lnklblRenew = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkLblActivate = new System.Windows.Forms.LinkLabel();
            this.lblRefresh = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pcbCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(62, 57);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(334, 69);
            this.lblMessage.TabIndex = 6082;
            this.lblMessage.Text = "label1";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOk.FlatAppearance.BorderSize = 2;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(154, 132);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 30);
            this.btnOk.TabIndex = 6086;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pcbCompany
            // 
            this.pcbCompany.BackColor = System.Drawing.Color.Gainsboro;
            this.pcbCompany.Image = ((System.Drawing.Image)(resources.GetObject("pcbCompany.Image")));
            this.pcbCompany.Location = new System.Drawing.Point(1, 1);
            this.pcbCompany.Name = "pcbCompany";
            this.pcbCompany.Size = new System.Drawing.Size(406, 50);
            this.pcbCompany.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbCompany.TabIndex = 6087;
            this.pcbCompany.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 54);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6088;
            this.pictureBox1.TabStop = false;
            // 
            // lnklblIdChat
            // 
            this.lnklblIdChat.AutoSize = true;
            this.lnklblIdChat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnklblIdChat.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblIdChat.Location = new System.Drawing.Point(2, 145);
            this.lnklblIdChat.Name = "lnklblIdChat";
            this.lnklblIdChat.Size = new System.Drawing.Size(88, 17);
            this.lnklblIdChat.TabIndex = 6104;
            this.lnklblIdChat.TabStop = true;
            this.lnklblIdChat.Text = "Chat With Us";
            this.lnklblIdChat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblIdChat_LinkClicked);
            // 
            // lnklblBuy
            // 
            this.lnklblBuy.AutoSize = true;
            this.lnklblBuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnklblBuy.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblBuy.Location = new System.Drawing.Point(2, 126);
            this.lnklblBuy.Name = "lnklblBuy";
            this.lnklblBuy.Size = new System.Drawing.Size(62, 17);
            this.lnklblBuy.TabIndex = 6105;
            this.lnklblBuy.TabStop = true;
            this.lnklblBuy.Text = "Buy Now";
            this.lnklblBuy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblBuy_LinkClicked);
            // 
            // lnklblQuotation
            // 
            this.lnklblQuotation.AutoSize = true;
            this.lnklblQuotation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnklblQuotation.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblQuotation.Location = new System.Drawing.Point(268, 145);
            this.lnklblQuotation.Name = "lnklblQuotation";
            this.lnklblQuotation.Size = new System.Drawing.Size(132, 17);
            this.lnklblQuotation.TabIndex = 6106;
            this.lnklblQuotation.TabStop = true;
            this.lnklblQuotation.Text = "Send me a quotation";
            this.lnklblQuotation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblQuotation_LinkClicked);
            // 
            // lnklblRenew
            // 
            this.lnklblRenew.AutoSize = true;
            this.lnklblRenew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnklblRenew.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnklblRenew.Location = new System.Drawing.Point(268, 145);
            this.lnklblRenew.Name = "lnklblRenew";
            this.lnklblRenew.Size = new System.Drawing.Size(79, 17);
            this.lnklblRenew.TabIndex = 6107;
            this.lnklblRenew.TabStop = true;
            this.lnklblRenew.Text = "Renew Now";
            this.lnklblRenew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnklblRenew_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Location = new System.Drawing.Point(1, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(406, 1);
            this.panel2.TabIndex = 6136;
            // 
            // lnkLblActivate
            // 
            this.lnkLblActivate.AutoSize = true;
            this.lnkLblActivate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkLblActivate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkLblActivate.Location = new System.Drawing.Point(268, 126);
            this.lnkLblActivate.Name = "lnkLblActivate";
            this.lnkLblActivate.Size = new System.Drawing.Size(89, 17);
            this.lnkLblActivate.TabIndex = 6137;
            this.lnkLblActivate.TabStop = true;
            this.lnkLblActivate.Text = "Activate Now";
            this.lnkLblActivate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblActivate_LinkClicked);
            // 
            // lblRefresh
            // 
            this.lblRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefresh.AutoSize = true;
            this.lblRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefresh.Location = new System.Drawing.Point(2, 109);
            this.lblRefresh.Name = "lblRefresh";
            this.lblRefresh.Size = new System.Drawing.Size(44, 17);
            this.lblRefresh.TabIndex = 6144;
            this.lblRefresh.TabStop = true;
            this.lblRefresh.Text = "About";
            this.lblRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRefresh_LinkClicked);
            // 
            // FormNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(408, 171);
            this.Controls.Add(this.lblRefresh);
            this.Controls.Add(this.lnkLblActivate);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lnklblRenew);
            this.Controls.Add(this.lnklblQuotation);
            this.Controls.Add(this.lnklblBuy);
            this.Controls.Add(this.lnklblIdChat);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pcbCompany);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNotification";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNotification_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormNotification_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pcbCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label lblMessage;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.PictureBox pcbCompany;
        private System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.LinkLabel lnklblIdChat;
        internal System.Windows.Forms.LinkLabel lnklblBuy;
        internal System.Windows.Forms.LinkLabel lnklblQuotation;
        internal System.Windows.Forms.LinkLabel lnklblRenew;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.LinkLabel lnkLblActivate;
        internal System.Windows.Forms.LinkLabel lblRefresh;
    }
}