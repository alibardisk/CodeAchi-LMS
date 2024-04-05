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
    public partial class FormTechnicalSupport : Form
    {
        public FormTechnicalSupport()
        {
            InitializeComponent();
        }

        private void FormTechnicalSupport_Load(object sender, EventArgs e)
        {
            rdbWhatsApp.Checked = true;
            txtbDetails.Select();
        }

        private void FormTechnicalSupport_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void rdbWhatsApp_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbWhatsApp.Checked)
            {
                lblCode.Visible = false;
                lblId.Text = "Your Whats App Id :";
                txtbId.Select();
                cmbHour.SelectedIndex = 0;
            }
        }

        private void rdbSkype_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSkype.Checked)
            {
                lblCode.Visible = false;
                lblId.Text = "Your Skype Id :";
                txtbId.Select();
                cmbHour.SelectedIndex = 0;
            }
        }

        private void rdbPhone_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPhone.Checked)
            {
                lblCode.Visible = true;
                lblId.Text = "Your Contact No :";
                txtbId.Select();
                cmbHour.SelectedIndex = 0;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //if(txtbDetails.Text=="")
            //{
            //    MessageBox.Show("Please describe your problem.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtbDetails.Select();
            //    return;
            //}
            //if (txtbId.Text == "")
            //{
            //    MessageBox.Show("Please enter your contact id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtbId.Select();
            //    return;
            //}
            //if (cmbHour.SelectedIndex==0)
            //{
            //    MessageBox.Show("Please select your available time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cmbHour.Select();
            //    return;
            //}
            //if(IsConnectedToInternet())
            //{
            //    try
            //    {
            //        WebRequest webRequest = WebRequest.Create(globalVarLms.dateApi);
            //        webRequest.Timeout = 8000;
            //        WebResponse webResponse = webRequest.GetResponse();
            //        Stream dataStream = webResponse.GetResponseStream();
            //        StreamReader strmReader = new StreamReader(dataStream);
            //        string currentDate = strmReader.ReadLine();
            //        string currentTime = DateTime.Now.ToString("HH:mm:ss tt");
            //        string contactType = "";
            //        if(rdbWhatsApp.Checked)
            //        {
            //            contactType = rdbWhatsApp.Text;
            //        }
            //        if (rdbSkype.Checked)
            //        {
            //            contactType = rdbSkype.Text;
            //        }
            //        if (rdbPhone.Checked)
            //        {
            //            contactType = rdbPhone.Text;
            //        }
            //        string queryToInsert = "INSERT INTO technical_support (macId,productName,problemDescription,"+
            //            "contactType,contactId,requestDate,requestTime,availableTime) VALUES('"+globalVarLms.machineId+"','" + Application.ProductName + "',"+
            //            "'" + txtbDetails.Text + "','" + contactType + "','" + txtbId.Text + "','" + currentDate + "','" + currentTime + "','" + cmbHour.Text + "')";
            //        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //        webRequest = WebRequest.Create(globalVarLms.insertApi + queryToInsert);
            //        webRequest.Timeout = 8000;
            //        webResponse = webRequest.GetResponse();
            //        MessageBox.Show("We have recived your request, "+Environment.NewLine+"We will contact as soon as possible!",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            //    }
            //    catch
            //    {
            //        MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        public bool IsConnectedToInternet()
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
    }
}
