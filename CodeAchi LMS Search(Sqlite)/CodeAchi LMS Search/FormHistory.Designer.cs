namespace CodeAchi_LMS_Search
{
    partial class FormHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHistory));
            this.label2 = new System.Windows.Forms.Label();
            this.txtbBrrId = new System.Windows.Forms.TextBox();
            this.txtbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvBook = new System.Windows.Forms.DataGridView();
            this.btnReturn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTtlRecord = new System.Windows.Forms.Label();
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.btnReissue = new System.Windows.Forms.Button();
            this.numDay = new System.Windows.Forms.NumericUpDown();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.acc_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_of_issue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expreDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.return_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDesignation = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDay)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 6137;
            this.label2.Text = "Borrower Id :";
            // 
            // txtbBrrId
            // 
            this.txtbBrrId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbBrrId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbBrrId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbBrrId.ForeColor = System.Drawing.Color.Black;
            this.txtbBrrId.Location = new System.Drawing.Point(94, 6);
            this.txtbBrrId.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtbBrrId.Name = "txtbBrrId";
            this.txtbBrrId.Size = new System.Drawing.Size(285, 25);
            this.txtbBrrId.TabIndex = 6136;
            this.txtbBrrId.TextChanged += new System.EventHandler(this.txtbBrrId_TextChanged);
            this.txtbBrrId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbBrrId_KeyDown);
            // 
            // txtbName
            // 
            this.txtbName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbName.Enabled = false;
            this.txtbName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbName.ForeColor = System.Drawing.Color.Black;
            this.txtbName.Location = new System.Drawing.Point(616, 6);
            this.txtbName.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.txtbName.Name = "txtbName";
            this.txtbName.Size = new System.Drawing.Size(285, 25);
            this.txtbName.TabIndex = 6138;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(485, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 17);
            this.label3.TabIndex = 6139;
            this.label3.Text = "Borrower Name :";
            // 
            // dgvBook
            // 
            this.dgvBook.AllowUserToAddRows = false;
            this.dgvBook.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBook.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvBook.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBook.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvBook.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvBook.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBook.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBook.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.acc_no,
            this.title,
            this.date_of_issue,
            this.expreDate,
            this.Fine,
            this.location,
            this.return_date,
            this.Author});
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBook.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvBook.EnableHeadersVisualStyles = false;
            this.dgvBook.Location = new System.Drawing.Point(7, 37);
            this.dgvBook.Name = "dgvBook";
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBook.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvBook.RowHeadersVisible = false;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvBook.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgvBook.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBook.Size = new System.Drawing.Size(894, 434);
            this.dgvBook.TabIndex = 6140;
            this.dgvBook.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBook_CellMouseClick);
            this.dgvBook.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBook_RowsAdded);
            this.dgvBook.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvBook_RowsRemoved);
            // 
            // btnReturn
            // 
            this.btnReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnReturn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReturn.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReturn.FlatAppearance.BorderSize = 2;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.ForeColor = System.Drawing.Color.White;
            this.btnReturn.Location = new System.Drawing.Point(783, 478);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(118, 30);
            this.btnReturn.TabIndex = 6141;
            this.btnReturn.Text = "&Return Book";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.EnabledChanged += new System.EventHandler(this.btnReturn_EnabledChanged);
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(4, 485);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(86, 17);
            this.label14.TabIndex = 6149;
            this.label14.Text = "Item Found :";
            // 
            // lblTtlRecord
            // 
            this.lblTtlRecord.AutoSize = true;
            this.lblTtlRecord.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtlRecord.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTtlRecord.Location = new System.Drawing.Point(89, 485);
            this.lblTtlRecord.Name = "lblTtlRecord";
            this.lblTtlRecord.Size = new System.Drawing.Size(15, 17);
            this.lblTtlRecord.TabIndex = 6148;
            this.lblTtlRecord.Text = "0";
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkbAll.Location = new System.Drawing.Point(15, 43);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(15, 14);
            this.chkbAll.TabIndex = 6150;
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // btnReissue
            // 
            this.btnReissue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnReissue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReissue.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReissue.FlatAppearance.BorderSize = 2;
            this.btnReissue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReissue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReissue.ForeColor = System.Drawing.Color.White;
            this.btnReissue.Location = new System.Drawing.Point(659, 478);
            this.btnReissue.Name = "btnReissue";
            this.btnReissue.Size = new System.Drawing.Size(118, 30);
            this.btnReissue.TabIndex = 6151;
            this.btnReissue.Text = "&Reissue Book";
            this.btnReissue.UseVisualStyleBackColor = false;
            this.btnReissue.EnabledChanged += new System.EventHandler(this.btnReissue_EnabledChanged);
            this.btnReissue.Click += new System.EventHandler(this.btnReissue_Click);
            // 
            // numDay
            // 
            this.numDay.Location = new System.Drawing.Point(513, 481);
            this.numDay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDay.Name = "numDay";
            this.numDay.Size = new System.Drawing.Size(57, 25);
            this.numDay.TabIndex = 6152;
            this.numDay.Visible = false;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // acc_no
            // 
            this.acc_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.acc_no.FillWeight = 30.92783F;
            this.acc_no.HeaderText = "Accession No.";
            this.acc_no.Name = "acc_no";
            this.acc_no.Width = 120;
            // 
            // title
            // 
            this.title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.title.FillWeight = 30.92783F;
            this.title.HeaderText = "Title";
            this.title.Name = "title";
            // 
            // date_of_issue
            // 
            this.date_of_issue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.date_of_issue.DefaultCellStyle = dataGridViewCellStyle13;
            this.date_of_issue.FillWeight = 30.92783F;
            this.date_of_issue.HeaderText = "Issue Date";
            this.date_of_issue.Name = "date_of_issue";
            // 
            // expreDate
            // 
            this.expreDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.expreDate.DefaultCellStyle = dataGridViewCellStyle14;
            this.expreDate.FillWeight = 30.92783F;
            this.expreDate.HeaderText = "Expected Date";
            this.expreDate.Name = "expreDate";
            this.expreDate.Width = 120;
            // 
            // Fine
            // 
            this.Fine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Fine.DefaultCellStyle = dataGridViewCellStyle15;
            this.Fine.HeaderText = "Fine";
            this.Fine.Name = "Fine";
            this.Fine.Width = 80;
            // 
            // location
            // 
            this.location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.location.DefaultCellStyle = dataGridViewCellStyle16;
            this.location.HeaderText = "Location";
            this.location.Name = "location";
            // 
            // return_date
            // 
            this.return_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.return_date.DefaultCellStyle = dataGridViewCellStyle17;
            this.return_date.FillWeight = 30.92783F;
            this.return_date.HeaderText = "Return Date";
            this.return_date.Name = "return_date";
            this.return_date.Visible = false;
            this.return_date.Width = 110;
            // 
            // Author
            // 
            this.Author.HeaderText = "Author";
            this.Author.Name = "Author";
            this.Author.Visible = false;
            // 
            // lblDesignation
            // 
            this.lblDesignation.AutoSize = true;
            this.lblDesignation.Location = new System.Drawing.Point(402, 485);
            this.lblDesignation.Name = "lblDesignation";
            this.lblDesignation.Size = new System.Drawing.Size(43, 17);
            this.lblDesignation.TabIndex = 6153;
            this.lblDesignation.Text = "label6";
            this.lblDesignation.Visible = false;
            // 
            // backgroundWorker1
            // 
            //this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FormHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(906, 513);
            this.Controls.Add(this.lblDesignation);
            this.Controls.Add(this.numDay);
            this.Controls.Add(this.btnReissue);
            this.Controls.Add(this.chkbAll);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblTtlRecord);
            this.Controls.Add(this.btnReturn);
            this.Controls.Add(this.dgvBook);
            this.Controls.Add(this.txtbName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbBrrId);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Issue History";
            this.Load += new System.EventHandler(this.FormHistory_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormHistory_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtbBrrId;
        internal System.Windows.Forms.TextBox txtbName;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.DataGridView dgvBook;
        internal System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTtlRecord;
        private System.Windows.Forms.CheckBox chkbAll;
        internal System.Windows.Forms.Button btnReissue;
        private System.Windows.Forms.NumericUpDown numDay;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn acc_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn title;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_of_issue;
        private System.Windows.Forms.DataGridViewTextBoxColumn expreDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fine;
        private System.Windows.Forms.DataGridViewTextBoxColumn location;
        private System.Windows.Forms.DataGridViewTextBoxColumn return_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        internal System.Windows.Forms.Label lblDesignation;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}