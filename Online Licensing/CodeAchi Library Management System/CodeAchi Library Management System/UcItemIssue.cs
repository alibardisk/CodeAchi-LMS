using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using MySql.Data.MySqlClient;

namespace CodeAchi_Library_Management_System
{
    public partial class UcItemIssue : UserControl
    {
        public ToolTip tooTip = new ToolTip();
        public UcItemIssue()
        {
            InitializeComponent();

            tooTip.OwnerDraw = true;
            tooTip.Draw += new DrawToolTipEventHandler(tooTip_Draw);
        }

        JObject jsonObj;
        double finePerDay = 0; int daysLate = 0, issueLimit = 0;
        AutoCompleteStringCollection autoCollAccession = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCollIsbn = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCollBorrower = new AutoCompleteStringCollection();
        bool notReference = false,isLost=false,isAvailable=false, htmlBody = false;
        string brrCategory = null,catAccess=null, itemCat = null,itemSubCat=null,currentUser, 
            jsonString = "", sederId = "", mailBody,issuedBorrower,reciverId,reciverAddress,
            reciverName, reciverId1;
        string mngmntMail = "";
        List<string> issuedAccession = new List<string> { };
        List<string> issuedTitle = new List<string> { };
        List<string> issuedAuthor = new List<string> { };
        List<string> issuedDate= new List<string> { };
        List<string> expectedDate = new List<string> { };
        List<string> blockList = new List<string> { };
        List<string> accessibleItem = new List<string> { };

