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
    public partial class FormDetails : Form
    {
        public FormDetails()
        {
            InitializeComponent();
        }

        private void FormDetails_Load(object sender, EventArgs e)
        {
            dgvItemDetails.ClearSelection();
        }
    }
}
