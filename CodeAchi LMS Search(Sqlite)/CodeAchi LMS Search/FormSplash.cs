using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_LMS_Search
{
    public partial class FormSplash : Form
    {
        public FormSplash()
        {
            InitializeComponent();
        }

        int reserveLimit = 0;
        string databasePath = "", hostIp = "", connectionString = "", machineId = "";
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            FormSetting databaseSetting = new FormSetting();
            databaseSetting.ShowDialog();
            FormSplash_Load(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            sqltCommnd.CommandText = "select productBlocked,productExpired,reserveSystemLimit,machineId from generalSettings where licenseType!='Demo'";
            sqltCommnd.CommandType = CommandType.Text;
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            bool licenseBlocked = false, licenseExpired = false, productActivate = false;
            reserveLimit = 0;
            string macIds = "";
            if (dataReader.HasRows)
            {
                productActivate = true;
                while (dataReader.Read())
                {
                    licenseBlocked = Convert.ToBoolean(dataReader["productBlocked"].ToString());
                    licenseExpired = Convert.ToBoolean(dataReader["productExpired"].ToString());
                    reserveLimit = Convert.ToInt32(dataReader["reserveSystemLimit"].ToString());
                    macIds = dataReader["machineId"].ToString();
                }
                dataReader.Close();
            }
            else
            {
                MessageBox.Show("Please purchase a license to use this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }

            if (productActivate)
            {
                if (reserveLimit == 0)
                {
                    MessageBox.Show("You have no permission to use this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else
                {
                    if (licenseBlocked)
                    {
                        MessageBox.Show("You can't use this feature due to block your product.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                    else
                    {
                        if (licenseExpired)
                        {
                            MessageBox.Show("Your license has been expired.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Exit();
                        }
                        else
                        {
                            if (macIds == "")
                            {
                                sqltCommnd.CommandText = "update generalSettings set machineId ='" + machineId + "'";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.ExecuteNonQuery();
                            }
                            else
                            {
                                if (macIds.Contains(machineId))
                                {

                                }
                                else
                                {
                                    string[] macList = macIds.Split('$');
                                    if (macList.Count() == reserveLimit)
                                    {
                                        MessageBox.Show("You have cross your mechine limit for this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        Application.Exit();
                                    }
                                    else
                                    {
                                        sqltCommnd.CommandText = "update generalSettings set machineId ='" + macIds + "$" + machineId + "'";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            FormMain mainForm = new FormMain();
            sqltCommnd = sqltConn.CreateCommand();
            string queryString = "select instName,instLogo,instAddress,instShortName,currSymbol," +
                   "databasePath,backupPath,backupHour,printerName,opacAvailable,notificationData,settingsData from generalSettings", settingsData = "";
            sqltCommnd.CommandText = queryString;
            dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    mainForm.lblInstName.Text = dataReader["instName"].ToString();
                    mainForm.lblInstAddress.Text = dataReader["instAddress"].ToString();

                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        memoryStream.Write(imageBytes, 0, imageBytes.Length);
                        mainForm.pcbLogo.Image = Image.FromStream(memoryStream, true);
                    }
                    catch
                    {
                        mainForm.pcbLogo.Image = Properties.Resources.your_logo;
                    }
                    mainForm.pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                dataReader.Close();
            }
            sqltCommnd.CommandText = "select brrId from borrowerDetails";
            sqltCommnd.CommandType = CommandType.Text;
            dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                autoCollData.Clear();
                List<string> idList = (from IDataRecord r in dataReader
                                       select (string)r["brrId"]
                    ).ToList();
                autoCollData.AddRange(idList.ToArray());
                mainForm.txtbBrrId.AutoCompleteMode = AutoCompleteMode.Suggest;
                mainForm.txtbBrrId.AutoCompleteSource = AutoCompleteSource.CustomSource;
                mainForm.txtbBrrId.AutoCompleteCustomSource = autoCollData;
            }
            dataReader.Close();
            sqltConn.Close();
            mainForm.reserveLimit = reserveLimit;
            mainForm.cmbItemCategory.Enabled = false;
            mainForm.cmbSearch.Enabled = false;
            mainForm.txtbSearch.Enabled = false;
            mainForm.txtbBrrId.Select();
            mainForm.connectionString = connectionString;
            this.Hide();
            mainForm.ShowDialog();
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                object o1 = regKey.GetValue("Data1");
                if (o1 != null)
                {
                    byteData = Convert.FromBase64String(o1.ToString());
                    machineId = Encoding.UTF8.GetString(byteData);
                    if (Properties.Settings.Default.machineId == "")
                    {
                        Properties.Settings.Default.machineId = machineId;
                        Properties.Settings.Default.Save();
                    }
                    if (Properties.Settings.Default.machineId == machineId)
                    {
                        o1 = regKey.GetValue("Data2");
                        if (o1 == null)
                        {
                            timer2.Start();
                        }
                        else
                        {
                            byteData = Convert.FromBase64String(o1.ToString());
                            databasePath = Encoding.UTF8.GetString(byteData);
                            o1 = regKey.GetValue("Data3");
                            if (o1 != null)
                            {
                                byteData = Convert.FromBase64String(o1.ToString());
                                hostIp = Encoding.UTF8.GetString(byteData);
                                if (hostIp != "Local")
                                {
                                    var hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                                    databasePath = databasePath.Replace(hostName, hostIp).Replace("\\", "/");
                                }
                            }
                            //var hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                            //databasePath = databasePath.Replace(hostName, hostIp).Replace("\\", "/");
                            connectionString = @"Data Source=" + databasePath + "/LMS.sl3;Version=3;Password=codeachi@lmssl;";
                            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                            try
                            {
                                sqltConn.Open();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                            }
                            if (sqltConn.State.ToString() == "Closed")
                            {
                                timer2.Start();
                            }
                            else
                            {
                                sqltConn.Close();
                                timer1.Start();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Some file replaced by someone.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Exit();
                    }
                }
            }
            else
            {
                MessageBox.Show("Data retriveing fail.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
