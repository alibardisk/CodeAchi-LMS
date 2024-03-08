using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormProblemReport : Form
    {
        public FormProblemReport()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        private void FormProblemReport_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string dateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
            if(IsConnectedToInternet())
            {
                if(txtbProblem.Text=="")
                {
                    MessageBox.Show("Please enter your problem.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbProblem.Select();
                    return;
                }
                try
                {
                    string queryToInsert = "INSERT INTO helpDesk (productName,email,contact,description,date_time) values" +
                        " ('"+ Application.ProductName +"','"+ globalVarLms.currentUserId +"','"+ globalVarLms.userContact +"','"+ txtbProblem.Text +"','"+ dateTime +"')";
                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.insertApi + queryToInsert);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if(requestResult=="Inserted")
                    {
                        MessageBox.Show("Your report has been recieved!"+Environment.NewLine+ "We will contact to \"" + globalVarLms.currentUserId +
                            "\" when resolved."+Environment.NewLine+ "Your comments are always important for  CodeAchi.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbProblem.Clear();
                    }
                }
                catch
                {
                    MessageBox.Show("Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
