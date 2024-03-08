using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CodeAchi_Library_Management_System
{
    public partial class FormDashBoard : Form
    {
        public FormDashBoard()
        {
            InitializeComponent();
        }

        private const uint SC_CLOSE = 0xf060;
        private const uint MF_GRAYED = 0x01;
        private const int MF_ENABLED = 0x00000000;
        private const int MF_DISABLED = 0x00000002;

        [DllImport("user32.dll")]
        private static extern int EnableMenuItem(IntPtr hMenu, uint wIDEnableItem, uint wEnable);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        Thread dueItems, reserveItems;
        string reserveSystemLimit =0.ToString(),blockReason="";
        DateTime currentDate = DateTime.Now.Date;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCollData1 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCollData2 = new AutoCompleteStringCollection();

        FormBackup dataBackup = new FormBackup();
        MySqlConnection mysqlConn;
        JObject jsonObj;
        bool htmlBody = false;
        string jsonString = "", sederId = "", mailBody,smsBody, issuedBorrower, reciverId, reciverAddress,
            reciverName,reciverContact="", reciverId1;

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        private void FormDashBoard_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.licenseType == "Demo")
            {
                pcbPremium.Visible = false;
            }
            upgradeLicenseToolStripMenuItem.Visible = false;
            renewLicenseToolStripMenuItem.Visible = false;
            ucBorrewerSetting1.Visible = false;
            ucAddBorrower1.Visible = false;
            ucItemSettings1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucItemIssue1.Visible = false;
            ucItemReissueReturn1.Visible = false;
            backgroundWorker1.RunWorkerAsync();
            loadFieldValue();
            mainMenu.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
            lblCompany1.Text = "© 2015-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            lblCompany2.Text = "© 2015-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
        }

        private void loadFieldValue()
        {
            if (FieldSettings.Default.itemEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        if (fieldName == "lblAccession")
                        {
                            dgvDueBooks.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            dgvReservationList.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvReservationList.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            dgvDueBooks.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvReservationList.Columns[5].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
        }

        private void borrowerSetting_Click(object sender, EventArgs e)
        {
            ucBorrewerSetting1.dgvBrrCategory.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucBorrewerSetting1.dgvBrrCategory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============Item Category&Subcategory Add=============
                string queryString = "select catName from itemSettings order by catName asc";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    List<string> cateList = new List<string> { };
                    while (dataReader.Read())
                    {
                        cateList.Add(dataReader["catName"].ToString());
                    }
                    dataReader.Close();
                    //================Remove panel control============
                    for (int i = 0; i < ucBorrewerSetting1.pnlChkb.Controls.Count; i++)
                    {
                        Control pnlControl = ucBorrewerSetting1.pnlChkb.Controls[i];
                        if (pnlControl is CheckBox)
                        {
                            ucBorrewerSetting1.pnlChkb.Controls.Remove(pnlControl);
                            pnlControl.Dispose();
                            i--;
                        }
                    }
                    CheckBox chkbButton = new CheckBox();
                    chkbButton.Name = "All";
                    chkbButton.Text = "All";
                    chkbButton.CheckedChanged += new EventHandler(ucBorrewerSetting1.CheckBox_CheckedChanged);
                    chkbButton.Location = new Point(2, ucBorrewerSetting1.pnlChkb.Controls.Count * 25);
                    ucBorrewerSetting1.pnlChkb.Controls.Add(chkbButton);
                    foreach (string catName in cateList)
                    {
                        queryString = "select subCatName from itemSubCategory where catName =@catName and notReference='" + true + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", catName);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            int i = 0;
                            while (dataReader.Read())
                            {
                                chkbButton = new CheckBox();
                                chkbButton.Name = i.ToString();
                                chkbButton.Width = 200;
                                chkbButton.Text = catName + "(" + dataReader["subCatName"] + ")";
                                chkbButton.CheckedChanged += new EventHandler(ucBorrewerSetting1.CheckBox_CheckedChanged);
                                chkbButton.Location = new Point(2, ucBorrewerSetting1.pnlChkb.Controls.Count * 25);
                                ucBorrewerSetting1.pnlChkb.Controls.Add(chkbButton);
                                i++;
                            }
                        }
                        dataReader.Close();
                    }
                }
                dataReader.Close();
                //=========================================Borrower category add to the list====================
                queryString = "select catName from borrowerSettings";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                ucBorrewerSetting1.dgvBrrCategory.Rows.Clear();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucBorrewerSetting1.dgvBrrCategory.Rows.Add(dataReader["catName"].ToString());
                    }
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "select catName from item_settings order by catName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                List<string> cateList = new List<string> { };
                CheckBox chkbButton = new CheckBox();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cateList.Add(dataReader["catName"].ToString());
                    }
                    dataReader.Close();
                    //================Remove panel control============
                    for (int i = 0; i < ucBorrewerSetting1.pnlChkb.Controls.Count; i++)
                    {
                        Control pnlControl = ucBorrewerSetting1.pnlChkb.Controls[i];
                        if (pnlControl is CheckBox)
                        {
                            ucBorrewerSetting1.pnlChkb.Controls.Remove(pnlControl);
                            pnlControl.Dispose();
                            i--;
                        }
                    }
                    
                    chkbButton.Name = "All";
                    chkbButton.Text = "All";
                    chkbButton.CheckedChanged += new EventHandler(ucBorrewerSetting1.CheckBox_CheckedChanged);
                    chkbButton.Location = new Point(2, ucBorrewerSetting1.pnlChkb.Controls.Count * 25);
                    ucBorrewerSetting1.pnlChkb.Controls.Add(chkbButton);
                }
                dataReader.Close();
               
                foreach (string catName in cateList)
                {
                    queryString = "select subCatName from item_subcategory where catName =@catName and notReference='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", catName);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        int i = 0;
                        while (dataReader.Read())
                        {
                            chkbButton = new CheckBox();
                            chkbButton.Name = i.ToString();
                            chkbButton.Width = 200;
                            chkbButton.Text = catName + "(" + dataReader["subCatName"] + ")";
                            chkbButton.CheckedChanged += new EventHandler(ucBorrewerSetting1.CheckBox_CheckedChanged);
                            chkbButton.Location = new Point(2, ucBorrewerSetting1.pnlChkb.Controls.Count * 25);
                            ucBorrewerSetting1.pnlChkb.Controls.Add(chkbButton);
                            i++;
                        }
                    }
                    dataReader.Close();
                }

                queryString = "select catName from borrower_settings order by catName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                ucBorrewerSetting1.dgvBrrCategory.Rows.Clear();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucBorrewerSetting1.dgvBrrCategory.Rows.Add(dataReader["catName"].ToString());
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }

            ucBorrewerSetting1.btnSave.Text = "Sa&ve";
            ucBorrewerSetting1.dgvBrrCategory.ClearSelection();
            ucAddBorrower1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucItemSettings1.Visible = false;
            ucItemIssue1.Visible = false;
            ucItemReissueReturn1.Visible = false;
            ucBorrewerSetting1.Visible = true;
            ucBorrewerSetting1.UcBorrewerSetting_Load(null, null);

            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
        }

        private void addBorrower_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnAddMember.BackColor = Color.FromArgb(54, 69, 79);

            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);

            ucAddBorrower1.dgvBrrDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucAddBorrower1.dgvBrrDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            ucAddBorrower1.cmbPlan.Items.Clear();
            ucAddBorrower1.cmbPlan.Items.Add("--Select--");

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============================Borrower category add to combobox======================
              
                string queryString = "select membrshpName from mbershipSetting";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucAddBorrower1.cmbPlan.Items.Add(dataReader["membrshpName"].ToString());
                    }
                }
                dataReader.Close();

                queryString = "select catName from borrowerSettings order by catName asc";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    ucAddBorrower1.cmbBrrCategory.Items.Clear();
                    ucAddBorrower1.cmbBrrCategory.Items.Add("Please select a Category...");
                    while (dataReader.Read())
                    {
                        ucAddBorrower1.cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
                    }
                    ucAddBorrower1.cmbBrrCategory.SelectedIndex = 0;
                    dataReader.Close();

                    ucAddBorrower1.cmbSearchBy.SelectedIndex = 0;
                    ucAddBorrower1.btnSave.Text = "Sa&ve";
                    ucBorrewerSetting1.Visible = false;
                    ucItemNormalEntry1.Visible = false;
                    ucItemSettings1.Visible = false;
                    ucItemIssue1.Visible = false;
                    ucItemReissueReturn1.Visible = false;
                    ucAddBorrower1.Visible = true;
                    Application.DoEvents();
                    timer3.Start();
                    ucAddBorrower1.UcAddBorrower_Load(null, null);

                    queryString = "select * from brrIdSetting";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        queryString = "insert into brrIdSetting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + false + "','','','','" + 0 + "');";
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.ExecuteNonQuery();
                    }
                    else
                    {
                        ucAddBorrower1.txtbBrrId.Enabled = false;
                    }
                    dataReader.Close();
                }
                else
                {
                    borrowerSetting_Click(null, null);
                    MessageBox.Show("To add borrower.You need to add some borrower category." + Environment.NewLine + "(Like - Student, Staff, Member etc.)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "select membrshpName from mbership_setting";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucAddBorrower1.cmbPlan.Items.Add(dataReader["membrshpName"].ToString());
                    }
                }
                dataReader.Close();

                bool borrowerSetting = false;
                queryString = "select catName from borrower_settings order by catName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if(dataReader.HasRows)
                {
                    borrowerSetting = true;
                    ucAddBorrower1.cmbBrrCategory.Items.Clear();
                    ucAddBorrower1.cmbBrrCategory.Items.Add("Please select a Category...");
                    while (dataReader.Read())
                    {
                        ucAddBorrower1.cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
                    }
                    ucAddBorrower1.cmbBrrCategory.SelectedIndex = 0;
                    dataReader.Close();
                }
               
                if (borrowerSetting)
                {
                    ucAddBorrower1.cmbSearchBy.SelectedIndex = 0;
                    ucAddBorrower1.btnSave.Text = "Sa&ve";
                    ucBorrewerSetting1.Visible = false;
                    ucItemNormalEntry1.Visible = false;
                    ucItemSettings1.Visible = false;
                    ucItemIssue1.Visible = false;
                    ucItemReissueReturn1.Visible = false;
                    ucAddBorrower1.Visible = true;
                    Application.DoEvents();
                    timer3.Start();
                    ucAddBorrower1.UcAddBorrower_Load(null, null);

                    queryString = "select * from brrid_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if(!dataReader.HasRows)
                    {
                        dataReader.Close();
                        queryString = "insert into brrid_setting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + false + "','','','','" + 0 + "');";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        ucAddBorrower1.txtbBrrId.Enabled = false;
                        dataReader.Close();
                    }
                    mysqlConn.Close();
                }
                else
                {
                    mysqlConn.Close();
                    borrowerSetting_Click(null, null);
                    MessageBox.Show("To add borrower.You need to add some borrower category." + Environment.NewLine + "(Like - Student, Staff, Member etc.)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
        }

        private void normalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnAddBooks.BackColor = Color.FromArgb(54, 69, 79);

            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);

            ucItemNormalEntry1.dgvItemDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemNormalEntry1.dgvItemDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            ucItemNormalEntry1.cmbItemCategory.Items.Clear();
            ucItemNormalEntry1.cmbItemCategory.Items.Add("Please select a Category...");

            if (Properties.Settings.Default.sqliteDatabase)
            { 
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //======================Item Category add to combobox============
                string queryString = "select catName from itemSettings order by catName asc";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucItemNormalEntry1.cmbItemCategory.Items.Add(dataReader["catName"].ToString());
                    }
                    dataReader.Close();

                    ucItemNormalEntry1.cmbItemCategory.SelectedIndex = 0;
                    ucItemNormalEntry1.cmbSearchBy.Text = "Please choose a type...";
                    ucItemNormalEntry1.btnSave.Text = "Sa&ve";
                    ucBorrewerSetting1.Visible = false;
                    ucItemSettings1.Visible = false;
                    ucItemIssue1.Visible = false;
                    ucAddBorrower1.Visible = false;
                    ucItemReissueReturn1.Visible = false;
                    ucItemNormalEntry1.Visible = true;
                    Application.DoEvents();
                    timer3.Start();
                    ucItemNormalEntry1.UcItemNormalEntry_Load(null, null);
                    Application.DoEvents();
                    if (ucItemNormalEntry1.backgroundWorker1.IsBusy)
                    {
                        ucItemNormalEntry1.backgroundWorker1.CancelAsync();
                        ucItemNormalEntry1.backgroundWorker1.Dispose();
                    }
                    ucItemNormalEntry1.backgroundWorker1.RunWorkerAsync();

                    queryString = "select * from accnSetting";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        queryString = "insert into accnSetting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + false + "','','','','" + 0 + "');";
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.ExecuteNonQuery();
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
            }
            else
            {
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
                string queryString = "select catName from item_settings order by catName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                bool itemsettingExist = false;
                if (dataReader.HasRows)
                {
                    itemsettingExist = true;
                    while (dataReader.Read())
                    {
                        ucItemNormalEntry1.cmbItemCategory.Items.Add(dataReader["catName"].ToString());
                    }
                }
                dataReader.Close();
                if(itemsettingExist)
                {
                    ucItemNormalEntry1.cmbItemCategory.SelectedIndex = 0;
                    ucItemNormalEntry1.cmbSearchBy.Text = "Please choose a type...";
                    ucItemNormalEntry1.btnSave.Text = "Sa&ve";
                    ucBorrewerSetting1.Visible = false;
                    ucItemSettings1.Visible = false;
                    ucItemIssue1.Visible = false;
                    ucAddBorrower1.Visible = false;
                    ucItemReissueReturn1.Visible = false;
                    ucItemNormalEntry1.Visible = true;
                    Application.DoEvents();
                    timer3.Start();
                    ucItemNormalEntry1.UcItemNormalEntry_Load(null, null);
                    Application.DoEvents();
                    if (ucItemNormalEntry1.backgroundWorker1.IsBusy)
                    {
                        ucItemNormalEntry1.backgroundWorker1.CancelAsync();
                        ucItemNormalEntry1.backgroundWorker1.Dispose();
                    }
                    ucItemNormalEntry1.backgroundWorker1.RunWorkerAsync();

                    queryString = "select * from accn_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if(!dataReader.HasRows)
                    {
                        dataReader.Close();
                        queryString = "insert into accn_setting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + false + "','','','','" + 0 + "');";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.ExecuteNonQuery();
                    }
                }
                mysqlConn.Close();
            }
            ucItemNormalEntry1.cmbItemCategory.Select();
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
        }

        private void itemSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ucItemSettings1.dgvItemCategory.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemSettings1.dgvItemCategory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            ucItemSettings1.dgvItemSubcat.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemSettings1.dgvItemSubcat.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            ucItemSettings1.dgvItemCategory.Rows.Clear();

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                //============================Item catery add to the list===================
                string queryString = "select catName from itemSettings order by catName asc";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucItemSettings1.dgvItemCategory.Rows.Add(dataReader["catName"].ToString());
                    }
                    dataReader.Close();
                }
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "select catName from item_settings order by catName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ucItemSettings1.dgvItemCategory.Rows.Add(dataReader["catName"].ToString());
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            ucItemSettings1.dgvItemCategory.ClearSelection();

            ucItemSettings1.btnSave.Text = "Sa&ve";
            ucBorrewerSetting1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucAddBorrower1.Visible = false;
            ucItemIssue1.Visible = false;
            ucItemReissueReturn1.Visible = false;
            ucItemSettings1.Visible = true;
            ucItemSettings1.UcItemSettings_Load(null, null);

            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
        }

        private void IssueBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnIssue.BackColor = Color.FromArgb(54, 69, 79);

            ucItemIssue1.dgvIssuedBook.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemIssue1.dgvIssuedBook.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            ucItemIssue1.dgvBook.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemIssue1.dgvBook.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            ucItemIssue1.UcItemIssue_Load(null, null);
            ucItemIssue1.timerBorrowerId.Start();

            ucItemSettings1.btnSave.Text = "Sa&ve";
            ucItemIssue1.txtbBrrId.Clear();
            ucItemIssue1.txtbAccn.Clear();
            ucItemIssue1.txtbIsbn.Clear();
            ucItemIssue1.numDay.Value = 0;
            ucBorrewerSetting1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucAddBorrower1.Visible = false;
            ucItemSettings1.Visible = false;
            ucItemReissueReturn1.Visible = false;
            ucItemIssue1.Visible = true;
            Application.DoEvents();
            ucItemIssue1.rdbAccn.Checked = false;
            ucItemIssue1.rdbIsbn.Checked = false;

            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
        }

        private void ReturnReissueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnReturn.BackColor = Color.FromArgb(54, 69, 79);
            ucItemReissueReturn1.dgvBook.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            ucItemReissueReturn1.dgvBook.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            ucItemReissueReturn1.UcItemReissueReturn_Load(null, null);

            ucItemReissueReturn1.timerAccession.Start();
            ucItemReissueReturn1.timerIsbn.Start();

            ucBorrewerSetting1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucAddBorrower1.Visible = false;
            ucItemSettings1.Visible = false;
            ucItemIssue1.Visible = false;
            ucItemReissueReturn1.Visible = true;
            Application.DoEvents();

            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
        }

        private void DashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnDashboard.BackColor = Color.FromArgb(54, 69, 79);

            if (Properties.Settings.Default.notificationData == "")
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
            ucBorrewerSetting1.Visible = false;
            ucAddBorrower1.Visible = false;
            ucItemSettings1.Visible = false;
            ucItemNormalEntry1.Visible = false;
            ucItemIssue1.Visible = false;
            ucItemReissueReturn1.Visible = false;
            Application.DoEvents();
            timerDrawChart.Start();

            if (reserveItems == null || !reserveItems.IsAlive)
            {
                reserveItems = new Thread(new ThreadStart(loadReservation));
                reserveItems.Priority = ThreadPriority.Lowest;
                reserveItems.IsBackground = true;
                reserveItems.Start();
            }

            if (dueItems == null || !dueItems.IsAlive)
            {
                dueItems = new Thread(new ThreadStart(loadDueItems));
                dueItems.Priority = ThreadPriority.Lowest;
                dueItems.IsBackground = true;
                dueItems.Start();
            }

            this.Text = Application.ProductName + " (v" + Application.ProductVersion + ")";
            
            string compareString = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from issuedItem where issueDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblTBIssued.Text = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select count(id) from issuedItem where returnDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblTBReturn.Text = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select count(id) from itemDetails where entryDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblItemAdded.Text = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select count(distinct brrId) from issuedItem where issueDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblTMIssued.Text = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select count(distinct brrId) from issuedItem where returnDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblTMReturn.Text = sqltCommnd.ExecuteScalar().ToString();

                sqltCommnd.CommandText = "select count(id) from borrowerDetails where entryDate = '" + compareString + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblMembrAdded.Text = sqltCommnd.ExecuteScalar().ToString();
                Application.DoEvents();

                sqltConn.Close();
            }
            else
            {
                try
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select count(id) from issued_item where issueDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblTBIssued.Text = mysqlCmd.ExecuteScalar().ToString();

                    queryString = "select count(id) from issued_item where returnDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblTBReturn.Text = mysqlCmd.ExecuteScalar().ToString();

                    queryString = "select count(id) from item_details where entryDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblItemAdded.Text = mysqlCmd.ExecuteScalar().ToString();

                    queryString = "select count(distinct brrId) from issued_item where issueDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblTMIssued.Text = mysqlCmd.ExecuteScalar().ToString();

                    queryString = "select count(distinct brrId) from issued_item where returnDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblTMReturn.Text = mysqlCmd.ExecuteScalar().ToString();

                    queryString = "select count(id) from borrower_details where entryDate = '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lblMembrAdded.Text = mysqlCmd.ExecuteScalar().ToString();
                    Application.DoEvents();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void loadDueItems()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    DateTime currentDate = DateTime.Now.Date, expDate = DateTime.Now.Date;
                    dgvDueBooks.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
                    dgvDueBooks.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    dgvDueBooks.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvDueBooks.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvDueBooks.Rows.Clear();
                    int daysLate = 0;
                    string exprDate = "";

                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        //sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails inner join borrowerDetails " +
                        //"where issuedItem.itemAccession=itemDetails.itemAccession and issuedItem.brrId=borrowerDetails.brrId and issuedItem.itemReturned='" + false + "'";

                        string compareDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");
                        string queryString = "select * from issuedItem inner join itemDetails inner join borrowerDetails " +
                        "where issuedItem.itemAccession=itemDetails.itemAccession and issuedItem.brrId=borrowerDetails.brrId and " +
                        "issuedItem.itemReturned='" + false + "' and (substr(issuedItem.expectedReturnDate,7,4) || " +
                        "substr(issuedItem.expectedReturnDate,4,2) || substr(issuedItem.expectedReturnDate,1,2) < '" + compareDate + "')";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                expDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                daysLate = Convert.ToInt32((currentDate - expDate).TotalDays);
                                exprDate = expDate.Day.ToString("00") + "/" + expDate.Month.ToString("00") + "/" + expDate.Year.ToString("0000");

                                dgvDueBooks.Rows.Add(dgvDueBooks.Rows.Count + 1, dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),FormatDate.getUserFormat(exprDate), daysLate.ToString());
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();

                        dgvDueBooks.ClearSelection();
                        sqltConn.Close();
                    }
                    else
                    {
                        try
                        {
                            mysqlConn = ConnectionClass.mysqlConnection();
                            if (mysqlConn.State == ConnectionState.Closed)
                            {
                                mysqlConn.Open();
                            }
                            MySqlCommand mysqlCmd;
                            
                            string compareDate = DateTime.Now.Day.ToString("00")+"/" + DateTime.Now.Month.ToString("00")+"/" + DateTime.Now.Year.ToString("0000");
                            string queryString = "select * from issued_item inner join item_details inner join borrower_details " +
                            "where issued_item.itemAccession=item_details.itemAccession and issued_item.brrId=borrower_details.brrId and " +
                            "issued_item.itemReturned='" + false + "' and str_to_date(issued_item.expectedReturnDate, '%d/%m/%Y') < str_to_date('"+compareDate+"', '%d/%m/%Y')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    expDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    daysLate = Convert.ToInt32((currentDate - expDate).TotalDays);

                                    dgvDueBooks.Rows.Add(dgvDueBooks.Rows.Count + 1, dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()), daysLate.ToString());
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                            dgvDueBooks.ClearSelection();
                        }
                        catch(Exception ex)
                        {
                           
                        }
                    }

                    jsonString = Properties.Settings.Default.notificationData;
                    if (jsonString != "")
                    {
                        jsonObj = JObject.Parse(jsonString);
                        if (jsonObj["DueNotificationDate"].ToString() == DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" +
                            DateTime.Now.Year.ToString("0000"))
                        {
                            if (Convert.ToBoolean(jsonObj["DueMail"].ToString()) || Convert.ToBoolean(jsonObj["DueSms"].ToString()))
                            {
                                bWorkerNotification.RunWorkerAsync();
                            }
                        }
                    }
                    try
                    {
                        IntPtr hMenu = GetSystemMenu(this.Handle, false);
                        EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                    }
                    catch
                    {

                    }
                }));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void loadReservation()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    dgvReservationList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
                    dgvReservationList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    dgvReservationList.Rows.Clear();
                    DateTime availabilityDate = DateTime.Now.Date;
                    dgvReservationList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvReservationList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvReservationList.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        sqltCommnd.CommandText = "select * from reservationList inner join borrowerDetails " +
                        "where reservationList.brrId=borrowerDetails.brrId";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            SQLiteCommand sqltCmd1 = sqltConn.CreateCommand();
                            SQLiteDataReader dataReader1 = null;

                            while (dataReader.Read())
                            {
                                if (dataReader["itemAccn"].ToString() != "")
                                {
                                    sqltCmd1.CommandText = "select rackNo from itemDetails where itemAccession=@itemAccn";
                                    sqltCmd1.CommandType = CommandType.Text;
                                    sqltCmd1.Parameters.AddWithValue("@itemAccn", dataReader["itemAccn"].ToString());
                                    dataReader1 = sqltCmd1.ExecuteReader();
                                    if (dataReader1.HasRows)
                                    {
                                        while (dataReader1.Read())
                                        {
                                            dgvReservationList.Rows.Add(dgvReservationList.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reserveDate"].ToString()),
                                           dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")", dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(),
                                           dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString(), dataReader1["rackNo"].ToString(), FormatDate.getUserFormat(dataReader["availableDate"].ToString()));
                                        }
                                    }
                                    dataReader1.Close();
                                }
                                else
                                {
                                    dgvReservationList.Rows.Add(dgvReservationList.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reserveDate"].ToString()),
                                     dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")", dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(),
                                     dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString(), "", FormatDate.getUserFormat(dataReader["availableDate"].ToString()));
                                }

                                if (dataReader["availableDate"].ToString() != "")
                                {
                                    availabilityDate = DateTime.ParseExact(dataReader["availableDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if ((DateTime.Now.Date - availabilityDate).TotalDays > Properties.Settings.Default.reserveDay - 1)
                                    {
                                        dgvReservationList.Rows[dgvReservationList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        Application.DoEvents();
                        dgvReservationList.ClearSelection();
                        sqltConn.Close();
                    }
                    else
                    {
                        mysqlConn = ConnectionClass.mysqlConnection();
                        if (mysqlConn.State == ConnectionState.Closed)
                        {
                            mysqlConn.Open();
                        }
                        MySqlCommand mysqlCmd;

                        MySqlConnection mysqlConn1;
                        mysqlConn1 = ConnectionClass.mysqlConnection();
                        if (mysqlConn1.State == ConnectionState.Closed)
                        {
                            mysqlConn1.Open();
                        }
                        MySqlCommand mysqlCmd1;
                        MySqlDataReader dataReader1;
                        string queryString1;

                        string queryString = "select * from reservation_list inner join borrower_details " +
                        "where reservation_list.brrId=borrower_details.brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["itemAccn"].ToString() != "")
                                {
                                    queryString1 = "select rackNo from item_details where itemAccession=@itemAccn";
                                    mysqlCmd1 = new MySqlCommand(queryString1, mysqlConn1);
                                    mysqlCmd1.Parameters.AddWithValue("@itemAccn", dataReader["itemAccn"].ToString());
                                    mysqlCmd1.CommandTimeout = 99999;
                                    dataReader1 = mysqlCmd1.ExecuteReader();
                                    if (dataReader1.HasRows)
                                    {
                                        while (dataReader1.Read())
                                        {
                                            dgvReservationList.Rows.Add(dgvReservationList.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reserveDate"].ToString()),
                                           dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")", dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(),
                                           dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString(), dataReader1["rackNo"].ToString(), FormatDate.getUserFormat(dataReader["availableDate"].ToString()));
                                        }
                                    }
                                    dataReader1.Close();
                                }
                                else
                                {
                                    dgvReservationList.Rows.Add(dgvReservationList.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reserveDate"].ToString()),
                                     dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")", dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(),
                                     dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString(), "", FormatDate.getUserFormat(dataReader["availableDate"].ToString()));
                                }

                                if (dataReader["availableDate"].ToString() != "")
                                {
                                    availabilityDate = DateTime.ParseExact(dataReader["availableDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if ((DateTime.Now.Date - availabilityDate).TotalDays > Properties.Settings.Default.reserveDay - 1)
                                    {
                                        dgvReservationList.Rows[dgvReservationList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        mysqlConn1.Close();
                        mysqlConn.Close();
                        Application.DoEvents();
                        dgvReservationList.ClearSelection();
                    }
                }));
            }
            catch
            {

            }
        }

        private void overAllChart()
        {
            try
            {
                int totalBooks = 0, totalType = 0, damageBooks = 0, lostBooks = 0, totalIssued = 0,
                     totalReturn = 0;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count(id) from itemDetails";// where word = '" + word + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select distinct itemTitle,itemAuthor from itemDetails";
                    sqltCommnd.CommandType = CommandType.Text;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    List<string> itemList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader
                                    select (string)r["itemTitle"]).ToList();
                    }
                    dataReader.Close();
                    totalType = itemList.Count;

                    sqltCommnd.CommandText = "select count(id) from itemDetails where isDamage = '" + true + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    damageBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select count(id) from itemDetails where isLost = '" + true + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    lostBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select count(id) from issuedItem";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select count(id) from issuedItem where itemReturned = '" + true + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalReturn = Convert.ToInt32(sqltCommnd.ExecuteScalar());
                    sqltConn.Close();
                }
                else
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select count(id) from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select distinct itemTitle,itemAuthor from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    List<string> itemList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader
                                    select (string)r["itemTitle"]).ToList();
                    }
                    dataReader.Close();
                    totalType = itemList.Count;

                    queryString = "select count(id) from item_details where isDamage = '" + true + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    damageBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select count(id) from item_details where isLost = '" + true + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    lostBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select count(id) from issued_item";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select count(id) from issued_item where itemReturned = '" + true + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalReturn = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    mysqlConn.Close();
                }

                pieChartTotal.Titles.Clear();
                Title chartTitle = new Title("Items Record", Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                pieChartTotal.Titles.Add(chartTitle);

                pieChartTotal.Series["Series1"].Points.Clear();
                if (totalBooks == 0)
                {
                    pieChartTotal.Series["Series1"].Points.Add(100);
                }
                else
                {
                    pieChartTotal.Series["Series1"].Points.Add(totalBooks);
                }
                //pieChartTotal.Series["Series1"].Points[0].Color = Color.SkyBlue;
                pieChartTotal.Series["Series1"].Points[0].LegendText = "Total Items - " + totalBooks.ToString();
                //pieChartTotal.Series["Series1"].Points[0].Label = totalBooks.ToString();
                pieChartTotal.Legends[0].Alignment = StringAlignment.Center;

                pieChartTotal.Series["Series1"].Points.Add(totalType);
                pieChartTotal.Series["Series1"].Points[1].LegendText = "Total Type of Title - " + totalType.ToString();

                pieChartTotal.Series["Series1"].Points.Add(lostBooks);
                pieChartTotal.Series["Series1"].Points[2].LegendText = "Items Lost - " + lostBooks.ToString();

                pieChartTotal.Series["Series1"].Points.Add(damageBooks);
                pieChartTotal.Series["Series1"].Points[3].LegendText = "Items Damage - " + damageBooks.ToString();

                pieChartTotal.Series["Series1"].Points.Add(totalIssued);
                pieChartTotal.Series["Series1"].Points[4].LegendText = "Items Issued - " + totalIssued.ToString();

                pieChartTotal.Series["Series1"].Points.Add(totalReturn);
                pieChartTotal.Series["Series1"].Points[5].LegendText = "Items Returned - " + totalReturn.ToString();
            }
            catch
            {

            }
        }

        private void monthlyChart()
        {
            try
            {
                int totalAdded = 0, totalIssued = 0, totalReturn = 0;
                string compareString = "%/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    sqltCommnd.CommandText = "select count(id) from itemDetails where entryDate like '" + compareString + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalAdded = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select count(id) from issuedItem where issueDate like '" + compareString + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar());

                    sqltCommnd.CommandText = "select count(id) from issuedItem where returnDate like '" + compareString + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    totalReturn = Convert.ToInt32(sqltCommnd.ExecuteScalar());
                    sqltConn.Close();
                }
                else
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select count(id) from item_details where entryDate like '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalAdded = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select count(id) from issued_item where issueDate like '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    queryString = "select count(id) from issued_item where returnDate like '" + compareString + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    totalReturn = Convert.ToInt32(mysqlCmd.ExecuteScalar());

                    mysqlConn.Close();
                }

                pieChartcurrentMonth.Titles.Clear();
                Title chartTitle = new Title("Items Record of " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                pieChartcurrentMonth.Titles.Add(chartTitle);

                pieChartcurrentMonth.Series["Series1"].Points.Clear();
                if (totalAdded == 0 && totalIssued == 0 && totalReturn == 0)
                {
                    pieChartcurrentMonth.Series["Series1"].Points.Add(100);
                }
                else
                {
                    pieChartcurrentMonth.Series["Series1"].Points.Add(totalAdded);
                }
                pieChartcurrentMonth.Series["Series1"].Points[0].Color = Color.SkyBlue;
                pieChartcurrentMonth.Series["Series1"].Points[0].LegendText = "Items Added - " + totalAdded.ToString();
                pieChartcurrentMonth.Legends[0].Alignment = StringAlignment.Center;

                pieChartcurrentMonth.Series["Series1"].Points.Add(totalIssued);
                pieChartcurrentMonth.Series["Series1"].Points[1].Color = Color.Orange;
                pieChartcurrentMonth.Series["Series1"].Points[1].LegendText = "Items Issued - " + totalIssued.ToString();

                pieChartcurrentMonth.Series["Series1"].Points.Add(totalReturn);
                pieChartcurrentMonth.Series["Series1"].Points[2].Color = Color.LightGray;
                pieChartcurrentMonth.Series["Series1"].Points[2].LegendText = "Items Returned - " + totalReturn.ToString();
            }
            catch
            {

            }
        }

        public void blankChart()
        {
            int totalBooks = 0, totalType = 0, lostBooks = 0, damageBooks = 0, totalIssued = 0, totalReturn = 0, totalAdded = 0;

            pieChartTotal.Titles.Clear();
            Title chartTitle = new Title("Items Record", Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
            pieChartTotal.Titles.Add(chartTitle);

            pieChartTotal.Series["Series1"].Points.Clear();
            if (totalBooks == 0)
            {
                pieChartTotal.Series["Series1"].Points.Add(100);
            }
            else
            {
                pieChartTotal.Series["Series1"].Points.Add(totalBooks);
            }
            //pieChartTotal.Series["Series1"].Points[0].Color = Color.SkyBlue;
            pieChartTotal.Series["Series1"].Points[0].LegendText = "Total Items - " + totalBooks.ToString();
            //pieChartTotal.Series["Series1"].Points[0].Label = totalBooks.ToString();
            pieChartTotal.Legends[0].Alignment = StringAlignment.Center;

            pieChartTotal.Series["Series1"].Points.Add(totalType);
            pieChartTotal.Series["Series1"].Points[1].LegendText = "Total Type of Title - " + totalType.ToString();

            pieChartTotal.Series["Series1"].Points.Add(lostBooks);
            pieChartTotal.Series["Series1"].Points[2].LegendText = "Items Lost - " + lostBooks.ToString();

            pieChartTotal.Series["Series1"].Points.Add(damageBooks);
            pieChartTotal.Series["Series1"].Points[3].LegendText = "Items Damage - " + damageBooks.ToString();

            pieChartTotal.Series["Series1"].Points.Add(totalIssued);
            pieChartTotal.Series["Series1"].Points[4].LegendText = "Items Issued - " + totalIssued.ToString();

            pieChartTotal.Series["Series1"].Points.Add(totalReturn);
            pieChartTotal.Series["Series1"].Points[5].LegendText = "Items Returned - " + totalReturn.ToString();
           
            pieChartcurrentMonth.Titles.Clear();
            chartTitle = new Title("Items Record of " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
            pieChartcurrentMonth.Titles.Add(chartTitle);

            pieChartcurrentMonth.Series["Series1"].Points.Clear();
            if (totalAdded == 0 && totalIssued == 0 && totalReturn == 0)
            {
                pieChartcurrentMonth.Series["Series1"].Points.Add(100);
            }
            else
            {
                pieChartcurrentMonth.Series["Series1"].Points.Add(totalAdded);
            }
            pieChartcurrentMonth.Series["Series1"].Points[0].Color = Color.SkyBlue;
            pieChartcurrentMonth.Series["Series1"].Points[0].LegendText = "Items Added - " + totalAdded.ToString();
            pieChartcurrentMonth.Legends[0].Alignment = StringAlignment.Center;

            pieChartcurrentMonth.Series["Series1"].Points.Add(totalIssued);
            pieChartcurrentMonth.Series["Series1"].Points[1].Color = Color.Orange;
            pieChartcurrentMonth.Series["Series1"].Points[1].LegendText = "Items Issued - " + totalIssued.ToString();

            pieChartcurrentMonth.Series["Series1"].Points.Add(totalReturn);
            pieChartcurrentMonth.Series["Series1"].Points[2].Color = Color.LightGray;
            pieChartcurrentMonth.Series["Series1"].Points[2].LegendText = "Items Returned - " + totalReturn.ToString();
        }

        private void getExcelFile(string fileName)
        {
            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

            //Create COM Objects. Create a COM object for everything that is referenced
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileName);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 2; i <= rowCount; i++)
            {
                //MessageBox.Show(xlRange.Cells[i, 2].Value2.ToString());
                string queryString = "insert into countryDetails (countryName,currencyName,cuurShort,currSymbol,dialCode) values (@countryName,@currencyName,@cuurShort,@currSymbol,@dialCode)";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@countryName", xlRange.Cells[i, 2].Value2.ToString());
                sqltCommnd.Parameters.AddWithValue("@currencyName", xlRange.Cells[i, 5].Value2.ToString());
                sqltCommnd.Parameters.AddWithValue("@cuurShort", xlRange.Cells[i, 6].Value2.ToString());
                sqltCommnd.Parameters.AddWithValue("@currSymbol", xlRange.Cells[i, 7].Value2.ToString());
                sqltCommnd.Parameters.AddWithValue("@dialCode", "+"+xlRange.Cells[i, 4].Value2.ToString());
                sqltCommnd.ExecuteNonQuery();
                label1.Text = i.ToString();
                Application.DoEvents();
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
            sqltConn.Close();
        }

        private void printBarcodeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormItemBarCode barCodePrint = new FormItemBarCode();
            barCodePrint.ShowDialog();
        }

        private void printIdCardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormBorrowerId idCardPrint = new FormBorrowerId();
            idCardPrint.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(lblInstName.Text== "DEMO COLLEGE")
            {
                //DatabaseChecking.CreerBase();
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select instName,instLogo,instAddress,instShortName,currSymbol," +
                        "databasePath,backupPath,backupHour,printerName,opacAvailable,notificationData,settingsData from generalSettings", settingsData = "";
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            globalVarLms.instName = dataReader["instName"].ToString();
                            lblInstName.Text = dataReader["instName"].ToString();
                            globalVarLms.instAddress = dataReader["instAddress"].ToString();
                            lblInstAddress.Text = dataReader["instAddress"].ToString();
                            globalVarLms.instShortName = dataReader["instShortName"].ToString();
                            globalVarLms.currSymbol = dataReader["currSymbol"].ToString();
                            globalVarLms.backupPath = dataReader["backupPath"].ToString();

                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbLogo.Image = Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbLogo.Image = Properties.Resources.your_logo;
                            }
                            pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                            globalVarLms.defaultPrinter = dataReader["printerName"].ToString();
                            Properties.Settings.Default.notificationData = dataReader["notificationData"].ToString();
                            Properties.Settings.Default.Save();
                            settingsData = dataReader["settingsData"].ToString();
                        }
                        dataReader.Close();
                        if (settingsData != "")
                        {
                            jsonObj = JObject.Parse(settingsData);
                            loadSettings(jsonObj);
                        }
                    }
                    sqltConn.Close();
                }
                else
                {
                    try
                    {
                        mysqlConn = ConnectionClass.mysqlConnection();
                        if (mysqlConn.State == ConnectionState.Closed)
                        {
                            mysqlConn.Open();
                        }
                        MySqlCommand mysqlCmd;
                        string queryString = queryString = "select instName,instLogo,instAddress,instShortName,currSymbol," +
                            "databasePath,backupPath,backupHour,printerName,opacAvailable,notificationData,settingsData from general_settings", settingsData = "";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                globalVarLms.instName = dataReader["instName"].ToString();
                                lblInstName.Text = dataReader["instName"].ToString();
                                globalVarLms.instAddress = dataReader["instAddress"].ToString();
                                lblInstAddress.Text = dataReader["instAddress"].ToString();
                                globalVarLms.instShortName = dataReader["instShortName"].ToString();
                                globalVarLms.currSymbol = dataReader["currSymbol"].ToString();
                                globalVarLms.backupPath = dataReader["backupPath"].ToString();

                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbLogo.Image = Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbLogo.Image = Properties.Resources.your_logo;
                                }
                                pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                                globalVarLms.defaultPrinter = dataReader["printerName"].ToString();
                                Properties.Settings.Default.notificationData = dataReader["notificationData"].ToString();
                                Properties.Settings.Default.Save();
                                settingsData = dataReader["settingsData"].ToString();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                        if (settingsData != "")
                        {
                            jsonObj = JObject.Parse(settingsData);
                            loadSettings(jsonObj);
                        }
                    }
                    catch
                    {

                    }
                }
                Application.DoEvents();
            }
            if (globalVarLms.instName != "")
            {
                label9.Text = "";
                label14.Text = "";
            }
            if (globalVarLms.licenseType != "Demo")
            {
                if (this.Text != Application.ProductName + " (v" + Application.ProductVersion + " - " + globalVarLms.licenseType + ")")
                {
                    this.Text = Application.ProductName + " (v" + Application.ProductVersion + " - " + globalVarLms.licenseType + ")";
                    pcbPremium.Visible = true;
                    Application.DoEvents();
                }
            }
            lblDateTime.Text =FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void generalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormGeneralSetting generalSetting = new FormGeneralSetting();
            generalSetting.ShowDialog();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select instName,instAddress,instLogo from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        lblInstName.Text = dataReader["instName"].ToString();
                        lblInstAddress.Text = dataReader["instAddress"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            pcbLogo.Image = Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            pcbLogo.Image = Properties.Resources.your_logo;
                        }
                        pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MySqlCommand mysqlCmd;
                string queryString = "select instName,instAddress,instLogo from general_settings";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        lblInstName.Text = dataReader["instName"].ToString();
                        lblInstAddress.Text = dataReader["instAddress"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            pcbLogo.Image = Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            pcbLogo.Image = Properties.Resources.your_logo;
                        }
                        pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void adminSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormAddLibrarian adminSetting = new FormAddLibrarian();
            adminSetting.ShowDialog();
        }

        private void databaseSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormDatabaseSetting databaseSetting = new FormDatabaseSetting();
            databaseSetting.ShowDialog();
            if (databaseSetting.isUpdate)
            {
                FormLogin loginPage = new FormLogin();
                loginPage.ShowDialog();
                DashboardToolStripMenuItem_Click(null, null);
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormChnagePassword changePassword = new FormChnagePassword();
            changePassword.ShowDialog();
        }

        private void timerDrawChart_Tick(object sender, EventArgs e)
        {
            timerDrawChart.Stop();
            overAllChart();
            Application.DoEvents();
            monthlyChart();
            Application.DoEvents();
        }

        private void borrowerRenewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormRenewMember renewForm = new FormRenewMember();
            renewForm.ShowDialog();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            FormActivate activateProduct = new FormActivate();
            activateProduct.ShowDialog();
            Application.Restart();
        }

        private void FormDashBoard_Activated(object sender, EventArgs e)
        {
            if (DashboardToolStripMenuItem.Enabled == false && ucItemSettings1.Visible == true)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select catName from itemSettings";
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        DashboardToolStripMenuItem.Enabled = true;
                        ItemsToolStripMenuItem.Enabled = true;
                        memberToolStripMenuItem.Enabled = true;
                        itemIssueToolStripMenuItem.Enabled = true;
                        moreToolStripMenuItem.Enabled = true;
                        helpMenu.Enabled = true;
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
                            //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select catName from item_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DashboardToolStripMenuItem.Enabled = true;
                        ItemsToolStripMenuItem.Enabled = true;
                        memberToolStripMenuItem.Enabled = true;
                        itemIssueToolStripMenuItem.Enabled = true;
                        moreToolStripMenuItem.Enabled = true;
                        helpMenu.Enabled = true;
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }

        private void FormDashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bWorkerUpdateDetails.RunWorkerAsync();
            }
            catch
            {

            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to close the application ? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    /* Cancel the Closing event from closing the form. */
                    e.Cancel = true;
                }
                else
                {
                    if (globalVarLms.backupRequired)
                    {
                        if (MessageBox.Show("New data is available for backup,"+Environment.NewLine+ "do you want to take the backup of the database now ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            bWorkerBackup.WorkerSupportsCancellation = true;
                            bWorkerBackup.RunWorkerAsync();
                            dataBackup.backupComplete = false;
                            dataBackup.getException = false;
                            dataBackup.ShowDialog();
                        }
                    }
                    foreach (var process in Process.GetProcessesByName(Application.ProductName))
                    {
                        process.Kill();
                    }
                    
                    /* Closing the form. */
                    e.Cancel = false;
                    //Application.Exit();
                }
            }
        }

        private void reportLostDamageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormLostDamage lostDamageForm = new FormLostDamage();
            lostDamageForm.ShowDialog();
        }

        private void bulkEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormItemEdit itemEdit = new FormItemEdit();
            itemEdit.ShowDialog();
        }

        private void editBorrowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormBorrowerEdit borrowerEdit = new FormBorrowerEdit();
            borrowerEdit.ShowDialog();
        }

        private void btnBuyNow_Click(object sender, EventArgs e)
        {
            if (btnBuyNow.Text == "Buy Now")
            {
                if (IsConnectedToInternet())
                {
                    Process.Start("http://codeachi.com/products/lms-license-options");
                }
                else
                {
                    MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (IsConnectedToInternet())
                {
                    Process.Start("http://codeachi.com/products/renew");
                }
                else
                {
                    MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout aboutForm = new FormAbout();
            aboutForm.ShowDialog();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!globalVarLms.isAdmin)
            {
                adminSettingToolStripMenuItem.Enabled = false;
                generalSettingToolStripMenuItem.Enabled = false;
                databaseSettingToolStripMenuItem.Enabled = false;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if(globalVarLms.addCateory== "brrCategory")
            {
                borrowerSetting_Click(null, null);
                timer3.Stop();
                globalVarLms.addCateory = "";
            }
            else if(globalVarLms.addCateory == "itemCategory")
            {
                itemSettingsToolStripMenuItem_Click(null, null);
                timer3.Stop();
                globalVarLms.addCateory = "";
            }
        }

        private void lnklblQuotation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormQuotation quotationForm = new FormQuotation();
            quotationForm.ShowDialog();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnReport.BackColor = Color.FromArgb(54, 69, 79);
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormReport reportForm = new FormReport();
            reportForm.StartPosition = FormStartPosition.Manual;
            reportForm.Location = new Point(this.Location.X + ((this.Width - reportForm.Width) / 2), this.Location.Y + 90);
            //if(this.WindowState==FormWindowState.Normal)
            //{
            //    reportForm.WindowState = FormWindowState.Normal;
            //}
            //else 
            if (this.WindowState == FormWindowState.Maximized)
            {
                reportForm.Height = 600;
                reportForm.pnlView.Height = 330;
            }
            reportForm.ShowDialog();
        }

        private void paymentToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormPayment paymentForm = new FormPayment();
            paymentForm.ShowDialog();
        }

        private void fineStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                MessageBox.Show("Please purchase any license to unlock this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Refresh();
            }

        }

        private void delaytoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                MessageBox.Show("Please purchase any license to unlock this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Refresh();
            }
        }

        private void memberDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            bool settingExist = false;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============================Borrower category add to combobox======================
                string queryString = "select catName from borrowerSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if(dataReader.HasRows)
                {
                    settingExist = true;
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "select catName from borrower_settings";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    settingExist = true;
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            if (!settingExist)
            {
                borrowerSetting_Click(null, null);
                MessageBox.Show("To add borrower.You need to add some borrower category." + Environment.NewLine + "(Like - Student, Staff, Member etc.)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Type officeType = Type.GetTypeFromProgID("Excel.Application");
                if (officeType == null)
                {
                    MessageBox.Show("Microsoft Excel is not installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FormImportBorrower importBorrower = new FormImportBorrower();
                importBorrower.ShowDialog();
            }
        }

        private void itemDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }

            int ttlBooks = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //======================Item Category add to combobox============

                sqltCommnd.CommandText = "select count(id) from itemDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                ttlBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
            }
            else
            {
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
                string queryString = "select count(id) from item_details;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                ttlBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                mysqlConn.Close();
            }
            if (ttlBooks >= globalVarLms.itemLimits)
            {
                MessageBox.Show("You can't add more than " + globalVarLms.itemLimits.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Microsoft Excel is not installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormImportItems itemImport = new FormImportItems();
            itemImport.ShowDialog();
        }

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (globalVarLms.licenseType == "Demo")
            //{
            //    if (globalVarLms.currentDate > globalVarLms.expiryDate)
            //    {
            //        globalVarLms.productExpire = true;
            //        FormNotification notificationForm = new FormNotification();
            //        notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
            //        notificationForm.lnklblRenew.Visible = false;
            //        notificationForm.ShowDialog();
            //    }
            //}
            //else
            //{
            //    if (globalVarLms.currentDate > globalVarLms.expiryDate)
            //    {
            //        globalVarLms.productExpire = true;
            //        btnBuyNow.Text = "Renew Now";
            //        Application.DoEvents();
            //        FormNotification notificationForm = new FormNotification();
            //        notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
            //        notificationForm.lnklblQuotation.Visible = false;
            //        notificationForm.lnklblBuy.Visible = false;
            //        notificationForm.ShowDialog();
            //    }
            //}
            //FormProblemReport reportProblem = new FormProblemReport();
            //reportProblem.ShowDialog();
            Process.Start("https://codeachi.com/chat-support/");
        }

        private void issueDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Microsoft Excel is not installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormMigration migrateDtata = new FormMigration();
            migrateDtata.rdbIssue.Checked = true;
            migrateDtata.ShowDialog();
        }

        private void reIssueDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Microsoft Excel is not installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormMigration migrateDtata = new FormMigration();
            migrateDtata.rdbReissue.Checked = true;
            migrateDtata.ShowDialog();
        }

        private void returnDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                MessageBox.Show("Microsoft Excel is not installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormMigration migrateDtata = new FormMigration();
            migrateDtata.rdbReturn.Checked = true;
            migrateDtata.ShowDialog();
        }

        private void sendMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }

            FormMail mailSend = new FormMail();
            mailSend.StartPosition = FormStartPosition.Manual;
            mailSend.Location = new Point(this.Location.X + ((this.Width - mailSend.Width) / 2), this.Location.Y + 110);
            mailSend.Show();
        }

        private void wishListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWishList wishList = new FormWishList();
            wishList.ShowDialog();
        }

        private string GetMACAddress()
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

        private void bWorkerGetDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            if (Properties.Settings.Default.currentDate != "" && Properties.Settings.Default.currentDate != "installatioDate")
            {
                DateTime tempDate = DateTime.ParseExact(Properties.Settings.Default.currentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (tempDate < DateTime.Now.Date)
                {
                    Properties.Settings.Default.currentDate = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                    Properties.Settings.Default.Save();
                }
            }
            reserveSystemLimit = 0.ToString();
            string installedDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            string macAddress = GetMACAddress();

            SQLiteConnection sqltConn;

            if (Properties.Settings.Default.sqliteDatabase)
            {
                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select reserveSystemLimit from generalSettings";
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["reserveSystemLimit"].ToString() != "")
                        {
                            reserveSystemLimit = dataReader["reserveSystemLimit"].ToString();
                        }
                    }
                    dataReader.Close();
                }
                sqltConn.Close();
            }
            else
            {
                try
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select reserveSystemLimit from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["reserveSystemLimit"].ToString() != "")
                            {
                                reserveSystemLimit = dataReader["reserveSystemLimit"].ToString();
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    WebRequest webRequest = WebRequest.Create(globalVarLms.dateApi);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        currentDate = DateTime.ParseExact(requestResult, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    Properties.Settings.Default.currentDate = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                    Properties.Settings.Default.Save();

                    string queryToCheck = "SELECT isBlocked,licenseKey,installDate,blocked_reason FROM installationDetails WHERE mac = '" + globalVarLms.machineId + "' and productName='" + Application.ProductName + "'";
                    //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    webResponse = webRequest.GetResponse();
                    dataStream = webResponse.GetResponseStream();
                    strmReader = new StreamReader(dataStream);
                    requestResult = strmReader.ReadLine();
                    if (requestResult == null)
                    {
                        string instName = "", instAddress = "", instMail = "", countryName = "";
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select instName,instAddress,instMail,countryName from generalSettings";
                            sqltCommnd.CommandText = queryString;
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    instName = dataReader["instName"].ToString();
                                    instAddress = dataReader["instAddress"].ToString();
                                    instMail = dataReader["instMail"].ToString();
                                    countryName = dataReader["countryName"].ToString();
                                }
                            }
                            dataReader.Close();
                            sqltConn.Close();
                        }
                        else
                        {
                            try
                            {
                                mysqlConn = ConnectionClass.mysqlConnection();
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                MySqlCommand mysqlCmd;
                                string queryString = "select instName,instAddress,instMail,countryName from general_settings";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        instName = dataReader["instName"].ToString();
                                        instAddress = dataReader["instAddress"].ToString();
                                        instMail = dataReader["instMail"].ToString();
                                        countryName = dataReader["countryName"].ToString();
                                    }
                                }
                                dataReader.Close();
                                mysqlConn.Close();
                            }
                            catch
                            {

                            }
                        }
                        string queryToInsert = "INSERT INTO installationDetails (mac,productName,isBlocked,licenseKey,installDate,ip,productUninstalled,org_name,email,country,address) VALUES('" + globalVarLms.machineId + "', '" + Application.ProductName + "','" + false + "','" + "Demo" + "','" + installedDate + "','" + GetGlobalIP() + "','" + false + "','" + instName + "','" + instMail + "','" + countryName + "','" + instAddress + "')";
                        //ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                        webRequest = WebRequest.Create(globalVarLms.insertApi + queryToInsert);
                        webRequest.Timeout = 8000;
                        webResponse = webRequest.GetResponse();
                        Properties.Settings.Default.licenseType = "Demo";
                        DateTime expiryDate = currentDate.AddDays(30);
                        Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                        Properties.Settings.Default.lastChecked = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                        Properties.Settings.Default.itemLimits = 1000;
                        Properties.Settings.Default.machineLimits = 1.ToString();
                        Properties.Settings.Default.Save();
                    }

                    queryToCheck = "SELECT isBlocked,licenseKey,installDate,blocked_reason FROM installationDetails WHERE mac = '" + globalVarLms.machineId + "' and productName='" + Application.ProductName + "'";
                    webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    webResponse = webRequest.GetResponse();
                    dataStream = webResponse.GetResponseStream();
                    strmReader = new StreamReader(dataStream);
                    requestResult = strmReader.ReadLine();
                    if (requestResult != null)
                    {
                        string[] dataList = requestResult.Split('$');
                        if (dataList[1] == "Demo")
                        {
                            Properties.Settings.Default.licenseType = "Demo";
                            DateTime expiryDate = DateTime.ParseExact(dataList[2], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            expiryDate = expiryDate.AddDays(30);
                            Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                            if (dataList[0] == "True")
                            {
                                Properties.Settings.Default.productBlocked = true;
                            }
                            else
                            {
                                Properties.Settings.Default.productBlocked = false;
                            }
                            blockReason = dataList[3];
                            Properties.Settings.Default.lastChecked = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                            Properties.Settings.Default.itemLimits = 1000;
                            Properties.Settings.Default.machineLimits = 1.ToString();
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default.serialKey = dataList[1];
                            Properties.Settings.Default.Save();
                            string serialKey = Properties.Settings.Default.serialKey;
                            if (IsConnectedToInternet() == true)
                            {
                                try
                                {
                                    queryToCheck = "SELECT reg_date, valid_month,licence_type,isBlocked,mac,machine_limits,itemLimits,reserveSystemLimits,status_reson,opacAvailable FROM new_license WHERE serial_key = '" + serialKey + "'";
                                    webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);    //"https://www.codeachi.com/Product/SelectData.php?Q="
                                    webRequest.Timeout = 8000;
                                    webResponse = webRequest.GetResponse();
                                    dataStream = webResponse.GetResponseStream();
                                    strmReader = new StreamReader(dataStream);
                                    requestResult = strmReader.ReadLine();
                                    if (requestResult == null)
                                    {
                                    }
                                    else
                                    {
                                        if (requestResult.Contains(globalVarLms.machineId))
                                        {
                                            string[] splitResult = requestResult.Split('$');
                                            int validMonth = Convert.ToInt16(splitResult[1]);
                                            string licenseType = splitResult[2];
                                            bool licenseBlocked = true;
                                            if (splitResult[3] == "True")
                                            {
                                                licenseBlocked = true;
                                            }
                                            else
                                            {
                                                licenseBlocked = false;
                                            }
                                            DateTime regDate = DateTime.ParseExact(splitResult[0], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            DateTime expiryDate = regDate.AddMonths(validMonth);
                                            Properties.Settings.Default.productBlocked = licenseBlocked;
                                            Properties.Settings.Default.expiryDate = expiryDate.Day.ToString("00") + "/" + expiryDate.Month.ToString("00") + "/" + expiryDate.Year.ToString("0000");
                                            Properties.Settings.Default.licenseType = licenseType;
                                            Properties.Settings.Default.lastChecked = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                                            Properties.Settings.Default.machineLimits = splitResult[5];
                                            Properties.Settings.Default.itemLimits = Convert.ToInt32(splitResult[6]);
                                            reserveSystemLimit = splitResult[7];
                                            blockReason = splitResult[8];
                                            Properties.Settings.Default.Save();
                                            globalVarLms.opacAvailable = Convert.ToBoolean(splitResult[9]);
                                        }
                                        else
                                        {
                                            Properties.Settings.Default.licenseType = "Demo";
                                            Properties.Settings.Default.itemLimits = 1000;
                                            Properties.Settings.Default.machineLimits = 1.ToString();
                                            Properties.Settings.Default.lastChecked = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000");
                                            Properties.Settings.Default.Save();
                                        }
                                    }
                                }
                                catch
                                {
                                    //////Server Not Responds
                                }
                            }
                            else
                            {
                                //////No Internet Connection
                            }
                        }
                        string ttlItems = 0.ToString();
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select count(id) from itemDetails;";
                            sqltCommnd.CommandType = CommandType.Text;
                            ttlItems = sqltCommnd.ExecuteScalar().ToString();
                            sqltConn.Close();
                        }
                        else
                        {
                            try
                            {
                                mysqlConn = ConnectionClass.mysqlConnection();
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                MySqlCommand mysqlCmd;
                                string queryString = "select count(id) from item_details";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                ttlItems = mysqlCmd.ExecuteScalar().ToString();
                                mysqlConn.Close();
                            }
                            catch
                            {

                            }
                        }

                        string lastUsed = currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                        string queryToUpdate = "UPDATE installationDetails set last_used='" + lastUsed + "',totalItems='" + ttlItems + "',ip='" + GetGlobalIP() + "',productUninstalled='" + false + "' WHERE mac = '" + globalVarLms.machineId + "' and productName='" + Application.ProductName + "'";
                        webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);
                        webRequest.Timeout = 8000;
                        webResponse = webRequest.GetResponse();
                        dataStream = webResponse.GetResponseStream();
                        strmReader = new StreamReader(dataStream);
                        requestResult = strmReader.ReadLine();
                    }
                    //updateSettingData();
                }
                catch
                {

                }
            }
            globalVarLms.productBlocked = Properties.Settings.Default.productBlocked;
            globalVarLms.expiryDate = DateTime.ParseExact(Properties.Settings.Default.expiryDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            globalVarLms.currentDate = DateTime.ParseExact(Properties.Settings.Default.currentDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            globalVarLms.licenseType = Properties.Settings.Default.licenseType;
            globalVarLms.licenseKey = Properties.Settings.Default.serialKey;
            globalVarLms.itemLimits = Properties.Settings.Default.itemLimits;
        }

        private string GetGlobalIP()
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

        private void bWorkerUpdateDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsConnectedToInternet() == true)
            {
                string txtbUserName = "", txtbUserMail = "", txtbContact = "", txtbUserCountry = "", txtbFullName = "", txtbOrgMail = "", txtbOrgAddress = "",
                      TxtbWebsite = "", txtbOrgContact = "";
                string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                string ttlItems = "";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count(id) from itemDetails;";
                    sqltCommnd.CommandType = CommandType.Text;
                    ttlItems = sqltCommnd.ExecuteScalar().ToString();
                   
                    sqltCommnd.CommandText = "select * from userDetails where userMail!=@userMail and isAdmin='" + true + "' limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbUserName = dataReader["userName"].ToString();
                            txtbUserMail = dataReader["userMail"].ToString();
                            txtbContact = dataReader["userContact"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltCommnd.CommandText = "select * from generalSettings";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbUserCountry = dataReader["countryName"].ToString();
                            txtbFullName = dataReader["instName"].ToString();
                            txtbOrgMail = dataReader["instMail"].ToString();
                            txtbOrgAddress = dataReader["instAddress"].ToString();
                            TxtbWebsite = dataReader["instWebsite"].ToString();
                            txtbOrgContact = dataReader["instContact"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    try
                    {
                        mysqlConn = ConnectionClass.mysqlConnection();
                        if (mysqlConn.State == ConnectionState.Closed)
                        {
                            mysqlConn.Open();
                        }
                        MySqlCommand mysqlCmd;
                        string queryString = "select count(id) from item_details;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        ttlItems = mysqlCmd.ExecuteScalar().ToString();

                        queryString = "select * from user_details where userMail!=@userMail and isAdmin='" + true + "' limit 1";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbUserName = dataReader["userName"].ToString();
                                txtbUserMail = dataReader["userMail"].ToString();
                                txtbContact = dataReader["userContact"].ToString();
                            }
                        }
                        dataReader.Close();

                        queryString = "select * from general_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbUserCountry = dataReader["countryName"].ToString();
                                txtbFullName = dataReader["instName"].ToString();
                                txtbOrgMail = dataReader["instMail"].ToString();
                                txtbOrgAddress = dataReader["instAddress"].ToString();
                                TxtbWebsite = dataReader["instWebsite"].ToString();
                                txtbOrgContact = dataReader["instContact"].ToString();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    catch
                    {

                    }
                }

                try
                {
                    string queryToUpdate = "UPDATE installationDetails set org_name='" + txtbFullName + "'" +
                            ",email='" + txtbOrgMail + "',country='" + txtbUserCountry + "',address='" + txtbOrgAddress + "'," +
                            "website='" + TxtbWebsite + "',orgContact='" + txtbOrgContact + "'," +
                            "cust_name='" + txtbUserName + "',contact='" + txtbContact + "',userMail='" + txtbUserMail + "'," +
                            "last_used='" + currentDateTime + "',totalItems='" + ttlItems + "' WHERE mac = '" + globalVarLms.machineId + "' and productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.updateApi + queryToUpdate);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                }
                catch
                {

                }
            }
        }

        private void dgvDueBooks_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlDue.Text = dgvDueBooks.Rows.Count.ToString();
        }

        private void dgvReservationList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlReserved.Text = dgvReservationList.Rows.Count.ToString();
        }

        private void dgvReservationList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvReservationList.HitTest(e.X, e.Y);
                dgvReservationList.ClearSelection();
                if (hti.RowIndex>=0)
                {
                    dgvReservationList.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvReservationList, new Point(e.X, e.Y));
                }
            }
        }

        private void cancelReservation_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "delete from reservationList where itemAccn=@itemAccn";
                sqltCommnd.Parameters.AddWithValue("@itemAccn", dgvReservationList.SelectedRows[0].Cells[3].Value.ToString());
                sqltCommnd.ExecuteNonQuery();
                dgvReservationList.Rows.RemoveAt(dgvReservationList.SelectedRows[0].Index);
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "delete from reservation_list where itemAccn=@itemAccn";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@itemAccn", dgvReservationList.SelectedRows[0].Cells[3].Value.ToString());
                mysqlCmd.ExecuteNonQuery();
                dgvReservationList.Rows.RemoveAt(dgvReservationList.SelectedRows[0].Index);
                mysqlConn.Close();
            }
            dgvReservationList.ClearSelection();
        }

        private void dgvReservationList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            int serialCount = 1;
            foreach(DataGridViewRow dgvRow in dgvReservationList.Rows)
            {
                dgvRow.Cells[0].Value = serialCount;
                serialCount++;
            }
            lblTtlReserved.Text = dgvReservationList.Rows.Count.ToString();
        }

        private void dgvDueBooks_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTtlDue.Text = dgvDueBooks.Rows.Count.ToString();
        }

        private void technicalSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTechnicalSupport technicalSupport = new FormTechnicalSupport();
            technicalSupport.ShowDialog();
        }

        private void reservationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormReservationSetting reservationSetting = new FormReservationSetting();
            reservationSetting.ShowDialog();
        }

        private void oPACSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(globalVarLms.licenseType=="Demo")
            {
                MessageBox.Show("You can,t use this feature in trial version.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if(!globalVarLms.opacAvailable)
            {
                MessageBox.Show("You can,t use this feature in this license.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            FormOpacSetting opacSetting = new FormOpacSetting();
            opacSetting.ShowDialog();
        }

        private void UserGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.FileName = "explorer"; //not the full application path
                myProcess.StartInfo.Arguments = Application.StartupPath + @"\CodeAchi_LMS_UserGuide.pdf";
                myProcess.Start();
            }
            catch
            {

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = Application.ProductName + " (v" + Application.ProductVersion + ")";
            lblLoginTime.Text = "Login Time " + DateTime.Now.ToString("hh:mm:ss tt");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        sqltConn.Open();
                    }
                    catch
                    {
                        string connectionString = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";
                        sqltConn = new SQLiteConnection(connectionString);
                        sqltConn.Open();
                    }
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from generalSettings where stepComplete='" + 5 + "'";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Close();
                    FormLogin loginForm = new FormLogin();
                    loginForm.ShowDialog();
                }
                else
                {
                    dataReader.Close();
                    timer1.Stop();
                    FormWizard wizardForm = new FormWizard();
                    wizardForm.ShowDialog();
                    timer1.Start();
                }
                sqltConn.Close();

                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select instName,instLogo,instAddress,instShortName,currSymbol," +
                    "databasePath,backupPath,backupHour,printerName,opacAvailable,notificationData,settingsData from generalSettings", settingsData = "";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        globalVarLms.instName = dataReader["instName"].ToString();
                        globalVarLms.instAddress = dataReader["instAddress"].ToString();
                        globalVarLms.instShortName = dataReader["instShortName"].ToString();
                        globalVarLms.currSymbol = dataReader["currSymbol"].ToString();
                        if (Properties.Settings.Default.backupPath == "")
                        {
                            globalVarLms.backupPath = dataReader["backupPath"].ToString();
                            if (Directory.Exists(globalVarLms.backupPath))
                            {
                                Properties.Settings.Default.backupPath = globalVarLms.backupPath;
                            }
                            else
                            {
                                string backupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\LMS_Backup";
                                if(!Directory.Exists(backupPath))
                                {
                                    Directory.CreateDirectory(backupPath);
                                }
                                Properties.Settings.Default.backupPath = backupPath;
                                globalVarLms.backupPath = backupPath;
                            }
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            globalVarLms.backupPath = Properties.Settings.Default.backupPath;
                        }
                        if (dataReader["opacAvailable"].ToString() != "")
                        {
                            globalVarLms.opacAvailable = Convert.ToBoolean(dataReader["opacAvailable"].ToString());
                        }
                        else
                        {
                            globalVarLms.opacAvailable = false;
                        }
                        if (dataReader["backupHour"].ToString() != "")
                        {
                            globalVarLms.backupHour = Convert.ToInt32(dataReader["backupHour"].ToString());
                        }
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            Bitmap objBitmap = new Bitmap(Image.FromStream(memoryStream, true), new Size(49, 42));
                            pcbLogo.Image = Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            pcbLogo.Image = Properties.Resources.your_logo;
                        }
                        pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                        globalVarLms.defaultPrinter = dataReader["printerName"].ToString();
                        Properties.Settings.Default.notificationData = dataReader["notificationData"].ToString();
                        Properties.Settings.Default.Save();
                        settingsData = dataReader["settingsData"].ToString();
                    }
                    dataReader.Close();
                    if (settingsData != "")
                    {
                        jsonObj = JObject.Parse(settingsData);
                        loadSettings(jsonObj);
                    }
                }
               
                if (globalVarLms.backupHour <= 5)
                {
                    globalVarLms.backupHour = globalVarLms.backupHour * 60;
                    if (globalVarLms.backupHour == 0)
                    {
                        globalVarLms.backupHour = 60;
                    }
                    sqltCommnd.CommandText = "update generalSettings set backupHour= '" + globalVarLms.backupHour + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.ExecuteNonQuery();
                }
                sqltConn.Close();

                DashboardToolStripMenuItem_Click(null, null);
            }
            else
            {
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
                string queryString = "select * from general_settings where stepComplete='" + 5 + "'";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    dataReader.Close();
                    mysqlConn.Close();
                    FormLogin loginForm = new FormLogin();
                    loginForm.ShowDialog();
                }
                else
                {
                    dataReader.Close();
                    mysqlConn.Close();
                    timer1.Stop();
                    FormWizard wizardForm = new FormWizard();
                    wizardForm.ShowDialog();
                    timer1.Start();
                }


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
                string settingsData = "";
                try
                {
                    queryString = "select instName,instLogo,instAddress,instShortName,currSymbol," +
                        "databasePath,backupPath,backupHour,printerName,opacAvailable,notificationData,settingsData from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            globalVarLms.instName = dataReader["instName"].ToString();
                            globalVarLms.instAddress = dataReader["instAddress"].ToString();
                            globalVarLms.instShortName = dataReader["instShortName"].ToString();
                            globalVarLms.currSymbol = dataReader["currSymbol"].ToString();
                            if (Properties.Settings.Default.backupPath == "")
                            {
                                globalVarLms.backupPath = dataReader["backupPath"].ToString();
                                if (Directory.Exists(globalVarLms.backupPath))
                                {
                                    Properties.Settings.Default.backupPath = globalVarLms.backupPath;
                                }
                                else
                                {
                                    string backupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\LMS_Backup";
                                    if (!Directory.Exists(backupPath))
                                    {
                                        Directory.CreateDirectory(backupPath);
                                    }
                                    Properties.Settings.Default.backupPath = backupPath;
                                    globalVarLms.backupPath = backupPath;
                                }
                                Properties.Settings.Default.Save();
                            }
                            else
                            {
                                globalVarLms.backupPath = Properties.Settings.Default.backupPath;
                            }
                            if (dataReader["opacAvailable"].ToString() != "")
                            {
                                globalVarLms.opacAvailable = Convert.ToBoolean(dataReader["opacAvailable"].ToString());
                            }
                            else
                            {
                                globalVarLms.opacAvailable = false;
                            }
                            if (dataReader["backupHour"].ToString() != "")
                            {
                                globalVarLms.backupHour = Convert.ToInt32(dataReader["backupHour"].ToString());
                            }
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                Bitmap objBitmap = new Bitmap(Image.FromStream(memoryStream, true), new Size(49, 42));
                                pcbLogo.Image = Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbLogo.Image = Properties.Resources.your_logo;
                            }
                            pcbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

                            globalVarLms.defaultPrinter = dataReader["printerName"].ToString();
                            Properties.Settings.Default.notificationData = dataReader["notificationData"].ToString();
                            Properties.Settings.Default.Save();
                            settingsData = dataReader["settingsData"].ToString();
                        }
                    }
                    dataReader.Close();
                    if (settingsData != "")
                    {
                        jsonObj = JObject.Parse(settingsData);
                        loadSettings(jsonObj);
                    }

                    if (globalVarLms.backupHour <= 5)
                    {
                        globalVarLms.backupHour = globalVarLms.backupHour * 60;
                        if (globalVarLms.backupHour == 0)
                        {
                            globalVarLms.backupHour = 60;
                        }
                        queryString = "update general_settings set backupHour= '" + globalVarLms.backupHour + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.ExecuteNonQuery();
                    }
                }
                catch
                {

                }
                mysqlConn.Close();
                
                DashboardToolStripMenuItem_Click(null, null);
            }
            lblInstName.Text = globalVarLms.instName;
            lblInstAddress.Text = globalVarLms.instAddress;
            if (globalVarLms.backupHour > 0)
            {
                timerBackup.Interval = globalVarLms.backupHour * 60 * 1000;
                timerBackup.Start();
            }
            if (!globalVarLms.isAdmin)
            {
                adminSettingToolStripMenuItem.Enabled = false;
            }
            licenseChecking();
        }

        private void licenseChecking()
        {
            SQLiteConnection sqltConn;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "update generalSettings set opacAvailable='" + globalVarLms.opacAvailable + "'; ";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.ExecuteNonQuery();
                sqltConn.Close();
            }
            else
            {
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MySqlCommand mysqlCmd;
                string queryString = "update general_settings set opacAvailable='" + globalVarLms.opacAvailable + "'; ";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
            }
           
            if (Properties.Settings.Default.lastChecked != "" || Properties.Settings.Default.lastChecked != null)
            {
                try
                {
                    globalVarLms.lastChecked = DateTime.ParseExact(Properties.Settings.Default.lastChecked, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (Convert.ToInt32((globalVarLms.currentDate - globalVarLms.lastChecked).TotalDays) > 60)
                    {
                        FormNotification notificationForm = new FormNotification();
                        notificationForm.lblMessage.Text = "Seems you are not connected to the internet since last 60 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.";
                        notificationForm.lnklblQuotation.Visible = false;
                        notificationForm.lnklblBuy.Visible = false;
                        notificationForm.lnklblRenew.Visible = false;
                        notificationForm.lnkLblActivate.Visible = false;
                        notificationForm.ShowDialog();
                    }
                }
                catch
                {

                }
            }
            else
            {
                FormNotification notificationForm = new FormNotification();
                notificationForm.lblMessage.Text = "Seems you are not connected to the internet since last 60 days." + Environment.NewLine + "Connect to the internet to receive uninterrupted services.";
                notificationForm.lnklblQuotation.Visible = false;
                notificationForm.lnklblBuy.Visible = false;
                notificationForm.lnklblRenew.Visible = false;
                notificationForm.lnkLblActivate.Visible = false;
                notificationForm.ShowDialog();
            }
           
            if (globalVarLms.licenseType == "Demo")
            {
                pcbPremium.Visible = false;
                upgradeLicenseToolStripMenuItem.Visible = false;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "update generalSettings set licenseType='" + globalVarLms.licenseType + "',licenseKey=''";
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "update general_settings set licenseType='" + globalVarLms.licenseType + "',licenseKey='';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                this.Text = Application.ProductName + " (v" + Application.ProductVersion + " - Trial)";
                btnActivate.Text = "Activate";
                globalVarLms.itemLimits = 1000;
                if (globalVarLms.productBlocked)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = blockReason; //"Seems like your trial version has been blocked!" + Environment.NewLine + "Contact us if you think it’s a mistake on our part.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
                else
                {
                    globalVarLms.productExpire = false;
                }
            }
            else
            {
                this.Text = Application.ProductName + " (v" + Application.ProductVersion + " - " + globalVarLms.licenseType + ")";
                pcbPremium.Visible = true;
                upgradeLicenseToolStripMenuItem.Visible = true;
                Application.DoEvents();
                if (globalVarLms.productBlocked)
                {
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = blockReason; //"Seems like your serial key has been blocked!" + Environment.NewLine + "Contact us if you think it’s a mistake on our part.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    renewLicenseToolStripMenuItem.Visible = true;
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.lnkLblActivate.Visible = false;
                    notificationForm.ShowDialog();
                }
                else
                {
                    globalVarLms.productExpire = false;
                }
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "update generalSettings set licenseType='" + globalVarLms.licenseType + "'," +
                    "productExpired='" + globalVarLms.productExpire + "', productBlocked='" + globalVarLms.productBlocked + "'" +
                    ",reserveSystemLimit='" + reserveSystemLimit + "',licenseKey='" + Properties.Settings.Default.serialKey + "'";
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "update general_settings set licenseType = '" + globalVarLms.licenseType + "'," +
                    "productExpired='" + globalVarLms.productExpire + "', productBlocked='" + globalVarLms.productBlocked + "'" +
                    ",reserveSystemLimit='" + reserveSystemLimit + "',licenseKey='" + Properties.Settings.Default.serialKey + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
            }

            //==================Check for update==========================
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    string queryToCheck = "SELECT promotional_url,img_height,img_width FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        string[] splitResult = requestResult.Split('$');
                        if (splitResult.Length == 4)
                        {
                            if (splitResult[0] != "")
                            {
                                FormEvent showEvent = new FormEvent();
                                showEvent.Height = Convert.ToInt32(splitResult[1]);
                                showEvent.Width = Convert.ToInt32(splitResult[2]);
                                showEvent.webBrowser1.Navigate(splitResult[0]);
                                showEvent.pcbClose.Location = new Point(showEvent.Width - 40, showEvent.pcbClose.Location.Y);
                                showEvent.ShowDialog();
                            }
                        }
                    }
                }
                catch
                {

                }
                try
                {
                    string queryToCheck = "SELECT productVersion,downloadUrl,fileName FROM productExeVersion WHERE isLastUpdate = '" + true + "' and productName='" + Application.ProductName + "' and databaseType='sqlite'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        string[] spliData = requestResult.Split('$');
                        Version latestVersion = new Version(spliData[0]);
                        string downloadUrl = spliData[1];
                        string fileName = spliData[2];
                        Version installedVersion = new Version(Application.ProductVersion);
                        if (latestVersion > installedVersion)
                        {
                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\" + fileName))
                            {
                                if (MessageBox.Show("New version of " + Application.ProductName + " downloaded." + Environment.NewLine + "Do you want to install the new version ?", "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (globalVarLms.currentDate > globalVarLms.expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        Process.Start(Application.StartupPath + @"\Updater.exe");
                                        Application.Exit();
                                    }
                                }
                            }
                            else
                            {
                                //if (MessageBox.Show(String.Format("You've got version {0} installed of " + Application.ProductName + ". Would you like to update to the latest version {1}?", installedVersion, latestVersion), "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                //{
                                    if (globalVarLms.currentDate > globalVarLms.expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        globalVarLms.tempValue = downloadUrl + "$" + fileName;
                                        FormUpgrade upgradeProduct = new FormUpgrade();
                                        upgradeProduct.lblProdVersion.Text = Application.ProductVersion;
                                        upgradeProduct.lblLatestVersion.Text = spliData[0];
                                        upgradeProduct.ShowDialog();
                                        globalVarLms.tempValue = "";
                                    }
                                //}
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void loadSettings(JObject jsonObj)
        {
            Properties.Settings.Default.mailType = jsonObj["mailType"].ToString();
            Properties.Settings.Default.mailId = jsonObj["mailId"].ToString();
            Properties.Settings.Default.mailPassword = jsonObj["mailPassword"].ToString();
            Properties.Settings.Default.mailHost = jsonObj["mailHost"].ToString();
            Properties.Settings.Default.mailPort = jsonObj["mailPort"].ToString();
            Properties.Settings.Default.mailSsl = jsonObj["mailSsl"].ToString();
            Properties.Settings.Default.smsApi = jsonObj["smsApi"].ToString();
            Properties.Settings.Default.blockedMail = jsonObj["blockedMail"].ToString();
            Properties.Settings.Default.blockedContact = jsonObj["blockedContact"].ToString();
            Properties.Settings.Default.reserveDay =Convert.ToInt32(jsonObj["reserveDay"].ToString());
            Properties.Settings.Default.hostName = jsonObj["hostName"].ToString();
            Properties.Settings.Default.databaseSeries = jsonObj["databaseSeries"].ToString();
            Properties.Settings.Default.Save();
        }

        private void upgradeLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormActivate activateProduct = new FormActivate();
            activateProduct.ShowDialog();
            bWorkerGetDetails.RunWorkerAsync();
        }

        private void renewLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet())
            {
                Process.Start("http://codeachi.com/products/renew");
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    string queryToCheck = "SELECT productVersion,downloadUrl,fileName FROM productExeVersion WHERE isLastUpdate = '" + true + "' and productName='" + Application.ProductName + "' and databaseType='sqlite'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        string[] spliData = requestResult.Split('$');
                        Version latestVersion = new Version(spliData[0]);
                        string downloadUrl = spliData[1];
                        string fileName = spliData[2];
                        Version installedVersion = new Version(Application.ProductVersion);
                        if (latestVersion > installedVersion)
                        {
                            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\" + fileName))
                            {
                                if (MessageBox.Show("New version of " + Application.ProductName + " downloaded." + Environment.NewLine + "Do you want to install the new version ?", "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    if (globalVarLms.currentDate > globalVarLms.expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        Process.Start(Application.StartupPath + @"\Updater.exe");
                                        Application.Exit();
                                    }
                                }
                            }
                            else
                            {
                                //if (MessageBox.Show(String.Format("You've got version {0} installed of " + Application.ProductName + ". Would you like to update to the latest version {1}?", installedVersion, latestVersion), "Update " + Application.ProductName + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                //{
                                    if (globalVarLms.currentDate > globalVarLms.expiryDate)
                                    {
                                        MessageBox.Show("To get full update please renew your license...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        globalVarLms.tempValue = downloadUrl + "$" + fileName;
                                        FormUpgrade upgradeProduct = new FormUpgrade();
                                        upgradeProduct.lblProdVersion.Text = Application.ProductVersion;
                                        upgradeProduct.lblLatestVersion.Text = spliData[0];
                                        upgradeProduct.ShowDialog();
                                        globalVarLms.tempValue = "";
                                    }
                                //}
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your version of CodeAchi LMS is up-to-date!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No update available!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch
                {
                    MessageBox.Show("Please try again later!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("please check your internet connection!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ReturnReissueToolStripMenuItem_Click(null, null);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            IssueBookToolStripMenuItem_Click(null, null);
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            addBorrower_Click(null, null);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            DashboardToolStripMenuItem_Click(null, null);
        }

        private void btnAddBooks_Click(object sender, EventArgs e)
        {
            normalEntryToolStripMenuItem_Click(null, null);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchLookupToolStripMenuItem_Click(null, null);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            reportToolStripMenuItem_Click(null, null);
        }

        private void cancelReservation_MouseEnter(object sender, EventArgs e)
        {
            cancelReservation.BackColor = Color.WhiteSmoke;
            cancelReservation.ForeColor = Color.Black;
        }

        private void cancelReservation_MouseLeave(object sender, EventArgs e)
        {
            cancelReservation.BackColor = Color.FromArgb(76, 82, 90);
            cancelReservation.ForeColor = Color.WhiteSmoke;
        }

        private void simHeiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("c:/windows/Fonts/malgunbd.ttf"))
            {
                MessageBox.Show("Font already installed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (IsConnectedToInternet() == true)
            {
                FormFont fontInstall = new FormFont();
                fontInstall.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void spineLabelPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your trial has expired!" + Environment.NewLine + "Hope you have enjoyed the trial." + Environment.NewLine + "Purchase now to avail the life time serial key!";
                    notificationForm.lnklblRenew.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            else
            {
                if (globalVarLms.currentDate > globalVarLms.expiryDate)
                {
                    globalVarLms.productExpire = true;
                    btnBuyNow.Text = "Renew Now";
                    Application.DoEvents();
                    FormNotification notificationForm = new FormNotification();
                    notificationForm.lblMessage.Text = "Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.";
                    notificationForm.lnklblQuotation.Visible = false;
                    notificationForm.lnklblBuy.Visible = false;
                    notificationForm.ShowDialog();
                }
            }
            FormSplineLabel splinePrinting = new FormSplineLabel();
            splinePrinting.ShowDialog();
        }

        private void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bWorkerBackup.WorkerSupportsCancellation = true;
            bWorkerBackup.RunWorkerAsync();
            dataBackup.backupComplete = false;
            dataBackup.getException = false;
            dataBackup.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormMembershipPlan membershipPlan = new FormMembershipPlan();
            membershipPlan.dgvPlan.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            membershipPlan.dgvPlan.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            membershipPlan.ShowDialog();
        }

        private void missingAccessinoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMissingAccession missingAccession = new FormMissingAccession();
            missingAccession.ShowDialog();
        }

        private void bWorkerBackup_DoWork(object sender, DoWorkEventArgs e)
        {
            backupDatabase();
        }

        private void moreSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMoreSetting moreSetting = new FormMoreSetting();
            moreSetting.ShowDialog();
        }

        private void bWorkerBackup_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataBackup.backupComplete = true;
        }

        private void itemAddToolStripMenu_Click(object sender, EventArgs e)
        {
            normalEntryToolStripMenuItem_Click(null, null);
        }

        private void recieptSettinhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRecieptSetting recieptSetting = new FormRecieptSetting();
            recieptSetting.ShowDialog();
            globalVarLms.issueReciept = Properties.Settings.Default.issueReciept;
            globalVarLms.reissueReciept = Properties.Settings.Default.reissueReciept;
            globalVarLms.returnReciept = Properties.Settings.Default.returnReciept;
            globalVarLms.paymentReciept= Properties.Settings.Default.paymentReciept;
            globalVarLms.recieptPrinter = Properties.Settings.Default.recieptPrinter;
            globalVarLms.recieptPaper = Properties.Settings.Default.recieptPaper;
        }

        public void backupDatabase()
        {
            if(globalVarLms.backupPath!="")
            {
                updateSettingData();
                try
                {
                    string backupName = "Backup " + DateTime.Now.Day.ToString("00") + "_" +
                           DateTime.Now.Month.ToString("00") + "_" + DateTime.Now.Year.ToString("0000") +
                          " " + DateTime.Now.Hour.ToString("00") + "_" + DateTime.Now.Minute.ToString("00") + ".sl3";
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        if (Directory.Exists(globalVarLms.backupPath))
                        {
                            using (var destination = new SQLiteConnection("Data Source=" + globalVarLms.backupPath + @"\" + backupName + "; Version=3;Password=codeachi@lmssl;"))
                            {
                                destination.Open();
                                sqltConn.BackupDatabase(destination, "main", "main", -1, null, 0);
                            }
                            globalVarLms.backupRequired = false;
                        }
                    }
                    else
                    {
                        MySqlConnection mysqlConn;
                        MySqlCommand mysqlCmd;

                        mysqlConn = ConnectionClass.mysqlConnection();
                        if (mysqlConn.State == ConnectionState.Closed)
                        {
                            mysqlConn.Open();
                        }

                        mysqlCmd = new MySqlCommand();
                        mysqlCmd.Connection = mysqlConn;
                        MySqlBackup mb = new MySqlBackup(mysqlCmd);
                        backupName = globalVarLms.backupPath + @"\" + backupName.Replace("sl3", "sql");
                        mb.ExportToFile(backupName);
                        mb.Dispose();
                        mysqlConn.Close();
                        globalVarLms.backupRequired = false;
                    }
                }
                catch
                {
                    dataBackup.getException = true;
                }
            }
            else
            {
                dataBackup.getException = true;
                MessageBox.Show("Please select a backup path from general setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                        //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void timerBackup_Tick(object sender, EventArgs e)
        {
            if (globalVarLms.backupRequired)
            {
                backupDatabase();
            }
        }

        private void notificationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNotificationSetting notificationSetting = new FormNotificationSetting();
            notificationSetting.ShowDialog();
        }

        private void bWorkerNotification_DoWork(object sender, DoWorkEventArgs e)
        {
            string mngmntMail = "";
            List<string> mailBlockList = Properties.Settings.Default.blockedMail.Split('$').ToList();
            List<string> contactBlockList = Properties.Settings.Default.blockedContact.Split('$').ToList();
            MailMessage mailMessage = new MailMessage();
            //======================================SMTP SETTINGS===================
            SmtpClient smtpServer = new SmtpClient(Properties.Settings.Default.mailHost, Convert.ToInt32(Properties.Settings.Default.mailPort));
            smtpServer.UseDefaultCredentials = false;
            smtpServer.Credentials = new NetworkCredential(Properties.Settings.Default.mailId, Properties.Settings.Default.mailPassword);
            smtpServer.EnableSsl = Convert.ToBoolean(Properties.Settings.Default.mailSsl);

            SQLiteConnection sqltConn=null;
            SQLiteCommand sqltCommnd=null;
            SQLiteDataReader sqltDataReader;
            MySqlCommand mysqlCmd;
            string queryString = "";
            MySqlDataReader dataReader;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["DueMailTemplate"].ToString());
                sqltDataReader = sqltCommnd.ExecuteReader();
                if (sqltDataReader.HasRows)
                {
                    while (sqltDataReader.Read())
                    {
                        sederId = sqltDataReader["senderId"].ToString();
                        mailBody = sqltDataReader["bodyText"].ToString();
                        if (Convert.ToBoolean(sqltDataReader["htmlBody"].ToString()))
                        {
                            htmlBody = true;
                        }
                    }
                }
                sqltDataReader.Close();

                sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["DueSmsTemplate"].ToString());
                sqltDataReader = sqltCommnd.ExecuteReader();
                if (sqltDataReader.HasRows)
                {
                    while (sqltDataReader.Read())
                    {
                        smsBody = sqltDataReader["bodyText"].ToString();
                    }
                }
                sqltDataReader.Close();

                sqltCommnd.CommandText = "select mngmntMail from generalSettings";
                sqltDataReader = sqltCommnd.ExecuteReader();
                if (sqltDataReader.HasRows)
                {
                    while (sqltDataReader.Read())
                    {
                        mngmntMail = sqltDataReader["mngmntMail"].ToString();
                    }
                }
                sqltDataReader.Close();
            }
            else
            {
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                queryString = "select * from notice_template where noticeType='Mail' and tempName=@tempName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["DueMailTemplate"].ToString());
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        sederId = dataReader["senderId"].ToString();
                        mailBody = dataReader["bodyText"].ToString();
                        if (Convert.ToBoolean(dataReader["htmlBody"].ToString()))
                        {
                            htmlBody = true;
                        }
                    }
                }
                dataReader.Close();

                queryString = "select * from notice_template where noticeType='Sms' and tempName=@tempName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["DueSmsTemplate"].ToString());
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        smsBody = dataReader["bodyText"].ToString();
                    }
                }
                dataReader.Close();

                queryString = "select mngmntMail from general_settings";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                    while (dataReader.Read())
                    {
                        mngmntMail = dataReader["mngmntMail"].ToString();
                    }
                }
                dataReader.Close();
            }

            foreach (DataGridViewRow dgvRow in dgvDueBooks.Rows)
            {
                reciverId1 = "";
                issuedBorrower = dgvRow.Cells[1].Value.ToString().Substring(dgvRow.Cells[1].Value.ToString().LastIndexOf("(") + 1).Replace(")", "");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    sqltCommnd.CommandText = "select brrName,brrAddress,brrMailId,brrContact,addnlMail from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", issuedBorrower);
                    sqltDataReader = sqltCommnd.ExecuteReader();
                    if (sqltDataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (sqltDataReader.Read())
                        {
                            reciverName = sqltDataReader["brrName"].ToString();
                            reciverAddress = sqltDataReader["brrAddress"].ToString();
                            reciverId = sqltDataReader["brrMailId"].ToString();
                            reciverContact = sqltDataReader["brrContact"].ToString();
                            reciverId1 = sqltDataReader["addnlMail"].ToString();
                        }
                    }
                    sqltDataReader.Close();
                }
                else
                {
                    queryString = "select brrName,brrAddress,brrMailId,brrContact,addnlMail from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", issuedBorrower);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            reciverName = dataReader["brrName"].ToString();
                            reciverAddress = dataReader["brrAddress"].ToString();
                            reciverId = dataReader["brrMailId"].ToString();
                            reciverContact = dataReader["brrContact"].ToString();
                            reciverId1 = dataReader["addnlMail"].ToString();
                        }
                    }
                    dataReader.Close();
                }
                if (Convert.ToBoolean(jsonObj["DueMail"].ToString()))
                {
                    if (mailBlockList.IndexOf(reciverId) == -1)
                    {
                        try
                        {
                            mailMessage = new MailMessage();
                            mailMessage.From = new MailAddress(sederId);
                            mailMessage.To.Add(reciverId);
                            if (Properties.Settings.Default.mailCarbonCopy)
                            {
                                if (mngmntMail != "")
                                {
                                    mailMessage.CC.Add(mngmntMail);
                                }
                            }
                            mailMessage.Subject = jsonObj["DueMailTemplate"].ToString();
                            mailMessage.IsBodyHtml = htmlBody;
                            mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", dgvRow.Cells[2].Value.ToString()).
                                 Replace("[$ItemTitle$]", dgvRow.Cells[3].Value.ToString()).
                                 Replace("[$DueDays$]", dgvRow.Cells[5].Value.ToString()).Replace("[$ExpectedDate$]", dgvRow.Cells[4].Value.ToString());

                            smtpServer.Send(mailMessage);
                            if (mailMessage.DeliveryNotificationOptions == DeliveryNotificationOptions.OnFailure)
                            {
                                if (reciverId1 != "")
                                {
                                    mailMessage.To.Clear();
                                    mailMessage.To.Add(reciverId1);
                                    smtpServer.Send(mailMessage);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                if (Convert.ToBoolean(jsonObj["DueSms"].ToString()))
                {
                    if (contactBlockList.IndexOf(reciverId) == -1)
                    {
                        try
                        {
                            string smsApi = Properties.Settings.Default.smsApi.Replace("[$Message$]", smsBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", dgvRow.Cells[2].Value.ToString()).
                                 Replace("[$ItemTitle$]", dgvRow.Cells[3].Value.ToString()).
                                 Replace("[$DueDays$]", dgvRow.Cells[5].Value.ToString()).Replace("[$ExpectedDate$]", dgvRow.Cells[4].Value.ToString()));
                            WebRequest webRequest = WebRequest.Create(smsApi.Replace("[$ContactNumber$]", reciverContact).Replace("[$BorrowerName$]", reciverName));
                            webRequest.Timeout = 8000;
                            WebResponse webResponse = webRequest.GetResponse();
                        }
                        catch
                        {

                        }
                    }
                }
            }
            if (Properties.Settings.Default.sqliteDatabase)
            {
                sqltConn.Clone();
            }
            else
            {
                mysqlConn.Close();
            }

            DateTime notificationDate = DateTime.Now.AddDays(7);
            if(jsonObj["DueNotificationCondition"].ToString()== "Once in a Day")
            {
                notificationDate = DateTime.Now.AddDays(1);
            }
            else if (jsonObj["DueNotificationCondition"].ToString() == "Once in every Two Days")
            {
                notificationDate = DateTime.Now.AddDays(2);
            }
            else if (jsonObj["DueNotificationCondition"].ToString() == "Once in a Week")
            {
                notificationDate = DateTime.Now.AddDays(7);
            }
            else if (jsonObj["DueNotificationCondition"].ToString() == "Once in a Month")
            {
                notificationDate = DateTime.Now.AddMonths(1);
            }
            jsonObj["DueNotificationDate"] = notificationDate.Day.ToString("00") + "/" + notificationDate.Month.ToString("00") + "/" +
                notificationDate.Year.ToString("0000");
            jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            Properties.Settings.Default.notificationData = jsonString;
            Properties.Settings.Default.Save();
        }

        private void searchLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonBackColorChange();
            btnSearch.BackColor = Color.FromArgb(54, 69, 79);
            FormSearchLookup searchAndLookup = new FormSearchLookup();
            searchAndLookup.ShowDialog();
        }

        private void buttonBackColorChange()
        {
            btnDashboard.BackColor = Color.FromArgb(45, 58, 66);
            btnAddBooks.BackColor = Color.FromArgb(45, 58, 66);
            btnAddMember.BackColor = Color.FromArgb(45, 58, 66);
            btnIssue.BackColor = Color.FromArgb(45, 58, 66);
            btnReturn.BackColor = Color.FromArgb(45, 58, 66);
            btnSearch.BackColor = Color.FromArgb(45, 58, 66);
            btnReport.BackColor = Color.FromArgb(45, 58, 66);
        }
    }

    class MenuColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Red; }
        }

        //public override Color MenuBorder  //Change color according your Need
        //{
        //    get { return Color.DimGray; }
        //}

        public override Color MenuItemBorder
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Red; }
        }
    }
}
