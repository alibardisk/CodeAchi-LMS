using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormQuotation : Form
    {
        public FormQuotation()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormQuotation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void ttlStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ttlMember_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FormQuotation_Load(object sender, EventArgs e)
        {
            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            string queryString = "select countryName from countryDetails";
            sqltCommnd.CommandText = queryString;
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                autoCollData.Clear();
                List<string> idList = (from IDataRecord r in dataReader
                                       select (string)r["countryName"]
                    ).ToList();
                autoCollData.AddRange(idList.ToArray());

                txtbCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbCountry.AutoCompleteCustomSource = autoCollData;
            }
            dataReader.Close();
            sqltCommnd = sqltConn.CreateCommand();
            queryString = "select userContact,userAddress from userDetails where userMail=@userMail";
            sqltCommnd.CommandText = queryString;
            sqltCommnd.Parameters.AddWithValue("@userMail", globalVarLms.currentUserId);
            dataReader = sqltCommnd.ExecuteReader();
            if(dataReader.HasRows)
            {
                while(dataReader.Read())
                {
                    txtbAddress.Text = dataReader["userAddress"].ToString();
                    txtbContact.Text = dataReader["userContact"].ToString();
                }
            }
            dataReader.Close();
            sqltCommnd = sqltConn.CreateCommand();
            queryString = "select countryName from generalSettings";
            sqltCommnd.CommandText = queryString;
            dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    txtbCountry.Text = dataReader["countryName"].ToString();
                }
            }
            dataReader.Close();
            txtbName.Text = globalVarLms.currentUserName;
            txtbMail.Text = globalVarLms.currentUserId;
            txtbOrgani.Text = globalVarLms.instName;
            cmbPurchase.SelectedIndex = 0;
            txtbTtlStock.Select();
            btnRequest.Enabled = false;
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

            if (txtbTtlStock.Text=="")
            {
                MessageBox.Show("Please enter the total Stock of Items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbTtlStock.Select();
                return;
            }

            if (txtbTtlMember.Text == "")
            {
                MessageBox.Show("Please enter the total number of members.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbTtlMember.Select();
                return;
            }

            if (cmbPurchase.SelectedIndex==0)
            {
                MessageBox.Show("Please select when you want to procure.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbPurchase.Select();
                return;
            }
            string macAddress = globalVarLms.machineId;
            if(IsConnectedToInternet())
            {
                try
                {
                    string currentDate = globalVarLms.currentDate.Day.ToString("00") + "/" + globalVarLms.currentDate.Month.ToString("00") + "/" + globalVarLms.currentDate.Year.ToString("0000");
                    string insertQuery = "INSERT INTO quotation_request (name,address,country,organization_name,"+
                        "user_email,contact,product,date_time,stock_books,members,status) values ('"+ txtbName.Text+"',"+
                        "'"+txtbAddress.Text+ "','" + txtbCountry.Text + "','" + txtbOrgani.Text + "','" + txtbMail.Text + "',"+
                        "'" + txtbContact.Text + "','" + Application.ProductName + "','" + currentDate + "',"+
                        "'" + txtbTtlStock.Text + "','" + txtbTtlMember.Text + "','" + cmbPurchase.Text + "')";
                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.insertApi+ insertQuery);
                    webRequest.Timeout = 600000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if(requestResult=="Inserted")
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

        private void txtbContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void chkbAgree_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAgree.Checked)
            {
                btnRequest.Enabled = true;
            }
            else
            {
                btnRequest.Enabled = false;
            }
        }
    }
}
