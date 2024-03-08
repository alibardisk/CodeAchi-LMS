namespace CodeAchi_Library_Management_System
{
    partial class FormMembershipPlan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMembershipPlan));
            this.dgvPlan = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roll_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbFees = new System.Windows.Forms.TextBox();
            this.cmbFrqncy = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtbMaxitems = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtbDesc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlan)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPlan
            // 
            this.dgvPlan.AllowUserToAddRows = false;
            this.dgvPlan.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPlan.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPlan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlan.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvPlan.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPlan.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPlan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPlan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.roll_no,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPlan.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPlan.EnableHeadersVisualStyles = false;
            this.dgvPlan.Location = new System.Drawing.Point(5, 135);
            this.dgvPlan.MultiSelect = false;
            this.dgvPlan.Name = "dgvPlan";
            this.dgvPlan.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPlan.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPlan.RowHeadersVisible = false;
            this.dgvPlan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlan.Size = new System.Drawing.Size(366, 107);
            this.dgvPlan.TabIndex = 6035;
            this.dgvPlan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvPlan_MouseDown);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "Sl No.";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 70;
            // 
            // roll_no
            // 
            this.roll_no.HeaderText = "Plan Name";
            this.roll_no.Name = "roll_no";
            this.roll_no.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "Days";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 60;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.HeaderText = "Fees";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.HeaderText = "Limit";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 60;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.Black;
            this.Label2.Location = new System.Drawing.Point(2, 3);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(78, 17);
            this.Label2.TabIndex = 6053;
            this.Label2.Text = "Plan Name :";
            // 
            // txtbName
            // 
            this.txtbName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbName.ForeColor = System.Drawing.Color.Black;
            this.txtbName.Location = new System.Drawing.Point(5, 23);
            this.txtbName.MaxLength = 500;
            this.txtbName.Name = "txtbName";
            this.txtbName.Size = new System.Drawing.Size(185, 25);
            this.txtbName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(197, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 6055;
            this.label1.Text = "Frequency :";
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
            this.btnReset.Location = new System.Drawing.Point(271, 99);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 30);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(165, 99);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(2, 245);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(213, 15);
            this.label19.TabIndex = 6073;
            this.label19.Text = "* To Update a Category right click on it.";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.updateToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.updateToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            this.updateToolStripMenuItem.MouseEnter += new System.EventHandler(this.updateToolStripMenuItem_MouseEnter);
            this.updateToolStripMenuItem.MouseLeave += new System.EventHandler(this.updateToolStripMenuItem_MouseLeave);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            this.deleteToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteToolStripMenuItem.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.MouseEnter += new System.EventHandler(this.deleteToolStripMenuItem_MouseEnter);
            this.deleteToolStripMenuItem.MouseLeave += new System.EventHandler(this.deleteToolStripMenuItem_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(2, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 17);
            this.label3.TabIndex = 6075;
            this.label3.Text = "Fees :";
            // 
            // txtbFees
            // 
            this.txtbFees.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbFees.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbFees.ForeColor = System.Drawing.Color.Black;
            this.txtbFees.Location = new System.Drawing.Point(5, 73);
            this.txtbFees.MaxLength = 500;
            this.txtbFees.Name = "txtbFees";
            this.txtbFees.Size = new System.Drawing.Size(87, 25);
            this.txtbFees.TabIndex = 2;
            this.txtbFees.Text = "0.00";
            this.txtbFees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbFees_KeyPress);
            this.txtbFees.Leave += new System.EventHandler(this.txtbFees_Leave);
            // 
            // cmbFrqncy
            // 
            this.cmbFrqncy.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbFrqncy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrqncy.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFrqncy.FormattingEnabled = true;
            this.cmbFrqncy.Items.AddRange(new object[] {
            "------Please Select------",
            "1_Daily",
            "7_Weekly",
            "30_Monthly",
            "90_Quaterly",
            "180_Halfyearly",
            "365_Yearly"});
            this.cmbFrqncy.Location = new System.Drawing.Point(200, 23);
            this.cmbFrqncy.Name = "cmbFrqncy";
            this.cmbFrqncy.Size = new System.Drawing.Size(171, 25);
            this.cmbFrqncy.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(98, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 6078;
            this.label5.Text = "Issue Limit :";
            // 
            // txtbMaxitems
            // 
            this.txtbMaxitems.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbMaxitems.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbMaxitems.ForeColor = System.Drawing.Color.Black;
            this.txtbMaxitems.Location = new System.Drawing.Point(101, 72);
            this.txtbMaxitems.MaxLength = 500;
            this.txtbMaxitems.Name = "txtbMaxitems";
            this.txtbMaxitems.Size = new System.Drawing.Size(89, 25);
            this.txtbMaxitems.TabIndex = 3;
            this.txtbMaxitems.Text = "0";
            this.txtbMaxitems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbMaxitems_KeyPress);
            this.txtbMaxitems.Leave += new System.EventHandler(this.txtbMaxitems_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(197, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 6080;
            this.label4.Text = "Description :";
            // 
            // txtbDesc
            // 
            this.txtbDesc.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbDesc.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbDesc.ForeColor = System.Drawing.Color.Black;
            this.txtbDesc.Location = new System.Drawing.Point(200, 71);
            this.txtbDesc.MaxLength = 500;
            this.txtbDesc.Name = "txtbDesc";
            this.txtbDesc.Size = new System.Drawing.Size(171, 25);
            this.txtbDesc.TabIndex = 4;
            // 
            // FormMembershipPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(376, 267);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbDesc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbMaxitems);
            this.Controls.Add(this.cmbFrqncy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtbFees);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtbName);
            this.Controls.Add(this.dgvPlan);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMembershipPlan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Membership Plan";
            this.Load += new System.EventHandler(this.FormMembershipPlan_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMembershipPlan_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlan)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtbName;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnReset;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label label19;
        public System.Windows.Forms.DataGridView dgvPlan;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtbFees;
        internal System.Windows.Forms.ComboBox cmbFrqncy;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn roll_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.TextBox txtbMaxitems;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtbDesc;
    }
}