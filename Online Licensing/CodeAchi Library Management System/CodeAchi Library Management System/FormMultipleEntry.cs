using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormMultipleEntry : Form
    {
        public FormMultipleEntry()
        {
            InitializeComponent();
        }

        string lastAccession = "";
        PasswordHasher passwordHasher = new PasswordHasher();
        string usageFilePath = Application.StartupPath + "/usage.json";

        private void FormMultipleEntry_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            int copyAdded = 0;
            if (globalVarLms.sqliteData)
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
                int ttlBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                if ((ttlBooks + numCopy.Value) >= globalVarLms.itemLimit)
                {
                    MessageBox.Show("You can't add more than " + globalVarLms.itemLimit.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (rdbAuto.Checked)
                {
                    if (numCopy.Value == 0)
                    {
                        MessageBox.Show("Please enter the number of copy.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        numCopy.Select();
                        return;
                    }
                    List<string> itemAccnList = new List<string> { };

                    int lastNumberofAccn = 0;
                    SQLiteDataReader dataReader;
                    sqltCommnd.CommandText = "select itemAccession from itemDetails";
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemAccnList = (from IDataRecord r in dataReader
                                        select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();
                    if (itemAccnList.IndexOf(txtbAccession.Text) >= 0)
                    {
                        MessageBox.Show("Accession No already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbAccession.SelectAll();
                        return;
                    }
                    string itemAccession = "", queryString = "";
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = Convert.ToInt32(numCopy.Value);
                    for (int copyCount = 1; copyCount <= numCopy.Value; copyCount++)
                    {
                        //=====================Insert=============
                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "INSERT INTO itemDetails (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                            "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                            "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                            "itemImage,digitalReference) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                            ",'" + txtbRack.Text + "','" + globalVarLms.ItemDetails[9] + "','" + globalVarLms.ItemDetails[10] + "'" +
                            ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                            ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + globalVarLms.ItemDetails[19] + "',@digitalReference);";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", globalVarLms.ItemDetails[1]);
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemCat", globalVarLms.ItemDetails[3]);
                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", globalVarLms.ItemDetails[4]);
                        sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemClassification", globalVarLms.ItemDetails[6]);
                        sqltCommnd.Parameters.AddWithValue("@itemSubject", globalVarLms.ItemDetails[7]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo1", globalVarLms.ItemDetails[11]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo2", globalVarLms.ItemDetails[12]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo3", globalVarLms.ItemDetails[13]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo4", globalVarLms.ItemDetails[14]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo5", globalVarLms.ItemDetails[15]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo6", globalVarLms.ItemDetails[16]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo7", globalVarLms.ItemDetails[17]);
                        sqltCommnd.Parameters.AddWithValue("@addInfo8", globalVarLms.ItemDetails[18]);
                        sqltCommnd.Parameters.AddWithValue("@digitalReference", globalVarLms.ItemDetails[20]);
                        sqltCommnd.ExecuteNonQuery();

                        progressBar1.Value = copyCount;
                        lblCount.Text = "Item Added - " + copyCount.ToString();
                        copyAdded = copyCount;
                        Application.DoEvents();
                        //===============generate itemaccession=====================
                        sqltCommnd.CommandText = "select itemAccession from itemDetails";
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            itemAccnList = (from IDataRecord r in dataReader
                                            select (string)r["itemAccession"]).ToList();
                        }
                        dataReader.Close();

                        itemAccession = txtbAccession.Text;

                        string numericPortion = new String(txtbAccession.Text.Where(Char.IsDigit).ToArray());
                        lastNumberofAccn = Convert.ToInt32(numericPortion);
                        lastNumberofAccn++;

                        itemAccession = txtbAccession.Text.Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                        while (itemAccnList.IndexOf(itemAccession) >= 0)
                        {
                            lastNumberofAccn++;
                            itemAccession = txtbAccession.Text.Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                        }
                        txtbAccession.Text = itemAccession;
                    }
                }
                else
                {
                    List<string> itemAccnList = new List<string> { };
                    SQLiteDataReader dataReader;
                    sqltCommnd.CommandText = "select itemAccession from itemDetails";
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemAccnList = (from IDataRecord r in dataReader
                                        select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();
                    string[] accnList = txtbAccession.Text.Split(',');
                    List<string> existData = new List<string> { };
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = accnList.Count();
                    string queryString = "";
                    int copyCount = 0;
                    foreach (string accnNumber in accnList)
                    {
                        if (itemAccnList.IndexOf(accnNumber) >= 0)
                        {
                            existData.Add("Accession No. : " + accnNumber + " [ not added due to duplicate accession no]");
                        }
                        else
                        {
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "INSERT INTO itemDetails (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                "itemImage,digitalReference) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                                ",'" + txtbRack.Text + "','" + globalVarLms.ItemDetails[9] + "','" + globalVarLms.ItemDetails[10] + "'" +
                                ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + globalVarLms.ItemDetails[19] + "',@digitalReference);";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemIsbn", globalVarLms.ItemDetails[1]);
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", accnNumber);
                            sqltCommnd.Parameters.AddWithValue("@itemCat", globalVarLms.ItemDetails[3]);
                            sqltCommnd.Parameters.AddWithValue("@itemSubCat", globalVarLms.ItemDetails[4]);
                            sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemClassification", globalVarLms.ItemDetails[6]);
                            sqltCommnd.Parameters.AddWithValue("@itemSubject", globalVarLms.ItemDetails[7]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo1", globalVarLms.ItemDetails[11]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo2", globalVarLms.ItemDetails[12]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo3", globalVarLms.ItemDetails[13]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo4", globalVarLms.ItemDetails[14]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo5", globalVarLms.ItemDetails[15]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo6", globalVarLms.ItemDetails[16]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo7", globalVarLms.ItemDetails[17]);
                            sqltCommnd.Parameters.AddWithValue("@addInfo8", globalVarLms.ItemDetails[18]);
                            sqltCommnd.Parameters.AddWithValue("@digitalReference", globalVarLms.ItemDetails[20]);
                            sqltCommnd.ExecuteNonQuery();
                            copyCount++;

                            progressBar1.Value = copyCount;
                            lblCount.Text = "Item Added - " + copyCount.ToString();
                            copyAdded = copyCount;
                            Application.DoEvents();
                            //===============generate itemaccession=====================
                            sqltCommnd.CommandText = "select itemAccession from itemDetails";
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                itemAccnList = (from IDataRecord r in dataReader
                                                select (string)r["itemAccession"]).ToList();
                            }
                            dataReader.Close();
                        }
                    }
                    if (existData.Count > 0)
                    {
                        File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for make item copy.txt", existData);
                        try
                        {
                            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for make item copy.txt");
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
                MySqlCommand mysqlCmd=null;
                string queryString = "select count(id) from item_details;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                int ttlBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                if ((ttlBooks + numCopy.Value) >= globalVarLms.itemLimit)
                {
                    MessageBox.Show("You can't add more than " + globalVarLms.itemLimit.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (rdbAuto.Checked)
                {
                    if (numCopy.Value == 0)
                    {
                        MessageBox.Show("Please enter the number of copy.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        numCopy.Select();
                        return;
                    }
                    List<string> itemAccnList = new List<string> { };

                    int lastNumberofAccn = 0;
                    MySqlDataReader dataReader=null;
                    string itemAccession = "";
                    queryString = "select itemAccession from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemAccnList = (from IDataRecord r in dataReader
                                        select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();

                    if (itemAccnList.IndexOf(txtbAccession.Text) >= 0)
                    {
                        MessageBox.Show("Accession No already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbAccession.SelectAll();
                        return;
                    }
                    
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = Convert.ToInt32(numCopy.Value);
                    for (int copyCount = 1; copyCount <= numCopy.Value; copyCount++)
                    {
                        //=====================Insert=============
                        queryString = "INSERT INTO item_details (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                            "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                            "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                            "itemImage,digitalReference) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                            ",'" + txtbRack.Text + "','" + globalVarLms.ItemDetails[9] + "','" + globalVarLms.ItemDetails[10] + "'" +
                            ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                            ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + globalVarLms.ItemDetails[19] + "',@digitalReference);";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", globalVarLms.ItemDetails[1]);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbAccession.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", globalVarLms.ItemDetails[3]);
                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", globalVarLms.ItemDetails[4]);
                        mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemClassification", globalVarLms.ItemDetails[6]);
                        mysqlCmd.Parameters.AddWithValue("@itemSubject", globalVarLms.ItemDetails[7]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo1", globalVarLms.ItemDetails[11]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo2", globalVarLms.ItemDetails[12]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo3", globalVarLms.ItemDetails[13]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo4", globalVarLms.ItemDetails[14]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo5", globalVarLms.ItemDetails[15]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo6", globalVarLms.ItemDetails[16]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo7", globalVarLms.ItemDetails[17]);
                        mysqlCmd.Parameters.AddWithValue("@addInfo8", globalVarLms.ItemDetails[18]);
                        mysqlCmd.Parameters.AddWithValue("@digitalReference", globalVarLms.ItemDetails[20]);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();

                        progressBar1.Value = copyCount;
                        lblCount.Text = "Item Added - " + copyCount.ToString();
                        copyAdded = copyCount;
                        Application.DoEvents();
                        //===============generate itemaccession=====================
                        queryString = "select itemAccession from item_details";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            itemAccnList = (from IDataRecord r in dataReader
                                            select (string)r["itemAccession"]).ToList();
                        }
                        dataReader.Close();

                        itemAccession = txtbAccession.Text;

                        string numericPortion = new String(txtbAccession.Text.Where(Char.IsDigit).ToArray());
                        lastNumberofAccn = Convert.ToInt32(numericPortion);
                        lastNumberofAccn++;

                        itemAccession = txtbAccession.Text.Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                        while (itemAccnList.IndexOf(itemAccession) >= 0)
                        {
                            lastNumberofAccn++;
                            itemAccession = txtbAccession.Text.Replace(numericPortion, formatAccession(numericPortion, lastNumberofAccn.ToString()));
                        }
                        txtbAccession.Text = itemAccession;
                    }
                }
                else
                {
                    List<string> itemAccnList = new List<string> { };
                    queryString = "select itemAccession from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemAccnList = (from IDataRecord r in dataReader
                                        select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();
                    string[] accnList = txtbAccession.Text.Split(',');
                    List<string> existData = new List<string> { };
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = accnList.Count();
                    int copyCount = 0;
                    foreach (string accnNumber in accnList)
                    {
                        if (itemAccnList.IndexOf(accnNumber) >= 0)
                        {
                            existData.Add("Accession No. : " + accnNumber + " [ not added due to duplicate accession no]");
                        }
                        else
                        {
                            queryString = "INSERT INTO item_details (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                "itemImage,digitalReference) VALUES (@itemTitle,@itemIsbn,@itemAccession,@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject" +
                                ",'" + txtbRack.Text + "','" + globalVarLms.ItemDetails[9] + "','" + globalVarLms.ItemDetails[10] + "'" +
                                ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                ",'" + currentDate + "','" + true + "','" + false + "','" + false + "','" + globalVarLms.ItemDetails[19] + "',@digitalReference);";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitel.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemIsbn", globalVarLms.ItemDetails[1]);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", accnNumber);
                            mysqlCmd.Parameters.AddWithValue("@itemCat", globalVarLms.ItemDetails[3]);
                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", globalVarLms.ItemDetails[4]);
                            mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemClassification", globalVarLms.ItemDetails[6]);
                            mysqlCmd.Parameters.AddWithValue("@itemSubject", globalVarLms.ItemDetails[7]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo1", globalVarLms.ItemDetails[11]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo2", globalVarLms.ItemDetails[12]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo3", globalVarLms.ItemDetails[13]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo4", globalVarLms.ItemDetails[14]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo5", globalVarLms.ItemDetails[15]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo6", globalVarLms.ItemDetails[16]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo7", globalVarLms.ItemDetails[17]);
                            mysqlCmd.Parameters.AddWithValue("@addInfo8", globalVarLms.ItemDetails[18]);
                            mysqlCmd.Parameters.AddWithValue("@digitalReference", globalVarLms.ItemDetails[20]);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                            copyCount++;

                            progressBar1.Value = copyCount;
                            lblCount.Text = "Item Added - " + copyCount.ToString();
                            copyAdded = copyCount;
                            Application.DoEvents();
                            //===============generate itemaccession=====================
                            queryString = "select itemAccession from item_details";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                itemAccnList = (from IDataRecord r in dataReader
                                                select (string)r["itemAccession"]).ToList();
                            }
                            dataReader.Close();
                        }
                    }
                    if (existData.Count > 0)
                    {
                        File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for make item copy.txt", existData);
                        try
                        {
                            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for make item copy.txt");
                        }
                        catch
                        {

                        }
                    }
                }
            }
            string jsonString = passwordHasher.Decrypt(File.ReadAllText(usageFilePath));
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);
            jsonObject.Usage["total-items"] = Convert.ToInt32(jsonObject.Usage["total-items"])+copyAdded;
            jsonString = passwordHasher.Encrypt(jsonObject.Usage.ToString());
            File.WriteAllText(usageFilePath, jsonString);
            globalVarLms.backupRequired = true;
            MessageBox.Show("Inserted Successfully.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        public string formatAccession(string numercPortion, string lastNumberofAccn)
        {
            while (lastNumberofAccn.Count() < numercPortion.Count())
            {
                lastNumberofAccn = "0" + lastNumberofAccn;
            }
            return lastNumberofAccn;
        }

        private void FormMultipleEntry_Load(object sender, EventArgs e)
        {
            lastAccession = txtbAccession.Text;
            rdbAuto.Checked = true;
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbAuto.Checked)
            {
                numCopy.Enabled = true;
                lblAccession.Text = "Starting Accession :";
                txtbAccession.Text = lastAccession;
                lblInfo.Visible = false;
            }
            else
            {
                numCopy.Enabled = false;
                lblAccession.Text = "Accession List :";
                lastAccession = txtbAccession.Text;
                txtbAccession.Clear();
                lblInfo.Visible = true;
            }
            progressBar1.Value = 0;
            lblCount.Text = "Item Added - " + 0.ToString();
        }
    }
}
