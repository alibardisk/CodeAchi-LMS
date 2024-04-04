using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
        APIRequest apiRequest = new APIRequest();
        PasswordHasher passwordHasher = new PasswordHasher();
        string configFilePath = Application.StartupPath + "/clms.json";

        private void btnLater_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnActivate_Click(object sender, EventArgs e)
        {
            if(txtbMailId.Text=="")
            {
                MessageBox.Show("Please enter your registered mail.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string serialKey = txtbSerial.Text;
            if(serialKey == "")
            {
                MessageBox.Show("Please enter your serial key.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var requestData = new
            {
                hardwareId = apiRequest.GetHardwareId(),
                machineId = globalVarLms.machineId,
                key= serialKey,
                email= txtbMailId.Text.Trim()
            };
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            string responseBody = await apiRequest.ActivateLicense(jsonString);
            if (responseBody != "")
            {
                if (IsValidJson(responseBody))
                {
                    var responseObject = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    jsonString = passwordHasher.Decrypt(File.ReadAllText(configFilePath));
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    if (jsonObject["LicenseName"] == null)
                    {
                        jsonObject.Add("LicenseName", responseObject.licenseName);
                    }
                    else
                    {
                        jsonObject["LicenseName"] = responseObject.licenseName;
                    }
                    jsonObject["isLicensed"] = responseObject.isLicensed;
                    string jsonData = JsonConvert.SerializeObject(jsonObject);
                    jsonData = passwordHasher.Encrypt(jsonData);
                    File.WriteAllText(configFilePath, jsonData);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(responseBody,Application.ProductName,MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static bool IsValidJson(string jsonString)
        {
            try
            {
                JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }
    }
}
