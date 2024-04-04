using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class UcAddBorrower : UserControl
    {
        public UcAddBorrower()
        {
            InitializeComponent();
        }

        int lastNumber = 0, validDays = 0, issueLimit = 0;
        double memFees = 0.00, totalFees = 0.00;
        string controlName = "", categoryLabel = "", idLabel = "";
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        List<string> brrIdList = new List<string> { };
        string defaultPass = "";

        private void cmbBrrCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            lblMessage.Text = "Maximum file size 500 KB";
            pcbBrrImage.Image = Properties.Resources.blankBrrImage;
            if (cmbBrrCategory.SelectedIndex == 0)
            {
                lblInfo1.Text = "Not set from borrower setting";
                lblInfo2.Text = "Not set from borrower setting";
                lblInfo3.Text = "Not set from borrower setting";
                lblInfo4.Text = "Not set from borrower setting";
                lblInfo5.Text = "Not set from borrower setting";
                txtbAddtional1.Enabled = false;
                txtbAdditional2.Enabled = false;
                txtbAdditional3.Enabled = false;
                txtbAdditional4.Enabled = false;
                txtbAdditional5.Enabled = false;
                clearData();
                return;
            }
            //================ Borrower id generation================
            txtbBrrId.TabIndex = 0;
            txtbBrrName.TabIndex = 1;
            txtbBrrAddress.TabIndex = 2;
            grpbGender.TabIndex = 3;
            txtbBrrMail.TabIndex = 4;
            txtb2ndMail.TabIndex = 5;
            txtbBrrContact.TabIndex = 6;
            cmbPlan.TabIndex = 7;
            txtbFreqncy.TabIndex = 8;
            dtpIssue.TabIndex = 9;
            GenerateBorrowerId();

            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                //=========================get additional info===================
                string queryString = "select capInfo,membershipFees,defaultPass from borrowerSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["capInfo"].ToString() != "")
                        {
                            string[] captionList = dataReader["capInfo"].ToString().Split('$');
                            for (int i = 0; i < captionList.Length; i++)
                            {
                                if (i == 0)
                                {
                                    lblInfo1.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAddtional1.Enabled = true;
                                    txtbAddtional1.TabIndex = 10;
                                    chkbFees.TabIndex = 111;
                                    btnUploadImage.TabIndex = 12;
                                    btnSave.TabIndex = 13;
                                    lblInfo2.Text = "Not set from borrower setting";
                                    lblInfo3.Text = "Not set from borrower setting";
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 1)
                                {
                                    lblInfo2.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional2.Enabled = true;
                                    txtbAdditional2.TabIndex = 11;
                                    chkbFees.TabIndex = 12;
                                    btnUploadImage.TabIndex = 13;
                                    btnSave.TabIndex = 14;
                                    lblInfo3.Text = "Not set from borrower setting";
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 2)
                                {
                                    lblInfo3.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional3.Enabled = true;
                                    txtbAdditional3.TabIndex = 12;
                                    chkbFees.TabIndex = 13;
                                    btnUploadImage.TabIndex = 14;
                                    btnSave.TabIndex = 15;
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 3)
                                {
                                    lblInfo4.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional4.Enabled = true;
                                    txtbAdditional4.TabIndex = 13;
                                    chkbFees.TabIndex = 14;
                                    btnUploadImage.TabIndex = 15;
                                    btnSave.TabIndex = 16;
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 4)
                                {
                                    lblInfo5.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional5.Enabled = true;
                                    txtbAdditional5.TabIndex = 14;
                                    chkbFees.TabIndex = 15;
                                    btnUploadImage.TabIndex = 16;
                                    btnSave.TabIndex = 17;
                                }
                            }
                        }
                        else
                        {
                            lblInfo1.Text = "Not set from borrower setting";
                            lblInfo2.Text = "Not set from borrower setting";
                            lblInfo3.Text = "Not set from borrower setting";
                            lblInfo4.Text = "Not set from borrower setting";
                            lblInfo5.Text = "Not set from borrower setting";
                            txtbAddtional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                            chkbFees.TabIndex = 10;
                            btnUploadImage.TabIndex = 11;
                            btnSave.TabIndex = 12;
                        }
                        defaultPass = dataReader["defaultPass"].ToString();
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
                string queryString = "select capInfo,membershipFees,defaultPass from borrower_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["capInfo"].ToString() != "")
                        {
                            string[] captionList = dataReader["capInfo"].ToString().Split('$');
                            for (int i = 0; i < captionList.Length; i++)
                            {
                                if (i == 0)
                                {
                                    lblInfo1.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAddtional1.Enabled = true;
                                    txtbAddtional1.TabIndex = 10;
                                    chkbFees.TabIndex = 111;
                                    btnUploadImage.TabIndex = 12;
                                    btnSave.TabIndex = 13;
                                    lblInfo2.Text = "Not set from borrower setting";
                                    lblInfo3.Text = "Not set from borrower setting";
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 1)
                                {
                                    lblInfo2.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional2.Enabled = true;
                                    txtbAdditional2.TabIndex = 11;
                                    chkbFees.TabIndex = 12;
                                    btnUploadImage.TabIndex = 13;
                                    btnSave.TabIndex = 14;
                                    lblInfo3.Text = "Not set from borrower setting";
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 2)
                                {
                                    lblInfo3.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional3.Enabled = true;
                                    txtbAdditional3.TabIndex = 12;
                                    chkbFees.TabIndex = 13;
                                    btnUploadImage.TabIndex = 14;
                                    btnSave.TabIndex = 15;
                                    lblInfo4.Text = "Not set from borrower setting";
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 3)
                                {
                                    lblInfo4.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional4.Enabled = true;
                                    txtbAdditional4.TabIndex = 13;
                                    chkbFees.TabIndex = 14;
                                    btnUploadImage.TabIndex = 15;
                                    btnSave.TabIndex = 16;
                                    lblInfo5.Text = "Not set from borrower setting";
                                }
                                else if (i == 4)
                                {
                                    lblInfo5.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional5.Enabled = true;
                                    txtbAdditional5.TabIndex = 14;
                                    chkbFees.TabIndex = 15;
                                    btnUploadImage.TabIndex = 16;
                                    btnSave.TabIndex = 17;
                                }
                            }
                        }
                        else
                        {
                            lblInfo1.Text = "Not set from borrower setting";
                            lblInfo2.Text = "Not set from borrower setting";
                            lblInfo3.Text = "Not set from borrower setting";
                            lblInfo4.Text = "Not set from borrower setting";
                            lblInfo5.Text = "Not set from borrower setting";
                            txtbAddtional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                            chkbFees.TabIndex = 10;
                            btnUploadImage.TabIndex = 111;
                            btnSave.TabIndex = 12;
                        }
                        defaultPass = dataReader["defaultPass"].ToString();
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string brrGender = null, currentDate = null;
            if (cmbBrrCategory.SelectedIndex == -1 || cmbBrrCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbBrrCategory.Select();
                return;
            }

            if (rdbtMale.Checked)
            {
                brrGender = "Male";
            }
            else if (rdbtFemale.Checked)
            {
                brrGender = "Female";
            }
            else if (rdbOther.Checked)
            {
                brrGender = "Others";
            }
            else
            {
                MessageBox.Show("Please select a gender.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbBrrName.Text == "")
            {
                MessageBox.Show("Please enter borrower name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrName.Select();
                return;
            }

            if (cmbPlan.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a membership plan.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbPlan.Select();
                return;
            }

            if (btnSave.Text == "Sa&ve")//=================Borrower adding==================
            {
                string fileName = "";
                currentDate = dtpIssue.Value.Day.ToString("00") + "/" + dtpIssue.Value.Month.ToString("00") + "/" + dtpIssue.Value.Year.ToString("0000");
                SQLiteConnection sqltConn = null;
                SQLiteCommand sqltCommnd = null;

                MySqlConnection mysqlConn = null;
                MySqlCommand mysqlCmd = null;
                string queryString = "";
                if (globalVarLms.sqliteData)
                {
                    sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    sqltCommnd = sqltConn.CreateCommand();
                    //====================check for Borrower id exist======================
                    queryString = "select brrId from borrowerDetails where brrId=@brrId";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Id can not be same for 2 borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();
                    if (pcbBrrImage.Image != Properties.Resources.blankBrrImage && pcbBrrImage.Image != Properties.Resources.uploadingFail)
                    {
                        if (!Directory.Exists(Properties.Settings.Default.databasePath + @"\BorrowerImage"))
                        {
                            Directory.CreateDirectory(Properties.Settings.Default.databasePath + @"\BorrowerImage");
                        }
                        fileName = Properties.Settings.Default.databasePath + @"\BorrowerImage\" + txtbBrrId.Text + ".jpg";
                        if (!File.Exists(fileName))
                        {
                            pcbBrrImage.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        fileName = txtbBrrId.Text + ".jpg";
                    }
                    //====================Insert Borrower details======================
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "INSERT INTO borrowerDetails (brrId,brrName,brrCategory," +
                        "brrMailId,brrGender,brrAddress,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                        "entryDate,opkPermission,imagePath,brrPass,memPlan,memFreq,addnlMail) VALUES (@brrId,@brrName,@brrCategory," +
                        "@brrMailId,'" + brrGender + "',@brrAddress,'" + txtbBrrContact.Text + "'" +
                        ",'" + txtbBrrRnwDate.Text + "',@addInfo1,@addInfo2,@addInfo3" +
                        ",@addInfo4,@addInfo5,'" + currentDate + "','" + chkbOpak.Checked + "','" + fileName + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "',@addnlMail);";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    sqltCommnd.Parameters.AddWithValue("@brrName", txtbBrrName.Text);
                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
                    sqltCommnd.Parameters.AddWithValue("@brrMailId", txtbBrrMail.Text);
                    sqltCommnd.Parameters.AddWithValue("@brrAddress", txtbBrrAddress.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo1", txtbAddtional1.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                    sqltCommnd.Parameters.AddWithValue("@brrPass", defaultPass);
                    sqltCommnd.Parameters.AddWithValue("@memPlan", cmbPlan.Text);
                    sqltCommnd.Parameters.AddWithValue("@addnlMail", txtb2ndMail.Text);
                    sqltCommnd.ExecuteNonQuery();

                    if (lastNumber > 0)
                    {
                        queryString = "update brrIdSetting  set lastNumber='" + lastNumber + "'";
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.ExecuteNonQuery();
                    }
                    if (totalFees > 0)
                    {
                        queryString = "select invidCount from paymentDetails where invidCount!=0 order by [id] desc limit 1";
                        sqltCommnd.CommandText = queryString;
                        dataReader = sqltCommnd.ExecuteReader();
                        int rowCount = 0;
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();
                        rowCount = rowCount + 1;
                        if (chkbFees.Checked)
                        {
                            string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                            queryString = "Insert Into paymentDetails (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt)" +
                                "values ('" + currentDate + "','" + invId + "',@memberId,'" + "Membership fees" + "','" + totalFees + "','" + true + "'," +
                                "'" + currentDate + "','" + globalVarLms.currentUserId + "','" + rowCount + "','" + 0 + "')";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            sqltCommnd.ExecuteNonQuery();

                            if (globalVarLms.paymentReciept)
                            {
                                generateMembershipReciept(invId, currentDate);
                            }
                        }
                        else
                        {
                            queryString = "Insert Into paymentDetails (feesDate,memberId,feesDesc,dueAmount,isPaid,discountAmnt,itemAccn,invidCount)" +
                                "values ('" + currentDate + "',@brrId,'" + "Membership fees" + "','" + totalFees + "','" + false + "','" + 0 + "','" + "" + "','" + 0 + "')";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                            sqltCommnd.ExecuteNonQuery();
                        }
                    }
                    sqltConn.Close();
                }
                else
                {
                    String base64String = "base64String";
                    if (pcbBrrImage.Image != Properties.Resources.blankBrrImage && pcbBrrImage.Image != Properties.Resources.uploadingFail)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            pcbBrrImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = memoryStream.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
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
                    //====================check for Borrower id exist======================
                    queryString = "select brrId from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Id can not be same for 2 borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();

                    queryString = "INSERT INTO borrower_details (brrId,brrName,brrCategory," +
                       "brrMailId,brrGender,brrAddress,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                       "entryDate,opkPermission,brrImage,brrPass,memPlan,memFreq,addnlMail) VALUES (@brrId,@brrName,@brrCategory," +
                       "@brrMailId,'" + brrGender + "',@brrAddress,'" + txtbBrrContact.Text + "'" +
                       ",'" + txtbBrrRnwDate.Text + "',@addInfo1,@addInfo2,@addInfo3" +
                       ",@addInfo4,@addInfo5,'" + currentDate + "','" + chkbOpak.Checked + "','" + base64String + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "',@addnlMail);";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrName", txtbBrrName.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrMailId", txtbBrrMail.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrAddress", txtbBrrAddress.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo1", txtbAddtional1.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrPass", defaultPass);
                    mysqlCmd.Parameters.AddWithValue("@memPlan", cmbPlan.Text);
                    mysqlCmd.Parameters.AddWithValue("@addnlMail", txtb2ndMail.Text);
                    mysqlCmd.ExecuteNonQuery();

                    if (lastNumber > 0)
                    {
                        queryString = "update brrid_setting  set lastNumber='" + lastNumber + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.ExecuteNonQuery();
                    }

                    if (totalFees > 0)
                    {
                        queryString = "select invidCount from payment_details where invidCount!=0 order by id desc limit 1";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        dataReader = mysqlCmd.ExecuteReader();
                        int rowCount = 0;
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();

                        rowCount = rowCount + 1;
                        if (chkbFees.Checked)
                        {
                            string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                            queryString = "Insert Into payment_details (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt)" +
                                "values ('" + currentDate + "','" + invId + "',@memberId,'" + "Membership fees" + "','" + totalFees + "','" + true + "'," +
                                "'" + currentDate + "','" + globalVarLms.currentUserId + "','" + rowCount + "','" + 0 + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            mysqlCmd.ExecuteNonQuery();

                            if (globalVarLms.paymentReciept)
                            {
                                generateMembershipReciept(invId, currentDate);
                            }
                        }
                        else
                        {
                            queryString = "Insert Into payment_details (feesDate,memberId,feesDesc,dueAmount,isPaid,discountAmnt,itemAccn,invidCount)" +
                                "values ('" + currentDate + "',@brrId,'" + "Membership fees" + "','" + totalFees + "','" + false + "','" + 0 + "','" + "" + "','" + 0 + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                            mysqlCmd.ExecuteNonQuery();
                        }
                    }
                    mysqlConn.Close();
                }
                dgvBrrDetails.Rows.Add(txtbBrrId.Text, txtbBrrName.Text, cmbBrrCategory.Text, brrGender, txtbBrrContact.Text, txtbBrrAddress.Text);
                dgvBrrDetails.ClearSelection();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Borrower added successfully !";
                showNotification();
                clearData();
                //====================Generate Borrower id======================
                GenerateBorrowerId();
            }
            else//===================Borrower updating====================
            {
                try
                {
                    string fileName = "";
                    if (globalVarLms.sqliteData)
                    {
                        if (pcbBrrImage.Image != Properties.Resources.blankBrrImage && pcbBrrImage.Image != Properties.Resources.uploadingFail)
                        {
                            if (!Directory.Exists(Properties.Settings.Default.databasePath + @"\BorrowerImage"))
                            {
                                Directory.CreateDirectory(Properties.Settings.Default.databasePath + @"\BorrowerImage");
                            }
                            Bitmap bmp1 = new Bitmap(pcbBrrImage.Image);
                            fileName = Properties.Settings.Default.databasePath + @"\BorrowerImage\" + txtbBrrId.Text + ".jpg";
                            if (File.Exists(fileName))
                            {
                                pcbBrrImage.Image.Dispose();
                                File.Delete(fileName);
                            }
                            bmp1.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            fileName = txtbBrrId.Text + ".jpg";
                        }

                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select entryDate from borrowerDetails where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        DateTime renewDate = DateTime.Now;
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                renewDate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                        dataReader.Close();

                        queryString = "update borrowerDetails set brrName='" + txtbBrrName.Text + "',brrCategory =:brrCategory" +
                            ",brrAddress=:brrAddress,brrGender='" + brrGender + "',brrMailId='" + txtbBrrMail.Text + "'" +
                            ",brrContact='" + txtbBrrContact.Text + "',mbershipDuration='" + txtbBrrRnwDate.Text + "'" +
                            ",addInfo1=:addInfo1,addInfo2=:addInfo2,addInfo3=:addInfo3,addInfo4=:addInfo4,addInfo5=:addInfo5," +
                            "opkPermission='" + chkbOpak.Checked + "',imagePath='" + fileName + "',brrPass=:brrPass" +
                            ",memPlan=:memPlan,memFreq='" + txtbFreqncy.Text + "',addnlMail='" + txtb2ndMail.Text + "' where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        sqltCommnd.Parameters.AddWithValue("brrCategory", cmbBrrCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("brrAddress", txtbBrrAddress.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo1", txtbAddtional1.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo2", txtbAdditional2.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo3", txtbAdditional3.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo4", txtbAdditional4.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo5", txtbAdditional5.Text);
                        sqltCommnd.Parameters.AddWithValue("brrPass", defaultPass);
                        sqltCommnd.Parameters.AddWithValue("memPlan", cmbPlan.Text);
                        sqltCommnd.ExecuteNonQuery();
                        sqltConn.Close();
                    }
                    else
                    {
                        String base64String = "base64String";
                        if (pcbBrrImage.Image != Properties.Resources.blankBrrImage && pcbBrrImage.Image != Properties.Resources.uploadingFail)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                pcbBrrImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] imageBytes = memoryStream.ToArray();
                                base64String = Convert.ToBase64String(imageBytes);
                            }
                        }
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
                        string queryString = "update borrower_details set brrName='" + txtbBrrName.Text + "',brrCategory =@brrCategory" +
                            ",brrAddress=@brrAddress,brrGender='" + brrGender + "',brrMailId='" + txtbBrrMail.Text + "'" +
                            ",brrContact='" + txtbBrrContact.Text + "',mbershipDuration='" + txtbBrrRnwDate.Text + "'" +
                            ",addInfo1=@addInfo1,addInfo2=@addInfo2,addInfo3=@addInfo3,addInfo4=@addInfo4,addInfo5=@addInfo5," +
                            "opkPermission='" + chkbOpak.Checked + "',brrImage='" + base64String + "',brrPass=@brrPass" +
                            ",memPlan=@memPlan,memFreq='" + txtbFreqncy.Text + "',addnlMail='" + txtb2ndMail.Text + "' where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
                        mysqlCmd.Parameters.AddWithValue("@brrAddress", txtbBrrAddress.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo1", txtbAddtional1.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                        mysqlCmd.Parameters.AddWithValue("@brrPass", defaultPass);
                        mysqlCmd.Parameters.AddWithValue("@memPlan", cmbPlan.Text);
                        mysqlCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                    dgvBrrDetails.SelectedRows[0].Cells[1].Value = txtbBrrName.Text;
                    dgvBrrDetails.SelectedRows[0].Cells[2].Value = cmbBrrCategory.Text;
                    dgvBrrDetails.SelectedRows[0].Cells[3].Value = brrGender;
                    dgvBrrDetails.SelectedRows[0].Cells[4].Value = txtbBrrContact.Text;
                    dgvBrrDetails.SelectedRows[0].Cells[5].Value = txtbBrrAddress.Text;
                    globalVarLms.backupRequired = true;
                    lblUserMessage.Text = "Borrower updated successfully !";
                    showNotification();
                }
                catch
                {

                }
                clearData();
                btnSave.Text = "Sa&ve";
                txtbBrrId.Enabled = true;
                cmbSearchBy.SelectedIndex = 0;
                cmbBrrCategory.SelectedIndex = 0;
            }
            pcbBrrImage.Image = Properties.Resources.blankBrrImage;
        }

        private void generateMembershipReciept(string invId, string currentDate)
        {
            string brrContact = "", instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "";
            System.Drawing.Image instLogo = null;
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //====================check for Borrower id exist======================
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

                queryString = "select brrContact from borrowerDetails where brrId=@brrId";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        brrContact = dataReader["brrContact"].ToString();
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

                    queryString = "select brrContact from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrContact = dataReader["brrContact"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
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
            string fileName = folderPath + @"\tempInvoice" + ".pdf";
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
                    instLogoJpg.SetAbsolutePosition(60, pdfToCreate.PageSize.Height - 70f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(50f, 50f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 12);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 44f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = "Call/website/email : " + instContact + " | " + instWebsite + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 56f);
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

                    pdfContent.ShowText("Prepared Date : " + currentDate + " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(40, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.LineTo(585, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 545;
                    PdfPCell pdfCell = null;

                    PdfPTable tempTable = new PdfPTable(4);
                    float[] columnWidth = { 80f, 225f, 80f, 160f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(2);
                    columnWidth = new float[] { 80f, 465f };
                    tempTable.SetTotalWidth(columnWidth);

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

                    pdfCell = new PdfPCell(new Phrase(lblContact.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
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
                    columnWidth = new float[] { 70f, 245f, 150f, 80f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Duration", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    columnWidth = new float[] { 70f, 245f, 150f, 80f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("1", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrRnwDate.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(memFees.ToString(), smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Total Amount :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + memFees.ToString(), smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.Border = 0;
                        pdfCell.BorderWidthTop = 0.5f;
                        pdfCell.BorderColorTop = BaseColor.DARK_GRAY;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 40, pdfToCreate.PageSize.Height - 85, pdfContent);

                    pdfToCreate.Close();
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
                        pdfContent.SetTextMatrix(175, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(175, pdfToCreate.PageSize.Height - 46f);
                    }
                    pdfContent.ShowText("Prepared Date : " + currentDate + " " + DateTime.Now.ToString("hh:mm:ss tt"));
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

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase(lblContact.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Duration", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
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

                    pdfCell = new PdfPCell(new Phrase("1", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrRnwDate.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(memFees.ToString(), smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Total Amount :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + memFees.ToString(), smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.Border = 0;
                        pdfCell.BorderWidthTop = 0.5f;
                        pdfCell.BorderColorTop = BaseColor.DARK_GRAY;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
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
                        pdfContent.SetTextMatrix(50, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(50, pdfToCreate.PageSize.Height - 46f);
                    }
                    pdfContent.ShowText("Prepared Date : " + currentDate + " " + DateTime.Now.ToString("hh:mm:ss tt"));
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

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(lblBrId.Text, smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase(lblContact.Text, smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
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
                    pdfTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Duration", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(4);
                    pdfTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("1", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Membership Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrRnwDate.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(memFees.ToString(), smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Total Amount :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + memFees.ToString(), smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.Border = 0;
                        pdfCell.BorderWidthTop = 0.5f;
                        pdfCell.BorderColorTop = BaseColor.DARK_GRAY;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
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

        private void clearData()
        {
            txtbBrrId.Clear();
            txtbBrrName.Clear();
            txtbBrrMail.Clear();
            txtb2ndMail.Clear();
            txtbBrrContact.Clear();
            txtbBrrAddress.Clear();
            txtbAddtional1.Clear();
            txtbAdditional2.Clear();
            txtbAdditional3.Clear();
            txtbAdditional4.Clear();
            txtbAdditional5.Clear();
            txtbBrrRnwDate.Text = 0.ToString();
            txtbFreqncy.Text = 1.ToString();
            if (cmbPlan.Items.Count > 0)
            {
                cmbPlan.SelectedIndex = 0;
            }
            pcbBrrImage.Image = Properties.Resources.blankBrrImage;
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtbSearch.Clear();
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "";
                //===========================Data add to autocomplete=================
                if (cmbSearchBy.Text == "All Borrower")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    dgvBrrDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrDetails.Rows.Add(dataReader["brrId"].ToString(), dataReader["brrName"].ToString(),
                                dataReader["brrCategory"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrContact"].ToString(),
                                dataReader["brrAddress"].ToString());
                        }
                    }
                    dataReader.Close();
                    dgvBrrDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else
                {
                    txtbSearch.Enabled = true;
                    if (cmbSearchBy.SelectedIndex == 2)
                    {
                        queryString = "select distinct brrId from borrowerDetails";
                    }
                    else if (cmbSearchBy.SelectedIndex == 3)
                    {
                        queryString = "select distinct brrName from borrowerDetails";
                    }
                    else if (cmbSearchBy.SelectedIndex == 4)
                    {
                        queryString = "select distinct brrCategory from borrowerDetails";
                    }
                    else if (cmbSearchBy.SelectedIndex == 5)
                    {
                        queryString = "select distinct entryDate from borrowerDetails";
                    }
                    if (queryString != "")
                    {
                        sqltCommnd.CommandText = queryString;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        autoCollData.Clear();
                        if (dataReader.HasRows)
                        {
                            if (cmbSearchBy.SelectedIndex == 5)
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select FormatDate.getUserFormat((string)r[0])).ToList();
                                autoCollData.AddRange(idList.ToArray());
                            }
                            else
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
                            }
                        }
                        dataReader.Close();
                    }

                    if (cmbSearchBy.SelectedIndex == 6)
                    {
                        autoCollData.Clear();
                        autoCollData.Add("Male");
                        autoCollData.Add("Female");
                        autoCollData.Add("Other");
                    }

                    txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtbSearch.AutoCompleteCustomSource = autoCollData;
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
                string queryString = "";
                if (cmbSearchBy.Text == "All Borrower")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    dgvBrrDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrDetails.Rows.Add(dataReader["brrId"].ToString(), dataReader["brrName"].ToString(),
                                dataReader["brrCategory"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrContact"].ToString(),
                                dataReader["brrAddress"].ToString());
                        }
                    }
                    dataReader.Close();
                    dgvBrrDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else
                {
                    txtbSearch.Enabled = true;
                    if (cmbSearchBy.SelectedIndex == 2)
                    {
                        queryString = "select distinct brrId from borrower_details";
                    }
                    else if (cmbSearchBy.SelectedIndex == 3)
                    {
                        queryString = "select distinct brrName from borrower_details";
                    }
                    else if (cmbSearchBy.SelectedIndex == 4)
                    {
                        queryString = "select distinct brrCategory from borrower_details";
                    }
                    else if (cmbSearchBy.SelectedIndex == 5)
                    {
                        queryString = "select distinct entryDate from borrower_details";
                    }
                    if (queryString != "")
                    {
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        autoCollData.Clear();
                        if (dataReader.HasRows)
                        {
                            if (cmbSearchBy.SelectedIndex == 5)
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select FormatDate.getUserFormat((string)r[0])).ToList();
                                autoCollData.AddRange(idList.ToArray());
                            }
                            else
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
                            }
                            //List<string> idList = (from IDataRecord r in dataReader
                            //                       select (string)r[0]
                            //).ToList();
                            //autoCollData.AddRange(idList.ToArray());
                        }
                        dataReader.Close();
                    }

                    if (cmbSearchBy.SelectedIndex == 6)
                    {
                        autoCollData.Clear();
                        autoCollData.Add("Male");
                        autoCollData.Add("Female");
                        autoCollData.Add("Other");
                    }

                    txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtbSearch.AutoCompleteCustomSource = autoCollData;
                }
                mysqlConn.Close();
            }

            lblTotal.Text = "Total : " + dgvBrrDetails.Rows.Count.ToString();
        }

        private void pcbFullView_Click(object sender, EventArgs e)
        {
            updateToolStripMenuItem.Enabled = false;
            dgvBrrDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            grpbBrrList.Location = new Point(9, 0);
            if (this.Height == 551)
            {
                dgvBrrDetails.Height = 453;
                grpbBrrList.Height = 540;
            }
            else if (this.Height == 603)
            {
                dgvBrrDetails.Height = 505;
                grpbBrrList.Height = 592;
            }
            dgvBrrDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            pcbNormalView.Visible = true;
        }

        private void pcbNormalView_Click(object sender, EventArgs e)
        {
            grpbBrrList.Location = new Point(9, 291);
            if (this.Height == 551)
            {
                grpbBrrList.Height = 251;
                dgvBrrDetails.Height = 161;
            }
            else if (this.Height == 603)
            {
                dgvBrrDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                dgvBrrDetails.Height = 213;
                grpbBrrList.Height = 303;
            }
            updateToolStripMenuItem.Enabled = true;
            pcbNormalView.Visible = false;
        }

        private void txtbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbBrrContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
        }

        private void txtbBrrRnwDate_Leave(object sender, EventArgs e)
        {
            if (txtbBrrRnwDate.Text == "")
            {
                txtbBrrRnwDate.Text = 0.ToString();
            }
        }

        private void txtbBrrRnwDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void GenerateBorrowerId()
        {
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from brrIdSetting";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    bool isAutoGenerate = false, isManualPrefix = false; string prefixText = "", joiningChar = "";
                    while (dataReader.Read())
                    {
                        isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                        isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                        prefixText = dataReader["prefixText"].ToString();
                        joiningChar = dataReader["joiningChar"].ToString();
                        //lastNumber = Convert.ToInt32(dataReader["lastNumber"].ToString());
                    }
                    dataReader.Close();
                    if (isAutoGenerate)  //Auto Generate
                    {
                        txtbBrrId.Enabled = false;
                        if (!isManualPrefix) //Manual prefix
                        {
                            if (cmbBrrCategory.Text.Length < 3)
                            {
                                prefixText = cmbBrrCategory.Text.ToUpper();
                            }
                            else
                            {
                                prefixText = cmbBrrCategory.Text.ToUpper();
                                prefixText = prefixText.Substring(0, 3);
                            }
                        }
                        //lastNumber++;
                        string brrId = "";
                        sqltCommnd.CommandText = "select brrId from borrowerDetails where brrCategory=@catName and brrId like @brrId order by [id] desc limit 1";    //  
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                brrId = dataReader["brrId"].ToString();
                            }
                            dataReader.Close();

                            sqltCommnd.CommandText = "select brrId from borrowerDetails where brrCategory=@catName and brrId like @brrId";    //  
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                            sqltCommnd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                brrIdList = (from IDataRecord r in dataReader
                                             select (string)r["brrId"]).ToList();
                            }
                            dataReader.Close();

                            int.TryParse(new string(brrId.SkipWhile(x => !char.IsDigit(x)).TakeWhile(x => char.IsDigit(x))
                         .ToArray()), out lastNumber);
                            lastNumber++;
                            brrId = prefixText + joiningChar + lastNumber.ToString();
                            while (brrIdList.IndexOf(brrId) >= 0)
                            {
                                lastNumber++;
                                brrId = prefixText + joiningChar + lastNumber.ToString();
                            }
                            brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                            while (brrIdList.IndexOf(brrId) >= 0)
                            {
                                lastNumber++;
                                brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                            }
                        }
                        else
                        {
                            lastNumber = 1;
                        }
                        dataReader.Close();
                        txtbBrrId.Text = prefixText + joiningChar + lastNumber.ToString("00000");
                        txtbBrrName.Select();
                    }
                    else
                    {
                        txtbBrrId.Enabled = true;
                        txtbBrrId.Select();
                        lastNumber = 0;
                    }
                }
            }
            else
            {
                try
                {
                    bool isAutoGenerate = false, isManualPrefix = false, settingExist = false; string prefixText = "", joiningChar = "";
                    MySqlConnection mysqlConn;
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select * from brrid_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        settingExist = true;
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                            prefixText = dataReader["prefixText"].ToString();
                            joiningChar = dataReader["joiningChar"].ToString();
                        }
                    }
                    dataReader.Close();
                    if (settingExist)
                    {
                        if (isAutoGenerate)
                        {
                            txtbBrrId.Enabled = false;
                            if (!isManualPrefix) //Manual prefix
                            {
                                if (cmbBrrCategory.SelectedIndex > 0)
                                {
                                    if (cmbBrrCategory.Text.Length < 3)
                                    {
                                        prefixText = cmbBrrCategory.Text.ToUpper();
                                    }
                                    else
                                    {
                                        prefixText = cmbBrrCategory.Text.ToUpper();
                                        prefixText = prefixText.Substring(0, 3);
                                    }
                                }
                            }

                            queryString = "select brrId from borrower_details where brrCategory=@catName and brrId like @brrId";    //  
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                            mysqlCmd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                brrIdList = (from IDataRecord r in dataReader
                                             select (string)r["brrId"]).ToList();
                            }
                            dataReader.Close();

                            string brrId = "";
                            queryString = "select brrId from borrower_details where brrCategory=@catName and brrId like @brrId order by id desc limit 1";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                            mysqlCmd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    brrId = dataReader["brrId"].ToString();
                                }
                                dataReader.Close();

                                int.TryParse(new string(brrId.SkipWhile(x => !char.IsDigit(x)).TakeWhile(x => char.IsDigit(x))
                            .ToArray()), out lastNumber);
                                lastNumber++;
                                brrId = prefixText + joiningChar + lastNumber.ToString();
                                while (brrIdList.IndexOf(brrId) >= 0)
                                {
                                    lastNumber++;
                                    brrId = prefixText + joiningChar + lastNumber.ToString();
                                }
                                brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                                while (brrIdList.IndexOf(brrId) >= 0)
                                {
                                    lastNumber++;
                                    brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                                }
                            }
                            else
                            {
                                dataReader.Close();
                                lastNumber = 1;
                            }

                            txtbBrrId.Text = prefixText + joiningChar + lastNumber.ToString("00000");
                            txtbBrrName.Select();
                        }
                        else
                        {
                            txtbBrrId.Enabled = true;
                            txtbBrrId.Select();
                            lastNumber = 0;
                        }
                    }
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void txtbBrrId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
            }
            else
            {
                if (txtbBrrId.Text.Length > 11)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.blankBrrImage;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbBrrImage.Image.Dispose();
            pcbBrrImage.Image = globalVarLms.bimapImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        public void UcAddBorrower_Load(object sender, EventArgs e)
        {
            clearData();
            txtbBrrRnwDate.Text = 0.ToString();
            pcbNormalView.Visible = false;
            if (globalVarLms.isAdmin)
            {
                btnIdSetting.Enabled = true;
            }
            else
            {
                btnIdSetting.Enabled = false;
            }
            loadFieldValue();
        }

        private void loadFieldValue()
        {
            if (FieldSettings.Default.borrowerEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        Control[] cntrolCollection = this.Controls.Find(fieldName, true);
                        foreach (Control cntrl in cntrolCollection)
                        {
                            //if (cntrl.Name != lblAddItem.Name && cntrl.Name != lblSearchOption.Name)
                            //{
                            cntrl.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            //}
                            //else
                            //{
                            //    cntrl.Text = fieldValue.Replace(fieldName + "=", "");
                            //}
                            if (cntrl.Name == lblBrCategory.Name)
                            {
                                cmbSearchBy.Items[4] = cntrl.Text.Replace(" :", "");
                                categoryLabel = cntrl.Text.Replace(" :", "");
                                if (cmbBrrCategory.Items.Count > 0)
                                {
                                    cmbBrrCategory.Items[0] = "Please select a " + cntrl.Text.Replace(" :", "") + "...";
                                }
                                dgvBrrDetails.Columns[2].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblBrId.Name)
                            {
                                cmbSearchBy.Items[2] = cntrl.Text.Replace(" :", "");
                                idLabel = cntrl.Text.Replace(" :", "");
                                dgvBrrDetails.Columns[0].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblBrName.Name)
                            {
                                cmbSearchBy.Items[3] = cntrl.Text.Replace(" :", "");
                                dgvBrrDetails.Columns[1].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblBrAddress.Name)
                            {
                                dgvBrrDetails.Columns[5].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblContact.Name)
                            {
                                dgvBrrDetails.Columns[4].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                        }
                    }
                }
            }
        }

        private void dgvBrrDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvBrrDetails.HitTest(e.X, e.Y);
                dgvBrrDetails.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvBrrDetails.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvBrrDetails, new Point(e.X, e.Y));
                }
            }
        }

        void tooTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Segoe UI", 9.0f);
            Brush b = new SolidBrush(Color.WhiteSmoke);
            e.Graphics.FillRectangle(b, e.Bounds);
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.Red, new PointF(2, 2));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
            btnSave.Text = "Sa&ve";
            cmbBrrCategory.SelectedIndex = 0;
            dgvBrrDetails.ClearSelection();
        }

        private void txtbBrrMail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvBrrDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTotal.Text = "Total : " + dgvBrrDetails.Rows.Count.ToString();
        }

        private void dgvBrrDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTotal.Text = "Total : " + dgvBrrDetails.Rows.Count.ToString();
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

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBrrDetails.SelectedRows.Count == 1)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from borrowerDetails where brrId=@brrId";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            DateTime entryDate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtbBrrRnwDate.Text = dataReader["mbershipDuration"].ToString();
                            cmbPlan.Text = dataReader["memPlan"].ToString();
                            txtbFreqncy.Text = dataReader["memFreq"].ToString();
                            cmbBrrCategory.Text = dataReader["brrCategory"].ToString();
                            txtbBrrId.Text = dataReader["brrId"].ToString();
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbBrrAddress.Text = dataReader["brrAddress"].ToString();
                            txtbBrrMail.Text = dataReader["brrMailId"].ToString();
                            txtbBrrContact.Text = dataReader["brrContact"].ToString();
                            txtbAddtional1.Text = dataReader["addInfo1"].ToString();
                            txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                            txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                            txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                            txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                            txtb2ndMail.Text = dataReader["addnlMail"].ToString();
                            if (dataReader["brrGender"].ToString() == "Male")
                            {
                                rdbtMale.Checked = true;
                            }
                            else if (dataReader["brrGender"].ToString() == "Female")
                            {
                                rdbtFemale.Checked = true;
                            }
                            else
                            {
                                rdbOther.Checked = true;
                            }
                            txtbBrrId.Enabled = false;
                            if (dataReader["opkPermission"].ToString() == "True")
                            {
                                chkbOpak.Checked = true;
                            }
                            else
                            {
                                chkbOpak.Checked = false;
                            }
                            try
                            {
                                if (dataReader["imagePath"].ToString() != "base64String")
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
                        btnSave.Text = "Update";
                    }
                    if (txtbFreqncy.Text == "")
                    {
                        txtbFreqncy.Text = 1.ToString();
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
                    string queryString = "select * from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            DateTime entryDate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            txtbBrrRnwDate.Text = dataReader["mbershipDuration"].ToString();
                            cmbPlan.Text = dataReader["memPlan"].ToString();
                            txtbFreqncy.Text = dataReader["memFreq"].ToString();
                            cmbBrrCategory.Text = dataReader["brrCategory"].ToString();
                            txtbBrrId.Text = dataReader["brrId"].ToString();
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbBrrAddress.Text = dataReader["brrAddress"].ToString();
                            txtbBrrMail.Text = dataReader["brrMailId"].ToString();
                            txtbBrrContact.Text = dataReader["brrContact"].ToString();
                            txtbAddtional1.Text = dataReader["addInfo1"].ToString();
                            txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                            txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                            txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                            txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                            txtb2ndMail.Text = dataReader["addnlMail"].ToString();
                            if (dataReader["brrGender"].ToString() == "Male")
                            {
                                rdbtMale.Checked = true;
                            }
                            else if (dataReader["brrGender"].ToString() == "Female")
                            {
                                rdbtFemale.Checked = true;
                            }
                            else
                            {
                                rdbOther.Checked = true;
                            }
                            txtbBrrId.Enabled = false;
                            if (dataReader["opkPermission"].ToString() == "True")
                            {
                                chkbOpak.Checked = true;
                            }
                            else
                            {
                                chkbOpak.Checked = false;
                            }
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
                        btnSave.Text = "Update";
                    }
                    if (txtbFreqncy.Text == "")
                    {
                        txtbFreqncy.Text = 1.ToString();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBrrDetails.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure want to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "delete from borrowerDetails where brrId=@brrId";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[0].Value.ToString());
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
                        string queryString = "delete from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[0].Value.ToString());
                        mysqlCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                    dgvBrrDetails.Rows.RemoveAt(dgvBrrDetails.SelectedRows[0].Index);
                    globalVarLms.backupRequired = true;
                    lblUserMessage.Text = "Borrower deleted successfully !";
                    showNotification();
                }
                dgvBrrDetails.ClearSelection();
            }
        }

        private void updateToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.WhiteSmoke;
            updateToolStripMenuItem.ForeColor = Color.Black;
        }

        private void updateToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            updateToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void deleteToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.WhiteSmoke;
            deleteToolStripMenuItem.ForeColor = Color.Black;
        }

        private void deleteToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            deleteToolStripMenuItem.ForeColor = Color.WhiteSmoke;
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

        private void lnkLblFingerPrint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("No fingerprint scanner detected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void resetPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvBrrDetails.Rows.Count > 0)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select defaultPass from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvBrrDetails.SelectedRows[0].Cells[2].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    string defaultPass = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            defaultPass = dataReader["defaultPass"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltCommnd.CommandText = "update borrowerDetails set brrPass=:brrPass where brrId=@brrId";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[2].Value.ToString());
                    sqltCommnd.Parameters.AddWithValue("brrPass", defaultPass);
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
                    string queryString = "select defaultPass from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvBrrDetails.SelectedRows[0].Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            defaultPass = dataReader["defaultPass"].ToString();
                        }
                    }
                    dataReader.Close();
                    queryString = "update borrower_details set brrPass=@brrPass where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", dgvBrrDetails.SelectedRows[0].Cells[2].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@brrPass", defaultPass);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                lblUserMessage.Text = "Password reset successfully !";
                showNotification();
            }
        }

        private void resetPasswordToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            resetPasswordToolStripMenuItem.BackColor = Color.WhiteSmoke;
            resetPasswordToolStripMenuItem.ForeColor = Color.Black;
        }

        private void txtbFreqncy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbFreqncy_Leave(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text == "")
            {
                txtbFreqncy.Text = 1.ToString();
            }
        }

        private void dgvBrrDetails_SelectionChanged(object sender, EventArgs e)
        {
            clearData();
            btnSave.Text = "Sa&ve";
            cmbBrrCategory.SelectedIndex = 0;
        }

        private void txtbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtbSearch.Text != "")
                {
                    dgvBrrDetails.Rows.Clear();
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        string queryString = "";
                        if (cmbSearchBy.SelectedIndex == 2)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails where [brrId]=  '" + txtbSearch.Text + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                        }
                        else if (cmbSearchBy.SelectedIndex == 3)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails where brrName=@brrName collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrName", txtbSearch.Text);
                        }
                        else if (cmbSearchBy.SelectedIndex == 4)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails where brrCategory=@brrCategory collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrCategory", txtbSearch.Text);
                        }
                        else if (cmbSearchBy.SelectedIndex == 5)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails where entryDate=@entryDate collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@entryDate", FormatDate.getAppFormat(txtbSearch.Text));
                        }
                        else if (cmbSearchBy.SelectedIndex == 6)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrowerDetails where brrGender=@brrGender collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrGender", txtbSearch.Text);
                        }

                        if (queryString != "")
                        {
                            contextMenuStrip1.Enabled = false;
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                dgvBrrDetails.Rows.Clear();
                                while (dataReader.Read())
                                {
                                    dgvBrrDetails.Rows.Add(dataReader["brrId"].ToString(), dataReader["brrName"].ToString(),
                                     dataReader["brrCategory"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrContact"].ToString(),
                                     dataReader["brrAddress"].ToString());
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            contextMenuStrip1.Enabled = true;
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
                        MySqlCommand mysqlCmd = null;
                        string queryString = "";
                        if (cmbSearchBy.SelectedIndex == 2)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details where lower(brrId)=  '" + txtbSearch.Text.ToLower() + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        }
                        else if (cmbSearchBy.SelectedIndex == 3)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details where lower(brrName)=@brrName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrName", txtbSearch.Text.ToLower());
                        }
                        else if (cmbSearchBy.SelectedIndex == 4)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details where lower(brrCategory)=@brrCategory";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrCategory", txtbSearch.Text.ToLower());
                        }
                        else if (cmbSearchBy.SelectedIndex == 5)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details where entryDate=@entryDate";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@entryDate", FormatDate.getAppFormat(txtbSearch.Text));
                        }
                        else if (cmbSearchBy.SelectedIndex == 6)
                        {
                            queryString = "select brrId,brrName,brrCategory,brrGender,brrContact,brrAddress from borrower_details where lower(brrGender)=@brrGender";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrGender", txtbSearch.Text.ToLower());
                        }

                        if (queryString != "")
                        {
                            contextMenuStrip1.Enabled = false;
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                dgvBrrDetails.Rows.Clear();
                                while (dataReader.Read())
                                {
                                    dgvBrrDetails.Rows.Add(dataReader["brrId"].ToString(), dataReader["brrName"].ToString(),
                                     dataReader["brrCategory"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrContact"].ToString(),
                                     dataReader["brrAddress"].ToString());
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            contextMenuStrip1.Enabled = true;
                        }
                        mysqlConn.Close();
                    }
                    dgvBrrDetails.ClearSelection();
                }
            }
        }

        private void btnRecieptSetting_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Receipt Setting", btnRecieptSetting);
        }

        private void btnRecieptSetting_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnRecieptSetting);
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

        private void btnAddCategory_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Add Category", btnAddCategory);
        }

        private void btnAddCategory_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnAddCategory);
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            globalVarLms.addCateory = "brrCategory";
        }

        private void btnIdSetting_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("Borrower Id Setting", btnIdSetting);
        }

        private void lblBrId_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbBrrId.Location.X, txtbBrrId.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblBrName_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbBrrName.Location.X, txtbBrrName.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            panelField.Visible = false;
        }

        private void lblBrAddress_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbBrrName.Location.X, txtbBrrName.Location.Y + 25);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblMailId_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbBrrMail.Location.X, txtbBrrMail.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblContact_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbBrrContact.Location.X, txtbBrrContact.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Control[] cntrolCollection = this.Controls.Find(controlName, true);
            foreach (Control cntrl in cntrolCollection)
            {
                cntrl.Text = txtbFieldName.Text;
                if (FieldSettings.Default.borrowerEntry != "")
                {
                    if (FieldSettings.Default.borrowerEntry.Contains(cntrl.Name + "="))
                    {
                        string prevText = FieldSettings.Default.borrowerEntry.Substring(FieldSettings.Default.borrowerEntry.IndexOf(cntrl.Name + "=") + (cntrl.Name + "=").Count());
                        prevText = prevText.Substring(0, prevText.IndexOf("|") + 1);
                        FieldSettings.Default.borrowerEntry = FieldSettings.Default.borrowerEntry.Replace(cntrl.Name + "=" + prevText, cntrl.Name + "=" + cntrl.Text + "|");
                    }
                    else
                    {
                        FieldSettings.Default.borrowerEntry = FieldSettings.Default.borrowerEntry + cntrl.Name + "=" + cntrl.Text + "|";
                    }
                }
                else
                {
                    FieldSettings.Default.borrowerEntry = cntrl.Name + "=" + cntrl.Text + "|";
                }

                if (cntrl.Name == lblBrCategory.Name)
                {
                    categoryLabel = cntrl.Text;
                    cmbSearchBy.Items[4] = cntrl.Text;
                    dgvBrrDetails.Columns[2].HeaderText = cntrl.Text;
                    if (cmbBrrCategory.Items.Count > 0)
                    {
                        cmbBrrCategory.Items[0] = "Please select a " + cntrl.Text + "...";
                    }
                }
                else if (cntrl.Name == lblBrId.Name)
                {
                    cmbSearchBy.Items[2] = cntrl.Text;
                    idLabel = cntrl.Text;
                    dgvBrrDetails.Columns[0].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblBrName.Name)
                {
                    cmbSearchBy.Items[3] = cntrl.Text;
                    dgvBrrDetails.Columns[1].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblBrAddress.Name)
                {
                    dgvBrrDetails.Columns[5].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblContact.Name)
                {
                    dgvBrrDetails.Columns[4].HeaderText = cntrl.Text;
                }
                cntrl.Text = cntrl.Text + " :";
            }
            FieldSettings.Default.Save();
            panelField.Visible = false;
        }

        private void btnIdSetting_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnIdSetting);
        }

        private void btnIdSetting_Click(object sender, EventArgs e)
        {
            cmbBrrCategory.SelectedIndex = 0;
            txtbBrrId.Clear();
            txtbBrrId.Enabled = false;
            FormBrrIdSetting idSetting = new FormBrrIdSetting();
            idSetting.ShowDialog();
        }

        private void lblBrCategory_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(cmbBrrCategory.Location.X, cmbBrrCategory.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void txtbFreqncy_TextChanged(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text != "")
            {
                if (memFees > 0)
                {
                    chkbFees.Enabled = true;
                    chkbFees.Text = "Paid Membership Fees : " + globalVarLms.currSymbol + (memFees * Convert.ToInt32(txtbFreqncy.Text)).ToString("0.00");
                    chkbFees.Checked = true;
                }
                else
                {
                    chkbFees.Enabled = false;
                    chkbFees.Text = "Paid Membership Fees : ";
                    chkbFees.Checked = false;
                }
                txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                totalFees = memFees * Convert.ToInt32(txtbFreqncy.Text);
            }
        }

        private void resetPasswordToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            resetPasswordToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            resetPasswordToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void cmbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkbFees.Text = "Paid Membership Fees : ";
            lblDuration.Text = "Duration :";
            validDays = 0; issueLimit = 0;
            memFees = 0.00;
            if (cmbPlan.SelectedIndex != 0)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select membrDurtn,membrFees,issueLimit from mbershipSetting where membrshpName=@membrshpName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@membrshpName", cmbPlan.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            memFees = Convert.ToDouble(dataReader["membrFees"].ToString());
                            validDays = Convert.ToInt32(dataReader["membrDurtn"].ToString());
                            issueLimit = Convert.ToInt32(dataReader["issueLimit"].ToString());
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
                    string queryString = "select membrDurtn,membrFees,issueLimit from mbership_setting where membrshpName=@membrshpName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@membrshpName", cmbPlan.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            memFees = Convert.ToDouble(dataReader["membrFees"].ToString());
                            validDays = Convert.ToInt32(dataReader["membrDurtn"].ToString());
                            issueLimit = Convert.ToInt32(dataReader["issueLimit"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }

                if (memFees > 0)
                {
                    chkbFees.Enabled = true;
                    chkbFees.Text = "Paid Membership Fees : " + globalVarLms.currSymbol + (memFees * Convert.ToInt32(txtbFreqncy.Text)).ToString("0.00");
                    chkbFees.Checked = true;
                }
                else
                {
                    chkbFees.Enabled = false;
                    chkbFees.Text = "Paid Membership Fees : ";
                    chkbFees.Checked = false;
                }
                txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                totalFees = memFees * Convert.ToInt32(txtbFreqncy.Text);
            }
        }
    }
}
