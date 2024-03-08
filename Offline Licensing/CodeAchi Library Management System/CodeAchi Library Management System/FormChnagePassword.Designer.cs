namespace CodeAchi_Library_Management_System
{
    partial class FormChnagePassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChnagePassword));
            this.label6 = new System.Windows.Forms.Label();
            this.txtbCurrent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbConfirm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbNew = new System.Windows.Forms.TextBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(24, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 17);
            this.label6.TabIndex = 6083;
            this.label6.Text = "Current Password :";
            // 
            // txtbCurrent
            // 
            this.txtbCurrent.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbCurrent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbCurrent.ForeColor = System.Drawing.Color.Black;
            this.txtbCurrent.Location = new System.Drawing.Point(27, 27);
            this.txtbCurrent.MaxLength = 500;
            this.txtbCurrent.Name = "txtbCurrent";
            this.txtbCurrent.Size = new System.Drawing.Size(220, 25);
            this.txtbCurrent.TabIndex = 0;
            this.txtbCurrent.TextChanged += new System.EventHandler(this.txtbCurrent_TextChanged);
            //this.txtbCurrent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbCurrent_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(24, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 6085;
            this.label1.Text = "Confirm Password :";
            // 
            // txtbConfirm
            // 
            this.txtbConfirm.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbConfirm.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbConfirm.ForeColor = System.Drawing.Color.Black;
            this.txtbConfirm.Location = new System.Drawing.Point(27, 141);
            this.txtbConfirm.MaxLength = 500;
            this.txtbConfirm.Name = "txtbConfirm";
            this.txtbConfirm.Size = new System.Drawing.Size(220, 25);
            this.txtbConfirm.TabIndex = 2;
            this.txtbConfirm.TextChanged += new System.EventHandler(this.txtbConfirm_TextChanged);
            //this.txtbConfirm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbConfirm_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 6087;
            this.label2.Text = "New Password :";
            // 
            // txtbNew
            // 
            this.txtbNew.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbNew.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbNew.ForeColor = System.Drawing.Color.Black;
            this.txtbNew.Location = new System.Drawing.Point(27, 84);
            this.txtbNew.MaxLength = 500;
            this.txtbNew.Name = "txtbNew";
            this.txtbNew.Size = new System.Drawing.Size(220, 25);
            this.txtbNew.TabIndex = 1;
            this.txtbNew.TextChanged += new System.EventHandler(this.txtbNew_TextChanged);
            //this.txtbNew.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtbNew_KeyDown);
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(66)))));
            this.btnChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChange.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnChange.FlatAppearance.BorderSize = 2;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChange.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChange.ForeColor = System.Drawing.Color.White;
            this.btnChange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChange.Location = new System.Drawing.Point(257, 138);
            this.btnChange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(89, 30);
            this.btnChange.TabIndex = 3;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // FormChnagePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 178);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbConfirm);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtbCurrent);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChnagePassword";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormChnagePassword_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtbCurrent;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtbConfirm;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TextBox txtbNew;
        internal System.Windows.Forms.Button btnChange;
    }
}