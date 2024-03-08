namespace CodeAchi_Library_Management_System
{
    partial class FormActivate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormActivate));
            this.txtbMailId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbSerial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLater = new System.Windows.Forms.Button();
            this.btnActivate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtbMailId
            // 
            this.txtbMailId.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbMailId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbMailId.ForeColor = System.Drawing.Color.Black;
            this.txtbMailId.Location = new System.Drawing.Point(98, 12);
            this.txtbMailId.MaxLength = 500;
            this.txtbMailId.Name = "txtbMailId";
            this.txtbMailId.Size = new System.Drawing.Size(248, 25);
            this.txtbMailId.TabIndex = 6083;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(10, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 6084;
            this.label3.Text = "E-mail Id :";
            // 
            // txtbSerial
            // 
            this.txtbSerial.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbSerial.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbSerial.ForeColor = System.Drawing.Color.Black;
            this.txtbSerial.Location = new System.Drawing.Point(98, 52);
            this.txtbSerial.MaxLength = 500;
            this.txtbSerial.Name = "txtbSerial";
            this.txtbSerial.Size = new System.Drawing.Size(248, 25);
            this.txtbSerial.TabIndex = 6085;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 6086;
            this.label1.Text = "License Key :";
            // 
            // btnLater
            // 
            this.btnLater.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(67)))), ((int)(((byte)(66)))));
            this.btnLater.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLater.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLater.FlatAppearance.BorderSize = 2;
            this.btnLater.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLater.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnLater.ForeColor = System.Drawing.Color.White;
            this.btnLater.Location = new System.Drawing.Point(236, 87);
            this.btnLater.Name = "btnLater";
            this.btnLater.Size = new System.Drawing.Size(110, 30);
            this.btnLater.TabIndex = 6088;
            this.btnLater.Text = "Activate Later";
            this.btnLater.UseVisualStyleBackColor = false;
            this.btnLater.Click += new System.EventHandler(this.btnLater_Click);
            // 
            // btnActivate
            // 
            this.btnActivate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnActivate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActivate.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnActivate.FlatAppearance.BorderSize = 2;
            this.btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.Location = new System.Drawing.Point(98, 87);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(110, 30);
            this.btnActivate.TabIndex = 6087;
            this.btnActivate.Text = "Activate Now";
            this.btnActivate.UseVisualStyleBackColor = false;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // FormActivate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 128);
            this.Controls.Add(this.btnLater);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.txtbSerial);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbMailId);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormActivate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activate Product";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtbMailId;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtbSerial;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnLater;
        internal System.Windows.Forms.Button btnActivate;
    }
}