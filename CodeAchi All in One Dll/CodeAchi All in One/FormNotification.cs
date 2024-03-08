using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public partial class FormNotification : Form
    {
        public FormNotification()
        {
            InitializeComponent();
        }

        private void FormNotification_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lblMessage.Text == "Seems you are not connected to the internet since last 15 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.")
            {
                Application.Exit();
            }
            else if (CheckProductStatus.productBlocked)
            {
                Application.Exit();
            }
            if (CheckProductStatus.licenseType != "Demo" && CheckProductStatus.productExpire)
            {
                this.Hide();
            }
            else
            {
                Application.Exit();
            }
        }

        private void lnklblBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                if (Application.ProductName == "CodeAchi School Management System")
                {
                    
                }
                else if (Application.ProductName == "CodeAchi PharmaSoft Retailer")
                {
                    Process.Start("https://www.codeachi.com/product/pharmasoft-billing-software/");
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void lnklblIdChat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start("http://codeachi.com/chat-support");
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lnklblRenew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start("http://codeachi.com/products/renew");
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lnkLblActivate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormActivate activateProduct = new FormActivate();
            this.Hide();
            activateProduct.ShowDialog();
        }

        private void lnklblQuotation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormQuotation quotationForm = new FormQuotation();
            quotationForm.ShowDialog();
        }

        private void FormNotification_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lblMessage.Text == "Seems you are not connected to the internet since last 15 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.")
            {
                Application.Exit();
            }
            else if (CheckProductStatus.productBlocked)
            {
                Application.Exit();
            }
            if (CheckProductStatus.licenseType != "Demo" && CheckProductStatus.productExpire)
            {
                this.Hide();
            }
            else
            {
                Application.Exit();
            }
        }

        private void FormNotification_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            if (Application.ProductName == "CodeAchi School Management System")
            {
                pictureBox1.Image = Properties.Resources.school_management;
            }
            else if (Application.ProductName == "CodeAchi PharmaSoft Retailer")
            {
                pictureBox1.Image = Properties.Resources.pharmaSoft;
            }
        }
    }
}
