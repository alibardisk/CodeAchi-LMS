using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class UcItemReissueReturn : UserControl
    {
        public ToolTip tooTip = new ToolTip();

        public UcItemReissueReturn()
        {
            InitializeComponent();

            tooTip.OwnerDraw = true;
            tooTip.Draw += new DrawToolTipEventHandler(tooTip_Draw);
        }

        JObject jsonObj;
        bool htmlBody = false; bool avoidFine = false, haveFine = false;
        string currentUser, jsonString = "", sederId = "", mailBody, issuedBorrower, reciverId, reciverAddress,
            reciverName, itemAuthor = "", mngmntMail, reciverId1;
        double finePerDay = 0, totalFine = 0; int daysLate = 0; DateTime expectReturn, returnDate, issueDate, reissueDate;
        List<string> issuedAccession = new List<string> { };
        List<string> issuedTitle = new List<string> { };
        List<string> issuedAuthor = new List<string> { };
        List<string> returndDate = new List<string> { };
        List<string> issuedDate = new List<string> { };
        List<string> expectedDate = new List<string> { };
        List<string> blockList = new List<string> { };
        List<string> tempReissueList = new List<string> { };
        AutoCompleteStringCollection autoCollAccession = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCollIsbn = new AutoCompleteStringCollection();

        public void UcItemReissueReturn_Load(object sender, EventArgs e)
        {
            timer2.Stop();
            timer3.Stop();
            lblBlink.Visible = false;
            lblMessage.Visible = false;
            lblReturnBlink.Visible = false;
            lblReturnMessage.Visible = false;
            lblNotification.Visible = false;
            txtbTitle.Enabled = false;
            txtbIsbn.Enabled = false;
            txtbMemberId.Enabled = false;
            txtbMemberName.Enabled = false;
            txtbIssueDate.Enabled = false;
            dtpReturn.Value = DateTime.Now;
            dtpReturn.CustomFormat = Properties.Settings.Default.dateFormat;
            txtbBarcode.Clear();
            txtbIsbn.Clear();
            txtbIssueDate.Clear();
            txtbMemberName.Clear();
            txtbMemberId.Clear();
            txtbTitle.Clear();
            txtbSearchBarcode.Clear();
            numDay.Value = 0;
            dgvBook.Rows.Clear();
            btnReissue.Enabled = false;
            btnReissueReset.Enabled = false;
            btnReturn.Enabled = false;
            btnReset.Enabled = false;
            currentUser = Properties.Settings.Default.currentUserId;
            btnRenew.Visible = false;
            jsonString = Properties.Settings.Default.notificationData;
            if (Properties.Settings.Default.reissueByAccn == true)
            {
                rdbRsAccn.Checked = true;
                txtbBarcode.Enabled = true;
                txtbBarcode.SelectAll();
            }
            else
            {
                rdbRsIsbn.Checked = true;
                txtbIsbn.Enabled = true;
                txtbIsbn.SelectAll();
            }
            if (Properties.Settings.Default.returnByAccn == true)
            {
                rdbAccn.Checked = true;
                txtbSearchBarcode.Enabled = true;
                txtbSearchBarcode.SelectAll();
            }
            else
            {
                rdbIsbn.Checked = true;
                txtbSearchIsbn.Enabled = true;
                txtbSearchIsbn.SelectAll();
            }

            txtbBarcode.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbBarcode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbBarcode.AutoCompleteCustomSource = autoCollAccession;

            txtbSearchBarcode.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbSearchBarcode.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbSearchBarcode.AutoCompleteCustomSource = autoCollAccession;

            txtbIsbn.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbIsbn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbIsbn.AutoCompleteCustomSource = autoCollIsbn;

            txtbSearchIsbn.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbSearchIsbn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbSearchIsbn.AutoCompleteCustomSource = autoCollIsbn;
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
                        if (fieldName == "lblAccession")
                        {
                            dgvBook.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbAccn.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            rdbRsAccn.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvBook.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            lblTitle.Text = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvBook.Columns[8].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            dgvBook.Columns[9].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblIsbn")
                        {
                            rdbIsbn.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            rdbRsIsbn.Text = fieldValue.Replace(fieldName + "=", "") + " :";
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
                            dgvBook.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrName")
                        {
                            lblBrName.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void btnReissue_Click(object sender, EventArgs e)
        {
            if (txtbBarcode.Text == "")
            {
                MessageBox.Show("Please enter an accession no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBarcode.Select();
                return;
            }
            issuedAccession.Clear();
            issuedTitle.Clear();
            issuedDate.Clear();
            expectedDate.Clear();
            reissueDate = DateTime.Now.Date;
            reissueDate = reissueDate.AddDays(Convert.ToDouble(numDay.Value));
            string currentDate = DateTime.Now.Day.ToString("00") +
                    "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            string strresIssue = reissueDate.Day.ToString("00") + "/" + reissueDate.Month.ToString("00") + "/" + reissueDate.Year.ToString("0000");
            if (!haveFine)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "update issuedItem set reissuedDate='" + currentDate + "' ,expectedReturnDate=" +
                        "'" + strresIssue + "',reissuedBy='" + currentUser + "' where itemAccession=@itemAccession and itemReturned='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
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
                    string queryString = "update issued_item set reissuedDate='" + currentDate + "' ,expectedReturnDate=" +
                        "'" + strresIssue + "',reissuedBy='" + currentUser + "' where itemAccession=@itemAccession and itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                timer1.Stop();
                lblNotification.Visible = false;
                lblUserMessage.Text = "Item reissued successfully !";
                showNotification();
                if (globalVarLms.reissueReciept)
                {
                    tempReissueList.Clear();
                    tempReissueList.Add(txtbBarcode.Text + "|" + FormatDate.getUserFormat(reissueDate.Day.ToString("00") + "/" + reissueDate.Month.ToString("00") + "/" + reissueDate.Year.ToString("0000")));
                    generateReissueReciept();
                }

                issuedBorrower = txtbMemberId.Text;
                issuedAccession.Add(txtbBarcode.Text);
                issuedTitle.Add(txtbTitle.Text);
                issuedDate.Add(FormatDate.getUserFormat(DateTime.Now.Day.ToString("00") +
                    "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000")));
                expectedDate.Add(FormatDate.getUserFormat(reissueDate.Day.ToString("00") + "/" + reissueDate.Month.ToString("00") + "/" + reissueDate.Year.ToString("0000")));

                txtbBarcode.Clear();
                txtbIsbn.Clear();
                txtbTitle.Clear();
                txtbMemberId.Clear();
                txtbMemberName.Clear();
                txtbIssueDate.Clear();
                numDay.Value = 1;
                jsonObj = JObject.Parse(jsonString);
                if (Convert.ToBoolean(jsonObj["ReissueMail"].ToString()) || Convert.ToBoolean(jsonObj["ReissueSms"].ToString()))
                {
                    bWorkerReissueNotification.RunWorkerAsync();
                }
            }
            else
            {
                if (!avoidFine)
                {
                    string strReturnDate = dtpReturn.Value.Day.ToString("00") + "/" + dtpReturn.Value.Month.ToString("00") + "/" + dtpReturn.Value.Year.ToString("0000");
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        SQLiteDataReader dataReader;
                        //string queryString = "select itemAccession,issueDate,expectedReturnDate from issuedItem where brrId=@brrId and [itemReturned]='" + false + "'";
                        string queryString = "select * from issuedItem inner join itemDetails inner join " +
                                             "itemSubCategory where itemDetails.itemAccession=issuedItem.itemAccession and " +
                                             "itemSubCategory.catName=itemDetails.itemCat and itemSubCategory.subCatName=itemDetails.itemSubCat " +
                                             "and issuedItem.brrId=@brrId and issuedItem.itemAccession=@itemAccession and issuedItem.itemReturned='" + false + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            FormPayment paymentForm = new FormPayment();

                            DataTable tempTable = new DataTable();
                            tempTable.Columns.Add("chkb", typeof(Boolean));
                            tempTable.Columns.Add("memberId", typeof(String));
                            tempTable.Columns.Add("accnNo", typeof(String));
                            tempTable.Columns.Add("itemTitle", typeof(String));
                            tempTable.Columns.Add("issueDate", typeof(String));
                            tempTable.Columns.Add("expDate", typeof(String));
                            tempTable.Columns.Add("returnDate", typeof(String));
                            tempTable.Columns.Add("fine", typeof(String));
                            while (dataReader.Read())
                            {
                                finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                returnDate = DateTime.Now.Date;
                                expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                if (finePerDay > 0 && daysLate > 0)
                                {
                                    tempTable.Rows.Add(false, txtbSerachId.Text, dataReader["itemAccession"].ToString(),
                                     dataReader["itemTitle"].ToString(), dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                    strReturnDate, Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00"));
                                }
                            }
                            dataReader.Close();

                            foreach (DataRow dataRow in tempTable.Rows)
                            {
                                if (Convert.ToDouble(dataRow[7].ToString()) > 0)
                                {
                                    queryString = "select * from paymentDetails where memberId=@memberId and feesDate='" + dataRow[4].ToString() + "' and itemAccn=@itemAccn";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                    sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        dataReader.Close();
                                        queryString = "update paymentDetails set feesDesc=:feesDesc,dueAmount='" + dataRow[7].ToString() + "'" +
                                       ",isPaid='" + false + "' where feesDate='" + dataRow[4].ToString() + "' and memberId=@memberId and itemAccn=@itemAccn";
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                        sqltCommnd.Parameters.AddWithValue("feesDesc", "Fine upto " + dataRow[6].ToString());
                                        sqltCommnd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dataReader.Close();
                                        queryString = "Insert Into paymentDetails (feesDate,memberId,itemAccn,feesDesc,dueAmount,isPaid,discountAmnt,invidCount)" +
                                                "values ('" + dataRow[4].ToString() + "',@memberId,@itemAccn"
                                                + ",@feesDesc,'" + dataRow[7].ToString() + "','" + false + "','" + 0 + "','" + 0 + "')";
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                        sqltCommnd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow[6].ToString());
                                        sqltCommnd.ExecuteNonQuery();
                                    }
                                }
                                dataReader.Close();
                            }
                            dataReader.Close();

                            paymentForm.txtbBrrId.Text = txtbMemberId.Text;
                            globalVarLms.itemList.Clear();
                            paymentForm.txtbBrrId.Enabled = false;
                            paymentForm.Show();
                            foreach (Control c in this.Parent.Controls)
                            {
                                if (c.Name != "panelHeader" && c.Name != "panelFooter")
                                {
                                    c.Enabled = false;
                                }
                            }
                            while (paymentForm.Visible)
                            {
                                paymentForm.BringToFront();
                                Application.DoEvents();
                            }
                            sqltConn.Close();
                            if (globalVarLms.itemList.Count > 0)
                            {
                                sqltConn = ConnectionClass.sqliteConnection();
                                if (sqltConn.State == ConnectionState.Closed)
                                {
                                    sqltConn.Open();
                                }
                                sqltCommnd = sqltConn.CreateCommand();
                                tempReissueList.Clear();
                                foreach (string itemAccn in globalVarLms.itemList)
                                {
                                    queryString = "update issuedItem set reissuedDate='" + currentDate + "' ,expectedReturnDate=" +
                                    "'" + strresIssue + "',reissuedBy='" + currentUser + "' where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccn);
                                    sqltCommnd.ExecuteNonQuery();

                                    tempReissueList.Add(itemAccn + "|" + FormatDate.getUserFormat(strresIssue));
                                }
                                globalVarLms.backupRequired = true;

                                if (globalVarLms.reissueReciept)
                                {
                                    generateReissueReciept();
                                }
                                timer1.Stop();
                                lblNotification.Visible = false;
                                lblUserMessage.Text = "Item reissued successfully !";
                                showNotification();

                                issuedBorrower = txtbMemberId.Text;
                                issuedAccession.Add(txtbBarcode.Text);
                                issuedTitle.Add(txtbTitle.Text);
                                issuedDate.Add(FormatDate.getUserFormat(currentDate));
                                expectedDate.Add(FormatDate.getUserFormat(strresIssue));
                                txtbBarcode.Clear();
                                txtbIsbn.Clear();
                                txtbTitle.Clear();
                                txtbMemberId.Clear();
                                txtbMemberName.Clear();
                                txtbIssueDate.Clear();
                                numDay.Maximum = 365;
                                numDay.Value = 1;
                                sqltConn.Close();
                                try
                                {
                                    jsonObj = JObject.Parse(jsonString);
                                    if (Convert.ToBoolean(jsonObj["ReissueMail"].ToString()) || Convert.ToBoolean(jsonObj["ReissueSms"].ToString()))
                                    {
                                        bWorkerReissueNotification.RunWorkerAsync();
                                    }
                                }
                                catch
                                {

                                }
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
                        string queryString = "select * from issued_item inner join item_details inner join " +
                                             "item_subcategory where item_details.itemAccession=issued_item.itemAccession and " +
                                             "item_subcategory.catName=item_details.itemCat and item_subcategory.subCatName=item_details.itemSubCat " +
                                             "and issued_item.brrId=@brrId and issued_item.itemAccession=@itemAccession and issued_item.itemReturned='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            FormPayment paymentForm = new FormPayment();

                            DataTable tempTable = new DataTable();
                            tempTable.Columns.Add("chkb", typeof(Boolean));
                            tempTable.Columns.Add("memberId", typeof(String));
                            tempTable.Columns.Add("accnNo", typeof(String));
                            tempTable.Columns.Add("itemTitle", typeof(String));
                            tempTable.Columns.Add("issueDate", typeof(String));
                            tempTable.Columns.Add("expDate", typeof(String));
                            tempTable.Columns.Add("returnDate", typeof(String));
                            tempTable.Columns.Add("fine", typeof(String));
                            while (dataReader.Read())
                            {
                                finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                returnDate = DateTime.Now.Date;
                                expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                if (finePerDay > 0 && daysLate > 0)
                                {
                                    tempTable.Rows.Add(false, txtbSerachId.Text, dataReader["itemAccession"].ToString(),
                                     dataReader["itemTitle"].ToString(), dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                    strReturnDate, Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00"));
                                }
                            }
                            dataReader.Close();

                            foreach (DataRow dataRow in tempTable.Rows)
                            {
                                if (Convert.ToDouble(dataRow[7].ToString()) > 0)
                                {
                                    queryString = "select * from payment_details where memberId=@memberId and feesDate='" + dataRow[4].ToString() + "' and itemAccn=@itemAccn";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        dataReader.Close();
                                        queryString = "update payment_details set feesDesc=@feesDesc,dueAmount='" + dataRow[7].ToString() + "'" +
                                       ",isPaid='" + false + "' where feesDate='" + dataRow[4].ToString() + "' and memberId=@memberId and itemAccn=@itemAccn";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                        mysqlCmd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow[6].ToString());
                                        mysqlCmd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        dataReader.Close();
                                        queryString = "Insert Into payment_details (feesDate,memberId,itemAccn,feesDesc,dueAmount,isPaid,discountAmnt,invidCount)" +
                                                "values ('" + dataRow[4].ToString() + "',@memberId,@itemAccn"
                                                + ",@feesDesc,'" + dataRow[7].ToString() + "','" + false + "','" + 0 + "','" + 0 + "')";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbMemberId.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow[2].ToString());
                                        mysqlCmd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow[6].ToString());
                                        mysqlCmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            paymentForm.txtbBrrId.Text = txtbMemberId.Text;
                            globalVarLms.itemList.Clear();
                            paymentForm.txtbBrrId.Enabled = false;
                            paymentForm.Show();
                            foreach (Control c in this.Parent.Controls)
                            {
                                if (c.Name != "panelHeader" && c.Name != "panelFooter")
                                {
                                    c.Enabled = false;
                                }
                            }
                            while (paymentForm.Visible)
                            {
                                paymentForm.BringToFront();
                                Application.DoEvents();
                            }
                            mysqlConn.Close();
                            if (globalVarLms.itemList.Count > 0)
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
                                tempReissueList.Clear();
                                foreach (string itemAccn in globalVarLms.itemList)
                                {
                                    queryString = "update issued_item set reissuedDate='" + currentDate + "' ,expectedReturnDate=" +
                                    "'" + strresIssue + "',reissuedBy='" + currentUser + "' where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemAccn);
                                    mysqlCmd.ExecuteNonQuery();

                                    tempReissueList.Add(itemAccn + "|" + FormatDate.getUserFormat(strresIssue));
                                }
                                globalVarLms.backupRequired = true;

                                if (globalVarLms.reissueReciept)
                                {
                                    generateReissueReciept();
                                }
                                timer1.Stop();
                                lblNotification.Visible = false;
                                lblUserMessage.Text = "Item reissued successfully !";
                                showNotification();

                                issuedBorrower = txtbMemberId.Text;
                                issuedAccession.Add(txtbBarcode.Text);
                                issuedTitle.Add(txtbTitle.Text);
                                issuedDate.Add(FormatDate.getUserFormat(currentDate));
                                expectedDate.Add(FormatDate.getUserFormat(strresIssue));
                                txtbBarcode.Clear();
                                txtbIsbn.Clear();
                                txtbTitle.Clear();
                                txtbMemberId.Clear();
                                txtbMemberName.Clear();
                                txtbIssueDate.Clear();
                                numDay.Maximum = 365;
                                numDay.Value = 1;
                                try
                                {
                                    jsonObj = JObject.Parse(jsonString);
                                    if (Convert.ToBoolean(jsonObj["ReissueMail"].ToString()) || Convert.ToBoolean(jsonObj["ReissueSms"].ToString()))
                                    {
                                        bWorkerReissueNotification.RunWorkerAsync();
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        mysqlConn.Close();
                    }
                    foreach (Control c in this.Parent.Controls)
                    {
                        c.Enabled = true;
                    }
                }
            }
            dtpReturn.Value = DateTime.Now;
        }

        private void generateReissueReciept()
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
            string instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "", queryString = "";
            SQLiteConnection sqltConn = null;
            SQLiteCommand sqltCommnd = null;
            SQLiteDataReader dataReader = null;
            MySqlConnection mysqlConn = null;
            MySqlCommand mysqlCmd = null;
            MySqlDataReader mydataReader = null;
            if (globalVarLms.sqliteData)
            {
                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select * from generalSettings";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
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
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }

                    queryString = "select * from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mydataReader = mysqlCmd.ExecuteReader();
                    if (mydataReader.HasRows)
                    {
                        while (mydataReader.Read())
                        {
                            instName = mydataReader["instName"].ToString();
                            instAddress = mydataReader["instAddress"].ToString();
                            instContact = mydataReader["instContact"].ToString();
                            instWebsite = mydataReader["instWebsite"].ToString();
                            instMail = mydataReader["instMail"].ToString();
                            cuurShort = mydataReader["currShortName"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(mydataReader["instLogo"].ToString());
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
                    mydataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            string[] accnList;
            string itemTitle = "";
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

                    pdfCell = new PdfPCell(new Phrase(txtbMemberId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbMemberName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Reissued By : ", smallFontBold));
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
                    columnWidth = new float[] { 60, 120, 450, 80 };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
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
                    foreach (string reissueItem in tempReissueList)
                    {
                        accnList = reissueItem.Split('|');
                        if (globalVarLms.sqliteData)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "select itemTitle from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemTitle = dataReader["itemTitle"].ToString();
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

                                queryString = "select itemTitle from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                                mysqlCmd.CommandTimeout = 99999;
                                mydataReader = mysqlCmd.ExecuteReader();
                                if (mydataReader.HasRows)
                                {
                                    while (mydataReader.Read())
                                    {
                                        itemTitle = mydataReader["itemTitle"].ToString();
                                    }
                                }
                                mydataReader.Close();
                                mysqlConn.Close();
                            }
                            catch
                            {

                            }
                        }

                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[0], smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemTitle, smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[1], smallFont));
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
                    pdfCell = new PdfPCell(new Phrase("The reissue reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 83f, pdfContent);

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
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "-" + currentDate.Month.ToString("00") + "-" + currentDate.Year.ToString("0000")) +
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

                    pdfCell = new PdfPCell(new Phrase(txtbMemberId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbMemberName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Reissued By : ", smallFontBold));
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
                    tempTable.TotalWidth = 295;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
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
                    tempTable.TotalWidth = 295;
                    int itemCount = 1;
                    foreach (string reissueItem in tempReissueList)
                    {
                        accnList = reissueItem.Split('|');
                        if (globalVarLms.sqliteData)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "select itemTitle from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemTitle = dataReader["itemTitle"].ToString();
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

                                queryString = "select itemTitle from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                                mysqlCmd.CommandTimeout = 99999;
                                mydataReader = mysqlCmd.ExecuteReader();
                                if (mydataReader.HasRows)
                                {
                                    while (mydataReader.Read())
                                    {
                                        itemTitle = mydataReader["itemTitle"].ToString();
                                    }
                                }
                                mydataReader.Close();
                                mysqlConn.Close();
                            }
                            catch
                            {

                            }
                        }

                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[0], smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemTitle, smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[1], smallFont));
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
                    pdfCell = new PdfPCell(new Phrase("The reissue reciept generated by " + Application.ProductName, smallFont));
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
                    pdfContent.ShowText(FormatDate.getUserFormat(currentDate.Day.ToString("00") + "-" + currentDate.Month.ToString("00") + "-" + currentDate.Year.ToString("0000")) +
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

                    pdfCell = new PdfPCell(new Phrase(txtbMemberId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbMemberName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Reissed By : ", smallFontBold));
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
                    tempTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
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
                    pdfCell.BorderWidthBottom = 0.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;

                    int itemCount = 1;
                    foreach (string reissueItem in tempReissueList)
                    {
                        accnList = reissueItem.Split('|');
                        if (globalVarLms.sqliteData)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "select itemTitle from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemTitle = dataReader["itemTitle"].ToString();
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

                                queryString = "select itemTitle from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", accnList[0]);
                                mysqlCmd.CommandTimeout = 99999;
                                mydataReader = mysqlCmd.ExecuteReader();
                                if (mydataReader.HasRows)
                                {
                                    while (mydataReader.Read())
                                    {
                                        itemTitle = mydataReader["itemTitle"].ToString();
                                    }
                                }
                                mydataReader.Close();
                                mysqlConn.Close();
                            }
                            catch
                            {

                            }
                        }

                        pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[0], smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemTitle, smallFont));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        tempTable.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(accnList[1], smallFont));
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
                    pdfCell = new PdfPCell(new Phrase("The reissue reciept generated by " + Application.ProductName, smallFont));
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
            tempReissueList.Clear();
        }

        private void btnReissueReset_Click(object sender, EventArgs e)
        {
            txtbBarcode.Clear();
            txtbIsbn.Clear();
            txtbTitle.Clear();
            txtbMemberId.Clear();
            txtbMemberName.Clear();
            txtbIssueDate.Clear();
            numDay.Value = 1;
            timer1.Stop();
            lblNotification.Visible = false;
        }

        private void txtbBarcode_TextChanged(object sender, EventArgs e)
        {
            if (txtbBarcode.Text != "")
            {

            }
            else
            {
                btnRenew.Visible = false;
            }
        }

        private void txtbReturDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && char.IsLetter(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbReturDate_Leave(object sender, EventArgs e)
        {
            bool isOK = Regex.IsMatch(txtbReturDate.Text, @"[01-31]\/[1-12]\/[1900-3000]");
            if (!isOK)
            {
                isOK = Regex.IsMatch(txtbReturDate.Text, @"[01-31]\/[0][1-12]\/[1900-3000]");
                if (!isOK)
                {
                    MessageBox.Show("Date not in correct format." + Environment.NewLine + "Required format - (dd/MM/yyyy)", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbReturDate.SelectAll();
                }
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAll.Checked)
            {
                foreach (DataGridViewRow dataRow in dgvBook.Rows)
                {
                    dataRow.Cells[0].Value = true;
                    dgvBook.CurrentCell = dataRow.Cells[1];
                }
            }
            else
            {
                foreach (DataGridViewRow dataRow in dgvBook.Rows)
                {
                    dataRow.Cells[0].Value = false;
                    dgvBook.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            bool rowChecked = false;
            foreach (DataGridViewRow dataRow in dgvBook.Rows)
            {
                if (dataRow.Cells[0].Value.ToString() == "True")
                {
                    rowChecked = true;
                }
            }
            if (!rowChecked)
            {
                MessageBox.Show("Please check some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            totalFine = 0.00;
            string queryString = "";
            SQLiteConnection sqltConn = null;
            SQLiteCommand sqltCommnd = null;
            SQLiteDataReader dataReader = null;
            MySqlConnection mysqlConn = null;
            MySqlCommand mysqlCmd = null;
            MySqlDataReader sqldataReader = null;
            globalVarLms.itemList.Clear();

            foreach (DataGridViewRow dataRow in dgvBook.Rows)
            {
                if (dataRow.Cells[0].Value.ToString() == "True" && Convert.ToDouble(dataRow.Cells[7].Value.ToString()) > 0)
                {
                    if (globalVarLms.sqliteData)
                    {
                        sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        sqltCommnd = sqltConn.CreateCommand();

                        queryString = "select * from paymentDetails where feesDate='" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "'" +
                            " and memberId=@memberId and itemAccn='" + dataRow.Cells[2].Value.ToString() + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            dataReader.Close();
                            queryString = "update paymentDetails set feesDesc=:feesDesc,[dueAmount]='" + dataRow.Cells[7].Value.ToString() + "'," +
                                "invidCount='" + 0 + "',isPaid='" + false + "' where [feesDate]='" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "'" +
                            " and memberId=@memberId and itemAccn=@itemAccn";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("feesDesc", "Fine upto " + dataRow.Cells[6].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                        }
                        else
                        {
                            dataReader.Close();
                            queryString = "Insert Into paymentDetails (feesDate,memberId,itemAccn,feesDesc,dueAmount,isPaid,discountAmnt,invidCount)" +
                                    "values ('" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "',@memberId,@itemAccn,@feesDesc," +
                                    "'" + dataRow.Cells[7].Value.ToString() + "','" + false + "','" + 0 + "','" + 0 + "')";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow.Cells[6].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                        }
                        totalFine = totalFine + Convert.ToDouble(dataRow.Cells[7].Value.ToString());
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

                        queryString = "select * from payment_details where feesDate='" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "'" +
                            " and memberId=@memberId and itemAccn='" + dataRow.Cells[2].Value.ToString() + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        sqldataReader = mysqlCmd.ExecuteReader();
                        if (sqldataReader.HasRows)
                        {
                            sqldataReader.Close();
                            queryString = "update payment_details set feesDesc=@feesDesc,dueAmount='" + dataRow.Cells[7].Value.ToString() + "'," +
                                "invidCount='" + 0 + "',isPaid='" + false + "' where feesDate='" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "'" +
                            " and memberId=@memberId and itemAccn=@itemAccn";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow.Cells[6].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sqldataReader.Close();
                            queryString = "Insert Into payment_details (feesDate,memberId,itemAccn,feesDesc,dueAmount,isPaid,discountAmnt,invidCount)" +
                                    "values ('" + FormatDate.getAppFormat(dataRow.Cells[4].Value.ToString()) + "',@memberId,@itemAccn,@feesDesc," +
                                    "'" + dataRow.Cells[7].Value.ToString() + "','" + false + "','" + 0 + "','" + 0 + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@feesDesc", "Fine upto " + dataRow.Cells[6].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();
                        }
                        totalFine = totalFine + Convert.ToDouble(dataRow.Cells[7].Value.ToString());
                        mysqlConn.Close();
                    }
                }
                else if (dataRow.Cells[0].Value.ToString() == "True" && Convert.ToDouble(dataRow.Cells[7].Value.ToString()) == 0)
                {
                    globalVarLms.itemList.Add(dataRow.Cells[2].Value.ToString());
                }
            }

            if (totalFine > 0)
            {
                FormPayment paymentForm = new FormPayment();
                paymentForm.txtbBrrId.Text = txtbSerachId.Text;
                paymentForm.txtbBrrId.Enabled = false;
                paymentForm.Show();
                foreach (Control c in this.Parent.Controls)
                {
                    if (c.Name != "panelHeader" && c.Name != "panelFooter")
                    {
                        c.Enabled = false;
                    }
                }
                while (paymentForm.Visible)
                {
                    paymentForm.BringToFront();
                    Application.DoEvents();
                }
            }

            if (globalVarLms.itemList.Count > 0)
            {
                string reserveId = "", brrId = "";
                issuedAccession.Clear();
                issuedTitle.Clear();
                issuedAuthor.Clear();
                returndDate.Clear();
                issuedDate.Clear();
                expectedDate.Clear();
                foreach (DataGridViewRow dataRow in dgvBook.Rows)
                {
                    if (globalVarLms.itemList.IndexOf(dataRow.Cells[2].Value.ToString()) >= 0)
                    {
                        if (globalVarLms.sqliteData)
                        {
                            sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            sqltCommnd = sqltConn.CreateCommand();

                            queryString = "update itemDetails set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();

                            queryString = "update issuedItem set itemReturned='" + true + "',returnDate='" + FormatDate.getAppFormat(dataRow.Cells[6].Value.ToString()) + "',returnedBy='" + currentUser + "' where " +
                                "brrId=@brrId and itemAccession=@itemAccession and itemReturned='" + false + "'";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();

                            sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select id,brrId from reservationList where itemTitle=@itemTitle and itemAuthor=@itemAuthor and availableDate='' order by [id] asc limit 1"; ;
                            sqltCommnd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[3].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@itemAuthor", dataRow.Cells[8].Value.ToString());
                            dataReader = sqltCommnd.ExecuteReader();
                            reserveId = "";
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    reserveId = dataReader["id"].ToString();
                                    brrId = dataReader["brrId"].ToString();
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

                            queryString = "update item_details set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "update issued_item set itemReturned='" + true + "',returnDate='" + FormatDate.getAppFormat(dataRow.Cells[6].Value.ToString()) + "',returnedBy='" + currentUser + "' where " +
                                "brrId=@brrId and itemAccession=@itemAccession and itemReturned='" + false + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "select id,brrId from reservation_list where itemTitle=@itemTitle and itemAuthor=@itemAuthor and availableDate='' order by id asc limit 1"; ;
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[3].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@itemAuthor", dataRow.Cells[8].Value.ToString());
                            sqldataReader = mysqlCmd.ExecuteReader();
                            reserveId = "";
                            if (sqldataReader.HasRows)
                            {
                                while (sqldataReader.Read())
                                {
                                    reserveId = sqldataReader["id"].ToString();
                                    brrId = sqldataReader["brrId"].ToString();
                                }
                            }
                            sqldataReader.Close();
                            mysqlConn.Close();
                        }

                        if (reserveId != "")
                        {
                            FormReservation reserveItem = new FormReservation();
                            if (globalVarLms.sqliteData)
                            {
                                sqltConn = ConnectionClass.sqliteConnection();
                                if (sqltConn.State == ConnectionState.Closed)
                                {
                                    sqltConn.Open();
                                }
                                sqltCommnd = sqltConn.CreateCommand();

                                sqltCommnd.CommandText = "select brrName from borrowerDetails where brrId=@brrId";
                                sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        reserveItem.txtbName.Text = dataReader["brrName"].ToString();
                                    }
                                }
                                dataReader.Close();
                                reserveItem.txtbTitle.Text = dataRow.Cells[3].Value.ToString();
                                reserveItem.txtbAuthor.Text = dataRow.Cells[8].Value.ToString();
                                reserveItem.txtbLocation.Text = dataRow.Cells[9].Value.ToString();
                                reserveItem.txtbLocation.Enabled = false;
                                reserveItem.ShowDialog();
                                sqltCommnd.CommandText = "update reservationList set reserveLocation='" + globalVarLms.tempValue + "'," +
                                    " availableDate='" + FormatDate.getAppFormat(dataRow.Cells[6].Value.ToString()) + "',itemAccn=@itemAccn where id='" + reserveId + "'";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                                sqltCommnd.ExecuteNonQuery();
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

                                    queryString = "select brrName from borrower_details where brrId=@brrId";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                                    mysqlCmd.CommandTimeout = 99999;
                                    sqldataReader = mysqlCmd.ExecuteReader();
                                    if (sqldataReader.HasRows)
                                    {
                                        while (sqldataReader.Read())
                                        {
                                            reserveItem.txtbName.Text = sqldataReader["brrName"].ToString();
                                        }
                                    }
                                    sqldataReader.Close();
                                    mysqlConn.Close();
                                    reserveItem.txtbTitle.Text = dataRow.Cells[3].Value.ToString();
                                    reserveItem.txtbAuthor.Text = dataRow.Cells[8].Value.ToString();
                                    reserveItem.txtbLocation.Text = dataRow.Cells[9].Value.ToString();
                                    reserveItem.txtbLocation.Enabled = false;
                                    reserveItem.ShowDialog();

                                    mysqlConn = ConnectionClass.mysqlConnection();
                                    if (mysqlConn.State == ConnectionState.Closed)
                                    {
                                        mysqlConn.Open();
                                    }
                                    queryString = "update reservation_list set reserveLocation='" + globalVarLms.tempValue + "'," +
                                        " availableDate='" + FormatDate.getAppFormat(dataRow.Cells[6].Value.ToString()) + "',itemAccn=@itemAccn where id='" + reserveId + "'";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                                    mysqlCmd.ExecuteNonQuery();
                                    mysqlConn.Close();
                                }
                                catch
                                {

                                }
                            }
                            globalVarLms.tempValue = "";
                            try
                            {
                                jsonObj = JObject.Parse(jsonString);
                                if (Convert.ToBoolean(jsonObj["ArrivedMail"].ToString()) || Convert.ToBoolean(jsonObj["ArrivedSms"].ToString()))
                                {
                                    ItemArrivedNotification(dataRow.Cells[1].Value.ToString(), dataRow.Cells[2].Value.ToString(),
                                        dataRow.Cells[3].Value.ToString());
                                }
                            }
                            catch
                            {

                            }
                        }
                        issuedBorrower = dataRow.Cells[1].Value.ToString();
                        issuedAccession.Add(dataRow.Cells[2].Value.ToString());
                        issuedTitle.Add(dataRow.Cells[3].Value.ToString());
                        issuedAuthor.Add(dataRow.Cells[8].Value.ToString());
                        returndDate.Add(dataRow.Cells[6].Value.ToString());
                        issuedDate.Add(dataRow.Cells[4].Value.ToString());
                        expectedDate.Add(dataRow.Cells[5].Value.ToString());
                    }
                }
                if (globalVarLms.returnReciept)
                {
                    generateReturnReciept();
                }

                txtbSearchBarcode.Clear();
                txtbSerachId.Clear();
                dgvBook.Rows.Clear();
                dgvBook.ClearSelection();
                chkbAll.Checked = false;
                dtpReturn.Value = DateTime.Now;
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Items returned successfully !";
                showNotification();
                try
                {
                    jsonObj = JObject.Parse(jsonString);
                    if (Convert.ToBoolean(jsonObj["ReturnMail"].ToString()) || Convert.ToBoolean(jsonObj["ReturnSms"].ToString()))
                    {
                        bWorkerNotification.RunWorkerAsync();
                    }
                }
                catch
                {

                }
            }
            foreach (Control c in this.Parent.Controls)
            {
                c.Enabled = true;
            }
        }

        private void generateReturnReciept()
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
            if (globalVarLms.sqliteData)
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

                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select brrName from borrowerDetails where brrId=@brrId";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        brrName = dataReader["brrName"].ToString();
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

                    queryString = "select brrName from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrName = dataReader["brrName"].ToString();
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

                    pdfCell = new PdfPCell(new Phrase(txtbSerachId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Returned By : ", smallFontBold));
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
                    columnWidth = new float[] { 60, 120, 450, 80 };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Fine", smallFontBold));
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
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        if (globalVarLms.itemList.IndexOf(dgvRow.Cells[2].Value.ToString()) >= 0)
                        {
                            pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[7].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);
                            itemCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.LIGHT_GRAY;
                    pdfTable.AddCell(pdfCell);

                    if (totalFine > 0)
                    {
                        pdfCell = new PdfPCell(new Phrase("Total Fine : " + totalFine.ToString("0.00"), smallFontBold));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.PaddingBottom = 2;
                        pdfCell.BorderWidthBottom = 1f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        pdfTable.AddCell(pdfCell);
                    }

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The return reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 83f, pdfContent);

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

                    pdfCell = new PdfPCell(new Phrase(txtbSerachId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Returned By : ", smallFontBold));
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
                    tempTable.TotalWidth = 295;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Fine", smallFontBold));
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
                    tempTable.TotalWidth = 295;
                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        if (globalVarLms.itemList.IndexOf(dgvRow.Cells[2].Value.ToString()) >= 0)
                        {
                            pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[7].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);
                            itemCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    if (totalFine > 0)
                    {
                        pdfCell = new PdfPCell(new Phrase("Total Fine : " + totalFine.ToString("0.00"), smallFontBold));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.PaddingBottom = 2;
                        pdfCell.BorderWidthBottom = 1f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        pdfTable.AddCell(pdfCell);
                    }

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The return reciept generated by " + Application.ProductName, smallFont));
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

                    pdfCell = new PdfPCell(new Phrase(txtbSerachId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrName.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Returned By : ", smallFontBold));
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
                    tempTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("Sl. No.", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[2].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(dgvBook.Columns[3].HeaderText, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Fine", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 0.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    tempTable.TotalWidth = 169;

                    int itemCount = 1;
                    foreach (DataGridViewRow dgvRow in dgvBook.Rows)
                    {
                        if (globalVarLms.itemList.IndexOf(dgvRow.Cells[2].Value.ToString()) >= 0)
                        {
                            pdfCell = new PdfPCell(new Phrase(itemCount.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dgvRow.Cells[7].Value.ToString(), smallFont));
                            pdfCell.BorderWidth = 0; //1.5f;
                            pdfCell.BorderColor = BaseColor.DARK_GRAY;
                            pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                            tempTable.AddCell(pdfCell);
                            itemCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    if (totalFine > 0)
                    {
                        pdfCell = new PdfPCell(new Phrase("Total Fine : " + totalFine.ToString("0.00"), smallFontBold));
                        pdfCell.BorderWidth = 0; //1.5f;
                        pdfCell.PaddingBottom = 2;
                        pdfCell.BorderWidthBottom = 0.5f;
                        pdfCell.BorderColor = BaseColor.DARK_GRAY;
                        pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                        pdfTable.AddCell(pdfCell);
                    }

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The return reciept generated by " + Application.ProductName, smallFont));
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvBook.Rows.Clear();
            txtbSerachId.Clear();
            txtbSearchBarcode.Clear();
            txtbSearchIsbn.Clear();
        }

        private void bWorkerNotification_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                mngmntMail = ""; reciverId1 = "";
                htmlBody = false;
                if (Convert.ToBoolean(jsonObj["ReturnMail"].ToString()))
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReturnMailTemplate"].ToString());
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
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReturnMailTemplate"].ToString());
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
                            mysqlConn.Clone();
                        }
                        catch
                        {

                        }
                    }
                    blockList = Properties.Settings.Default.blockedMail.Split('$').ToList();

                    if (blockList.IndexOf(reciverId) == -1)
                    {
                        try
                        {
                            SmtpClient smtpServer = new SmtpClient(Properties.Settings.Default.mailHost, Convert.ToInt32(Properties.Settings.Default.mailPort));
                            MailMessage mailMessage = new MailMessage();
                            //======================================SMTP SETTINGS===================
                            smtpServer.UseDefaultCredentials = false;
                            smtpServer.Credentials = new NetworkCredential(Properties.Settings.Default.mailId, Properties.Settings.Default.mailPassword);
                            smtpServer.EnableSsl = Convert.ToBoolean(Properties.Settings.Default.mailSsl);

                            mailMessage.From = new MailAddress(sederId);
                            mailMessage.To.Add(reciverId);
                            if (Properties.Settings.Default.mailCarbonCopy)
                            {
                                if (mngmntMail != "")
                                {
                                    mailMessage.CC.Add(mngmntMail);
                                }
                            }
                            mailMessage.Subject = jsonObj["ReturnMailTemplate"].ToString();
                            mailMessage.IsBodyHtml = htmlBody;
                            mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", string.Join(",", issuedAccession)).
                                 Replace("[$ItemTitle$]", string.Join(",", issuedTitle)).Replace("[$ReturnDate$]", string.Join(",", returndDate)).
                                 Replace("[$IssueDate$]", string.Join(",", issuedDate)).Replace("[$ExpectedDate$]", string.Join(",", expectedDate)).
                                 Replace("[$ItemAuthor$]", string.Join(",", issuedAuthor));

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
                if (Convert.ToBoolean(jsonObj["ReturnSms"].ToString()))
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReturnSmsTemplate"].ToString());
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
                            string queryString = "select * from notice_emplate where noticeType='Sms' and tempName=@tempName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReturnSmsTemplate"].ToString());
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
                                while (dataReader.Read())
                                {
                                    reciverName = dataReader["brrName"].ToString();
                                    reciverAddress = dataReader["brrAddress"].ToString();
                                    reciverId = dataReader["brrContact"].ToString();
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Clone();
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
                                     Replace("[$ItemTitle$]", string.Join(",", issuedTitle)).Replace("[$ReturnDate$]", string.Join(",", returndDate)).
                                     Replace("[$IssueDate$]", string.Join(",", issuedDate)).Replace("[$ExpectedDate$]", string.Join(",", expectedDate))).
                                      Replace("[$ItemAuthor$]", string.Join(",", issuedAuthor));
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

        private void bWorkerReissueNotification_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                mngmntMail = ""; reciverId1 = ""; htmlBody = false;
                if (Convert.ToBoolean(jsonObj["ReissueMail"].ToString()))
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReissueMailTemplate"].ToString());
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
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReissueMailTemplate"].ToString());
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
                            mysqlConn.Clone();
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
                            mailMessage.Subject = jsonObj["ReissueMailTemplate"].ToString();
                            mailMessage.IsBodyHtml = htmlBody;
                            mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", string.Join(",", issuedAccession)).
                                 Replace("[$ItemTitle$]", issuedTitle[0]).Replace("[$ItemAuthor$]", itemAuthor).
                                 Replace("[$IssueDate$]", issuedDate[0]).Replace("[$ExpectedDate$]", expectedDate[0]);

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
                if (Convert.ToBoolean(jsonObj["ReissueSms"].ToString()))
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReissueSmsTemplate"].ToString());
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
                            mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReissueSmsTemplate"].ToString());
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
                                while (dataReader.Read())
                                {
                                    reciverName = dataReader["brrName"].ToString();
                                    reciverAddress = dataReader["brrAddress"].ToString();
                                    reciverId = dataReader["brrContact"].ToString();
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Clone();
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
                                 Replace("[$ItemTitle$]", issuedTitle[0]).Replace("[$ItemAuthor$]", itemAuthor).
                                 Replace("[$IssueDate$]", issuedDate[0]).Replace("[$ExpectedDate$]", expectedDate[0]));
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

        private void ItemArrivedNotification(string issuedBorrower, string itemAccession, string itemTitle)
        {
            mngmntMail = ""; reciverId1 = "";
            htmlBody = false;
            if (Convert.ToBoolean(jsonObj["ArrivedMail"].ToString()))
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ArrivedMailTemplate"].ToString());
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
                        mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ArrivedMailTemplate"].ToString());
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
                        mysqlConn.Clone();
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
                        mailMessage.Subject = jsonObj["ArrivedMailTemplate"].ToString();
                        mailMessage.IsBodyHtml = htmlBody;
                        mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                                Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", itemAccession).
                                 Replace("[$ItemTitle$]", itemTitle);

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
            if (Convert.ToBoolean(jsonObj["ArrivedSms"].ToString()))
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ArrivedSmsTemplate"].ToString());
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
                        mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ArrivedSmsTemplate"].ToString());
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
                            while (dataReader.Read())
                            {
                                reciverName = dataReader["brrName"].ToString();
                                reciverAddress = dataReader["brrAddress"].ToString();
                                reciverId = dataReader["brrContact"].ToString();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Clone();
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
                                Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", itemAccession).
                                 Replace("[$ItemTitle$]", itemTitle));
                        WebRequest webRequest = WebRequest.Create(smsApi.Replace("[$ContactNumber$]", reciverId).Replace("[$BorrowerName$]", reciverName));
                        webRequest.Timeout = 8000;
                        WebResponse webResponse = webRequest.GetResponse();
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void txtbSearchBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReturnItem();
            }
        }

        string brrName = "";
        public void ReturnItem()
        {
            if (txtbSearchBarcode.Text != "")
            {
                if (dgvBook.Rows.Count == 0)
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        SQLiteDataReader dataReader;

                        string queryString = "select brrId from issuedItem where itemAccession=@itemAccession and [itemReturned]='" + false + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbSearchBarcode.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            tooTip.Hide(txtbSearchBarcode);
                            while (dataReader.Read())
                            {
                                txtbSerachId.Text = dataReader["brrId"].ToString();
                            }
                        }
                        else
                        {
                            txtbSerachId.Text = "";
                            tooTip.AutomaticDelay = 1000;
                            tooTip.Show("Still not issued", txtbSearchBarcode);
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
                        string queryString = "select brrId from issued_item where itemAccession=@itemAccession and itemReturned='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbSearchBarcode.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            tooTip.Hide(txtbSearchBarcode);
                            while (dataReader.Read())
                            {
                                txtbSerachId.Text = dataReader["brrId"].ToString();
                            }
                        }
                        else
                        {
                            txtbSerachId.Text = "";
                            tooTip.AutomaticDelay = 1000;
                            tooTip.Show("Still not issued", txtbSearchBarcode);
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    if (dgvBook.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow dataRow in dgvBook.Rows)
                        {
                            if (dataRow.Cells[2].Value.ToString() == txtbSearchBarcode.Text)
                            {
                                dataRow.Cells[0].Value = true;
                                dgvBook.CurrentCell = dataRow.Cells[1];
                            }
                        }
                    }
                }
                else
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        SQLiteDataReader dataReader;

                        string queryString = "select brrId from issuedItem where itemAccession=@itemAccession and [itemReturned]='" + false + "'";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbSearchBarcode.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            tooTip.Hide(txtbSearchBarcode);

                            foreach (DataGridViewRow dataRow in dgvBook.Rows)
                            {
                                if (dataRow.Cells[2].Value.ToString() == txtbSearchBarcode.Text)
                                {
                                    dataRow.Cells[0].Value = true;
                                    dgvBook.CurrentCell = dataRow.Cells[1];
                                    txtbSearchBarcode.SelectAll();
                                }
                            }
                        }
                        else
                        {
                            txtbSerachId.Text = "";
                            tooTip.AutomaticDelay = 1000;
                            tooTip.Show("Still not issued", txtbSearchBarcode);

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
                        string queryString = "select brrId from issued_item where itemAccession=@itemAccession and itemReturned='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbSearchBarcode.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            tooTip.Hide(txtbSearchBarcode);

                            foreach (DataGridViewRow dataRow in dgvBook.Rows)
                            {
                                if (dataRow.Cells[2].Value.ToString() == txtbSearchBarcode.Text)
                                {
                                    dataRow.Cells[0].Value = true;
                                    dgvBook.CurrentCell = dataRow.Cells[1];
                                    txtbSearchBarcode.SelectAll();
                                }
                            }
                        }
                        else
                        {
                            txtbSerachId.Text = "";
                            tooTip.AutomaticDelay = 1000;
                            tooTip.Show("Still not issued", txtbSearchBarcode);

                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            else
            {
                tooTip.Hide(txtbSearchBarcode);
            }
        }

        private void txtbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReissueItem();
            }
        }

        public void ReissueItem()
        {
            if (txtbBarcode.Text != "")
            {
                avoidFine = false;
                haveFine = false;
                daysLate = 0;
                reissueDate = DateTime.Now.Date;
                int issueDay = 0;
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from issuedItem inner join itemDetails where " +
                        "itemDetails.itemAccession=issuedItem.itemAccession and issuedItem.itemAccession=@itemAccession and itemReturned='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        btnReissue.Enabled = true;
                        while (dataReader.Read())
                        {
                            txtbMemberId.Text = dataReader["brrId"].ToString();
                            txtbIssueDate.Text = FormatDate.getUserFormat(dataReader["issueDate"].ToString());
                            issueDate = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtbTitle.Text = dataReader["itemTitle"].ToString();
                            txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                            itemAuthor = dataReader["itemAuthor"].ToString();
                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        issueDay = Convert.ToInt32((expectReturn.Date - issueDate.Date).TotalDays);
                        if (issueDay > numDay.Maximum)
                        {
                            numDay.Maximum = issueDay;
                        }
                        numDay.Value = issueDay;
                        dataReader.Close();
                        Application.DoEvents();

                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "select * from borrowerDetails inner join borrowerSettings where borrowerDetails.brrCategory= borrowerSettings.catName and borrowerDetails.brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                txtbMemberName.Text = dataReader["brrName"].ToString();
                                if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                                {
                                    entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                                avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                            }
                            dataReader.Close();
                            Application.DoEvents();

                            if (renewDate.Date <= DateTime.Now.Date)
                            {
                                btnRenew.Visible = true;
                                tooTip.AutomaticDelay = 1000;
                                tooTip.Show("Membership expired, Please renew now.", txtbBarcode);
                                btnReissue.Enabled = false;
                            }
                            else
                            {
                                btnRenew.Visible = false;
                                btnReissue.Enabled = true;
                                btnReissueReset.Enabled = true;
                                tooTip.Hide(txtbBarcode);
                                if (!avoidFine)
                                {
                                    //=======================================================
                                    //queryString = "select itemAccession,issueDate,expectedReturnDate from issuedItem where brrId=@brrId and [itemReturned]='" + false + "'";
                                    queryString = "select * from issuedItem inner join itemDetails inner join " +
                                        "itemSubCategory where itemDetails.itemAccession=issuedItem.itemAccession and " +
                                        "itemSubCategory.catName=itemDetails.itemCat and itemSubCategory.subCatName=itemDetails.itemSubCat " +
                                        "and issuedItem.brrId=@brrId and issuedItem.itemReturned='" + false + "'";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        int dayLate = 0;
                                        DataTable tempTable = new DataTable();
                                        tempTable.Columns.Add("chkb", typeof(Boolean));
                                        tempTable.Columns.Add("accnNo", typeof(String));
                                        tempTable.Columns.Add("itemTitle", typeof(String));
                                        tempTable.Columns.Add("expDate", typeof(String));
                                        tempTable.Columns.Add("fine", typeof(String));
                                        while (dataReader.Read())
                                        {
                                            returnDate = DateTime.Now.Date;
                                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            if (expectReturn < reissueDate)
                                            {
                                                dayLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                                                finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                                if (dayLate < 0)
                                                {
                                                    dayLate = 0;
                                                }
                                                if (finePerDay > 0)
                                                {
                                                    if (dataReader["itemAccession"].ToString() == txtbBarcode.Text)
                                                    {
                                                        haveFine = true;
                                                    }
                                                    tempTable.Rows.Add(false, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                                        dataReader["expectedReturnDate"].ToString(), Math.Round(dayLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00"));
                                                }
                                            }
                                        }
                                        dataReader.Close();

                                        if (tempTable.Rows.Count > 0)
                                        {
                                            if (haveFine)
                                            {
                                                lblNotification.Text = "*You have some fine.";
                                            }
                                            else
                                            {
                                                lblNotification.Text = "*You have fine some other items.";
                                            }
                                            timer1.Interval = 250;
                                            timer1.Start();
                                            lblNotification.Visible = true;
                                        }
                                        else
                                        {
                                            timer1.Stop();
                                            lblNotification.Visible = false;
                                        }
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                        dataReader.Close();
                    }
                    else
                    {
                        btnRenew.Visible = false;
                        dataReader.Close();
                        txtbTitle.Clear();
                        txtbIsbn.Clear();
                        txtbMemberId.Clear();
                        txtbMemberName.Clear();
                        txtbIssueDate.Clear();
                        numDay.Value = 1;
                        MessageBox.Show("Item still not issued.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string queryString = "select * from issued_item inner join item_details where " +
                        "item_details.itemAccession=issued_item.itemAccession and issued_item.itemAccession=@itemAccession and itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbBarcode.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        btnReissue.Enabled = true;
                        while (dataReader.Read())
                        {
                            txtbMemberId.Text = dataReader["brrId"].ToString();
                            txtbIssueDate.Text = FormatDate.getUserFormat(dataReader["issueDate"].ToString());
                            issueDate = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtbTitle.Text = dataReader["itemTitle"].ToString();
                            txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                            itemAuthor = dataReader["itemAuthor"].ToString();
                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        issueDay = Convert.ToInt32((expectReturn.Date - issueDate.Date).TotalDays);
                        if (issueDay > numDay.Maximum)
                        {
                            numDay.Maximum = issueDay;
                        }
                        numDay.Value = issueDay;
                        dataReader.Close();
                        Application.DoEvents();

                        queryString = "select * from borrower_details inner join borrower_settings where borrower_details.brrCategory= borrower_settings.catName and borrower_details.brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                txtbMemberName.Text = dataReader["brrName"].ToString();
                                if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                                {
                                    entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                                avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                            }
                            dataReader.Close();
                            Application.DoEvents();

                            if (renewDate.Date <= DateTime.Now.Date)
                            {
                                btnRenew.Visible = true;
                                tooTip.AutomaticDelay = 1000;
                                tooTip.Show("Membership expired, Please renew now.", txtbBarcode);
                                btnReissue.Enabled = false;
                            }
                            else
                            {
                                btnRenew.Visible = false;
                                btnReissue.Enabled = true;
                                btnReissueReset.Enabled = true;
                                tooTip.Hide(txtbBarcode);
                                if (!avoidFine)
                                {
                                    //=======================================================
                                    //queryString = "select itemAccession,issueDate,expectedReturnDate from issuedItem where brrId=@brrId and [itemReturned]='" + false + "'";
                                    queryString = "select * from issued_item inner join item_details inner join " +
                                        "item_subcategory where item_details.itemAccession=issued_item.itemAccession and " +
                                        "item_subcategory.catName=item_details.itemCat and item_subcategory.subCatName=item_details.itemSubCat " +
                                        "and issued_item.brrId=@brrId and issued_item.itemReturned='" + false + "'";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbMemberId.Text);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        int dayLate = 0;
                                        DataTable tempTable = new DataTable();
                                        tempTable.Columns.Add("chkb", typeof(Boolean));
                                        tempTable.Columns.Add("accnNo", typeof(String));
                                        tempTable.Columns.Add("itemTitle", typeof(String));
                                        tempTable.Columns.Add("expDate", typeof(String));
                                        tempTable.Columns.Add("fine", typeof(String));
                                        while (dataReader.Read())
                                        {
                                            returnDate = DateTime.Now.Date;
                                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            if (expectReturn < reissueDate)
                                            {
                                                dayLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                                                finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                                if (dayLate < 0)
                                                {
                                                    dayLate = 0;
                                                }
                                                if (finePerDay > 0)
                                                {
                                                    if (dataReader["itemAccession"].ToString() == txtbBarcode.Text)
                                                    {
                                                        haveFine = true;
                                                    }
                                                    tempTable.Rows.Add(false, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                                        dataReader["expectedReturnDate"].ToString(), Math.Round(dayLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00"));
                                                }
                                            }
                                        }
                                        dataReader.Close();

                                        if (tempTable.Rows.Count > 0)
                                        {
                                            if (haveFine)
                                            {
                                                lblNotification.Text = "*You have some fine.";
                                            }
                                            else
                                            {
                                                lblNotification.Text = "*You have fine some other items.";
                                            }
                                            timer1.Interval = 250;
                                            timer1.Start();
                                            lblNotification.Visible = true;
                                        }
                                        else
                                        {
                                            timer1.Stop();
                                            lblNotification.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        dataReader.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            dataReader.Close();
                        }
                    }
                    else
                    {
                        btnRenew.Visible = false;
                        dataReader.Close();
                        txtbTitle.Clear();
                        txtbIsbn.Clear();
                        txtbMemberId.Clear();
                        txtbMemberName.Clear();
                        txtbIssueDate.Clear();
                        numDay.Value = 1;
                        MessageBox.Show("Item still not issued.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    mysqlConn.Close();
                }
            }
            else
            {
                btnRenew.Visible = false;
                btnReissue.Enabled = false;
                btnReissueReset.Enabled = false;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            btnClose_Click(null, null);
            timer4.Stop();
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
            timer4.Start();
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

        private void dgvBook_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnReturn.Enabled = true;
            btnReissue.Enabled = true;
        }

        private void dgvBook_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgvBook.Rows.Count == 0)
            {
                btnReturn.Enabled = false;
                btnReissue.Enabled = false;
            }
        }

        private void btnReissue_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnReissue.Enabled == true)
            {
                btnReissue.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnReissue.BackColor = Color.DimGray;
            }
        }

        private void btnReturn_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnReturn.Enabled == true)
            {
                btnReturn.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnReturn.BackColor = Color.DimGray;
            }
        }

        private void rdbIsbn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIsbn.Checked)
            {
                txtbSearchBarcode.Enabled = false;
                txtbSearchBarcode.Clear();
                txtbSearchIsbn.Enabled = true;
                txtbSearchIsbn.Select();
                Properties.Settings.Default.returnByAccn = false;
                Properties.Settings.Default.Save();
            }
        }

        private void rdbAccn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAccn.Checked)
            {
                btnReissueReset_Click(null, null);
                txtbSearchIsbn.Enabled = false;
                txtbSearchIsbn.Clear();
                txtbSearchBarcode.Enabled = true;
                txtbSearchBarcode.Select();
                Properties.Settings.Default.returnByAccn = true;
                Properties.Settings.Default.Save();
            }
        }

        private void rdbRsAccn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbRsAccn.Checked)
            {
                btnReissueReset_Click(null, null);
                txtbIsbn.Enabled = false;
                txtbIsbn.Clear();
                txtbBarcode.Enabled = true;
                txtbBarcode.Select();
                Properties.Settings.Default.reissueByAccn = true;
                Properties.Settings.Default.Save();
            }
        }

        private void rdbRsIsbn_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbRsIsbn.Checked)
            {
                txtbBarcode.Enabled = false;
                txtbBarcode.Clear();
                txtbIsbn.Enabled = true;
                txtbIsbn.Select();
                Properties.Settings.Default.reissueByAccn = false;
                Properties.Settings.Default.Save();
            }
        }

        private void timerAccession_Tick(object sender, EventArgs e)
        {
            timerAccession.Stop();
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                //====================get Issued accession No===============
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select itemAccession from issuedItem where [itemReturned]='" + false + "'";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollAccession.Clear();
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["itemAccession"]
                        ).ToList();
                    autoCollAccession.AddRange(idList.ToArray());

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
                    string queryString = "select itemAccession from issued_item where itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollAccession.Clear();
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["itemAccession"]
                            ).ToList();
                        autoCollAccession.AddRange(idList.ToArray());
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

        private void timerIsbn_Tick(object sender, EventArgs e)
        {
            timerIsbn.Stop();
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                //====================get Issued accession No===============
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select itemIsbn from itemDetails inner join issuedItem where issuedItem.itemAccession=itemDetails.itemAccession and issuedItem.itemReturned='" + false + "'";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollIsbn.Clear();
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["itemIsbn"]
                        ).ToList();
                    autoCollIsbn.AddRange(idList.ToArray());
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
                    string queryString = "select itemIsbn from item_details inner join issued_item where issued_item.itemAccession=item_details.itemAccession and issued_item.itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollIsbn.Clear();
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["itemIsbn"]
                            ).ToList();
                        autoCollIsbn.AddRange(idList.ToArray());

                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void btnRenew_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Renew Member", btnRenew);
        }

        private void btnRenew_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRenew);
        }

        private void btnRecieptSetting_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Receipt Setting", btnRecieptSetting);
        }

        private void btnRecieptSetting_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRecieptSetting);
        }

        private void btnTutorial_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("See Tutorial", btnTutorial);
        }

        private void btnTutorial_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnTutorial);
        }

        private void btnRecieptSetting1_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Receipt setting", btnRecieptSetting1);
        }

        private void btnRecieptSetting1_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRecieptSetting1);
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            FormRenewMember renewForm = new FormRenewMember();
            renewForm.txtbBrrId.Text = txtbMemberId.Text;
            renewForm.ShowDialog();
            btnRenew.Visible = false;
            ReissueItem();
        }

        private void btnRecieptSetting1_Click(object sender, EventArgs e)
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

        private void txtbIsbn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) == 1)
                    {
                        sqltCommnd.CommandText = "select itemAccession from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbBarcode.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        ReissueItem();
                    }
                    else if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.Columns[2].HeaderText = "Member Id";
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        sqltCommnd.CommandText = "select * from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
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
                                formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "");
                            }
                        }
                        dataReader.Close();
                        foreach (DataGridViewRow dgvRow in formIsbnItems.dgvAccnList.Rows)
                        {
                            sqltCommnd.CommandText = "select brrId from issuedItem where itemAccession = @itemAccession and itemReturned='" + false + "';";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[1].Value.ToString());
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    formIsbnItems.dgvAccnList.Rows[dgvRow.Index].Cells[2].Value = dataReader["brrId"].ToString();
                                }
                            }
                            dataReader.Close();
                        }
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To reissue an item right/double click on it.";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbBarcode.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            ReissueItem();
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
                    string queryString = "select count(id) from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) == 1)
                    {
                        queryString = "select itemAccession from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbBarcode.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        ReissueItem();
                    }
                    else if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.Columns[2].HeaderText = "Member Id";
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        queryString = "select * from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                formIsbnItems.lblIsbn.Text = dataReader["itemIsbn"].ToString();
                                formIsbnItems.lblTitle.Text = dataReader["itemTitle"].ToString();
                                formIsbnItems.lblAuthor.Text = dataReader["itemAuthor"].ToString();
                                formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "");
                            }
                        }
                        dataReader.Close();
                        foreach (DataGridViewRow dgvRow in formIsbnItems.dgvAccnList.Rows)
                        {
                            queryString = "select brrId from issued_item where itemAccession = @itemAccession and itemReturned='" + false + "';";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[1].Value.ToString());
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    formIsbnItems.dgvAccnList.Rows[dgvRow.Index].Cells[2].Value = dataReader["brrId"].ToString();
                                }
                            }
                            dataReader.Close();
                        }
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To reissue an item right/double click on it.";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbBarcode.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            ReissueItem();
                        }
                    }
                }
            }
        }

        private void txtbSearchIsbn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                    if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) == 1)
                    {
                        sqltCommnd.CommandText = "select itemAccession from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbSearchBarcode.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        ReturnItem();
                    }
                    else if (Convert.ToInt32(sqltCommnd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.Columns[2].HeaderText = "Member Id";
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        sqltCommnd.CommandText = "select * from itemDetails where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                formIsbnItems.lblIsbn.Text = dataReader["itemIsbn"].ToString();
                                formIsbnItems.lblTitle.Text = dataReader["itemTitle"].ToString();
                                formIsbnItems.lblAuthor.Text = dataReader["itemAuthor"].ToString();
                                formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "");
                            }
                        }
                        dataReader.Close();
                        foreach (DataGridViewRow dgvRow in formIsbnItems.dgvAccnList.Rows)
                        {
                            sqltCommnd.CommandText = "select brrId from issuedItem where itemAccession = @itemAccession and itemReturned='" + false + "';";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[1].Value.ToString());
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    formIsbnItems.dgvAccnList.Rows[dgvRow.Index].Cells[2].Value = dataReader["brrId"].ToString();
                                }
                            }
                            dataReader.Close();
                        }
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To return an item right/double click on it.";
                        formIsbnItems.operationName = "Return";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbSearchBarcode.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            ReturnItem();
                        }
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
                    string queryString = "select count(id) from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) == 1)
                    {
                        queryString = "select itemAccession from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbSearchBarcode.Text = dataReader["itemAccession"].ToString();
                            }
                        }
                        dataReader.Close();
                        ReturnItem();
                    }
                    else if (Convert.ToInt32(mysqlCmd.ExecuteScalar()) > 1)
                    {
                        FormIsbnItems formIsbnItems = new FormIsbnItems();
                        formIsbnItems.dgvAccnList.Columns[2].HeaderText = "Member Id";
                        formIsbnItems.dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
                        formIsbnItems.dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        queryString = "select * from item_details where itemIsbn = @itemIsbn and itemAvailable='" + false + "';";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbSearchIsbn.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                formIsbnItems.lblIsbn.Text = dataReader["itemIsbn"].ToString();
                                formIsbnItems.lblTitle.Text = dataReader["itemTitle"].ToString();
                                formIsbnItems.lblAuthor.Text = dataReader["itemAuthor"].ToString();
                                formIsbnItems.dgvAccnList.Rows.Add(formIsbnItems.dgvAccnList.Rows.Count + 1, dataReader["itemAccession"].ToString(), "");
                            }
                        }
                        dataReader.Close();

                        foreach (DataGridViewRow dgvRow in formIsbnItems.dgvAccnList.Rows)
                        {
                            queryString = "select brrId from issued_item where itemAccession = @itemAccession and itemReturned='" + false + "';";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    formIsbnItems.dgvAccnList.Rows[dgvRow.Index].Cells[2].Value = dataReader["brrId"].ToString();
                                }
                            }
                            dataReader.Close();
                        }
                        formIsbnItems.dgvAccnList.ClearSelection();
                        formIsbnItems.lblInfo.Text = "* To return an item right/double click on it.";
                        formIsbnItems.operationName = "Return";
                        formIsbnItems.ShowDialog();
                        if (formIsbnItems.dgvAccnList.SelectedRows.Count > 0)
                        {
                            txtbSearchBarcode.Text = formIsbnItems.dgvAccnList.SelectedRows[0].Cells[1].Value.ToString();
                            ReturnItem();
                        }
                    }
                    mysqlConn.Close();
                }
            }
        }

        private void UcItemReissueReturn_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            lblNotification.Visible = false;
        }

        private void dgvBook_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblBlink.Visible = !lblBlink.Visible;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            lblReturnBlink.Visible = !lblReturnBlink.Visible;
        }

        private void txtbSerachId_TextChanged(object sender, EventArgs e)
        {
            if (txtbSerachId.Text != "")
            {
                string strReturnDate = dtpReturn.Value.Day.ToString("00") + "/" + dtpReturn.Value.Month.ToString("00") + "/" + dtpReturn.Value.Year.ToString("0000");
                dgvBook.Rows.Clear();
                bool avoidFine = false; string brrCategory = "";
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    SQLiteDataReader dataReader;

                    sqltCommnd.CommandText = "select brrCategory from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrCategory = dataReader["brrCategory"].ToString();
                        }
                    }
                    dataReader.Close();

                    sqltCommnd.CommandText = "select avoidFine from borrowerSettings where catName=@catName";
                    sqltCommnd.Parameters.AddWithValue("@catName", brrCategory);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                        }
                    }
                    dataReader.Close();

                    string queryString = "select itemAccession,issueDate,expectedReturnDate from issuedItem where brrId=@brrId and [itemReturned]='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        string itemCat = "", itemSubcat = "";
                        DataTable tempTable = new DataTable();
                        tempTable.Columns.Add("chkb", typeof(Boolean));
                        tempTable.Columns.Add("memberId", typeof(String));
                        tempTable.Columns.Add("accnNo", typeof(String));
                        tempTable.Columns.Add("itemTitle", typeof(String));
                        tempTable.Columns.Add("issueDate", typeof(String));
                        tempTable.Columns.Add("exoDate", typeof(String));
                        tempTable.Columns.Add("returnDate", typeof(String));
                        tempTable.Columns.Add("fine", typeof(String));
                        tempTable.Columns.Add("author", typeof(String));
                        tempTable.Columns.Add("location", typeof(String));
                        while (dataReader.Read())
                        {
                            //returnDate = DateTime.ParseExact(txtbReturDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            returnDate = dtpReturn.Value.Date;
                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                            if (daysLate < 0)
                            {
                                daysLate = 0;
                            }
                            tempTable.Rows.Add(false, txtbSerachId.Text, dataReader["itemAccession"].ToString(),
                                "", dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                strReturnDate, daysLate);
                        }
                        dataReader.Close();
                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "select itemTitle,itemCat,itemSubCat,itemAuthor,rackNo from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow[2].ToString());
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][3] = dataReader["itemTitle"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][8] = dataReader["itemAuthor"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][9] = dataReader["rackNo"].ToString();
                                    itemCat = dataReader["itemCat"].ToString();
                                    itemSubcat = dataReader["itemSubCat"].ToString();
                                }
                                dataReader.Close();

                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "select subCatFine from itemSubCategory where catName=@catName and subCatName=@subCatName";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                                sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubcat);
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                    }
                                }
                                dataReader.Close();
                                daysLate = Convert.ToInt32(dataRow[7].ToString());
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                if (!avoidFine)
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00");
                                }
                                else
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = 0.00.ToString("0.00");
                                }
                            }
                            dgvBook.Rows.Add(false, dataRow[1].ToString(), dataRow[2].ToString(), dataRow[3].ToString(),
                               FormatDate.getUserFormat(dataRow[4].ToString()), FormatDate.getUserFormat(dataRow[5].ToString()),
                               FormatDate.getUserFormat(dataRow[6].ToString()), dataRow[7].ToString(),
                                dataRow[8].ToString(), dataRow[9].ToString());
                        }
                        dgvBook.ClearSelection();
                    }
                    else
                    {
                        dgvBook.Rows.Clear();
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
                        string queryString = "select brrCategory from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                brrCategory = dataReader["brrCategory"].ToString();
                            }
                        }
                        dataReader.Close();

                        queryString = "select avoidFine from borrower_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", brrCategory);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                            }
                        }
                        dataReader.Close();

                        queryString = "select itemAccession,issueDate,expectedReturnDate from issued_item where brrId=@brrId and itemReturned='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbSerachId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string itemCat = "", itemSubcat = "";
                            DataTable tempTable = new DataTable();
                            tempTable.Columns.Add("chkb", typeof(Boolean));
                            tempTable.Columns.Add("memberId", typeof(String));
                            tempTable.Columns.Add("accnNo", typeof(String));
                            tempTable.Columns.Add("itemTitle", typeof(String));
                            tempTable.Columns.Add("issueDate", typeof(String));
                            tempTable.Columns.Add("exoDate", typeof(String));
                            tempTable.Columns.Add("returnDate", typeof(String));
                            tempTable.Columns.Add("fine", typeof(String));
                            tempTable.Columns.Add("author", typeof(String));
                            tempTable.Columns.Add("location", typeof(String));
                            while (dataReader.Read())
                            {
                                //returnDate = DateTime.ParseExact(txtbReturDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                returnDate = dtpReturn.Value.Date;
                                expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                tempTable.Rows.Add(false, txtbSerachId.Text, dataReader["itemAccession"].ToString(),
                                    "", dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                    strReturnDate, daysLate);
                            }
                            dataReader.Close();

                            foreach (DataRow dataRow in tempTable.Rows)
                            {
                                queryString = "select itemTitle,itemCat,itemSubCat,itemAuthor,rackNo from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow[2].ToString());
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][3] = dataReader["itemTitle"].ToString();
                                        tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][8] = dataReader["itemAuthor"].ToString();
                                        tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][9] = dataReader["rackNo"].ToString();
                                        itemCat = dataReader["itemCat"].ToString();
                                        itemSubcat = dataReader["itemSubCat"].ToString();
                                    }
                                    dataReader.Close();

                                    queryString = "select subCatFine from item_subcategory where catName=@catName and subCatName=@subCatName";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                                    mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubcat);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                        }
                                    }
                                    dataReader.Close();
                                    daysLate = Convert.ToInt32(dataRow[7].ToString());
                                    if (daysLate < 0)
                                    {
                                        daysLate = 0;
                                    }
                                    if (!avoidFine)
                                    {
                                        tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00");
                                    }
                                    else
                                    {
                                        tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = 0.00.ToString("0.00");
                                    }
                                }
                                dgvBook.Rows.Add(false, dataRow[1].ToString(), dataRow[2].ToString(), dataRow[3].ToString(),
                                    FormatDate.getUserFormat(dataRow[4].ToString()), FormatDate.getUserFormat(dataRow[5].ToString()), FormatDate.getUserFormat(dataRow[6].ToString()), dataRow[7].ToString(),
                                    dataRow[8].ToString(), dataRow[9].ToString());
                            }
                            dgvBook.ClearSelection();
                        }
                        else
                        {
                            dataReader.Close();
                            dgvBook.Rows.Clear();
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
                dgvBook.Rows.Clear();
            }
            if (dgvBook.Rows.Count > 0)
            {
                btnReset.Enabled = true;
            }
            else
            {
                btnReset.Enabled = false;
            }
        }

        void tooTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Segoe UI", 9.0f);
            Brush b = new SolidBrush(Color.AntiqueWhite);
            e.Graphics.FillRectangle(b, e.Bounds);
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.IndianRed, new PointF(2, 2));
        }
    }
}
