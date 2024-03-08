namespace CodeAchi_Library_Management_System
{
    partial class FormProblemReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProblemReport));
            this.label1 = new System.Windows.Forms.Label();
            this.txtbProblem = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 17);
            this.label1.TabIndex = 6116;
            this.label1.Text = "Describe your problem :";
            // 
            // txtbProblem
            // 
            this.txtbProblem.BackColor = System.Drawing.Color.Gainsboro;
            this.txtbProblem.Location = new System.Drawing.Point(15, 95);
            this.txtbProblem.Name = "txtbProblem";
            this.txtbProblem.Size = new System.Drawing.Size(384, 112);
            this.txtbProblem.TabIndex = 6117;
            this.txtbProblem.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(6, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(407, 1);
            this.panel1.TabIndex = 6118;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.DimGray;
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSend.FlatAppearance.BorderSize = 2;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(307, 214);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(92, 30);
            this.btnSend.TabIndex = 6120;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(380, 51);
            this.label2.TabIndex = 6119;
            this.label2.Text = "At CodeAchi we are trying to resolve all your issues efficiently.\r\nPlease allow u" +
    "s some time as we are on the way to resolve your\r\nproblems. We will get back to " +
    "you on your registered email ID.";
            // 
            // FormProblemReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 251);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtbProblem);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProblemReport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report a Problem";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormProblemReport_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtbProblem;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label2;
    }
}