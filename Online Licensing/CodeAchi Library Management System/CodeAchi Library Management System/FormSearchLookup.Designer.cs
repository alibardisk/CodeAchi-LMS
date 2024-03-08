namespace CodeAchi_Library_Management_System
{
    partial class FormSearchLookup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearchLookup));
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.lblFldName = new System.Windows.Forms.Label();
            this.txtbSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblScanning = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pcbBook = new System.Windows.Forms.PictureBox();
            this.rdbFull = new System.Windows.Forms.RadioButton();
            this.rdbPartial = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerItems = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerBorrower = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.FlatAppearance.BorderSize = 2;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(781, 96);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 30);
            this.btnReset.TabIndex = 6164;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.FlatAppearance.BorderSize = 2;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(678, 96);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 6163;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToResizeColumns = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDetails.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetails.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDetails.EnableHeadersVisualStyles = false;
            this.dgvDetails.Location = new System.Drawing.Point(12, 133);
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.RowTemplate.Height = 78;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(870, 356);
            this.dgvDetails.TabIndex = 6162;
            this.dgvDetails.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetails_CellContentClick);
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFldName.ForeColor = System.Drawing.Color.Black;
            this.lblFldName.Location = new System.Drawing.Point(594, 9);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(81, 17);
            this.lblFldName.TabIndex = 6161;
            this.lblFldName.Text = "Field Name :";
            // 
            // txtbSearch
            // 
            this.txtbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSearch.ForeColor = System.Drawing.Color.Black;
            this.txtbSearch.Location = new System.Drawing.Point(597, 30);
            this.txtbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbSearch.Name = "txtbSearch";
            this.txtbSearch.Size = new System.Drawing.Size(285, 25);
            this.txtbSearch.TabIndex = 6160;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(396, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 6159;
            this.label1.Text = "Search By :";
            // 
            // cmbSearch
            // 
            this.cmbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Location = new System.Drawing.Point(399, 30);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(190, 25);
            this.cmbSearch.TabIndex = 6158;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.ForeColor = System.Drawing.Color.Black;
            this.categoryLabel.Location = new System.Drawing.Point(187, 9);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(68, 17);
            this.categoryLabel.TabIndex = 6157;
            this.categoryLabel.Text = "Category :";
            // 
            // cmbCategory
            // 
            this.cmbCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(190, 30);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(200, 25);
            this.cmbCategory.TabIndex = 6156;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 17);
            this.label3.TabIndex = 6166;
            this.label3.Text = "Search Type :";
            // 
            // cmbType
            // 
            this.cmbType.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "-------Select--------",
            "Member",
            "Item"});
            this.cmbType.Location = new System.Drawing.Point(12, 30);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(172, 25);
            this.cmbType.TabIndex = 6165;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelStatus.Controls.Add(this.lblScanning);
            this.panelStatus.Controls.Add(this.progressBar1);
            this.panelStatus.Location = new System.Drawing.Point(236, 237);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(419, 87);
            this.panelStatus.TabIndex = 6169;
            // 
            // lblScanning
            // 
            this.lblScanning.AutoSize = true;
            this.lblScanning.BackColor = System.Drawing.Color.Gainsboro;
            this.lblScanning.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScanning.ForeColor = System.Drawing.Color.DimGray;
            this.lblScanning.Location = new System.Drawing.Point(5, 9);
            this.lblScanning.Name = "lblScanning";
            this.lblScanning.Size = new System.Drawing.Size(314, 25);
            this.lblScanning.TabIndex = 6163;
            this.lblScanning.Text = "Please Wait...While Fetching Data...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 45);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(404, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6145;
            // 
            // pcbBook
            // 
            this.pcbBook.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbBook.Image = ((System.Drawing.Image)(resources.GetObject("pcbBook.Image")));
            this.pcbBook.Location = new System.Drawing.Point(757, 189);
            this.pcbBook.Name = "pcbBook";
            this.pcbBook.Size = new System.Drawing.Size(96, 118);
            this.pcbBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbBook.TabIndex = 6172;
            this.pcbBook.TabStop = false;
            // 
            // rdbFull
            // 
            this.rdbFull.AutoSize = true;
            this.rdbFull.Checked = true;
            this.rdbFull.Location = new System.Drawing.Point(678, 69);
            this.rdbFull.Name = "rdbFull";
            this.rdbFull.Size = new System.Drawing.Size(85, 21);
            this.rdbFull.TabIndex = 6173;
            this.rdbFull.TabStop = true;
            this.rdbFull.Text = "Full Match";
            this.rdbFull.UseVisualStyleBackColor = true;
            // 
            // rdbPartial
            // 
            this.rdbPartial.AutoSize = true;
            this.rdbPartial.Location = new System.Drawing.Point(780, 69);
            this.rdbPartial.Name = "rdbPartial";
            this.rdbPartial.Size = new System.Drawing.Size(101, 21);
            this.rdbPartial.TabIndex = 6174;
            this.rdbPartial.Text = "Partial match";
            this.rdbPartial.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorkerItems
            // 
            this.backgroundWorkerItems.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // backgroundWorkerBorrower
            // 
            this.backgroundWorkerBorrower.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBorrower_DoWork);
            // 
            // FormSearchLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 501);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.lblFldName);
            this.Controls.Add(this.txtbSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.rdbPartial);
            this.Controls.Add(this.rdbFull);
            this.Controls.Add(this.pcbBook);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSearchLookup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search & Lookup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSearchLookup_FormClosing);
            this.Load += new System.EventHandler(this.FormFind_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormFind_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.DataGridView dgvDetails;
        internal System.Windows.Forms.Label lblFldName;
        internal System.Windows.Forms.TextBox txtbSearch;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cmbSearch;
        internal System.Windows.Forms.Label categoryLabel;
        internal System.Windows.Forms.ComboBox cmbCategory;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblScanning;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.PictureBox pcbBook;
        private System.Windows.Forms.RadioButton rdbFull;
        private System.Windows.Forms.RadioButton rdbPartial;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerItems;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBorrower;
    }
}