namespace CodeAchi_LMS_Search
{
    partial class FormEbook
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEbook));
            this.BrowserPdf = new System.Windows.Forms.WebBrowser();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblFileNo = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BrowserPdf
            // 
            this.BrowserPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowserPdf.Location = new System.Drawing.Point(2, 2);
            this.BrowserPdf.MinimumSize = new System.Drawing.Size(20, 20);
            this.BrowserPdf.Name = "BrowserPdf";
            this.BrowserPdf.Size = new System.Drawing.Size(1053, 553);
            this.BrowserPdf.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNext.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.Location = new System.Drawing.Point(587, 561);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(25, 30);
            this.btnNext.TabIndex = 6134;
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblFileNo
            // 
            this.lblFileNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblFileNo.AutoSize = true;
            this.lblFileNo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileNo.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblFileNo.Location = new System.Drawing.Point(530, 565);
            this.lblFileNo.Name = "lblFileNo";
            this.lblFileNo.Size = new System.Drawing.Size(51, 21);
            this.lblFileNo.TabIndex = 6136;
            this.lblFileNo.Text = "label1";
            this.lblFileNo.SizeChanged += new System.EventHandler(this.lblFileNo_SizeChanged);
            // 
            // btnPrev
            // 
            this.btnPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrev.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrev.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrev.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.Location = new System.Drawing.Point(499, 561);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(25, 30);
            this.btnPrev.TabIndex = 6137;
            this.btnPrev.UseVisualStyleBackColor = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // FormEbook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 594);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.lblFileNo);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.BrowserPdf);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormEbook";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "E-book";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEbook_FormClosed);
            this.Load += new System.EventHandler(this.FormEbook_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser BrowserPdf;
        internal System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblFileNo;
        internal System.Windows.Forms.Button btnPrev;
    }
}