using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Ubiety.Dns.Core.Common;

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
        APIRequest apiRequest = new APIRequest();
        PasswordHasher passwordHasher= new PasswordHasher();
        string configFilePath = Application.StartupPath + "/clms.json";
        string usageFilePath = Application.StartupPath + "/usage.json";

        private async void FormWizard_Load(object sender, EventArgs e)
        {
            btnNext.Enabled = false;
            lblCompany1.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
            txtbDatabasePath.Text = databasePath;
            btnPrev.Visible = false;
            this.Text = Application.ProductName + " Setup Wizard";
            lblStep.Text = "Library Setup Wizard (Step 1)";
            lblStepCount.Text = 1.ToString();
            var requestData = new
            {
                hardwareId = apiRequest.GetHardwareId(),
                productName = Application.ProductName.Replace("CodeAchi ", ""),
                softwareVersion = "offline/" + Application.ProductVersion.ToString(),
                source = File.ReadAllText(Application.StartupPath + "/sourcefile.txt")
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            string responseBody = await apiRequest.GenerateUniqueId(jsonString);
            if (responseBody != "")
            {
                btnNext.Enabled = true;
                // Deserialize the JSON response
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                // Sample data to serialize
                string connectionString = "Data Source=" + txtbDatabasePath.Text + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";
                var dataToSerialize = new
                {
                    InstallationId = responseObject.machineId,
                    KeyFeatures = responseObject.features,
                    ConnectionString=connectionString,
                    SQLiteData = true
                };
                string jsonData = JsonConvert.SerializeObject(dataToSerialize);
                jsonData = passwordHasher.Encrypt(jsonData);
                File.WriteAllText(configFilePath, jsonData);
                globalVarLms.machineId = responseObject.machineId;
                globalVarLms.sqliteData = true;

                var Usage = new
                {
                    Usage = responseObject.features,
                };
                jsonData = JsonConvert.SerializeObject(Usage);
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonData);
                jsonObject.Usage["total-items"] = 0;
                jsonObject.Usage["total-member"] = 0;
                jsonData = passwordHasher.Encrypt(jsonObject.Usage.ToString());
                File.WriteAllText(usageFilePath, jsonData);
                btnNext.Enabled = true;

                SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                if (sqltConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        sqltConn.Open();
                    }
                    catch
                    {
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

                txtbOrgCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbOrgCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbOrgCountry.AutoCompleteCustomSource = autoCollData;

                txtbUserCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbUserCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbUserCountry.AutoCompleteCustomSource = autoCollData;
                globalVarLms.connectionString = connectionString;
            }
            else
            {
                MessageBox.Show("Connection failure. Contact our support at www.codeachi.com.", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
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

        private async void btnNext_Click(object sender, EventArgs e)
        {
            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            string queryString = "select [countryName] from countryDetails";
            sqltCommnd.CommandText = queryString;
            SQLiteDataReader dataReader;
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

                    pnlStep3.Visible = false;
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

                    pnlStep3.Visible = true;
                    txtbFullName.Select();
                    userType = "Official";
                }
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
                lblStep.Text = "Library Setup Wizard (Step 2)";
                lblStepCount.Text = 2.ToString();

                string jsonString = passwordHasher.Decrypt(File.ReadAllText(configFilePath));
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                string connectionString = "Data Source=" + txtbDatabasePath.Text + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";
                jsonObject["ConnectionString"] = connectionString;
                globalVarLms.connectionString = connectionString;
                string jsonData = JsonConvert.SerializeObject(jsonObject);
                jsonData = passwordHasher.Encrypt(jsonData);
                File.WriteAllText(configFilePath, jsonData);

                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from itemDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                string ttlItems = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from userDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                string ttlUser = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from borrowerDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                string ttlMember = sqltCommnd.ExecuteScalar().ToString();

                jsonString = passwordHasher.Decrypt(File.ReadAllText(usageFilePath));
                jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                jsonObject["total-items"] = ttlItems;
                jsonObject["total-member"] = ttlMember;
                jsonObject["total-librarian"] = ttlUser;
                jsonData = passwordHasher.Encrypt(jsonObject.ToString());
                File.WriteAllText(usageFilePath, jsonData);
            }
            else if (lblStepCount.Text == 2.ToString())
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
                var requestData = new
                {
                    email = txtbUserMail.Text,
                    product = Application.ProductName.Replace("CodeAchi ", ""),

                };
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                string result = await apiRequest.SendOTP(jsonString);
                if (result == "OTP sent successfully!")
                {
                    FormOtp formOtp = new FormOtp();
                    formOtp.jsonString = jsonString;
                    formOtp.ShowDialog();
                    if (formOtp.otpVerified)
                    {
                        btnNext.Enabled = false;
                        var clientData = new
                        {
                            name = txtbUserName.Text,
                            designation = txtbUserDesignation.Text,
                            email = txtbUserMail.Text,
                            phone = txtbContact.Text
                        };
                        var requestData1 = new
                        {
                            hardwareId = apiRequest.GetHardwareId(),
                            machineId = globalVarLms.machineId,
                            client = clientData
                        };
                        jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requestData1);
                        var clientResult= await apiRequest.UpdateClient(jsonString);
                        if(clientResult == "Client data updated successfully")
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
                            queryString = "select id,userMail from userDetails where userMail=@userMail";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@userMail", txtbUserMail.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                string id = "";
                                while (dataReader.Read())
                                {
                                    id = dataReader["id"].ToString();
                                }
                                dataReader.Close();
                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "update userDetails set userName=:userName,userDesig=:userDesig,userMail=:userMail," +
                                    "userContact='" + txtbContact.Text + "',userAddress=:userAddress,userPriviledge" +
                                    "='" + "EI$EM$IR$RP" + "',isActive='" + true + "',isAdmin='" + true + "',userImage='" + userImage + "' where id='"+id+"'";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("userName", txtbUserName.Text);
                                sqltCommnd.Parameters.AddWithValue("userDesig", txtbUserDesignation.Text);
                                sqltCommnd.Parameters.AddWithValue("userMail", txtbUserMail.Text);
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
                            txtbCountry.Text = txtbUserCountry.Text;
                            txtbDesignation.Text=txtbUserDesignation.Text;
                            txtbName.Text = txtbUserName.Text;
                            txtbMailId.Text = txtbUserMail.Text;
                            while (pnlStep2.Location.X > -642)
                            {
                                pnlStep2.Location = new Point(pnlStep2.Location.X - 5, pnlStep2.Location.Y);
                            }
                            sqltCommnd.CommandText = "select * from generalSettings";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    txtbFullName.Text = dataReader["instName"].ToString();
                                    txtbShortName.Text = dataReader["instShortName"].ToString();
                                    txtbOrgAddress.Text = dataReader["instAddress"].ToString();
                                    txtbOrgContact.Text = dataReader["instContact"].ToString();
                                    txtbOrgMail.Text = dataReader["instMail"].ToString();
                                    txtbOrgCountry.Text = dataReader["countryName"].ToString();
                                    TxtbWebsite.Text = dataReader["instWebsite"].ToString();
                                }
                            }
                            dataReader.Close();
                            txtbFullName.TabIndex = 0;
                            txtbShortName.TabIndex = 1;
                            txtbOrgAddress.TabIndex = 2;
                            txtbOrgMail.TabIndex = 3;
                            TxtbWebsite.TabIndex = 4;
                            txtbOrgContact.TabIndex = 5;
                            txtbOrgCountry.TabIndex = 6;
                            btnUpload.TabIndex = 7;
                            btnNext.TabIndex = 8;
                            btnPrev.Visible = true;
                            lblStep.Text = "Library Setup Wizard (Step 3)";
                            lblStepCount.Text = 3.ToString();
                            btnNext.Enabled = true;
                        }
                    }
                    else
                    {

                    }
                }
            }
            else if (lblStepCount.Text == 3.ToString())
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
                btnNext.Enabled = false;
                var org = new
                {
                    name = txtbFullName.Text,
                    website = TxtbWebsite.Text,
                    email = txtbOrgMail.Text,
                    phone = txtbOrgContact.Text
                };
                var requestData1 = new
                {
                    hardwareId = apiRequest.GetHardwareId(),
                    machineId = globalVarLms.machineId,
                    org = org
                };
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requestData1);
                var clientResult = await apiRequest.UpdateOrganization(jsonString);
                if (clientResult == "Organization data updated successfully")
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

                    sqltCommnd.CommandText = "select * from generalSettings";
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        sqltCommnd = sqltConn.CreateCommand();
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
                    else
                    {
                        dataReader.Close();
                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "Insert into generalSettings (instName,instShortName,instAddress,instWebsite,instMail,countryName,backupPath,notificationData,settingsData,backupHour,instLogo,instContact)" +
                        " values (@instName,@instShortName,@instAddress,@instWebsite,@instMail,@countryName,@backupPath,@notificationData,@settingsData,'" + 60 + "',@instLogo,'"+ txtbOrgContact .Text+ "');";
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
                        sqltCommnd.Parameters.AddWithValue("instLogo", base64String);
                        sqltCommnd.ExecuteNonQuery();
                    }
                   
                    while (pnlStep3.Location.X > -642)
                    {
                        pnlStep3.Location = new Point(pnlStep3.Location.X - 5, pnlStep3.Location.Y);
                    }
                    lblStep.Text = "Library Setup Wizard (Step 4)";
                    lblStepCount.Text = 4.ToString();
                    btnNext.Enabled = true; btnPrev.Enabled=true;

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
            string settingsData = "{\"mailType\" : \"" + Properties.Settings.Default.mailType + "\"," +
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
                while (pnlStep3.Location.X <= 12)
                {
                    pnlStep3.Location = new Point(pnlStep3.Location.X + 5, pnlStep3.Location.Y);
                    Application.DoEvents();
                }
                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 2)";
                lblStepCount.Text = 2.ToString();
                pnlStep3.Location = new Point(10, pnlStep3.Location.Y);
            }
            else if (lblStepCount.Text == 4.ToString())
            {
                while (pnlStep2.Location.X <= 12)
                {
                    pnlStep2.Location = new Point(pnlStep2.Location.X + 5, pnlStep2.Location.Y);
                    Application.DoEvents();
                }

                btnPrev.Visible = true;
                lblStep.Text = "Library Setup Wizard (Step 3)";
                lblStepCount.Text = 3.ToString();
                pnlStep2.Location = new Point(10, pnlStep3.Location.Y);
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
                pnlStep4.Location = new Point(10, pnlStep3.Location.Y);
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
