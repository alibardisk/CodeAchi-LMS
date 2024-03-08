using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public class CheckProductStatus
    {
        public static String insertApi = "http://codeachi.com/Product/LMS/InsertData.php?Q=";
        public static String selectApi = "http://codeachi.com/Product/LMS/SelectData.php?Q=";
        public static String updateApi = "http://codeachi.com/Product/LMS/UpdateData.php?Q=";
        public static String dateApi = "http://codeachi.com/Product/LMS/serverdate.php";

        public static string reserveSystemLimit;
        public static string serialKey;
        public static string machineLimits;
        public static string machineId;
        public static string licenseType;
        public static string blockReason;
        public static string strExpiryDate= DateTime.Now.Day.ToString("00") + "/" +
            DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
        public static string strLastChecked = DateTime.Now.Day.ToString("00") + "/" +
            DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
        public static string strCurrentDate = DateTime.Now.Day.ToString("00") + "/" +
            DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");

        public static bool productBlocked;
        public static bool productExpire;
        public static bool licenseChecked=false;

        public static int itemLimits;

        public static DateTime expiryDate = DateTime.Now.Date;
        public static DateTime currentDate = DateTime.Now.Date;
        public static DateTime lastChecked = DateTime.Now.Date;

        public static void checkProductDetails()
        {
            //BackgroundWorker backgroundWorker1 = new BackgroundWorker();
            //backgroundWorker1.WorkerSupportsCancellation = true;
            //backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
            //backgroundWorker1.RunWorkerAsync();
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                getRegistryValue(regKey);
                if (IsConnectedToInternet() == true)
                {
                    try
                    {
                        WebRequest webRequest = WebRequest.Create(dateApi);
                        webRequest.Timeout = 4000;
                        WebResponse webResponse = webRequest.GetResponse();
                        Stream dataStream = webResponse.GetResponseStream();
                        StreamReader strmReader = new StreamReader(dataStream);
                        string requestResult = strmReader.ReadLine();
                        if (requestResult != "")
                        {
                            strCurrentDate = requestResult;
                        }
                        string queryToCheck = "SELECT isBlocked,licenseKey,installDate,blocked_reason FROM installationDetails WHERE mac = '" + machineId + "' and productName='" + Application.ProductName + "'";
                        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        webRequest = WebRequest.Create(selectApi + queryToCheck);
                        webRequest.Timeout = 4000;
                        webResponse = webRequest.GetResponse();
                        dataStream = webResponse.GetResponseStream();
                        strmReader = new StreamReader(dataStream);
                        requestResult = strmReader.ReadLine();
                        if (requestResult != null)
                        {
                            string[] dataList = requestResult.Split('$');
                            if (dataList[1] == "Demo")
                            {
                                licenseType = "Demo";
                                DateTime expryDate = DateTime.ParseExact(dataList[2], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                expryDate = expryDate.AddDays(30);
                                strExpiryDate = expryDate.Day.ToString("00") + "/" + expryDate.Month.ToString("00") + "/" + expryDate.Year.ToString("0000");
                                if (dataList[0] == "True")
                                {
                                    productBlocked = true;
                                }
                                else
                                {
                                    productBlocked = false;
                                }
                                blockReason = dataList[3];
                                strLastChecked = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                                itemLimits = 1000;
                                machineLimits = 1.ToString();
                            }
                            else
                            {
                                serialKey = dataList[1];
                                if (IsConnectedToInternet() == true)
                                {
                                    try
                                    {
                                        queryToCheck = "SELECT reg_date, valid_month,licence_type,isBlocked,mac,machine_limits,itemLimits,reserveSystemLimits,status_reson,opacAvailable FROM new_license WHERE serial_key = '" + serialKey + "'";
                                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                        webRequest = WebRequest.Create(selectApi + queryToCheck);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                        webRequest.Timeout = 4000;
                                        webResponse = webRequest.GetResponse();
                                        dataStream = webResponse.GetResponseStream();
                                        strmReader = new StreamReader(dataStream);
                                        requestResult = strmReader.ReadLine();
                                        if (requestResult == null)
                                        {
                                        }
                                        else
                                        {
                                            if (requestResult.Contains(machineId))
                                            {
                                                string[] splitResult = requestResult.Split('$');
                                                int validMonth = Convert.ToInt16(splitResult[1]);
                                                licenseType = splitResult[2];
                                                bool licenseBlocked = true;
                                                if (splitResult[3] == "True")
                                                {
                                                    licenseBlocked = true;
                                                }
                                                else
                                                {
                                                    licenseBlocked = false;
                                                }
                                                DateTime regDate = DateTime.ParseExact(splitResult[0], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                DateTime expryDate = regDate.AddMonths(validMonth);
                                                productBlocked = licenseBlocked;
                                                strExpiryDate = expryDate.Day.ToString("00") + "/" + expryDate.Month.ToString("00") + "/" + expryDate.Year.ToString("0000");
                                                strLastChecked = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                                                machineLimits = splitResult[5];
                                                itemLimits = Convert.ToInt32(splitResult[6]);
                                                reserveSystemLimit = splitResult[7];
                                                blockReason = splitResult[8];
                                                //globalVarLms.opacAvailable = Convert.ToBoolean(splitResult[9]);
                                            }
                                            else
                                            {
                                                licenseType = "Demo";
                                                itemLimits = 1000;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        //////Server Not Responds
                                    }
                                }
                                else
                                {
                                    //////No Internet Connection
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                setRegistryValue(regKey);
                licenseChecking();
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        public static void getRegistryValue(RegistryKey regKey)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            object o1 = regKey.GetValue("Data1");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                machineId = Encoding.UTF8.GetString(byteData);
            }

            o1 = regKey.GetValue("Data2");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                licenseType = Encoding.UTF8.GetString(byteData);
            }

            o1 = regKey.GetValue("Data3");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                machineLimits = Encoding.UTF8.GetString(byteData);
            }

            o1 = regKey.GetValue("Data4");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                productBlocked =Convert.ToBoolean(Encoding.UTF8.GetString(byteData));
            }

            o1 = regKey.GetValue("Data5");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                strExpiryDate = Encoding.UTF8.GetString(byteData);
            }

            o1 = regKey.GetValue("Data6");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                strLastChecked = Encoding.UTF8.GetString(byteData);
            }

            o1 = regKey.GetValue("Data7");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                strCurrentDate = Encoding.UTF8.GetString(byteData);
                DateTime tempDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if(tempDate<DateTime.Now.Date)
                {
                    strCurrentDate = DateTime.Now.Day.ToString("00") + "/" +
                        DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                }
            }

            o1 = regKey.GetValue("Data8");
            if (o1 != null)
            {
                byteData = Convert.FromBase64String(o1.ToString());
                blockReason = Encoding.UTF8.GetString(byteData);
            }
        }

        private static void setRegistryValue(RegistryKey regKey)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(licenseType);
            regKey.SetValue("Data2", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(machineLimits);
            regKey.SetValue("Data3", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(productBlocked.ToString());
            regKey.SetValue("Data4", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(strExpiryDate);
            regKey.SetValue("Data5", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(strLastChecked);
            regKey.SetValue("Data6", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(strCurrentDate);
            regKey.SetValue("Data7", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(blockReason);
            regKey.SetValue("Data8", Convert.ToBase64String(byteData));
        }

        private static bool IsConnectedToInternet()
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

        private static void licenseChecking()
        {
            currentDate = DateTime.ParseExact(strCurrentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            expiryDate = DateTime.ParseExact(strExpiryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (strLastChecked != "" || strLastChecked != null)
            {
                try
                {
                    lastChecked = DateTime.ParseExact(strLastChecked, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                   
                    if (Convert.ToInt32((currentDate - lastChecked).TotalDays) > 60)
                    {
                        FormNotification notificationForm = new FormNotification();
                        notificationForm.lblMessage.Text = "Seems you are not connected to the internet since last 60 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.";
                        notificationForm.lnklblQuotation.Visible = false;
                        notificationForm.lnklblBuy.Visible = false;
                        notificationForm.lnklblRenew.Visible = false;
                        notificationForm.lnkLblActivate.Visible = false;
                        notificationForm.ShowDialog();
                    }
                }
                catch
                {

                }
            }
            else
            {
                FormNotification notificationForm = new FormNotification();
                notificationForm.lblMessage.Text = "Seems you are not connected to the internet since last 60 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.";
                notificationForm.lnklblQuotation.Visible = false;
                notificationForm.lnklblBuy.Visible = false;
                notificationForm.lnklblRenew.Visible = false;
                notificationForm.lnkLblActivate.Visible = false;
                notificationForm.ShowDialog();
            }
            if (licenseType == "Demo")
            {
                //pcbPremium.Visible = false;
                //upgradeLicenseToolStripMenuItem.Visible = false;
                //this.Text = Application.ProductName + " (v" + Application.ProductVersion + " - Trial)";
                //btnActivate.Text = "Activate";
                itemLimits = 1000;
                if (productBlocked)
                {
                    productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = blockReason; //"Seems like your trial version has been blocked!" + Environment.NewLine + "Contact us if you think it’s a mistake on our part.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else if (currentDate > expiryDate)
                {
                    productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
                else
                {
                    productExpire = false;
                }
            }
            else
            {
                //this.Text = Application.ProductName + " (v" + Application.ProductVersion + " - " + globalVarLms.licenseType + ")";
                //pcbPremium.Visible = true;
                //upgradeLicenseToolStripMenuItem.Visible = true;
                Application.DoEvents();
                if (productBlocked)
                {
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = blockReason; //"Seems like your serial key has been blocked!" + Environment.NewLine + "Contact us if you think it’s a mistake on our part.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else if (currentDate > expiryDate)
                {
                    productExpire = true;
                    //renewLicenseToolStripMenuItem.Visible = true;
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else
                {
                    productExpire = false;
                }
            }
            licenseChecked = true;

            ////==================Check for promotion==========================
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    string queryToCheck = "SELECT promotional_url,img_height,img_width FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(selectApi + queryToCheck);
                    webRequest.Timeout = 4000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "" && requestResult!=null)
                    {
                        string[] splitResult = requestResult.Split('$');
                        if (splitResult.Length == 4)
                        {
                            if (splitResult[0] != "")
                            {
                                FormEvent showEvent = new FormEvent();
                                showEvent.Height = Convert.ToInt32(splitResult[1]);
                                showEvent.Width = Convert.ToInt32(splitResult[2]);
                                showEvent.webBrowser1.Navigate(splitResult[0]);
                                showEvent.pcbClose.Location = new Point(showEvent.Width - 40, showEvent.pcbClose.Location.Y);
                                showEvent.ShowDialog();
                            }
                        }
                    }
                }
                catch
                {

                }

                ////==================Check for update==========================
                try
                {
                    string queryToCheck = "SELECT productVersion,downloadUrl,fileName FROM productExeVersion WHERE isLastUpdate = '" + true + "' and productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(selectApi + queryToCheck);
                    webRequest.Timeout = 4000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "" && requestResult!=null)
                    {
                        string[] spliData = requestResult.Split('$');
                        Version latestVersion = new Version(spliData[0]);
                        string downloadUrl = spliData[1];
                        string fileName = spliData[2];
                        Version installedVersion = new Version(Application.ProductVersion);
                        if (latestVersion > installedVersion)
                        {
                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\" + fileName))
                            {
                                if (MessageBox.Show("New version of " + Application.ProductName + " downloaded." + Environment.NewLine + "Do you want to install the new version ?", "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (currentDate > expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        Process.Start(Application.StartupPath + @"\Updater.exe");
                                        Application.Exit();
                                    }
                                }
                            }
                            else
                            {
                                if (MessageBox.Show(String.Format("You've got version {0} installed of " + Application.ProductName + ". Would you like to update to the latest version {1}?", installedVersion, latestVersion), "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (currentDate > expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        FormUpgrade upgradeProduct = new FormUpgrade();
                                        upgradeProduct.downloadUrl = downloadUrl + "$" + fileName;
                                        upgradeProduct.lblProdVersion.Text = Application.ProductVersion;
                                        upgradeProduct.lblLatestVersion.Text = spliData[0];
                                        upgradeProduct.ShowDialog();
                                        upgradeProduct.downloadUrl = "";
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public static void lastUsedTime(string ttlItems)
        {
            try
            {
                string lastUsed = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                string queryToUpdate = "UPDATE installationDetails set last_used='" + lastUsed + "',totalItems='" + ttlItems + "',ip='" + GetGlobalIP() + "',productUninstalled='" + false + "' WHERE mac = '" + machineId + "' and productName='" + Application.ProductName + "'";
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebRequest webRequest = WebRequest.Create(updateApi + queryToUpdate);
                webRequest.Timeout = 3000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader strmReader = new StreamReader(dataStream);
            }
            catch
            {

            }
        }

        private static string GetGlobalIP()
        {
            string IPAddress = string.Empty;
            try
            {
                WebRequest webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/global_IP.php");
                webRequest.Timeout = 2000;
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

        public void setDefaultLicense()
        {
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);

            byteData = Encoding.UTF8.GetBytes("Demo");
            regKey.SetValue("Data2", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(1.ToString());
            regKey.SetValue("Data3", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(false.ToString());
            regKey.SetValue("Data4", Convert.ToBase64String(byteData));

            expiryDate = DateTime.Now.Date.AddDays(30);
            strExpiryDate = expiryDate.Day.ToString("00") + "/" +
            expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
            byteData = Encoding.UTF8.GetBytes(strExpiryDate);
            regKey.SetValue("Data5", Convert.ToBase64String(byteData));

            strCurrentDate = getCurrentDate();
            byteData = Encoding.UTF8.GetBytes(strCurrentDate);
            regKey.SetValue("Data6", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(strCurrentDate);
            regKey.SetValue("Data7", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes("Not Exist");
            regKey.SetValue("Data8", Convert.ToBase64String(byteData));
        }

        private string getCurrentDate()
        {
            string currentDate= DateTime.Now.Day.ToString("00") + "/" +
            DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            try
            {

            }
            catch
            {
                WebRequest webRequest = WebRequest.Create(dateApi);
                webRequest.Timeout = 2000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader strmReader = new StreamReader(dataStream);
                string requestResult = strmReader.ReadLine();
                if (requestResult != "")
                {
                    currentDate = requestResult;
                }
            }
            return currentDate;
        }

        public void  reportProblem(string currentUserId, string userContact)
        {
            FormReportProblem reportProblem = new FormReportProblem();
            reportProblem.currentUserId = currentUserId;
            reportProblem.userContact = userContact;
            reportProblem.ShowDialog();
        }

        public static void checkForUpdate()
        {
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    string queryToCheck = "SELECT productVersion,downloadUrl,fileName FROM productExeVersion WHERE isLastUpdate = '" + true + "' and productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(selectApi + queryToCheck);
                    webRequest.Timeout = 3000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        string[] spliData = requestResult.Split('$');
                        Version latestVersion = new Version(spliData[0]);
                        string downloadUrl = spliData[1];
                        string fileName = spliData[2];
                        Version installedVersion = new Version(Application.ProductVersion);
                        if (latestVersion > installedVersion)
                        {
                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\" + fileName))
                            {
                                if (MessageBox.Show("New version of " + Application.ProductName + " downloaded." + Environment.NewLine + "Do you want to install the new version ?", "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (currentDate > expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        Process.Start(Application.StartupPath + @"\Updater.exe");
                                        Application.Exit();
                                    }
                                }
                            }
                            else
                            {
                                if (MessageBox.Show(String.Format("You've got version {0} installed of " + Application.ProductName + ". Would you like to update to the latest version {1}?", installedVersion, latestVersion), "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (currentDate > expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        FormUpgrade upgradeProduct = new FormUpgrade();
                                        upgradeProduct.downloadUrl= downloadUrl + "$" + fileName;
                                        upgradeProduct.lblProdVersion.Text = Application.ProductVersion;
                                        upgradeProduct.lblLatestVersion.Text = spliData[0];
                                        upgradeProduct.ShowDialog();
                                        upgradeProduct.downloadUrl = "";
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your version of CodeAchi LMS is up-to-date!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No update available!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Please try again later!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("please check your internet connection!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
