using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormServerConnection : Form
    {
        public FormServerConnection()
        {
            InitializeComponent();
        }

        public string licenseKey = "", licenseType = "";
        public bool connectionFail = false;

        private void FormServerConnection_Load(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
            btnUpdate.Enabled = false;
            if (!globalVarLms.sqliteData)
            {
                string[] dataList = globalVarLms.connectionString.Split(';');
                txtbHostIp.Text = dataList[0].Replace("server=", "");
                txtbUserName.Text = dataList[2].Replace(" user id=", "");
                txtbPassword.Text = dataList[3].Replace(" password=", "");
                txtbSchema.Text = dataList[1].Replace(" database=", "");
            }
        }

        private void FormServerConnection_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtbHostIp.Text == "")
            {
                MessageBox.Show("Please enter the server ip.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbUserName.Text == "")
            {
                MessageBox.Show("Please enter the user name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter the password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbSchema.Text == "")
            {
                MessageBox.Show("Please enter the schema name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string connectionstring = "server=" + txtbHostIp.Text + "; database=" + txtbSchema.Text + "; user id=" + txtbUserName.Text + "; password=" + txtbPassword.Text + "; pooling=false";
            MySqlConnection mysqlConn;

            mysqlConn = new MySqlConnection(connectionstring);
            try
            {
                mysqlConn.Open();
                if (mysqlConn.State == ConnectionState.Open)
                {
                    MessageBox.Show("Connection opened.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnUpdate.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Connectio not opened.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormServerConnection_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectionstring = "server=" + txtbHostIp.Text + "; database=" + txtbSchema.Text + "; user id=" + txtbUserName.Text + "; password=" + txtbPassword.Text + "; pooling=false";
            MySqlConnection mysqlConn;

            mysqlConn = new MySqlConnection(connectionstring);
            try
            {
                mysqlConn.Open();
                if (!connectionFail)
                {
                    MySqlCommand mysqlCmd;
                    string queryString = "select licenseType,licenseKey from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            licenseType = dataReader["licenseType"].ToString();
                            licenseKey = dataReader["licenseKey"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                    if (licenseType == "Colossal" || licenseType == "Grand" || licenseType == "Jumbo")
                    {

                        FormActivate activateProduct = new FormActivate();
                        activateProduct.licenseKey = licenseKey;
                        activateProduct.ShowDialog();
                        if (globalVarLms.licenseName == "Colossal" || globalVarLms.licenseName == "Grand" || globalVarLms.licenseName == "Jumbo")
                        {
                            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
                            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                            if (regKey == null)
                            {
                                if (Environment.Is64BitProcess)
                                {
                                    regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                                }
                                else
                                {
                                    regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                                }
                            }

                            byteData = Encoding.UTF8.GetBytes(connectionstring);
                            regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                            regKey.Close();
                            Properties.Settings.Default.databasePath = connectionstring;
                            globalVarLms.sqliteData = false;
                            Properties.Settings.Default.Save();
                            MessageBox.Show("Database setting updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show("This feature is not avialable in this license version.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("This feature is not avialable in this license version.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
                    byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                    if (regKey == null)
                    {
                        if (Environment.Is64BitProcess)
                        {
                            regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                        }
                        else
                        {
                            regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                        }
                    }

                    byteData = Encoding.UTF8.GetBytes(connectionstring);
                    regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                    regKey.Close();
                    Properties.Settings.Default.databasePath = connectionstring;
                    globalVarLms.sqliteData = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Database setting updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
