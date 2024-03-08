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
    public partial class FormMoreSetting : Form
    {
        public FormMoreSetting()
        {
            InitializeComponent();
        }

        private void FormMoreSetting_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.holiDay==chkbMon.Text)
            {
                chkbMon.Checked = true;
            }
            else
            {
                chkbMon.Checked = false;
            }
            if (Properties.Settings.Default.holiDay == chkbTue.Text)
            {
                chkbTue.Checked = true;
            }
            else
            {
                chkbTue.Checked = false;
            }
            if (Properties.Settings.Default.holiDay == chkbWed.Text)
            {
                chkbWed.Checked = true;
            }
            else
            {
                chkbWed.Checked = false;
            }
            if (Properties.Settings.Default.holiDay == chkbThur.Text)
            {
                chkbThur.Checked = true;
            }
            else
            {
                chkbThur.Checked = false;
            }
            if (Properties.Settings.Default.holiDay == chkbFri.Text)
            {
                chkbFri.Checked = true;
            }
            else
            {
                chkbFri.Checked = false;
            }
            if (Properties.Settings.Default.holiDay == chkbSat.Text)
            {
                chkbSat.Checked = true;
            }
            else
            {
                chkbSat.Checked = false;
            }

            if (Properties.Settings.Default.includeHoliday == true)
            {
                chkbInclude.Checked = true;
            }
            else
            {
                chkbInclude.Checked = false;
            }

            cmbDateFormat.Text = Properties.Settings.Default.dateFormat;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (chkbMon.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbMon.Text;
            }
            if (chkbTue.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbTue.Text;
            }
            if (chkbWed.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbWed.Text;
            }
            if (chkbThur.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbThur.Text;
            }
            if (chkbFri.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbFri.Text;
            }
            if (chkbSat.Checked == true)
            {
                Properties.Settings.Default.holiDay = chkbSat.Text;
            }

            if (chkbInclude.Checked == true)
            {
                Properties.Settings.Default.includeHoliday = true;
            }
            else
            {
                Properties.Settings.Default.includeHoliday = false;
            }
            Properties.Settings.Default.dateFormat = cmbDateFormat.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkbMon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbMon.Checked)
            {
                chkbSat.Checked = false;
                chkbTue.Checked = false;
                chkbWed.Checked = false;
                chkbThur.Checked = false;
                chkbFri.Checked = false;
            }
        }

        private void chkbTue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbTue.Checked)
            {
                chkbMon.Checked = false;
                chkbSat.Checked = false;
                chkbWed.Checked = false;
                chkbThur.Checked = false;
                chkbFri.Checked = false;
            }
        }

        private void chkbWed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbWed.Checked)
            {
                chkbMon.Checked = false;
                chkbTue.Checked = false;
                chkbSat.Checked = false;
                chkbThur.Checked = false;
                chkbFri.Checked = false;
            }
        }

        private void chkbThur_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbThur.Checked)
            {
                chkbMon.Checked = false;
                chkbTue.Checked = false;
                chkbWed.Checked = false;
                chkbSat.Checked = false;
                chkbFri.Checked = false;
            }
        }

        private void chkbFri_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbFri.Checked)
            {
                chkbMon.Checked = false;
                chkbTue.Checked = false;
                chkbWed.Checked = false;
                chkbThur.Checked = false;
                chkbSat.Checked = false;
            }
        }

        private void chkbSat_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbSat.Checked)
            {
                chkbMon.Checked = false;
                chkbTue.Checked = false;
                chkbWed.Checked = false;
                chkbThur.Checked = false;
                chkbFri.Checked = false;
            }
        }
    }
}
