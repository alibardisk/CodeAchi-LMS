using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormEmailSetting : Form
    {
        public FormEmailSetting()
        {
            InitializeComponent();
        }

        string[] stopWordArray;
        string combineString = "";
        bool isTested = false;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormEmailSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void FormEmailSetting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.mailType == rdbManual.Text)
            {
                rdbManual.Checked = true;
            }
            else if (Properties.Settings.Default.mailType == rdbGmail.Text)
            {
                rdbGmail.Checked = true;
            }
            else if (Properties.Settings.Default.mailType == rdbYmail.Text)
            {
                rdbYmail.Checked = true;
            }
            else if (Properties.Settings.Default.mailType == rdbOutlook.Text)
            {
                rdbOutlook.Checked = true;
            }
            else
            {
                rdbGmail.Checked = true;
            }
            if (Properties.Settings.Default.blockedMail != "")
            {
                string[] blockedList = Properties.Settings.Default.blockedMail.Split('$');
                lstbBlockedList.Items.AddRange(blockedList);
            }
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                //==============Borrower Id add to autocomplete=============
                string queryString = "select brrMailId from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollData.Clear();
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["brrMailId"]
                        ).ToList();
                    autoCollData.AddRange(idList.ToArray());
                }
                dataReader.Close();
                txtbId.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbId.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbId.AutoCompleteCustomSource = autoCollData;
                sqltConn.Close();
            }
            else
            {
                try
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select brrMailId from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["brrMailId"]
                            ).ToList();
                        autoCollData.AddRange(idList.ToArray());
                    }
                    dataReader.Close();
                    txtbId.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtbId.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtbId.AutoCompleteCustomSource = autoCollData;
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void rdbManual_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.mailId != "" && Properties.Settings.Default.mailType == rdbManual.Text)
            {
                txtbMailId.Text = Properties.Settings.Default.mailId;
                txtbPassword.Text = Properties.Settings.Default.mailPassword;
                txtbHost.Text = Properties.Settings.Default.mailHost;
                txtbPort.Text = Properties.Settings.Default.mailPort;
                cmbSsl.Text = Properties.Settings.Default.mailSsl;
            }
            else
            {
                txtbMailId.Text = "";
                txtbPassword.Text = "";
                txtbHost.Text = "";
                txtbPort.Text = "";
                cmbSsl.Text = "";
            }
        }

        private void rdbGmail_CheckedChanged(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.mailId!="" && Properties.Settings.Default.mailType == rdbGmail.Text)
            {
                txtbMailId.Text = Properties.Settings.Default.mailId;
                txtbPassword.Text = Properties.Settings.Default.mailPassword;
                txtbHost.Text = Properties.Settings.Default.mailHost;
                txtbPort.Text = Properties.Settings.Default.mailPort;
                cmbSsl.Text = Properties.Settings.Default.mailSsl;
            }
            else
            {
                txtbMailId.Text = "";
                txtbPassword.Text = "";
                txtbHost.Text = "smtp.gmail.com";
                txtbPort.Text = "587";
                cmbSsl.Text = "True";
            }
        }

        private void rdbYmail_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.mailId != "" && Properties.Settings.Default.mailType == rdbYmail.Text)
            {
                txtbMailId.Text = Properties.Settings.Default.mailId;
                txtbPassword.Text = Properties.Settings.Default.mailPassword;
                txtbHost.Text = Properties.Settings.Default.mailHost;
                txtbPort.Text = Properties.Settings.Default.mailPort;
                cmbSsl.Text = Properties.Settings.Default.mailSsl;
            }
            else
            {
                txtbMailId.Text = "";
                txtbPassword.Text = "";
                txtbHost.Text = "smtp.mail.yahoo.com";
                txtbPort.Text = "587";
                cmbSsl.Text = "True";
            }
        }

        private void rdbOutlook_CheckedChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.mailId != "" && Properties.Settings.Default.mailType == rdbOutlook.Text)
            {
                txtbMailId.Text = Properties.Settings.Default.mailId;
                txtbPassword.Text = Properties.Settings.Default.mailPassword;
                txtbHost.Text = Properties.Settings.Default.mailHost;
                txtbPort.Text = Properties.Settings.Default.mailPort;
                cmbSsl.Text = Properties.Settings.Default.mailSsl;
            }
            else
            {
                txtbMailId.Text = "";
                txtbPassword.Text = "";
                txtbHost.Text = "smtp.live.com";
                txtbPort.Text = "25";
                cmbSsl.Text = "True";
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtbMailId.Text == "")
            {
                MessageBox.Show("Please enter an email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter the password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }
            if (txtbHost.Text == "")
            {
                MessageBox.Show("Please enter the host name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbHost.Select();
                return;
            }
            if (txtbPort.Text == "")
            {
                MessageBox.Show("Please enter the port number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPort.Select();
                return;
            }
            if (cmbSsl.Text == "")
            {
                MessageBox.Show("Please enter the ssl", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbSsl.Select();
                return;
            }
            if (txtbSenderId.Text == "")
            {
                MessageBox.Show("Please enter a reciver email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbSenderId.Select();
                return;
            }
            try
            {
                lblStatus.Text = "Trying to connect Host";
                SmtpClient smtpServer = new SmtpClient(txtbHost.Text.TrimEnd(), Convert.ToInt32(txtbPort.Text.TrimEnd()));
                MailMessage mailMessage = new MailMessage();
                //======================================SMTP SETTINGS===================
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential(txtbMailId.Text.TrimEnd(), txtbPassword.Text.TrimEnd());
                smtpServer.EnableSsl = Convert.ToBoolean(cmbSsl.Text);
                lblStatus.Text = "Host Connected";
                //===================================== SMTP =====================
                mailMessage.From = new MailAddress(txtbMailId.Text.TrimEnd());
                mailMessage.To.Add(txtbSenderId.Text.TrimEnd());
                mailMessage.Subject = "Test connection";
                mailMessage.Body = "Email testing successfully.";
                lblStatus.Text = "Email prepared";
                smtpServer.Send(mailMessage);
                lblStatus.Text = "Email Sent successfully."+Environment.NewLine+"Now you can save the settings.";
                isTested = true;
            }
            catch(Exception ex)
            {
                isTested = false;
                MessageBox.Show(ex.Message,Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isTested)
            {
                MessageBox.Show("Please check connection and save.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (rdbManual.Checked)
            {
                Properties.Settings.Default.mailType = rdbManual.Text;
            }
            else if (rdbGmail.Checked)
            {
                Properties.Settings.Default.mailType = rdbGmail.Text;
            }
            else if (rdbYmail.Checked)
            {
                Properties.Settings.Default.mailType = rdbYmail.Text;
            }
            else if (rdbOutlook.Checked)
            {
                Properties.Settings.Default.mailType = rdbOutlook.Text;
            }
            Properties.Settings.Default.mailId= txtbMailId.Text.TrimEnd();
            Properties.Settings.Default.mailPassword=txtbPassword.Text.TrimEnd();
            Properties.Settings.Default.mailHost= txtbHost.Text.TrimEnd();
            Properties.Settings.Default.mailPort= txtbPort.Text.TrimEnd();
            Properties.Settings.Default.mailSsl= cmbSsl.Text.TrimEnd();
            Properties.Settings.Default.Save();
            lblStatus.Text = "";
            MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbId_TextChanged(object sender, EventArgs e)
        {
            if(txtbId.Text!="")
            {
                if(lstbBlockedList.Items.IndexOf(txtbId.Text)!=-1)
                {
                    lstbBlockedList.SelectedIndex = lstbBlockedList.Items.IndexOf(txtbId.Text);
                }
            }
        }

        private void btnBlocked_Click(object sender, EventArgs e)
        {
            if(txtbId.Text=="")
            {
                MessageBox.Show("Please enter an email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstbBlockedList.Items.IndexOf(txtbId.Text) != -1)
            {
                MessageBox.Show("Email id already blocked.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            lstbBlockedList.Items.Add(txtbId.Text);
            stopWordArray = new string[lstbBlockedList.Items.Count];
            lstbBlockedList.Items.CopyTo(stopWordArray, 0);
            combineString = string.Join("$", stopWordArray);
            Properties.Settings.Default.blockedMail = combineString;
            Properties.Settings.Default.Save();
            txtbId.Clear();
            txtbId.Select();
        }

        private void lstbBlockedList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //select the item under the mouse pointer
                lstbBlockedList.SelectedIndex = lstbBlockedList.IndexFromPoint(e.Location);
                if (lstbBlockedList.SelectedIndex != -1)
                {
                    contextMenuStrip1.Show(lstbBlockedList,e.Location);
                }
            }
        }

        private void unblockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            combineString = "";
            lstbBlockedList.Items.Remove(lstbBlockedList.SelectedItem.ToString());
            if (lstbBlockedList.Items.Count > 0)
            {
                stopWordArray = new string[lstbBlockedList.Items.Count];
                lstbBlockedList.Items.CopyTo(stopWordArray, 0);
                combineString = string.Join("$", stopWordArray);
            }
            Properties.Settings.Default.blockedMail = combineString;
            Properties.Settings.Default.Save();
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

        private void btnTutorial_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("See Tutorial", btnTutorial);
        }

        private void btnTutorial_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnTutorial);
        }

        private void btnTutorial_Click(object sender, EventArgs e)
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
    }
}
