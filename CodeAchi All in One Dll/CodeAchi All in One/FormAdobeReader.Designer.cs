namespace CodeAchi_AllinOne
{
    partial class FormAdobeReader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdobeReader));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 68);
            this.label1.TabIndex = 6147;
            this.label1.Text = "Opps! There is no Adobe Reader installed in your System,\r\nTo get all the features" +
    " you need to install the Adobe Reader.\r\nTo install this click on \"Continue\" butt" +
    "on otherwise click on\r\n\"Skip\" button.";
            // 
            // btnSkip
            // 
            this.btnSkip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnSkip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSkip.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSkip.FlatAppearance.BorderSize = 2;
            this.btnSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkip.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSkip.ForeColor = System.Drawing.Color.White;
            this.btnSkip.Location = new System.Drawing.Point(267, 75);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(118, 30);
            this.btnSkip.TabIndex = 6146;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = false;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.DimGray;
            this.btnContinue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinue.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnContinue.FlatAppearance.BorderSize = 2;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinue.ForeColor = System.Drawing.Color.White;
            this.btnContinue.Location = new System.Drawing.Point(143, 75);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(118, 30);
            this.btnContinue.TabIndex = 6145;
            this.btnContinue.Text = "Continue >>";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(10, 134);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(129, 21);
            this.lblProgress.TabIndex = 6144;
            this.lblProgress.Text = "Downloaded 0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 112);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(371, 18);
            this.progressBar1.TabIndex = 6143;
            // 
            // Timer2
            // 
            this.Timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // FormAdobeReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 163);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAdobeReader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Adobe Reader Installation";
            this.Load += new System.EventHandler(this.FormAdobeReader_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormAdobeReader_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnSkip;
        internal System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Timer Timer2;
    }
}