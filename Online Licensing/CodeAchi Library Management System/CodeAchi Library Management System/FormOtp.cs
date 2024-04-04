using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormOtp : Form
    {
        public FormOtp()
        {
            InitializeComponent();
        }
        public bool otpVerified=false;
        public string jsonString="";
        APIRequest apiRequest = new APIRequest();

        private void txtbUserCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (txtbOtp.Text == "")
            {
                MessageBox.Show("Please enter the otp.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbOtp.Select();
            }
            else
            {
                dynamic jsonObject= Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                var requestData = new
                {
                    email = jsonObject.email,
                    otp = txtbOtp.Text.Trim(),
                    machineId = globalVarLms.machineId
                };
                var jsonStringV = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                var result =await apiRequest.VerifyOTP(jsonStringV);
                if(result== "OTP verified successfully!")
                {
                    otpVerified = true;
                    this.Hide();
                }
            }
        }

        private void FormOtp_Load(object sender, EventArgs e)
        {
            timer1.Start();
            label2.Visible=false;
            lblResend.Visible=false;
            otpVerified=false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            label2.Visible = true;
            lblResend.Visible = true;
        }

        private async void lblResend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string result = await apiRequest.SendOTP(jsonString);
            if (result == "OTP sent successfully!")
            {
                timer1.Start();
                label2.Visible = false;
                lblResend.Visible = false;
            }
        }
    }
}
