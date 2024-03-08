namespace CodeAchi_AllinOne
{
    partial class FormEvent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEvent));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pcbClose = new System.Windows.Forms.PictureBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.pcbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pcbClose
            // 
            this.pcbClose.BackColor = System.Drawing.Color.Transparent;
            this.pcbClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pcbClose.Image = ((System.Drawing.Image)(resources.GetObject("pcbClose.Image")));
            this.pcbClose.Location = new System.Drawing.Point(514, 11);
            this.pcbClose.Name = "pcbClose";
            this.pcbClose.Size = new System.Drawing.Size(21, 21);
            this.pcbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pcbClose.TabIndex = 3;
            this.pcbClose.TabStop = false;
            this.pcbClose.Click += new System.EventHandler(this.pcbClose_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(-9, -16);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(592, 404);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // FormEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 389);
            this.Controls.Add(this.pcbClose);
            this.Controls.Add(this.webBrowser1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormEvent";
            this.Load += new System.EventHandler(this.FormEvent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcbClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.PictureBox pcbClose;
        internal System.Windows.Forms.WebBrowser webBrowser1;
    }
}