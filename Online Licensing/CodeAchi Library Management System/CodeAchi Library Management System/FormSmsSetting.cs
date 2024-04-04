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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormSmsSetting : Form
    {
        public FormSmsSetting()
        {
            InitializeComponent();
        }

        string[] stopWordArray;
        string combineString = "";
        bool isTested = false;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormSmsSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void FormSmsSetting_Load(object sender, EventArgs e)
        {
            txtbApi.Text = Properties.Settings.Default.smsApi;
            if (Properties.Settings.Default.blockedContact != "")
            {
                string[] blockedList = Properties.Settings.Default.blockedContact.Split('$');
                lstbBlockedList.Items.AddRange(blockedList);
            }
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                //==============Borrower Id add to autocomplete=============
                string queryString = "select brrContact from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollData.Clear();
                    List<string> contactList = (from IDataRecord r in dataReader
                                                select (string)r["brrContact"]
                        ).ToList();
                    autoCollData.AddRange(contactList.ToArray());
                }
                dataReader.Close();
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
                    string queryString = "select brrContact from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> contactList = (from IDataRecord r in dataReader
                                                    select (string)r["brrContact"]
                            ).ToList();
                        autoCollData.AddRange(contactList.ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            txtbId.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbId.AutoCompleteCustomSource = autoCollData;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtbApi.Text == "")
            {
                MessageBox.Show("Please enter a sms api.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbApi.Select();
                return;
            }
            if (txtbSenderId.Text== "")
            {
                MessageBox.Show("Please enter a contact no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbSenderId.Select();
                return;
            }
            try
            {
                string messageBody = "SMS testing successfully.";
                string smsApi = txtbApi.Text.TrimEnd().Replace("[$ContactNumber$]", txtbSenderId.Text.TrimEnd()).
                    Replace("[$Message$]", messageBody);
                WebRequest webRequest = WebRequest.Create(smsApi);
                webRequest.Timeout = 8000;
                WebResponse webResponse = webRequest.GetResponse();
                isTested = true;
                MessageBox.Show("SMS sent successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                isTested = false;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isTested)
            {
                MessageBox.Show("Please check connection and save.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Properties.Settings.Default.smsApi = txtbApi.Text.TrimEnd();
            Properties.Settings.Default.Save();
            MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtbId_TextChanged(object sender, EventArgs e)
        {
            if (txtbId.Text != "")
            {
                if (lstbBlockedList.Items.IndexOf(txtbId.Text) != -1)
                {
                    lstbBlockedList.SelectedIndex = lstbBlockedList.Items.IndexOf(txtbId.Text);
                }
            }
        }

        private void btnBlocked_Click(object sender, EventArgs e)
        {
            if (txtbId.Text == "")
            {
                MessageBox.Show("Please enter a borrower id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstbBlockedList.Items.IndexOf(txtbId.Text) != -1)
            {
                MessageBox.Show("Contact already blocked.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            lstbBlockedList.Items.Add(txtbId.Text);
            stopWordArray = new string[lstbBlockedList.Items.Count];
            lstbBlockedList.Items.CopyTo(stopWordArray, 0);
            combineString = string.Join("$", stopWordArray);
            Properties.Settings.Default.blockedContact = combineString;
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
                    contextMenuStrip1.Show(lstbBlockedList, e.Location);
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
            Properties.Settings.Default.blockedContact = combineString;
            Properties.Settings.Default.Save();
        }

        private void txtbId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbSenderId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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
