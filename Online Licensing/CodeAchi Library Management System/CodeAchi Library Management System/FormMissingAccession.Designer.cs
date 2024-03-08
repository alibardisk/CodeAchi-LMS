namespace CodeAchi_Library_Management_System
{
    partial class FormMissingAccession
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMissingAccession));
            this.button1 = new System.Windows.Forms.Button();
            this.dgvAccnList = new System.Windows.Forms.DataGridView();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtbSuffix = new System.Windows.Forms.TextBox();
            this.txtbPrefix = new System.Windows.Forms.TextBox();
            this.rdbPrefix = new System.Windows.Forms.RadioButton();
            this.rdbSuffix = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pcbLoad = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTtlRecord = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccnList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 151);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvAccnList
            // 
            this.dgvAccnList.AllowUserToAddRows = false;
            this.dgvAccnList.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAccnList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAccnList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccnList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvAccnList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvAccnList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccnList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvAccnList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccnList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemTitle,
            this.accnNo});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccnList.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvAccnList.EnableHeadersVisualStyles = false;
            this.dgvAccnList.Location = new System.Drawing.Point(169, 12);
            this.dgvAccnList.MultiSelect = false;
            this.dgvAccnList.Name = "dgvAccnList";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccnList.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvAccnList.RowHeadersVisible = false;
            this.dgvAccnList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvAccnList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccnList.Size = new System.Drawing.Size(203, 169);
            this.dgvAccnList.TabIndex = 6089;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemTitle.DefaultCellStyle = dataGridViewCellStyle8;
            this.itemTitle.FillWeight = 68.26347F;
            this.itemTitle.HeaderText = "Sl. No.";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
            this.itemTitle.Width = 80;
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.accnNo.FillWeight = 131.7365F;
            this.accnNo.HeaderText = "Accession No.";
            this.accnNo.Name = "accnNo";
            this.accnNo.ReadOnly = true;
            this.accnNo.Width = 120;
            // 
            // txtbSuffix
            // 
            this.txtbSuffix.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSuffix.Location = new System.Drawing.Point(24, 120);
            this.txtbSuffix.Name = "txtbSuffix";
            this.txtbSuffix.Size = new System.Drawing.Size(137, 25);
            this.txtbSuffix.TabIndex = 6091;
            // 
            // txtbPrefix
            // 
            this.txtbPrefix.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPrefix.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtbPrefix.Location = new System.Drawing.Point(24, 65);
            this.txtbPrefix.Name = "txtbPrefix";
            this.txtbPrefix.Size = new System.Drawing.Size(137, 25);
            this.txtbPrefix.TabIndex = 6090;
            // 
            // rdbPrefix
            // 
            this.rdbPrefix.AutoSize = true;
            this.rdbPrefix.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbPrefix.Location = new System.Drawing.Point(7, 38);
            this.rdbPrefix.Name = "rdbPrefix";
            this.rdbPrefix.Size = new System.Drawing.Size(123, 21);
            this.rdbPrefix.TabIndex = 6094;
            this.rdbPrefix.TabStop = true;
            this.rdbPrefix.Text = "Search by Prefix";
            this.rdbPrefix.UseVisualStyleBackColor = true;
            // 
            // rdbSuffix
            // 
            this.rdbSuffix.AutoSize = true;
            this.rdbSuffix.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbSuffix.Location = new System.Drawing.Point(7, 96);
            this.rdbSuffix.Name = "rdbSuffix";
            this.rdbSuffix.Size = new System.Drawing.Size(122, 21);
            this.rdbSuffix.TabIndex = 6093;
            this.rdbSuffix.TabStop = true;
            this.rdbSuffix.Text = "Search by Suffix";
            this.rdbSuffix.UseVisualStyleBackColor = true;
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbAll.Location = new System.Drawing.Point(7, 12);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(154, 21);
            this.rdbAll.TabIndex = 6092;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All Missing Accession";
            this.rdbAll.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Silver;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnSearch.FlatAppearance.BorderSize = 2;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(58, 151);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(103, 30);
            this.btnSearch.TabIndex = 6095;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pcbLoad
            // 
            this.pcbLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcbLoad.Image = ((System.Drawing.Image)(resources.GetObject("pcbLoad.Image")));
            this.pcbLoad.Location = new System.Drawing.Point(0, 0);
            this.pcbLoad.Name = "pcbLoad";
            this.pcbLoad.Size = new System.Drawing.Size(377, 223);
            this.pcbLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbLoad.TabIndex = 6096;
            this.pcbLoad.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(166, 184);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(157, 17);
            this.label14.TabIndex = 6148;
            this.label14.Text = "Total Missing Accession :";
            // 
            // lblTtlRecord
            // 
            this.lblTtlRecord.AutoSize = true;
            this.lblTtlRecord.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtlRecord.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTtlRecord.Location = new System.Drawing.Point(321, 184);
            this.lblTtlRecord.Name = "lblTtlRecord";
            this.lblTtlRecord.Size = new System.Drawing.Size(15, 17);
            this.lblTtlRecord.TabIndex = 6147;
            this.lblTtlRecord.Text = "0";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMissingAccession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 223);
            this.Controls.Add(this.pcbLoad);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblTtlRecord);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.rdbPrefix);
            this.Controls.Add(this.rdbSuffix);
            this.Controls.Add(this.rdbAll);
            this.Controls.Add(this.txtbSuffix);
            this.Controls.Add(this.txtbPrefix);
            this.Controls.Add(this.dgvAccnList);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormMissingAccession";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Missing Accession";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMissingAccession_FormClosing);
            this.Load += new System.EventHandler(this.FormMissingAccession_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMissingAccession_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccnList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.DataGridView dgvAccnList;
        private System.Windows.Forms.TextBox txtbSuffix;
        private System.Windows.Forms.TextBox txtbPrefix;
        private System.Windows.Forms.RadioButton rdbPrefix;
        private System.Windows.Forms.RadioButton rdbSuffix;
        private System.Windows.Forms.RadioButton rdbAll;
        internal System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.PictureBox pcbLoad;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTtlRecord;
        private System.Windows.Forms.Timer timer1;
    }
}