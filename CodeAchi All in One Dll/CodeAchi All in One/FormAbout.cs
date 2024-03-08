using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {
            this.Text = "About - " + Application.ProductName;
            lblProdVersion.Text = Application.ProductVersion;
            //lblDbSeries.Text = Properties.Settings.Default.databaseSeries;
            lblLimits.Text = CheckProductStatus.machineLimits;
            lblLicense.Text = CheckProductStatus.licenseType;
            DateTime renewDate = CheckProductStatus.expiryDate.AddDays(1);
            lblExpire.Text = renewDate.Day.ToString("00") + "/" + renewDate.Month.ToString("00") + "/" + renewDate.Year.ToString("0000");
            lblItemLimt.Text = CheckProductStatus.itemLimits.ToString();
            lblMac.Text = CheckProductStatus.machineId;
            lblIp.Text = GetGlobalIP();
            if (Application.ProductName == "CodeAchi School Management System")
            {
                
            }
            else if (Application.ProductName== "CodeAchi PharmaSoft Retailer")
            {
                lnklblBuy.Text = "https://www.codeachi.com/product/pharmasoft-billing-software/";
            }
        }

        public string GetGlobalIP()
        {
            string IPAddress = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/global_IP.php");
                webRequest.Timeout = 2000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader strmReader = new StreamReader(dataStream);
                IPAddress = strmReader.ReadLine();
            }
            catch
            {

            }
            return IPAddress;
        }

        private void FormAbout_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnklblUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormActivate activateProduct = new FormActivate();
            activateProduct.ShowDialog();
        }

        private void lblSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start(lblSite.Text);
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lnklblBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start(lnklblBuy.Text);
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start(lblSupport.Text);
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

        private void lblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormAbout_Load(null, null);
        }

        private void lblMac_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblMac.Text);
        }
    }
}
