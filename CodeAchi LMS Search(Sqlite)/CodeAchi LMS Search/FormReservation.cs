using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_LMS_Search
{
    public partial class FormReservation : Form
    {
        public FormReservation()
        {
            InitializeComponent();
        }

        private const uint SC_CLOSE = 0xf060;
        private const uint MF_GRAYED = 0x01;
        private const int MF_ENABLED = 0x00000000;
        private const int MF_DISABLED = 0x00000002;

        [DllImport("user32.dll")]
        private static extern int EnableMenuItem(IntPtr hMenu, uint wIDEnableItem, uint wEnable);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        private void FormReservation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                   this.DisplayRectangle);
        }

        private void rdbOriginal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOriginal.Checked)
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
            this.Hide();
        }

        private void FormReservation_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }
    }
}
