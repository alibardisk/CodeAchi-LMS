
namespace CodeAchi_Library_Management_System
{
    partial class FormDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDetails));
            this.dgvItemDetails = new System.Windows.Forms.DataGridView();
            this.accnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvItemDetails
            // 
            this.dgvItemDetails.AllowUserToAddRows = false;
            this.dgvItemDetails.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItemDetails.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvItemDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItemDetails.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvItemDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItemDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(82)))), ((int)(((byte)(90)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvItemDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItemDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accnNo,
            this.itemTitle});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItemDetails.DefaultCellStyle = dataGridViewCellStyle9;
            this.dgvItemDetails.EnableHeadersVisualStyles = false;
            this.dgvItemDetails.Location = new System.Drawing.Point(3, 2);
            this.dgvItemDetails.MultiSelect = false;
            this.dgvItemDetails.Name = "dgvItemDetails";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItemDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvItemDetails.RowHeadersVisible = false;
            this.dgvItemDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItemDetails.Size = new System.Drawing.Size(593, 399);
            this.dgvItemDetails.TabIndex = 6102;
            // 
            // accnNo
            // 
            this.accnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accnNo.DefaultCellStyle = dataGridViewCellStyle8;
            this.accnNo.FillWeight = 131.7365F;
            this.accnNo.HeaderText = "Accession No.";
            this.accnNo.Name = "accnNo";
            this.accnNo.ReadOnly = true;
            this.accnNo.Width = 200;
            // 
            // itemTitle
            // 
            this.itemTitle.FillWeight = 68.26347F;
            this.itemTitle.HeaderText = "Title";
            this.itemTitle.Name = "itemTitle";
            this.itemTitle.ReadOnly = true;
            // 
            // FormDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 403);
            this.Controls.Add(this.dgvItemDetails);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Details";
            this.Load += new System.EventHandler(this.FormDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItemDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.DataGridView dgvItemDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn accnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTitle;
    }
}