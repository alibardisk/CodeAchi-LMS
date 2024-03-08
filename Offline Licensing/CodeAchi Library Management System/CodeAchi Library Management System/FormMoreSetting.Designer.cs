
namespace CodeAchi_Library_Management_System
{
    partial class FormMoreSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMoreSetting));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkbSat = new System.Windows.Forms.CheckBox();
            this.chkbFri = new System.Windows.Forms.CheckBox();
            this.chkbThur = new System.Windows.Forms.CheckBox();
            this.chkbWed = new System.Windows.Forms.CheckBox();
            this.chkbTue = new System.Windows.Forms.CheckBox();
            this.chkbMon = new System.Windows.Forms.CheckBox();
            this.chkbSun = new System.Windows.Forms.CheckBox();
            this.cmbDateFormat = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkbInclude = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkbSat);
            this.groupBox1.Controls.Add(this.chkbFri);
            this.groupBox1.Controls.Add(this.chkbThur);
            this.groupBox1.Controls.Add(this.chkbWed);
            this.groupBox1.Controls.Add(this.chkbTue);
            this.groupBox1.Controls.Add(this.chkbMon);
            this.groupBox1.Controls.Add(this.chkbSun);
            this.groupBox1.Location = new System.Drawing.Point(6, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 207);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Set Holidays";
            // 
            // chkbSat
            // 
            this.chkbSat.AutoSize = true;
            this.chkbSat.Location = new System.Drawing.Point(9, 179);
            this.chkbSat.Name = "chkbSat";
            this.chkbSat.Size = new System.Drawing.Size(78, 21);
            this.chkbSat.TabIndex = 456486;
            this.chkbSat.Text = "Saturday";
            this.chkbSat.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbSat.UseVisualStyleBackColor = true;
            this.chkbSat.CheckedChanged += new System.EventHandler(this.chkbSat_CheckedChanged);
            // 
            // chkbFri
            // 
            this.chkbFri.AutoSize = true;
            this.chkbFri.Location = new System.Drawing.Point(9, 154);
            this.chkbFri.Name = "chkbFri";
            this.chkbFri.Size = new System.Drawing.Size(62, 21);
            this.chkbFri.TabIndex = 456485;
            this.chkbFri.Text = "Friday";
            this.chkbFri.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbFri.UseVisualStyleBackColor = true;
            this.chkbFri.CheckedChanged += new System.EventHandler(this.chkbFri_CheckedChanged);
            // 
            // chkbThur
            // 
            this.chkbThur.AutoSize = true;
            this.chkbThur.Location = new System.Drawing.Point(9, 129);
            this.chkbThur.Name = "chkbThur";
            this.chkbThur.Size = new System.Drawing.Size(80, 21);
            this.chkbThur.TabIndex = 456484;
            this.chkbThur.Text = "Thursday";
            this.chkbThur.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbThur.UseVisualStyleBackColor = true;
            this.chkbThur.CheckedChanged += new System.EventHandler(this.chkbThur_CheckedChanged);
            // 
            // chkbWed
            // 
            this.chkbWed.AutoSize = true;
            this.chkbWed.Location = new System.Drawing.Point(9, 102);
            this.chkbWed.Name = "chkbWed";
            this.chkbWed.Size = new System.Drawing.Size(94, 21);
            this.chkbWed.TabIndex = 456483;
            this.chkbWed.Text = "Wednesday";
            this.chkbWed.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbWed.UseVisualStyleBackColor = true;
            this.chkbWed.CheckedChanged += new System.EventHandler(this.chkbWed_CheckedChanged);
            // 
            // chkbTue
            // 
            this.chkbTue.AutoSize = true;
            this.chkbTue.Location = new System.Drawing.Point(9, 74);
            this.chkbTue.Name = "chkbTue";
            this.chkbTue.Size = new System.Drawing.Size(75, 21);
            this.chkbTue.TabIndex = 456482;
            this.chkbTue.Text = "Tuesday";
            this.chkbTue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbTue.UseVisualStyleBackColor = true;
            this.chkbTue.CheckedChanged += new System.EventHandler(this.chkbTue_CheckedChanged);
            // 
            // chkbMon
            // 
            this.chkbMon.AutoSize = true;
            this.chkbMon.Location = new System.Drawing.Point(9, 47);
            this.chkbMon.Name = "chkbMon";
            this.chkbMon.Size = new System.Drawing.Size(75, 21);
            this.chkbMon.TabIndex = 456481;
            this.chkbMon.Text = "Monday";
            this.chkbMon.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbMon.UseVisualStyleBackColor = true;
            this.chkbMon.CheckedChanged += new System.EventHandler(this.chkbMon_CheckedChanged);
            // 
            // chkbSun
            // 
            this.chkbSun.AutoSize = true;
            this.chkbSun.Checked = true;
            this.chkbSun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbSun.Enabled = false;
            this.chkbSun.Location = new System.Drawing.Point(9, 22);
            this.chkbSun.Name = "chkbSun";
            this.chkbSun.Size = new System.Drawing.Size(69, 21);
            this.chkbSun.TabIndex = 456480;
            this.chkbSun.Text = "Sunday";
            this.chkbSun.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbSun.UseVisualStyleBackColor = true;
            // 
            // cmbDateFormat
            // 
            this.cmbDateFormat.BackColor = System.Drawing.Color.Gainsboro;
            this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFormat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDateFormat.FormattingEnabled = true;
            this.cmbDateFormat.Items.AddRange(new object[] {
            "dd/MM/yyyy",
            "MM/dd/yy",
            "dd/MM/yy",
            "yy/MM/dd"});
            this.cmbDateFormat.Location = new System.Drawing.Point(130, 57);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Size = new System.Drawing.Size(172, 25);
            this.cmbDateFormat.TabIndex = 456477;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(127, 37);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(87, 17);
            this.label15.TabIndex = 456476;
            this.label15.Text = "Date Format :";
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
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.Location = new System.Drawing.Point(202, 90);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 456478;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // chkbInclude
            // 
            this.chkbInclude.AutoSize = true;
            this.chkbInclude.Location = new System.Drawing.Point(130, 12);
            this.chkbInclude.Name = "chkbInclude";
            this.chkbInclude.Size = new System.Drawing.Size(184, 21);
            this.chkbInclude.TabIndex = 456479;
            this.chkbInclude.Text = "Issue Items Include Holiday";
            this.chkbInclude.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.chkbInclude.UseVisualStyleBackColor = true;
            // 
            // FormMoreSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(313, 211);
            this.Controls.Add(this.chkbInclude);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cmbDateFormat);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FormMoreSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "More Setting";
            this.Load += new System.EventHandler(this.FormMoreSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.ComboBox cmbDateFormat;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkbInclude;
        private System.Windows.Forms.CheckBox chkbSat;
        private System.Windows.Forms.CheckBox chkbFri;
        private System.Windows.Forms.CheckBox chkbThur;
        private System.Windows.Forms.CheckBox chkbWed;
        private System.Windows.Forms.CheckBox chkbTue;
        private System.Windows.Forms.CheckBox chkbMon;
        private System.Windows.Forms.CheckBox chkbSun;
    }
}