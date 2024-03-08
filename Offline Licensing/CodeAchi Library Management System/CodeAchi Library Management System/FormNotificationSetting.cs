using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormNotificationSetting : Form
    {
        public FormNotificationSetting()
        {
            InitializeComponent();
        }

        string jsonString = "";

        private void FormNotificationSetting_Load(object sender, EventArgs e)
        {
            cmbIssueMail.Items.Add("Please select a email template...");
            cmbIssueSms.Items.Add("Please select a SMS template...");
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
                    cmbIssueMail.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                        ).ToArray());
                }
                dataReader.Close();

                sqltCommnd.CommandText = "select tempName from noticeTemplate where noticeType!='Mail'";
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    cmbIssueSms.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
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
                        cmbIssueMail.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                            ).ToArray());
                    }
                    dataReader.Close();

                    queryString = "select tempName from notice_template where noticeType!='Mail'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        cmbIssueSms.Items.AddRange((from IDataRecord r in dataReader select (string)r["tempName"]
                            ).ToArray());

                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }

            cmbReissueMail.Items.AddRange(cmbIssueMail.Items.Cast<Object>().ToArray());
            cmbReturnMail.Items.AddRange(cmbIssueMail.Items.Cast<Object>().ToArray());
            cmbReservedMail.Items.AddRange(cmbIssueMail.Items.Cast<Object>().ToArray());
            cmbArrivedMail.Items.AddRange(cmbIssueMail.Items.Cast<Object>().ToArray());
            cmbDueMail.Items.AddRange(cmbIssueMail.Items.Cast<Object>().ToArray());

            cmbIssueMail.SelectedIndex = 0;
            cmbReissueMail.SelectedIndex = 0;
            cmbReturnMail.SelectedIndex = 0;
            cmbReservedMail.SelectedIndex = 0;
            cmbArrivedMail.SelectedIndex = 0;
            cmbDueMail.SelectedIndex = 0;
            cmbIssueMail.Enabled = false;
            cmbReissueMail.Enabled = false;
            cmbReturnMail.Enabled = false;
            cmbReservedMail.Enabled = false;
            cmbArrivedMail.Enabled = false;
            cmbDueMail.Enabled = false;

            cmbReissueSms.Items.AddRange(cmbIssueSms.Items.Cast<Object>().ToArray());
            cmbReturnSms.Items.AddRange(cmbIssueSms.Items.Cast<Object>().ToArray());
            cmbReservedSms.Items.AddRange(cmbIssueSms.Items.Cast<Object>().ToArray());
            cmbArrivedSms.Items.AddRange(cmbIssueSms.Items.Cast<Object>().ToArray());
            cmbDueSms.Items.AddRange(cmbIssueSms.Items.Cast<Object>().ToArray());

            cmbIssueSms.SelectedIndex = 0;
            cmbReissueSms.SelectedIndex = 0;
            cmbReturnSms.SelectedIndex = 0;
            cmbReservedSms.SelectedIndex = 0;
            cmbArrivedSms.SelectedIndex = 0;
            cmbDueSms.SelectedIndex = 0;
            cmbNoticeTime.SelectedIndex = 0;
            cmbIssueSms.Enabled = false;
            cmbReissueSms.Enabled = false;
            cmbReturnSms.Enabled = false;
            cmbReservedSms.Enabled = false;
            cmbArrivedSms.Enabled = false;
            cmbDueSms.Enabled = false;
            cmbNoticeTime.Enabled = false;
           
            try
            {
                jsonString = Properties.Settings.Default.notificationData;
                JObject jsonObj = JObject.Parse(jsonString);

                chkbIssueMail.Checked = Convert.ToBoolean(jsonObj["IssueMail"].ToString());
                cmbIssueMail.Text = jsonObj["IssueMailTemplate"].ToString();
                chkbReissueMail.Checked = Convert.ToBoolean(jsonObj["ReissueMail"].ToString());
                cmbReissueMail.Text = jsonObj["ReissueMailTemplate"].ToString();
                chkbReturnMail.Checked = Convert.ToBoolean(jsonObj["ReturnMail"].ToString());
                cmbReturnMail.Text = jsonObj["ReturnMailTemplate"].ToString();
                chkbReseveMail.Checked = Convert.ToBoolean(jsonObj["ReservedMail"].ToString());
                cmbReservedMail.Text = jsonObj["ReservedMailTemplate"].ToString();
                chkbArrivedMail.Checked = Convert.ToBoolean(jsonObj["ArrivedMail"].ToString());
                cmbArrivedMail.Text = jsonObj["ArrivedMailTemplate"].ToString();
                chkbDueMail.Checked = Convert.ToBoolean(jsonObj["DueMail"].ToString());
                cmbDueMail.Text = jsonObj["DueMailTemplate"].ToString();

                chkbIssueSms.Checked = Convert.ToBoolean(jsonObj["IssueSms"].ToString());
                cmbIssueSms.Text = jsonObj["IssueSmsTemplate"].ToString();
                chkbReissueSms.Checked = Convert.ToBoolean(jsonObj["ReissueSms"].ToString());
                cmbReissueSms.Text = jsonObj["ReissueSmsTemplate"].ToString();
                chkbReturnSms.Checked = Convert.ToBoolean(jsonObj["ReturnSms"].ToString());
                cmbReturnSms.Text = jsonObj["ReturnSmsTemplate"].ToString();
                chkbReserveSms.Checked = Convert.ToBoolean(jsonObj["ReservedSms"].ToString());
                cmbReservedSms.Text = jsonObj["ReservedSmsTemplate"].ToString();
                chkbArrivedSms.Checked = Convert.ToBoolean(jsonObj["ArrivedSms"].ToString());
                cmbArrivedSms.Text = jsonObj["ArrivedSmsTemplate"].ToString();
                chkbDueSms.Checked = Convert.ToBoolean(jsonObj["DueSms"].ToString());
                cmbDueSms.Text = jsonObj["DueSmsTemplate"].ToString();

                cmbNoticeTime.Text = jsonObj["DueNotificationCondition"].ToString();
            }
            catch
            {
                Properties.Settings.Default.notificationData = "{\"IssueMail\" : \"" + false + "\"," +
                                    "\"IssueMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"IssueSms\" : \"" + false + "\"," +
                                    "\"IssueSmsTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReissueMail\" : \"" + false + "\"," +
                                    "\"ReissueMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReissueSms\" : \"" + false + "\"," +
                                    "\"ReissueSmsTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReturnMail\" : \"" + false + "\"," +
                                    "\"ReturnMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReturnSms\" : \"" + false + "\"," +
                                    "\"ReturnSmsTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReservedMail\" : \"" + false + "\"," +
                                    "\"ReservedMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"ReservedSms\" : \"" + false + "\"," +
                                    "\"ReservedSmsTemplate\" : \"" + "Template" + "\"," +
                                     "\"ArrivedMail\" : \"" + false + "\"," +
                                    "\"ArrivedMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"ArrivedSms\" : \"" + false + "\"," +
                                    "\"ArrivedSmsTemplate\" : \"" + "Template" + "\"," +
                                    "\"DueMail\" : \"" + false + "\"," +
                                    "\"DueMailTemplate\" : \"" + "Template" + "\"," +
                                    "\"DueSms\" : \"" + false + "\"," +
                                    "\"DueSmsTemplate\" : \"" + "Template" + "\"," +
                                    "\"DueNotificationDate\" : \"" + "Date" + "\"," +
                                    "\"DueNotificationCondition\" : \"" + "Condition" + "\"" + "}";
                Properties.Settings.Default.Save();
            }

            if(Properties.Settings.Default.mailCarbonCopy)
            {
                chkbMngmnt.Checked = true;
            }
            else
            {
                chkbMngmnt.Checked = false;
            }
        }

        private void FormNotificationSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            JObject jsonObj = JObject.Parse(jsonString);
            if (chkbIssueMail.Checked)
            {
                if(cmbIssueMail.SelectedIndex==0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["IssueMail"] = true;
                jsonObj["IssueMailTemplate"] = cmbIssueMail.Text;
            }
            else
            {
                jsonObj["IssueMail"] = false;
            }

            if (chkbReissueMail.Checked)
            {
                if (cmbReissueMail.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReissueMail"] = true;
                jsonObj["ReissueMailTemplate"] = cmbReissueMail.Text;
            }
            else
            {
                jsonObj["ReissueMail"] = false;
            }

            if (chkbReturnMail.Checked)
            {
                if (cmbReturnMail.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReturnMail"] = true;
                jsonObj["ReturnMailTemplate"] = cmbReturnMail.Text;
            }
            else
            {
                jsonObj["ReturnMail"] = false;
            }

            if (chkbReseveMail.Checked)
            {
                if (cmbReservedMail.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReservedMail"] = true;
                jsonObj["ReservedMailTemplate"] = cmbReservedMail.Text;
            }
            else
            {
                jsonObj["ReservedMail"] = false;
            }

            if (chkbArrivedMail.Checked)
            {
                if (cmbArrivedMail.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ArrivedMail"] = true;
                jsonObj["ArrivedMailTemplate"] = cmbArrivedMail.Text;
            }
            else
            {
                jsonObj["ArrivedMail"] = false;
            }

            if (chkbDueMail.Checked)
            {
                if (cmbDueMail.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbNoticeTime.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a notification duration.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["DueMail"] = true;
                jsonObj["DueMailTemplate"] = cmbDueMail.Text;
                jsonObj["DueNotificationCondition"] = cmbNoticeTime.Text;
                jsonObj["DueNotificationDate"] = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" +
                DateTime.Now.Year.ToString("0000");
            }
            else
            {
                jsonObj["DueMail"] = false;
            }


            if (chkbIssueSms.Checked)
            {
                if (cmbIssueSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["IssueSms"] = true;
                jsonObj["IssueSmsTemplate"] = cmbIssueSms.Text;
            }
            else
            {
                jsonObj["IssueSms"] = false;
            }

            if (chkbReissueSms.Checked)
            {
                if (cmbReissueSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReissueSms"] = true;
                jsonObj["ReissueSmsTemplate"] = cmbReissueSms.Text;
            }
            else
            {
                jsonObj["ReissueSms"] = false;
            }

            if (chkbReturnSms.Checked)
            {
                if (cmbReturnSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReturnSms"] = true;
                jsonObj["ReturnSmsTemplate"] = cmbReturnSms.Text;
            }
            else
            {
                jsonObj["ReturnSms"] = false;
            }

            if (chkbReserveSms.Checked)
            {
                if (cmbReservedSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ReservedSms"] = true;
                jsonObj["ReservedSmsTemplate"] = cmbReservedSms.Text;
            }
            else
            {
                jsonObj["ReservedSms"] = false;
            }

            if (chkbArrivedSms.Checked)
            {
                if (cmbArrivedSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["ArrivedSms"] = true;
                jsonObj["ArrivedSmsTemplate"] = cmbArrivedSms.Text;
            }
            else
            {
                jsonObj["ArrivedSms"] = false;
            }

            if (chkbDueSms.Checked)
            {
                if (cmbDueSms.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a email template.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (cmbNoticeTime.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a notification duration.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                jsonObj["DueSms"] = true;
                jsonObj["DueSmsTemplate"] = cmbDueSms.Text;
                jsonObj["DueNotificationCondition"] = cmbNoticeTime.Text;
                jsonObj["DueNotificationDate"] = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" +
                DateTime.Now.Year.ToString("0000");
            }
            else
            {
                jsonObj["DueSms"] = false;
            }

            if(chkbMngmnt.Checked)
            {
                Properties.Settings.Default.mailCarbonCopy = true;
            }
            else
            {
                Properties.Settings.Default.mailCarbonCopy = false;
            }
            jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            Properties.Settings.Default.notificationData = jsonString;
            Properties.Settings.Default.Save();
            MessageBox.Show("Save successfully.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void chkbIssueMail_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbIssueMail.Checked)
            {
                cmbIssueMail.Enabled = true;
            }
            else
            {
                cmbIssueMail.Enabled = false;
            }
        }

        private void chkbIssueSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbIssueSms.Checked)
            {
                cmbIssueSms.Enabled = true;
            }
            else
            {
                cmbIssueSms.Enabled = false;
            }
        }

        private void chkbReissueMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReissueMail.Checked)
            {
                cmbReissueMail.Enabled = true;
            }
            else
            {
                cmbReissueMail.Enabled = false;
            }
        }

        private void chkbReissueSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReissueSms.Checked)
            {
                cmbReissueSms.Enabled = true;
            }
            else
            {
                cmbReissueSms.Enabled = false;
            }
        }

        private void chkbReturnMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReturnMail.Checked)
            {
                cmbReturnMail.Enabled = true;
            }
            else
            {
                cmbReturnMail.Enabled = false;
            }
        }

        private void chkbReturnSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReturnSms.Checked)
            {
                cmbReturnSms.Enabled = true;
            }
            else
            {
                cmbReturnSms.Enabled = false;
            }
        }

        private void chkbReseveMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReseveMail.Checked)
            {
                cmbReservedMail.Enabled = true;
            }
            else
            {
                cmbReservedMail.Enabled = false;
            }
        }

        private void chkbReserveSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbReserveSms.Checked)
            {
                cmbReservedSms.Enabled = true;
            }
            else
            {
                cmbReservedSms.Enabled = false;
            }
        }

        private void chkbArrivedMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbArrivedMail.Checked)
            {
                cmbArrivedMail.Enabled = true;
            }
            else
            {
                cmbArrivedMail.Enabled = false;
            }
        }

        private void chkbArrivedSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbArrivedSms.Checked)
            {
                cmbArrivedSms.Enabled = true;
            }
            else
            {
                cmbArrivedSms.Enabled = false;
            }
        }

        private void chkbDueMail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbDueMail.Checked)
            {
                cmbDueMail.Enabled = true;
                cmbNoticeTime.Enabled = true;
            }
            else
            {
                cmbDueMail.Enabled = false;
                if(chkbDueSms.Checked)
                {
                    cmbNoticeTime.Enabled = true;
                }
                else
                {
                    cmbNoticeTime.Enabled = false;
                }
            }
        }

        private void chkbDueSms_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbDueSms.Checked)
            {
                cmbDueSms.Enabled = true;
                cmbNoticeTime.Enabled = true;
            }
            else
            {
                cmbDueSms.Enabled = false;
                if (chkbDueMail.Checked)
                {
                    cmbNoticeTime.Enabled = true;
                }
                else
                {
                    cmbNoticeTime.Enabled = false;
                }
            }
        }
    }
}
