namespace CodeAchi_Library_Management_System
{
    partial class FormReservation
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
            this.lblFldName = new System.Windows.Forms.Label();
            this.txtbTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbAuthor = new System.Windows.Forms.TextBox();
            this.txtbLocation = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReserve = new System.Windows.Forms.Button();
            this.rdbOriginal = new System.Windows.Forms.RadioButton();
            this.rdbNew = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFldName.ForeColor = System.Drawing.Color.Black;
            this.lblFldName.Location = new System.Drawing.Point(7, 50);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(39, 17);
            this.lblFldName.TabIndex = 6061;
            this.lblFldName.Text = "Title :";
            // 
            // txtbTitle
            // 
            this.txtbTitle.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbTitle.ForeColor = System.Drawing.Color.Black;
            this.txtbTitle.Location = new System.Drawing.Point(63, 47);
            this.txtbTitle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbTitle.Name = "txtbTitle";
            this.txtbTitle.ReadOnly = true;
            this.txtbTitle.Size = new System.Drawing.Size(346, 25);
            this.txtbTitle.TabIndex = 6060;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(7, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 6063;
            this.label1.Text = "Author :";
            // 
            // txtbAuthor
            // 
            this.txtbAuthor.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbAuthor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbAuthor.ForeColor = System.Drawing.Color.Black;
            this.txtbAuthor.Location = new System.Drawing.Point(63, 81);
            this.txtbAuthor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbAuthor.Name = "txtbAuthor";
            this.txtbAuthor.ReadOnly = true;
            this.txtbAuthor.Size = new System.Drawing.Size(346, 25);
            this.txtbAuthor.TabIndex = 6062;
            // 
            // txtbLocation
            // 
            this.txtbLocation.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbLocation.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbLocation.ForeColor = System.Drawing.Color.Black;
            this.txtbLocation.Location = new System.Drawing.Point(10, 147);
            this.txtbLocation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbLocation.Name = "txtbLocation";
            this.txtbLocation.Size = new System.Drawing.Size(221, 25);
            this.txtbLocation.TabIndex = 6064;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(75)))), ((int)(((byte)(73)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(387, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(22, 30);
            this.btnCancel.TabIndex = 6127;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReserve
            // 
            this.btnReserve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReserve.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnReserve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReserve.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnReserve.FlatAppearance.BorderSize = 2;
            this.btnReserve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReserve.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReserve.ForeColor = System.Drawing.Color.White;
            this.btnReserve.Location = new System.Drawing.Point(238, 145);
            this.btnReserve.Name = "btnReserve";
            this.btnReserve.Size = new System.Drawing.Size(100, 30);
            this.btnReserve.TabIndex = 6126;
            this.btnReserve.Text = "Save";
            this.btnReserve.UseVisualStyleBackColor = false;
            this.btnReserve.Click += new System.EventHandler(this.btnReserve_Click);
            // 
            // rdbOriginal
            // 
            this.rdbOriginal.AutoSize = true;
            this.rdbOriginal.Checked = true;
            this.rdbOriginal.Location = new System.Drawing.Point(10, 118);
            this.rdbOriginal.Name = "rdbOriginal";
            this.rdbOriginal.Size = new System.Drawing.Size(125, 21);
            this.rdbOriginal.TabIndex = 6128;
            this.rdbOriginal.TabStop = true;
            this.rdbOriginal.Text = "Original Location";
            this.rdbOriginal.UseVisualStyleBackColor = true;
            this.rdbOriginal.CheckedChanged += new System.EventHandler(this.rdbOriginal_CheckedChanged);
            // 
            // rdbNew
            // 
            this.rdbNew.AutoSize = true;
            this.rdbNew.Location = new System.Drawing.Point(161, 118);
            this.rdbNew.Name = "rdbNew";
            this.rdbNew.Size = new System.Drawing.Size(230, 21);
            this.rdbNew.TabIndex = 6129;
            this.rdbNew.Text = "New Location(Reserve Rack\\Shelf) :";
            this.rdbNew.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(7, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 17);
            this.label2.TabIndex = 6131;
            this.label2.Text = "Name :";
            // 
            // txtbName
            // 
            this.txtbName.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbName.ForeColor = System.Drawing.Color.Black;
            this.txtbName.Location = new System.Drawing.Point(63, 12);
            this.txtbName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtbName.Name = "txtbName";
            this.txtbName.ReadOnly = true;
            this.txtbName.Size = new System.Drawing.Size(346, 25);
            this.txtbName.TabIndex = 6130;
            // 
            // FormReservation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 191);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbName);
            this.Controls.Add(this.rdbNew);
            this.Controls.Add(this.rdbOriginal);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReserve);
            this.Controls.Add(this.txtbLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbAuthor);
            this.Controls.Add(this.lblFldName);
            this.Controls.Add(this.txtbTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormReservation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormReservation";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormReservation_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblFldName;
        internal System.Windows.Forms.TextBox txtbTitle;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbAuthor;
        internal System.Windows.Forms.TextBox txtbLocation;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnReserve;
        private System.Windows.Forms.RadioButton rdbOriginal;
        private System.Windows.Forms.RadioButton rdbNew;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtbName;
    }
}