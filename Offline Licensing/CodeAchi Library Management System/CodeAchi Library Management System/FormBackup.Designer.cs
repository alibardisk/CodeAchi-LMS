
namespace CodeAchi_Library_Management_System
{
    partial class FormBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackup));
            this.pcbloading = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pcbloading)).BeginInit();
            this.SuspendLayout();
            // 
            // pcbloading
            // 
            this.pcbloading.BackColor = System.Drawing.Color.Transparent;
            this.pcbloading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pcbloading.Image = ((System.Drawing.Image)(resources.GetObject("pcbloading.Image")));
            this.pcbloading.Location = new System.Drawing.Point(0, 5);
            this.pcbloading.Name = "pcbloading";
            this.pcbloading.Size = new System.Drawing.Size(331, 197);
            this.pcbloading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbloading.TabIndex = 24;
            this.pcbloading.TabStop = false;
            this.pcbloading.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 204);
            this.ControlBox = false;
            this.Controls.Add(this.pcbloading);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBackup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Taking Data Backup...";
            this.Load += new System.EventHandler(this.FormBackup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcbloading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.PictureBox pcbloading;
        private System.Windows.Forms.Timer timer1;
    }
}