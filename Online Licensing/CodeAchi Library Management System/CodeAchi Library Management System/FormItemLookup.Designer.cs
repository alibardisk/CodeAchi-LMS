namespace CodeAchi_Library_Management_System
{
    partial class FormItemLookup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemLookup));
            this.lblFldName = new System.Windows.Forms.Label();
            this.txtbSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.cmbItemCategory = new System.Windows.Forms.ComboBox();
            this.dgvItemDetails = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.issueItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reserveItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label22 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblScanning = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTtlRecord = new System.Windows.Forms.Label();
            this.pcbBook = new System.Windows.Forms.PictureBox();
            this.backgroundWorkerItems = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFldName.ForeColor = System.Drawing.Color.Black;
            this.lblFldName.Location = new System.Drawing.Point(449, 5);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(81, 17);
            this.lblFldName.TabIndex = 6059;
            this.lblFldName.Text = "Field Name :";
            // 
            // txtbSearch
            // 
            this.txtbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSearch.ForeColor = System.Drawing.Color.Black;
            this.txtbSearch.Location = new System.Drawing.Point(452, 25);
            this.txtbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbSearch.Name = "txtbSearch";
            this.txtbSearch.Size = new System.Drawing.Size(238, 25);
            this.txtbSearch.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(232, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 6057;
            this.label1.Text = "Search By :";
            // 
            // cmbSearch
            // 
            this.cmbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Location = new System.Drawing.Point(235, 25);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(204, 25);
            this.cmbSearch.TabIndex = 1;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.ForeColor = System.Drawing.Color.Black;
            this.categoryLabel.Location = new System.Drawing.Point(12, 5);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(144, 17);
            this.categoryLabel.TabIndex = 6055;
            this.categoryLabel.Text = "Book && Item Category :";
            // 
            // cmbItemCategory
            // 
            this.cmbItemCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbItemCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbItemCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbItemCategory.FormattingEnabled = true;
            this.cmbItemCategory.Location = new System.Drawing.Point(15, 25);
            this.cmbItemCategory.Name = "cmbItemCategory";
            this.cmbItemCategory.Size = new System.Drawing.Size(204, 25);
            this.cmbItemCategory.TabIndex = 0;
            this.cmbItemCategory.SelectedIndexChanged += new System.EventHandler(this.cmbItemCategory_SelectedIndexChanged);
            // 
            // dgvItemDetails
            // 
            this.dgvItemDetails.AllowUserToAddRows = false;
            this.dgvItemDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItemDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItemDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItemDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvItemDetails.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvItemDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItemDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItemDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column7,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemDetails.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvItemDetails.EnableHeadersVisualStyles = false;
            this.dgvItemDetails.Location = new System.Drawing.Point(12, 69);
            this.dgvItemDetails.MultiSelect = false;
            this.dgvItemDetails.Name = "dgvItemDetails";
            this.dgvItemDetails.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvItemDetails.RowHeadersVisible = false;
            this.dgvItemDetails.RowTemplate.Height = 78;
            this.dgvItemDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItemDetails.Size = new System.Drawing.Size(902, 362);
            this.dgvItemDetails.TabIndex = 6074;
            this.dgvItemDetails.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvItemDetails_RowsAdded);
            this.dgvItemDetails.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvItemDetails_RowsRemoved);
            this.dgvItemDetails.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvItemDetails_MouseDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "Sl No";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 70;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.HeaderText = "Image";
            this.Column7.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column7.Width = 75;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "Accession No";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 120;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.HeaderText = "Title of Item";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "Author";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 250;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column5.HeaderText = "Avilable";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Exp Date";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.Width = 85;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.issueItem,
            this.reserveItemToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 48);
            // 
            // issueItem
            // 
            this.issueItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.issueItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.issueItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.issueItem.Name = "issueItem";
            this.issueItem.Size = new System.Drawing.Size(142, 22);
            this.issueItem.Text = "Issue Item";
            this.issueItem.Click += new System.EventHandler(this.issueItem_Click);
            this.issueItem.MouseEnter += new System.EventHandler(this.issueItem_MouseEnter);
            this.issueItem.MouseLeave += new System.EventHandler(this.issueItem_MouseLeave);
            // 
            // reserveItemToolStripMenuItem
            // 
            this.reserveItemToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.reserveItemToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reserveItemToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.reserveItemToolStripMenuItem.Name = "reserveItemToolStripMenuItem";
            this.reserveItemToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.reserveItemToolStripMenuItem.Text = "Reserve Item";
            this.reserveItemToolStripMenuItem.Click += new System.EventHandler(this.reserveItemToolStripMenuItem_Click);
            this.reserveItemToolStripMenuItem.MouseEnter += new System.EventHandler(this.reserveItemToolStripMenuItem_MouseEnter);
            this.reserveItemToolStripMenuItem.MouseLeave += new System.EventHandler(this.reserveItemToolStripMenuItem_MouseLeave);
            // 
            // label22
            // 
            this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(12, 436);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(227, 15);
            this.label22.TabIndex = 6119;
            this.label22.Text = "* To issue/reserve an item right click on it.";
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
            this.btnReset.Location = new System.Drawing.Point(814, 20);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 30);
            this.btnReset.TabIndex = 4;
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
            this.btnSearch.Location = new System.Drawing.Point(706, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelStatus.Controls.Add(this.lblScanning);
            this.panelStatus.Controls.Add(this.progressBar1);
            this.panelStatus.Location = new System.Drawing.Point(273, 179);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(419, 87);
            this.panelStatus.TabIndex = 6144;
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
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(784, 434);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(86, 17);
            this.label14.TabIndex = 6144;
            this.label14.Text = "Item Found :";
            // 
            // lblTtlRecord
            // 
            this.lblTtlRecord.AutoSize = true;
            this.lblTtlRecord.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtlRecord.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTtlRecord.Location = new System.Drawing.Point(869, 434);
            this.lblTtlRecord.Name = "lblTtlRecord";
            this.lblTtlRecord.Size = new System.Drawing.Size(15, 17);
            this.lblTtlRecord.TabIndex = 6143;
            this.lblTtlRecord.Text = "0";
            // 
            // pcbBook
            // 
            this.pcbBook.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pcbBook.Image = ((System.Drawing.Image)(resources.GetObject("pcbBook.Image")));
            this.pcbBook.Location = new System.Drawing.Point(405, 171);
            this.pcbBook.Name = "pcbBook";
            this.pcbBook.Size = new System.Drawing.Size(70, 78);
            this.pcbBook.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbBook.TabIndex = 6145;
            this.pcbBook.TabStop = false;
            // 
            // backgroundWorkerItems
            // 
            this.backgroundWorkerItems.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerItems_DoWork);
            // 
            // FormItemLookup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 460);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblTtlRecord);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.dgvItemDetails);
            this.Controls.Add(this.lblFldName);
            this.Controls.Add(this.txtbSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.cmbItemCategory);
            this.Controls.Add(this.pcbBook);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemLookup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Book & Item Lookup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormItemLookup_FormClosing);
            this.Load += new System.EventHandler(this.FormItemLookup_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormItemLookup_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbBook)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblFldName;
        internal System.Windows.Forms.TextBox txtbSearch;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.ComboBox cmbSearch;
        internal System.Windows.Forms.Label categoryLabel;
        internal System.Windows.Forms.ComboBox cmbItemCategory;
        internal System.Windows.Forms.DataGridView dgvItemDetails;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem issueItem;
        internal System.Windows.Forms.Label label22;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ToolStripMenuItem reserveItemToolStripMenuItem;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.Label lblScanning;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTtlRecord;
        internal System.Windows.Forms.PictureBox pcbBook;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.ComponentModel.BackgroundWorker backgroundWorkerItems;
    }
}