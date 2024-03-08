using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormMail : Form
    {
        public FormMail()
        {
            InitializeComponent();
        }

        bool sendingCancel = false;
        string tempId = "";
        string[] attchmentList;

        private void FormMail_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void FormMail_Load(object sender, EventArgs e)
        {
            dgvBorrower.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvBorrower.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            cmbMailTemplate.Items.Add("Please select a template...");
            cmbSmsTemplate.Items.Add("Please select a template...");

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select tempName from noticeTemplate where noticeType='Mail'";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    cmbMailTemplate.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                        ).ToArray());
                }
                dataReader.Close();

                sqltCommnd.CommandText = "select tempName from noticeTemplate where noticeType!='Mail'";
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    cmbSmsTemplate.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                        ).ToArray());
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
                    string queryString = "select tempName from notice_template where noticeType='Mail'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        cmbMailTemplate.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                            ).ToArray());
                    }
                    dataReader.Close();

                    queryString = "select tempName from notice_template where noticeType!='Mail'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        cmbSmsTemplate.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                            ).ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            cmbSmsTemplate.SelectedIndex = 0;
            cmbMailTemplate.SelectedIndex = 0;
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblAttach.Text = "";
            panelStatus.Visible = false;
            chkbMail.Checked = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtbName.Text == "")
            {
                MessageBox.Show("please enter the name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbMail.Text == "")
            {
                MessageBox.Show("please enter the email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbContact.Text == "")
            {
                MessageBox.Show("please enter the contact no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow[] dgvRow = dgvBorrower.Rows.OfType<DataGridViewRow>()
                 .Where(x => (string)x.Cells["emailId"].Value == txtbMail.Text)
                 .ToArray<DataGridViewRow>();

            if (dgvRow.Count() != 0)
            {
                MessageBox.Show("Already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dgvBorrower.Rows.Add(false, txtbName.Text, txtbMail.Text, txtbContact.Text);
            dgvBorrower.ClearSelection();

            txtbContact.Clear();
            txtbMail.Clear();
            txtbName.Clear();
        }

        private void btnMailSave_Click(object sender, EventArgs e)
        {
            if(txtbMailSubject.Text=="")
            {
                MessageBox.Show("Please add a subject.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbFrom.Text == "")
            {
                MessageBox.Show("Please enter a sender email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbMailBody.Text == "")
            {
                MessageBox.Show("Please enter the email body.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool htmlBody = false;
            if(rdbHBody.Checked)
            {
                htmlBody = true;
            }
            if(btnMailSave.Text!="Update Template")
            {
                if(cmbMailTemplate.Items.IndexOf(txtbMailSubject.Text)!=-1)
                {
                    MessageBox.Show("Template already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "insert into noticeTemplate (tempName,senderId,htmlBody,bodyText,noticeType)" +
                        " values (@tempName,@senderId,'" + htmlBody + "',@bodyText,'Mail')";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", txtbMailSubject.Text);
                    sqltCommnd.Parameters.AddWithValue("@senderId", txtbFrom.Text);
                    sqltCommnd.Parameters.AddWithValue("@bodyText", txtbMailBody.Text);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "insert into notice_template (tempName,senderId,htmlBody,bodyText,noticeType)" +
                        " values (@tempName,@senderId,'" + htmlBody + "',@bodyText,'Mail')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", txtbMailSubject.Text);
                    mysqlCmd.Parameters.AddWithValue("@senderId", txtbFrom.Text);
                    mysqlCmd.Parameters.AddWithValue("@bodyText", txtbMailBody.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                cmbMailTemplate.Items.Add(txtbMailSubject.Text);
                MessageBox.Show("Template save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "update noticeTemplate set tempName=:tempName, senderId=:senderId," +
                        "htmlBody='" + htmlBody + "',bodyText=:bodyText where id='" + tempId + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("tempName", txtbMailSubject.Text);
                    sqltCommnd.Parameters.AddWithValue("senderId", txtbFrom.Text);
                    sqltCommnd.Parameters.AddWithValue("bodyText", txtbMailBody.Text);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "update notice_template set tempName=@tempName, senderId=@senderId," +
                        "htmlBody='" + htmlBody + "',bodyText=@bodyText where id='" + tempId + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", txtbMailSubject.Text);
                    mysqlCmd.Parameters.AddWithValue("@senderId", txtbFrom.Text);
                    mysqlCmd.Parameters.AddWithValue("@bodyText", txtbMailBody.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Template Update successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbMailTemplate.SelectedIndex = 0;
            }
            txtbMailSubject.Clear();
            txtbFrom.Clear();
            rdbNBody.Checked = true;
            txtbMailBody.Clear();
        }

        private void cmbMailTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelStatus.Visible = false;
            if (cmbMailTemplate.SelectedIndex>0)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", cmbMailTemplate.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            tempId = dataReader["id"].ToString();
                            txtbMailSubject.Text = dataReader["tempName"].ToString();
                            txtbFrom.Text = dataReader["senderId"].ToString();
                            txtbMailBody.Text = dataReader["bodyText"].ToString();
                            if (Convert.ToBoolean(dataReader["htmlBody"].ToString()))
                            {
                                rdbHBody.Checked = true;
                            }
                            else
                            {
                                rdbNBody.Checked = true;
                            }
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select * from notice_template where noticeType='Mail' and tempName=@tempName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", cmbMailTemplate.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            tempId = dataReader["id"].ToString();
                            txtbMailSubject.Text = dataReader["tempName"].ToString();
                            txtbFrom.Text = dataReader["senderId"].ToString();
                            txtbMailBody.Text = dataReader["bodyText"].ToString();
                            if (Convert.ToBoolean(dataReader["htmlBody"].ToString()))
                            {
                                rdbHBody.Checked = true;
                            }
                            else
                            {
                                rdbNBody.Checked = true;
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                btnMailSave.Text = "Update Template";
            }
            else
            {
                txtbMailSubject.Clear();
                txtbFrom.Clear();
                rdbNBody.Checked = true;
                txtbMailBody.Clear();
                btnMailSave.Text = "Save as Template";
                tempId = "";
            }
        }

        private void btnSmsSave_Click(object sender, EventArgs e)
        {
            if (txtbSmsSubject.Text == "")
            {
                MessageBox.Show("Please add a subject.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbSmsBody.Text == "")
            {
                MessageBox.Show("Please enter the email body.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           
            if (btnSmsSave.Text != "Update Template")
            {
                if (cmbSmsTemplate.Items.IndexOf(txtbSmsSubject.Text) != -1)
                {
                    MessageBox.Show("Template already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "insert into noticeTemplate (tempName,htmlBody,bodyText,noticeType)" +
                        " values (@tempName,'" + false + "',@bodyText,'Sms')";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", txtbSmsSubject.Text);
                    sqltCommnd.Parameters.AddWithValue("@bodyText", txtbSmsBody.Text);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "insert into notice_template (tempName,htmlBody,bodyText,noticeType)" +
                        " values (@tempName,'" + false + "',@bodyText,'Sms')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", txtbSmsSubject.Text);
                    mysqlCmd.Parameters.AddWithValue("@bodyText", txtbSmsBody.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                cmbSmsTemplate.Items.Add(txtbSmsSubject.Text);
                MessageBox.Show("Template save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "update noticeTemplate set tempName=:tempName," +
                        "htmlBody='" + false + "',bodyText=:bodyText where id='" + tempId + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("tempName", txtbSmsSubject.Text);
                    sqltCommnd.Parameters.AddWithValue("bodyText", txtbSmsBody.Text);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "update notice_template set tempName=:tempName," +
                        "htmlBody='" + false + "',bodyText=:bodyText where id='" + tempId + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("tempName", txtbSmsSubject.Text);
                    mysqlCmd.Parameters.AddWithValue("bodyText", txtbSmsBody.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Template Update successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbSmsTemplate.SelectedIndex = 0;
            }
            txtbSmsSubject.Clear();
            txtbSmsBody.Clear();
        }

        private void cmbSmsTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelStatus.Visible = false;
            if (cmbSmsTemplate.SelectedIndex > 0)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", cmbSmsTemplate.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            tempId = dataReader["id"].ToString();
                            txtbSmsSubject.Text = dataReader["tempName"].ToString();
                            txtbSmsBody.Text = dataReader["bodyText"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select * from notice_template where noticeType='Sms' and tempName=@tempName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", cmbSmsTemplate.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            tempId = dataReader["id"].ToString();
                            txtbSmsSubject.Text = dataReader["tempName"].ToString();
                            txtbSmsBody.Text = dataReader["bodyText"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                btnSmsSave.Text = "Update Template";
            }
            else
            {
                txtbMailSubject.Clear();
                txtbFrom.Clear();
                rdbNBody.Checked = true;
                txtbMailBody.Clear();
                btnSmsSave.Text = "Save as Template";
                tempId = "";
            }
        }

        private void lnkLblAddBorrower_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormMemberLookup findMember = new FormMemberLookup();
            findMember.lblInfo.Text = "* Select row/rows to Send Emails/SMS (Press Ctrl+A for all selection).";
            findMember.btnSend.Visible = true;
            findMember.dgvBrrDetails.MultiSelect = true;
            findMember.contextMenuStrip1.Enabled =false;
            globalVarLms.memberList.Clear();
            globalVarLms.mailSending = true;
            findMember.ShowDialog();
            string[] dataList;
            foreach(string memberInfo in globalVarLms.memberList)
            {
                dataList = memberInfo.Split('$');
                dgvBorrower.Rows.Add(false,dataList[0], dataList[1], dataList[2]);
            }
            dgvBorrower.ClearSelection();
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAll.Checked)
            {
                foreach (DataGridViewRow dataRow in dgvBorrower.Rows)
                {
                    dataRow.Cells[0].Value = true;
                    dgvBorrower.CurrentCell = dataRow.Cells[1];
                }
                dgvBorrower.ClearSelection();
                Application.DoEvents();
            }
            else
            {
                foreach (DataGridViewRow dataRow in dgvBorrower.Rows)
                {
                    dataRow.Cells[0].Value = false;
                    dgvBorrower.CurrentCell = dataRow.Cells[1];
                }
                dgvBorrower.ClearSelection();
                Application.DoEvents();
            }
        }

        private void dgvBorrower_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void txtbMailBody_TextChanged(object sender, EventArgs e)
        {
            if(txtbMailBody.Text!="")
            {
                lblInfo1.Text = "Total Character - "+txtbMailBody.TextLength;
                lblInfo1.Visible = true;
            }
            else
            {
                lblInfo1.Text = "";
                lblInfo1.Visible = false;
            }
        }

        private void txtbSmsBody_TextChanged(object sender, EventArgs e)
        {
            if (txtbSmsBody.Text != "")
            {
                lblInfo2.Text = "Total Character - " + txtbSmsBody.TextLength;
                lblInfo2.Visible = true;
            }
            else
            {
                lblInfo2.Text = "";
                lblInfo2.Visible = false;
            }
        }

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = Application.ProductName + " Select Attachment File";
            selectFile.Multiselect = true;
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                lblAttach.Text = "";
                attchmentList = selectFile.FileNames;
                foreach (string attachFile in attchmentList)
                {
                    if (lblAttach.Text == "")
                    {
                        lblAttach.Text = Path.GetFileName(attachFile);
                    }
                    else
                    {
                        lblAttach.Text = lblAttach.Text +", "+ Path.GetFileName(attachFile);
                    }
                }
            }
        }

        private void lnkLblEmailsetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormEmailSetting mailSetting = new FormEmailSetting();
            mailSetting.ShowDialog();
            updateSettingData();
        }

        private void updateSettingData()
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
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "update generalSettings set notificationData=:notificationData,settingsData=:settingsData;";
                sqltCommnd.Parameters.AddWithValue("notificationData", Properties.Settings.Default.notificationData);
                sqltCommnd.Parameters.AddWithValue("settingsData", settingsData);
                sqltCommnd.ExecuteNonQuery();
                sqltConn.Close();
            }
            else
            {
                MySqlConnection mysqlConn;
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MySqlCommand mysqlCmd;
                string queryString = "update general_settings set notificationData=@notificationData,settingsData=@settingsData;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@notificationData", Properties.Settings.Default.notificationData);
                mysqlCmd.Parameters.AddWithValue("@settingsData", settingsData);
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
            }
        }

        private void lnkLblSmsSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormSmsSetting smsSetting = new FormSmsSetting();
            smsSetting.ShowDialog();
            updateSettingData();
        }

        private void chkbMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbMail.Checked)
            {
                panelStatus.Visible = false;
            }
        }

        private void chkbSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbSms.Checked)
            {
                panelStatus.Visible = false;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            sendingCancel = false;
            if (dgvBorrower.Rows.Count == 0)
            {
                MessageBox.Show("Please add some details.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow[] dgvCheckedRows = dgvBorrower.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            if (dgvCheckedRows.Count() == 0)
            {
                MessageBox.Show("Please check some emails.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (chkbMail.Checked)
            {
                if (Properties.Settings.Default.mailType=="")
                {
                    MessageBox.Show("Please set your email setting first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if(txtbMailSubject.Text=="")
                {
                    MessageBox.Show("Please enter an email subject.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbFrom.Text == "")
                {
                    MessageBox.Show("Please enter a sender email id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbMailBody.Text == "")
                {
                    MessageBox.Show("Please enter the email body.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                bool htmlBody = false;
                if(rdbHBody.Checked)
                {
                    htmlBody = true;
                }
                progressBar1.Maximum = dgvCheckedRows.Count();
                lblTtlText.Text = "Total Mail :";
                lblTtlMail.Text = dgvCheckedRows.Count().ToString();
                lblSent.Text = "Sent Mail :";
                panelStatus.Visible = true;
                lblSend.Visible = false;
                Application.DoEvents();
                int mailCount = 0, tempCount = 0;
                List<string> blockList = Properties.Settings.Default.blockedMail.Split('$').ToList();

                SmtpClient smtpServer = new SmtpClient(Properties.Settings.Default.mailHost, Convert.ToInt32(Properties.Settings.Default.mailPort));
                MailMessage mailMessage = new MailMessage();
                Attachment mailAttachments = null;// new Attachment(attchmentList)
                //======================================SMTP SETTINGS===================
                smtpServer.UseDefaultCredentials = false;
                smtpServer.Credentials = new NetworkCredential(Properties.Settings.Default.mailId, Properties.Settings.Default.mailPassword);
                smtpServer.EnableSsl = Convert.ToBoolean(Properties.Settings.Default.mailSsl);
                //===================================== SMTP =====================
               
                foreach (DataGridViewRow dgvRow in dgvCheckedRows)
                {
                    if(sendingCancel)
                    {
                        break;
                    }
                    if (blockList.IndexOf(dgvRow.Cells[2].Value.ToString()) == -1)
                    {
                        if(tempCount == 5)
                        {
                            Thread.Sleep(5000);
                            tempCount = 0;
                        }
                        try
                        {
                            mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress(txtbFrom.Text);
                            mailMessage.To.Add(dgvRow.Cells[2].Value.ToString().TrimEnd());
                            mailMessage.Subject = txtbMailSubject.Text;
                            mailMessage.IsBodyHtml = htmlBody;
                            mailMessage.Body = txtbMailBody.Text.TrimEnd().Replace("[$BorrowerName$]", dgvRow.Cells[1].Value.ToString());
                            if (attchmentList != null)
                            {
                                foreach (string attachFile in attchmentList)
                                {
                                    mailAttachments = new Attachment(attachFile);
                                    mailMessage.Attachments.Add(mailAttachments);
                                }
                            }
                            smtpServer.Send(mailMessage);
                            mailCount++;
                            tempCount++;
                            progressBar1.Value = mailCount;
                            lblSntMail.Text = mailCount.ToString();
                            Application.DoEvents();
                        }
                        catch
                        {

                        }
                    }
                }
                MessageBox.Show("Email sent successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblSend.Visible = true;
            }
            if(chkbSms.Checked)
            {
                if (Properties.Settings.Default.smsApi == "")
                {
                    MessageBox.Show("Please set your sms setting first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbSmsSubject.Text == "")
                {
                    MessageBox.Show("Please enter an sms subject.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtbSmsBody.Text == "")
                {
                    MessageBox.Show("Please enter the email body.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string messageBody = "Subject : "+txtbSmsSubject.Text+Environment.NewLine+Environment.NewLine+txtbSmsBody.Text;

                progressBar1.Maximum = dgvCheckedRows.Count();
                lblTtlText.Text = "Total SMS :";
                lblTtlMail.Text = dgvCheckedRows.Count().ToString();
                lblSent.Text = "Sent SMS :";
                panelStatus.Visible = true;
                lblSend.Visible = false;
                Application.DoEvents();
                int smsCount = 0, tempCount = 0;
                List<string> blockList = Properties.Settings.Default.blockedContact.Split('$').ToList();

                string smsApi = Properties.Settings.Default.smsApi.Replace("[$Message$]", messageBody); ;
                WebRequest webRequest;
                WebResponse webResponse;

                foreach (DataGridViewRow dgvRow in dgvCheckedRows)
                {
                    if (sendingCancel)
                    {
                        break;
                    }
                    if (blockList.IndexOf(dgvRow.Cells[3].Value.ToString()) == -1)
                    {
                        if (tempCount == 2)
                        {
                            Thread.Sleep(000);
                            tempCount = 0;
                        }
                        try
                        {
                            webRequest =WebRequest.Create(smsApi.Replace("[$ContactNumber$]", dgvRow.Cells[3].Value.ToString().TrimEnd()).Replace("[$BorrowerName$]", dgvRow.Cells[1].Value.ToString()));
                            webRequest.Timeout = 8000;
                            webResponse = webRequest.GetResponse();

                            smsCount++;
                            tempCount++;
                            progressBar1.Value = smsCount;
                            lblSntMail.Text = smsCount.ToString();
                            Application.DoEvents();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                MessageBox.Show("SMS sent successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblSend.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            sendingCancel = true;
        }

        private void txtbContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("[$BorrowerName$], [$BorrowerId$], [$EmilId$], [$Address$], [$ItemTitle$]," + Environment.NewLine +
                             "[$ItemAccession$], [$ItemAuthor$], [$IssueDate$], [$ExpectedDate$]," + Environment.NewLine +
                             "[$ReturnDate$],[$DueDays$], [$DueAmount$]", "Keword Variables");
        }
    }
}