        public void UcItemIssue_Load(object sender, EventArgs e)
        {
            txtbAccn.Enabled = false;
            txtbIsbn.Enabled = false;
            rdbIsbn.Enabled = false;
            rdbAccn.Enabled = false;
            numDay.Enabled = false;
            dtpIssue.Enabled = false;
            btnAdd.Enabled = false;
            btnIssueBook.Enabled = false;
            btnReset.Enabled = false;
            dtpIssue.Value = DateTime.Now;
            txtbAccn.Clear();
            txtbCategory.Clear();
            txtbBrrId.Clear();
            txtbBrrName.Clear();
            txtbBrrRnwDate.Clear();
            txtbFees.Text = 0.00.ToString("0.00");
            txtbIssueditem.Text = 0.ToString();
            dgvBook.Rows.Clear();
            dgvIssuedBook.Rows.Clear();
            currentUser = Properties.Settings.Default.currentUserId;
            lblBlink.Visible = false;
            lblMessage.Visible = false;
            btnRenew.Visible = false;
            btnItemSearch.Enabled = false;
            jsonString = Properties.Settings.Default.notificationData;
            dtpIssue.CustomFormat = Properties.Settings.Default.dateFormat;
            Label10.Text = "Renew Date (" + Properties.Settings.Default.dateFormat + ") :";
            label11.Text = "(" + Properties.Settings.Default.dateFormat + ")";

            txtbAccn.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbAccn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbAccn.AutoCompleteCustomSource = autoCollAccession;

            txtbIsbn.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbIsbn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbIsbn.AutoCompleteCustomSource = autoCollIsbn;

            txtbBrrId.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbBrrId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbBrrId.AutoCompleteCustomSource = autoCollBorrower;
            loadFieldValue();
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
                        if(fieldName== "lblAccession")
                        {
                            dgvBook.Columns[0].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            dgvIssuedBook.Columns[0].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbAccn.Text= fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if(fieldName == "lblTitle")
                        {
                            dgvBook.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            dgvIssuedBook.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblCategory")
                        {
                            dgvBook.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblSubject")
                        {
                            dgvBook.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvBook.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            dgvBook.Columns[5].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblIsbn")
                        {
                            rdbIsbn.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }

            if (FieldSettings.Default.borrowerEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        if (fieldName == "lblBrId")
                        {
                            lblBrId.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrName")
                        {
                            lblBrName.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            lblBrCategory.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtbBrrId.Text=="")
            {
                MessageBox.Show("Please enter a borrower id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrId.SelectAll();
                return;
            }
            if (txtbAccn.Text != "")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select itemTitle,itemCat,itemSubCat,itemSubject,itemAuthor,rackNo,itemAvailable,isLost" +
                        " from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            isAvailable = Convert.ToBoolean(dataReader["itemAvailable"].ToString());
                            isLost = Convert.ToBoolean(dataReader["isLost"].ToString());
                            itemCat = dataReader["itemCat"].ToString();
                            itemSubCat = dataReader["itemSubCat"].ToString();
                        }
                        dataReader.Close();
                        if (isLost)
                        {
                            MessageBox.Show("Item lost.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if (!isAvailable)
                        {
                            MessageBox.Show("Item not available.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }

                        sqltCommnd.CommandText = "select issueDays,notReference from itemSubCategory where catName=@catName and subCatName=@subCatName";
                        sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                        sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubCat);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                notReference = Convert.ToBoolean(dataReader["notReference"].ToString());
                            }
                        }
                        dataReader.Close();
                        if (!notReference)
                        {
                            MessageBox.Show("This is a reference copy, you can't issue this item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if (accessibleItem.IndexOf(txtbAccn.Text) == -1)
                        {
                            MessageBox.Show("This borrower category don't have access to this item category," + Environment.NewLine +
                                "to give access go to borrower setting & update this category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if ((dgvBook.Rows.Count + dgvIssuedBook.Rows.Count) == issueLimit)
                        {
                            MessageBox.Show("You can't issue more than " + issueLimit.ToString() + " items to this borrower", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        foreach (DataGridViewRow dataRow in dgvBook.Rows)
                        {
                            if (dataRow.Cells[0].Value.ToString() == txtbAccn.Text)
                            {
                                MessageBox.Show("Item already added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtbAccn.SelectAll();
                                return;
                            }
                        }
                        sqltCommnd.CommandText = "select itemAccn from reservationList where itemAccn=@itemAccn and brrId!='" + txtbBrrId.Text + "'";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", txtbAccn.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            MessageBox.Show("This item reserved by someone.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            dataReader.Close();
                            return;
                        }
                        dataReader.Close();
                        string queryString = "select itemTitle,itemCat,itemSubject,itemAuthor,rackNo from itemDetails where itemAccession=@itemAccession and itemAvailable='" + true + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        string issueDate = dtpIssue.Value.Day.ToString("00") + "/" + dtpIssue.Value.Month.ToString("00") + "/" + dtpIssue.Value.Year.ToString("0000");
                        DateTime returnDate = dtpIssue.Value.Date;
                        if (Properties.Settings.Default.includeHoliday)
                        {
                            returnDate = dtpIssue.Value.AddDays(Convert.ToDouble(numDay.Value));
                        }
                        else
                        {
                            returnDate = AddBusinessDays(dtpIssue.Value.Date, Convert.ToInt32(numDay.Value));
                        }
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBook.Rows.Add(txtbAccn.Text, dataReader["itemTitle"].ToString(), dataReader["itemCat"].ToString(),
                                    dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(),
                                   FormatDate.getUserFormat(issueDate), FormatDate.getUserFormat(returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000")));
                            }
                        }
                        dgvBook.ClearSelection();
                        dataReader.Close();
                        sqltConn.Close();
                        txtbAccn.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Item not exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbAccn.SelectAll();
                        return;
                    }
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
                    string queryString = "select itemTitle,itemCat,itemSubCat,itemSubject,itemAuthor,rackNo,itemAvailable,isLost" +
                        " from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            isAvailable = Convert.ToBoolean(dataReader["itemAvailable"].ToString());
                            isLost = Convert.ToBoolean(dataReader["isLost"].ToString());
                            itemCat = dataReader["itemCat"].ToString();
                            itemSubCat = dataReader["itemSubCat"].ToString();
                        }
                        dataReader.Close();
                        if (isLost)
                        {
                            MessageBox.Show("Item lost.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if (!isAvailable)
                        {
                            MessageBox.Show("Item not available.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }

                        queryString = "select issueDays,notReference from item_subcategory where catName=@catName and subCatName=@subCatName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                        mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubCat);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                notReference = Convert.ToBoolean(dataReader["notReference"].ToString());
                            }
                        }
                        dataReader.Close();
                        if (!notReference)
                        {
                            MessageBox.Show("This is a reference copy, you can't issue this item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if (accessibleItem.IndexOf(txtbAccn.Text) == -1)
                        {
                            MessageBox.Show("This borrower category don't have access to this item category," + Environment.NewLine +
                                "to give access go to borrower setting & update this category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            return;
                        }
                        if ((dgvBook.Rows.Count + dgvIssuedBook.Rows.Count) == issueLimit)
                        {
                            MessageBox.Show("You can't issue more than " + issueLimit.ToString() + " items to this borrower", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        foreach (DataGridViewRow dataRow in dgvBook.Rows)
                        {
                            if (dataRow.Cells[0].Value.ToString() == txtbAccn.Text)
                            {
                                MessageBox.Show("Item already added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtbAccn.SelectAll();
                                return;
                            }
                        }
                        queryString = "select itemAccn from reservation_list where itemAccn=@itemAccn and brrId!='" + txtbBrrId.Text + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", txtbAccn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            MessageBox.Show("This item reserved by someone.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.SelectAll();
                            dataReader.Close();
                            return;
                        }
                        dataReader.Close();

                        queryString = "select itemTitle,itemCat,itemSubject,itemAuthor,rackNo from item_details where itemAccession=@itemAccession and itemAvailable='" + true + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        string issueDate = dtpIssue.Value.Day.ToString("00") + "/" + dtpIssue.Value.Month.ToString("00") + "/" + dtpIssue.Value.Year.ToString("0000");
                        DateTime returnDate = dtpIssue.Value.Date;
                        if (Properties.Settings.Default.includeHoliday)
                        {
                            returnDate = dtpIssue.Value.AddDays(Convert.ToDouble(numDay.Value));
                        }
                        else
                        {
                            returnDate = AddBusinessDays(dtpIssue.Value.Date, Convert.ToInt32(numDay.Value));
                        }
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBook.Rows.Add(txtbAccn.Text, dataReader["itemTitle"].ToString(), dataReader["itemCat"].ToString(),
                                    dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(),
                                    FormatDate.getUserFormat(issueDate), FormatDate.getUserFormat(returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000")));
                            }
                        }
                        dgvBook.ClearSelection();
                        dataReader.Close();
                        txtbAccn.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Item not exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbAccn.SelectAll();
                        return;
                    }
                    mysqlConn.Close();
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBook.SelectedRows.Count == 1)
            {
                txtbAccn.Text = dgvBook.SelectedRows[0].Cells[0].Value.ToString();
                dtpIssue.Value = DateTime.ParseExact(FormatDate.getAppFormat(dgvBook.SelectedRows[0].Cells["isueDate"].Value.ToString()), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime issueDate= DateTime.ParseExact(FormatDate.getAppFormat(dgvBook.SelectedRows[0].Cells["isueDate"].Value.ToString()), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime returnDate= DateTime.ParseExact(FormatDate.getAppFormat(dgvBook.SelectedRows[0].Cells["returnDate"].Value.ToString()), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                numDay.Value =Convert.ToDecimal((returnDate - issueDate).TotalDays);
                dgvBook.Rows.RemoveAt(dgvBook.SelectedRows[0].Index);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgvBook.SelectedRows.Count==1)
            {
                dgvBook.Rows.RemoveAt(dgvBook.SelectedRows[0].Index);
            }
        }

        private void removeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvBook.Rows.Clear();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pcbBrrImage.Image = Properties.Resources.blankBrrImage;
            txtbBrrId.Clear();
            txtbBrrName.Clear();
            txtbBrrRnwDate.Clear();
            txtbFees.Text=0.00.ToString("0.00");
            txtbIssueditem.Text=0.ToString();
            txtbAccn.Clear();
            numDay.Value = 1;
            dgvBook.Rows.Clear();
            dgvIssuedBook.Rows.Clear();
            dtpIssue.Value = DateTime.Now;
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {
            txtbAccn.Clear();
            if (txtbBrrId.Text != "")
            {
                
            }
            else
            {
                btnRenew.Visible = false;
                tooTip.Hide(txtbBrrId);
                txtbBrrName.Clear();
                txtbBrrRnwDate.Clear();
                txtbCategory.Clear();
                txtbFees.Clear();
                txtbIssueditem.Clear();
                dgvIssuedBook.Rows.Clear();
                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                btnItemSearch.Enabled = false;
            }
        }

        private void txtbIssueDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsLetter(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblBlink.Visible = !lblBlink.Visible;
        }

        private void txtbBrrId_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                borrowerChecking();
            }
        }

        public void borrowerChecking()
        {
            txtbAccn.Clear();
            txtbIsbn.Clear();
            if (txtbBrrId.Text != "")
            {
                issueLimit = 0;
                string membrshpName = "";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from borrowerDetails inner join mbershipSetting where borrowerDetails.memPlan=mbershipSetting.membrshpName and borrowerDetails.brrId=@brrId";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbCategory.Text = dataReader["brrCategory"].ToString();
                            if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                            {
                                entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                            txtbBrrRnwDate.Text =FormatDate.getUserFormat(renewDate.Day.ToString("00") + "/" + renewDate.Month.ToString("00") + "/" + renewDate.Year.ToString("0000"));
                            brrCategory = dataReader["brrCategory"].ToString();
                            membrshpName = dataReader["memPlan"].ToString();
                            issueLimit = Convert.ToInt32(dataReader["issueLimit"].ToString());
                            try
                            {
                                if(dataReader["imagePath"].ToString()!="base64String")
                                {
                                    pcbBrrImage.Image = System.Drawing.Image.FromFile(Properties.Settings.Default.databasePath + @"\BorrowerImage\" + dataReader["imagePath"].ToString());
                                }
                                else
                                {
                                    pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                                }
                            }
                            catch
                            {
                                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                            }
                        }
                        dataReader.Close();

                        timerIssueDetails.Start();

                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select count(id) from issuedItem where itemReturned = '" + false + "' and brrId=@brrId;";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        txtbIssueditem.Text = sqltCommnd.ExecuteScalar().ToString();

                        queryString = "select sum(dueAmount) as totalDue from paymentDetails where memberId=@memberId and [isPaid]='" + false + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        double ttlDue = 0.00;
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["totalDue"].ToString() != "")
                                {
                                    ttlDue = Convert.ToDouble(dataReader["totalDue"].ToString());
                                }
                            }
                        }
                        txtbFees.Text = Math.Round(ttlDue, 2, MidpointRounding.ToEven).ToString("0.00");
                        dataReader.Close();
                        if (renewDate.Date <= DateTime.Now.Date)
                        {
                            btnRenew.Visible = true;
                            MessageBox.Show("Membership expired, Please renew now.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.Enabled = false;
                            txtbIsbn.Enabled = false;
                            rdbAccn.Enabled = false;
                            rdbIsbn.Enabled = false;
                        }
                        else
                        {
                            btnItemSearch.Enabled = true;
                            tooTip.Hide(txtbBrrId);
                            btnRenew.Visible = false;
                            
                            dgvIssuedBook.ClearSelection();
                            rdbAccn.Enabled = true;
                            rdbIsbn.Enabled = true;
                            if (Properties.Settings.Default.issueByAccn == true)
                            {
                                rdbAccn.Checked = true;
                            }
                            else
                            {
                                rdbIsbn.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        btnRenew.Visible = false;
                        txtbBrrName.Clear();
                        txtbBrrRnwDate.Clear();
                        txtbCategory.Clear();
                        txtbFees.Clear();
                        txtbIssueditem.Clear();
                        dgvIssuedBook.Rows.Clear();
                        pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                        btnItemSearch.Enabled = false;
                        MessageBox.Show("Borrower doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string queryString = "select * from borrower_details inner join mbership_setting where borrower_details.memPlan=mbership_setting.membrshpName and borrower_details.brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbCategory.Text = dataReader["brrCategory"].ToString();
                            if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                            {
                                entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                            txtbBrrRnwDate.Text =FormatDate.getUserFormat(renewDate.Day.ToString("00") + "/" + renewDate.Month.ToString("00") + "/" + renewDate.Year.ToString("0000"));
                            brrCategory = dataReader["brrCategory"].ToString();
                            membrshpName = dataReader["memPlan"].ToString();
                            issueLimit = Convert.ToInt32(dataReader["issueLimit"].ToString());
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbBrrImage.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                            }
                        }
                        dataReader.Close();

                        timerIssueDetails.Start();

                        queryString = "select count(id) from issued_item where itemReturned = '" + false + "' and brrId=@brrId;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        txtbIssueditem.Text = mysqlCmd.ExecuteScalar().ToString();

                        queryString = "select sum(dueAmount) as totalDue from payment_details where memberId=@memberId and isPaid='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        double ttlDue = 0.00;
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["totalDue"].ToString() != "")
                                {
                                    ttlDue = Convert.ToDouble(dataReader["totalDue"].ToString());
                                }
                            }
                        }
                        txtbFees.Text = Math.Round(ttlDue, 2, MidpointRounding.ToEven).ToString("0.00");
                        dataReader.Close();

                        if (renewDate.Date <= DateTime.Now.Date)
                        {
                            btnRenew.Visible = true;
                            MessageBox.Show("Membership expired, Please renew now.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtbAccn.Enabled = false;
                            txtbIsbn.Enabled = false;
                            rdbAccn.Enabled = false;
                            rdbIsbn.Enabled = false;
                        }
                        else
                        {
                            btnItemSearch.Enabled = true;
                            tooTip.Hide(txtbBrrId);
                            btnRenew.Visible = false;

                            dgvIssuedBook.ClearSelection();
                            rdbAccn.Enabled = true;
                            rdbIsbn.Enabled = true;
                            if (Properties.Settings.Default.issueByAccn == true)
                            {
                                rdbAccn.Checked = true;
                            }
                            else
                            {
                                rdbIsbn.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        dataReader.Close();
                        btnRenew.Visible = false;
                        txtbBrrName.Clear();
                        txtbBrrRnwDate.Clear();
                        txtbCategory.Clear();
                        txtbFees.Clear();
                        txtbIssueditem.Clear();
                        dgvIssuedBook.Rows.Clear();
                        pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                        btnItemSearch.Enabled = false;
                        MessageBox.Show("Borrower doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    mysqlConn.Close();
                }
            }
            else
            {
                btnRenew.Visible = false;
                tooTip.Hide(txtbBrrId);
                txtbBrrName.Clear();
                txtbBrrRnwDate.Clear();
                txtbCategory.Clear();
                txtbFees.Clear();
                txtbIssueditem.Clear();
                dgvIssuedBook.Rows.Clear();
                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                btnItemSearch.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            btnClose_Click(null, null);
            timer2.Stop();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            while (panelNotification.Width > 1)
            {
                panelNotification.Location = new Point(panelNotification.Location.X + 1, panelNotification.Location.Y);
                panelNotification.Width = panelNotification.Width - 1;
                Application.DoEvents();
            }
            panelNotification.Visible = false;
        }

        private void showNotification()
        {
            lblProductName.Text = Application.ProductName;
            panelNotification.Visible = true;
            while (panelNotification.Width < 278)
            {
                panelNotification.Location = new Point(panelNotification.Location.X - 1, panelNotification.Location.Y);
                panelNotification.Width = panelNotification.Width + 1;
                Application.DoEvents();
            }
            timer2.Start();
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

        private void removeToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            removeToolStripMenuItem.BackColor = Color.WhiteSmoke;
            removeToolStripMenuItem.ForeColor = Color.Black;
        }

        private void removeToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            removeToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            removeToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void removeAllToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            removeAllToolStripMenuItem.BackColor = Color.WhiteSmoke;
            removeAllToolStripMenuItem.ForeColor = Color.Black;
        }

        private void removeAllToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            removeAllToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            removeAllToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void btnAdd_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnAdd.Enabled == true)
            {
                btnAdd.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnAdd.BackColor = Color.DimGray;
            }
        }

        private void btnIssueBook_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnIssueBook.Enabled == true)
            {
                btnIssueBook.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnIssueBook.BackColor = Color.DimGray;
            }
        }

        private void rdbAccn_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbAccn.Checked)
            {
                txtbAccn.Enabled = true;
                txtbIsbn.Enabled = false;
                txtbIsbn.Clear();
                txtbAccn.Select();
                Properties.Settings.Default.issueByAccn = true;
                Properties.Settings.Default.Save();
                Application.DoEvents();
            }
        }

        private void txtbIsbn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemIsbn = @itemIsbn;";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) == 1)
                    {
                        sqltCommnd.CommandText = "select itemAccession from itemDetails where itemIsbn = @itemIsbn;";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbAccn.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        btnAdd_Click(null, null);
                    }
                    else if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        sqltCommnd.CommandText = "select * from itemDetails where itemIsbn = @itemIsbn;";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                formIsbnItems.lblIsbn.Text = dataReader["itemIsbn"].ToString();
                                formIsbnItems.lblTitle.Text = dataReader["itemTitle"].ToString();
                                formIsbnItems.lblAuthor.Text = dataReader["itemAuthor"].ToString();
                                if (dataReader["itemAvailable"].ToString() == "True")
                                {
                                    formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "Yes");
                                }
                                else
                                {
                                    formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "No");
                                    formIsbnItems.dgvAccnList.Rows[formIsbnItems.dgvAccnList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                            }
                        }
                        dataReader.Close();
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To issue an item right/double click on it.";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbAccn.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            btnAdd_Click(null, null);
                        }
                    }
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
                    string queryString = "select count(id) from item_details where itemIsbn = @itemIsbn;";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    
                    if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) == 1)
                    {
                        queryString = "select itemAccession from item_details where itemIsbn = @itemIsbn;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbAccn.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                        btnAdd_Click(null, null);
                    }
                    else if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        queryString = "select * from item_details where itemIsbn = @itemIsbn;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                formIsbnItems.lblIsbn.Text = dataReader["itemIsbn"].ToString();
                                formIsbnItems.lblTitle.Text = dataReader["itemTitle"].ToString();
                                formIsbnItems.lblAuthor.Text = dataReader["itemAuthor"].ToString();
                                if (dataReader["itemAvailable"].ToString() == "True")
                                {
                                    formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "Yes");
                                }
                                else
                                {
                                    formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "No");
                                    formIsbnItems.dgvAccnList.Rows[formIsbnItems.dgvAccnList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To issue an item right/double click on it.";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbAccn.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            btnAdd_Click(null, null);
                        }
                    }
                }
            }
        }

        private void rdbIsbn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIsbn.Checked)
            {
                txtbIsbn.Enabled = true;
                txtbAccn.Enabled = false;
                txtbAccn.Clear();
                txtbIsbn.Select();
                Properties.Settings.Default.issueByAccn = false;
                Properties.Settings.Default.Save();
            }
            Application.DoEvents();
        }

