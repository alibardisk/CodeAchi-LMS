using MySql.Data.MySqlClient;
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
    public partial class FormReportSetting : Form
    {
        public FormReportSetting()
        {
            InitializeComponent();
        }

        private void FormReportSetting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.reportFullName)
            {
                rdbFullName.Checked = true;
            }
            else
            {
                rdbShortName.Checked = true;
            }
            if (Properties.Settings.Default.reportAddress)
            {
                chkbAddress.Checked = true;
            }
            if (Properties.Settings.Default.reportContact)
            {
                chkbContact.Checked = true;
            }
            if (Properties.Settings.Default.reportMail)
            {
                chkbMail.Checked = true;
            }
            if (Properties.Settings.Default.reportSite)
            {
                chkbWebsite.Checked = true;
            }
        }

        private void FormReportSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isFullName=false, isAddress=false, isContact=false, isMail=false, isWebsite=false;
            if(rdbFullName.Checked)
            {
                isFullName = true;
            }
            if(chkbAddress.Checked)
            {
                isAddress = true;
            }
            if(chkbContact.Checked)
            {
                isContact = true;
            }
            if(chkbMail.Checked)
            {
                isMail = true;
            }
            if(chkbWebsite.Checked)
            {
                isWebsite = true;
            }
            Properties.Settings.Default.reportFullName = isFullName;
            Properties.Settings.Default.reportAddress = isAddress;
            Properties.Settings.Default.reportContact = isContact;
            Properties.Settings.Default.reportMail = isMail;
            Properties.Settings.Default.reportSite = isWebsite;
            Properties.Settings.Default.Save();
            MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
