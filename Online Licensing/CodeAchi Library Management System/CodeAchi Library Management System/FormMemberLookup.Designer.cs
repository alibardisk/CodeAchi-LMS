namespace CodeAchi_Library_Management_System
{
    partial class FormMemberLookup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMemberLookup));
            this.lblBrCategory = new System.Windows.Forms.Label();
            this.cmbBrrCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.txtbSearch = new System.Windows.Forms.TextBox();
            this.lblFldName = new System.Windows.Forms.Label();
            this.dgvBrrDetails = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pcbBook = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrrDetails)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBrCategory
            // 
            this.lblBrCategory.AutoSize = true;
            this.lblBrCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrCategory.ForeColor = System.Drawing.Color.Black;
            this.lblBrCategory.Location = new System.Drawing.Point(9, 5);
            this.lblBrCategory.Name = "lblBrCategory";
            this.lblBrCategory.Size = new System.Drawing.Size(126, 17);
            this.lblBrCategory.TabIndex = 6049;
            this.lblBrCategory.Text = "Borrower Category :";
            // 
            // cmbBrrCategory
            // 
            this.cmbBrrCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbBrrCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrrCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBrrCategory.FormattingEnabled = true;
            this.cmbBrrCategory.Location = new System.Drawing.Point(12, 26);
            this.cmbBrrCategory.Name = "cmbBrrCategory";
            this.cmbBrrCategory.Size = new System.Drawing.Size(204, 25);
            this.cmbBrrCategory.TabIndex = 6048;
            this.cmbBrrCategory.SelectedIndexChanged += new System.EventHandler(this.cmbBrrCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(223, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 6051;
            this.label1.Text = "Search By :";
            // 
            // cmbSearch
            // 
            this.cmbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Location = new System.Drawing.Point(226, 26);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(204, 25);
            this.cmbSearch.TabIndex = 6050;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // txtbSearch
            // 
            this.txtbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSearch.ForeColor = System.Drawing.Color.Black;
            this.txtbSearch.Location = new System.Drawing.Point(441, 26);
            this.txtbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbSearch.Name = "txtbSearch";
            this.txtbSearch.Size = new System.Drawing.Size(224, 25);
            this.txtbSearch.TabIndex = 6052;
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFldName.ForeColor = System.Drawing.Color.Black;
            this.lblFldName.Location = new System.Drawing.Point(438, 5);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(81, 17);
            this.lblFldName.TabIndex = 6053;
            this.lblFldName.Text = "Field Name :";
            // 
            // dgvBrrDetails
            // 
            this.dgvBrrDetails.AllowUserToAddRows = false;
            this.dgvBrrDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBrrDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvBrrDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvBrrDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBrrDetails.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvBrrDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBrrDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBrrDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvBrrDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBrrDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Image,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column8});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBrrDetails.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvBrrDetails.EnableHeadersVisualStyles = false;
            this.dgvBrrDetails.Location = new System.Drawing.Point(12, 74);
            this.dgvBrrDetails.MultiSelect = false;
            this.dgvBrrDetails.Name = "dgvBrrDetails";
            this.dgvBrrDetails.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBrrDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvBrrDetails.RowHeadersVisible = false;
            this.dgvBrrDetails.RowTemplate.Height = 78;
            this.dgvBrrDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBrrDetails.Size = new System.Drawing.Size(870, 337);
            this.dgvBrrDetails.TabIndex = 6075;
            this.dgvBrrDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvBrrDetails_MouseDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column1.HeaderText = "Sl No";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 65;
            // 
            // Image
            // 
            this.Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Image.HeaderText = "Image";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Width = 75;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "Id";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "Name";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "Gender";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 65;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.HeaderText = "Email";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 180;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.HeaderText = "Contact";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column8.HeaderText = "Address";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 26);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.updateToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.updateToolStripMenuItem.Text = "Issue Item";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            this.updateToolStripMenuItem.MouseEnter += new System.EventHandler(this.updateToolStripMenuItem_MouseEnter);
            this.updateToolStripMenuItem.MouseLeave += new System.EventHandler(this.updateToolStripMenuItem_MouseLeave);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(12, 432);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(243, 15);
            this.lblInfo.TabIndex = 6118;
            this.lblInfo.Text = "* To issue item of a borrower right click on it.";
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSend.FlatAppearance.BorderSize = 2;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(759, 417);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(123, 30);
            this.btnSend.TabIndex = 6153;
            this.btnSend.Text = "Send Email/Sms";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
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
            this.btnReset.Location = new System.Drawing.Point(782, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 30);
            this.btnReset.TabIndex = 6155;
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
            this.btnSearch.Location = new System.Drawing.Point(676, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 6154;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pcbBook
            // 
            this.pcbBook.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbBook.Image = ((System.Drawing.Image)(resources.GetObject("pcbBook.Image")));
            this.pcbBook.Location = new System.Drawing.Point(410, 187);
            this.pcbBook.Name = "pcbBook";
            this.pcbBook.Size = new System.Drawing.Size(70, 78);
            this.pcbBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbBook.TabIndex = 6156;
            this.pcbBook.TabStop = false;
            // 
            // FormMemberLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 452);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.dgvBrrDetails);
            this.Controls.Add(this.lblFldName);
            this.Controls.Add(this.txtbSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.lblBrCategory);
            this.Controls.Add(this.cmbBrrCategory);
            this.Controls.Add(this.pcbBook);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMemberLookup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Member Lookup";
            this.Load += new System.EventHandler(this.FormMemberLookup_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMemberLookup_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrrDetails)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblBrCategory;
        internal System.Windows.Forms.ComboBox cmbBrrCategory;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cmbSearch;
        internal System.Windows.Forms.TextBox txtbSearch;
        internal System.Windows.Forms.Label lblFldName;
        internal System.Windows.Forms.DataGridView dgvBrrDetails;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        internal System.Windows.Forms.Label lblInfo;
        internal System.Windows.Forms.Button btnSend;
        internal System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.PictureBox pcbBook;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}