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
    public partial class FormQuotation : Form
    {
        public FormQuotation()
        {
            InitializeComponent();
        }

        private void FormQuotation_Load(object sender, EventArgs e)
        {
            cmbPurchase.SelectedIndex = 0;
            btnRequest.Enabled = false;
            if (Application.ProductName == "CodeAchi School Management System")
            {
                label9.Text = "Organization Name :";
            }
            else if (Application.ProductName == "CodeAchi PharmaSoft Retailer")
            {
                label9.Text = "Shop Name :";
                txtbTtlMember.Enabled = false;
                txtbTtlStock.Enabled = false;
            }
        }

        private void FormQuotation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void txtbTtlStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbTtlMember_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void chkbAgree_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAgree.Checked)
            {
                btnRequest.Enabled = true;
            }
            else
            {
                btnRequest.Enabled = false;
            }
        }

        private void lnklblPrvPolicy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start("http://codeachi.com/terms-policy");
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            if (txtbName.Text == "")
            {
                MessageBox.Show("Please enter your name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbName.Select();
                return;
            }

            if (txtbContact.Text == "")
            {
                MessageBox.Show("Please enter your address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbContact.Select();
                return;
            }

            if (txtbAddress.Text == "")
            {
                MessageBox.Show("Please enter your address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbAddress.Select();
                return;
            }
            if (txtbMail.Text == "")
            {
                MessageBox.Show("Please enter your email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMail.Select();
                return;
            }

            if (txtbOrgani.Text == "")
            {
                MessageBox.Show("Please enter the organization name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbOrgani.Select();
                return;
            }

            if (txtbCountry.Text == "")
            {
                MessageBox.Show("Please enter the country name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbCountry.Select();
                return;
            }

            if (txtbTtlStock.Enabled == true)
            {
                if (txtbTtlStock.Text == "0")
                {
                    MessageBox.Show("Please enter the total Stock of Items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbTtlStock.Select();
                    return;
                }
            }

            if (txtbTtlMember.Enabled == true)
            {
                if (txtbTtlMember.Text == "0")
                {
                    MessageBox.Show("Please enter the total number of members.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbTtlMember.Select();
                    return;
                }
            }

            if (cmbPurchase.SelectedIndex == 0)
            {
                MessageBox.Show("Please select when you want to procure.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbPurchase.Select();
                return;
            }
            //string macAddress = globalVarLms.machineId;
            if (IsConnectedToInternet())
            {
                try
                {
                    string insertQuery = "INSERT INTO quotation_request (name,address,country,organization_name," +
                        "user_email,contact,product,date_time,stock_books,members,status) values ('" + txtbName.Text + "'," +
                        "'" + txtbAddress.Text + "','" + txtbCountry.Text + "','" + txtbOrgani.Text + "','" + txtbMail.Text + "'," +
                        "'" + txtbContact.Text + "','" + Application.ProductName + "','" + CheckProductStatus.currentDate + "'," +
                        "'" + txtbTtlStock.Text + "','" + txtbTtlMember.Text + "','" + cmbPurchase.Text + "')";
                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(CheckProductStatus.insertApi + insertQuery);
                    webRequest.Timeout = 600000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult == "Inserted")
                    {
                        MessageBox.Show("Your Request recieved!" + Environment.NewLine + "allow us few time to get back to you," +
                            Environment.NewLine + "Make sure you added \"lms@codeachi.com\" [Copy]" + Environment.NewLine +
                            "to your safe sender list to avoid missing email from us.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
