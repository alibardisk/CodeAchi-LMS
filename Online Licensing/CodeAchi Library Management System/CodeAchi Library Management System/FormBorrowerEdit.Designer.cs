namespace CodeAchi_Library_Management_System
{
    partial class FormBorrowerEdit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBorrowerEdit));
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblBrCategory = new System.Windows.Forms.Label();
            this.dgvBorrower = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.lstbFieldList = new System.Windows.Forms.ListBox();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.txtbValue = new System.Windows.Forms.TextBox();
            this.txtbFreqncy = new System.Windows.Forms.TextBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtbBrrRnwDate = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.lblField = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.txtbAccn = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBorrower)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbCategory
            // 
            this.cmbCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.ForeColor = System.Drawing.Color.Black;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(13, 27);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(178, 25);
            this.cmbCategory.TabIndex = 6102;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // lblBrCategory
            // 
            this.lblBrCategory.AutoSize = true;
            this.lblBrCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblBrCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrCategory.ForeColor = System.Drawing.Color.Black;
            this.lblBrCategory.Location = new System.Drawing.Point(10, 7);
            this.lblBrCategory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBrCategory.Name = "lblBrCategory";
            this.lblBrCategory.Size = new System.Drawing.Size(106, 17);
            this.lblBrCategory.TabIndex = 6104;
            this.lblBrCategory.Text = "Select Category :";
            // 
            // dgvBorrower
            // 
            this.dgvBorrower.AllowUserToAddRows = false;
            this.dgvBorrower.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBorrower.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBorrower.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBorrower.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvBorrower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBorrower.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBorrower.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBorrower.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBorrower.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.accnNo,
            this.itemTitle,
            this.itemAuthor});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBorrower.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBorrower.EnableHeadersVisualStyles = false;
            this.dgvBorrower.Location = new System.Drawing.Point(9, 78);
            this.dgvBorrower.MultiSelect = false;
            this.dgvBorrower.Name = "dgvBorrower";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBorrower.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBorrower.RowHeadersVisible = false;
            this.dgvBorrower.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBorrower.Size = new System.Drawing.Size(609, 152);
            this.dgvBorrower.TabIndex = 6109;
            this.dgvBorrower.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBorrower_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.accnNo.HeaderText = "Borrower Id";
            this.accnNo.Name = "accnNo";
            this.accnNo.Width = 120;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemTitle.HeaderText = "Name";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.Width = 200;
            // 
            // itemAuthor
            // 
            this.itemAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemAuthor.HeaderText = "Address";
            this.itemAuthor.Name = "itemAuthor";
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Location = new System.Drawing.Point(18, 84);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(15, 14);
            this.chkbAll.TabIndex = 6110;
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlStatus.Controls.Add(this.label4);
            this.pnlStatus.Controls.Add(this.progressBar1);
            this.pnlStatus.Location = new System.Drawing.Point(164, 140);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(298, 38);
            this.pnlStatus.TabIndex = 6111;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkOrange;
            this.label4.Location = new System.Drawing.Point(4, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 17);
            this.label4.TabIndex = 6093;
            this.label4.Text = "Please wait while loading...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(286, 10);
            this.progressBar1.TabIndex = 6091;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(9, 236);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 17);
            this.label3.TabIndex = 6113;
            this.label3.Text = "Field Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lstbFieldList
            // 
            this.lstbFieldList.BackColor = System.Drawing.Color.Gainsboro;
            this.lstbFieldList.FormattingEnabled = true;
            this.lstbFieldList.ItemHeight = 17;
            this.lstbFieldList.Location = new System.Drawing.Point(9, 253);
            this.lstbFieldList.Name = "lstbFieldList";
            this.lstbFieldList.Size = new System.Drawing.Size(179, 140);
            this.lstbFieldList.TabIndex = 6112;
            this.lstbFieldList.SelectedIndexChanged += new System.EventHandler(this.lstbFieldList_SelectedIndexChanged);
            // 
            // cmbValue
            // 
            this.cmbValue.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbValue.ForeColor = System.Drawing.Color.Black;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Items.AddRange(new object[] {
            "Please select a gender...",
            "Male",
            "Female",
            "Others"});
            this.cmbValue.Location = new System.Drawing.Point(206, 256);
            this.cmbValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(217, 25);
            this.cmbValue.TabIndex = 6118;
            this.cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.FlatAppearance.BorderSize = 2;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(518, 236);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 6117;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(203, 236);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 17);
            this.label5.TabIndex = 6116;
            this.label5.Text = "Field Value :";
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUpdate.FlatAppearance.BorderSize = 2;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(323, 333);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 6115;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtbValue
            // 
            this.txtbValue.Location = new System.Drawing.Point(206, 256);
            this.txtbValue.Name = "txtbValue";
            this.txtbValue.Size = new System.Drawing.Size(217, 25);
            this.txtbValue.TabIndex = 6114;
            this.txtbValue.Enter += new System.EventHandler(this.txtbValue_Enter);
            this.txtbValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbValue_KeyPress);
            // 
            // txtbFreqncy
            // 
            this.txtbFreqncy.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFreqncy.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbFreqncy.ForeColor = System.Drawing.Color.Black;
            this.txtbFreqncy.Location = new System.Drawing.Point(206, 302);
            this.txtbFreqncy.MaxLength = 500;
            this.txtbFreqncy.Name = "txtbFreqncy";
            this.txtbFreqncy.Size = new System.Drawing.Size(83, 25);
            this.txtbFreqncy.TabIndex = 456482;
            this.txtbFreqncy.Text = "1";
            this.txtbFreqncy.TextChanged += new System.EventHandler(this.txtbFreqncy_TextChanged);
            this.txtbFreqncy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbFreqncy_KeyPress);
            this.txtbFreqncy.Leave += new System.EventHandler(this.txtbFreqncy_Leave);
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.ForeColor = System.Drawing.Color.Black;
            this.lblDuration.Location = new System.Drawing.Point(203, 284);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(65, 17);
            this.lblDuration.TabIndex = 456485;
            this.lblDuration.Text = "Duration :";
            // 
            // txtbBrrRnwDate
            // 
            this.txtbBrrRnwDate.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrRnwDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrRnwDate.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrRnwDate.Location = new System.Drawing.Point(334, 302);
            this.txtbBrrRnwDate.MaxLength = 500;
            this.txtbBrrRnwDate.Name = "txtbBrrRnwDate";
            this.txtbBrrRnwDate.ReadOnly = true;
            this.txtbBrrRnwDate.Size = new System.Drawing.Size(89, 25);
            this.txtbBrrRnwDate.TabIndex = 456483;
            this.txtbBrrRnwDate.Text = "0";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.ForeColor = System.Drawing.Color.Black;
            this.Label10.Location = new System.Drawing.Point(331, 283);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(56, 17);
            this.Label10.TabIndex = 456484;
            this.Label10.Text = "Validity :";
            // 
            // cmbSearch
            // 
            this.cmbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearch.ForeColor = System.Drawing.Color.Black;
            this.cmbSearch.FormattingEnabled = true;
            this.cmbSearch.Items.AddRange(new object[] {
            "Please select a type...",
            "Accession",
            "Subcategory",
            "Title",
            "Location"});
            this.cmbSearch.Location = new System.Drawing.Point(206, 28);
            this.cmbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(223, 25);
            this.cmbSearch.TabIndex = 456492;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.BackColor = System.Drawing.Color.Transparent;
            this.lblField.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblField.ForeColor = System.Drawing.Color.Black;
            this.lblField.Location = new System.Drawing.Point(434, 7);
            this.lblField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(46, 17);
            this.lblField.TabIndex = 456491;
            this.lblField.Text = "Value :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(203, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 456490;
            this.label1.Text = "Edit By :";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(479, 59);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(137, 15);
            this.lblMessage.TabIndex = 456488;
            this.lblMessage.Text = "Please press enter to add";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(467, 53);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(17, 21);
            this.lblNotification.TabIndex = 456487;
            this.lblNotification.Text = "*";
            // 
            // txtbAccn
            // 
            this.txtbAccn.BackColor = System.Drawing.Color.White;
            this.txtbAccn.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbAccn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbAccn.ForeColor = System.Drawing.Color.Black;
            this.txtbAccn.Location = new System.Drawing.Point(437, 28);
            this.txtbAccn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbAccn.Name = "txtbAccn";
            this.txtbAccn.Size = new System.Drawing.Size(177, 25);
            this.txtbAccn.TabIndex = 456486;
            this.txtbAccn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbAccn_KeyDown);
            // 
            // FormBorrowerEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 398);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.txtbAccn);
            this.Controls.Add(this.txtbFreqncy);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.txtbBrrRnwDate);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.cmbValue);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtbValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstbFieldList);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.chkbAll);
            this.Controls.Add(this.dgvBorrower);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblBrCategory);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBorrowerEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit or Delete Borrower";
            this.Load += new System.EventHandler(this.FormEditBorrower_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormEditBorrower_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBorrower)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ComboBox cmbCategory;
        internal System.Windows.Forms.Label lblBrCategory;
        internal System.Windows.Forms.DataGridView dgvBorrower;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemAuthor;
        private System.Windows.Forms.CheckBox chkbAll;
        private System.Windows.Forms.Panel pnlStatus;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar1;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lstbFieldList;
        internal System.Windows.Forms.ComboBox cmbValue;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TextBox txtbValue;
        internal System.Windows.Forms.TextBox txtbFreqncy;
        internal System.Windows.Forms.Label lblDuration;
        internal System.Windows.Forms.TextBox txtbBrrRnwDate;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.ComboBox cmbSearch;
        internal System.Windows.Forms.Label lblField;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblNotification;
        internal System.Windows.Forms.TextBox txtbAccn;
    }
}