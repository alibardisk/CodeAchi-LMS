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
    public partial class FormRecieptSetting : Form
    {
        public FormRecieptSetting()
        {
            InitializeComponent();
        }

        private void FormRecieptSetting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.issueReciept)
            {
                rdbIssue.Checked = true;
            }
            if (Properties.Settings.Default.reissueReciept)
            {
                rdbReissue.Checked = true;
            }
            if (Properties.Settings.Default.returnReciept)
            {
                rdbReturn.Checked = true;
            }
            if (Properties.Settings.Default.paymentReciept)
            {
                rdbPayment.Checked = true;
            }
            txtbPrinter.Text = Properties.Settings.Default.recieptPrinter;
            if (cmbPaper.Items.IndexOf(Properties.Settings.Default.recieptPaper) >= 0)
            {
                cmbPaper.Text = Properties.Settings.Default.recieptPaper;
            }
            else
            {
                cmbPaper.SelectedIndex = 0;
            }
        }

        private void FormRecieptSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnPrinter_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                txtbPrinter.Text = printDialog.PrinterSettings.PrinterName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool issueReciept = false, reissueRecipt = false, returnReciept = false,paymentReciept=false;
            if(rdbIssue.Checked)
            {
                issueReciept = true;
            }
            if (rdbReissue.Checked)
            {
                reissueRecipt = true;
            }
            if (rdbReturn.Checked)
            {
                returnReciept = true;
            }
            if(rdbPayment.Checked)
            {
                paymentReciept = true;
            }
            if (txtbPrinter.Text == "")
            {
                MessageBox.Show("Please select a printer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbPaper.SelectedIndex == 0)
            {
                MessageBox.Show("Please select the paper name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbPaper.Select();
                return;
            }
            Properties.Settings.Default.issueReciept = issueReciept;
            Properties.Settings.Default.reissueReciept = reissueRecipt;
            Properties.Settings.Default.returnReciept = returnReciept;
            Properties.Settings.Default.paymentReciept = paymentReciept;
            Properties.Settings.Default.recieptPaper = cmbPaper.Text;
            Properties.Settings.Default.recieptPrinter = txtbPrinter.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Update successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
