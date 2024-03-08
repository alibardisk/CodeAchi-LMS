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
    public partial class FormReservationSetting : Form
    {
        public FormReservationSetting()
        {
            InitializeComponent();
        }

        private void FormReservationSetting_Load(object sender, EventArgs e)
        {
            numReserveDay.Value = Properties.Settings.Default.reserveDay;
        }

        private void FormReservationSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.reserveDay =Convert.ToInt32(numReserveDay.Value);
            Properties.Settings.Default.Save();
            MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
