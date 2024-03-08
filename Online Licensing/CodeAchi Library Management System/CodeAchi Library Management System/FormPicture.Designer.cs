namespace CodeAchi_Library_Management_System
{
    partial class FormPicture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPicture));
            this.panelBack = new System.Windows.Forms.Panel();
            this.lblScanning = new System.Windows.Forms.Label();
            this.pcbViewer = new System.Windows.Forms.PictureBox();
            this.cmbScanner = new System.Windows.Forms.ComboBox();
            this.rdbScanner = new System.Windows.Forms.RadioButton();
            this.cmbCamera = new System.Windows.Forms.ComboBox();
            this.rdbCamera = new System.Windows.Forms.RadioButton();
            this.txtbImagePath = new System.Windows.Forms.TextBox();
            this.rdbBrowse = new System.Windows.Forms.RadioButton();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCrop = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbViewer)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBack
            // 
            this.panelBack.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelBack.Controls.Add(this.lblScanning);
            this.panelBack.Controls.Add(this.pcbViewer);
            this.panelBack.Location = new System.Drawing.Point(7, 10);
            this.panelBack.Name = "panelBack";
            this.panelBack.Size = new System.Drawing.Size(384, 307);
            this.panelBack.TabIndex = 1;
            // 
            // lblScanning
            // 
            this.lblScanning.AutoSize = true;
            this.lblScanning.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblScanning.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanning.ForeColor = System.Drawing.Color.DimGray;
            this.lblScanning.Location = new System.Drawing.Point(69, 134);
            this.lblScanning.Name = "lblScanning";
            this.lblScanning.Size = new System.Drawing.Size(215, 25);
            this.lblScanning.TabIndex = 6162;
            this.lblScanning.Text = "Scanning...Please wait...";
            // 
            // pcbViewer
            // 
            this.pcbViewer.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pcbViewer.Location = new System.Drawing.Point(5, 5);
            this.pcbViewer.Name = "pcbViewer";
            this.pcbViewer.Size = new System.Drawing.Size(373, 297);
            this.pcbViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbViewer.TabIndex = 0;
            this.pcbViewer.TabStop = false;
            this.pcbViewer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbViewer_MouseDown);
            this.pcbViewer.MouseEnter += new System.EventHandler(this.pcbViewer_MouseEnter);
            this.pcbViewer.MouseLeave += new System.EventHandler(this.pcbViewer_MouseLeave);
            this.pcbViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pcbViewer_MouseMove);
            this.pcbViewer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbViewer_MouseUp);
            // 
            // cmbScanner
            // 
            this.cmbScanner.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbScanner.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbScanner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScanner.Enabled = false;
            this.cmbScanner.FormattingEnabled = true;
            this.cmbScanner.Location = new System.Drawing.Point(87, 126);
            this.cmbScanner.Name = "cmbScanner";
            this.cmbScanner.Size = new System.Drawing.Size(212, 25);
            this.cmbScanner.TabIndex = 6150;
            this.cmbScanner.SelectedIndexChanged += new System.EventHandler(this.cmbScanner_SelectedIndexChanged);
            // 
            // rdbScanner
            // 
            this.rdbScanner.AutoSize = true;
            this.rdbScanner.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rdbScanner.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbScanner.Location = new System.Drawing.Point(6, 127);
            this.rdbScanner.Name = "rdbScanner";
            this.rdbScanner.Size = new System.Drawing.Size(81, 21);
            this.rdbScanner.TabIndex = 6149;
            this.rdbScanner.TabStop = true;
            this.rdbScanner.Text = "Scanner :";
            this.rdbScanner.UseVisualStyleBackColor = true;
            this.rdbScanner.CheckedChanged += new System.EventHandler(this.rdbScanner_CheckedChanged);
            // 
            // cmbCamera
            // 
            this.cmbCamera.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmbCamera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCamera.Enabled = false;
            this.cmbCamera.FormattingEnabled = true;
            this.cmbCamera.Location = new System.Drawing.Point(87, 75);
            this.cmbCamera.Name = "cmbCamera";
            this.cmbCamera.Size = new System.Drawing.Size(212, 25);
            this.cmbCamera.TabIndex = 6148;
            this.cmbCamera.SelectedIndexChanged += new System.EventHandler(this.cmbCamera_SelectedIndexChanged);
            // 
            // rdbCamera
            // 
            this.rdbCamera.AutoSize = true;
            this.rdbCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rdbCamera.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbCamera.Location = new System.Drawing.Point(6, 75);
            this.rdbCamera.Name = "rdbCamera";
            this.rdbCamera.Size = new System.Drawing.Size(79, 21);
            this.rdbCamera.TabIndex = 6147;
            this.rdbCamera.TabStop = true;
            this.rdbCamera.Text = "Camera :";
            this.rdbCamera.UseVisualStyleBackColor = true;
            this.rdbCamera.CheckedChanged += new System.EventHandler(this.rdbCamera_CheckedChanged);
            // 
            // txtbImagePath
            // 
            this.txtbImagePath.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbImagePath.Enabled = false;
            this.txtbImagePath.Location = new System.Drawing.Point(6, 41);
            this.txtbImagePath.Name = "txtbImagePath";
            this.txtbImagePath.Size = new System.Drawing.Size(234, 25);
            this.txtbImagePath.TabIndex = 6146;
            // 
            // rdbBrowse
            // 
            this.rdbBrowse.AutoSize = true;
            this.rdbBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rdbBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbBrowse.Location = new System.Drawing.Point(6, 14);
            this.rdbBrowse.Name = "rdbBrowse";
            this.rdbBrowse.Size = new System.Drawing.Size(94, 21);
            this.rdbBrowse.TabIndex = 6145;
            this.rdbBrowse.TabStop = true;
            this.rdbBrowse.Text = "Computer :";
            this.rdbBrowse.UseVisualStyleBackColor = true;
            this.rdbBrowse.CheckedChanged += new System.EventHandler(this.rdbBrowse_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.Silver;
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(246, 41);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(53, 25);
            this.btnBrowse.TabIndex = 6153;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbBrowse);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtbImagePath);
            this.groupBox1.Controls.Add(this.cmbScanner);
            this.groupBox1.Controls.Add(this.rdbCamera);
            this.groupBox1.Controls.Add(this.rdbScanner);
            this.groupBox1.Controls.Add(this.cmbCamera);
            this.groupBox1.Location = new System.Drawing.Point(401, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 168);
            this.groupBox1.TabIndex = 6154;
            this.groupBox1.TabStop = false;
            // 
            // btnCrop
            // 
            this.btnCrop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnCrop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCrop.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCrop.FlatAppearance.BorderSize = 2;
            this.btnCrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrop.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCrop.Location = new System.Drawing.Point(567, 187);
            this.btnCrop.Name = "btnCrop";
            this.btnCrop.Size = new System.Drawing.Size(122, 29);
            this.btnCrop.TabIndex = 6155;
            this.btnCrop.Text = "Crop Image";
            this.btnCrop.UseVisualStyleBackColor = false;
            this.btnCrop.Click += new System.EventHandler(this.btnCrop_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnCapture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCapture.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCapture.FlatAppearance.BorderSize = 2;
            this.btnCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapture.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapture.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCapture.Location = new System.Drawing.Point(414, 187);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(122, 29);
            this.btnCapture.TabIndex = 6156;
            this.btnCapture.Text = "Capture Image";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.Location = new System.Drawing.Point(414, 225);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 29);
            this.btnSave.TabIndex = 6157;
            this.btnSave.Text = "Save Image";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.FlatAppearance.BorderSize = 2;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnReset.Location = new System.Drawing.Point(567, 225);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(122, 29);
            this.btnReset.TabIndex = 6158;
            this.btnReset.Text = "Reset Image";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Calisto MT", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.Red;
            this.Label8.Location = new System.Drawing.Point(12, 320);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(14, 17);
            this.Label8.TabIndex = 6160;
            this.Label8.Text = "*";
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo2.ForeColor = System.Drawing.Color.Red;
            this.lblInfo2.Location = new System.Drawing.Point(23, 322);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(298, 15);
            this.lblInfo2.TabIndex = 6161;
            this.lblInfo2.Text = "To crop an image drag and select the area using mouse";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // FormPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 342);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.btnCrop);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelBack);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPicture";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Image";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPicture_FormClosing);
            this.Load += new System.EventHandler(this.FormPicture_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormPicture_Paint);
            this.panelBack.ResumeLayout(false);
            this.panelBack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbViewer)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelBack;
        private System.Windows.Forms.PictureBox pcbViewer;
        private System.Windows.Forms.ComboBox cmbScanner;
        private System.Windows.Forms.RadioButton rdbScanner;
        private System.Windows.Forms.ComboBox cmbCamera;
        private System.Windows.Forms.RadioButton rdbCamera;
        private System.Windows.Forms.TextBox txtbImagePath;
        private System.Windows.Forms.RadioButton rdbBrowse;
        internal System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button btnCrop;
        internal System.Windows.Forms.Button btnCapture;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label lblInfo2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lblScanning;
    }
}