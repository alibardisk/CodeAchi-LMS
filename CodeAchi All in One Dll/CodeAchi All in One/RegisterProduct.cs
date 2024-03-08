using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public class RegisterProduct
    {

        static string machineId = "";

        public static void registerDetails(string userType, string orgName, string orgMail, string orgCountry, string orgAddress, string orgWebsite,string orgContact, string userName, string userContact, string userMail)
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
                    string machineId = Encoding.UTF8.GetString(byteData);
                    if (IsConnectedToInternet())
                    {
                        try
                        {
                            string installedDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                            WebRequest webRequest = WebRequest.Create(CheckProductStatus.dateApi);
                            webRequest.Timeout = 4000;
                            WebResponse webResponse = webRequest.GetResponse();
                            Stream dataStream = webResponse.GetResponseStream();
                            StreamReader strmReader = new StreamReader(dataStream);
                            string requestResult = strmReader.ReadLine();
                            if (requestResult != "")
                            {
                                installedDate = requestResult;
                            }
                            string installTime = DateTime.Now.ToString("hh:mm:ss tt");
                            string queryToCheck = "SELECT isBlocked,licenseKey,installDate FROM installationDetails WHERE mac = '" + machineId + "' and productName='" + Application.ProductName + "'";
                            //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                            webRequest = WebRequest.Create(CheckProductStatus.selectApi + queryToCheck);
                            webRequest.Timeout = 4000;
                            webResponse = webRequest.GetResponse();
                            dataStream = webResponse.GetResponseStream();
                            strmReader = new StreamReader(dataStream);
                            requestResult = strmReader.ReadLine();
                            if (requestResult == null)
                            {
                                string queryToInsert = "INSERT INTO installationDetails (mac,productName,isBlocked,licenseKey,installDate,installTime,useFor,org_name,email,country,address,website,orgContact,cust_name,contact,userMail)" +
                                    " VALUES('" + machineId + "', '" + Application.ProductName + "','" + false + "','" + "Demo" + "','" + installedDate + "','" + installTime + "','" + userType + "'," +
                                    "'" + orgName + "','" + orgMail + "','" + orgCountry + "','" + orgAddress + "','" + orgWebsite + "','" + orgContact + "','" + userName + "','" + userContact + "','" + userMail + "')";

                                //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                                webRequest = WebRequest.Create(CheckProductStatus.insertApi + queryToInsert);
                                webRequest.Timeout = 4000;
                                webResponse = webRequest.GetResponse();
                                dataStream = webResponse.GetResponseStream();
                                strmReader = new StreamReader(dataStream);
                                requestResult = strmReader.ReadLine();
                                if (requestResult == "Inserted")
                                {
                                    o1 = regKey.GetValue("Data2");
                                    if (o1 == null)
                                    {
                                        setDefaultRegistryValue(regKey);
                                    }
                                    MessageBox.Show("Product registered successfully.", Application.ProductName, MessageBoxButtons.OK);
                                    Application.Restart();
                                }
                                else
                                {
                                    MessageBox.Show("Product not registered.", Application.ProductName, MessageBoxButtons.OK);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Product Alrady registered.", Application.ProductName, MessageBoxButtons.OK);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Please check your internet connection and try again.", Application.ProductName, MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            sMacAddress = Regex.Replace(sMacAddress, ".{2}", "$0:");
            sMacAddress = sMacAddress.Remove(sMacAddress.Length - 1, 1);
            return sMacAddress;
        }

        public static bool IsConnectedToInternet()
        {
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

        public static bool productRegistered()
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
                    o1 = regKey.GetValue("Data2");
                    if (o1 == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return false;
            }
        }

        private static void setDefaultRegistryValue(RegistryKey regKey)
        {
            byte[] byteData = Encoding.UTF8.GetBytes("Demo");
            regKey.SetValue("Data2", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(1.ToString());
            regKey.SetValue("Data3", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(false.ToString());
            regKey.SetValue("Data4", Convert.ToBase64String(byteData));

            CheckProductStatus.expiryDate = DateTime.Now.Date.AddDays(30);
            CheckProductStatus.strExpiryDate= CheckProductStatus.expiryDate.Day.ToString("00") + "/" +
            CheckProductStatus.expiryDate.Month.ToString("00") + "/" + CheckProductStatus.expiryDate.Year.ToString("0000");
            byteData = Encoding.UTF8.GetBytes(CheckProductStatus.strExpiryDate);
            regKey.SetValue("Data5", Convert.ToBase64String(byteData));

            CheckProductStatus.currentDate = DateTime.Now.Date;
            CheckProductStatus.strCurrentDate = CheckProductStatus.expiryDate.Day.ToString("00") + "/" +
            CheckProductStatus.expiryDate.Month.ToString("00") + "/" + CheckProductStatus.expiryDate.Year.ToString("0000");
            byteData = Encoding.UTF8.GetBytes(CheckProductStatus.strCurrentDate);
            regKey.SetValue("Data6", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes(CheckProductStatus.strCurrentDate);
            regKey.SetValue("Data7", Convert.ToBase64String(byteData));

            byteData = Encoding.UTF8.GetBytes("Not Exist");
            regKey.SetValue("Data8", Convert.ToBase64String(byteData));
        }
    }
}
