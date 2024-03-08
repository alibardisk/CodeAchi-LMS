
namespace CodeAchi_Library_Management_System
{
    partial class FormIsbnItems
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIsbnItems));
            this.lblIsbn_ = new System.Windows.Forms.Label();
            this.lblIsbn = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle_ = new System.Windows.Forms.Label();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.lblAuthor_ = new System.Windows.Forms.Label();
            this.dgvAccnList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.issueItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reserveItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccnList)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblIsbn_
            // 
            this.lblIsbn_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIsbn_.AutoSize = true;
            this.lblIsbn_.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsbn_.ForeColor = System.Drawing.Color.Black;
            this.lblIsbn_.Location = new System.Drawing.Point(4, 8);
            this.lblIsbn_.Name = "lblIsbn_";
            this.lblIsbn_.Size = new System.Drawing.Size(69, 17);
            this.lblIsbn_.TabIndex = 6064;
            this.lblIsbn_.Text = "ISBN No. :";
            // 
            // lblIsbn
            // 
            this.lblIsbn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblIsbn.AutoSize = true;
            this.lblIsbn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsbn.ForeColor = System.Drawing.Color.Black;
            this.lblIsbn.Location = new System.Drawing.Point(79, 8);
            this.lblIsbn.Name = "lblIsbn";
            this.lblIsbn.Size = new System.Drawing.Size(0, 17);
            this.lblIsbn.TabIndex = 6065;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(79, 37);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 17);
            this.lblTitle.TabIndex = 6067;
            // 
            // lblTitle_
            // 
            this.lblTitle_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTitle_.AutoSize = true;
            this.lblTitle_.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle_.ForeColor = System.Drawing.Color.Black;
            this.lblTitle_.Location = new System.Drawing.Point(4, 37);
            this.lblTitle_.Name = "lblTitle_";
            this.lblTitle_.Size = new System.Drawing.Size(40, 17);
            this.lblTitle_.TabIndex = 6066;
            this.lblTitle_.Text = "Title :";
            // 
            // lblAuthor
            // 
            this.lblAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.ForeColor = System.Drawing.Color.Black;
            this.lblAuthor.Location = new System.Drawing.Point(79, 64);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(240, 17);
            this.lblAuthor.TabIndex = 6069;
            // 
            // lblAuthor_
            // 
            this.lblAuthor_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAuthor_.AutoSize = true;
            this.lblAuthor_.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor_.ForeColor = System.Drawing.Color.Black;
            this.lblAuthor_.Location = new System.Drawing.Point(4, 64);
            this.lblAuthor_.Name = "lblAuthor_";
            this.lblAuthor_.Size = new System.Drawing.Size(58, 17);
            this.lblAuthor_.TabIndex = 6068;
            this.lblAuthor_.Text = "Author :";
            // 
            // dgvAccnList
            // 
            this.dgvAccnList.AllowUserToAddRows = false;
            this.dgvAccnList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAccnList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccnList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccnList.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvAccnList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvAccnList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccnList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAccnList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccnList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.accnNo,
            this.itemTitle});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccnList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAccnList.EnableHeadersVisualStyles = false;
            this.dgvAccnList.Location = new System.Drawing.Point(6, 86);
            this.dgvAccnList.MultiSelect = false;
            this.dgvAccnList.Name = "dgvAccnList";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccnList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAccnList.RowHeadersVisible = false;
            this.dgvAccnList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvAccnList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccnList.Size = new System.Drawing.Size(313, 110);
            this.dgvAccnList.TabIndex = 6089;
            this.dgvAccnList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvAccnList_MouseDoubleClick);
            this.dgvAccnList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvAccnList_MouseDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.FillWeight = 158.4523F;
            this.Column1.HeaderText = "Sl No.";
            this.Column1.Name = "Column1";
            this.Column1.Width = 70;
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.accnNo.FillWeight = 73.28419F;
            this.accnNo.HeaderText = "Accession No.";
            this.accnNo.Name = "accnNo";
            this.accnNo.ReadOnly = true;
            this.accnNo.Width = 140;
            // 
            // itemTitle
            // 
            this.itemTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemTitle.FillWeight = 68.26347F;
            this.itemTitle.HeaderText = "Available";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
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
            // 
            // reserveItemToolStripMenuItem
            // 
            this.reserveItemToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.reserveItemToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reserveItemToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.reserveItemToolStripMenuItem.Name = "reserveItemToolStripMenuItem";
            this.reserveItemToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.reserveItemToolStripMenuItem.Text = "Reserve Item";
            this.reserveItemToolStripMenuItem.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Location = new System.Drawing.Point(3, 199);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(227, 15);
            this.lblInfo.TabIndex = 6120;
            this.lblInfo.Text = "* To issue an item right/double click on it.";
            // 
            // FormIsbnItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(325, 220);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.dgvAccnList);
            this.Controls.Add(this.lblAuthor);
            this.Controls.Add(this.lblAuthor_);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTitle_);
            this.Controls.Add(this.lblIsbn);
            this.Controls.Add(this.lblIsbn_);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormIsbnItems";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item List";
            this.Load += new System.EventHandler(this.FormIsbnItems_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormIsbnItems_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccnList)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblIsbn_;
        internal System.Windows.Forms.Label lblIsbn;
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Label lblTitle_;
        internal System.Windows.Forms.Label lblAuthor;
        internal System.Windows.Forms.Label lblAuthor_;
        internal System.Windows.Forms.DataGridView dgvAccnList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem issueItem;
        private System.Windows.Forms.ToolStripMenuItem reserveItemToolStripMenuItem;
        internal System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
    }
}