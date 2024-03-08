using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormReservation : Form
    {
        public FormReservation()
        {
            InitializeComponent();
        }

        private void FormReservation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.DimGray, 2),
                       this.DisplayRectangle);
        }

        private void rdbOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbOriginal.Checked)
            {
                txtbLocation.Enabled = false;
            }
            else
            {
                txtbLocation.Enabled = true;
                txtbLocation.SelectAll();
            }
        }

        private void btnReserve_Click(object sender, EventArgs e)
        {
            globalVarLms.tempValue = txtbLocation.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
