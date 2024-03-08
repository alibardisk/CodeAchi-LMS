using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormOpacSetting : Form
    {
        public FormOpacSetting()
        {
            InitializeComponent();
        }

        private void FormOpacSetting_Load(object sender, EventArgs e)
        {
            lblNotification.Text = "";
        }

        private void FormOpacSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void pcbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pcbClose_MouseEnter(object sender, EventArgs e)
        {
            pcbClose.BackColor = Color.Red;
        }

        private void pcbClose_MouseLeave(object sender, EventArgs e)
        {
            pcbClose.BackColor = Color.DimGray;
        }
    }
}
