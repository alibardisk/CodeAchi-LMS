using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
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
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        private void FormAbout_Load(object sender, EventArgs e)
        {
            this.Text ="About - "+ Application.ProductName;
            lblProdVersion.Text = Application.ProductVersion;
            lblDbSeries.Text = Properties.Settings.Default.databaseSeries;
            lblLimits.Text = Properties.Settings.Default.machineLimits;
            lblLicense.Text= Properties.Settings.Default.licenseType;
            DateTime renewDate = globalVarLms.expiryDate.AddDays(1);
            lblExpire.Text = renewDate.Day.ToString("00") + "/" + renewDate.Month.ToString("00") + "/" + renewDate.Year.ToString("0000");
            lblItemLimt.Text = globalVarLms.itemLimits.ToString();
            lblMac.Text =globalVarLms.machineId;
            lblIp.Text = GetGlobalIP();
            label16.Text = "     "+DateTime.Now.Year.ToString()+" CodeAchi Technologies Pvt.. Ltd.";

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'marketingDetails'";
                sqltCommnd.CommandType = CommandType.Text;
                if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) > 0)
                {
                    sqltCommnd.CommandText = "SELECT * FROM marketingDetails WHERE compName != 'CodeAchi'";
                    sqltCommnd.CommandType = CommandType.Text;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            lblCompany.Text = dataReader["compName"].ToString();
                            lblAddress.Text = dataReader["compAddress"].ToString();
                            lblWebsite.Text = dataReader["compWebsite"].ToString();
                            lblEmail.Text = dataReader["compMail"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["compLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pictureBox1.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        this.Height = 345;
                    }
                    dataReader.Close();
                }
                else
                {
                    this.Height = 345;
                }
                sqltConn.Close();
            }
            else
            {
                this.Height = 345;
            }
        }

        public string GetGlobalIP()
        {
            string IPAddress = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/global_IP.php");
                webRequest.Timeout = 8000;
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

        private void FormAbout_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void lnklblUpgrade_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormActivate activateProduct = new FormActivate();
            activateProduct.ShowDialog();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void lblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormAbout_Load(null, null);
        }

        private void pcbFacebook_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + "select youtube_url from installUninstallUrl where productName='" + Application.ProductName + "'");
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        requestResult = requestResult.Replace("$", "");
                        Process.Start(requestResult);
                    }
                }
                catch
                {
                    MessageBox.Show("Pleasetry again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pcbTwitter_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + "select youtube_url from installUninstallUrl where productName='" + Application.ProductName + "'");
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        requestResult = requestResult.Replace("$", "");
                        Process.Start(requestResult);
                    }
                }
                catch
                {
                    MessageBox.Show("Pleasetry again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pcbYoutube_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + "select youtube_url from installUninstallUrl where productName='" + Application.ProductName + "'");
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        requestResult = requestResult.Replace("$", "");
                        Process.Start(requestResult);
                    }
                }
                catch
                {
                    MessageBox.Show("Pleasetry again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pcbLinkedIn_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + "select youtube_url from installUninstallUrl where productName='" + Application.ProductName + "'");
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        requestResult = requestResult.Replace("$", "");
                        Process.Start(requestResult);
                    }
                }
                catch
                {
                    MessageBox.Show("Pleasetry again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lblMac_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblMac.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string txtbUserMail = "", settingsData = "", copyInfo = "", ttlItems="";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from itemDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                ttlItems = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select * from userDetails where userMail!=@userMail and isAdmin='" + true + "' limit 1";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbUserMail = dataReader["userMail"].ToString();
                    }
                }
                dataReader.Close();
                sqltCommnd.CommandText = "select settingsData from generalSettings";
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        settingsData = dataReader["settingsData"].ToString();
                        if (settingsData != "")
                        {
                            JObject jsonObj = JObject.Parse(settingsData);
                            copyInfo = "Machine Id : " + lblMac.Text + Environment.NewLine + "Serial Key : " + Properties.Settings.Default.serialKey +
                                Environment.NewLine + "License Name : " + lblLicense.Text + Environment.NewLine +
                                "Renew Date : " + lblExpire.Text + Environment.NewLine + "User Mail : " + txtbUserMail;
                        }
                    }
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
                MySqlConnection mysqlConn;
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MySqlCommand mysqlCmd;
                string queryString = "select count(id) from item_details;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                ttlItems = mysqlCmd.ExecuteScalar().ToString();

                queryString = "select * from user_details where userMail!=@userMail and isAdmin='" + true + "' limit 1";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbUserMail = dataReader["userMail"].ToString();
                    }
                }
                dataReader.Close();

                queryString = "select settingsData from general_settings";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        settingsData = dataReader["settingsData"].ToString();
                        if (settingsData != "")
                        {
                            JObject jsonObj = JObject.Parse(settingsData);
                            copyInfo = "Machine Id : " + lblMac.Text + Environment.NewLine + "Serial Key : " + Properties.Settings.Default.serialKey +
                                Environment.NewLine + "License Name : " + lblLicense.Text + Environment.NewLine +
                                "Renew Date : " + lblExpire.Text + Environment.NewLine + "User Mail : " + txtbUserMail;
                        }
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            Clipboard.SetText(copyInfo);
        }
    }
}