        private void txtbAccn_Leave(object sender, EventArgs e)
        {
            if(txtbAccn.Text=="")
            {
                timer1.Stop();
                lblBlink.Visible = false;
                lblMessage.Visible = false;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timerIssueDetails.Stop();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catlogAccess,maxCheckin from borrowerSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", brrCategory);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                dgvIssuedBook.Rows.Clear();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        catAccess = dataReader["catlogAccess"].ToString();
                    }
                    dataReader.Close();
                }
                timerAccession.Start();
                //////============================Issued Items==========================
                DateTime currentDate = DateTime.Now.Date, returnDate; double totalFine = 0;
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select * from issuedItem inner join itemDetails inner join itemSubCategory " +
                    "where issuedItem.itemAccession=itemDetails.itemAccession and itemDetails.itemCat=itemSubCategory.catName and " +
                    "itemDetails.itemSubCat=itemSubCategory.subCatName and issuedItem.brrId=@brrId and issuedItem.itemReturned='" + false + "'";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        returnDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        daysLate = Convert.ToInt32((currentDate - returnDate).TotalDays);

                        finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                        totalFine = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven);
                        if (totalFine < 0)
                        {
                            totalFine = 0;
                        }
                        dgvIssuedBook.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),FormatDate.getUserFormat(dataReader["issueDate"].ToString()), totalFine.ToString("0.00"));
                    }
                }
                dataReader.Close();
                sqltConn.Close();
                Application.DoEvents();
            }
            else
            {
                catAccess = "";
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
                string queryString = "select catlogAccess,maxCheckin from borrower_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", brrCategory);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                dgvIssuedBook.Rows.Clear();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        catAccess = dataReader["catlogAccess"].ToString();
                    }
                }
                dataReader.Close();
                timerAccession.Start();
                //////============================Issued Items==========================
                DateTime currentDate = DateTime.Now.Date, returnDate; double totalFine = 0;

                queryString = "select * from issued_item inner join item_details inner join item_subcategory " +
                    "where issued_item.itemAccession=item_details.itemAccession and item_details.itemCat=item_subcategory.catName and " +
                    "item_details.itemSubCat=item_subcategory.subCatName and issued_item.brrId=@brrId and issued_item.itemReturned='" + false + "'";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        returnDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        daysLate = Convert.ToInt32((currentDate - returnDate).TotalDays);

                        finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                        totalFine = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven);
                        if (totalFine < 0)
                        {
                            totalFine = 0;
                        }
                        dgvIssuedBook.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),FormatDate.getUserFormat(dataReader["issueDate"].ToString()), totalFine.ToString("0.00"));
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
                Application.DoEvents();
            }
        }

        private void timerAccession_Tick(object sender, EventArgs e)
        {
            timerAccession.Stop();
            getAccession();
        }

        private void timerBorrowerId_Tick(object sender, EventArgs e)
        {
            timerBorrowerId.Stop();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                //==============Borrower Id add to autocomplete=============
                string queryString = "select brrId from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollBorrower.Clear();
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["brrId"]
                        ).ToList();
                    autoCollBorrower.AddRange(idList.ToArray());
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
                    string queryString = "select brrId from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollBorrower.Clear();
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["brrId"]
                            ).ToList();
                        autoCollBorrower.AddRange(idList.ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            globalVarLms.issueReciept = Properties.Settings.Default.issueReciept;
            globalVarLms.reissueReciept = Properties.Settings.Default.reissueReciept;
            globalVarLms.returnReciept = Properties.Settings.Default.returnReciept;
            globalVarLms.paymentReciept = Properties.Settings.Default.paymentReciept;
            globalVarLms.recieptPrinter = Properties.Settings.Default.recieptPrinter;
            globalVarLms.recieptPaper = Properties.Settings.Default.recieptPaper;
        }

        private void btnRenew_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Renew Member", btnRenew);
        }

        private void btnRenew_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRenew);
        }

        private void btnMemberSearch_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Member Lookup", btnMemberSearch);
        }

        private void btnMemberSearch_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnMemberSearch);
        }

        private void btnTutorial_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("See Tutorial", btnTutorial);
        }

        private void btnTutorial_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnTutorial);
        }

        private void btnRecieptSetting_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Receipt Setting", btnRecieptSetting);
        }

        private void btnRecieptSetting_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRecieptSetting);
        }

        private void btnPrint_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Print List", btnPrint);
        }

        private void btnPrint_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnPrint);
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            FormRenewMember renewForm = new FormRenewMember();
            renewForm.txtbBrrId.Text = txtbBrrId.Text;
            renewForm.ShowDialog();
            btnRenew.Visible = false;
            borrowerChecking();
        }

        private void btnMemberSearch_Click(object sender, EventArgs e)
        {
            globalVarLms.brrId = "";
            FormMemberLookup memberLookup = new FormMemberLookup();
            memberLookup.lblInfo.Text = "* To issue item of a borrower right click on it.";
            memberLookup.btnSend.Visible = false;
            memberLookup.dgvBrrDetails.MultiSelect = false;
            memberLookup.contextMenuStrip1.Enabled = true;
            globalVarLms.mailSending = false;
            memberLookup.ShowDialog();
            if (globalVarLms.brrId != "")
            {
                txtbBrrId.Text = globalVarLms.brrId;
                borrowerChecking();
            }
        }

        private void btnItemSearch_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Item Lookup/Reserve", btnItemSearch);
        }

        private void btnItemSearch_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnItemSearch);
        }

        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            globalVarLms.itemAccn = "";
            globalVarLms.tempValue = catAccess;
            globalVarLms.brrId = txtbBrrId.Text;
            FormItemLookup itemLookup = new FormItemLookup();
            itemLookup.ShowDialog();
            if (globalVarLms.itemAccn != "")
            {
                txtbAccn.Text = globalVarLms.itemAccn;
            }
            globalVarLms.tempValue = "";
            globalVarLms.brrId = "";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (globalVarLms.recieptPaper == "")
            {
                MessageBox.Show("Please select a paper format from reciept setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (globalVarLms.recieptPrinter == "")
            {
                MessageBox.Show("Please select a printer from reciept setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string folderPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch
                {

                }
            }
            Directory.CreateDirectory(folderPath);
            string fileName = "tempReport";
            if (File.Exists(folderPath + @"\" + fileName + ".pdf"))
            {
                File.Delete(folderPath + @"\" + fileName + ".pdf");
            }
            fileName = folderPath + @"\" + fileName + ".pdf";
            System.Drawing.Image instLogo = null;
            string instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        instName = dataReader["instName"].ToString();
                        instAddress = dataReader["instAddress"].ToString();
                        instContact = dataReader["instContact"].ToString();
                        instWebsite = dataReader["instWebsite"].ToString();
                        instMail = dataReader["instMail"].ToString();
                        cuurShort = dataReader["currShortName"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            instLogo = Properties.Resources.NoImageAvailable;
                        }
                    }
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
                    string queryString = "select * from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            instName = dataReader["instName"].ToString();
                            instAddress = dataReader["instAddress"].ToString();
                            instContact = dataReader["instContact"].ToString();
                            instWebsite = dataReader["instWebsite"].ToString();
                            instMail = dataReader["instMail"].ToString();
                            cuurShort = dataReader["currShortName"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                instLogo = Properties.Resources.NoImageAvailable;
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
            if (globalVarLms.recieptPaper == "A4")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    Document pdfToCreate = new Document(PageSize.A4);
                    pdfToCreate.SetMargins(40, 10, 160, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 12, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 12, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 70f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(50f, 50f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 15);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 12);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 44f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = "Call/website/email : " + instContact + " | " + instWebsite + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 12);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 59f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 12);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(360, pdfToCreate.PageSize.Height - 74f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(360, pdfToCreate.PageSize.Height - 64f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText("Prepared Date : " + FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 87f);
                    pdfContent.LineTo(585, pdfToCreate.PageSize.Height - 87f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 575;

                    PdfPTable tempTable = new PdfPTable(6);
                    float[] columnWidth = { 60, 120, 200,120, 120,90 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[4].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[7].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    //pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(6);
                    tempTable.SetTotalWidth(columnWidth);
                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[3].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[4].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[7].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 85f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }
            else if (globalVarLms.recieptPaper == "Roll Paper 80 x 297 mm")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(315f, 1169f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(200, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(200, pdfToCreate.PageSize.Height - 46f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(305, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 295;

                    PdfPTable tempTable = new PdfPTable(4);
                    pdfTable.TotalWidth = 295;

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[5].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    //pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    pdfTable.TotalWidth = 295;

                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[5].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);
                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }
            else
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(189f, 12897f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(90, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(90, pdfToCreate.PageSize.Height - 46f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(179, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 169;

                    PdfPTable tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[5].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    //pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;

                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[5].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);
                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }

            // Create the printer settings for our printer
            var printerSettings = new PrinterSettings
            {
                PrinterName = globalVarLms.recieptPrinter,
                Copies = (short)1,
                FromPage = (short)1,
            };

            // Create our page settings for the paper size selected
            var pageSettings = new PageSettings(printerSettings)
            {
                Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0),
            };
            foreach (PaperSize paperSize in printerSettings.PaperSizes)
            {
                //MessageBox.Show(paperSize.PaperName + "," + paperSize.Width + "," + paperSize.Height);
                if (paperSize.PaperName == "A4")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
                else if (paperSize.PaperName == "Roll Paper 80 x 297 mm")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
            }
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(fileName))
            {
                using (var printDocument = pdfDocument.CreatePrintDocument())
                {
                    printDocument.PrinterSettings = printerSettings;
                    printDocument.DefaultPageSettings = pageSettings;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                    //https://stackoverflow.com/questions/6103705/how-can-i-send-a-file-document-to-the-printer-and-have-it-print
                }
            }
        }

        private void btnRecieptSetting_Click(object sender, EventArgs e)
        {
            FormRecieptSetting recieptSetting = new FormRecieptSetting();
            recieptSetting.ShowDialog();
            globalVarLms.issueReciept = Properties.Settings.Default.issueReciept;
            globalVarLms.reissueReciept = Properties.Settings.Default.reissueReciept;
            globalVarLms.returnReciept = Properties.Settings.Default.returnReciept;
            globalVarLms.paymentReciept = Properties.Settings.Default.paymentReciept;
            globalVarLms.recieptPrinter = Properties.Settings.Default.recieptPrinter;
            globalVarLms.recieptPaper = Properties.Settings.Default.recieptPaper;
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

        private void dgvBook_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvBook.HitTest(e.X, e.Y);
                dgvBook.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvBook.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvBook, new Point(e.X, e.Y));
                }
            }
        }

        private void txtbBrrName_TextChanged(object sender, EventArgs e)
        {
            if (txtbBrrName.Text != "")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    //============================Issued Items==========================
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select itemAccn,itemTitle,itemAuthor,reserveLocation from reservationList where brrId=@brrId and itemAccn!=''";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBook.Rows.Add(dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(), "", "",
                                dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString());
                        }
                        Application.DoEvents();
                    }
                    dataReader.Close();
                    dgvBook.ClearSelection();

                    DateTime currDate = DateTime.Now;
                    string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000"), returnDate = "";
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        sqltCommnd.CommandText = "select itemCat,itemSubCat,itemSubject from itemDetails where itemAccession=@itemAccession";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[0].Value.ToString());
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvRow.Cells[2].Value = dataReader["itemCat"].ToString();
                                dgvRow.Cells[3].Value = dataReader["itemSubject"].ToString();
                                itemCat = dataReader["itemCat"].ToString();
                                itemSubCat = dataReader["itemSubCat"].ToString();
                            }
                            dataReader.Close();

                            sqltCommnd.CommandText = "select issueDays from itemSubCategory where catName=@catName and subCatName=@subCatName";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                            sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubCat);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                                }
                            }
                            dataReader.Close();
                            currDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                            returnDate = currDate.Day.ToString("00") + "/" + currDate.Month.ToString("00") + "/" + currDate.Year.ToString("0000");
                            dgvRow.Cells[6].Value = issueDate;
                            dgvRow.Cells[7].Value = returnDate;
                        }
                    }
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
                        string queryString = "select itemAccn,itemTitle,itemAuthor,reserveLocation from reservation_list where brrId=@brrId and itemAccn!=''";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBook.Rows.Add(dataReader["itemAccn"].ToString(), dataReader["itemTitle"].ToString(), "", "",
                                    dataReader["itemAuthor"].ToString(), dataReader["reserveLocation"].ToString());
                            }
                            Application.DoEvents();
                        }
                        dataReader.Close();
                        dgvBook.ClearSelection();

                        DateTime currDate = DateTime.Now;
                        string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000"), returnDate = "";
                        foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                        {
                            queryString = "select itemCat,itemSubCat,itemSubject from item_details where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[0].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvRow.Cells[2].Value = dataReader["itemCat"].ToString();
                                    dgvRow.Cells[3].Value = dataReader["itemSubject"].ToString();
                                    itemCat = dataReader["itemCat"].ToString();
                                    itemSubCat = dataReader["itemSubCat"].ToString();
                                }
                                dataReader.Close();

                                queryString = "select issueDays from item_subcategory where catName=@catName and subCatName=@subCatName";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                                mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubCat);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                                    }
                                }
                                dataReader.Close();
                                currDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                                returnDate = currDate.Day.ToString("00") + "/" + currDate.Month.ToString("00") + "/" + currDate.Year.ToString("0000");
                                dgvRow.Cells[6].Value = issueDate;
                                dgvRow.Cells[7].Value = returnDate;
                            }
                        }
                        mysqlConn.Close();
                    }
                    catch
                    {

                    }
                }
                numDay.Value = 1;
            }
            else
            {
                dgvBook.Rows.Clear();
            }
        }

        private void dgvBook_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnIssueBook.Enabled = true;
            btnReset.Enabled = true;
        }

        private void dgvBook_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(dgvBook.Rows.Count==0)
            {
                btnIssueBook.Enabled = false;
                btnReset.Enabled = false;
            }
        }

        private void txtbAccn_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                btnAdd_Click(null, null);
            }
        }

        private void bWorkerNotification_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                mngmntMail = "";reciverId1 = "";
                htmlBody = false;
                if (Convert.ToBoolean(jsonObj["IssueMail"].ToString()))
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
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["IssueMailTemplate"].ToString());
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
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
                        sqltCommnd.CommandText = "select brrName,brrAddress,brrMailId,brrContact,addnlMail from borrowerDetails where brrId=@brrId";
                        sqltCommnd.Parameters.AddWithValue("@brrId", issuedBorrower);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                reciverName = dataReader["brrName"].ToString();
                                reciverAddress = dataReader["brrAddress"].ToString();
                                reciverId = dataReader["brrMailId"].ToString();
                                reciverId1 = dataReader["addnlMail"].ToString();
                            }
                        }
                        dataReader.Close();

                        sqltCommnd.CommandText = "select mngmntMail from generalSettings";
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                mngmntMail = dataReader["mngmntMail"].ToString();
                            }
                        }
                        dataReader.Close();
                        sqltConn.Clone();
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
                            string queryString = "select * from notice_template where noticeType='Mail' and tempName=@tempName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["IssueMailTemplate"].ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
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
                                    reciverId1 = dataReader["addnlMail"].ToString();
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
                            mysqlConn.Close();
                        }
                        catch
                        {

                        }
                    }

                    blockList = Properties.Settings.Default.blockedMail.Split('$').ToList();

                    if (blockList.IndexOf(reciverId) == -1)
                    {
                        SmtpClient smtpServer = new SmtpClient(Properties.Settings.Default.mailHost, Convert.ToInt32(Properties.Settings.Default.mailPort));
                        MailMessage mailMessage = new MailMessage();
                        //======================================SMTP SETTINGS===================
                        smtpServer.UseDefaultCredentials = false;
                        smtpServer.Credentials = new NetworkCredential(Properties.Settings.Default.mailId, Properties.Settings.Default.mailPassword);
                        smtpServer.EnableSsl = Convert.ToBoolean(Properties.Settings.Default.mailSsl);
                        try
                        {
                            mailMessage.From = new MailAddress(sederId);
                            mailMessage.To.Add(reciverId);
                            if (Properties.Settings.Default.mailCarbonCopy)
                            {
                                if (mngmntMail != "")
                                {
                                    mailMessage.CC.Add(mngmntMail);
                                }
                            }
                            mailMessage.Subject = jsonObj["IssueMailTemplate"].ToString();
                            mailMessage.IsBodyHtml = htmlBody;
                            mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", string.Join(",", issuedAccession)).
                                 Replace("[$ItemTitle$]", string.Join(",", issuedTitle)).Replace("[$ItemAuthor$]", string.Join(",", issuedAuthor)).
                                 Replace("[$IssueDate$]", string.Join(",", issuedDate)).Replace("[$ExpectedDate$]", string.Join(",", expectedDate));

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
                if (Convert.ToBoolean(jsonObj["IssueSms"].ToString()))
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
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["IssueSmsTemplate"].ToString());
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                mailBody = dataReader["bodyText"].ToString();
                            }
                        }
                        dataReader.Close();

                        sqltCommnd.CommandText = "select brrName,brrAddress,brrMailId,brrContact from borrowerDetails where brrId=@brrId";
                        sqltCommnd.Parameters.AddWithValue("@brrId", issuedBorrower);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                reciverName = dataReader["brrName"].ToString();
                                reciverAddress = dataReader["brrAddress"].ToString();
                                reciverId = dataReader["brrContact"].ToString();
                            }
                        }
                        dataReader.Close();
                        sqltConn.Clone();
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
                            string queryString = "select * from notice_template where noticeType='Sms' and tempName=@tempName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["IssueSmsTemplate"].ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    mailBody = dataReader["bodyText"].ToString();
                                }
                            }
                            dataReader.Close();
                            queryString = "select brrName,brrAddress,brrMailId,brrContact from borrower_details where brrId=@brrId";
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
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                        catch
                        {

                        }
                    }

                    blockList = Properties.Settings.Default.blockedContact.Split('$').ToList();
                    if (blockList.IndexOf(reciverId) == -1)
                    {
                        try
                        {
                            string smsApi = Properties.Settings.Default.smsApi.Replace("[$Message$]", mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                    Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                    Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", string.Join(",", issuedAccession)).
                                     Replace("[$ItemTitle$]", string.Join(",", issuedTitle)).Replace("[$ItemAuthor$]", string.Join(",", issuedAuthor)).
                                     Replace("[$IssueDate$]", string.Join(",", issuedDate)).Replace("[$ExpectedDate$]", string.Join(",", expectedDate))); ;
                            WebRequest webRequest = WebRequest.Create(smsApi.Replace("[$ContactNumber$]", reciverId).Replace("[$BorrowerName$]", reciverName));
                            webRequest.Timeout = 8000;
                            WebResponse webResponse = webRequest.GetResponse();
                        }
                        catch
                        {

                        }
                    }
                }
            }));
        }

        private void txtbAccn_TextChanged(object sender, EventArgs e)
        {
            if (txtbAccn.Text != "")
            {
                btnAdd.Enabled = true;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        if (autoCollAccession.IndexOf(txtbAccn.Text) != -1)
                        {
                            while (dataReader.Read())
                            {
                                itemCat = dataReader["itemCat"].ToString();
                                itemSubCat = dataReader["itemSubCat"].ToString();
                            }
                            dataReader.Close();

                            queryString = "select issueDays,notReference from itemSubCategory where catName=@catName and subCatName=@subCatName";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                            sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubCat);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                                }
                            }
                            dataReader.Close();
                        }
                    }
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
                        string queryString = "select itemCat,itemSubCat from item_details where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            if (autoCollAccession.IndexOf(txtbAccn.Text) != -1)
                            {
                                while (dataReader.Read())
                                {
                                    itemCat = dataReader["itemCat"].ToString();
                                    itemSubCat = dataReader["itemSubCat"].ToString();
                                }
                                dataReader.Close();

                                queryString = "select issueDays,notReference from item_subcategory where catName=@catName and subCatName=@subCatName";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                                mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubCat);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                        mysqlConn.Close();
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                numDay.Value = 1;
                btnAdd.Enabled = false;
            }
        }

        private void txtbIssueDate_Leave(object sender, EventArgs e)
        {
            string compareString = txtbIssueDate.Text.Substring(0, 2);
            bool isOK = Regex.IsMatch(compareString, @"[01-31]");
            if (isOK)
            {
                compareString = txtbIssueDate.Text.Substring(txtbIssueDate.Text.IndexOf("/") + 1, 2);
                isOK = Regex.IsMatch(compareString, @"[01-12]");
                if (isOK)
                {
                    try
                    {
                        compareString = txtbIssueDate.Text.Substring(txtbIssueDate.Text.LastIndexOf("/") + 1, 4);
                        isOK = Regex.IsMatch(compareString, @"[1900-3000]");
                        if (isOK)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Year not in correct format." + Environment.NewLine + "Required format - (dd/MM/yyyy)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SendKeys.Send("+{TAB}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Year not in correct format." + Environment.NewLine + "Required format - (dd/MM/yyyy)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SendKeys.Send("+{TAB}");
                    }
                }
                else
                {
                    MessageBox.Show("Month not in correct format." + Environment.NewLine + "Required format - (dd/MM/yyyy)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SendKeys.Send("+{TAB}");
                }
            }
            else
            {
                MessageBox.Show("Days not in correct format." + Environment.NewLine + "Required format - (dd/MM/yyyy)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SendKeys.Send("+{TAB}");
            }
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            if (txtbBrrId.Text == "")
            {
                MessageBox.Show("Please enter the Borrower id", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrId.Select();
                return;
            }
            if (dgvBook.Rows.Count > 0)
            {
                btnIssueBook.Enabled = false;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    int issuedItems = Convert.ToInt32(txtbIssueditem.Text) + dgvBook.Rows.Count;
                    string queryString = "";
                    issuedAccession.Clear();
                    issuedTitle.Clear();
                    issuedAuthor.Clear();
                    issuedDate.Clear();
                    expectedDate.Clear();

                    foreach (DataGridViewRow dataRow in dgvBook.Rows)
                    {
                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "update itemDetails set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[0].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();

                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "INSERT INTO issuedItem (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issuedBy) " +
                         " values (@brrId,@itemAccession" +
                         ",'" + FormatDate.getAppFormat(dataRow.Cells["isueDate"].Value.ToString()) + "','" + FormatDate.getAppFormat(dataRow.Cells["returnDate"].Value.ToString()) + "'" +
                         ",'" + false + "','" + currentUser + "')";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells["accnNo"].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();

                        sqltCommnd.CommandText = "delete from reservationList where itemAccn=@itemAccn";
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells["accnNo"].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();

                        autoCollAccession.Remove(dataRow.Cells["accnNo"].Value.ToString());
                        issuedBorrower = txtbBrrId.Text;
                        issuedAccession.Add(dataRow.Cells["accnNo"].Value.ToString());
                        issuedTitle.Add(dataRow.Cells[1].Value.ToString());
                        issuedAuthor.Add(dataRow.Cells[4].Value.ToString());
                        issuedDate.Add(dataRow.Cells["isueDate"].Value.ToString());
                        expectedDate.Add(dataRow.Cells["returnDate"].Value.ToString());
                    }
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
                    int issuedItems = Convert.ToInt32(txtbIssueditem.Text) + dgvBook.Rows.Count;
                    string queryString = "";
                    issuedAccession.Clear();
                    issuedTitle.Clear();
                    issuedAuthor.Clear();
                    issuedDate.Clear();
                    expectedDate.Clear();

                    foreach (DataGridViewRow dataRow in dgvBook.Rows)
                    {
                        queryString = "update item_details set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[0].Value.ToString());
                        mysqlCmd.ExecuteNonQuery();

                        queryString = "INSERT INTO issued_item (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issuedBy) " +
                         " values (@brrId,@itemAccession" +
                         ",'" + FormatDate.getAppFormat(dataRow.Cells["isueDate"].Value.ToString()) + "','" + FormatDate.getAppFormat(dataRow.Cells["returnDate"].Value.ToString()) + "'" +
                         ",'" + false + "','" + currentUser + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells["accnNo"].Value.ToString());
                        mysqlCmd.ExecuteNonQuery();

                        queryString = "delete from reservation_list where itemAccn=@itemAccn";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells["accnNo"].Value.ToString());
                        mysqlCmd.ExecuteNonQuery();

                        autoCollAccession.Remove(dataRow.Cells["accnNo"].Value.ToString());
                        issuedBorrower = txtbBrrId.Text;
                        issuedAccession.Add(dataRow.Cells["accnNo"].Value.ToString());
                        issuedTitle.Add(dataRow.Cells[1].Value.ToString());
                        issuedAuthor.Add(dataRow.Cells[4].Value.ToString());
                        issuedDate.Add(dataRow.Cells["isueDate"].Value.ToString());
                        expectedDate.Add(dataRow.Cells["returnDate"].Value.ToString());
                    }
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Items issued successfully !";
                showNotification();

                if (globalVarLms.issueReciept)
                {
                    generateIssueReciept();
                }
                try
                {
                    jsonObj = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(jsonObj["IssueMail"].ToString()) || Convert.ToBoolean(jsonObj["IssueSms"].ToString()))
                    {
                        bWorkerNotification.RunWorkerAsync();
                    }
                }
                catch
                {

                }
                dgvBook.Rows.Clear();
                dgvIssuedBook.Rows.Clear();
                txtbBrrId.Clear();
                txtbBrrName.Clear();
                txtbCategory.Clear();
                txtbBrrRnwDate.Clear();
                txtbIssueditem.Text = 0.ToString();
                txtbFees.Text = 0.00.ToString("0.00");
                rdbIsbn.Checked = false;
                rdbAccn.Checked = false;
                txtbAccn.Enabled = false;
                txtbIsbn.Enabled = false;
                txtbIsbn.Clear();
                txtbAccn.Clear();
                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
            }
            else
            {
                MessageBox.Show("Please add some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void generateIssueReciept()
        {
            string folderPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch
                {

                }
            }
            Directory.CreateDirectory(folderPath);
            string fileName = "tempReport";
            if (File.Exists(folderPath + @"\" + fileName + ".pdf"))
            {
                File.Delete(folderPath + @"\" + fileName + ".pdf");
            }
            fileName = folderPath + @"\" + fileName + ".pdf";
            System.Drawing.Image instLogo = null;
            string instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        instName = dataReader["instName"].ToString();
                        instAddress = dataReader["instAddress"].ToString();
                        instContact = dataReader["instContact"].ToString();
                        instWebsite = dataReader["instWebsite"].ToString();
                        instMail = dataReader["instMail"].ToString();
                        cuurShort = dataReader["currShortName"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            instLogo = Properties.Resources.NoImageAvailable;
                        }
                    }
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
                    string queryString = "select * from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            instName = dataReader["instName"].ToString();
                            instAddress = dataReader["instAddress"].ToString();
                            instContact = dataReader["instContact"].ToString();
                            instWebsite = dataReader["instWebsite"].ToString();
                            instMail = dataReader["instMail"].ToString();
                            cuurShort = dataReader["currShortName"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                instLogo = Properties.Resources.NoImageAvailable;
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
            if (globalVarLms.recieptPaper == "A4")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    Document pdfToCreate = new Document(PageSize.A4);
                    pdfToCreate.SetMargins(40, 10, 160, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 70f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(50f, 50f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 12);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 44f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = "Call/website/email : " + instContact + " | " + instWebsite + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(65, pdfToCreate.PageSize.Height - 56f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 9);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(410, pdfToCreate.PageSize.Height - 68f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(410, pdfToCreate.PageSize.Height - 58f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText("Prepared Date : " + FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.LineTo(585, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 575;

                    PdfPTable tempTable = new PdfPTable(2);
                    float[] columnWidth = { 80, 630 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Issued By :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    //============Shop Details====================
                    tempTable = new PdfPTable(4);
                    columnWidth =new float[] { 60, 120, 450,80 };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Due Date", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    tempTable.SetTotalWidth(columnWidth);
                    int itemCount = 1;
                    foreach(DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells["returnDate"].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);
                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The issue reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 85f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }
            else if (globalVarLms.recieptPaper == "Roll Paper 80 x 297 mm")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(315f, 1169f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(200, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(200, pdfToCreate.PageSize.Height - 46f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(305, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 295;

                    PdfPTable tempTable = new PdfPTable(2);
                    float[] columnWidth = { 65, 230 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Issued By :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    pdfTable.TotalWidth = 295;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Due Date", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    pdfTable.TotalWidth = 295;
                   
                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells["returnDate"].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);
                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The issue reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }
            else
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(189f, 12897f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                  BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(90, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(90, pdfToCreate.PageSize.Height - 46f);
                    }
                    DateTime currentDate = DateTime.Now;
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "/" + currentDate.Month.ToString("00") + "/" + currentDate.Year.ToString("0000")) +
                " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(179, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 169;

                    PdfPTable tempTable = new PdfPTable(2);
                    float[] columnWidth = { 65, 104 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Issued By :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    //============Shop Details====================
                    tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[0].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[1].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Due Date", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;
                    
                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[0].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[1].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(dgvRow.Cells["returnDate"].Value.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);
                        itemCount++;
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The issue reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }

            // Create the printer settings for our printer
            var printerSettings = new PrinterSettings
            {
                PrinterName = globalVarLms.recieptPrinter,
                Copies = (short)1,
                FromPage = (short)1,
            };

            // Create our page settings for the paper size selected
            var pageSettings = new PageSettings(printerSettings)
            {
                Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0),
            };
            foreach (PaperSize paperSize in printerSettings.PaperSizes)
            {
                //MessageBox.Show(paperSize.PaperName + "," + paperSize.Width + "," + paperSize.Height);
                if (paperSize.PaperName == "A4")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
                else if (paperSize.PaperName == "Roll Paper 80 x 297 mm")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
            }
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(fileName))
            {
                using (var printDocument = pdfDocument.CreatePrintDocument())
                {
                    printDocument.PrinterSettings = printerSettings;
                    printDocument.DefaultPageSettings = pageSettings;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                    //https://stackoverflow.com/questions/6103705/how-can-i-send-a-file-document-to-the-printer-and-have-it-print
                }
            }
        }

        public void getAccession()
        {
            string queryString = "", catName = null, subCatName = null;
            string[] catSubcatList = catAccess.Split('$');
            autoCollAccession.Clear();
            accessibleItem.Clear();
            autoCollIsbn.Clear();

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = null;
                SQLiteDataReader dataReader = null;

                foreach (string catSubCat in catSubcatList)
                {
                    catName = catSubCat.Substring(0, catSubCat.IndexOf("("));
                    subCatName = catSubCat.Replace(catName + "(", "");
                    subCatName = subCatName.Substring(0, subCatName.LastIndexOf(")"));

                    queryString = "select itemAccession from itemDetails where itemCat=@itemCat and" +
                     " itemSubCat=@itemSubCat and [itemAvailable]='" + true + "' and [isLost]='" + false + "'";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemCat", catName);
                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", subCatName);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        accessibleItem.AddRange((from IDataRecord r in dataReader
                                                 select (string)r["itemAccession"]
                           ).ToArray());
                    }
                    dataReader.Close();
                }

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select itemAccession from itemDetails";
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollAccession.AddRange((from IDataRecord r in dataReader
                                           select (string)r["itemAccession"]
                       ).ToArray());
                }
                dataReader.Close();

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select itemIsbn from itemDetails";
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollIsbn.AddRange((from IDataRecord r in dataReader
                                            select (string)r["itemIsbn"]
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
                    MySqlDataReader dataReader = null;

                    foreach (string catSubCat in catSubcatList)
                    {
                        catName = catSubCat.Substring(0, catSubCat.IndexOf("("));
                        subCatName = catSubCat.Replace(catName + "(", "");
                        subCatName = subCatName.Substring(0, subCatName.LastIndexOf(")"));

                        queryString = "select itemAccession from item_details where itemCat=@itemCat and" +
                         " itemSubCat=@itemSubCat and itemAvailable='" + true + "' and isLost='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", catName);
                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", subCatName);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            accessibleItem.AddRange((from IDataRecord r in dataReader
                                                     select (string)r["itemAccession"]
                               ).ToArray());
                        }
                        dataReader.Close();
                    }

                    queryString = "select itemAccession from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollAccession.AddRange((from IDataRecord r in dataReader
                                                    select (string)r["itemAccession"]
                           ).ToArray());
                    }
                    dataReader.Close();

                    queryString = "select itemIsbn from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollIsbn.AddRange((from IDataRecord r in dataReader
                                               select (string)r["itemIsbn"]
                          ).ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            numDay.Enabled = true;
            dtpIssue.Enabled = true;
        }

        void tooTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Segoe UI", 9.0f);
            Brush b = new SolidBrush(Color.WhiteSmoke);
            e.Graphics.FillRectangle(b, e.Bounds);
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.Red, new PointF(2, 2));
        }

        public static DateTime AddBusinessDays(DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);

            if (Properties.Settings.Default.holiDay == "Monday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Monday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            else if (Properties.Settings.Default.holiDay == "Tuesday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Tuesday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            else if (Properties.Settings.Default.holiDay == "Wednesday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Wednesday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            else if (Properties.Settings.Default.holiDay == "Thursday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Thursday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            else if (Properties.Settings.Default.holiDay == "Friday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Friday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            else if (Properties.Settings.Default.holiDay == "Saturday")
            {
                for (var i = 0; i < unsignedDays; i++)
                {
                    do
                    {
                        current = current.AddDays(sign);
                    }
                    while (current.DayOfWeek == DayOfWeek.Saturday ||
                        current.DayOfWeek == DayOfWeek.Sunday);
                }
            }
            return current;
        }

    }
}
