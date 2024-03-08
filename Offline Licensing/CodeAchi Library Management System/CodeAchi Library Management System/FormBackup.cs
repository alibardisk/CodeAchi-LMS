using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormBackup : Form
    {
        public FormBackup()
        {
            InitializeComponent();
        }

        public bool backupComplete;
        public bool getException;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(backupComplete)
            {
                timer1.Stop();
                MessageBox.Show("Backup successfully done.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            else if(getException)
            {
                this.Hide();
            }
        }

        private void FormBackup_Load(object sender, EventArgs e)
        {
            pcbloading.Visible = true;
        }
    }
}
