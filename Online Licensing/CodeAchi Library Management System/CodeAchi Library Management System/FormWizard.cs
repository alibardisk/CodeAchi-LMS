using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormWizard : Form
    {
        public FormWizard()
        {
            InitializeComponent();
        }

        String userImage = "base64String";
        string userType = "";
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormWizard_Load(object sender, EventArgs e)
        {
            lblCompany1.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
            if (File.Exists(databasePath + @"\LMS.config"))
            {
                txtbDatabasePath.Text = File.ReadAllText(databasePath + @"\LMS.config");
            }
            else
            {
                txtbDatabasePath.Text = Properties.Settings.Default.databasePath;
            }
            btnPrev.Visible = false;
            this.Text = Application.ProductName + " Setup Wizard";
            lblStep.Text = "Library Setup Wizard (Step 1)";
            lblStepCount.Text = 1.ToString();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        sqltConn.Open();
                    }
                    catch
                    {
                        string connectionString = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";
                        sqltConn = new SQLiteConnection(connectionString);
                        sqltConn.Open();
                    }
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select [countryName] from countryDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    List<string> countryList = (from IDataRecord r in dataReader select (string)r["countryName"]).ToList();
                    autoCollData.AddRange(countryList.ToArray());
                }
                dataReader.Close();
            }

            txtbOrgCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbOrgCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbOrgCountry.AutoCompleteCustomSource = autoCollData;

            txtbUserCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbUserCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbUserCountry.AutoCompleteCustomSource = autoCollData;
        }

        public static bool CheckForInternetConnection()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }

        public bool IsConnectedToInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("https://codeachi.com/"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            string queryString = "";

            string macAddress = globalVarLms.machineId;
            if (lblStepCount.Text == 1.ToString())
            {
                if (rdbPersonal.Checked)
                {
                    txtbUserName.TabIndex = 0;
                    txtbUserDesignation.TabIndex = 1;
                    txtbUserMail.TabIndex = 2;
                    txtbContact.TabIndex = 3;
                    txtbAddress.TabIndex = 4;
                    txtbUserCountry.TabIndex = 5;
                    btnUpload.TabIndex = 6;
                    btnNext.TabIndex = 7;

                    pnlStep2.Visible = false;
                    txtbUserName.Select();
                    userType = "Personal";
                }
                else
                {
                    txtbFullName.TabIndex = 0;
                    txtbShortName.TabIndex = 1;
                    txtbOrgAddress.TabIndex = 2;
                    txtbOrgMail.TabIndex = 3;
                    TxtbWebsite.TabIndex = 4;
                    txtbOrgContact.TabIndex = 5;
                    txtbOrgCountry.TabIndex = 6;
                    btnUpload.TabIndex = 7;
                    btnNext.TabIndex = 8;

                    pnlStep2.Visible = true;
                    txtbFullName.Select();
                    userType = "Official";
                }
                sqltCommnd.CommandText = "select stepComplete from generalSettings";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                int stepComplete = 0;
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        stepComplete = Convert.ToInt32(dataReader["stepComplete"].ToString());
                    }
                }
                dataReader.Close();
                if (stepComplete == 5 || stepComplete == 4 || stepComplete == 3)
                {
                    sqltCommnd.CommandText = "select * from userDetails where userMail!=@userMail and isAdmin='" + true + "' limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbUserName.Text = dataReader["userName"].ToString();
                            txtbUserDesignation.Text = dataReader["userDesig"].ToString();
                            txtbUserMail.Text = dataReader["userMail"].ToString();
                            txtbContact.Text = dataReader["userContact"].ToString();
                            txtbAddress.Text = dataReader["userAddress"].ToString();
                            txtbPassword.Text = dataReader["userPassword"].ToString();
                            txtbPass1.Text = dataReader["userPassword"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["userImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbUser.Image = Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbUser.Image = Properties.Resources.blankBrrImage;
                            }
                        }
                    }
                    dataReader.Close();
                    sqltCommnd.CommandText = "select countryName from generalSettings";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbUserCountry.Text = dataReader["countryName"].ToString();
                        }
                    }
                    dataReader.Close();
                    while (pnlStep2.Location.X > -642)
                    {
                        pnlStep2.Location = new Point(pnlStep2.Location.X - 5, pnlStep2.Location.Y);
                    }
                    while (pnlStp1.Location.X > -642)
                    {
                        pnlStp1.Location = new Point(pnlStp1.Location.X - 5, pnlStp1.Location.Y);
                    }
                    txtbUserName.TabIndex = 0;
                    txtbUserDesignation.TabIndex = 1;
                    txtbUserMail.TabIndex = 2;
                    txtbContact.TabIndex = 3;
                    txtbAddress.TabIndex = 5;
                    txtbUserCountry.TabIndex = 4;
                    btnUpload.TabIndex = 6;
                    btnNext.TabIndex = 7;

                    btnPrev.Visible = true;
                    lblStep.Text = "Library Setup Wizard (Step 3)";
                    lblStepCount.Text = 3.ToString();
                    //txtbUserCountry.Select();
                }
                else if (stepComplete == 2)
                {
                    while (pnlStep2.Location.X > -642)
                    {
                        pnlStep2.Location = new Point(pnlStep2.Location.X - 5, pnlStep2.Location.Y);
                    }
                    while (pnlStp1.Location.X > -642)
                    {
                        pnlStp1.Location = new Point(pnlStp1.Location.X - 5, pnlStp1.Location.Y);
                    }
                    txtbUserName.TabIndex = 0;
                    txtbUserDesignation.TabIndex = 1;
                    txtbUserMail.TabIndex = 2;
                    txtbContact.TabIndex = 3;
                    txtbAddress.TabIndex = 4;
                    txtbUserCountry.TabIndex = 5;
                    btnUpload.TabIndex = 6;
                    btnNext.TabIndex = 7;

                    btnPrev.Visible = true;
                    lblStep.Text = "Library Setup Wizard (Step 3)";
                    lblStepCount.Text = 3.ToString();
                    txtbUserName.Select();
                }
                else
                {
                    sqltCommnd.CommandText = "insert into generalSettings (stepComplete) values('" + 1 + "')";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.ExecuteNonQuery();
                    while (pnlStp1.Location.X > -642)
                    {
                        pnlStp1.Location = new Point(pnlStp1.Location.X - 5, pnlStp1.Location.Y);
                    }
                    btnPrev.Visible = true;
                    lblStep.Text = "Library Setup Wizard (Step 2)";
                    lblStepCount.Text = 2.ToString();
                }
            }
            else if (lblStepCount.Text == 2.ToString())
            {
                if (txtbFullName.Text == "")
                {
                    MessageBox.Show("Please enter your organization name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbFullName.Select();
                    return;
                }
                if (txtbShortName.Text == "")
                {
                    MessageBox.Show("Please enter your organization short name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbShortName.Select();
                    return;
                }
                if (txtbOrgMail.Text == "")
                {
                    MessageBox.Show("Please enter organization email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbOrgMail.Select();
                    return;
                }
                if (txtbOrgCountry.Text == "")
                {
                    MessageBox.Show("Please enter organization country name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbOrgCountry.Select();
                    return;
                }

                if (IsConnectedToInternet())
                {
                    try
                    {
                        string installedDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        WebRequest webRequest = WebRequest.Create(globalVarLms.dateApi);
                        webRequest.Timeout = 8000;
                        WebResponse webResponse = webRequest.GetResponse();
                        Stream dataStream = webResponse.GetResponseStream();
                        StreamReader strmReader = new StreamReader(dataStream);
                        string requestResult = strmReader.ReadLine();
                        if (requestResult != "")
                        {
                            installedDate = requestResult;
                        }
                        string installTime = DateTime.Now.ToString("hh:mm:ss tt");
                        string queryToCheck = "SELECT isBlocked,licenseKey,installDate FROM installationDetails WHERE mac = '" + macAddress + "' and productName='" + Application.ProductName + "'";
                        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                        webRequest.Timeout = 8000;
                        webResponse = webRequest.GetResponse();
                        dataStream = webResponse.GetResponseStream();
                        strmReader = new StreamReader(dataStream);
                        requestResult = strmReader.ReadLine();
                        if (requestResult == null)
                        {
                            string queryToInsert = "INSERT INTO installationDetails (mac,productName,isBlocked,licenseKey,installDate,installTime,useFor,org_name,email,country,address,website,orgContact)" +
                                " VALUES('" + macAddress + "', '" + Application.ProductName + "','" + false + "','" + "Demo" + "','" + installedDate + "','" + installTime + "','" + userType + "'," +
                                "'" + txtbFullName.Text + "','" + txtbOrgMail.Text + "','" + txtbOrgCountry.Text + "','" + txtbOrgAddress.Text + "','" + TxtbWebsite.Text + "','" + txtbOrgContact.Text + "')";

                            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            webRequest = WebRequest.Create(globalVarLms.insertApi + queryToInsert);
                            webRequest.Timeout = 8000;
                            webResponse = webRequest.GetResponse();
                        }
                        else
                        {

                            string queryToUpdate = "UPDATE installationDetails set useFor='" + userType + "',org_name='" + txtbFullName.Text + "'" +
                            ",email='" + txtbOrgMail.Text + "',country='" + txtbOrgCountry.Text + "',address='" + txtbOrgAddress.Text + "'," +
                            "installTime='" + installTime + "',website='" + TxtbWebsite.Text + "',orgContact='" + txtbOrgContact.Text + "' WHERE mac = '" + macAddress + "' and productName='" + Application.ProductName + "'";
                            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                            webRequest.Timeout = 8000;
                            webResponse = webRequest.GetResponse();
                            dataStream = webResponse.GetResponseStream();
                            strmReader = new StreamReader(dataStream);
                            requestResult = strmReader.ReadLine();
                        }
                        if (requestResult == "Updated" || requestResult == "Inserted")
                        {
                            String base64String = "base64String";
                            if (pcbLogo.Image != Properties.Resources.NoImageAvailable || pcbLogo.Image != Properties.Resources.uploadingFail ||
                                pcbLogo.Image != Properties.Resources.your_logo)
                            {
                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    pcbLogo.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] imageBytes = memoryStream.ToArray();
                                    base64String = Convert.ToBase64String(imageBytes);
                                }
                            }
                            queryString = "update generalSettings set instName=:instName,instShortName=:instShortName,instAddress=:instAddress" +
                                    ",instLogo='" + base64String + "',instContact='" + txtbOrgContact.Text + "',instWebsite" +
                                    "=:instWebsite,instMail=:instMail,countryName=:countryName,stepComplete='" + lblStepCount.Text + "'" +
                                    ",backupPath=:backupPath,backupHour='" + 60 + "',notificationData=:notificationData,settingsData=:settingsData";

                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("instName", txtbFullName.Text);
                            sqltCommnd.Parameters.AddWithValue("instShortName", txtbShortName.Text);
                            sqltCommnd.Parameters.AddWithValue("instAddress", txtbOrgAddress.Text.Replace(Environment.NewLine, ""));
                            sqltCommnd.Parameters.AddWithValue("instWebsite", TxtbWebsite.Text);
                            sqltCommnd.Parameters.AddWithValue("instMail", txtbOrgMail.Text);
                            sqltCommnd.Parameters.AddWithValue("countryName", txtbOrgCountry.Text);
                            sqltCommnd.Parameters.AddWithValue("backupPath", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName);
                            sqltCommnd.Parameters.AddWithValue("notificationData", getNotificationData());
                            sqltCommnd.Parameters.AddWithValue("settingsData", getSettingsData());
                            sqltCommnd.ExecuteNonQuery();
                        }
                        while (pnlStep2.Location.X > -642)
                        {
                            pnlStep2.Location = new Point(pnlStep2.Location.X - 5, pnlStep2.Location.Y);
                        }
                        txtbUserName.TabIndex = 0;
                        txtbUserDesignation.TabIndex = 1;
                        txtbUserMail.TabIndex = 2;
                        txtbContact.TabIndex = 3;
                        txtbAddress.TabIndex = 4;
                        txtbUserCountry.TabIndex = 5;
                        btnUpload.TabIndex = 6;
                        btnNext.TabIndex = 7;

                        btnPrev.Visible = true;
                        lblStep.Text = "Library Setup Wizard (Step 3)";
                        lblStepCount.Text = 3.ToString();
                        //    //    txtbUserName.Select();
                    }
                    catch
                    {
                        MessageBox.Show("Unable to connect please try again.", Application.ProductName, MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK);
                }
            }
            else if (lblStepCount.Text == 3.ToString())
            {
                if (txtbUserName.Text == "")
                {
                    MessageBox.Show("Please enter your name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbUserName.Select();
                    return;
                }
                if (txtbUserDesignation.Text == "")
                {
                    MessageBox.Show("Please enter your designation.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbUserDesignation.Select();
                    return;
                }
                if (txtbUserMail.Text == "")
                {
                    MessageBox.Show("Please enter your email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbUserMail.Select();
                    return;
                }
                if (txtbContact.Text == "")
                {
                    MessageBox.Show("Please enter your contact no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbContact.Select();
                    return;
                }
                if (txtbUserCountry.Text == "")
                {
                    MessageBox.Show("Please enter your country name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbUserCountry.Select();
                    return;
                }
                if (txtbAddress.Text == "")
                {
                    MessageBox.Show("Please enter your address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbAddress.Select();
                    return;
                }
                if (IsConnectedToInternet())
                {
                    try
                    {
                        string queryToUpdate = "UPDATE installationDetails set cust_name='" + txtbUserName.Text + "',contact='" + txtbContact.Text + "',userMail='" + txtbUserMail.Text + "' WHERE mac = '" + macAddress + "' and productName='" + Application.ProductName + "'";
                        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        WebRequest webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                        webRequest.Timeout = 8000;
                        WebResponse webResponse = webRequest.GetResponse();
                        Stream dataStream = webResponse.GetResponseStream();
                        StreamReader strmReader = new StreamReader(dataStream);
                        string requestResult = strmReader.ReadLine();
                        if (requestResult == "Updated")
                        {
                            if (pcbUser.Image != Properties.Resources.blankBrrImage && pcbUser.Image != Properties.Resources.uploadingFail)
                            {
                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    pcbUser.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] imageBytes = memoryStream.ToArray();
                                    userImage = Convert.ToBase64String(imageBytes);
                                }
                            }
                            globalVarLms.userContact = txtbContact.Text;
                            queryString = "select userMail from userDetails where userMail=@userMail";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@userMail", txtbUserMail.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                dataReader.Close();
                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "update userDetails set userName=:userName,userDesig=:userDesig," +
                                    "userContact='" + txtbContact.Text + "',userAddress=:userAddress,userPriviledge" +
                                    "='" + "EI$EM$IR$RP" + "',isActive='" + true + "',isAdmin='" + true + "',userImage='" + userImage + "'";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("userName", txtbUserName.Text);
                                sqltCommnd.Parameters.AddWithValue("userDesig", txtbUserDesignation.Text);
                                sqltCommnd.Parameters.AddWithValue("@userMail", txtbUserMail.Text);
                                sqltCommnd.Parameters.AddWithValue("userAddress", txtbAddress.Text);
                                sqltCommnd.ExecuteNonQuery();
                            }
                            else
                            {
                                dataReader.Close();
                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "Insert into userDetails (userName,userDesig,userMail,userContact,userAddress,userPriviledge,isActive,isAdmin,userImage)" +
                                " values (@userName,@userDesig,@userMail,'" + txtbContact.Text + "',@userAddress,'" + "EI$EM$IR$RP" + "','" + true + "','" + true + "','" + userImage + "');";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("@userName", txtbUserName.Text);
                                sqltCommnd.Parameters.AddWithValue("@userDesig", txtbUserDesignation.Text);
                                sqltCommnd.Parameters.AddWithValue("@userMail", txtbUserMail.Text);
                                sqltCommnd.Parameters.AddWithValue("@userAddress", txtbAddress.Text);
                                sqltCommnd.ExecuteNonQuery();
                            }

                            queryString = "update generalSettings set stepComplete='" + lblStepCount.Text + "'";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.ExecuteNonQuery();

                            txtbName.Text = txtbUserName.Text;
                            txtbDesignation.Text = txtbUserDesignation.Text;
                            txtbMailId.Text = txtbUserMail.Text;
                            txtbCountry.Text = txtbUserCountry.Text;
                            while (pnlStep3.Location.X > -642)
                            {
                                pnlStep3.Location = new Point(pnlStep3.Location.X - 5, pnlStep3.Location.Y);
                            }
                            btnPrev.Visible = true;
                            lblStep.Text = "Library Setup Wizard (Step 4)";
                            lblStepCount.Text = 4.ToString();

                            queryString = "select currencyName,cuurShort,currSymbol from countryDetails where countryName=@countryName";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                cmbCurrName.Items.Clear();
                                cmbCurrShort.Items.Clear();
                                cmbCurrSymbol.Items.Clear();
                                while (dataReader.Read())
                                {
                                    string[] currnList = dataReader["currencyName"].ToString().Split(',');
                                    foreach (string curName in currnList)
                                    {
                                        cmbCurrName.Items.Add(curName);
                                    }
                                    cmbCurrName.SelectedIndex = 0;
                                    currnList = dataReader["cuurShort"].ToString().Split(',');
                                    foreach (string curName in currnList)
                                    {
                                        cmbCurrShort.Items.Add(curName);
                                    }
                                    cmbCurrShort.SelectedIndex = 0;
                                    currnList = dataReader["currSymbol"].ToString().Split(',');
                                    foreach (string curName in currnList)
                                    {
                                        cmbCurrSymbol.Items.Add(curName);
                                    }
                                    cmbCurrSymbol.SelectedIndex = 0;
                                }
                                dataReader.Close();
                            }
                            else
                            {
                                dataReader.Close();
                                queryString = "select currencyName,cuurShort,currSymbol from countryDetails";
                                sqltCommnd.CommandText = queryString;
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    cmbCurrName.Items.Clear();
                                    cmbCurrShort.Items.Clear();
                                    cmbCurrSymbol.Items.Clear();
                                    while (dataReader.Read())
                                    {
                                        string[] currnList = dataReader["currencyName"].ToString().Split(',');
                                        foreach (string curName in currnList)
                                        {
                                            cmbCurrName.Items.Add(curName.TrimStart());
                                        }
                                    }
                                    dataReader.Close();
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unable to connect please try again.", Application.ProductName, MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK);
                }
            }
            else if (lblStepCount.Text == 4.ToString())
            {
                if (cmbCurrName.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select currency name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbCurrShort.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select currency shortName.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbCurrSymbol.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select currency symbol.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "update generalSettings set currencyName=:currencyName,currShortName=:currShortName,currSymbol=:currSymbol,stepComplete='" + lblStepCount.Text + "'";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("currencyName", cmbCurrName.Text);
                sqltCommnd.Parameters.AddWithValue("currShortName", cmbCurrShort.Text);
                sqltCommnd.Parameters.AddWithValue("currSymbol", cmbCurrSymbol.Text);
                sqltCommnd.ExecuteNonQuery();

                while (pnlStep4.Location.X > -642)
                {
                    pnlStep4.Location = new Point(pnlStep4.Location.X - 5, pnlStep4.Location.Y);
                }
                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 5)";
                lblStepCount.Text = 5.ToString();
                txtbPassword.Select();
            }
            else if (lblStepCount.Text == 5.ToString())
            {
                if (txtbPassword.Text == "")
                {
                    MessageBox.Show("Please enter a password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbPass1.Text == "")
                {
                    MessageBox.Show("Please confirm your password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbPass1.Text != txtbPassword.Text)
                {
                    MessageBox.Show("Passwor mot match.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                queryString = "update userDetails set userPassword=:userPassword where userMail=@userMail";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                sqltCommnd.Parameters.AddWithValue("userPassword", txtbPassword.Text);
                sqltCommnd.ExecuteNonQuery();

                queryString = "update generalSettings set stepComplete='" + lblStepCount.Text + "'";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.ExecuteNonQuery();

                globalVarLms.currentUserName = txtbUserName.Text;
                Properties.Settings.Default.currentUserId = txtbMailId.Text;
                globalVarLms.currentUserId = txtbMailId.Text;
                globalVarLms.currentPassword = txtbPassword.Text;
                Properties.Settings.Default.Save();
                Application.Exit();
            }
        }

        private string getNotificationData()
        {
            string notificationData = "";
            if (Properties.Settings.Default.notificationData == "")
            {
                notificationData = "{\"IssueMail\" : \"" + false + "\"," +
                                  "\"IssueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"IssueSms\" : \"" + false + "\"," +
                                  "\"IssueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReissueMail\" : \"" + false + "\"," +
                                  "\"ReissueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReissueSms\" : \"" + false + "\"," +
                                  "\"ReissueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReturnMail\" : \"" + false + "\"," +
                                  "\"ReturnMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReturnSms\" : \"" + false + "\"," +
                                  "\"ReturnSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReservedMail\" : \"" + false + "\"," +
                                  "\"ReservedMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReservedSms\" : \"" + false + "\"," +
                                  "\"ReservedSmsTemplate\" : \"" + "Template" + "\"," +
                                   "\"ArrivedMail\" : \"" + false + "\"," +
                                  "\"ArrivedMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ArrivedSms\" : \"" + false + "\"," +
                                  "\"ArrivedSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueMail\" : \"" + false + "\"," +
                                  "\"DueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueSms\" : \"" + false + "\"," +
                                  "\"DueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueNotificationDate\" : \"" + "Date" + "\"," +
                                  "\"DueNotificationCondition\" : \"" + "Condition" + "\"" + "}";
            }
            else
            {
                notificationData = Properties.Settings.Default.notificationData;
            }
            return notificationData;
        }

        private string getSettingsData()
        {
            string settingsData = "{\"licenseType\" : \"" + Properties.Settings.Default.licenseType + "\"," +
                                   "\"serialKey\" : \"" + Properties.Settings.Default.serialKey + "\"," +
                                   "\"productBlocked\" : \"" + Properties.Settings.Default.productBlocked + "\"," +
                                   "\"expiryDate\" : \"" + Properties.Settings.Default.expiryDate + "\"," +
                                   "\"lastChecked\" : \"" + Properties.Settings.Default.lastChecked + "\"," +
                                   "\"machineLimits\" : \"" + Properties.Settings.Default.machineLimits + "\"," +
                                   "\"itemLimits\" : \"" + Properties.Settings.Default.itemLimits + "\"," +
                                   "\"mailType\" : \"" + Properties.Settings.Default.mailType + "\"," +
                                   "\"mailId\" : \"" + Properties.Settings.Default.mailId + "\"," +
                                   "\"mailPassword\" : \"" + Properties.Settings.Default.mailPassword + "\"," +
                                   "\"mailHost\" : \"" + Properties.Settings.Default.mailHost + "\"," +
                                   "\"mailPort\" : \"" + Properties.Settings.Default.mailPort + "\"," +
                                   "\"mailSsl\" : \"" + Properties.Settings.Default.mailSsl + "\"," +
                                   "\"smsApi\" : \"" + Properties.Settings.Default.smsApi + "\"," +
                                    "\"blockedMail\" : \"" + Properties.Settings.Default.blockedMail + "\"," +
                                   "\"blockedContact\" : \"" + Properties.Settings.Default.blockedContact + "\"," +
                                   "\"reserveDay\" : \"" + Properties.Settings.Default.reserveDay + "\"," +
                                   "\"hostName\" : \"" + Properties.Settings.Default.hostName + "\"," +
                                   "\"databaseSeries\" : \"" + Properties.Settings.Default.databaseSeries + "\"" + "}";
            return settingsData;
        }

        private void txtbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void txtbUserCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.blankBrrImage;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbUser.Image = globalVarLms.bimapImage;
        }

        private void txtbOrgContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbOrgCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.NoImageAvailable;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbLogo.Image = globalVarLms.bimapImage;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblNotification1.Visible = !lblNotification1.Visible;
        }

        private void FormWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to close the application ? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    /* Cancel the Closing event from closing the form. */
                    e.Cancel = true;
                }
                else
                {
                    /* Closing the form. */
                    e.Cancel = false;
                    Application.Exit();
                }
            }
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void txtbPass1_TextChanged(object sender, EventArgs e)
        {
            txtbPass1.UseSystemPasswordChar = true;
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fileDialog = new FolderBrowserDialog();
            fileDialog.Description = Application.ProductName + " select database path.";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
                txtbDatabasePath.Text = fileDialog.SelectedPath;
                if (databasePath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
                {
                    MessageBox.Show("You can't choose this location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (File.Exists(txtbDatabasePath.Text + @"\LMS.sl3"))
                {

                }
                else
                {
                    File.Copy(databasePath + @"\LMS.sl3", txtbDatabasePath.Text + @"\LMS.sl3");
                }
                Properties.Settings.Default.databasePath = txtbDatabasePath.Text;
                Properties.Settings.Default.Save();
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
                byteData = Encoding.UTF8.GetBytes(Properties.Settings.Default.databasePath);
                regKey.SetValue("Data2", Convert.ToBase64String(byteData)); //key
                regKey.Close();
                GrantAccess(txtbDatabasePath.Text);
                Application.DoEvents();
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

        private void txtbAddress_Leave(object sender, EventArgs e)
        {
            txtbUserCountry.Select();
        }

        private void txtbPassword_Leave(object sender, EventArgs e)
        {
            txtbPass1.Select();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (lblStepCount.Text == 2.ToString())
            {
                while (pnlStp1.Location.X <= 12)
                {
                    pnlStp1.Location = new Point(pnlStp1.Location.X + 5, pnlStp1.Location.Y);
                    Application.DoEvents();
                }
                btnPrev.Visible = false;
                lblStep.Text = "Library Setup Wizard (Step 1)";
                lblStepCount.Text = 1.ToString();
                pnlStp1.Location = new Point(12, pnlStp1.Location.Y);
            }
            else if (lblStepCount.Text == 3.ToString())
            {
                while (pnlStep2.Location.X <= 12)
                {
                    pnlStep2.Location = new Point(pnlStep2.Location.X + 5, pnlStep2.Location.Y);
                    Application.DoEvents();
                }
                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 2)";
                lblStepCount.Text = 2.ToString();
                pnlStep2.Location = new Point(10, pnlStep2.Location.Y);
            }
            else if (lblStepCount.Text == 4.ToString())
            {
                while (pnlStep3.Location.X <= 12)
                {
                    pnlStep3.Location = new Point(pnlStep3.Location.X + 5, pnlStep3.Location.Y);
                    Application.DoEvents();
                }

                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 3)";
                lblStepCount.Text = 3.ToString();
                pnlStep3.Location = new Point(10, pnlStep2.Location.Y);
            }
            else if (lblStepCount.Text == 5.ToString())
            {
                while (pnlStep4.Location.X <= 12)
                {
                    pnlStep4.Location = new Point(pnlStep4.Location.X + 5, pnlStep4.Location.Y);
                    Application.DoEvents();
                }
                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 4)";
                lblStepCount.Text = 4.ToString();
                pnlStep4.Location = new Point(10, pnlStep2.Location.Y);
            }
        }

        private void txtbUserMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void lblTutorial_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This data will be shown in various important places" + Environment.NewLine + "like Dashboard, Report, Invoice etc.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This will be the Admin account." + Environment.NewLine + "Admin is the super user for the system.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This will set your default currency." + Environment.NewLine + "Default currency will be used for fine or payment collection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void cmbCurrName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCurrName.Text != "")
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select currencyName,cuurShort,currSymbol from countryDetails where currencyName like @currencyName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@currencyName", "%" + cmbCurrName.Text + "%");
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    cmbCurrShort.Items.Clear();
                    cmbCurrSymbol.Items.Clear();
                    while (dataReader.Read())
                    {
                        string[] currnList = dataReader["cuurShort"].ToString().Split(',');
                        foreach (string curName in currnList)
                        {
                            cmbCurrShort.Items.Add(curName.TrimStart());
                        }
                        cmbCurrShort.SelectedIndex = 0;
                        currnList = dataReader["currSymbol"].ToString().Split(',');
                        foreach (string curName in currnList)
                        {
                            cmbCurrSymbol.Items.Add(curName.TrimStart());
                        }
                        cmbCurrSymbol.SelectedIndex = 0;
                    }
                    dataReader.Close();
                }
            }
        }

        private void lnkLblConnect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormServerConnection serverConnection = new FormServerConnection();
            serverConnection.ShowDialog();
        }
    }
}
