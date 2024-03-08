
namespace CodeAchi_Library_Management_System
{
    partial class FormRecieptSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecieptSetting));
            this.rdbIssue = new System.Windows.Forms.CheckBox();
            this.rdbReissue = new System.Windows.Forms.CheckBox();
            this.rdbReturn = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbPaper = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnPrinter = new System.Windows.Forms.Button();
            this.txtbPrinter = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.rdbPayment = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // rdbIssue
            // 
            this.rdbIssue.AutoSize = true;
            this.rdbIssue.Location = new System.Drawing.Point(8, 10);
            this.rdbIssue.Name = "rdbIssue";
            this.rdbIssue.Size = new System.Drawing.Size(133, 21);
            this.rdbIssue.TabIndex = 0;
            this.rdbIssue.Text = "Print Issue Receipt";
            this.rdbIssue.UseVisualStyleBackColor = true;
            // 
            // rdbReissue
            // 
            this.rdbReissue.AutoSize = true;
            this.rdbReissue.Location = new System.Drawing.Point(8, 37);
            this.rdbReissue.Name = "rdbReissue";
            this.rdbReissue.Size = new System.Drawing.Size(148, 21);
            this.rdbReissue.TabIndex = 1;
            this.rdbReissue.Text = "Print Reissue Receipt";
            this.rdbReissue.UseVisualStyleBackColor = true;
            // 
            // rdbReturn
            // 
            this.rdbReturn.AutoSize = true;
            this.rdbReturn.Location = new System.Drawing.Point(187, 12);
            this.rdbReturn.Name = "rdbReturn";
            this.rdbReturn.Size = new System.Drawing.Size(142, 21);
            this.rdbReturn.TabIndex = 2;
            this.rdbReturn.Text = "Print Return Receipt";
            this.rdbReturn.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(184, 75);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 17);
            this.label17.TabIndex = 456541;
            this.label17.Text = "Paper Name :";
            // 
            // cmbPaper
            // 
            this.cmbPaper.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaper.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaper.FormattingEnabled = true;
            this.cmbPaper.Items.AddRange(new object[] {
            "--Select--",
            "A4",
            "Roll Paper 58(48) x 210",
            "Roll Paper 58(48) x 297",
            "Roll Paper 58(48) x 3276",
            "Roll Paper 80 x 297 mm"});
            this.cmbPaper.Location = new System.Drawing.Point(187, 95);
            this.cmbPaper.Name = "cmbPaper";
            this.cmbPaper.Size = new System.Drawing.Size(179, 25);
            this.cmbPaper.TabIndex = 456540;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(5, 75);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(92, 17);
            this.label16.TabIndex = 456539;
            this.label16.Text = "Printer Name :";
            // 
            // btnPrinter
            // 
            this.btnPrinter.BackColor = System.Drawing.Color.Silver;
            this.btnPrinter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrinter.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.btnPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrinter.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrinter.Location = new System.Drawing.Point(88, 126);
            this.btnPrinter.Name = "btnPrinter";
            this.btnPrinter.Size = new System.Drawing.Size(93, 26);
            this.btnPrinter.TabIndex = 456537;
            this.btnPrinter.Text = "Printer";
            this.btnPrinter.UseVisualStyleBackColor = false;
            this.btnPrinter.Click += new System.EventHandler(this.btnPrinter_Click);
            // 
            // txtbPrinter
            // 
            this.txtbPrinter.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbPrinter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbPrinter.ForeColor = System.Drawing.Color.Black;
            this.txtbPrinter.Location = new System.Drawing.Point(8, 95);
            this.txtbPrinter.MaxLength = 15;
            this.txtbPrinter.Name = "txtbPrinter";
            this.txtbPrinter.ReadOnly = true;
            this.txtbPrinter.Size = new System.Drawing.Size(173, 25);
            this.txtbPrinter.TabIndex = 456538;
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
            this.btnSave.Location = new System.Drawing.Point(266, 166);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 456542;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rdbPayment
            // 
            this.rdbPayment.AutoSize = true;
            this.rdbPayment.Location = new System.Drawing.Point(187, 37);
            this.rdbPayment.Name = "rdbPayment";
            this.rdbPayment.Size = new System.Drawing.Size(153, 21);
            this.rdbPayment.TabIndex = 456543;
            this.rdbPayment.Text = "Print Payment Receipt";
            this.rdbPayment.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Location = new System.Drawing.Point(4, 70);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(363, 1);
            this.panel3.TabIndex = 456544;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.Location = new System.Drawing.Point(4, 159);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(363, 1);
            this.panel1.TabIndex = 456545;
            // 
            // FormRecieptSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(371, 200);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.rdbPayment);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.cmbPaper);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnPrinter);
            this.Controls.Add(this.txtbPrinter);
            this.Controls.Add(this.rdbReturn);
            this.Controls.Add(this.rdbReissue);
            this.Controls.Add(this.rdbIssue);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRecieptSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receipt Setting";
            this.Load += new System.EventHandler(this.FormRecieptSetting_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormRecieptSetting_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox rdbIssue;
        private System.Windows.Forms.CheckBox rdbReissue;
        private System.Windows.Forms.CheckBox rdbReturn;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.ComboBox cmbPaper;
        internal System.Windows.Forms.Label label16;
        internal System.Windows.Forms.Button btnPrinter;
        internal System.Windows.Forms.TextBox txtbPrinter;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox rdbPayment;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
    }
}