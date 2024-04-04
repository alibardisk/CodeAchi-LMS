using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormSplash : Form
    {
        public FormSplash()
        {
            InitializeComponent();
        }

        FormDashBoard mainForm = new FormDashBoard();
        PasswordHasher passwordHasher = new PasswordHasher();

        private void FormSplash_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.callSettingsUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                FieldSettings.Default.Upgrade();
                Properties.Settings.Default.callSettingsUpgrade = false;
            }

            if (File.Exists(Application.StartupPath + "/Updater1.exe"))
            {
                if (File.Exists(Application.StartupPath + "/Updater.exe"))
                {
                    File.Delete(Application.StartupPath + "/Updater.exe");
                }
                File.Move(Application.StartupPath + "/Updater1.exe", Application.StartupPath + "/Updater.exe");
            }
            string configFilePath = Application.StartupPath + "/clms.json";
            if (File.Exists(configFilePath))
            {
                string jsonString = passwordHasher.Decrypt(File.ReadAllText(configFilePath));
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                globalVarLms.connectionString = jsonObject.ConnectionString;
                globalVarLms.machineId = jsonObject.InstallationId;
                globalVarLms.sqliteData = jsonObject.SQLiteData;

                string ttlUser = "0";
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count(id) from userDetails;";
                    sqltCommnd.CommandType = CommandType.Text;
                    ttlUser = sqltCommnd.ExecuteScalar().ToString();
                   
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
                                lblCompny.Text = "Marketed by " + dataReader["compName"].ToString();
                            }
                            lblCompny.Visible = true;
                        }
                        dataReader.Close();
                    }
                    sqltConn.Close();
                }
                //else
                //{
                //    //mysqlConn = ConnectionClass.mysqlConnection();
                //    //if(mysqlConn.State==ConnectionState.Closed)
                //    //{
                //    //    mysqlConn.Open();
                //    //}
                //    //MySqlCommand mysqlCmd;
                //    //string queryString= "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'marketingDetails'";
                //    //mysqlCmd =new MySqlCommand()
                //}
                if (Convert.ToInt32(ttlUser) > 1)
                {
                    DatabaseChecking.CreerBase();
                    mainForm.bWorkerGetDetails.RunWorkerAsync();
                    timer1.Start();
                }
                else
                {
                    FormWizard wizardForm = new FormWizard();
                    wizardForm.ShowDialog();
                }
            }
            else
            {
                FormWizard wizardForm = new FormWizard();
                wizardForm.ShowDialog();
            }
        }

        public static string BinaryToString(string data)
        {
            try
            {
                List<Byte> byteList = new List<Byte>();

                for (int i = 0; i < data.Length; i += 8)
                {
                    byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
                }
                return Encoding.ASCII.GetString(byteList.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return "";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            globalVarLms.issueReciept = Properties.Settings.Default.issueReciept;
            globalVarLms.reissueReciept = Properties.Settings.Default.reissueReciept;
            globalVarLms.returnReciept = Properties.Settings.Default.returnReciept;
            globalVarLms.paymentReciept = Properties.Settings.Default.paymentReciept;
            globalVarLms.recieptPrinter = Properties.Settings.Default.recieptPrinter;
            globalVarLms.recieptPaper = Properties.Settings.Default.recieptPaper;
            this.Hide();
            mainForm.timer1.Start();
            mainForm.blankChart();
            mainForm.ShowDialog();
        }

        private void lblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            timer1.Stop();
            FormAbout aboutForm = new FormAbout();
            aboutForm.ShowDialog();
            timer1.Start();
        }
    }
}
