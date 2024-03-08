namespace CodeAchi_Library_Management_System
{
    partial class FormItemEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormItemEdit));
            this.dgvItem = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtbAccn = new System.Windows.Forms.TextBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cmbSubcategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblField = new System.Windows.Forms.Label();
            this.lstbFieldList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbValue = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.cmbSearch = new System.Windows.Forms.ComboBox();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItem.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItem.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItem.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dgvItem.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItem.EnableHeadersVisualStyles = false;
            this.dgvItem.Location = new System.Drawing.Point(10, 76);
            this.dgvItem.MultiSelect = false;
            this.dgvItem.Name = "dgvItem";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItem.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItem.RowHeadersVisible = false;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(609, 140);
            this.dgvItem.TabIndex = 6083;
            this.dgvItem.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItem_CellMouseClick);
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
            this.accnNo.HeaderText = "Accession No.";
            this.accnNo.Name = "accnNo";
            this.accnNo.Width = 120;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemTitle.HeaderText = "Title";
            this.itemTitle.Name = "itemTitle";
            // 
            // itemAuthor
            // 
            this.itemAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemAuthor.HeaderText = "Author";
            this.itemAuthor.Name = "itemAuthor";
            this.itemAuthor.Width = 200;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(482, 56);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(137, 15);
            this.lblMessage.TabIndex = 6097;
            this.lblMessage.Text = "Please press enter to add";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(470, 50);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(17, 21);
            this.lblNotification.TabIndex = 6096;
            this.lblNotification.Text = "*";
            // 
            // cmbCategory
            // 
            this.cmbCategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.ForeColor = System.Drawing.Color.Black;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(10, 25);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(178, 25);
            this.cmbCategory.TabIndex = 6093;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // txtbAccn
            // 
            this.txtbAccn.BackColor = System.Drawing.Color.White;
            this.txtbAccn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbAccn.ForeColor = System.Drawing.Color.Black;
            this.txtbAccn.Location = new System.Drawing.Point(404, 25);
            this.txtbAccn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbAccn.Name = "txtbAccn";
            this.txtbAccn.Size = new System.Drawing.Size(215, 25);
            this.txtbAccn.TabIndex = 6094;
            this.txtbAccn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbAccn_KeyDown);
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.BackColor = System.Drawing.Color.Transparent;
            this.categoryLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.ForeColor = System.Drawing.Color.Black;
            this.categoryLabel.Location = new System.Drawing.Point(7, 4);
            this.categoryLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(68, 17);
            this.categoryLabel.TabIndex = 6095;
            this.categoryLabel.Text = "Category :";
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Location = new System.Drawing.Point(18, 82);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(15, 14);
            this.chkbAll.TabIndex = 6098;
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cmbSubcategory
            // 
            this.cmbSubcategory.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSubcategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubcategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSubcategory.ForeColor = System.Drawing.Color.Black;
            this.cmbSubcategory.FormattingEnabled = true;
            this.cmbSubcategory.Location = new System.Drawing.Point(404, 25);
            this.cmbSubcategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSubcategory.Name = "cmbSubcategory";
            this.cmbSubcategory.Size = new System.Drawing.Size(215, 25);
            this.cmbSubcategory.TabIndex = 6099;
            this.cmbSubcategory.SelectedIndexChanged += new System.EventHandler(this.cmbSubcategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(206, 4);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 6100;
            this.label1.Text = "Edit By :";
            // 
            // lblField
            // 
            this.lblField.AutoSize = true;
            this.lblField.BackColor = System.Drawing.Color.Transparent;
            this.lblField.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblField.ForeColor = System.Drawing.Color.Black;
            this.lblField.Location = new System.Drawing.Point(401, 4);
            this.lblField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(88, 17);
            this.lblField.TabIndex = 6101;
            this.lblField.Text = "Subcategory :";
            // 
            // lstbFieldList
            // 
            this.lstbFieldList.BackColor = System.Drawing.Color.Gainsboro;
            this.lstbFieldList.FormattingEnabled = true;
            this.lstbFieldList.ItemHeight = 17;
            this.lstbFieldList.Location = new System.Drawing.Point(9, 242);
            this.lstbFieldList.Name = "lstbFieldList";
            this.lstbFieldList.Size = new System.Drawing.Size(179, 123);
            this.lstbFieldList.TabIndex = 6103;
            this.lstbFieldList.SelectedIndexChanged += new System.EventHandler(this.lstbFieldList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(9, 225);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 17);
            this.label3.TabIndex = 6104;
            this.label3.Text = "Field Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtbValue
            // 
            this.txtbValue.Location = new System.Drawing.Point(209, 245);
            this.txtbValue.Name = "txtbValue";
            this.txtbValue.Size = new System.Drawing.Size(205, 25);
            this.txtbValue.TabIndex = 6;
            this.txtbValue.Enter += new System.EventHandler(this.txtbValue_Enter);
            this.txtbValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbValue_KeyPress);
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
            this.btnUpdate.Location = new System.Drawing.Point(314, 276);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 6077;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(206, 225);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 17);
            this.label5.TabIndex = 6106;
            this.label5.Text = "Field Value :";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.FlatAppearance.BorderSize = 2;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(519, 218);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 6107;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cmbValue
            // 
            this.cmbValue.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbValue.ForeColor = System.Drawing.Color.Black;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Location = new System.Drawing.Point(209, 245);
            this.cmbValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(205, 25);
            this.cmbValue.TabIndex = 6108;
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
            this.cmbSearch.Location = new System.Drawing.Point(209, 25);
            this.cmbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSearch.Name = "cmbSearch";
            this.cmbSearch.Size = new System.Drawing.Size(178, 25);
            this.cmbSearch.TabIndex = 6111;
            this.cmbSearch.SelectedIndexChanged += new System.EventHandler(this.cmbSearch_SelectedIndexChanged);
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
            this.btnTutorial.Location = new System.Drawing.Point(593, 3);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654671;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // FormItemEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 372);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.cmbSearch);
            this.Controls.Add(this.cmbValue);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtbValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstbFieldList);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.cmbSubcategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkbAll);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.txtbAccn);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.dgvItem);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit or Delete Items";
            this.Load += new System.EventHandler(this.FormItemEdit_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormItemEdit_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.DataGridView dgvItem;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblNotification;
        internal System.Windows.Forms.ComboBox cmbCategory;
        internal System.Windows.Forms.TextBox txtbAccn;
        internal System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.CheckBox chkbAll;
        private System.Windows.Forms.Timer timer1;
        internal System.Windows.Forms.ComboBox cmbSubcategory;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lblField;
        private System.Windows.Forms.ListBox lstbFieldList;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbValue;
        internal System.Windows.Forms.Button btnUpdate;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.ComboBox cmbValue;
        internal System.Windows.Forms.ComboBox cmbSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemAuthor;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
    }
}