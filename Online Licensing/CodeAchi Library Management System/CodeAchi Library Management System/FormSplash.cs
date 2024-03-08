using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
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
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey newregKey = null;
            if (Environment.Is64BitProcess)
            {
                newregKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), false);
                if (newregKey != null)
                {
                    object o1 = newregKey.GetValue("Data1");
                    byteData = Convert.FromBase64String(o1.ToString());
                    globalVarLms.machineId = Encoding.UTF8.GetString(byteData);
                    o1 = newregKey.GetValue("Data2");
                    byteData = Convert.FromBase64String(o1.ToString());
                    Properties.Settings.Default.databasePath = Encoding.UTF8.GetString(byteData);
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                newregKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), false);
                if (newregKey != null)
                {
                    object o1 = newregKey.GetValue("Data1");
                    byteData = Convert.FromBase64String(o1.ToString());
                    globalVarLms.machineId = Encoding.UTF8.GetString(byteData);
                    o1 = newregKey.GetValue("Data2");
                    byteData = Convert.FromBase64String(o1.ToString());
                    Properties.Settings.Default.databasePath = Encoding.UTF8.GetString(byteData);
                    Properties.Settings.Default.Save();
                }
            }

            if (newregKey == null)
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                if (regKey != null)
                {
                    object o1 = regKey.GetValue("Data1");
                    byteData = Convert.FromBase64String(o1.ToString());
                    globalVarLms.machineId = Encoding.UTF8.GetString(byteData);
                    o1 = regKey.GetValue("Data2");
                    byteData = Convert.FromBase64String(o1.ToString());
                    Properties.Settings.Default.databasePath = Encoding.UTF8.GetString(byteData);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    //Change Registry Location===========
                    regKey = Registry.CurrentUser.OpenSubKey(Convert.ToBase64String(byteData), true);
                    if (regKey != null)
                    {
                        object o1 = regKey.GetValue("Data1");
                        byteData = Convert.FromBase64String(o1.ToString());
                        globalVarLms.machineId = Encoding.UTF8.GetString(byteData);
                        o1 = regKey.GetValue("Data2");
                        byteData = Convert.FromBase64String(o1.ToString());
                        Properties.Settings.Default.databasePath = Encoding.UTF8.GetString(byteData);
                        Properties.Settings.Default.Save();

                        compName = Encoding.UTF8.GetBytes("CodeAchi");
                        byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                        regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                        byteData = Encoding.UTF8.GetBytes(globalVarLms.machineId);
                        regKey.SetValue("Data1", Convert.ToBase64String(byteData));
                        byteData = Encoding.UTF8.GetBytes(Properties.Settings.Default.databasePath);
                        regKey.SetValue("Data2", Convert.ToBase64String(byteData));

                        byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                        Registry.CurrentUser.DeleteSubKey(Convert.ToBase64String(byteData));
                    }
                    else
                    {
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\LMS.js"))
                        {
                            compName = Encoding.UTF8.GetBytes("CodeAchi");
                            byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                            regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                            globalVarLms.machineId = BinaryToString(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\LMS.js"));
                            byteData = Encoding.UTF8.GetBytes(globalVarLms.machineId);
                            regKey.SetValue("Data1", Convert.ToBase64String(byteData)); //key
                            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
                            if (File.Exists(databasePath + @"\LMS.config"))
                            {
                                Properties.Settings.Default.databasePath = File.ReadAllText(databasePath + @"\LMS.config");
                                Properties.Settings.Default.Save();
                                byteData = Encoding.UTF8.GetBytes(Properties.Settings.Default.databasePath);
                                regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                                File.Delete(databasePath + @"\LMS.config");
                            }
                            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\LMS.js");
                        }
                        else
                        {
                            MessageBox.Show("Data retrieving fail.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Exit();
                        }
                    }
                }
            }

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
                            lblCompny.Text = "Marketed by " + dataReader["compName"].ToString();
                        }
                        lblCompny.Visible = true;
                    }
                    dataReader.Close();
                }
                sqltConn.Close();
            }
            else
            {
                //mysqlConn = ConnectionClass.mysqlConnection();
                //if(mysqlConn.State==ConnectionState.Closed)
                //{
                //    mysqlConn.Open();
                //}
                //MySqlCommand mysqlCmd;
                //string queryString= "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'marketingDetails'";
                //mysqlCmd =new MySqlCommand()
            }
            DatabaseChecking.CreerBase();
            mainForm.bWorkerGetDetails.RunWorkerAsync();
            timer1.Start();
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
