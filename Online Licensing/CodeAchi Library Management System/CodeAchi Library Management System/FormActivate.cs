using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormActivate : Form
    {
        public FormActivate()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public string licenseKey = "";

        private void btnLater_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if(txtbMailId.Text=="")
            {
                MessageBox.Show("Please enter your registered mail.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool internetConnection = IsConnectedToInternet();
            string serialKey = txtbSerial.Text;
            string macAddress =globalVarLms.machineId;
            string globalIP = GetGlobalIP();
            DateTime currentDate = DateTime.Now;
            if(licenseKey!="")
            {
                if(licenseKey!=serialKey)
                {
                    MessageBox.Show("Serial key isn't matched.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (internetConnection == true)
            {
                WebRequest webRequest = WebRequest.Create(globalVarLms.dateApi);
                webRequest.Timeout = 8000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader strmReader = new StreamReader(dataStream);
                string requestResult = strmReader.ReadLine();
                if(requestResult!="")
                {
                    currentDate= DateTime.ParseExact(requestResult, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                string queryToCheck = "SELECT machine_limits, mac, reg_date, valid_month,licence_type,isBlocked,product_id,opacAvailable FROM new_license WHERE serial_key = '" + serialKey + "'";
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);  
                webRequest.Timeout = 600000;
                webResponse = webRequest.GetResponse();
                dataStream = webResponse.GetResponseStream();
                strmReader = new StreamReader(dataStream);
                requestResult = strmReader.ReadLine();
                
                if (requestResult == null)
                {
                    MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbSerial.SelectAll();
                    return;
                }
                else
                {
                    //requestResult = requestResult.Replace("$<br>", "");
                    string[] splitResult = requestResult.Split('$');
                    if (splitResult[6] == Application.ProductName)
                    {
                        if (splitResult[0] == "")
                        {
                            MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbSerial.SelectAll();
                            return;
                        }
                        if (splitResult[3] == "")
                        {
                            MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbSerial.SelectAll();
                            return;
                        }
                        if (splitResult[4] == "")
                        {
                            MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbSerial.SelectAll();
                            return;
                        }
                        if (splitResult[5] == "")
                        {
                            MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbSerial.SelectAll();
                            return;
                        }
                        int mechineLimit = Convert.ToInt16(splitResult[0]);
                        int validMonth = Convert.ToInt16(splitResult[3]);
                        string licenseType = splitResult[4];
                        bool licenseStatus = true;
                        //DateTime currentDate = DateTime.ParseExact(splitResult[7], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (splitResult[5] == "False")
                        {
                            licenseStatus = true;
                        }
                        else
                        {
                            licenseStatus = false;
                            MessageBox.Show("Your serial key already blocked.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        string[] macList = splitResult[1].Split(',');
                        DateTime regDate = currentDate.Date;
                        
                        if (splitResult[2] != "")
                        {
                            regDate = DateTime.ParseExact(splitResult[2], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        DateTime expiryDate = regDate.AddMonths(validMonth);
                        if (expiryDate < currentDate.Date)
                        {
                            MessageBox.Show("Serial Key has been expired.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        
                        if (splitResult[1] == "")
                        {
                            string registerDate = regDate.Date.Day.ToString("00") + "/" + regDate.Date.Month.ToString("00") + "/" + regDate.Date.Year.ToString("0000");
                            internetConnection = IsConnectedToInternet();
                            if (internetConnection == true)
                            {
                                string queryToUpdate = "UPDATE new_license set mac='" + macAddress + "', reg_date='" + registerDate + "',cust_id='"+ txtbMailId.Text+"' WHERE serial_key = '" + serialKey + "'";
                                //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                webRequest.Timeout = 8000;
                                webResponse = webRequest.GetResponse();
                                dataStream = webResponse.GetResponseStream();
                                strmReader = new StreamReader(dataStream);
                                requestResult = strmReader.ReadLine();
                                if (requestResult == "Updated")
                                {
                                    if (internetConnection == true)
                                    {
                                        queryToUpdate = "UPDATE installationDetails set licenseKey='" + txtbSerial.Text + "', ip='" + globalIP + "'WHERE mac='" + macAddress + "'";
                                        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                        webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                        webRequest.Timeout = 8000;
                                        webResponse = webRequest.GetResponse();
                                        dataStream = webResponse.GetResponseStream();
                                        strmReader = new StreamReader(dataStream);
                                        requestResult = strmReader.ReadLine();
                                        if (requestResult == "Updated")
                                        {
                                            Properties.Settings.Default.productBlocked = licenseStatus;
                                            Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                                            Properties.Settings.Default.licenseType = licenseType;
                                            Properties.Settings.Default.serialKey = txtbSerial.Text;
                                            Properties.Settings.Default.machineLimits = mechineLimit.ToString();
                                            Properties.Settings.Default.Save();
                                            globalVarLms.productBlocked = licenseStatus;
                                            globalVarLms.expiryDate = expiryDate;
                                            globalVarLms.licenseType = licenseType;
                                            globalVarLms.opacAvailable = Convert.ToBoolean(splitResult[7]);
                                            MessageBox.Show("Product Registered Successfully." + Environment.NewLine + "Restart the Application to use full version features…", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            this.Hide();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            if (splitResult[1].Contains(macAddress))
                            {
                                if (internetConnection == true)
                                {
                                    string queryToUpdate = "UPDATE installationDetails set licenseKey='" + txtbSerial.Text + "', ip='" + globalIP + "' WHERE mac='" + macAddress + "'";
                                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                    webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                    webRequest.Timeout = 8000;
                                    webResponse = webRequest.GetResponse();
                                    dataStream = webResponse.GetResponseStream();
                                    strmReader = new StreamReader(dataStream);
                                    requestResult = strmReader.ReadLine();
                                    if (requestResult == "Updated")
                                    {
                                        Properties.Settings.Default.productBlocked = licenseStatus;
                                        Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                                        Properties.Settings.Default.licenseType = licenseType;
                                        Properties.Settings.Default.serialKey = txtbSerial.Text;
                                        Properties.Settings.Default.machineLimits = mechineLimit.ToString();
                                        Properties.Settings.Default.Save();
                                        globalVarLms.productBlocked = licenseStatus;
                                        globalVarLms.expiryDate = expiryDate;
                                        globalVarLms.licenseType = licenseType;
                                        globalVarLms.opacAvailable = Convert.ToBoolean(splitResult[7]);
                                        MessageBox.Show("Product Registered Successfully." + Environment.NewLine + "Restart the Application to use full version features…", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.Hide();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else if (mechineLimit > macList.Length)
                            {
                                macAddress = splitResult[1] + "," + macAddress;
                                string registerDate = regDate.Date.Day.ToString("00") + "/" + regDate.Date.Month.ToString("00") + "/" + regDate.Date.Year.ToString("0000");
                                internetConnection = IsConnectedToInternet();
                                if (internetConnection == true)
                                {
                                    string queryToUpdate = "UPDATE new_license set mac='" + macAddress + "', reg_date='" + registerDate + "' WHERE serial_key = '" + serialKey + "'";
                                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                    webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                    webRequest.Timeout = 8000;
                                    webResponse = webRequest.GetResponse();
                                    dataStream = webResponse.GetResponseStream();
                                    strmReader = new StreamReader(dataStream);
                                    requestResult = strmReader.ReadLine();
                                    
                                    if (requestResult == "Updated")
                                    {
                                        if (internetConnection == true)
                                        {
                                            queryToUpdate = "UPDATE installationDetails set licenseKey='" + txtbSerial.Text + "', ip='" + globalIP + "' WHERE mac='" + globalVarLms.machineId + "'";
                                            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                            webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                            webRequest.Timeout = 8000;
                                            webResponse = webRequest.GetResponse();
                                            dataStream = webResponse.GetResponseStream();
                                            strmReader = new StreamReader(dataStream);
                                            requestResult = strmReader.ReadLine();
                                            if (requestResult == "Updated")
                                            {
                                                Properties.Settings.Default.productBlocked = licenseStatus;
                                                Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                                                Properties.Settings.Default.licenseType = licenseType;
                                                Properties.Settings.Default.serialKey = txtbSerial.Text;
                                                Properties.Settings.Default.machineLimits = mechineLimit.ToString();
                                                Properties.Settings.Default.Save();

                                                globalVarLms.productBlocked = licenseStatus;
                                                globalVarLms.expiryDate = expiryDate;
                                                globalVarLms.licenseType = licenseType;
                                                globalVarLms.opacAvailable = Convert.ToBoolean(splitResult[7]);
                                                MessageBox.Show("Product Registered Successfully." + Environment.NewLine + "Restart the Application to use full version features…", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                this.Hide();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtbSerial.SelectAll();
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Enter a Valid Serial Key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbSerial.SelectAll();
                    }
                }
            }
            else
            {
                MessageBox.Show("Unable to connect.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
