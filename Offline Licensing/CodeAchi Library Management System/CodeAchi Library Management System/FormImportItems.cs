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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormImportItems : Form
    {
        public FormImportItems()
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

        int lastNumber = 0;
        bool isAutoGenerate = false, isManualPrefix = false, noPrefix=false; string prefixText = "", joiningChar = "";
        List<string> accnList = new List<string> { };

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAuto.Checked)
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
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                            noPrefix = Convert.ToBoolean(dataReader["noPrefix"].ToString());
                            prefixText = dataReader["prefixText"].ToString();
                            joiningChar = dataReader["joiningChar"].ToString();
                            lastNumber = Convert.ToInt32(dataReader["lastNumber"].ToString());
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
                    string queryString = "select * from accn_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
                            noPrefix = Convert.ToBoolean(dataReader["noPrefix"].ToString());
                            prefixText = dataReader["prefixText"].ToString();
                            joiningChar = dataReader["joiningChar"].ToString();
                            lastNumber = Convert.ToInt32(dataReader["lastNumber"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }

                if (!isAutoGenerate)  //Auto Generate
                {
                    MessageBox.Show("Please change accession setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (globalVarLms.isAdmin)
                    {
                        FormAccnSetting accnSetting = new FormAccnSetting();
                        accnSetting.grpbAuto.Enabled = false;
                        accnSetting.rdbAuto.Enabled = true;
                        accnSetting.ShowDialog();
                    }
                    rdbAuto.Checked = false;
                }
                txtbFileName.Clear();
            }
            else
            {
                txtbFileName.Clear();
            }
        }

        public string formatAccession(string numercPortion, string lastNumberofAccn)
        {
            while (lastNumberofAccn.Count() < numercPortion.Count())
            {
                lastNumberofAccn = "0" + lastNumberofAccn;
            }
            return lastNumberofAccn;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtbFileName.Text;
                btnImport.Enabled = false;
                IntPtr hMenu = GetSystemMenu(this.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                string capInfo = "", queryString="";
                SQLiteConnection sqltConn = null;
                SQLiteCommand sqltCommnd = null;
                SQLiteDataReader dataReader = null;
                MySqlConnection mysqlConn=null;
                MySqlCommand mysqlCmd=null;
                MySqlDataReader sqldataReader = null;

                if (Properties.Settings.Default.sqliteDatabase)
                {
                    sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "select [capInfo] from itemSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            capInfo = dataReader["capInfo"].ToString();
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

                    queryString = "select capInfo from item_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    sqldataReader = mysqlCmd.ExecuteReader();
                    if (sqldataReader.HasRows)
                    {
                        while (sqldataReader.Read())
                        {
                            capInfo = sqldataReader["capInfo"].ToString();
                        }
                    }
                    sqldataReader.Close();
                    mysqlConn.Close();
                }
                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(txtbFileName.Text);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                int ttlData = xlRange.Rows.Count - 1;
                if (ttlData == 0)
                {
                    MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnImport.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                }
                else
                {
                    int ttlBooks = 0;
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select count(id) from itemDetails;";
                        sqltCommnd.CommandType = CommandType.Text;
                        ttlBooks = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                        ttlBooks = ttlBooks + ttlData;
                    }
                    else
                    {
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
                        queryString = "select count(id) from item_details;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        ttlBooks = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                        ttlBooks = ttlBooks + ttlData;
                        mysqlConn.Close();
                    }
                    if (ttlBooks >= globalVarLms.itemLimits)
                    {
                        MessageBox.Show("You can't add more than " + globalVarLms.itemLimits.ToString() + " items in this license!" + Environment.NewLine + "Please update your license to add more items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string itemId="", itemTitle, itemIsbn, itemCategory = cmbItemCategory.Text, itemSubcategory = cmbSubCategory.Text, itemAuthor, itemClassifi,
                        itemSubject, rackNo, ttlPages, itemPrice, addInfo1 = "", addInfo2 = "", addInfo3 = "", addInfo4 = "", addInfo5 = "",
                        addInfo6 = "", addInfo7 = "", addInfo8 = "", entryDate = "", imgPath = "";
                    entryDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                    string numericPortion, itemAccession;
                    lblTotal.Text = ttlData.ToString();
                    progressBar1.Maximum = ttlData;
                    string[] addInfoList = capInfo.Split('$');
                    List<string> existData = new List<string> { };
                    int itemCount = 0;
                    if (rdbAuto.Checked)
                    {
                        if (!isManualPrefix) //Manual prefix
                        {
                            if (Properties.Settings.Default.sqliteDatabase)
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
                                    if (itemSubcategory.Length < 3)
                                    {
                                        prefixText = itemSubcategory.ToUpper();
                                    }
                                    else
                                    {
                                        prefixText = itemSubcategory.ToUpper();
                                        prefixText = prefixText.Substring(0, 3);
                                    }
                                }
                                dataReader.Close();
                            }
                            else
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                queryString = "select shortName from item_subcategory where catName=@catName and subCatName=@subCatName";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);

                                mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                mysqlCmd.CommandTimeout = 99999;
                                sqldataReader = mysqlCmd.ExecuteReader();
                                if (sqldataReader.HasRows)
                                {
                                    while (sqldataReader.Read())
                                    {
                                        prefixText = sqldataReader["shortName"].ToString();
                                    }
                                }
                                else
                                {
                                    if (itemSubcategory.Length < 3)
                                    {
                                        prefixText = itemSubcategory.ToUpper();
                                    }
                                    else
                                    {
                                        prefixText = itemSubcategory.ToUpper();
                                        prefixText = prefixText.Substring(0, 3);
                                    }
                                }
                                sqldataReader.Close();
                                mysqlConn.Close();
                            }
                        }
                        int existCount = 0;
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            itemTitle = xlRange.Cells[i, "A"].Value2.ToString();

                            if (noPrefix)
                            {
                                itemAccession = "";
                                if (Properties.Settings.Default.sqliteDatabase)
                                {
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

                                        numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                        lastNumber = Convert.ToInt32(numericPortion);
                                        lastNumber++;

                                        itemAccession = formatAccession(numericPortion, lastNumber.ToString());

                                        while (accnList.IndexOf(itemAccession) >= 0)
                                        {
                                            lastNumber++;
                                            itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                        }
                                        itemId = itemAccession;
                                    }
                                    else
                                    {
                                        lastNumber = 1;
                                        itemId = lastNumber.ToString("00000");
                                    }
                                    dataReader.Close();
                                }
                                else
                                {
                                    if (mysqlConn.State == ConnectionState.Closed)
                                    {
                                        mysqlConn.Open();
                                    }
                                    queryString = "select itemAccession from item_details order by id desc limit 1";    //  
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    sqldataReader = mysqlCmd.ExecuteReader();
                                    if (sqldataReader.HasRows)
                                    {
                                        while (sqldataReader.Read())
                                        {
                                            itemAccession = sqldataReader["itemAccession"].ToString();
                                        }
                                        sqldataReader.Close();

                                        queryString = "select itemAccession from item_details";    // itemCat=@catName and itemSubCat=@subCatName and 
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.CommandTimeout = 99999;
                                        sqldataReader = mysqlCmd.ExecuteReader();
                                        if (sqldataReader.HasRows)
                                        {
                                            accnList = (from IDataRecord r in sqldataReader
                                                        select (string)r["itemAccession"]).ToList();
                                        }
                                        sqldataReader.Close();

                                        numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                        lastNumber = Convert.ToInt32(numericPortion);
                                        lastNumber++;

                                        itemAccession = formatAccession(numericPortion, lastNumber.ToString());

                                        while (accnList.IndexOf(itemAccession) >= 0)
                                        {
                                            lastNumber++;
                                            itemAccession = formatAccession(numericPortion, lastNumber.ToString());
                                        }
                                        itemId = itemAccession;
                                    }
                                    else
                                    {
                                        lastNumber = 1;
                                        itemId = lastNumber.ToString("00000");
                                        sqldataReader.Close();
                                    }
                                    mysqlConn.Close();
                                }
                            }
                            else
                            {
                                itemAccession = "";
                                if (Properties.Settings.Default.sqliteDatabase)
                                {
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

                                        numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                        lastNumber = Convert.ToInt32(numericPortion);
                                        lastNumber++;

                                        itemId = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                        while (accnList.IndexOf(itemId) >= 0)
                                        {
                                            lastNumber++;
                                            itemId = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                        }
                                    }
                                    else
                                    {
                                        dataReader.Close();
                                        lastNumber = 1;
                                    }
                                    itemId = prefixText + joiningChar + lastNumber.ToString("00000");
                                    itemId = itemId.Replace(" ", "");
                                }
                                else
                                {
                                    if (mysqlConn.State == ConnectionState.Closed)
                                    {
                                        mysqlConn.Open();
                                    }
                                    queryString = "select itemAccession from item_details where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession order by [id] desc limit 1";    //  
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                                    mysqlCmd.CommandTimeout = 99999;
                                    sqldataReader = mysqlCmd.ExecuteReader();
                                    if (sqldataReader.HasRows)
                                    {
                                        while (sqldataReader.Read())
                                        {
                                            itemAccession = sqldataReader["itemAccession"].ToString();
                                        }
                                        sqldataReader.Close();

                                        queryString = "select itemAccession from item_details where itemCat=@catName and itemSubCat=@subCatName and itemAccession like @itemAccession";    // itemCat=@catName and itemSubCat=@subCatName and 
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@subCatName", cmbSubCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemAccession", prefixText + joiningChar + "%");
                                        mysqlCmd.CommandTimeout = 99999;
                                        sqldataReader = mysqlCmd.ExecuteReader();
                                        if (sqldataReader.HasRows)
                                        {
                                            accnList = (from IDataRecord r in sqldataReader
                                                        select (string)r["itemAccession"]).ToList();
                                        }
                                        sqldataReader.Close();

                                        numericPortion = new String(itemAccession.Where(Char.IsDigit).ToArray());
                                        lastNumber = Convert.ToInt32(numericPortion);
                                        lastNumber++;

                                        itemId = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                        while (accnList.IndexOf(itemId) >= 0)
                                        {
                                            lastNumber++;
                                            itemId = prefixText + joiningChar + formatAccession(numericPortion, lastNumber.ToString());
                                        }
                                    }
                                    else
                                    {
                                        sqldataReader.Close();
                                        lastNumber = 1;
                                    }
                                    mysqlConn.Close();
                                    itemId = prefixText + joiningChar + lastNumber.ToString("00000");
                                    itemId = itemId.Replace(" ", "");
                                }
                            }

                            existCount = 0;
                            if (Properties.Settings.Default.sqliteDatabase)
                            {
                                sqltCommnd.CommandText = "select count(itemAccession) from itemDetails where itemAccession=@itemAccession";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                                existCount = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                            }
                            else
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                queryString = "select count(itemAccession) from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                                mysqlCmd.CommandTimeout = 99999;
                                existCount = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                            }

                            if (existCount>0)
                            {
                                existData.Add("Accession : " + itemId + " Title : " + itemTitle + " [ not added due to duplicate accession number]");
                            }
                            else
                            {
                                itemIsbn = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                                itemAuthor = xlRange.Cells[i, "C"].Value2 != null ? xlRange.Cells[i, "C"].Value2.ToString() : "";
                                itemClassifi = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "";
                                itemSubject = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                rackNo = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : "";
                                ttlPages = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : 0.ToString();
                                itemPrice = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : 0.00.ToString();
                                if (capInfo != "" && capInfo != null)
                                {
                                    if (addInfoList.Length == 1)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 2)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 3)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 4)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 5)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                        addInfo5 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 6)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                        addInfo5 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                        addInfo6 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 7)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                        addInfo5 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                        addInfo6 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                        addInfo7 = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "P"].Value2 != null ? xlRange.Cells[i, "P"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 8)
                                    {
                                        addInfo1 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                        addInfo5 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                        addInfo6 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                        addInfo7 = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : "";
                                        addInfo8 = xlRange.Cells[i, "P"].Value2 != null ? xlRange.Cells[i, "P"].Value2.ToString() : "";
                                        imgPath = xlRange.Cells[i, "Q"].Value2 != null ? xlRange.Cells[i, "Q"].Value2.ToString() : null;
                                    }
                                }
                                else
                                {
                                    imgPath = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : null;
                                }

                                Image itemImage = null;
                                String base64String = "base64String";
                                try
                                {
                                    if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                    {
                                        itemImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                        using (MemoryStream memoryStream = new MemoryStream())
                                        {
                                            itemImage.Save(memoryStream, itemImage.RawFormat);
                                            byte[] imageBytes = memoryStream.ToArray();
                                            base64String = Convert.ToBase64String(imageBytes);
                                        }
                                    }
                                }
                                catch
                                {
                                    base64String = "base64String";
                                }

                                if (!ttlPages.All(char.IsNumber))
                                {
                                    ttlPages = 0.ToString();
                                }

                                if (!itemPrice.Replace(".", "").All(char.IsNumber))
                                {
                                    itemPrice = 0.00.ToString("00");
                                }
                                //====================Insert Borrower details======================
                                if (Properties.Settings.Default.sqliteDatabase)
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    queryString = "INSERT INTO itemDetails (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                        "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                        "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                        "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession," +
                                        "@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject,@rackNo,'" + ttlPages + "','" + itemPrice + "'" +
                                        ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                        ",'" + entryDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@itemTitle", itemTitle);
                                    sqltCommnd.Parameters.AddWithValue("@itemIsbn", itemIsbn);
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", itemCategory);
                                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", itemSubcategory);
                                    sqltCommnd.Parameters.AddWithValue("@itemAuthor", itemAuthor);
                                    sqltCommnd.Parameters.AddWithValue("@itemClassification", itemClassifi);
                                    sqltCommnd.Parameters.AddWithValue("@itemSubject", itemSubject);
                                    sqltCommnd.Parameters.AddWithValue("@rackNo", rackNo);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo6", addInfo6);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo7", addInfo7);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo8", addInfo8);
                                    sqltCommnd.ExecuteNonQuery();
                                }
                                else
                                {
                                    queryString = "INSERT INTO item_details (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                        "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                        "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                        "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession," +
                                        "@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject,@rackNo,'" + ttlPages + "','" + itemPrice + "'" +
                                        ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                        ",'" + entryDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemTitle", itemTitle);
                                    mysqlCmd.Parameters.AddWithValue("@itemIsbn", itemIsbn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                                    mysqlCmd.Parameters.AddWithValue("@itemCat", itemCategory);
                                    mysqlCmd.Parameters.AddWithValue("@itemSubCat", itemSubcategory);
                                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor);
                                    mysqlCmd.Parameters.AddWithValue("@itemClassification", itemClassifi);
                                    mysqlCmd.Parameters.AddWithValue("@itemSubject", itemSubject);
                                    mysqlCmd.Parameters.AddWithValue("@rackNo", rackNo);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo6", addInfo6);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo7", addInfo7);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo8", addInfo8);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();
                                    mysqlConn.Close();
                                }

                                itemCount++;
                                progressBar1.Value = itemCount;
                                lblItem.Text = itemCount.ToString();
                                Application.DoEvents();
                            }
                        }
                        if (lastNumber > 0)
                        {
                            if (Properties.Settings.Default.sqliteDatabase)
                            {
                                queryString = "update accnSetting  set lastNumber='" + lastNumber + "'";
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.ExecuteNonQuery();
                                sqltConn.Close();
                            }
                            else
                            {
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                queryString = "update accn_setting  set lastNumber='" + lastNumber + "'";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                mysqlCmd.ExecuteNonQuery();
                                mysqlConn.Close();
                            }
                        }
                    }
                    else
                    {
                        int existCount = 0;
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            itemId =xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            itemTitle = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                            if (itemId.Contains("©"))
                            {
                                existData.Add("Accession : " + itemId + " Title : " + itemTitle + " [ not added due to unwanted character found in the accession numnbe. Please only enter allowed value in accession.]");
                            }
                            else
                            {
                                existCount = 0;
                                if (Properties.Settings.Default.sqliteDatabase)
                                {
                                    sqltCommnd.CommandText = "select count(itemAccession) from itemDetails where itemAccession=@itemAccession";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                                    existCount = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                }
                                else
                                {
                                    if (mysqlConn.State == ConnectionState.Closed)
                                    {
                                        mysqlConn.Open();
                                    }
                                    queryString = "select count(itemAccession) from item_details where itemAccession=@itemAccession";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                                    mysqlCmd.CommandTimeout = 99999;
                                    existCount = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                }

                                if (existCount > 0)
                                {
                                    existData.Add("Accession : " + itemId + " Title : " + itemTitle + " [ not added due to duplicate accession number]");
                                }
                                else
                                {
                                    itemIsbn = xlRange.Cells[i, "C"].Value2 != null ? xlRange.Cells[i, "C"].Value2.ToString() : "";
                                    itemAuthor = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "";
                                    itemClassifi = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                    itemSubject = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : "";
                                    rackNo = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                    ttlPages = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : 0.ToString();
                                    itemPrice = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : 0.00.ToString();
                                    if (capInfo != "" && capInfo != null)
                                    {
                                        if (addInfoList.Length == 1)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 2)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 3)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 4)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 5)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                            addInfo5 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 6)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                            addInfo5 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                            addInfo6 = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "P"].Value2 != null ? xlRange.Cells[i, "P"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 7)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                            addInfo5 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                            addInfo6 = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : "";
                                            addInfo7 = xlRange.Cells[i, "P"].Value2 != null ? xlRange.Cells[i, "P"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "Q"].Value2 != null ? xlRange.Cells[i, "Q"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 8)
                                        {
                                            addInfo1 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : "";
                                            addInfo5 = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : "";
                                            addInfo6 = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : "";
                                            addInfo7 = xlRange.Cells[i, "P"].Value2 != null ? xlRange.Cells[i, "P"].Value2.ToString() : "";
                                            addInfo8 = xlRange.Cells[i, "Q"].Value2 != null ? xlRange.Cells[i, "Q"].Value2.ToString() : "";
                                            imgPath = xlRange.Cells[i, "R"].Value2 != null ? xlRange.Cells[i, "R"].Value2.ToString() : null;
                                        }
                                    }
                                    else
                                    {
                                        imgPath = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : null;
                                    }

                                    Image itemImage = null;
                                    String base64String = "base64String";
                                    try
                                    {
                                        if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                        {
                                            itemImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                            using (MemoryStream memoryStream = new MemoryStream())
                                            {
                                                itemImage.Save(memoryStream, itemImage.RawFormat);
                                                byte[] imageBytes = memoryStream.ToArray();
                                                base64String = Convert.ToBase64String(imageBytes);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        base64String = "base64String";
                                    }

                                    //====================Insert Borrower details======================
                                    if (Properties.Settings.Default.sqliteDatabase)
                                    {
                                        sqltCommnd = sqltConn.CreateCommand();
                                        queryString = "INSERT INTO itemDetails (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                            "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                            "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                            "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession," +
                                            "@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject,@rackNo,'" + ttlPages + "','" + itemPrice + "'" +
                                            ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                            ",'" + entryDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@itemTitle", itemTitle);
                                        sqltCommnd.Parameters.AddWithValue("@itemIsbn", itemIsbn);
                                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId.ToUpper());
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", itemCategory);
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", itemSubcategory);
                                        sqltCommnd.Parameters.AddWithValue("@itemAuthor", itemAuthor);
                                        sqltCommnd.Parameters.AddWithValue("@itemClassification", itemClassifi);
                                        sqltCommnd.Parameters.AddWithValue("@itemSubject", itemSubject);
                                        sqltCommnd.Parameters.AddWithValue("@rackNo", rackNo);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo6", addInfo6);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo7", addInfo7);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo8", addInfo8);
                                        sqltCommnd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        queryString = "INSERT INTO item_details (itemTitle,itemIsbn,itemAccession,itemCat,itemSubCat," +
                                            "itemAuthor,itemClassification,itemSubject,rackNo,totalPages,itemPrice,addInfo1,addInfo2," +
                                            "addInfo3,addInfo4,addInfo5,addInfo6,addInfo7,addInfo8,entryDate,itemAvailable,isLost,isDamage," +
                                            "itemImage) VALUES (@itemTitle,@itemIsbn,@itemAccession," +
                                            "@itemCat,@itemSubCat,@itemAuthor,@itemClassification,@itemSubject,@rackNo,'" + ttlPages + "','" + itemPrice + "'" +
                                            ",@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5,@addInfo6,@addInfo7,@addInfo8" +
                                            ",'" + entryDate + "','" + true + "','" + false + "','" + false + "','" + base64String + "');";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemTitle", itemTitle);
                                        mysqlCmd.Parameters.AddWithValue("@itemIsbn", itemIsbn);
                                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId.ToUpper());
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", itemCategory);
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", itemSubcategory);
                                        mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor);
                                        mysqlCmd.Parameters.AddWithValue("@itemClassification", itemClassifi);
                                        mysqlCmd.Parameters.AddWithValue("@itemSubject", itemSubject);
                                        mysqlCmd.Parameters.AddWithValue("@rackNo", rackNo);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo6", addInfo6);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo7", addInfo7);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo8", addInfo8);
                                        mysqlCmd.CommandTimeout = 99999;
                                        mysqlCmd.ExecuteNonQuery();
                                        mysqlConn.Close();
                                    }
                                    itemCount++;
                                    progressBar1.Value = itemCount;
                                    lblItem.Text = itemCount.ToString();
                                    Application.DoEvents();
                                }
                            }
                        }
                    }
                    btnImport.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                    txtbFileName.Clear();
                    globalVarLms.backupRequired = true;
                    MessageBox.Show("Items successfully imported.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (existData.Count > 0)
                    {
                        File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for import items.txt", existData);
                        try
                        {
                            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for import items.txt");
                        }
                        catch
                        {

                        }
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                xlWorkbook.Close(true);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            }
            catch
            {
                btnImport.Enabled=true;
            }
        }

        private void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            if (cmbItemCategory.SelectedIndex > 0)
            {
                cmbSubCategory.Items.Clear();
                cmbSubCategory.Items.Add("Please select subcategory...");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select subCatName from itemSubCategory where catName=@catName";
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
                    string queryString = "select subCatName from item_subcategory where catName=@catName";
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
                    mysqlConn.Close();
                }
                cmbSubCategory.SelectedIndex = 0;
                btnBrowse.Enabled = true;
            }
            else
            {
                btnBrowse.Enabled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            if (cmbItemCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbSubCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!rdbManual.Checked && !rdbAuto.Checked)
            {
                MessageBox.Show("Please select id type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string tempPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
            Directory.CreateDirectory(tempPath);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = Application.ProductName + "enter file name.";
            saveDialog.InitialDirectory = tempPath;
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel file (*.xlsx)|*.xlsx";
            if (rdbAuto.Checked)
            {
                saveDialog.FileName = cmbItemCategory.Text+"("+cmbSubCategory.Text + ")_" + rdbAuto.Text;
            }
            else
            {
                saveDialog.FileName = cmbItemCategory.Text+"("+cmbSubCategory.Text + ")_" + rdbManual.Text;
            }
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                string capInfo = "";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select [capInfo] from itemSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            capInfo = dataReader["capInfo"].ToString();
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
                    string queryString = "select capInfo from item_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            capInfo = dataReader["capInfo"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                string[] addInfoList = capInfo.Split('$');
                string columnHeader = "Title,ISBN,Author,Classification No,Subject,Rack No,No of Pages,Price,Image Name";
                if (capInfo != "" && capInfo != null)
                {
                    if (addInfoList.Length == 1)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                         "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 2)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                         "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                         ",Image Name";
                    }
                    else if (addInfoList.Length == 3)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                            "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                            "," + addInfoList[2].Replace("3_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 4)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                             "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                             "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 5)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                            "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                            "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                             "," + addInfoList[4].Replace("5_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 6)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                           "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                           "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                            "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 7)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                           "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                           "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                            "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") +
                            "," + addInfoList[6].Replace("7_", "") + ",Image Name";
                    }
                    else if (addInfoList.Length == 8)
                    {
                        columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                            "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                            "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                             "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") +
                             "," + addInfoList[6].Replace("7_", "") + "," + addInfoList[7].Replace("8_", "") + ",Image Name";
                    }
                }
                if (!rdbAuto.Checked)
                {
                    columnHeader = "Accession No," + columnHeader;
                }

                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);//xlApp.Workbooks.Add(misValue);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                int colPos = 1;
                foreach (string headerName in columnHeader.Split(','))
                {
                    xlWorksheet.Cells[1, colPos] = headerName;
                    colPos++;
                }
                xlWorksheet.Columns.AutoFit();
                var columnHeadingsRange = xlWorksheet.Range[
               xlWorksheet.Cells[1, "A"],
               xlWorksheet.Cells[1, "Y"]];
                columnHeadingsRange.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbDarkSeaGreen;
                columnHeadingsRange.Font.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbWhite;

                xlWorkbook.SaveAs(fileName);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);

                xlWorkbook = xlApp.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlApp.Visible = true;
                xlApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                btnGenerate.Enabled = false;
                while (xlApp.Visible == true)
                {
                    Application.DoEvents();
                }
                btnGenerate.Enabled = true;
                txtbFileName.Text = saveDialog.FileName;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            if (cmbItemCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbSubCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!rdbManual.Checked && !rdbAuto.Checked)
            {
                MessageBox.Show("Please select id type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string capInfo = "";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select [capInfo] from itemSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        capInfo = dataReader["capInfo"].ToString();
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
                string queryString = "select capInfo from item_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        capInfo = dataReader["capInfo"].ToString();
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            string[] addInfoList = capInfo.Split('$');
            string columnHeader = "Title,ISBN,Author,Classification No,Subject,Rack No,No of Pages,Price,Image Name";
            if (capInfo != "" && capInfo != null)
            {
                if (addInfoList.Length == 1)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                     "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 2)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                     "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                     ",Image Name";
                }
                else if (addInfoList.Length == 3)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                        "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                        "," + addInfoList[2].Replace("3_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 4)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                         "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                         "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 5)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                        "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                        "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                         "," + addInfoList[4].Replace("5_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 6)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                       "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                       "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                        "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 7)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                       "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                       "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                        "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") +
                        "," + addInfoList[6].Replace("7_", "") + ",Image Name";
                }
                else if (addInfoList.Length == 8)
                {
                    columnHeader = "Title,ISBN,Author,Classification No,Subject," +
                        "Rack No,No of Pages,Price," + addInfoList[0].Replace("1_", "") + "," + addInfoList[1].Replace("2_", "") +
                        "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                         "," + addInfoList[4].Replace("5_", "") + "," + addInfoList[5].Replace("6_", "") +
                         "," + addInfoList[6].Replace("7_", "") + "," + addInfoList[7].Replace("8_", "") + ",Image Name";
                }
            }
            if (!rdbAuto.Checked)
            {
                columnHeader = "Accession No," + columnHeader;
            }
            OpenFileDialog selectFile = new OpenFileDialog();
            selectFile.Title = Application.ProductName + " Select Excel File";
            selectFile.Filter = "Exce File|*.xlsx;*.xls";
            selectFile.Multiselect = false;
            if (selectFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(selectFile.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

                    int columnIndex = columnHeader.Split(',').Count();
                    string colLetter = String.Empty;
                    int modvalue = 0;

                    while (columnIndex > 0)
                    {
                        modvalue = (columnIndex - 1) % 26;
                        colLetter = (char)(65 + modvalue) + colLetter;
                        columnIndex = (int)((columnIndex - modvalue) / 26);
                    }

                    System.Array returnedValue;
                    Microsoft.Office.Interop.Excel.Range range = xlWorksheet.get_Range("A1", colLetter + "1".ToString());
                    returnedValue = (System.Array)range.Cells.Value;
                    string[] strArray = returnedValue.OfType<object>().Select(o => o.ToString()).ToArray();
                    if (string.Join(",", strArray) == columnHeader)
                    {
                        txtbFileName.Text = selectFile.FileName;
                    }
                    else
                    {
                        txtbFileName.Clear();
                        MessageBox.Show("File not in correct format.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                    xlWorkbook.Close(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                }
                catch
                {
                    MessageBox.Show("File already opend." + Environment.NewLine + "Please close the file snd try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                txtbFileName.Clear();
            }
        }

        private void txtbFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtbFileName.Text != "")
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }
        }

        private void cmbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
        }

        private void rdbManual_CheckedChanged(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
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

        private void btnImage_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            if (browseFolder.ShowDialog() == DialogResult.OK)
            {
                txtbImage.Text = browseFolder.SelectedPath;
            }
        }

        private void btnImport_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnImport.Enabled == true)
            {
                btnImport.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnImport.BackColor = Color.DimGray;
            }
        }

        private void FormImportItems_Load(object sender, EventArgs e)
        {
            cmbItemCategory.Items.Clear();
            cmbItemCategory.Items.Add("Please select a Category...");

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //======================Item Category add to combobox============
                string queryString = "select catName from itemSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbItemCategory.Items.Add(dataReader["catName"].ToString());
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
                    string queryString = "select catName from item_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbItemCategory.Items.Add(dataReader["catName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            cmbItemCategory.SelectedIndex = 0;
            rdbAuto.Checked = false;
            rdbManual.Checked = false;
            btnImport.Enabled = false;
        }

        private void FormImportItems_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }
    }
}
