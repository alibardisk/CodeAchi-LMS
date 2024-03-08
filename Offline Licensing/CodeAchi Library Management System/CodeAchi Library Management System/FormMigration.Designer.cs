namespace CodeAchi_Library_Management_System
{
    partial class FormMigration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMigration));
            this.rdbIssue = new System.Windows.Forms.RadioButton();
            this.rdbReissue = new System.Windows.Forms.RadioButton();
            this.rdbReturn = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbFileName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblItem = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rdbIssue
            // 
            this.rdbIssue.AutoSize = true;
            this.rdbIssue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbIssue.Location = new System.Drawing.Point(12, 8);
            this.rdbIssue.Name = "rdbIssue";
            this.rdbIssue.Size = new System.Drawing.Size(89, 21);
            this.rdbIssue.TabIndex = 6128;
            this.rdbIssue.TabStop = true;
            this.rdbIssue.Text = "Issue Data";
            this.rdbIssue.UseVisualStyleBackColor = true;
            this.rdbIssue.CheckedChanged += new System.EventHandler(this.rdbIssue_CheckedChanged);
            // 
            // rdbReissue
            // 
            this.rdbReissue.AutoSize = true;
            this.rdbReissue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbReissue.Location = new System.Drawing.Point(12, 84);
            this.rdbReissue.Name = "rdbReissue";
            this.rdbReissue.Size = new System.Drawing.Size(14, 13);
            this.rdbReissue.TabIndex = 6129;
            this.rdbReissue.TabStop = true;
            this.rdbReissue.UseVisualStyleBackColor = true;
            this.rdbReissue.Visible = false;
            this.rdbReissue.CheckedChanged += new System.EventHandler(this.rdbReissue_CheckedChanged);
            // 
            // rdbReturn
            // 
            this.rdbReturn.AutoSize = true;
            this.rdbReturn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbReturn.Location = new System.Drawing.Point(126, 8);
            this.rdbReturn.Name = "rdbReturn";
            this.rdbReturn.Size = new System.Drawing.Size(99, 21);
            this.rdbReturn.TabIndex = 6130;
            this.rdbReturn.TabStop = true;
            this.rdbReturn.Text = "Return Data";
            this.rdbReturn.UseVisualStyleBackColor = true;
            this.rdbReturn.CheckedChanged += new System.EventHandler(this.rdbReturn_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 121);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(356, 12);
            this.progressBar1.TabIndex = 6131;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnImport.FlatAppearance.BorderSize = 2;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(261, 139);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(107, 30);
            this.btnImport.TabIndex = 6133;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.EnabledChanged += new System.EventHandler(this.btnImport_EnabledChanged);
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerate.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGenerate.FlatAppearance.BorderSize = 2;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(261, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(107, 30);
            this.btnGenerate.TabIndex = 6138;
            this.btnGenerate.Text = "Generate Excel";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.Silver;
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(296, 79);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(74, 30);
            this.btnBrowse.TabIndex = 6141;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 6140;
            this.label1.Text = "File Name :";
            // 
            // txtbFileName
            // 
            this.txtbFileName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFileName.Location = new System.Drawing.Point(90, 46);
            this.txtbFileName.Name = "txtbFileName";
            this.txtbFileName.ReadOnly = true;
            this.txtbFileName.Size = new System.Drawing.Size(280, 25);
            this.txtbFileName.TabIndex = 6139;
            this.txtbFileName.TextChanged += new System.EventHandler(this.txtbFileName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(114, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 17);
            this.label6.TabIndex = 6145;
            this.label6.Text = "Imported :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 6144;
            this.label3.Text = "Total :";
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblItem.Location = new System.Drawing.Point(187, 152);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(15, 17);
            this.lblItem.TabIndex = 6143;
            this.lblItem.Text = "0";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTotal.Location = new System.Drawing.Point(57, 152);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(15, 17);
            this.lblTotal.TabIndex = 6142;
            this.lblTotal.Text = "0";
            // 
            // btnToolTip
            // 
            this.btnToolTip.AutoPopDelay = 5000;
            this.btnToolTip.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnToolTip.InitialDelay = 50;
            this.btnToolTip.ReshowDelay = 0;
            // 
            // btnTutorial
            // 
            this.btnTutorial.BackColor = System.Drawing.Color.Transparent;
            this.btnTutorial.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTutorial.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnTutorial.FlatAppearance.BorderSize = 0;
            this.btnTutorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTutorial.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTutorial.Image = ((System.Drawing.Image)(resources.GetObject("btnTutorial.Image")));
            this.btnTutorial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTutorial.Location = new System.Drawing.Point(264, 91);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654673;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // FormMigration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 182);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbFileName);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.rdbReturn);
            this.Controls.Add(this.rdbReissue);
            this.Controls.Add(this.rdbIssue);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMigration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Migration";
            this.Load += new System.EventHandler(this.FormMigration_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMigration_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.RadioButton rdbIssue;
        internal System.Windows.Forms.RadioButton rdbReissue;
        internal System.Windows.Forms.RadioButton rdbReturn;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Button btnImport;
        internal System.Windows.Forms.Button btnGenerate;
        internal System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbFileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
    }
}