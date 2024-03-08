using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public partial class FormServerSetting : Form
    {
        public FormServerSetting()
        {
            InitializeComponent();
        }

        private const uint SC_CLOSE = 0xf060;
        private const uint MF_GRAYED = 0x01;
        private const int MF_ENABLED = 0x00000000;
        private const int MF_DISABLED = 0x00000002;

        [DllImport("user32.dll")]
        private static extern int EnableMenuItem(IntPtr hMenu, uint wIDEnableItem, uint wEnable);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        public string databaseName="", scriptName= "";
        string queryString, SqlConnStr;
        RegistryKey regKey;
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlCmd = new SqlCommand();
        SqlDataReader dataReader =null;
        string fileName = "";

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (cmbServerName.Text == "")
            {
                MessageBox.Show("Please select/enter Server Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbServerName.Focus();
                return;
            }
            if (cmbAuthentication.SelectedIndex == 0)
            {
                MessageBox.Show("Please select/enter Authenticatio Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbAuthentication.Focus();
                return;
            }

            if (cmbAuthentication.SelectedIndex == 2)
            {
                if (txtUserName.Text.Length == 0)
                {
                    MessageBox.Show("please enter user name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    return;
                }
                if (txtPassword.Text.Length == 0)
                {
                    MessageBox.Show("please enter password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                    return;
                }
            }

            Cursor = Cursors.WaitCursor;
            Timer2.Enabled = true;
            SqlConnection SqlConn = new SqlConnection();
            if (cmbAuthentication.SelectedIndex == 1)
            {
                SqlConnStr = "Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True";
            }
            else if (cmbAuthentication.SelectedIndex == 2)
            {
                SqlConnStr = "Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog=master;User ID=" + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text + ";MultipleActiveResultSets=True";
            }
            if ((SqlConn.State == ConnectionState.Closed))
            {
                SqlConn.ConnectionString = SqlConnStr;
                try
                {
                    SqlConn.Open();
                    btnblankDb.Enabled = true;
                    MessageBox.Show("Succsessfull DB Connnection", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(("Invalid DB SqlConnnection" + ("\r\n" + ex.Message)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbAuthentication.Enabled = true;
        }

        private void cmbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmbAuthentication.SelectedIndex == 2))
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                txtUserName.Text = "";
                txtPassword.Text = "";
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
                txtUserName.Text = "";
                txtPassword.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            regKey = Registry.CurrentUser.OpenSubKey(("SOFTWARE\\"+ Convert.ToBase64String(compName) + "\\"+ Convert.ToBase64String(byteData)), true);
            try
            {
                if ((cmbServerName.Text == ""))
                {
                    MessageBox.Show("Please Select/Enter Server Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                    cmbServerName.Focus();
                    return;
                }
                if (cmbAuthentication.SelectedIndex==0)
                {
                    MessageBox.Show("Please select/enter Authenticatio Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbAuthentication.Focus();
                    return;
                }
                if ((cmbAuthentication.SelectedIndex == 2))
                {
                    if ((txtUserName.Text.Length == 0))
                    {
                        MessageBox.Show("please enter user name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        txtUserName.Focus();
                        return;
                    }

                    if ((txtPassword.Text.Length == 0))
                    {
                        MessageBox.Show("please enter password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        txtPassword.Focus();
                        return;
                    }
                }

                Cursor = Cursors.WaitCursor;
                Timer2.Enabled = true;
                if ((cmbAuthentication.SelectedIndex == 1))
                {
                    sqlConn = new SqlConnection(("Data source="
                                    + (cmbServerName.Text + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True")));
                }

                if ((cmbAuthentication.SelectedIndex == 2))
                {
                    sqlConn = new SqlConnection(("Data Source="
                                    + (cmbServerName.Text.Trim() + (";Initial Catalog=master;User ID="
                                    + (txtUserName.Text.Trim() + (";Password="
                                    + (txtPassword.Text + ";MultipleActiveResultSets=True")))))));
                }

                sqlConn.Open();
                if ((sqlConn.State == ConnectionState.Open))
                {
                    if ((MessageBox.Show("It will create the Demo DB and configure the sql server, Do you want to proceed?",Application.ProductName,MessageBoxButtons.YesNo ,MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        if ((cmbAuthentication.SelectedIndex == 1))
                        {
                            byteData = Encoding.UTF8.GetBytes("Data Source="+cmbServerName.Text.Trim() + ";Initial Catalog=ERPS_DB;Integrated Security=True;MultipleActiveResultSets=True");
                            regKey.SetValue("Data10", Convert.ToBase64String(byteData));
                        }

                        if ((cmbAuthentication.SelectedIndex == 2))
                        {
                            byteData = Encoding.UTF8.GetBytes("Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog=ERPS_DB;User ID="
                                             + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text + ";MultipleActiveResultSets=True");
                            regKey.SetValue("Data10", Convert.ToBase64String(byteData));
                        }

                        CreateDemoDatabase();
                        MessageBox.Show("DB has been created and SQL Server setting has been saved successfully..." + ("\r\n" + "Application will be closed,Please start it again"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(("Unable to connect to sql server" + ("\r\n" + ex.Message)), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if ((sqlConn.State == ConnectionState.Open))
                {
                    sqlConn.Close();
                }

            }
        }

        private void CreateDemoDatabase()
        {
            try
            {
                if ((cmbServerName.Text == ""))
                {
                    MessageBox.Show("Please Select/Enter Server Name", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                    cmbServerName.Focus();
                    return;
                }

                if ((cmbAuthentication.SelectedIndex == 1))
                {
                    if ((txtUserName.Text.Length == 0))
                    {
                        MessageBox.Show("please enter user name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtUserName.Focus();
                        return;
                    }

                    if ((txtPassword.Text.Length == 0))
                    {
                        MessageBox.Show("please enter password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPassword.Focus();
                        return;
                    }

                }

                sqlConn = new SqlConnection(("Data source="
                                + (cmbServerName.Text + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True")));
                sqlConn.Open();
                string cb2 = "Select * from sysdatabases where name=\'ERPS_DB\'";
                sqlCmd = new SqlCommand(cb2);
                sqlCmd.Connection = sqlConn;
                dataReader = sqlCmd.ExecuteReader();
                if (dataReader.Read())
                {
                    sqlConn = new SqlConnection(("Data source="
                                    + (cmbServerName.Text + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True")));
                    sqlConn.Open();
                    string cb1 = "Drop Database ERPS_DB";
                    sqlCmd = new SqlCommand(cb1);
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.ExecuteNonQuery();
                    sqlConn.Close();
                    try
                    {
                        sqlConn = new SqlConnection(("Data source="
                                        + (cmbServerName.Text + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True")));
                        sqlConn.Open();
                        string cb = "Create Database ERPS_DB";
                        sqlCmd = new SqlCommand(cb);
                        sqlCmd.Connection = sqlConn;
                        sqlCmd.ExecuteNonQuery();
                        sqlConn.Close();
                        using (StreamReader sr = File.OpenText(Application.StartupPath + "\\DemoDBScript.sql"))
                        {
                            queryString = sr.ReadToEnd();
                            Server server = new Server(new ServerConnection(sqlConn));
                            server.ConnectionContext.ExecuteNonQuery(queryString);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    sqlConn = new SqlConnection(("Data source="
                                    + (cmbServerName.Text + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True")));
                    sqlConn.Open();
                    string cb3 = "Create Database ERPS_DB";
                    sqlCmd = new SqlCommand(cb3);
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.ExecuteNonQuery();
                    sqlConn.Close();
                    using (StreamReader sr = File.OpenText(Application.StartupPath + "\\DemoDBScript.sql"))
                    {
                        queryString = sr.ReadToEnd();
                        Server server = new Server(new ServerConnection(sqlConn));
                        server.ConnectionContext.ExecuteNonQuery(queryString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            sqlConn.Close();
        }

        private void btnblankDb_Click(object sender, EventArgs e)
        {
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            regKey = Registry.CurrentUser.OpenSubKey(("SOFTWARE\\" + Convert.ToBase64String(compName) + "\\" + Convert.ToBase64String(byteData)), true);
            try
            {
                if ((cmbServerName.Text == ""))
                {
                    MessageBox.Show("Please Select/Enter Server Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                    cmbServerName.Focus();
                    return;
                }
                if (cmbAuthentication.SelectedIndex==0)
                {
                    MessageBox.Show("Please select/enter Authenticatio Name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbAuthentication.Focus();
                    return;
                }
                if ((cmbAuthentication.SelectedIndex == 2))
                {
                    if ((txtUserName.Text.Length == 0))
                    {
                        MessageBox.Show("please enter user name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        txtUserName.Focus();
                        return;
                    }

                    if ((txtPassword.Text.Length == 0))
                    {
                        MessageBox.Show("please enter password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        txtPassword.Focus();
                        return;
                    }
                }

                Cursor = Cursors.WaitCursor;
                Timer2.Enabled = true;
                if (cmbAuthentication.SelectedIndex == 1)
                {
                    SqlConnStr = "Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True";
                }
                if (cmbAuthentication.SelectedIndex == 2)
                {
                    SqlConnStr = "Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog=master;User ID=" + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text + ";MultipleActiveResultSets=True";
                }
                sqlConn = new SqlConnection(SqlConnStr);
                sqlConn.Open();
                if ((sqlConn.State == ConnectionState.Open))
                {
                    if ((MessageBox.Show("It will create the Blank DB and configure the sql server, Do you want to proceed?", Application.ProductName,MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes))
                    {
                        CreateBlankDatabase();
                        if ((cmbAuthentication.SelectedIndex == 1))
                        {
                            byteData = Encoding.UTF8.GetBytes("Data Source="+cmbServerName.Text.Trim() + ";Initial Catalog="+databaseName+";Integrated Security=True;MultipleActiveResultSets=True");
                            regKey.SetValue("Data10", Convert.ToBase64String(byteData));
                        }

                        if ((cmbAuthentication.SelectedIndex == 2))
                        {
                            byteData = Encoding.UTF8.GetBytes("Data Source=" + cmbServerName.Text.Trim() + ";Initial Catalog="+databaseName+";User ID="
                                            + txtUserName.Text.Trim() + ";Password=" + txtPassword.Text + ";MultipleActiveResultSets=True");
                            regKey.SetValue("Data10", Convert.ToBase64String(byteData));
                        }
                        MessageBox.Show(("DB has been created and SQL Server setting has been saved successfully..." + ("\r\n" + "Application will be closed,Please start it again")), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to sql server" + ("\r\n" + ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlConn.Close();
        }

        private void CreateBlankDatabase()
        {
            try
            {
                sqlConn = new SqlConnection(SqlConnStr);
                sqlConn.Open();
                string cb2 = "Select * from sysdatabases where name=\'"+databaseName+"\'";
                sqlCmd = new SqlCommand(cb2);
                sqlCmd.Connection = sqlConn;
                dataReader = sqlCmd.ExecuteReader();
                if (dataReader.Read())
                {
                    //sqlConn = new SqlConnection(SqlConnStr);
                    //sqlConn.Open();
                    //string cb1 = "Drop Database " + databaseName + "";
                    //sqlCmd = new SqlCommand(cb1);
                    //sqlCmd.Connection = sqlConn;
                    //sqlCmd.ExecuteNonQuery();
                    //sqlConn.Close();
                    //try
                    //{
                    //    sqlConn = new SqlConnection(SqlConnStr);
                    //    sqlConn.Open();
                    //    string cb = "Create Database "+databaseName;
                    //    sqlCmd = new SqlCommand(cb);
                    //    sqlCmd.Connection = sqlConn;
                    //    sqlCmd.ExecuteNonQuery();
                    //    sqlConn.Close();
                    //    using (StreamReader sr = File.OpenText(Application.StartupPath + @"\"+scriptName))
                    //    {
                    //        queryString = sr.ReadToEnd();
                    //        Server server = new Server(new ServerConnection(sqlConn));
                    //        server.ConnectionContext.ExecuteNonQuery(queryString);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
                else
                {
                    sqlConn = new SqlConnection(SqlConnStr);
                    sqlConn.Open();
                    string cb3 = "Create Database "+databaseName;
                    sqlCmd = new SqlCommand(cb3);
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.ExecuteNonQuery();
                    sqlConn.Close();
                    using (StreamReader sr = File.OpenText(Application.StartupPath + @"\"+scriptName))
                    {
                        queryString = sr.ReadToEnd();
                        Server server = new Server(new ServerConnection(sqlConn));
                        server.ConnectionContext.ExecuteNonQuery(queryString);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlConn.Close();
        }

        private void FormServerSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                     this.DisplayRectangle);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            Timer2.Enabled = false;
        }

        private void FormServerSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to close the application ? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    /// Cancel the Closing event from closing the form. /
                    e.Cancel = true;
                }
                else
                {
                    /// Closing the form. /
                    e.Cancel = false;
                    Application.Exit();
                }
            }
        }

        private void FormServerSetting_Load(object sender, EventArgs e)
        {
            panelDownload.Visible = false;
            cmbAuthentication.SelectedIndex = 0;
            btnblankDb.Enabled = false;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void lnkLblSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Timer2.Enabled = true;
                cmbServerName.Items.Clear();
                DataTable sqlServerList = SqlDataSourceEnumerator.Instance.GetDataSources();
                foreach (System.Data.DataRow row in sqlServerList.Rows)
                {
                    if ((row["InstanceName"] as string) != null)
                        cmbServerName.Items.Add(row["ServerName"] + "\\" + row["InstanceName"]);
                    else
                        cmbServerName.Items.Add(row["ServerName"].ToString());
                }
                Cursor = Cursors.Default;
                if (sqlServerList.Rows.Count == 0)
                {
                    if (MessageBox.Show("Opps! There is no SQLServer installed in your LAN," + Environment.NewLine +
                        " To run this application you need SQLserver 2008 as database server." + Application.ProductName + " Would you like to install it?",
                        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        panelDownload.Visible = true;
                        string tempPath = Path.GetTempPath() + Application.ProductName;
                        if (Directory.Exists(tempPath))
                        {
                            Directory.Delete(tempPath, true);
                        }
                        Directory.CreateDirectory(tempPath);
                       
                        IntPtr hMenu = GetSystemMenu(this.Handle, false);
                        EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                        //==================Check for update==========================
                        if (IsConnectedToInternet() == true)
                        {
                            try
                            {
                                Cursor = Cursors.WaitCursor;
                                Timer2.Enabled = true;
                                string queryToCheck = "SELECT sqlUrl FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                WebRequest webRequest = WebRequest.Create(CheckProductStatus.selectApi + queryToCheck);
                                webRequest.Timeout = 8000;
                                WebResponse webResponse = webRequest.GetResponse();
                                Stream dataStream = webResponse.GetResponseStream();
                                StreamReader strmReader = new StreamReader(dataStream);
                                string requestResult = strmReader.ReadLine();
                                if (requestResult != "")
                                {
                                    requestResult = requestResult.Replace("$", "");
                                    fileName = tempPath + requestResult.Substring(requestResult.LastIndexOf("/"));
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                                        wc.DownloadFileAsync(new System.Uri(requestResult), fileName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Cursor = Cursors.Default;
                                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                        }
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    cmbServerName.SelectedIndex = 0;
                }
            }
            catch
            {
                MessageBox.Show("Sorry unable to find SQL Server instance" + Environment.NewLine + "If you have installed SQL Server then enter name of SQL Server instance manually", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
        }

        public bool IsConnectedToInternet()
        {
            //int desc;
            //return InternetGetConnectedState(out desc, 0);
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

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblProgress.Text = "Downloaded " + e.ProgressPercentage.ToString() + "%";
            Application.DoEvents();
            if (e.ProgressPercentage == 100)
            {
                panelDownload.Visible = true;
                string tempPath = Path.GetTempPath() + Application.ProductName;
                Process InstallProcess = new Process();
                InstallProcess.StartInfo.FileName = fileName;
                InstallProcess.StartInfo.Arguments = "File";
                InstallProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                InstallProcess.Start();
                WindowHelper.BringProcessToFront(InstallProcess);
                InstallProcess.WaitForExit();
                btnTestConnection.Enabled = false;
                MessageBox.Show("The System Required Restart." + Environment.NewLine + "Please close all opened application and click 'Ok'", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ProcessStartInfo startinfo = new ProcessStartInfo("shutdown.exe", "-r");
                Process.Start(startinfo);
                IntPtr hMenu = GetSystemMenu(this.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                Cursor = Cursors.Default;
            }
        }
    }

    public static class WindowHelper
    {
        public static void BringProcessToFront(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);
        }

        const int SW_RESTORE = 9;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
    }
}
