namespace CodeAchi_Library_Management_System
{
    partial class FormImportItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportItems));
            this.lblItem = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.rdbAuto = new System.Windows.Forms.RadioButton();
            this.rdbManual = new System.Windows.Forms.RadioButton();
            this.btnImport = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Label5 = new System.Windows.Forms.Label();
            this.cmbItemCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSubCategory = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtbFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            this.btnImage = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbImage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblItem.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblItem.Location = new System.Drawing.Point(186, 259);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(15, 17);
            this.lblItem.TabIndex = 6089;
            this.lblItem.Text = "0";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTotal.Location = new System.Drawing.Point(56, 259);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(15, 17);
            this.lblTotal.TabIndex = 6088;
            this.lblTotal.Text = "0";
            // 
            // rdbAuto
            // 
            this.rdbAuto.AutoSize = true;
            this.rdbAuto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAuto.Location = new System.Drawing.Point(152, 82);
            this.rdbAuto.Name = "rdbAuto";
            this.rdbAuto.Size = new System.Drawing.Size(110, 21);
            this.rdbAuto.TabIndex = 6083;
            this.rdbAuto.TabStop = true;
            this.rdbAuto.Text = "Auto Generate";
            this.rdbAuto.UseVisualStyleBackColor = true;
            this.rdbAuto.CheckedChanged += new System.EventHandler(this.rdbAuto_CheckedChanged);
            // 
            // rdbManual
            // 
            this.rdbManual.AutoSize = true;
            this.rdbManual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbManual.Location = new System.Drawing.Point(13, 82);
            this.rdbManual.Name = "rdbManual";
            this.rdbManual.Size = new System.Drawing.Size(130, 21);
            this.rdbManual.TabIndex = 6082;
            this.rdbManual.TabStop = true;
            this.rdbManual.Text = "Manual Accession";
            this.rdbManual.UseVisualStyleBackColor = true;
            this.rdbManual.CheckedChanged += new System.EventHandler(this.rdbManual_CheckedChanged);
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
            this.btnImport.Location = new System.Drawing.Point(262, 246);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(107, 30);
            this.btnImport.TabIndex = 6084;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.EnabledChanged += new System.EventHandler(this.btnImport_EnabledChanged);
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 226);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(356, 12);
            this.progressBar1.TabIndex = 6085;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.Black;
            this.Label5.Location = new System.Drawing.Point(10, 10);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(68, 17);
            this.Label5.TabIndex = 6117;
            this.Label5.Text = "Category :";
            // 
            // cmbItemCategory
            // 
            this.cmbItemCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbItemCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemCategory.FormattingEnabled = true;
            this.cmbItemCategory.Location = new System.Drawing.Point(113, 7);
            this.cmbItemCategory.Name = "cmbItemCategory";
            this.cmbItemCategory.Size = new System.Drawing.Size(256, 25);
            this.cmbItemCategory.TabIndex = 6116;
            this.cmbItemCategory.SelectedIndexChanged += new System.EventHandler(this.cmbItemCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 6125;
            this.label1.Text = "Subcategory :";
            // 
            // cmbSubCategory
            // 
            this.cmbSubCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSubCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSubCategory.FormattingEnabled = true;
            this.cmbSubCategory.Location = new System.Drawing.Point(113, 41);
            this.cmbSubCategory.Name = "cmbSubCategory";
            this.cmbSubCategory.Size = new System.Drawing.Size(256, 25);
            this.cmbSubCategory.TabIndex = 6124;
            this.cmbSubCategory.SelectedIndexChanged += new System.EventHandler(this.cmbSubCategory_SelectedIndexChanged);
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
            this.btnGenerate.Location = new System.Drawing.Point(272, 77);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(97, 30);
            this.btnGenerate.TabIndex = 6126;
            this.btnGenerate.Text = "Generate Excel";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtbFileName
            // 
            this.txtbFileName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFileName.Location = new System.Drawing.Point(89, 153);
            this.txtbFileName.Name = "txtbFileName";
            this.txtbFileName.ReadOnly = true;
            this.txtbFileName.Size = new System.Drawing.Size(280, 25);
            this.txtbFileName.TabIndex = 6127;
            this.txtbFileName.TextChanged += new System.EventHandler(this.txtbFileName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 6128;
            this.label2.Text = "File Name :";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.Silver;
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(285, 186);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(84, 30);
            this.btnBrowse.TabIndex = 6129;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 259);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 6130;
            this.label3.Text = "Total :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(113, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 17);
            this.label6.TabIndex = 6131;
            this.label6.Text = "Imported :";
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
            this.btnTutorial.Location = new System.Drawing.Point(253, 198);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654672;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // btnImage
            // 
            this.btnImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImage.BackColor = System.Drawing.Color.Silver;
            this.btnImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImage.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImage.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImage.Location = new System.Drawing.Point(315, 116);
            this.btnImage.Name = "btnImage";
            this.btnImage.Size = new System.Drawing.Size(54, 25);
            this.btnImage.TabIndex = 5654678;
            this.btnImage.Text = "...";
            this.btnImage.UseVisualStyleBackColor = false;
            this.btnImage.Click += new System.EventHandler(this.btnImage_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 5654677;
            this.label4.Text = "Image Folder :";
            // 
            // txtbImage
            // 
            this.txtbImage.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbImage.Location = new System.Drawing.Point(104, 116);
            this.txtbImage.Name = "txtbImage";
            this.txtbImage.ReadOnly = true;
            this.txtbImage.Size = new System.Drawing.Size(207, 25);
            this.txtbImage.TabIndex = 5654676;
            // 
            // FormImportItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 286);
            this.Controls.Add(this.btnImage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbImage);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbFileName);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSubCategory);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.cmbItemCategory);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.rdbAuto);
            this.Controls.Add(this.rdbManual);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnImport);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportItems";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Excel Data for Books & Items";
            this.Load += new System.EventHandler(this.FormImportItems_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormImportItems_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.Label lblTotal;
        internal System.Windows.Forms.RadioButton rdbAuto;
        private System.Windows.Forms.RadioButton rdbManual;
        internal System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox cmbItemCategory;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cmbSubCategory;
        internal System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtbFileName;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
        internal System.Windows.Forms.Button btnImage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtbImage;
    }
}