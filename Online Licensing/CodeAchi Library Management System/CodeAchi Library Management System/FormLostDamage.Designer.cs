namespace CodeAchi_Library_Management_System
{
    partial class FormLostDamage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLostDamage));
            this.cmbSearchBy = new System.Windows.Forms.ComboBox();
            this.txtbSearch = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.dgvItem = new System.Windows.Forms.DataGridView();
            this.chkbAll = new System.Windows.Forms.CheckBox();
            this.rdbLost = new System.Windows.Forms.RadioButton();
            this.rdbDamage = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbComment = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lblNotification = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnTutorial = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdbPerfect = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbSearchBy
            // 
            this.cmbSearchBy.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchBy.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchBy.ForeColor = System.Drawing.Color.Black;
            this.cmbSearchBy.FormattingEnabled = true;
            this.cmbSearchBy.Items.AddRange(new object[] {
            "Please choose a type...",
            "Accession No",
            "Borrower Id",
            "Location"});
            this.cmbSearchBy.Location = new System.Drawing.Point(91, 6);
            this.cmbSearchBy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSearchBy.Name = "cmbSearchBy";
            this.cmbSearchBy.Size = new System.Drawing.Size(186, 25);
            this.cmbSearchBy.TabIndex = 6079;
            this.cmbSearchBy.SelectedIndexChanged += new System.EventHandler(this.cmbSearchBy_SelectedIndexChanged);
            // 
            // txtbSearch
            // 
            this.txtbSearch.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSearch.ForeColor = System.Drawing.Color.Black;
            this.txtbSearch.Location = new System.Drawing.Point(301, 6);
            this.txtbSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbSearch.Name = "txtbSearch";
            this.txtbSearch.Size = new System.Drawing.Size(210, 25);
            this.txtbSearch.TabIndex = 6080;
            this.txtbSearch.TextChanged += new System.EventHandler(this.txtbSearch_TextChanged);
            this.txtbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbSearch_KeyDown);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(12, 9);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 17);
            this.label18.TabIndex = 6081;
            this.label18.Text = "Search By :";
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItem.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItem.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvItem.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItem.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.accnNo,
            this.itemTitle,
            this.itemAuthor,
            this.Status});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItem.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvItem.EnableHeadersVisualStyles = false;
            this.dgvItem.Location = new System.Drawing.Point(15, 57);
            this.dgvItem.MultiSelect = false;
            this.dgvItem.Name = "dgvItem";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItem.RowHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvItem.RowHeadersVisible = false;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(549, 183);
            this.dgvItem.TabIndex = 6082;
            this.dgvItem.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvItem_CellMouseClick);
            // 
            // chkbAll
            // 
            this.chkbAll.AutoSize = true;
            this.chkbAll.Location = new System.Drawing.Point(23, 62);
            this.chkbAll.Name = "chkbAll";
            this.chkbAll.Size = new System.Drawing.Size(15, 14);
            this.chkbAll.TabIndex = 6083;
            this.chkbAll.UseVisualStyleBackColor = true;
            this.chkbAll.CheckedChanged += new System.EventHandler(this.chkbAll_CheckedChanged);
            // 
            // rdbLost
            // 
            this.rdbLost.AutoSize = true;
            this.rdbLost.Location = new System.Drawing.Point(8, 48);
            this.rdbLost.Name = "rdbLost";
            this.rdbLost.Size = new System.Drawing.Size(50, 21);
            this.rdbLost.TabIndex = 6084;
            this.rdbLost.Text = "Lost";
            this.rdbLost.UseVisualStyleBackColor = true;
            // 
            // rdbDamage
            // 
            this.rdbDamage.AutoSize = true;
            this.rdbDamage.Location = new System.Drawing.Point(76, 47);
            this.rdbDamage.Name = "rdbDamage";
            this.rdbDamage.Size = new System.Drawing.Size(75, 21);
            this.rdbDamage.TabIndex = 6085;
            this.rdbDamage.Text = "Damage";
            this.rdbDamage.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(208, 248);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 6087;
            this.label4.Text = "Comment :";
            // 
            // txtbComment
            // 
            this.txtbComment.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbComment.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbComment.ForeColor = System.Drawing.Color.Black;
            this.txtbComment.Location = new System.Drawing.Point(211, 268);
            this.txtbComment.MaxLength = 500;
            this.txtbComment.Multiline = true;
            this.txtbComment.Name = "txtbComment";
            this.txtbComment.Size = new System.Drawing.Size(262, 54);
            this.txtbComment.TabIndex = 6086;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(479, 292);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 30);
            this.btnSave.TabIndex = 6088;
            this.btnSave.Text = "Sa&ve";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(374, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 15);
            this.label7.TabIndex = 6092;
            this.label7.Text = "Please press enter to add";
            // 
            // lblNotification
            // 
            this.lblNotification.AutoSize = true;
            this.lblNotification.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotification.ForeColor = System.Drawing.Color.Red;
            this.lblNotification.Location = new System.Drawing.Point(362, 31);
            this.lblNotification.Name = "lblNotification";
            this.lblNotification.Size = new System.Drawing.Size(17, 21);
            this.lblNotification.TabIndex = 6091;
            this.lblNotification.Text = "*";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            this.btnTutorial.Location = new System.Drawing.Point(538, 6);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new System.Drawing.Size(26, 18);
            this.btnTutorial.TabIndex = 5654673;
            this.btnTutorial.UseVisualStyleBackColor = false;
            this.btnTutorial.Click += new System.EventHandler(this.btnTutorial_Click);
            this.btnTutorial.MouseEnter += new System.EventHandler(this.btnTutorial_MouseEnter);
            this.btnTutorial.MouseLeave += new System.EventHandler(this.btnTutorial_MouseLeave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbPerfect);
            this.groupBox1.Controls.Add(this.rdbLost);
            this.groupBox1.Controls.Add(this.rdbDamage);
            this.groupBox1.Location = new System.Drawing.Point(15, 250);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 75);
            this.groupBox1.TabIndex = 5654674;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Item Status";
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
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.Width = 70;
            // 
            // rdbPerfect
            // 
            this.rdbPerfect.AutoSize = true;
            this.rdbPerfect.Location = new System.Drawing.Point(8, 22);
            this.rdbPerfect.Name = "rdbPerfect";
            this.rdbPerfect.Size = new System.Drawing.Size(170, 21);
            this.rdbPerfect.TabIndex = 6086;
            this.rdbPerfect.Text = "Found in good condition";
            this.rdbPerfect.UseVisualStyleBackColor = true;
            // 
            // FormLostDamage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 329);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnTutorial);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblNotification);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbComment);
            this.Controls.Add(this.chkbAll);
            this.Controls.Add(this.dgvItem);
            this.Controls.Add(this.cmbSearchBy);
            this.Controls.Add(this.txtbSearch);
            this.Controls.Add(this.label18);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLostDamage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lost or Damage Entry";
            this.Load += new System.EventHandler(this.FormLostDamage_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormLostDamage_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ComboBox cmbSearchBy;
        internal System.Windows.Forms.TextBox txtbSearch;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.DataGridView dgvItem;
        private System.Windows.Forms.CheckBox chkbAll;
        private System.Windows.Forms.RadioButton rdbLost;
        private System.Windows.Forms.RadioButton rdbDamage;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbComment;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblNotification;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolTip btnToolTip;
        internal System.Windows.Forms.Button btnTutorial;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.RadioButton rdbPerfect;
    }
}