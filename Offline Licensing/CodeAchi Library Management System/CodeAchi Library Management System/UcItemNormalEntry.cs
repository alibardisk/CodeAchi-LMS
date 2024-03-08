using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
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
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class UcItemNormalEntry : UserControl
    {
        public UcItemNormalEntry()
        {
            InitializeComponent();
        }

        string controlName = "", categoryLabel = "", accessionLabel = "", subcategoryLabel = "";
        bool isUpdate = false;
        int lastNumber = 0;
        AutoCompleteStringCollection autoCompData = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompTitle = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompIsbn = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompAuthor = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompClassification = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompSubject = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompRack = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompPages = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompPrices = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo1 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo2 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo3 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo4 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo5 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo6 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo7 = new AutoCompleteStringCollection();
        AutoCompleteStringCollection autoCompInfo8 = new AutoCompleteStringCollection();
        List<string> accnList = new List<string> { };

        private void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            lblMessage.Text = "Maximum file size 500 KB";
            pcbBook.Image = Properties.Resources.NoImageAvailable;
            if (cmbItemCategory.SelectedIndex == 0)
            {
                lblInfo1.Text = "Not set from item setting";
                lblInfo2.Text = "Not set from item setting";
                lblInfo3.Text = "Not set from item setting";
                lblInfo4.Text = "Not set from item setting";
                lblInfo5.Text = "Not set from item setting";
                lblInfo6.Text = "Not set from item setting";
                lblInfo7.Text = "Not set from item setting";
                lblInfo8.Text = "Not set from item setting";
                txtbAdditional1.Enabled = false;
                txtbAdditional2.Enabled = false;
                txtbAdditional3.Enabled = false;
                txtbAdditional4.Enabled = false;
                txtbAdditional5.Enabled = false;
                txtbAdditional6.Enabled = false;
                txtbAdditional7.Enabled = false;
                txtbAdditional8.Enabled = false;
                if (cmbSubCategory.Items.Count > 0)
                {
                    cmbSubCategory.SelectedIndex = 0;
                }
                return;
            }
            txtbAccession.TabIndex = 0;
            txtbIsbn.TabIndex = 1;
            btnIsbnSearch.TabIndex = 2;
            txtbTitel.TabIndex = 3;
            txtbAuthor.TabIndex = 4;
            txtbCalssification.TabIndex = 5;
            txtbSubject.TabIndex = 6;
            txtbRack.TabIndex = 7;
            txtbPages.TabIndex = 8;
            txtbPrice.TabIndex = 9;
            cmbSubCategory.Items.Clear();
            cmbSubCategory.Items.Add("Please select a " + subcategoryLabel + "...");

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //========================Item Subcategory add to the combobox====================
                string queryString = "select subCatName from itemSubCategory where catName=@catName order by subCatName asc";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbSubCategory.Items.Add(dataReader["subCatName"].ToString());
                    }
                }
                dataReader.Close();
                cmbSubCategory.SelectedIndex = 0;
                //========================get additional info====================
                queryString = "select capInfo from itemSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                dataReader = sqltCommnd.ExecuteReader();
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
                                    txtbAdditional1.Enabled = true;
                                    txtbAdditional1.TabIndex = 10;
                                    btnUpload.TabIndex = 11;
                                    btnSave.TabIndex = 12;
                                    lblInfo2.Text = "Not set from item setting";
                                    lblInfo3.Text = "Not set from item setting";
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 1)
                                {
                                    lblInfo2.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional2.Enabled = true;
                                    txtbAdditional2.TabIndex = 11;
                                    btnUpload.TabIndex = 12;
                                    btnSave.TabIndex = 13;
                                    lblInfo3.Text = "Not set from item setting";
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 2)
                                {
                                    lblInfo3.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional3.Enabled = true;
                                    txtbAdditional3.TabIndex = 12;
                                    btnUpload.TabIndex = 13;
                                    btnSave.TabIndex = 14;
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 3)
                                {
                                    lblInfo4.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional4.Enabled = true;
                                    txtbAdditional4.TabIndex = 13;
                                    btnUpload.TabIndex = 14;
                                    btnSave.TabIndex = 15;
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 4)
                                {
                                    lblInfo5.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional5.Enabled = true;
                                    txtbAdditional5.TabIndex = 14;
                                    btnUpload.TabIndex = 15;
                                    btnSave.TabIndex = 16;
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 5)
                                {
                                    lblInfo6.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional6.Enabled = true;
                                    txtbAdditional6.TabIndex = 15;
                                    btnUpload.TabIndex = 16;
                                    btnSave.TabIndex = 17;
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 6)
                                {
                                    lblInfo7.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional7.Enabled = true;
                                    txtbAdditional7.TabIndex = 16;
                                    btnUpload.TabIndex = 17;
                                    btnSave.TabIndex = 18;
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 7)
                                {
                                    lblInfo8.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional8.Enabled = true;
                                    txtbAdditional8.TabIndex = 17;
                                    btnUpload.TabIndex = 18;
                                    btnSave.TabIndex = 19;
                                }
                            }
                        }
                        else
                        {
                            lblInfo1.Text = "Not set from item setting";
                            lblInfo2.Text = "Not set from item setting";
                            lblInfo3.Text = "Not set from item setting";
                            lblInfo4.Text = "Not set from item setting";
                            lblInfo5.Text = "Not set from item setting";
                            lblInfo6.Text = "Not set from item setting";
                            lblInfo7.Text = "Not set from item setting";
                            lblInfo8.Text = "Not set from item setting";
                            txtbAdditional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                            txtbAdditional6.Enabled = false;
                            txtbAdditional7.Enabled = false;
                            txtbAdditional8.Enabled = false;
                            btnUpload.TabIndex = 10;
                            btnSave.TabIndex = 11;
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
                string queryString = "select subCatName from item_subcategory where catName=@catName order by subCatName asc";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbSubCategory.Items.Add(dataReader["subCatName"].ToString());
                    }
                }
                dataReader.Close();

                queryString = "select capInfo from item_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
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
                                    txtbAdditional1.Enabled = true;
                                    txtbAdditional1.TabIndex = 10;
                                    btnUpload.TabIndex = 11;
                                    btnSave.TabIndex = 12;
                                    lblInfo2.Text = "Not set from item setting";
                                    lblInfo3.Text = "Not set from item setting";
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 1)
                                {
                                    lblInfo2.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional2.Enabled = true;
                                    txtbAdditional2.TabIndex = 11;
                                    btnUpload.TabIndex = 12;
                                    btnSave.TabIndex = 13;
                                    lblInfo3.Text = "Not set from item setting";
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 2)
                                {
                                    lblInfo3.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional3.Enabled = true;
                                    txtbAdditional3.TabIndex = 12;
                                    btnUpload.TabIndex = 13;
                                    btnSave.TabIndex = 14;
                                    lblInfo4.Text = "Not set from item setting";
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 3)
                                {
                                    lblInfo4.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional4.Enabled = true;
                                    txtbAdditional4.TabIndex = 13;
                                    btnUpload.TabIndex = 14;
                                    btnSave.TabIndex = 15;
                                    lblInfo5.Text = "Not set from item setting";
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 4)
                                {
                                    lblInfo5.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional5.Enabled = true;
                                    txtbAdditional5.TabIndex = 14;
                                    btnUpload.TabIndex = 15;
                                    btnSave.TabIndex = 16;
                                    lblInfo6.Text = "Not set from item setting";
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 5)
                                {
                                    lblInfo6.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional6.Enabled = true;
                                    txtbAdditional6.TabIndex = 15;
                                    btnUpload.TabIndex = 16;
                                    btnSave.TabIndex = 17;
                                    lblInfo7.Text = "Not set from item setting";
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 6)
                                {
                                    lblInfo7.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional7.Enabled = true;
                                    txtbAdditional7.TabIndex = 16;
                                    btnUpload.TabIndex = 17;
                                    btnSave.TabIndex = 18;
                                    lblInfo8.Text = "Not set from item setting";
                                }
                                else if (i == 7)
                                {
                                    lblInfo8.Text = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                    txtbAdditional8.Enabled = true;
                                    txtbAdditional8.TabIndex = 17;
                                    btnUpload.TabIndex = 18;
                                    btnSave.TabIndex = 19;
                                }
                            }
                        }
                        else
                        {
                            lblInfo1.Text = "Not set from item setting";
                            lblInfo2.Text = "Not set from item setting";
                            lblInfo3.Text = "Not set from item setting";
                            lblInfo4.Text = "Not set from item setting";
                            lblInfo5.Text = "Not set from item setting";
                            lblInfo6.Text = "Not set from item setting";
                            lblInfo7.Text = "Not set from item setting";
                            lblInfo8.Text = "Not set from item setting";
                            txtbAdditional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                            txtbAdditional6.Enabled = false;
                            txtbAdditional7.Enabled = false;
                            txtbAdditional8.Enabled = false;
                            btnUpload.TabIndex = 10;
                            btnSave.TabIndex = 11;
                        }
                    }
                }
                dataReader.Close();

                mysqlConn.Close();
            }
            cmbSubCategory.SelectedIndex = 0;
            cmbSubCategory.Select();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbItemCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbSubCategory.SelectedIndex == -1 || cmbSubCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbTitel.Text == "")
            {
                MessageBox.Show("Please add the title.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbTitel.Select();
                return;
            }
            if (txtbRack.Text == "")
            {
                MessageBox.Show("Please enter the location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbRack.Select();
                return;
            }
            if (txtbAccession.Text == "")
            {
                MessageBox.Show("Please add the accession.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbAccession.Select();
                return;
            }
            if (btnSave.Text == "Sa&ve")    //=======================Item Inserting=============
            {
                int ttlBooks = 0;
                string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                String base64String = "base64String";
                if (pcbBook.Image != Properties.Resources.NoImageAvailable && pcbBook.Image != Properties.Resources.uploadingFail)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        pcbBook.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = memoryStream.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                    }
                }

                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    //====================check for Accession no exist======================
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count(id) from itemDetails;";
                    sqltCommnd.CommandType = CommandType.Text;
                    ttlBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                    if (ttlBooks >= globalVarLms.itemLimits)
                    {
                        MessageBox.Show("You can't add more than " + globalVarLms.itemLimits.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string queryString = "select itemAccession from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Accession Number can not be same for 2 Item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();

                    //=====================Insert=============
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "INSERT INTO itemDetails (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                        "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                        "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                        "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                        ",'" + txtbRack.Text + "','" + txtbPages.Text + "','" + txtbPrice.Text + "'" +
                        ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                        ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubCategory.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemClassification", txtbCalssification.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemSubject", txtbSubject.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo1", txtbAdditional1.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo6", txtbAdditional6.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo7", txtbAdditional7.Text);
                    sqltCommnd.Parameters.AddWithValue("@addInfo8", txtbAdditional8.Text);
                    sqltCommnd.ExecuteNonQuery();

                    if (autoCompTitle.IndexOf(txtbTitel.Text) == -1)
                    {
                        autoCompTitle.Add(txtbTitel.Text);
                        txtbTitel.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbTitel.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbTitel.AutoCompleteCustomSource = autoCompTitle;
                    }
                    if (lastNumber > 0)
                    {
                        queryString = "update accnSetting  set lastNumber='" + lastNumber + "'";
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.ExecuteNonQuery();
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
                    string queryString = "select count(id) from item_details;";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    ttlBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                    if (ttlBooks >= globalVarLms.itemLimits)
                    {
                        MessageBox.Show("You can't add more than " + globalVarLms.itemLimits.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    queryString = "select itemAccession from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Accession Number can not be same for 2 Item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();

                    queryString = "INSERT INTO item_details (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                       "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                       "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                       "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                       ",'" + txtbRack.Text + "','" + txtbPages.Text + "','" + txtbPrice.Text + "'" +
                       ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                       ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubCategory.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemClassification", txtbCalssification.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemSubject", txtbSubject.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo1", txtbAdditional1.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo6", txtbAdditional6.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo7", txtbAdditional7.Text);
                    mysqlCmd.Parameters.AddWithValue("@addInfo8", txtbAdditional8.Text);
                    mysqlCmd.ExecuteNonQuery();

                    if (autoCompTitle.IndexOf(txtbTitel.Text) == -1)
                    {
                        autoCompTitle.Add(txtbTitel.Text);
                        txtbTitel.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbTitel.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbTitel.AutoCompleteCustomSource = autoCompTitle;
                    }
                    if (lastNumber > 0)
                    {
                        queryString = "update accn_setting  set lastNumber='" + lastNumber + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                    mysqlConn.Close();
                }
                dgvItemDetails.Rows.Add(txtbAccession.Text, txtbTitel.Text, txtbSubject.Text, txtbAuthor.Text, txtbRack.Text, "Available");
                dgvItemDetails.ClearSelection();
                addDataAuto();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Item added successfully !";
                showNotification();
                clearData();
                isUpdate = false;
                GenerateAccession();
            }
            else //====================Item updating===============
            {
                try
                {
                    String base64String = "base64String";
                    if (pcbBook.Image != Properties.Resources.NoImageAvailable && pcbBook.Image != Properties.Resources.uploadingFail)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            pcbBook.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = memoryStream.ToArray();
                            base64String = Convert.ToBase64String(imageBytes);
                        }
                    }
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "update itemDetails set itemTitle=:itemTitle,itemIsbn='" + txtbIsbn.Text + "'" +
                            ",itemCat=:itemCat,itemSubCat=:itemSubCat,itemAuthor=:itemAuthor" +
                            ",itemClassification=:itemClassification,itemSubject=:itemSubject" +
                            ",rackNo='" + txtbRack.Text + "',totalPages='" + txtbPages.Text + "',itemPrice='" + txtbPrice.Text + "'" +
                            ",addInfo1=:addInfo1,addInfo2=:addInfo2,addInfo3=:addInfo3" +
                            ",addInfo4=:addInfo4,addInfo5=:addInfo5,addInfo6=:addInfo6" +
                            ",addInfo7=:addInfo7,addInfo8=:addInfo8,itemImage = '" + base64String + "' where itemAccession=@itemAccession";

                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                        sqltCommnd.Parameters.AddWithValue("itemTitle", txtbTitel.Text);
                        sqltCommnd.Parameters.AddWithValue("itemCat", cmbItemCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("itemSubCat", cmbSubCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("itemAuthor", txtbAuthor.Text);
                        sqltCommnd.Parameters.AddWithValue("itemClassification", txtbCalssification.Text);
                        sqltCommnd.Parameters.AddWithValue("itemSubject", txtbSubject.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo1", txtbAdditional1.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo2", txtbAdditional2.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo3", txtbAdditional3.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo4", txtbAdditional4.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo5", txtbAdditional5.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo6", txtbAdditional6.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo7", txtbAdditional7.Text);
                        sqltCommnd.Parameters.AddWithValue("addInfo8", txtbAdditional8.Text);
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
                        string queryString = "update item_details set itemTitle=@itemTitle,itemIsbn='" + txtbIsbn.Text + "'" +
                            ",itemCat=@itemCat,itemSubCat=@itemSubCat,itemAuthor=@itemAuthor" +
                            ",itemClassification=@itemClassification,itemSubject=@itemSubject" +
                            ",rackNo='" + txtbRack.Text + "',totalPages='" + txtbPages.Text + "',itemPrice='" + txtbPrice.Text + "'" +
                            ",addInfo1=@addInfo1,addInfo2=@addInfo2,addInfo3=@addInfo3" +
                            ",addInfo4=@addInfo4,addInfo5=@addInfo5,addInfo6=@addInfo6" +
                            ",addInfo7=@addInfo7,addInfo8=@addInfo8,itemImage = '" + base64String + "' where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubCategory.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemClassification", txtbCalssification.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemSubject", txtbSubject.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo1", txtbAdditional1.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo2", txtbAdditional2.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo3", txtbAdditional3.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo4", txtbAdditional4.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo5", txtbAdditional5.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo6", txtbAdditional6.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo7", txtbAdditional7.Text);
                        mysqlCmd.Parameters.AddWithValue("@addInfo8", txtbAdditional8.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                    dgvItemDetails.SelectedRows[0].Cells[1].Value = txtbTitel.Text;
                    dgvItemDetails.SelectedRows[0].Cells[2].Value = txtbSubject.Text;
                    dgvItemDetails.SelectedRows[0].Cells[3].Value = txtbAuthor.Text;
                    dgvItemDetails.SelectedRows[0].Cells[4].Value = txtbRack.Text;
                    globalVarLms.backupRequired = true;
                    lblUserMessage.Text = "Item updated successfully !";
                    showNotification();
                }
                catch
                {

                }
                clearData();
                isUpdate = false;
                btnSave.Text = "Sa&ve";
                txtbAccession.Enabled = true;
                cmbSearchBy.SelectedIndex = 0;

            }
            pcbBook.Image = Properties.Resources.NoImageAvailable;
        }

        private void addDataAuto()
        {
            if (autoCompIsbn.IndexOf(txtbIsbn.Text) < 0)
            {
                autoCompIsbn.Add(txtbIsbn.Text);
            }

            if (autoCompTitle.IndexOf(txtbTitel.Text) < 0)
            {
                autoCompTitle.Add(txtbTitel.Text);
            }

            if (autoCompAuthor.IndexOf(txtbAuthor.Text) < 0)
            {
                autoCompAuthor.Add(txtbAuthor.Text);
            }

            if (autoCompClassification.IndexOf(txtbCalssification.Text) < 0)
            {
                autoCompClassification.Add(txtbCalssification.Text);
            }

            if (autoCompSubject.IndexOf(txtbSubject.Text) < 0)
            {
                autoCompSubject.Add(txtbSubject.Text);
            }

            if (autoCompRack.IndexOf(txtbRack.Text) < 0)
            {
                autoCompRack.Add(txtbRack.Text);
            }

            if (autoCompPages.IndexOf(txtbPages.Text) < 0)
            {
                autoCompPages.Add(txtbPages.Text);
            }

            if (autoCompPrices.IndexOf(txtbPrice.Text) < 0)
            {
                autoCompPrices.Add(txtbPrice.Text);
            }

            if (autoCompInfo1.IndexOf(txtbAdditional1.Text) < 0)
            {
                autoCompInfo1.Add(txtbAdditional1.Text);
            }

            if (autoCompInfo2.IndexOf(txtbAdditional2.Text) < 0)
            {
                autoCompInfo2.Add(txtbAdditional2.Text);
            }

            if (autoCompInfo3.IndexOf(txtbAdditional3.Text) < 0)
            {
                autoCompInfo3.Add(txtbAdditional3.Text);
            }

            if (autoCompInfo4.IndexOf(txtbAdditional4.Text) < 0)
            {
                autoCompInfo4.Add(txtbAdditional4.Text);
            }

            if (autoCompInfo5.IndexOf(txtbAdditional5.Text) < 0)
            {
                autoCompInfo5.Add(txtbAdditional5.Text);
            }

            if (autoCompInfo6.IndexOf(txtbAdditional6.Text) < 0)
            {
                autoCompInfo6.Add(txtbAdditional6.Text);
            }

            if (autoCompInfo7.IndexOf(txtbAdditional7.Text) < 0)
            {
                autoCompInfo7.Add(txtbAdditional7.Text);
            }

            if (autoCompInfo8.IndexOf(txtbAdditional8.Text) < 0)
            {
                autoCompInfo8.Add(txtbAdditional8.Text);
            }
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtbSearch.Clear();
            string queryString = "";
            dgvItemDetails.Rows.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //===========================Data add to autocomplete=================
                if (cmbSearchBy.Text == "All Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            if (Convert.ToBoolean(dataReader["itemAvailable"].ToString()) == true)
                            {
                                if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                                    dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                                    dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                       dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                                }
                            }
                            else
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                    dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                                dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Available Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where itemAvailable='" + true + "' and isLost='" + false + "' and isDamage='" + false + "'";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Issued Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where itemAvailable='" + false + "'";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Lost/Damage Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where isLost='" + true + "' or isDamage='" + true + "'";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                  dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                            }
                            else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                  dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else
                {
                    txtbSearch.Enabled = true;
                    if (cmbSearchBy.SelectedIndex == 2)
                    {
                        queryString = "select distinct itemTitle from itemDetails";
                        txtbSearch.Width = 503;
                    }
                    else if (cmbSearchBy.SelectedIndex == 3)
                    {
                        queryString = "select distinct itemAccession from itemDetails";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 4)
                    {
                        queryString = "select distinct itemIsbn from itemDetails";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 5)
                    {
                        queryString = "select distinct itemAuthor from itemDetails";
                        txtbSearch.Width = 503;
                    }
                    else if (cmbSearchBy.SelectedIndex == 6)
                    {
                        queryString = "select distinct itemCat from itemDetails";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 7)
                    {
                        queryString = "select distinct rackNo from itemDetails";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 8)
                    {
                        queryString = "select distinct itemSubject from itemDetails";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 9)
                    {
                        queryString = "select distinct subCatName from itemSubCategory";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.Text == "Entry Date")
                    {
                        queryString = "select distinct entryDate from itemDetails";
                        txtbSearch.Width = 228;
                    }

                    if (queryString != "")
                    {
                        sqltCommnd.CommandText = queryString;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        autoCompData.Clear();
                        if (dataReader.HasRows)
                        {
                            if (cmbSearchBy.Text == "Entry Date")
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select FormatDate.getUserFormat((string)r[0])).ToList();
                                autoCompData.AddRange(idList.ToArray());
                            }
                            else
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCompData.AddRange(idList.ToArray());
                            }
                        }
                        dataReader.Close();
                        txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbSearch.AutoCompleteCustomSource = autoCompData;
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
                if (cmbSearchBy.Text == "All Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            if (Convert.ToBoolean(dataReader["itemAvailable"].ToString()) == true)
                            {
                                if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                                    dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                                    dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                }
                                else
                                {
                                    dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                       dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                                }
                            }
                            else
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                    dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                                dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Available Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where itemAvailable='" + true + "' and isLost='" + false + "' and isDamage='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Issued Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where itemAvailable='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                      dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else if (cmbSearchBy.Text == "Lost/Damage Items")
                {
                    contextMenuStrip1.Enabled = false;
                    txtbSearch.Enabled = false;
                    queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where isLost='" + true + "' or isDamage='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    dgvItemDetails.Rows.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            contextMenuStrip1.Visible = false;
                            if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                  dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                            }
                            else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                            {
                                dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                  dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    dgvItemDetails.ClearSelection();
                    contextMenuStrip1.Enabled = true;
                }
                else
                {
                    txtbSearch.Enabled = true;
                    if (cmbSearchBy.SelectedIndex == 2)
                    {
                        queryString = "select distinct itemTitle from item_details";
                        txtbSearch.Width = 503;
                    }
                    else if (cmbSearchBy.SelectedIndex == 3)
                    {
                        queryString = "select distinct itemAccession from item_details";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 4)
                    {
                        queryString = "select distinct itemIsbn from item_details";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 5)
                    {
                        queryString = "select distinct itemAuthor from item_details";
                        txtbSearch.Width = 503;
                    }
                    else if (cmbSearchBy.SelectedIndex == 6)
                    {
                        queryString = "select distinct itemCat from item_details";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 7)
                    {
                        queryString = "select distinct rackNo from item_details";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 8)
                    {
                        queryString = "select distinct itemSubject from item_details";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.SelectedIndex == 9)
                    {
                        queryString = "select distinct subCatName from item_subcategory";
                        txtbSearch.Width = 228;
                    }
                    else if (cmbSearchBy.Text == "Entry Date")
                    {
                        queryString = "select distinct entryDate from item_details";
                        txtbSearch.Width = 228;
                    }

                    if (queryString != "")
                    {
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        autoCompData.Clear();
                        if (dataReader.HasRows)
                        {
                            if (cmbSearchBy.Text == "Entry Date")
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select FormatDate.getUserFormat((string)r[0])).ToList();
                                autoCompData.AddRange(idList.ToArray());
                            }
                            else
                            {
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCompData.AddRange(idList.ToArray());
                            }
                            //List<string> idList = (from IDataRecord r in dataReader
                            //                       select (string)r[0]).ToList();
                            //autoCompData.AddRange(idList.ToArray());
                        }
                        dataReader.Close();
                        txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbSearch.AutoCompleteCustomSource = autoCompData;
                    }
                }
                mysqlConn.Close();
            }
            lblTotal.Text = "Total : " + dgvItemDetails.Rows.Count.ToString();
        }

        private void pcbFullView_Click(object sender, EventArgs e)
        {
            updateToolStripMenuItem.Enabled = false;
            dgvItemDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
            grpbItemList.Location = new Point(8, 0);
            if (this.Height == 551)
            {
                dgvItemDetails.Height = 453;
                grpbItemList.Height = 540;
            }
            else if (this.Height == 603)
            {
                dgvItemDetails.Height = 505;
                grpbItemList.Height = 592;
            }
            dgvItemDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            pcbNormalView.Visible = true;
        }

        private void pcbNormalView_Click(object sender, EventArgs e)
        {
            grpbItemList.Location = new Point(8, 338);
            if (this.Height == 551)
            {
                grpbItemList.Height = 203;
                dgvItemDetails.Height = 123;
            }
            else if (this.Height == 603)
            {
                dgvItemDetails.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
                dgvItemDetails.Height = 175;
                grpbItemList.Height = 255;
            }
            updateToolStripMenuItem.Enabled = true;
            pcbNormalView.Visible = false;
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItemDetails.SelectedRows.Count == 1)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        isUpdate = true;
                        while (dataReader.Read())
                        {
                            try
                            {
                                cmbItemCategory.Text = dataReader["itemCat"].ToString();
                                txtbTitel.Text = dataReader["itemTitle"].ToString();
                                txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                                txtbAccession.Text = dataReader["itemAccession"].ToString();
                                cmbSubCategory.Text = dataReader["itemSubCat"].ToString();
                                txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                                txtbCalssification.Text = dataReader["itemClassification"].ToString();
                                txtbSubject.Text = dataReader["itemSubject"].ToString();
                                txtbRack.Text = dataReader["rackNo"].ToString();
                                txtbPages.Text = dataReader["totalPages"].ToString();
                                txtbPrice.Text = dataReader["itemPrice"].ToString();
                                txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                                txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                                txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                                txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                                txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                                txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                                txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                                txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                                txtbAccession.Enabled = false;

                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBook.Image = Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBook.Image = Properties.Resources.NoImageAvailable;
                                }
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message);
                            }
                        }
                        btnSave.Text = "Update";
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
                    string queryString = "select * from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        isUpdate = true;
                        while (dataReader.Read())
                        {
                            try
                            {
                                cmbItemCategory.Text = dataReader["itemCat"].ToString();
                                txtbTitel.Text = dataReader["itemTitle"].ToString();
                                txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                                txtbAccession.Text = dataReader["itemAccession"].ToString();
                                cmbSubCategory.Text = dataReader["itemSubCat"].ToString();
                                txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                                txtbCalssification.Text = dataReader["itemClassification"].ToString();
                                txtbSubject.Text = dataReader["itemSubject"].ToString();
                                txtbRack.Text = dataReader["rackNo"].ToString();
                                txtbPages.Text = dataReader["totalPages"].ToString();
                                txtbPrice.Text = dataReader["itemPrice"].ToString();
                                txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                                txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                                txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                                txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                                txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                                txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                                txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                                txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                                txtbAccession.Enabled = false;

                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBook.Image = Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBook.Image = Properties.Resources.NoImageAvailable;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        btnSave.Text = "Update";
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItemDetails.Rows.Count > 0)
            {
                if (MessageBox.Show("Are you sure want to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }

                        string queryString = "delete from itemDetails where itemAccession=@itemAccn";
                        SQLiteCommand sqltCommnd = new SQLiteCommand(queryString, sqltConn);
                        string itemAccn = dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString();
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", itemAccn);
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
                        string queryString = "delete from item_details where itemAccession=@itemAccn";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        string itemAccn = dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString();
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", itemAccn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        mysqlConn.Close();
                    }
                    dgvItemDetails.Rows.RemoveAt(dgvItemDetails.SelectedRows[0].Index);
                    globalVarLms.backupRequired = true;
                    lblUserMessage.Text = "Item deleted successfully !";
                    showNotification();
                }
                dgvItemDetails.ClearSelection();
            }
        }

        public void clearData()
        {
            txtbTitel.Clear();
            txtbIsbn.Clear();
            txtbAccession.Clear();
            txtbAuthor.Clear();
            txtbCalssification.Clear();
            txtbSubject.Clear();
            txtbRack.Clear();
            txtbPages.Text = 0.ToString();
            txtbPrice.Text = 0.00.ToString("0.00");
            txtbAdditional1.Clear();
            txtbAdditional2.Clear();
            txtbAdditional3.Clear();
            txtbAdditional4.Clear();
            txtbAdditional5.Clear();
            txtbAdditional6.Clear();
            txtbAdditional7.Clear();
            txtbAdditional8.Clear();
            pcbBook.Image = Properties.Resources.NoImageAvailable;
            lblFileAdded.Text = "0 File Added";
        }

        private void txtbSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        public void UcItemNormalEntry_Load(object sender, EventArgs e)
        {
            clearData();
            pcbNormalView.Visible = false;
            if (globalVarLms.isAdmin)
            {
                btnAccnSetting.Enabled = true;
            }
            else
            {
                btnAccnSetting.Enabled = false;
            }
            loadFieldValue();
        }

        private void getAutoData()
        {
            txtbIsbn.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbIsbn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbIsbn.AutoCompleteCustomSource = autoCompIsbn;
            autoCompIsbn.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                sqltCommnd.CommandText = "select distinct itemIsbn from itemDetails";
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompIsbn.AddRange((from IDataRecord r in dataReader
                                           select (string)r["itemIsbn"]).ToArray());
                }
                dataReader.Close();

                txtbAuthor.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAuthor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAuthor.AutoCompleteCustomSource = autoCompAuthor;

                autoCompAuthor.Clear();
                string queryString = "";
                if (cmbSubCategory.SelectedIndex != -1 && cmbSubCategory.SelectedIndex != 0)
                {
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "select distinct itemAuthor from itemDetails";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCompAuthor.AddRange((from IDataRecord r in dataReader
                                                 select (string)r["itemAuthor"]).ToArray());
                    }
                    dataReader.Close();
                }

                txtbCalssification.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbCalssification.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbCalssification.AutoCompleteCustomSource = autoCompClassification;

                autoCompClassification.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct itemClassification from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompClassification.AddRange((from IDataRecord r in dataReader
                                                     select (string)r["itemClassification"]).ToArray());
                }
                dataReader.Close();

                txtbSubject.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbSubject.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbSubject.AutoCompleteCustomSource = autoCompSubject;

                autoCompSubject.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct itemSubject from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompSubject.AddRange((from IDataRecord r in dataReader
                                              select (string)r["itemSubject"]).ToArray());
                }
                dataReader.Close();

                txtbRack.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbRack.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbRack.AutoCompleteCustomSource = autoCompRack;

                autoCompRack.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct rackNo from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompRack.AddRange((from IDataRecord r in dataReader
                                           select (string)r["rackNo"]).ToArray());
                }
                dataReader.Close();

                txtbPages.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbPages.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbPages.AutoCompleteCustomSource = autoCompPages;

                autoCompPages.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct totalPages from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompPages.AddRange((from IDataRecord r in dataReader
                                            select (string)r["totalPages"]).ToArray());
                }
                dataReader.Close();

                txtbPrice.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbPrice.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbPrice.AutoCompleteCustomSource = autoCompPrices;

                autoCompPrices.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct itemPrice from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompPrices.AddRange((from IDataRecord r in dataReader
                                             select (string)r["itemPrice"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional1.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional1.AutoCompleteCustomSource = autoCompInfo1;

                autoCompInfo1.Clear();
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select distinct addInfo1 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo1.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo1"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional2.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional2.AutoCompleteCustomSource = autoCompInfo2;

                autoCompInfo2.Clear();
                queryString = "select distinct addInfo2 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo2.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo2"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional3.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional3.AutoCompleteCustomSource = autoCompInfo3;

                autoCompInfo3.Clear();
                queryString = "select distinct addInfo3 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo3.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo3"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional4.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional4.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional4.AutoCompleteCustomSource = autoCompInfo4;

                autoCompInfo4.Clear();
                queryString = "select distinct addInfo4 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo4.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo4"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional5.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional5.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional5.AutoCompleteCustomSource = autoCompInfo5;

                autoCompInfo5.Clear();
                queryString = "select distinct addInfo5 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo5.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo5"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional6.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional6.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional6.AutoCompleteCustomSource = autoCompInfo6;

                autoCompInfo6.Clear();
                queryString = "select distinct addInfo6 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo6.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo6"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional7.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional7.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional7.AutoCompleteCustomSource = autoCompInfo7;

                autoCompInfo7.Clear();
                queryString = "select distinct addInfo7 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo7.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo7"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional8.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional8.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional8.AutoCompleteCustomSource = autoCompInfo8;

                autoCompInfo8.Clear();
                queryString = "select distinct addInfo8 from itemDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo8.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo8"]).ToArray());
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

                string queryString = "select distinct itemIsbn from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompIsbn.AddRange((from IDataRecord r in dataReader
                                           select (string)r["itemIsbn"]).ToArray());
                }
                dataReader.Close();

                txtbAuthor.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAuthor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAuthor.AutoCompleteCustomSource = autoCompAuthor;

                autoCompAuthor.Clear();
                if (cmbSubCategory.SelectedIndex != -1 && cmbSubCategory.SelectedIndex != 0)
                {
                    queryString = "select distinct itemAuthor from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCompAuthor.AddRange((from IDataRecord r in dataReader
                                                 select (string)r["itemAuthor"]).ToArray());
                    }
                    dataReader.Close();
                }

                txtbCalssification.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbCalssification.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbCalssification.AutoCompleteCustomSource = autoCompClassification;

                autoCompClassification.Clear();
                queryString = "select distinct itemClassification from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompClassification.AddRange((from IDataRecord r in dataReader
                                                     select (string)r["itemClassification"]).ToArray());
                }
                dataReader.Close();

                txtbSubject.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbSubject.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbSubject.AutoCompleteCustomSource = autoCompSubject;

                autoCompSubject.Clear();
                queryString = "select distinct itemSubject from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompSubject.AddRange((from IDataRecord r in dataReader
                                              select (string)r["itemSubject"]).ToArray());
                }
                dataReader.Close();

                txtbRack.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbRack.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbRack.AutoCompleteCustomSource = autoCompRack;

                autoCompRack.Clear();
                queryString = "select distinct rackNo from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompRack.AddRange((from IDataRecord r in dataReader
                                           select (string)r["rackNo"]).ToArray());
                }
                dataReader.Close();

                txtbPages.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbPages.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbPages.AutoCompleteCustomSource = autoCompPages;

                autoCompPages.Clear();
                queryString = "select distinct totalPages from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompPages.AddRange((from IDataRecord r in dataReader
                                            select (string)r["totalPages"]).ToArray());
                }
                dataReader.Close();

                txtbPrice.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbPrice.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbPrice.AutoCompleteCustomSource = autoCompPrices;

                autoCompPrices.Clear();
                queryString = "select distinct itemPrice from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompPrices.AddRange((from IDataRecord r in dataReader
                                             select (string)r["itemPrice"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional1.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional1.AutoCompleteCustomSource = autoCompInfo1;

                autoCompInfo1.Clear();
                queryString = "select distinct addInfo1 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo1.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo1"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional2.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional2.AutoCompleteCustomSource = autoCompInfo2;

                autoCompInfo2.Clear();
                queryString = "select distinct addInfo2 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo2.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo2"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional3.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional3.AutoCompleteCustomSource = autoCompInfo3;

                autoCompInfo3.Clear();
                queryString = "select distinct addInfo3 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo3.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo3"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional4.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional4.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional4.AutoCompleteCustomSource = autoCompInfo4;

                autoCompInfo4.Clear();
                queryString = "select distinct addInfo4 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo4.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo4"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional5.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional5.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional5.AutoCompleteCustomSource = autoCompInfo5;

                autoCompInfo5.Clear();
                queryString = "select distinct addInfo5 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo5.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo5"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional6.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional6.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional6.AutoCompleteCustomSource = autoCompInfo6;

                autoCompInfo6.Clear();
                queryString = "select distinct addInfo6 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo6.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo6"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional7.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional7.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional7.AutoCompleteCustomSource = autoCompInfo7;

                autoCompInfo7.Clear();
                queryString = "select distinct addInfo7 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo7.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo7"]).ToArray());
                }
                dataReader.Close();

                txtbAdditional8.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbAdditional8.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbAdditional8.AutoCompleteCustomSource = autoCompInfo8;

                autoCompInfo8.Clear();
                queryString = "select distinct addInfo8 from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCompInfo8.AddRange((from IDataRecord r in dataReader
                                            select (string)r["addInfo8"]).ToArray());
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void cmbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdate)
            {
                if (cmbSubCategory.SelectedIndex != -1 && cmbSubCategory.SelectedIndex != 0)
                {
                    GenerateAccession();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        sqltCommnd.CommandText = "select distinct itemTitle from itemDetails";
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            autoCompTitle.Clear();
                            List<string> idList = (from IDataRecord r in dataReader
                                                   select (string)r["itemTitle"]).ToList();
                            autoCompTitle.AddRange(idList.ToArray());
                            txtbTitel.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbTitel.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbTitel.AutoCompleteCustomSource = autoCompTitle;
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
                        string queryString = "select distinct itemTitle from item_details";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            autoCompTitle.Clear();
                            List<string> idList = (from IDataRecord r in dataReader
                                                   select (string)r["itemTitle"]).ToList();
                            autoCompTitle.AddRange(idList.ToArray());
                            txtbTitel.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbTitel.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbTitel.AutoCompleteCustomSource = autoCompTitle;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                else
                {
                    txtbAccession.Clear();
                }
            }
        }

        private void GenerateAccession()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from accnSetting";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    bool isAutoGenerate = false, isManualPrefix = false, noPrefix = false; string prefixText = "", joiningChar = "";
                    while (dataReader.Read())
                    {
                        isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                        isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                        if (isAutoGenerate)
                        {
                            noPrefix = Convert.ToBoolean(dataReader["noPrefix"].ToString());
                        }
                        prefixText = dataReader["prefixText"].ToString();
                        joiningChar = dataReader["joiningChar"].ToString();
                    }
                    dataReader.Close();
                    if (isAutoGenerate)  //Auto Generate
                    {
                        txtbAccession.Enabled = false;
                        if (noPrefix)
                        {
                            string itemAccession = "";
                            sqltCommnd.CommandText = "select itemAccession from itemDetails order by [id] desc limit 1";    //  
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemAccession = dataReader["itemAccession"].ToString();
                                }
                                dataReader.Close();

                                sqltCommnd.CommandText = "select itemAccession from itemDetails";    // itemCat=@catName and itemSubCat=@subCatName and 
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    accnList = (from IDataRecord r in dataReader
                                                select (string)r["itemAccession"]).ToList();
                                }
                                dataReader.Close();

                                string numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                lastNumber = Convert.ToInt32(numericPortion);
                                lastNumber++;

                                itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                while (accnList.IndexOf(itemAccession) >= 0)
                                {
                                    lastNumber++;
                                    itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                }
                                txtbAccession.Text = itemAccession;
                            }
                            else
                            {
                                lastNumber = 1;
                                txtbAccession.Text = lastNumber.ToString("00000");
                            }
                            dataReader.Close();
                        }
                        else
                        {
                            if (!isManualPrefix) //Manual prefix
                            {
                                queryString = "select shortName from itemSubCategory where catName=@catName and subCatName=@subCatName";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                sqltCommnd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        prefixText = dataReader["shortName"].ToString();
                                    }
                                }
                                else
                                {
                                    prefixText = "";
                                }
                                dataReader.Close();
                            }

                            string itemAccession = "";
                            sqltCommnd.CommandText = "select itemAccession from itemDetails where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession order by [id] desc limit 1";    //  
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                            sqltCommnd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemAccession = dataReader["itemAccession"].ToString();
                                }
                                dataReader.Close();

                                sqltCommnd.CommandText = "select itemAccession from itemDetails where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession";    // itemCat=@catName and itemSubCat=@subCatName and 
                                sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                sqltCommnd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                sqltCommnd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    accnList = (from IDataRecord r in dataReader
                                                select (string)r["itemAccession"]).ToList();
                                }
                                dataReader.Close();

                                string numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                lastNumber = Convert.ToInt32(numericPortion);
                                lastNumber++;

                                itemAccession = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                while (accnList.IndexOf(itemAccession) >= 0)
                                {
                                    lastNumber++;
                                    itemAccession = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                }
                                txtbAccession.Text = itemAccession;
                            }
                            else
                            {
                                lastNumber = 1;
                                txtbAccession.Text = prefixText + joiningChar + lastNumber.ToString("00000");
                            }
                            dataReader.Close();
                        }
                        txtbIsbn.Select();
                    }
                    else
                    {
                        txtbAccession.Enabled = true;
                        txtbAccession.Select();
                        lastNumber = 0;
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
                    string queryString = "select * from accn_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        bool isAutoGenerate = false, isManualPrefix = false, noPrefix = false; string prefixText = "", joiningChar = "";
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                            if (isAutoGenerate)
                            {
                                noPrefix = Convert.ToBoolean(dataReader["noPrefix"].ToString());
                            }
                            prefixText = dataReader["prefixText"].ToString();
                            joiningChar = dataReader["joiningChar"].ToString();
                        }
                        dataReader.Close();
                        if (isAutoGenerate)
                        {
                            txtbAccession.Enabled = false;
                            if (noPrefix)
                            {
                                string itemAccession = "";
                                queryString = "select itemAccession from item_details order by id desc limit 1";    //  
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        itemAccession = dataReader["itemAccession"].ToString();
                                    }
                                    dataReader.Close();

                                    queryString = "select itemAccession from item_details";    // itemCat=@catName and itemSubCat=@subCatName and 
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        accnList = (from IDataRecord r in dataReader
                                                    select (string)r["itemAccession"]).ToList();
                                    }
                                    dataReader.Close();

                                    string numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                    lastNumber = Convert.ToInt32(numericPortion);
                                    lastNumber++;

                                    itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                    while (accnList.IndexOf(itemAccession) >= 0)
                                    {
                                        lastNumber++;
                                        itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                    }
                                    txtbAccession.Text = itemAccession;
                                }
                                else
                                {
                                    lastNumber = 1;
                                    txtbAccession.Text = lastNumber.ToString("00000");
                                }
                                dataReader.Close();
                            }
                            else
                            {
                                if (!isManualPrefix) //Manual prefix
                                {
                                    queryString = "select shortName from item_subcategory where catName=@catName and subCatName=@subCatName";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            prefixText = dataReader["shortName"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        prefixText = "";
                                    }
                                    dataReader.Close();
                                }

                                string itemAccession = "";
                                queryString = "select itemAccession from item_details where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession order by id desc limit 1";    //  
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        itemAccession = dataReader["itemAccession"].ToString();
                                    }
                                    dataReader.Close();

                                    queryString = "select itemAccession from item_details where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession";    // itemCat=@catName and itemSubCat=@subCatName and 
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        accnList = (from IDataRecord r in dataReader
                                                    select (string)r["itemAccession"]).ToList();
                                    }
                                    dataReader.Close();

                                    string numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                    lastNumber = Convert.ToInt32(numericPortion);
                                    lastNumber++;

                                    itemAccession = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                    while (accnList.IndexOf(itemAccession) >= 0)
                                    {
                                        lastNumber++;
                                        itemAccession = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                    }
                                    txtbAccession.Text = itemAccession;
                                }
                                else
                                {
                                    lastNumber = 1;
                                    txtbAccession.Text = prefixText + joiningChar + lastNumber.ToString("00000");
                                }
                                dataReader.Close();
                            }
                            txtbIsbn.Select();
                        }
                        else
                        {
                            txtbAccession.Enabled = true;
                            txtbAccession.Select();
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

        private void txtbAccession_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '/' && e.KeyChar != '\\' && e.KeyChar != '-' &&
                e.KeyChar != '_' && e.KeyChar != '.' && e.KeyChar != '~' && !char.IsDigit(e.KeyChar) &&
                !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("\"" + e.KeyChar.ToString() + "\" is not allowed." + Environment.NewLine +
                    "Only { /, \\, -, _, ~, . } and space are allowed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                try
                {
                    if (!char.IsControl(e.KeyChar))
                    {
                        if (!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar))
                        {
                            if (txtbAccession.Text.Count(f => f == '/') == 1 ||
                            txtbAccession.Text.Count(f => f == '\\') == 1 || txtbAccession.Text.Count(f => f == '-') == 1 ||
                            txtbAccession.Text.Count(f => f == '_') == 1 || txtbAccession.Text.Count(f => f == ' ') == 1)
                            {
                                e.Handled = true;
                            }
                        }
                        if (char.IsLetter(e.KeyChar))
                        {
                            if (txtbAccession.Text.Count(char.IsLetter) > 2)
                            {
                                e.Handled = true;
                                MessageBox.Show("You can't use more than three alphabet." + Environment.NewLine + "Ex- ACN-000001", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }

                        if (txtbAccession.Text.Length > 12)
                        {
                            e.Handled = true;
                            MessageBox.Show("You can't use more than thirteen alphanumeric value." + Environment.NewLine + "Ex- ACN-000001", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.NoImageAvailable;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbBook.Image.Dispose();
            pcbBook.Image = globalVarLms.bimapImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void dgvItemDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvItemDetails.HitTest(e.X, e.Y);
                dgvItemDetails.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvItemDetails.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvItemDetails, new Point(e.X, e.Y));
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            isUpdate = false;
            clearData();
            btnSave.Text = "Sa&ve";
            cmbItemCategory.SelectedIndex = 0;
            if (cmbSubCategory.Items.Count > 0)
            {
                cmbSubCategory.SelectedIndex = 0;
            }
            txtbAccession.Clear();
            dgvItemDetails.ClearSelection();
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
                    string queryString = "select * from itemDetails where itemIsbn=@itemIsbn limit 1";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbTitel.Text = dataReader["itemTitle"].ToString();
                            txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                            txtbCalssification.Text = dataReader["itemClassification"].ToString();
                            txtbSubject.Text = dataReader["itemSubject"].ToString();
                            txtbRack.Text = dataReader["rackNo"].ToString();
                            txtbPages.Text = dataReader["totalPages"].ToString();
                            txtbPrice.Text = dataReader["itemPrice"].ToString();
                            txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                            txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                            txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                            txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                            txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                            txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                            txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                            txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbBook.Image = Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbBook.Image = Properties.Resources.NoImageAvailable;
                            }
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        txtbTitel.Clear();
                        txtbAuthor.Clear();
                        txtbCalssification.Clear();
                        txtbSubject.Clear();
                        txtbRack.Clear();
                        txtbPages.Text = 0.ToString();
                        txtbPrice.Text = 0.00.ToString("0.00");
                        txtbAdditional1.Clear();
                        txtbAdditional2.Clear();
                        txtbAdditional3.Clear();
                        txtbAdditional4.Clear();
                        txtbAdditional5.Clear();
                        txtbAdditional6.Clear();
                        txtbAdditional7.Clear();
                        txtbAdditional8.Clear();
                        pcbBook.Image = Properties.Resources.NoImageAvailable;
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
                    string queryString = "select * from item_details where itemIsbn=@itemIsbn limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", txtbIsbn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbTitel.Text = dataReader["itemTitle"].ToString();
                            txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                            txtbCalssification.Text = dataReader["itemClassification"].ToString();
                            txtbSubject.Text = dataReader["itemSubject"].ToString();
                            txtbRack.Text = dataReader["rackNo"].ToString();
                            txtbPages.Text = dataReader["totalPages"].ToString();
                            txtbPrice.Text = dataReader["itemPrice"].ToString();
                            txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                            txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                            txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                            txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                            txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                            txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                            txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                            txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbBook.Image = Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbBook.Image = Properties.Resources.NoImageAvailable;
                            }
                            Application.DoEvents();
                        }
                    }
                    else
                    {
                        txtbTitel.Clear();
                        txtbAuthor.Clear();
                        txtbCalssification.Clear();
                        txtbSubject.Clear();
                        txtbRack.Clear();
                        txtbPages.Text = 0.ToString();
                        txtbPrice.Text = 0.00.ToString("0.00");
                        txtbAdditional1.Clear();
                        txtbAdditional2.Clear();
                        txtbAdditional3.Clear();
                        txtbAdditional4.Clear();
                        txtbAdditional5.Clear();
                        txtbAdditional6.Clear();
                        txtbAdditional7.Clear();
                        txtbAdditional8.Clear();
                        pcbBook.Image = Properties.Resources.NoImageAvailable;
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }

        private void dgvItemDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTotal.Text = "Total : " + dgvItemDetails.Rows.Count.ToString();
        }

        private void dgvItemDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTotal.Text = "Total : " + dgvItemDetails.Rows.Count.ToString();
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

        private void lnkLblDigitalReference_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtbAccession.Text == "")
            {
                MessageBox.Show("Please enter an Accession no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = Application.ProductName + " Select Digital File";
            selectFile.Multiselect = true;
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                List<string> referenceList = new List<string> { };
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select digitalReference from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    sqltCommnd.CommandType = CommandType.Text;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["digitalReference"].ToString() != "")
                            {
                                referenceList.Add(dataReader["digitalReference"].ToString());
                            }
                        }
                    }
                    dataReader.Close();
                    if (!Directory.Exists(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                    {
                        Directory.CreateDirectory(Properties.Settings.Default.databasePath + @"\Digital Reference");
                    }
                    foreach (string digitalFile in selectFile.FileNames)
                    {
                        File.Copy(digitalFile, Properties.Settings.Default.databasePath + @"\Digital Reference\" + "(" + StringExtention.replacrCharacter(txtbAccession.Text) + ")" + Path.GetFileName(digitalFile), true);
                        referenceList.Add("(" + StringExtention.replacrCharacter(txtbAccession.Text) + ")" + Path.GetFileName(digitalFile));
                    }
                    sqltCommnd.CommandText = "update itemDetails set digitalReference=:digitalReference where itemTitle=@itemTitle";
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    sqltCommnd.Parameters.AddWithValue("digitalReference", string.Join("?", referenceList));
                    sqltCommnd.CommandType = CommandType.Text;
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
                    string queryString = "select digitalReference from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["digitalReference"].ToString() != "")
                            {
                                referenceList.Add(dataReader["digitalReference"].ToString());
                            }
                        }
                    }
                    dataReader.Close();
                    if (!Directory.Exists(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                    {
                        Directory.CreateDirectory(Properties.Settings.Default.databasePath + @"\Digital Reference");
                    }
                    foreach (string digitalFile in selectFile.FileNames)
                    {
                        File.Copy(digitalFile, Properties.Settings.Default.databasePath + @"\Digital Reference\" + "(" + StringExtention.replacrCharacter(txtbAccession.Text) + ")" + Path.GetFileName(digitalFile), true);
                        referenceList.Add("(" + StringExtention.replacrCharacter(txtbAccession.Text) + ")" + Path.GetFileName(digitalFile));
                    }
                    queryString = "update item_details set digitalReference=@digitalReference where itemTitle=@itemTitle";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    mysqlCmd.Parameters.AddWithValue("@digitalReference", string.Join("?", referenceList));
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                lblFileAdded.Text = selectFile.FileNames.Count().ToString() + " File Added";
            }
        }

        private void lnkLblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtbAccession.Text == "")
            {
                MessageBox.Show("Please enter an Accession no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Directory.Exists(Properties.Settings.Default.databasePath + @"\Digital Reference"))
            {
                foreach (string digitalFile in Directory.GetFiles(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                {
                    if (digitalFile.Contains("(" + StringExtention.replacrCharacter(txtbAccession.Text) + ")"))
                    {
                        try
                        {
                            File.Delete(digitalFile);
                        }
                        catch
                        {

                        }
                    }
                }
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "update itemDetails set digitalReference='' where itemTitle=@itemTitle";
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    sqltCommnd.CommandType = CommandType.Text;
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
                    string queryString = "update item_details set digitalReference='' where itemTitle=@itemTitle";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                MessageBox.Show("Reference deleted successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtbTitel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtbTitel.Text != "")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select * from itemDetails where itemTitle=@itemTitle";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                                txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                                txtbCalssification.Text = dataReader["itemClassification"].ToString();
                                txtbSubject.Text = dataReader["itemSubject"].ToString();
                                txtbRack.Text = dataReader["rackNo"].ToString();
                                txtbPages.Text = dataReader["totalPages"].ToString();
                                txtbPrice.Text = dataReader["itemPrice"].ToString();//Convert.ToDouble(dataReader["itemPrice"].ToString()).ToString("0.00");
                                txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                                txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                                txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                                txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                                txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                                txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                                txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                                txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBook.Image = Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBook.Image = Properties.Resources.NoImageAvailable;
                                }
                            }
                        }
                        else
                        {
                            txtbIsbn.Clear();
                            txtbAuthor.Clear();
                            txtbCalssification.Clear();
                            txtbSubject.Clear();
                            txtbRack.Clear();
                            txtbPages.Text = 0.ToString();
                            txtbPrice.Text = 0.00.ToString("0.00");
                            txtbAdditional1.Clear();
                            txtbAdditional2.Clear();
                            txtbAdditional3.Clear();
                            txtbAdditional4.Clear();
                            txtbAdditional5.Clear();
                            txtbAdditional6.Clear();
                            txtbAdditional7.Clear();
                            txtbAdditional8.Clear();
                            pcbBook.Image = Properties.Resources.NoImageAvailable;
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
                        string queryString = "select * from item_details where itemTitle=@itemTitle";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbIsbn.Text = dataReader["itemIsbn"].ToString();
                                txtbAuthor.Text = dataReader["itemAuthor"].ToString();
                                txtbCalssification.Text = dataReader["itemClassification"].ToString();
                                txtbSubject.Text = dataReader["itemSubject"].ToString();
                                txtbRack.Text = dataReader["rackNo"].ToString();
                                txtbPages.Text = dataReader["totalPages"].ToString();
                                txtbPrice.Text = dataReader["itemPrice"].ToString();//Convert.ToDouble(dataReader["itemPrice"].ToString()).ToString("0.00");
                                txtbAdditional1.Text = dataReader["addInfo1"].ToString();
                                txtbAdditional2.Text = dataReader["addInfo2"].ToString();
                                txtbAdditional3.Text = dataReader["addInfo3"].ToString();
                                txtbAdditional4.Text = dataReader["addInfo4"].ToString();
                                txtbAdditional5.Text = dataReader["addInfo5"].ToString();
                                txtbAdditional6.Text = dataReader["addInfo6"].ToString();
                                txtbAdditional7.Text = dataReader["addInfo7"].ToString();
                                txtbAdditional8.Text = dataReader["addInfo8"].ToString();
                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBook.Image = Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBook.Image = Properties.Resources.NoImageAvailable;
                                }
                            }
                        }
                        else
                        {
                            txtbIsbn.Clear();
                            txtbAuthor.Clear();
                            txtbCalssification.Clear();
                            txtbSubject.Clear();
                            txtbRack.Clear();
                            txtbPages.Text = 0.ToString();
                            txtbPrice.Text = 0.00.ToString("0.00");
                            txtbAdditional1.Clear();
                            txtbAdditional2.Clear();
                            txtbAdditional3.Clear();
                            txtbAdditional4.Clear();
                            txtbAdditional5.Clear();
                            txtbAdditional6.Clear();
                            txtbAdditional7.Clear();
                            txtbAdditional8.Clear();
                            pcbBook.Image = Properties.Resources.NoImageAvailable;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
        }

        private void makeCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItemDetails.Rows.Count > 0)
            {
                globalVarLms.ItemDetails.Clear();
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }

                    string queryString = "select * from itemDetails where itemAccession=@itemAccn";
                    SQLiteCommand sqltCommnd = new SQLiteCommand(queryString, sqltConn);
                    string itemAccn = dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString();
                    sqltCommnd.Parameters.AddWithValue("@itemAccn", itemAccn);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            globalVarLms.ItemDetails.Add(dataReader["itemTitle"].ToString());           //0
                            globalVarLms.ItemDetails.Add(dataReader["itemIsbn"].ToString());            //1
                            globalVarLms.ItemDetails.Add(dataReader["itemAccession"].ToString());       //2
                            globalVarLms.ItemDetails.Add(dataReader["itemCat"].ToString());             //3
                            globalVarLms.ItemDetails.Add(dataReader["itemSubCat"].ToString());          //4
                            globalVarLms.ItemDetails.Add(dataReader["itemAuthor"].ToString());          //5
                            globalVarLms.ItemDetails.Add(dataReader["itemClassification"].ToString());  //6
                            globalVarLms.ItemDetails.Add(dataReader["itemSubject"].ToString());         //7
                            globalVarLms.ItemDetails.Add(dataReader["rackNo"].ToString());              //8
                            globalVarLms.ItemDetails.Add(dataReader["totalPages"].ToString());          //9
                            globalVarLms.ItemDetails.Add(dataReader["itemPrice"].ToString());           //10
                            globalVarLms.ItemDetails.Add(dataReader["addInfo1"].ToString());            //11
                            globalVarLms.ItemDetails.Add(dataReader["addInfo2"].ToString());            //12
                            globalVarLms.ItemDetails.Add(dataReader["addInfo3"].ToString());            //13
                            globalVarLms.ItemDetails.Add(dataReader["addInfo4"].ToString());            //14
                            globalVarLms.ItemDetails.Add(dataReader["addInfo5"].ToString());            //15
                            globalVarLms.ItemDetails.Add(dataReader["addInfo6"].ToString());            //16
                            globalVarLms.ItemDetails.Add(dataReader["addInfo7"].ToString());            //17
                            globalVarLms.ItemDetails.Add(dataReader["addInfo8"].ToString());            //18
                            globalVarLms.ItemDetails.Add(dataReader["itemImage"].ToString());           //19
                            globalVarLms.ItemDetails.Add(dataReader["digitalReference"].ToString());    //20
                        }
                    }
                    dataReader.Close();
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
                    string queryString = "select * from item_details where itemAccession=@itemAccn";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    string itemAccn = dgvItemDetails.SelectedRows[0].Cells[0].Value.ToString();
                    mysqlCmd.Parameters.AddWithValue("@itemAccn", itemAccn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            globalVarLms.ItemDetails.Add(dataReader["itemTitle"].ToString());           //0
                            globalVarLms.ItemDetails.Add(dataReader["itemIsbn"].ToString());            //1
                            globalVarLms.ItemDetails.Add(dataReader["itemAccession"].ToString());       //2
                            globalVarLms.ItemDetails.Add(dataReader["itemCat"].ToString());             //3
                            globalVarLms.ItemDetails.Add(dataReader["itemSubCat"].ToString());          //4
                            globalVarLms.ItemDetails.Add(dataReader["itemAuthor"].ToString());          //5
                            globalVarLms.ItemDetails.Add(dataReader["itemClassification"].ToString());  //6
                            globalVarLms.ItemDetails.Add(dataReader["itemSubject"].ToString());         //7
                            globalVarLms.ItemDetails.Add(dataReader["rackNo"].ToString());              //8
                            globalVarLms.ItemDetails.Add(dataReader["totalPages"].ToString());          //9
                            globalVarLms.ItemDetails.Add(dataReader["itemPrice"].ToString());           //10
                            globalVarLms.ItemDetails.Add(dataReader["addInfo1"].ToString());            //11
                            globalVarLms.ItemDetails.Add(dataReader["addInfo2"].ToString());            //12
                            globalVarLms.ItemDetails.Add(dataReader["addInfo3"].ToString());            //13
                            globalVarLms.ItemDetails.Add(dataReader["addInfo4"].ToString());            //14
                            globalVarLms.ItemDetails.Add(dataReader["addInfo5"].ToString());            //15
                            globalVarLms.ItemDetails.Add(dataReader["addInfo6"].ToString());            //16
                            globalVarLms.ItemDetails.Add(dataReader["addInfo7"].ToString());            //17
                            globalVarLms.ItemDetails.Add(dataReader["addInfo8"].ToString());            //18
                            globalVarLms.ItemDetails.Add(dataReader["itemImage"].ToString());           //19
                            globalVarLms.ItemDetails.Add(dataReader["digitalReference"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }

                if (globalVarLms.ItemDetails.Count > 0)
                {
                    List<string> itemAccnList = new List<string> { };
                    int lastNumberofAccn = 0;
                    FormMultipleEntry multipleEntry = new FormMultipleEntry();
                    multipleEntry.txtbTitel.Text = globalVarLms.ItemDetails[0];
                    multipleEntry.txtbAuthor.Text = globalVarLms.ItemDetails[5];
                    multipleEntry.txtbRack.Text = globalVarLms.ItemDetails[8];

                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }

                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand(); ;
                        sqltCommnd.CommandText = "select itemAccession from itemDetails";
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            itemAccnList = (from IDataRecord r in dataReader
                                            select (string)r["itemAccession"]).ToList();
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
                            string queryString = "select itemAccession from item_details";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    itemAccnList = (from IDataRecord r in dataReader
                                                    select (string)r["itemAccession"]).ToList();
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                        catch
                        {

                        }
                    }
                    string itemAccession = globalVarLms.ItemDetails[2];
                    string numericPortion = new String(globalVarLms.ItemDetails[2].Where(Char.IsDigit).ToArray());
                    lastNumberofAccn = Convert.ToInt32(numericPortion);
                    lastNumberofAccn++;

                    itemAccession = globalVarLms.ItemDetails[2].Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                    while (itemAccnList.IndexOf(itemAccession) >= 0)
                    {
                        lastNumberofAccn++;
                        itemAccession = globalVarLms.ItemDetails[2].Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                    }

                    multipleEntry.txtbAccession.Text = itemAccession;
                    multipleEntry.ShowDialog();
                }
            }
            dgvItemDetails.ClearSelection();
        }

        public string formatAccession(string numercPortion, string lastNumberofAccn)
        {
            while (lastNumberofAccn.Count() < numercPortion.Count())
            {
                lastNumberofAccn = "0" + lastNumberofAccn;
            }
            return lastNumberofAccn;
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

        private void makeCopyToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            makeCopyToolStripMenuItem.BackColor = Color.WhiteSmoke;
            makeCopyToolStripMenuItem.ForeColor = Color.Black;
        }

        private void makeCopyToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            makeCopyToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            makeCopyToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    getAutoData();
                }));
            }
            catch
            {

            }
        }

        private void dgvItemDetails_SelectionChanged(object sender, EventArgs e)
        {
            isUpdate = false;
            clearData();
            btnSave.Text = "Sa&ve";
            cmbItemCategory.SelectedIndex = 0;
            if (cmbSubCategory.Items.Count > 0)
            {
                cmbSubCategory.SelectedIndex = 0;
            }
            txtbAccession.Clear();
        }

        private void txtbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtbSearch.Text != "")
                {
                    dgvItemDetails.Rows.Clear();
                    string queryString = "";
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        if (cmbSearchBy.SelectedIndex == 2)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemTitle]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 3)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemAccession]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 4)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemIsbn]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 5)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemAuthor]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 6)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemCat]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 7)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [rackNo]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 8)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemSubject]=  @paraName collate nocase";
                        }
                        else if (cmbSearchBy.SelectedIndex == 9)
                        {
                            string subCatName = txtbSearch.Text;//.Replace(catName + "(", "").Replace(")", "");
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [itemSubCat] = @paraName collate nocase";
                        }
                        else if (cmbSearchBy.Text == "Entry Date")
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from itemDetails where [entryDate]=  @paraName collate nocase";
                        }

                        if (queryString != "")
                        {
                            contextMenuStrip1.Enabled = false;
                            sqltCommnd.CommandText = queryString;
                            if (cmbSearchBy.Text == "Entry Date")
                            {
                                sqltCommnd.Parameters.AddWithValue("@paraName", FormatDate.getAppFormat(txtbSearch.Text));
                            }
                            else
                            {
                                sqltCommnd.Parameters.AddWithValue("@paraName", txtbSearch.Text);
                            }
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    contextMenuStrip1.Visible = false;
                                    if (Convert.ToBoolean(dataReader["itemAvailable"].ToString()) == true)
                                    {
                                        if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                              dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                                            dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                        else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                              dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                                            dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                        else
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                               dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                                        }
                                    }
                                    else
                                    {
                                        dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                            dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                                        dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                                    }
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
                        if (cmbSearchBy.SelectedIndex == 2)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemTitle)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 3)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemAccession)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 4)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemIsbn)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 5)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemAuthor)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 6)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemCat)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 7)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(rackNo)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 8)
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemSubject)=  @paraName";
                        }
                        else if (cmbSearchBy.SelectedIndex == 9)
                        {
                            //string catName = txtbSearch.Text.Substring(0, txtbSearch.Text.IndexOf("("));
                            string subCatName = txtbSearch.Text;//.Replace(catName + "(", "").Replace(")", "");
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where lower(itemSubCat) = @paraName";
                        }
                        else if (cmbSearchBy.Text == "Entry Date")
                        {
                            queryString = "select itemAccession,itemTitle,itemSubject,itemAuthor,rackNo,itemAvailable,isLost,isDamage from item_details where entryDate=  @paraName";
                        }
                        if (queryString != "")
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

                            contextMenuStrip1.Enabled = false;
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            if (cmbSearchBy.Text == "Entry Date")
                            {
                                mysqlCmd.Parameters.AddWithValue("@paraName", FormatDate.getAppFormat(txtbSearch.Text));
                            }
                            else
                            {
                                mysqlCmd.Parameters.AddWithValue("@paraName", txtbSearch.Text.ToLower());
                            }
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    contextMenuStrip1.Visible = false;
                                    if (Convert.ToBoolean(dataReader["itemAvailable"].ToString()) == true)
                                    {
                                        if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                              dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Lost");
                                            dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                        else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                              dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Damage");
                                            dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                        else
                                        {
                                            dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                               dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Available");
                                        }
                                    }
                                    else
                                    {
                                        dgvItemDetails.Rows.Add(dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(),
                                            dataReader["itemSubject"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["rackNo"].ToString(), "Issued");
                                        dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            contextMenuStrip1.Enabled = true;
                            mysqlConn.Close();
                        }
                    }
                    dgvItemDetails.ClearSelection();
                }
            }
        }

        private void lblCategory_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(cmbItemCategory.Location.X, cmbItemCategory.Location.Y);
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

        private void btnChange_Click(object sender, EventArgs e)
        {
            Control[] cntrolCollection = this.Controls.Find(controlName, true);
            foreach (Control cntrl in cntrolCollection)
            {
                cntrl.Text = txtbFieldName.Text;
                if (FieldSettings.Default.itemEntry != "")
                {
                    if (FieldSettings.Default.itemEntry.Contains(cntrl.Name + "="))
                    {
                        string prevText = FieldSettings.Default.itemEntry.Substring(FieldSettings.Default.itemEntry.IndexOf(cntrl.Name + "=") + (cntrl.Name + "=").Count());
                        prevText = prevText.Substring(0, prevText.IndexOf("|") + 1);
                        FieldSettings.Default.itemEntry = FieldSettings.Default.itemEntry.Replace(cntrl.Name + "=" + prevText, cntrl.Name + "=" + cntrl.Text + "|");
                    }
                    else
                    {
                        FieldSettings.Default.itemEntry = FieldSettings.Default.itemEntry + cntrl.Name + "=" + cntrl.Text + "|";
                    }
                }
                else
                {
                    FieldSettings.Default.itemEntry = cntrl.Name + "=" + cntrl.Text + "|";
                }
                if (cntrl.Name == lblAccession.Name)
                {
                    cmbSearchBy.Items[3] = cntrl.Text;
                    accessionLabel = cntrl.Text;
                    dgvItemDetails.Columns[0].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblTitle.Name)
                {
                    cmbSearchBy.Items[2] = cntrl.Text;
                    dgvItemDetails.Columns[1].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblAuthor.Name)
                {
                    cmbSearchBy.Items[5] = cntrl.Text;
                    dgvItemDetails.Columns[3].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblSubject.Name)
                {
                    cmbSearchBy.Items[8] = cntrl.Text;
                    dgvItemDetails.Columns[2].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblLocation.Name)
                {
                    cmbSearchBy.Items[7] = cntrl.Text;
                    dgvItemDetails.Columns[4].HeaderText = cntrl.Text;
                }
                else if (cntrl.Name == lblCategory.Name)
                {
                    categoryLabel = cntrl.Text;
                    cmbSearchBy.Items[6] = cntrl.Text;
                    if (cmbItemCategory.Items.Count > 0)
                    {
                        cmbItemCategory.Items[0] = "Please select a " + cntrl.Text + "...";
                    }
                }
                else if (cntrl.Name == lblSubCategory.Name)
                {
                    cmbSearchBy.Items[9] = cntrl.Text;
                    subcategoryLabel = cntrl.Text;
                }
                else if (cntrl.Name == lblIsbn.Name)
                {
                    cmbSearchBy.Items[4] = cntrl.Text;
                }
                if (cntrl.Name != lblAddItem.Name && cntrl.Name != lblSearchOption.Name)
                {
                    cntrl.Text = cntrl.Text + " :";
                }
            }
            FieldSettings.Default.Save();
            panelField.Visible = false;
        }

        private void lblSubCategory_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(cmbSubCategory.Location.X, cmbSubCategory.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblAccession_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbAccession.Location.X, txtbAccession.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblIsbn_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbIsbn.Location.X, txtbIsbn.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbTitel.Location.X, 227);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblAuthor_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbAuthor.Location.X, 227);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
            panelField.BringToFront();
        }

        private void lblClassification_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbCalssification.Location.X, txtbCalssification.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblSubject_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbSubject.Location.X, txtbSubject.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblLocation_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbRack.Location.X, txtbRack.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblPages_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbPages.Location.X, txtbPages.Location.Y);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblPrice_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(txtbPrice.Location.X, 227);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblSearchBy_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(lblSearchBy.Location.X, 227);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
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
                        Control[] cntrolCollection = this.Controls.Find(fieldName, true);
                        foreach (Control cntrl in cntrolCollection)
                        {
                            if (cntrl.Name != lblAddItem.Name && cntrl.Name != lblSearchOption.Name)
                            {
                                cntrl.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                            else
                            {
                                cntrl.Text = fieldValue.Replace(fieldName + "=", "");
                            }

                            if (cntrl.Name == lblAccession.Name)
                            {
                                cmbSearchBy.Items[3] = cntrl.Text.Replace(" :", "");
                                accessionLabel = cntrl.Text.Replace(" :", "");
                                dgvItemDetails.Columns[0].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblTitle.Name)
                            {
                                cmbSearchBy.Items[2] = cntrl.Text.Replace(" :", "");
                                dgvItemDetails.Columns[1].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblAuthor.Name)
                            {
                                cmbSearchBy.Items[5] = cntrl.Text.Replace(" :", "");
                                dgvItemDetails.Columns[3].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblSubject.Name)
                            {
                                cmbSearchBy.Items[8] = cntrl.Text.Replace(" :", "");
                                dgvItemDetails.Columns[2].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblLocation.Name)
                            {
                                cmbSearchBy.Items[7] = cntrl.Text.Replace(" :", "");
                                dgvItemDetails.Columns[4].HeaderText = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblCategory.Name)
                            {
                                cmbSearchBy.Items[6] = cntrl.Text.Replace(" :", "");
                                categoryLabel = cntrl.Text.Replace(" :", "");
                                if (cmbItemCategory.Items.Count > 0)
                                {
                                    cmbItemCategory.Items[0] = "Please select a " + cntrl.Text.Replace(" :", "") + "...";
                                }
                            }
                            else if (cntrl.Name == lblSubCategory.Name)
                            {
                                cmbSearchBy.Items[9] = cntrl.Text.Replace(" :", "");
                                subcategoryLabel = cntrl.Text.Replace(" :", "");
                            }
                            else if (cntrl.Name == lblIsbn.Name)
                            {
                                cmbSearchBy.Items[4] = cntrl.Text.Replace(" :", "");
                            }
                        }
                    }
                }
            }
        }

        private void lblSearchOption_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(lblSearchBy.Location.X, 227);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
        }

        private void lblAddItem_Click(object sender, EventArgs e)
        {
            Label clickedControl = (Label)sender;
            panelField.Location = new Point(lblCategory.Location.X, lblCategory.Location.X);
            controlName = clickedControl.Name;
            txtbFieldName.Text = clickedControl.Text.Replace(" :", "");
            txtbFieldName.Focus();
            txtbFieldName.SelectAll();
            panelField.Visible = true;
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
            if (categoryLabel == "")
            {
                categoryLabel = "Category";
            }
            btnToolTip.Show("Add " + categoryLabel, btnAddCategory);
        }

        private void btnAddCategory_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnAddCategory);
        }

        private void btnAccnSetting_MouseEnter(object sender, EventArgs e)
        {
            if (accessionLabel == "")
            {
                accessionLabel = "Accession No. ";
            }
            btnToolTip.Show(accessionLabel + " Setting", btnAccnSetting);
        }

        private void btnAccnSetting_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnAccnSetting);
        }

        private void btnTutorial_MouseEnter(object sender, EventArgs e)
        {
            btnToolTip.Show("See Tutorial", btnTutorial);
        }

        private void btnTutorial_MouseLeave(object sender, EventArgs e)
        {
            btnToolTip.Hide(btnTutorial);
        }

        private void btnIsbnSearch_Click(object sender, EventArgs e)
        {
            if (cmbItemCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbSubCategory.SelectedIndex == -1 || cmbSubCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtbIsbn.Text == "")
            {
                MessageBox.Show("Please enter a ISBN/ISSN no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbIsbn.Select();
                return;
            }
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    WebRequest webRequest = WebRequest.Create("https://www.googleapis.com/books/v1/volumes?q=isbn:" + txtbIsbn.Text.TrimEnd());
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadToEnd();

                    if (requestResult.Contains("\"title\": \""))
                    {
                        JObject jObject = JObject.Parse(requestResult);
                        txtbTitel.Text = jObject["items"][0]["volumeInfo"]["title"].ToString();
                        txtbAuthor.Text = jObject["items"][0]["volumeInfo"]["authors"][0].ToString();
                        txtbPages.Text = jObject["items"][0]["volumeInfo"]["pageCount"].ToString();

                        if (requestResult.Contains("\"smallThumbnail\": \""))
                        {
                            string imageLink = jObject["items"][0]["volumeInfo"]["imageLinks"]["smallThumbnail"].ToString();
                            WebClient webClient = new WebClient();
                            byte[] bytes = webClient.DownloadData(imageLink);
                            MemoryStream memoryStream = new MemoryStream(bytes);
                            pcbBook.Image = Image.FromStream(memoryStream);
                        }
                        //============Publisher Name==================
                        if (lblInfo1.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional1.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo2.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional2.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo3.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional3.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo4.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional4.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo5.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional5.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo6.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional6.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo7.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional7.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        else if (lblInfo8.Text.ToLower() == "publisher :")
                        {
                            txtbAdditional8.Text = jObject["items"][0]["volumeInfo"]["publisher"].ToString();
                        }
                        //============Publishing Date==================
                        if (lblInfo1.Text.ToLower() == "publishing date :" || lblInfo1.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional1.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo2.Text.ToLower() == "publishing date :" || lblInfo2.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional2.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo3.Text.ToLower() == "publishing date :" || lblInfo3.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional3.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo4.Text.ToLower() == "publishing date :" || lblInfo4.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional4.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo5.Text.ToLower() == "publishing date :" || lblInfo5.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional5.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo6.Text.ToLower() == "publishing date :" || lblInfo6.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional6.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo7.Text.ToLower() == "publishing date :" || lblInfo7.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional7.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                        else if (lblInfo8.Text.ToLower() == "publishing date :" || lblInfo8.Text.ToLower() == "publishing year :")
                        {
                            txtbAdditional8.Text = jObject["items"][0]["volumeInfo"]["publishedDate"].ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Cursor = Cursors.Default;
        }

        private void btnAccnSetting_Click(object sender, EventArgs e)
        {
            cmbItemCategory.SelectedIndex = 0;
            txtbAccession.Text = "";
            txtbAccession.Enabled = false;
            FormAccnSetting accnSetting = new FormAccnSetting();
            accnSetting.ShowDialog();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            globalVarLms.addCateory = "itemCategory";
        }
    }
    public static class StringExtention
    {
        public static string replacrCharacter(string itemAccession)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char c in itemAccession)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == ' ' || c == '-')
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append("_");
                }
            }
            return stringBuilder.ToString();
        }
    }


}
