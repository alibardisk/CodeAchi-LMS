namespace CodeAchi_AllinOne
{
    partial class FormUpgrade
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpgrade));
            this.bWorkerGetDetails = new System.ComponentModel.BackgroundWorker();
            this.lblLatestVersion = new System.Windows.Forms.Label();
            this.lblProdVersion = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // bWorkerGetDetails
            // 
            this.bWorkerGetDetails.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorkerGetDetails_DoWork);
            this.bWorkerGetDetails.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bWorkerGetDetails_RunWorkerCompleted);
            // 
            // lblLatestVersion
            // 
            this.lblLatestVersion.AutoSize = true;
            this.lblLatestVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatestVersion.ForeColor = System.Drawing.Color.Tomato;
            this.lblLatestVersion.Location = new System.Drawing.Point(134, 33);
            this.lblLatestVersion.Name = "lblLatestVersion";
            this.lblLatestVersion.Size = new System.Drawing.Size(15, 17);
            this.lblLatestVersion.TabIndex = 6142;
            this.lblLatestVersion.Text = "0";
            // 
            // lblProdVersion
            // 
            this.lblProdVersion.AutoSize = true;
            this.lblProdVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdVersion.ForeColor = System.Drawing.Color.Tomato;
            this.lblProdVersion.Location = new System.Drawing.Point(134, 9);
            this.lblProdVersion.Name = "lblProdVersion";
            this.lblProdVersion.Size = new System.Drawing.Size(15, 17);
            this.lblProdVersion.TabIndex = 6141;
            this.lblProdVersion.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 17);
            this.label5.TabIndex = 6140;
            this.label5.Text = "Latest Version :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 6139;
            this.label1.Text = "Installed Version :";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(137, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 30);
            this.btnCancel.TabIndex = 6138;
            this.btnCancel.Text = "Upgrade Later";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.DimGray;
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.FlatAppearance.BorderSize = 2;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(267, 121);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(118, 30);
            this.btnUpdate.TabIndex = 6137;
            this.btnUpdate.Text = "Upgrade Now";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(10, 94);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(129, 21);
            this.lblProgress.TabIndex = 6136;
            this.lblProgress.Text = "Downloaded 0%";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 62);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(371, 23);
            this.progressBar1.TabIndex = 6135;
            // 
            // FormUpgrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(394, 163);
            this.Controls.Add(this.lblLatestVersion);
            this.Controls.Add(this.lblProdVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpgrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upgrade Product";
            this.Load += new System.EventHandler(this.FormUpgrade_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormUpgrade_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bWorkerGetDetails;
        internal System.Windows.Forms.Label lblLatestVersion;
        internal System.Windows.Forms.Label lblProdVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}