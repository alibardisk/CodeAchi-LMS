namespace CodeAchi_Library_Management_System
{
    partial class FormWishList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWishList));
            this.txtbAuthor = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtbType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbPublication = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dgvWishList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemPublication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.count1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnCsv = new System.Windows.Forms.Button();
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWishList)).BeginInit();
            this.SuspendLayout();
            // 
            // txtbAuthor
            // 
            this.txtbAuthor.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbAuthor.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbAuthor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbAuthor.ForeColor = System.Drawing.Color.Black;
            this.txtbAuthor.Location = new System.Drawing.Point(95, 61);
            this.txtbAuthor.MaxLength = 500;
            this.txtbAuthor.Name = "txtbAuthor";
            this.txtbAuthor.Size = new System.Drawing.Size(280, 25);
            this.txtbAuthor.TabIndex = 2;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.ForeColor = System.Drawing.Color.Black;
            this.lblAuthor.Location = new System.Drawing.Point(17, 64);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(58, 17);
            this.lblAuthor.TabIndex = 6090;
            this.lblAuthor.Text = "Author :";
            // 
            // txtbType
            // 
            this.txtbType.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbType.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbType.ForeColor = System.Drawing.Color.Black;
            this.txtbType.Location = new System.Drawing.Point(95, 8);
            this.txtbType.MaxLength = 500;
            this.txtbType.Name = "txtbType";
            this.txtbType.Size = new System.Drawing.Size(280, 25);
            this.txtbType.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(17, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 6088;
            this.label3.Text = "Item Type :";
            // 
            // txtbPublication
            // 
            this.txtbPublication.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPublication.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbPublication.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPublication.ForeColor = System.Drawing.Color.Black;
            this.txtbPublication.Location = new System.Drawing.Point(483, 61);
            this.txtbPublication.MaxLength = 500;
            this.txtbPublication.Name = "txtbPublication";
            this.txtbPublication.Size = new System.Drawing.Size(280, 25);
            this.txtbPublication.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(395, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 6094;
            this.label2.Text = "Publication :";
            // 
            // txtbTitle
            // 
            this.txtbTitle.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbTitle.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbTitle.ForeColor = System.Drawing.Color.Black;
            this.txtbTitle.Location = new System.Drawing.Point(483, 8);
            this.txtbTitle.MaxLength = 500;
            this.txtbTitle.Name = "txtbTitle";
            this.txtbTitle.Size = new System.Drawing.Size(280, 25);
            this.txtbTitle.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(395, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(72, 17);
            this.lblTitle.TabIndex = 6092;
            this.lblTitle.Text = "Item Title :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(17, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 15);
            this.label9.TabIndex = 6127;
            this.label9.Text = "(Ex : Books,CD,DVD)";
            // 
            // dgvWishList
            // 
            this.dgvWishList.AllowUserToAddRows = false;
            this.dgvWishList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvWishList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvWishList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvWishList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvWishList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvWishList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWishList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvWishList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWishList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.itemTitle,
            this.itemType,
            this.itemAuthor,
            this.itemPublication,
            this.count1});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvWishList.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvWishList.EnableHeadersVisualStyles = false;
            this.dgvWishList.Location = new System.Drawing.Point(12, 147);
            this.dgvWishList.Name = "dgvWishList";
            this.dgvWishList.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWishList.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvWishList.RowHeadersVisible = false;
            this.dgvWishList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWishList.Size = new System.Drawing.Size(768, 245);
            this.dgvWishList.TabIndex = 6128;
            this.dgvWishList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvWishList_RowsAdded);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "Sl No";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 70;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.itemTitle.HeaderText = "Title of Item";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
            // 
            // itemType
            // 
            this.itemType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemType.HeaderText = "Item Type";
            this.itemType.Name = "itemType";
            this.itemType.ReadOnly = true;
            this.itemType.Width = 90;
            // 
            // itemAuthor
            // 
            this.itemAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemAuthor.DefaultCellStyle = dataGridViewCellStyle4;
            this.itemAuthor.HeaderText = "Author";
            this.itemAuthor.Name = "itemAuthor";
            this.itemAuthor.ReadOnly = true;
            this.itemAuthor.Width = 150;
            // 
            // itemPublication
            // 
            this.itemPublication.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemPublication.HeaderText = "Publication";
            this.itemPublication.Name = "itemPublication";
            this.itemPublication.ReadOnly = true;
            this.itemPublication.Width = 150;
            // 
            // count1
            // 
            this.count1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.count1.DefaultCellStyle = dataGridViewCellStyle5;
            this.count1.HeaderText = "Count";
            this.count1.Name = "count1";
            this.count1.ReadOnly = true;
            this.count1.Width = 70;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReset.FlatAppearance.BorderSize = 2;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(666, 96);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(97, 30);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.FlatAppearance.BorderSize = 2;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(562, 96);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(97, 30);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Location = new System.Drawing.Point(8, 135);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(776, 1);
            this.panel3.TabIndex = 6144;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 406);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 17);
            this.label5.TabIndex = 6146;
            this.label5.Text = "Total Items :";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTotal.Location = new System.Drawing.Point(107, 406);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(15, 17);
            this.lblTotal.TabIndex = 6145;
            this.lblTotal.Text = "0";
            // 
            // btnCsv
            // 
            this.btnCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCsv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnCsv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCsv.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCsv.FlatAppearance.BorderSize = 2;
            this.btnCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCsv.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCsv.ForeColor = System.Drawing.Color.White;
            this.btnCsv.Location = new System.Drawing.Point(646, 399);
            this.btnCsv.Name = "btnCsv";
            this.btnCsv.Size = new System.Drawing.Size(134, 30);
            this.btnCsv.TabIndex = 6147;
            this.btnCsv.Text = "Export as CSV";
            this.btnCsv.UseVisualStyleBackColor = false;
            this.btnCsv.Click += new System.EventHandler(this.btnCsv_Click);
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
            this.btnTutorial.Location = new System.Drawing.Point(529, 108);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654673;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // FormWishList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 435);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.btnCsv);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvWishList);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtbPublication);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtbAuthor);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.txtbType);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWishList";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wish List Creation";
            this.Load += new System.EventHandler(this.FormWishList_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormWishList_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWishList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtbAuthor;
        internal System.Windows.Forms.Label lblAuthor;
        internal System.Windows.Forms.TextBox txtbType;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtbPublication;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtbTitle;
        internal System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.DataGridView dgvWishList;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemPublication;
        private System.Windows.Forms.DataGridViewTextBoxColumn count1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotal;
        internal System.Windows.Forms.Button btnCsv;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
    }
}