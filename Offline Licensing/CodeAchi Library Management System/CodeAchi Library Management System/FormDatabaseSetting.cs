using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormDatabaseSetting : Form
    {
        public FormDatabaseSetting()
        {
            InitializeComponent();
        }

        public bool isUpdate = false, migrationComplete = false;
        FormBackup dataBackup = new FormBackup();

        private void FormDatabaseSetting_Load(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Colossal" || globalVarLms.licenseType == "Grand" || globalVarLms.licenseType == "Jumbo")
            {
                rdbMysql.Enabled = true;
            }
            else
            {
                rdbMysql.Enabled = false;
            }
            panelMigration.Visible = false;
            btnMigrate.Enabled = false;
            txtbPassword.UseSystemPasswordChar = true;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                rdbSqlite.Checked = true;
                string databasePath = Properties.Settings.Default.databasePath;
                if (Properties.Settings.Default.hostName != "")
                {
                    string hostName = databasePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    //txtbHostIp.Text = hostName;
                    databasePath = databasePath.Replace(hostName, Properties.Settings.Default.hostName).Replace("\\", "/");
                    txtbDatabasePath.Text = databasePath;
                    chkbIp.Checked = true;
                }
                else
                {
                    chkbIp.Checked = false;
                    txtbHostIp.Enabled = false;
                    txtbDatabasePath.Text = databasePath;
                }

                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select databasePath,backupPath,backupHour from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbDatabasePath.Text = dataReader["databasePath"].ToString();
                    }
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
                rdbMysql.Checked = true;
                string[] dataList = Properties.Settings.Default.databasePath.Split(';');
                txtbHostIp.Text = dataList[0].Replace("server=", "");
                txtbUserName.Text = dataList[2].Replace(" user id=", "");
                txtbPassword.Text = dataList[3].Replace(" password=", "");
                txtbSchema.Text = dataList[1].Replace(" database=", "");
            }
            txtbBackupPath.Text = globalVarLms.backupPath;
            try
            {
                if (globalVarLms.backupHour <= 5)
                {
                    numHour.Value = globalVarLms.backupHour * 60;
                }
                else
                {
                    numHour.Value = globalVarLms.backupHour;
                }
            }
            catch
            {
                numHour.Value = 60;
            }
            btnUpdate.Enabled = false;
        }

        private void FormDatabaseSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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

            if (rdbSqlite.Checked)
            {
                try
                {
                    string databasePath = txtbDatabasePath.Text.TrimEnd();
                    //if (chkbIp.Checked)
                    //{
                    //    hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    //    databasePath = databasePath.Replace(hostName, txtbHostIp.Text.TrimEnd()).Replace("\\", "/");
                    //}

                    byteData = Encoding.UTF8.GetBytes(databasePath);
                    regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                    regKey.Close();

                    Properties.Settings.Default.databasePath = databasePath;
                    Properties.Settings.Default.hostName = "";
                    Properties.Settings.Default.sqliteDatabase = true;
                    Properties.Settings.Default.sqliteConnection = "";
                    Properties.Settings.Default.Save();

                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "update generalSettings set databasePath=:databasePath";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("databasePath", databasePath);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();

                    DatabaseChecking.CreerBase();
                    btnUpdate.Enabled = false;
                    isUpdate = true;
                    MessageBox.Show("Database setting updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {

                }
            }
            else
            {
                string connectionstring = "server=" + txtbHostIp.Text + "; database=" + txtbSchema.Text + "; user id=" + txtbUserName.Text + "; password=" + txtbPassword.Text + "; pooling=false";
                MySqlConnection mysqlConn;
                MySqlCommand mysqlCmd;
                mysqlConn = new MySqlConnection(connectionstring);
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
                byteData = Encoding.UTF8.GetBytes(connectionstring);
                regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                regKey.Close();

                if (Properties.Settings.Default.sqliteDatabase)
                {
                    Properties.Settings.Default.sqliteConnection = "Data Source=" + Properties.Settings.Default.databasePath + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";
                }
                Properties.Settings.Default.databasePath = connectionstring;
                Properties.Settings.Default.sqliteDatabase = false;
                Properties.Settings.Default.Save();

                try
                {
                    string queryString = "update general_settings set databasePath=@databasePath";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@databasePath", connectionstring);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                    DatabaseChecking.CreerBase();
                }
                catch
                {

                }
                btnUpdate.Enabled = false;
                if (globalVarLms.isAdmin)
                {
                    btnMigrate.Enabled = true;
                }
                isUpdate = true;
                MessageBox.Show("Database setting updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            browseFolder.Description = Application.ProductName + " select database path.";
            if (browseFolder.ShowDialog() == DialogResult.OK)
            {
                if (browseFolder.SelectedPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
                {
                    MessageBox.Show("You can't choose this location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (File.Exists(browseFolder.SelectedPath + @"\LMS.sl3"))
                {
                    txtbDatabasePath.Text = browseFolder.SelectedPath;
                }
                else
                {
                    MessageBox.Show("No databse found in the selected location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (rdbSqlite.Checked)
            {
                if (txtbDatabasePath.Text != "")
                {
                    string connectionString = "Data Source=" + txtbDatabasePath.Text + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";  //"DemoLms"
                    //if (chkbIp.Checked)
                    //{
                    //    if (txtbHostIp.Text == "")
                    //    {
                    //        MessageBox.Show("Please enter the server ip.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        txtbHostIp.Select();
                    //        return;
                    //    }
                    //    string databasePath = txtbDatabasePath.Text.TrimEnd();
                    //    GrantAccess(databasePath);
                    //    var hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                    //    databasePath = databasePath.Replace(hostName, txtbHostIp.Text).Replace("\\", "/");
                    //    connectionString = @"Data Source=" + databasePath + "/LMS.sl3;Version=3;Password=codeachi@lmssl;";
                    //}
                    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                    try
                    {
                        sqltConn.Open();
                    }
                    catch
                    {

                    }
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        MessageBox.Show("Connection not open.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Connection opened.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sqltConn.Close();
                        btnUpdate.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("No database path found." + Environment.NewLine + "Please select a database path.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
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
                        MessageBox.Show("Connectio opened.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.NoPropagateInherit,
                AccessControlType.Allow));

            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        private void btnBrowseBackupLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            browseFolder.Description = Application.ProductName + " select backup path.";
            if (browseFolder.ShowDialog() == DialogResult.OK)
            {
                txtbBackupPath.Text = browseFolder.SelectedPath;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (txtbBackupFile.Text != "")
            {
                if (MessageBox.Show("If you restore its replace all data." + Environment.NewLine + "Do you continue ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string backupFileName = "BackupBeforeRestore " + DateTime.Now.Day.ToString("00") + "_" +
                    DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year.ToString("0000") +
                   " " + DateTime.Now.Hour.ToString("00") + "_" + DateTime.Now.Minute.ToString("00") + ".sl3";
                    string fileName = Path.GetFileName(txtbBackupFile.Text);
                    string backupPath = txtbBackupFile.Text.Replace(fileName, "");
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        File.Copy(Properties.Settings.Default.databasePath + @"\LMS.sl3", backupPath + backupFileName);
                        File.Copy(Properties.Settings.Default.databasePath + @"\LMS.sl3", Properties.Settings.Default.databasePath + @"\" + backupFileName);
                        File.Copy(txtbBackupFile.Text, Properties.Settings.Default.databasePath + @"\LMS.sl3", true);
                        DatabaseChecking.CreerBase();
                        MessageBox.Show("Database restore successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        bWorkerBackup.WorkerSupportsCancellation = true;
                        bWorkerBackup.RunWorkerAsync();
                        dataBackup.backupComplete = false;
                        dataBackup.getException = false;
                        dataBackup.Text = "Restoring Database...";
                        dataBackup.ShowDialog();

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
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a backup file.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chkbIp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbIp.Checked)
            {
                if (globalVarLms.licenseType == "Demo")
                {
                    MessageBox.Show("You can't use this feature in trial version.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Properties.Settings.Default.hostName != "")
                {
                    byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
                    byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                    if (regKey != null)
                    {
                        object o1 = regKey.GetValue("Data2");
                        byteData = Convert.FromBase64String(o1.ToString());
                        string databasePath = Encoding.UTF8.GetString(byteData);
                        string hostName = databasePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        txtbHostIp.Text = hostName;
                        databasePath = databasePath.Replace(hostName, Properties.Settings.Default.hostName).Replace("\\", "/");
                        txtbDatabasePath.Text = databasePath;
                        chkbIp.Checked = true;
                    }
                    // string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
                    //databasePath = File.ReadAllText(databasePath + @"\LMS.config");
                }
                txtbHostIp.Enabled = true;
                txtbHostIp.Select();
            }
            else
            {
                txtbHostIp.Clear();
                txtbHostIp.Enabled = false;
            }
        }

        private void btnBrowseBackupFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = Application.ProductName + " Select Backup File";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                selectFile.Filter = "Backup File|*.sl3";
            }
            else
            {
                selectFile.Filter = "Backup File|*.sql";
            }
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                txtbBackupFile.Text = selectFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtbBackupPath.Text == "")
            {
                MessageBox.Show("Please select a backup path.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                GrantAccess(txtbBackupPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "update generalSettings set backupHour='" + numHour.Value + "' ";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.ExecuteNonQuery();
                sqltConn.Close();
                Properties.Settings.Default.backupPath = txtbBackupPath.Text;
                Properties.Settings.Default.Save();
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
                string queryString = "update general_settings set backupHour='" + numHour.Value + "' ";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
                Properties.Settings.Default.backupPath = txtbBackupPath.Text;
                Properties.Settings.Default.Save();
            }

            globalVarLms.backupPath = txtbBackupPath.Text;
            globalVarLms.backupHour = Convert.ToInt32(numHour.Value);
            MessageBox.Show("Backup setting save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtbDatabasePath_TextChanged(object sender, EventArgs e)
        {
            if (txtbDatabasePath.Text == "")
            {
                btnUpdate.Enabled = false;
            }
        }

        private void btnUpdate_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnUpdate.Enabled == true)
            {
                btnUpdate.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnUpdate.BackColor = Color.DimGray;
            }
        }

        private void rdbSqlite_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSqlite.Checked)
            {
                txtbBackupPath.Enabled = true;
                btnBrowsePath.Enabled = true;
                txtbHostIp.Enabled = false;
                txtbUserName.Enabled = false;
                txtbPassword.Enabled = false;
                btnMigrate.Enabled = false;
                txtbSchema.Enabled = false;
            }
        }

        private void rdbMysql_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMysql.Checked)
            {
                txtbBackupPath.Enabled = false;
                btnBrowsePath.Enabled = false;
                txtbHostIp.Enabled = true;
                txtbUserName.Enabled = true;
                txtbPassword.Enabled = true;
                txtbSchema.Enabled = true;
            }
        }

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("When you migrate data, the old data is removed" + Environment.NewLine + "from the database you're about to migrate." + Environment.NewLine + "Do you want to keep going? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                panelMigration.Visible = true;
                Application.DoEvents();
                migrationComplete = false;
                DatabaseMigration();
                panelMigration.Visible = false;
                if (migrationComplete)
                {
                    MessageBox.Show("Database converted successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DatabaseMigration()
        {
            if (Properties.Settings.Default.sqliteConnection != "")
            {
                MySqlConnection mysqlConn;
                MySqlCommand mysqlCmd;

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

                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\lms.sql", Properties.Resources.lms);
                mysqlCmd = new MySqlCommand();
                mysqlCmd.Connection = mysqlConn;
                MySqlBackup mb = new MySqlBackup(mysqlCmd);
                mb.ImportFromFile(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\lms.sql");
                mb.Dispose();
                mysqlConn.Close();
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\lms.sql");
                DatabaseChecking.CreerBase();

                SQLiteConnection sqltConn = new SQLiteConnection(Properties.Settings.Default.sqliteConnection, true);
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCmd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader = null;

                try
                {
                    //======================General Setting====================
                    mysqlConn.Open();
                    sqltCmd.CommandText = "select * from generalSettings";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "General Setting Transaction";
                        string queryString = "insert into general_settings (instName,instShortName,instAddress," +
                            "cityCode,instLogo,instContact,instWebsite,instMail,libraryName,countryName," +
                            "dialCode,currencyName,currShortName,currSymbol,databasePath,backupPath,backupHour," +
                            "printerName,productExpired,licenseType,licenseKey,stepComplete,productBlocked," +
                            "reserveSystemLimit,opacAvailable,machineId,notificationData,settingsData,mngmntMail) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20," +
                           "@p21,@p22,@p23,@p24,@p25,@p26,@p27,@p28,@p29)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p11", dataReader[11].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p12", dataReader[12].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p13", dataReader[13].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p14", dataReader[14].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p15", dataReader[15].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p16", dataReader[16].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p17", dataReader[17].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p18", dataReader[18].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p19", dataReader[19].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p20", dataReader[20].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p21", dataReader[21].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p22", dataReader[22].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p23", dataReader[23].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p24", dataReader[24].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p25", dataReader[25].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p26", dataReader[26].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p27", dataReader[27].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p28", dataReader[28].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p29", dataReader[29].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================User Details====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from userDetails";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "User Details Transaction";
                        string queryString = "insert into user_details (userName,userDesig,userMail," +
                            "userContact,userPassword,userAddress,userPriviledge,isActive,isAdmin,userImage) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Accession Setting====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from accnSetting";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Accession Setting Transaction";
                        string queryString = "insert into accn_setting (isAutoGenerate,isManualPrefix," +
                            "prefixText,joiningChar,lastNumber,noPrefix) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Borrower setting====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from borrowerSettings";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Borrower setting Transaction";
                        string queryString = "insert into borrower_settings (catName,catDesc,catlogAccess," +
                            "capInfo,maxCheckin,maxDue,membershipFees,defaultPass,avoidFine) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Borrower Id Setting====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from brrIdSetting";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Borrower Id Setting Transaction";
                        string queryString = "insert into brrid_setting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values(" +
                           "@p1,@p2,@p3,@p4,@p5)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Country Details====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from countryDetails";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Country Details Transaction";
                        string queryString = "insert into country_details (countryName,currencyName,cuurShort,currSymbol,dialCode) values(" +
                           "@p1,@p2,@p3,@p4,@p5)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Item Setting====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from itemSettings";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Item Setting Transaction";
                        string queryString = "insert into item_settings (catName,catDesc,capInfo,isConsume) values(" +
                           "@p1,@p2,@p3,@p4)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================SubCategory Setting====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from itemSubCategory";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Item Subcategory Transaction";
                        string queryString = "insert into item_subcategory (catName,subCatName," +
                            "shortName,subCatDesc,notReference,issueDays,subCatFine) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================lost damage Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from lostDamage";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Lost/Damage Transaction";
                        string queryString = "insert into lost_damage (itemAccn,itemStatus,statusComment,brrId,entryDate,entryBy) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Membership Setting Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from mbershipSetting";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Membership Setting Transaction";
                        string queryString = "insert into mbership_setting (membrshpName,planFreqncy," +
                            "membrDurtn,membrFees,issueLimit,planDesc) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Notice Template Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from noticeTemplate";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Notice Template Transaction";
                        string queryString = "insert into notice_template (tempName,senderId,htmlBody,bodyText,noticeType) values(" +
                           "@p1,@p2,@p3,@p4,@p5)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Reservation List Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from reservationList";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Reservation List Transaction";
                        string queryString = "insert into reservation_list (brrId,itemAccn,itemTitle," +
                            "itemAuthor,reserveLocation,reserveDate,availableDate) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================User Activity Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from userActivity";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "User Activity Transaction";
                        string queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values(" +
                           "@p1,@p2,@p3,@p4,@p5)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Wish List Entry====================
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from wishList";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Wish List Transaction";
                        string queryString = "insert into wish_list (itemType,itemTitle,itemAuthor,itemPublication,queryDate,queryCount) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    migrationComplete = true;
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Payment Details====================
                    int tempCount = 0;
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from paymentDetails";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Payment History Transaction";
                        string queryString = "insert into payment_details (feesDate,memberId,invId,itemAccn," +
                            "feesDesc,dueAmount,isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount," +
                            "paymentMode,paymentRef) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            if (tempCount == 100)
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                else
                                {
                                    mysqlConn.Close();
                                    mysqlConn.Open();
                                }
                                tempCount = 0;
                            }
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p11", dataReader[11].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p12", dataReader[12].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p13", dataReader[13].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p14", dataReader[14].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            tempCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Borrower Details====================
                    tempCount = 0;
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from borrowerDetails";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Borrower Details Transaction";
                        string queryString = "insert into borrower_details (brrId,brrName,brrCategory,brrAddress," +
                            "brrGender,brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3," +
                            "addInfo4,addInfo5,entryDate,brrImage,opkPermission,renewDate,brrPass,memPlan,memFreq,addMail) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20,@p21)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            if (tempCount == 100)
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                else
                                {
                                    mysqlConn.Close();
                                    mysqlConn.Open();
                                }
                                tempCount = 0;
                            }
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p11", dataReader[11].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p12", dataReader[12].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p13", dataReader[13].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p14", dataReader[14].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p15", dataReader[15].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p16", dataReader[16].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p17", dataReader[17].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p18", dataReader[18].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p19", dataReader[19].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p20", dataReader[20].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p21", dataReader[21].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            tempCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Item Details====================
                    tempCount = 0;
                    sqltConn.Open();
                    mysqlConn.Open();
                    sqltCmd.CommandText = "select * from itemDetails";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblTable.Text = "Item Details Transaction";
                        string queryString = "insert into item_details (itemTitle,itemIsbn,itemAccession," +
                           "itemCat,itemSubCat,itemAuthor,itemClassification,itemSubject,rackNo,totalPages," +
                           "itemPrice,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5,addInfo6,addInfo7," +
                           "addInfo8,entryDate,itemAvailable,isLost,isDamage,itemImage,digitalReference) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18,@p19,@p20," +
                           "@p21,@p22,@p23,@p24,@p25)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            if (tempCount == 100)
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                else
                                {
                                    mysqlConn.Close();
                                    mysqlConn.Open();
                                }
                                tempCount = 0;
                            }
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p11", dataReader[11].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p12", dataReader[12].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p13", dataReader[13].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p14", dataReader[14].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p15", dataReader[15].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p16", dataReader[16].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p17", dataReader[17].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p18", dataReader[18].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p19", dataReader[19].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p20", dataReader[20].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p21", dataReader[21].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p22", dataReader[22].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p23", dataReader[23].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p24", dataReader[24].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p25", dataReader[25].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            tempCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();

                    //======================Issued Item====================
                    tempCount = 0;
                    mysqlConn.Open();
                    sqltConn.Open();
                    sqltCmd.CommandText = "select * from issuedItem";
                    dataReader = sqltCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        lblCount.Text = 0.ToString();
                        lblTable.Text = "Issue History Transaction";
                        string queryString = "insert into issued_item (brrId,itemAccession,issueDate,reissuedDate," +
                            "expectedReturnDate,returnDate,itemReturned,issuedBy,issueComment,reissuedBy," +
                            "reissuedComment,returnedBy,returnedComment) values(" +
                           "@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13)";
                        progressBar1.Visible = true;
                        int itemCount = 0;
                        while (dataReader.Read())
                        {
                            if (tempCount == 100)
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                else
                                {
                                    mysqlConn.Close();
                                    mysqlConn.Open();
                                }
                                tempCount = 0;
                            }
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@p1", dataReader[1].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p2", dataReader[2].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p3", dataReader[3].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p4", dataReader[4].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p5", dataReader[5].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p6", dataReader[6].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p7", dataReader[7].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p8", dataReader[8].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p9", dataReader[9].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p10", dataReader[10].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p11", dataReader[11].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p12", dataReader[12].ToString());
                            mysqlCmd.Parameters.AddWithValue("@p13", dataReader[13].ToString());
                            mysqlCmd.ExecuteNonQuery();
                            itemCount++;
                            tempCount++;
                            Application.DoEvents();
                            lblCount.Text = itemCount.ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    mysqlConn.Close();


                }
                catch (Exception ex)
                {
                    sqltConn.Close();
                    mysqlConn.Close();
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                DatabaseChecking.CreerBase();
            }
        }

        private void bWorkerBackup_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string backupFileName = "BackupBeforeRestore " + DateTime.Now.Day.ToString("00") + "_" +
                   DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year.ToString("0000") +
                  " " + DateTime.Now.Hour.ToString("00") + "_" + DateTime.Now.Minute.ToString("00") + ".sl3";
                string fileName = Path.GetFileName(txtbBackupFile.Text);
                string backupPath = txtbBackupFile.Text.Replace(fileName, "");
                MySqlConnection mysqlConn;
                MySqlCommand mysqlCmd;

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
                //==================backup==============
                mysqlCmd = new MySqlCommand();
                mysqlCmd.Connection = mysqlConn;
                MySqlBackup mb = new MySqlBackup(mysqlCmd);
                backupFileName = backupPath + backupFileName.Replace("sl3", "sql");
                mb.ExportToFile(backupFileName);
                mb.Dispose();
                mysqlConn.Close();

                //======================restore====================
                mysqlConn.Open();
                mysqlCmd.Connection = mysqlConn;
                mysqlCmd.CommandTimeout = 99999;
                mb = new MySqlBackup(mysqlCmd);
                mb.ImportFromFile(txtbBackupFile.Text);
                mb.Dispose();
                mysqlConn.Close();
                dataBackup.getException = true;
                MessageBox.Show("Database restore successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                DatabaseChecking.CreerBase();
            }
            catch (Exception ex)
            {
                dataBackup.getException = true;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
