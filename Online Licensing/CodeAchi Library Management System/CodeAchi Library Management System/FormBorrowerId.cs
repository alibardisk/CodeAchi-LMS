using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormBorrowerId : Form
    {
        private System.Drawing.Font printFont;
        private System.Drawing.Font printFontBig;
        private string borrowerId = "BRC-1234567";

        public FormBorrowerId()
        {
            InitializeComponent();
        }
        string categoryText = "Category Name",idText="Borrower Id.";
        public static AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormBorrowerId_Load(object sender, EventArgs e)
        {
            timer1.Interval = 450;
            timer1.Start();
            btnBrowse.Enabled = false;
            panelDate.Visible = false;
            lblMessage.Visible = false;
            lblBlink.Visible = false;
            cmbCategory.Visible = false;
            cmbPaper.Enabled = false;
            numUpBlock.Enabled = false;
            cmbOption.SelectedIndex = 0;
            chkbQr.Checked = true;
            chkbFooter.Checked = true;
            dtpTo.CustomFormat = Properties.Settings.Default.dateFormat;
            dtpFrom.CustomFormat = Properties.Settings.Default.dateFormat;

            dgvBrrList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvBrrList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            GetBorrowerId();
            txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbInfo.AutoCompleteCustomSource = autoCollData;
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("-------select--------");

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catName from borrowerSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbCategory.Items.Add(dataReader["catName"].ToString());
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
                    string queryString = "select catName from borrower_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbCategory.Items.Add(dataReader["catName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            cmbCategory.SelectedIndex = 0;
            btnPrint.Enabled = false;
            txtbPrinterName.Text = Properties.Settings.Default.icardPrinter;
            loadFieldValue();
        }

        public static void GetBorrowerId()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============================Add all Items======================
                string queryString = "select brrId from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                autoCollData.Clear();
                if (dataReader.HasRows)
                {
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["brrId"]
                            ).ToList();
                    autoCollData.AddRange(idList.ToArray());
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
                    string queryString = "select brrId from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    autoCollData.Clear();
                    if (dataReader.HasRows)
                    {
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["brrId"]
                                ).ToList();
                        autoCollData.AddRange(idList.ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void FormBorrowerId_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void rdbManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbManual.Checked)
            {
                dgvBrrList.Rows.Clear();
                panelDate.Visible = false;
                cmbCategory.Visible = false;
                lblInfo.Text = "Enter "+idText+" :";
                txtbInfo.Clear();
                timer1.Interval = 450;
                timer1.Start();
                label7.Visible = true;
                GetBorrowerId();
                txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbInfo.AutoCompleteCustomSource = autoCollData;
            }
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDate.Checked)
            {
                timer1.Stop();
                dgvBrrList.Rows.Clear();
                panelDate.Visible = true;
                lblInfo.Text = "Enter "+idText+" :";
            }
        }

        private void rdbCategory_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCategory.Checked)
            {
                dgvBrrList.Rows.Clear();
                panelDate.Visible = false;
                cmbCategory.Visible = true;
                lblInfo.Text = categoryText+" :";
                timer1.Stop();
                lblNotification.Visible = false;
                label7.Visible = false;
            }
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
                        if (fieldName == "lblBrId")
                        {
                            idText = fieldValue.Replace(fieldName + "=", "") ;
                            dgvBrrList.Columns[1].HeaderText= fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrName")
                        {
                            dgvBrrList.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            categoryText = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void txtbInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if(txtbInfo.Text!="")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        //==============================Add all Items======================
                        string queryString = "";
                        SQLiteDataReader dataReader;
                        queryString = "select brrId,brrName from borrowerDetails where brrId=@brrId collate nocase";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbInfo.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBrrList.Rows.Add(true, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
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
                        string queryString = "select brrId,brrName from borrower_details where lower(brrId)=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbInfo.Text.ToLower());
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBrrList.Rows.Add(true, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    autoCollData.Remove(txtbInfo.Text);
                    dgvBrrList.ClearSelection();
                    txtbInfo.Clear();
                }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dtpFrom.Value.Date;
            DateTime tempdate = dtpFrom.Value.Date;
            string filterDate = "";
            dgvBrrList.Rows.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = null;
                SQLiteDataReader dataReader = null;

                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select brrId,brrName from borrowerDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
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
                MySqlCommand mysqlCmd=null;
                MySqlDataReader dataReader = null;
                string queryString = "";
                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    queryString = "select brrId,brrName from borrower_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
                }
                mysqlConn.Close();
            }
            dgvBrrList.ClearSelection();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dtpFrom.Value.Date;
            DateTime tempdate = dtpFrom.Value.Date;
            string filterDate = "";
            dgvBrrList.Rows.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = null;
                SQLiteDataReader dataReader = null;

                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select brrId,brrName from borrowerDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
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
                MySqlDataReader dataReader = null;
                string queryString = "";
                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    queryString = "select brrId,brrName from borrower_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
                }
                mysqlConn.Close();
            }
            dgvBrrList.ClearSelection();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rdbCategory.Checked && cmbCategory.SelectedIndex !=-1 && cmbCategory.SelectedIndex !=0)
            {
                dgvBrrList.Rows.Clear();
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select brrId,brrName from borrowerDetails where brrCategory=@brrCategory";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
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
                    string queryString = "select brrId,brrName from borrower_details where brrCategory=@brrCategory";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBrrList.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                dgvBrrList.ClearSelection();
            }
        }

        private void dgvBrrList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dgvBrrList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (cmbOption.SelectedIndex==0)
            {
                MessageBox.Show("Please select a type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbOption.Select();
                return;
            }
            if (rdbPrinter.Checked && txtbPrinterName.Text == "")
            {
                lblMessage.Text = "Please select printer";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                btnPrinter.Select();
                return;
            }
            if (rdbPdf.Checked && txtbPath.Text == "")
            {
                lblMessage.Text = "Please enter the file name";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                btnBrowse.Select();
                return;
            }
            if(cmbOption.SelectedIndex==2 && cmbPaper.SelectedIndex==-1)
            {
                lblMessage.Text = "Please select the paper size";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                btnBrowse.Select();
                return;
            }
            if (dgvBrrList.Rows.Count == 0)
            {
                lblMessage.Text = "Please add some item";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                return;
            }
            DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            if (dgvCheckedRows.Count()==0)
            {
                lblMessage.Text = "Please check some items";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                return;
            }
            //======================Borrower Id Generate================
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
            string fileName = "borrowerId";
            fileName = folderPath + @"\" + fileName + ".pdf";
            if (rdbPdf.Checked)
            {
                fileName = txtbPath.Text;
            }
            else
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            string instName = "", instAddress = "", instShortName = "", queryString=""; System.Drawing.Image instLogo = null;
            SQLiteConnection sqltConn = null;
            SQLiteCommand sqltCommnd = null;
            SQLiteDataReader dataReader = null;

            MySqlConnection mysqlConn=null;
            MySqlCommand mysqlCmd = null;
            MySqlDataReader sqldataReader =null;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                sqltCommnd = sqltConn.CreateCommand();
                queryString = "select instName,instLogo,instAddress,instShortName from generalSettings";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        instName = dataReader["instName"].ToString();
                        instAddress = dataReader["instAddress"].ToString();
                        instShortName = dataReader["instShortName"].ToString();
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
                queryString = "select instName,instLogo,instAddress,instShortName from general_settings";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                sqldataReader = mysqlCmd.ExecuteReader();
                if (sqldataReader.HasRows)
                {
                    while (sqldataReader.Read())
                    {
                        instName = sqldataReader["instName"].ToString();
                        instAddress = sqldataReader["instAddress"].ToString();
                        instShortName = sqldataReader["instShortName"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(sqldataReader["instLogo"].ToString());
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
                sqldataReader.Close();
                mysqlConn.Close();
            }
            if(instAddress.Length>60)
            {
                instAddress = instAddress.Substring(0, 60);
            }

            if (cmbOption.Text== "Print Barcode")
            {
                if (cmbPaper.Text == "A4_24L (3x8)")
                {
                    try
                    { 
                        BarcodeFor3x8(fileName);

                        if (rdbPrinter.Checked)//============Print Barcode===============
                        {
                            printA4Paper(fileName);
                        }
                        else
                        {
                            MessageBox.Show("Borrower id generated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try
                            {
                                Process.Start(txtbPath.Text);
                            }
                            catch
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (cmbPaper.Text == "A4_30L (3x10)")
                {
                    try
                    { 
                        BarcodeFor3x10(fileName);

                        if (rdbPrinter.Checked)//============Print Barcode===============
                        {
                            printA4Paper(fileName);
                        }
                        else
                        {
                            MessageBox.Show("Borrower id generated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try
                            {
                                Process.Start(txtbPath.Text);
                            }
                            catch
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (cmbPaper.Text == "Barcode Printer (5x2.5 Cm_1x1)")
                {
                    printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                    printFontBig = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        printFont = new System.Drawing.Font("Malgun Gothic", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                        printFontBig = new System.Drawing.Font("Malgun Gothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                    }
                    if (rdbPrinter.Checked)
                    {
                        //printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        PrintDocument printDocument = new PrintDocument();
                        printDocument.PrinterSettings.PrinterName = txtbPrinterName.Text;
                        dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                        // Set the page orientation to landscape.
                        foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes)
                        {
                            int paperHeight = 0;
                            if (dgvCheckedRows.Length % 2 == 0)
                            {
                                paperHeight = Convert.ToInt32(108.5 * (dgvCheckedRows.Length / 2));
                            }
                            else
                            {
                                paperHeight = Convert.ToInt32(108.5 * ((dgvCheckedRows.Length + 1) / 2));
                            }
                            printDocument.DefaultPageSettings.PaperSize = new PaperSize(paperSize.PaperName, 202, paperHeight);
                            //break;
                        }

                        printDocument.PrintPage += new PrintPageEventHandler(this.BarcodePrinter5x25Cm_1x1);
                        printDocument.Print();
                    }
                    else
                    {
                        try
                        {
                            PdfBarcodePrinter5x25_1x1(fileName);
                            MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try
                            {
                                Process.Start(txtbPath.Text);
                            }
                            catch
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (cmbPaper.Text == "Barcode Printer (5x2.5 Cm_2x1)")
                {
                    printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                    printFontBig = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        printFont = new System.Drawing.Font("Malgun Gothic", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                        printFontBig = new System.Drawing.Font("Malgun Gothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                    }
                    if (rdbPrinter.Checked)
                    {
                        //printA4Paper(fileName);
                        //printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                        PrintDocument printDocument = new PrintDocument();
                        printDocument.PrinterSettings.PrinterName = txtbPrinterName.Text;
                        dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                        // Set the page orientation to landscape.
                        foreach (PaperSize paperSize in printDocument.PrinterSettings.PaperSizes)
                        {
                            int paperHeight = 0;
                            if (dgvCheckedRows.Length % 2 == 0)
                            {
                                paperHeight = Convert.ToInt32(108.5 * (dgvCheckedRows.Length / 2));
                            }
                            else
                            {
                                paperHeight = Convert.ToInt32(108.5 * ((dgvCheckedRows.Length + 1) / 2));
                            }
                            printDocument.DefaultPageSettings.PaperSize = new PaperSize(paperSize.PaperName, 404, paperHeight);
                            //break;
                        }

                        printDocument.PrintPage += new PrintPageEventHandler(this.BarcodePrinter5x25Cm_2x1);
                        printDocument.Print();
                    }
                    else
                    {
                        try
                        { 
                            PdfBarcodePrinter5x25_2x1(fileName);
                            MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try
                            {
                                Process.Start(txtbPath.Text);
                            }
                            catch
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if (instLogo == null)
                    {
                        instLogo = Properties.Resources.NoImageAvailable;
                    }
                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);

                    using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                    {
                        Document pdfToCreate = new Document(PageSize.A4);

                        Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                        System.Drawing.Image imgBarcode;

                        Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                        System.Drawing.Image imgQrcode;

                        float pageWidth = pdfToCreate.PageSize.Width;
                        float pageHeight = pdfToCreate.PageSize.Height;
                        float imglocX = 0, imglocY = 0, rectYpos = 0;
                        int columnCount = 0, rowCount = 0;
                        float rowDistance = pageHeight / 5;
                        iTextSharp.text.Image barcodeJpg;
                        iTextSharp.text.Image qrcodeJpg;
                        iTextSharp.text.Rectangle rectAngle;
                        iTextSharp.text.Image memberImgJpg = null;
                        pdfToCreate.SetMargins(0, 0, 0, 0);
                        PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                        BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                        BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                        if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                        {
                            baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        }
                        pdfToCreate.Open();
                        PdfContentByte pdfContent = pdWriter.DirectContent;

                        DateTime entrydate = DateTime.Now;
                        string borrowerId = "", brrAddress = "", brrName = "", issueDate = "", expDate = "", brrContact = "",brrCategory="";
                        System.Drawing.Image brrImage = null;

                        foreach (DataGridViewRow dataRow in dgvCheckedRows)
                        {
                            borrowerId = dataRow.Cells[1].Value.ToString();
                            if (borrowerId != "")
                            {
                                if (Properties.Settings.Default.sqliteDatabase)
                                {
                                    sqltConn = ConnectionClass.sqliteConnection();
                                    if (sqltConn.State == ConnectionState.Closed)
                                    {
                                        sqltConn.Open();
                                    }
                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "select brrName,brrAddress,brrContact,entryDate,mbershipDuration,imagePath,brrCategory from borrowerDetails where brrId=@brrId";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", borrowerId);
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            brrName = dataReader["brrName"].ToString();
                                            brrAddress = dataReader["brrAddress"].ToString();
                                            brrContact = dataReader["brrContact"].ToString();
                                            issueDate = dataReader["entryDate"].ToString();
                                            brrCategory = dataReader["brrCategory"].ToString();
                                            entrydate = DateTime.ParseExact(issueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            entrydate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                                            expDate = entrydate.Day.ToString("00") + "/" + entrydate.Month.ToString("00") + "/" + entrydate.Year.ToString("0000");
                                            try
                                            {
                                                brrImage = System.Drawing.Image.FromFile(Properties.Settings.Default.databasePath + @"\BorrowerImage\" + dataReader["imagePath"].ToString());
                                            }
                                            catch
                                            {
                                                brrImage = Properties.Resources.blankBrrImage;
                                            }
                                            memberImgJpg = iTextSharp.text.Image.GetInstance(brrImage, BaseColor.WHITE);
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
                                        queryString = "select brrName,brrAddress,brrContact,entryDate,mbershipDuration,imagePath,brrCategory from borrower_details where brrId=@brrId";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrId", borrowerId);
                                        mysqlCmd.CommandTimeout = 99999;
                                        sqldataReader = mysqlCmd.ExecuteReader();
                                        if (sqldataReader.HasRows)
                                        {
                                            while (sqldataReader.Read())
                                            {
                                                brrName = sqldataReader["brrName"].ToString();
                                                brrAddress = sqldataReader["brrAddress"].ToString();
                                                brrContact = sqldataReader["brrContact"].ToString();
                                                issueDate = sqldataReader["entryDate"].ToString();
                                                brrCategory = sqldataReader["brrCategory"].ToString();
                                                entrydate = DateTime.ParseExact(issueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                entrydate = entrydate.AddDays(Convert.ToInt32(sqldataReader["mbershipDuration"].ToString()));
                                                expDate = entrydate.Day.ToString("00") + "/" + entrydate.Month.ToString("00") + "/" + entrydate.Year.ToString("0000");
                                                try
                                                {
                                                    brrImage = System.Drawing.Image.FromFile(Properties.Settings.Default.databasePath + @"\BorrowerImage\" + dataReader["imagePath"].ToString());
                                                }
                                                catch
                                                {
                                                    brrImage = Properties.Resources.blankBrrImage;
                                                }
                                                memberImgJpg = iTextSharp.text.Image.GetInstance(brrImage, BaseColor.WHITE);
                                            }
                                        }
                                        sqldataReader.Close();
                                        mysqlConn.Close();
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                                imgBarcode = barCode.Draw(borrowerId, 25);
                                imgQrcode = qrCode.Draw(borrowerId, 25);

                                barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                                qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                                if (rowCount == 0)
                                {
                                    rectYpos = pageHeight - 20;
                                }
                                else
                                {
                                    rectYpos = (pageHeight - (20 + (160 * rowCount)));
                                }
                                imglocY = rectYpos - 43;
                                if (columnCount % 2 == 0)
                                {
                                    rectAngle = new iTextSharp.text.Rectangle(20, rectYpos, 277.5f, rectYpos - 150.4f);
                                    imglocX = 22;
                                }
                                else
                                {
                                    rectAngle = new iTextSharp.text.Rectangle(317.5f, rectYpos, 575, rectYpos - 150.4f);
                                    imglocX = 319.5f;
                                    rowCount++;
                                }

                                rectAngle.Border = iTextSharp.text.Rectangle.BOX;//================Draw Recangle Line=====
                                rectAngle.BorderWidth = 1.5f;
                                rectAngle.BorderColor = BaseColor.DARK_GRAY;
                                pdfContent.Rectangle(rectAngle);
                                pdfContent.Stroke();

                                instLogoJpg.SetAbsolutePosition(imglocX, imglocY);//================Institute Logo=====
                                instLogoJpg.ScaleAbsolute(40f, 40f);
                                //instLogoJpg.ScalePercent(30f);
                                pdfToCreate.Add(instLogoJpg);

                                pdfContent.BeginText();//================Institute Name=====
                                pdfContent.SetColorFill(new BaseColor(Color.FromArgb(235, 68, 38)));

                                if (instName.Length <= 40)
                                {
                                    pdfContent.SetFontAndSize(baseFontBold, 10);
                                }
                                else if (instName.Length <= 43)
                                {
                                    pdfContent.SetFontAndSize(baseFontBold, 9);
                                           
                                }
                                else
                                {
                                    pdfContent.SetFontAndSize(baseFontBold, 10);
                                    instName = instShortName;
                                }
                                pdfContent.SetTextMatrix(imglocX + 45, imglocY + 25);
                                pdfContent.ShowText(instName);
                                pdfContent.EndText();
                                pdfContent.SetColorFill(new BaseColor(Color.Black));

                                pdfContent.BeginText();//================Institute Address=====
                                pdfContent.SetFontAndSize(baseFont, 7);
                                pdfContent.SetTextMatrix(imglocX + 45, imglocY + 15);
                                pdfContent.ShowText(instAddress);
                                pdfContent.EndText();

                                pdfContent.BeginText();//================Institute Address=====
                                pdfContent.SetFontAndSize(baseFont, 7);
                                pdfContent.SetTextMatrix(imglocX + 45, imglocY + 6);
                                pdfContent.ShowText("Library Membership Card");
                                pdfContent.EndText();

                                pdfContent.SetLineWidth(0.5f);//================Draw Black Line=====
                                pdfContent.MoveTo(imglocX - 2, imglocY - 3);
                                pdfContent.LineTo(imglocX - 2 + rectAngle.Width, imglocY - 3);
                                pdfContent.Stroke();

                                memberImgJpg.SetAbsolutePosition(imglocX + 3, imglocY - 77);//================Member Image=====
                                memberImgJpg.ScaleAbsolute(60f, 70f);
                                pdfToCreate.Add(memberImgJpg);

                                barcodeJpg.SetAbsolutePosition(imglocX + 3, imglocY - 93);//================Member Barcode=====
                                barcodeJpg.ScalePercent(52f);
                                pdfToCreate.Add(barcodeJpg);

                                pdfContent.BeginText();//================Member Address=====
                                pdfContent.SetFontAndSize(baseFont, 7);
                                pdfContent.SetTextMatrix(imglocX + 3, imglocY - 103);
                                pdfContent.ShowText(brrAddress);
                                pdfContent.EndText();

                                pdfContent.BeginText();//================Member Name=====
                                pdfContent.SetFontAndSize(baseFontBold, 9);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 17);
                                pdfContent.ShowText(brrName);
                                pdfContent.EndText();

                                pdfContent.SetColorFill(new BaseColor(Color.FromArgb(235, 68, 38)));
                                pdfContent.BeginText();//================Member Name=====
                                pdfContent.SetFontAndSize(baseFontBold, 9);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 30);
                                pdfContent.ShowText(brrCategory);
                                pdfContent.EndText();
                                pdfContent.SetColorFill(new BaseColor(Color.Black));

                                pdfContent.BeginText();//================Member Id=====
                                pdfContent.SetFontAndSize(baseFont, 8);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 43);
                                pdfContent.ShowText("Member Id : ");
                                pdfContent.EndText();

                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFontBold, 8);
                                pdfContent.SetTextMatrix(imglocX + 115, imglocY - 43);
                                pdfContent.ShowText(borrowerId);
                                pdfContent.EndText();

                                pdfContent.BeginText();//================Member Issue date=====
                                pdfContent.SetFontAndSize(baseFont, 8);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 57.5f);
                                pdfContent.ShowText("Issue Date : " + issueDate);
                                pdfContent.EndText();

                                pdfContent.BeginText();//================Member Expiry date=====
                                pdfContent.SetFontAndSize(baseFont, 8);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 71.5f);
                                pdfContent.ShowText("Expiry Date : " + expDate);
                                pdfContent.EndText();

                                pdfContent.BeginText();//================Member Contact=====
                                pdfContent.SetFontAndSize(baseFont, 8);
                                pdfContent.SetTextMatrix(imglocX + 70, imglocY - 85.5f);
                                pdfContent.ShowText("Contact : " + brrContact);
                                pdfContent.EndText();

                                if (chkbQr.Checked)
                                {
                                    qrcodeJpg.SetAbsolutePosition(imglocX + 191, imglocY - 93);
                                    qrcodeJpg.ScaleAbsolute(60f, 70f);
                                    //qrcodeJpg.ScalePercent(20f);
                                    pdfToCreate.Add(qrcodeJpg);
                                }
                                columnCount++;
                                if (columnCount % 10 == 0)
                                {
                                    pdfToCreate.NewPage();
                                    columnCount = 0;
                                    rowCount = 0;
                                    imglocX = 0; imglocY = 0; rectYpos = 0;
                                }
                            }
                        }
                        pdfToCreate.Close();
                        outputStream.Close();
                    }

                    if (rdbPrinter.Checked)//============Print Id Card===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Borrower id generated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try
                        {
                            Process.Start(txtbPath.Text);
                        }
                        catch
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
           
            dgvBrrList.Rows.Clear();
            chkbAll.Checked = false;
        }

        private void printA4Paper(string fileName)
        {
            // Create the printer settings for our printer
            var printerSettings = new PrinterSettings
            {
                PrinterName = txtbPrinterName.Text,
                Copies = (short)1,
                FromPage = (short)1,
            };

            // Create our page settings for the paper size selected
            var pageSettings = new PageSettings(printerSettings)
            {
                Margins = new Margins(0, 0, 0, 0),
            };
            foreach (PaperSize paperSize in printerSettings.PaperSizes)
            {
                if (paperSize.PaperName == "A4")
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

        private void BarcodePrinter5x25Cm_1x1(object sender, PrintPageEventArgs ev)
        {
            float yPos = 0;
            float leftMargin = 20.0f;
            Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            System.Drawing.Image imgBarcode;
            Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            System.Drawing.Image imgQrcode;
            Bitmap bmImage = new Bitmap(50, 50);
            Graphics grph;

            DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            yPos = printFont.GetHeight(ev.Graphics);
            foreach (DataGridViewRow dataRow in dgvCheckedRows)
            {
                borrowerId = dataRow.Cells[1].Value.ToString();
                imgBarcode = barCode.Draw(borrowerId, 25);
                imgQrcode = qrCode.Draw(borrowerId, 25);

                // Print each line of the file.
                ev.Graphics.DrawString("CodeAchi LMS", printFont, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                ev.Graphics.DrawString(DateTime.Now.ToShortDateString(), printFont, Brushes.Black,
                        leftMargin + 100, yPos, new StringFormat());

                yPos = yPos + 10;
                ev.Graphics.DrawImage(imgBarcode, leftMargin, yPos);

                yPos = yPos + imgBarcode.Height + 5;
                ev.Graphics.DrawString(borrowerId, printFontBig, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                if (chkbFooter.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString("*Dont tamper this sticker.", printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }

                if (chkbQr.Checked)
                {
                    bmImage = new Bitmap(50, 50);
                    grph = Graphics.FromImage((System.Drawing.Image)bmImage);
                    bmImage.SetResolution(imgQrcode.HorizontalResolution,
                     imgQrcode.VerticalResolution);
                    grph.InterpolationMode = InterpolationMode.High;
                    grph.DrawImage(imgQrcode, 0, 0, 50, 50);
                    ev.Graphics.DrawImage(bmImage, leftMargin + 120, yPos - 40);
                }
                yPos = yPos + 59.7f;
            }
            //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument?view=netframework-4.8
        }

        private void BarcodePrinter5x25Cm_2x1(object sender, PrintPageEventArgs ev)
        {
            float yPos = 0, tempYpos = 0; ;
            float leftMargin = 20.0f;
            int columnCount = 0;
            Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            System.Drawing.Image imgBarcode;
            Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            System.Drawing.Image imgQrcode;
            Bitmap bmImage = new Bitmap(50, 50);
            Graphics grph;

            DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            yPos = printFont.GetHeight(ev.Graphics);
            tempYpos = yPos;

            foreach (DataGridViewRow dataRow in dgvCheckedRows)
            {
                borrowerId = dataRow.Cells[1].Value.ToString();
                imgBarcode = barCode.Draw(borrowerId, 25);
                imgQrcode = qrCode.Draw(borrowerId, 25);

                // Print each line of the file.
                ev.Graphics.DrawString("CodeAchi LMS", printFont, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                ev.Graphics.DrawString(DateTime.Now.ToShortDateString(), printFont, Brushes.Black,
                        leftMargin + 100, yPos, new StringFormat());

                yPos = yPos + 10;
                ev.Graphics.DrawImage(imgBarcode, leftMargin, yPos);

                yPos = yPos + imgBarcode.Height + 5;
                ev.Graphics.DrawString(borrowerId, printFontBig, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                if (chkbFooter.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString("*Dont tamper this sticker.", printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }
                if (chkbQr.Checked)
                {
                    bmImage = new Bitmap(50, 50);
                    grph = Graphics.FromImage((System.Drawing.Image)bmImage);
                    bmImage.SetResolution(imgQrcode.HorizontalResolution,
                     imgQrcode.VerticalResolution);
                    grph.InterpolationMode = InterpolationMode.High;
                    grph.DrawImage(imgQrcode, 0, 0, 50, 50);
                    ev.Graphics.DrawImage(bmImage, leftMargin + 120, yPos - 40);
                }

                columnCount++;
                if (columnCount == 1)
                {
                    leftMargin = 222.0f;
                    yPos = tempYpos;
                }
                else
                {
                    leftMargin = 20.0f;
                    columnCount = 0;
                    yPos = yPos + 59.7f;
                    tempYpos = yPos;
                }
            }
            //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument?view=netframework-4.8
        }

        private void PdfBarcodePrinter5x25_1x1(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
               
                var pgSize = new iTextSharp.text.Rectangle(404, 100);
                Document pdfToCreate = new Document(pgSize);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;
                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                float imglocX = 15.0f, imglocY = 52;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    imgBarcode = barCode.Draw(itemAccn, 25);
                    imgQrcode = qrCode.Draw(itemAccn, 25);

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 6);
                    pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                    pdfContent.ShowText("CodeAchi LMS");
                    pdfContent.EndText();

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 6);
                    pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                    pdfContent.ShowText(DateTime.Now.ToShortDateString());
                    pdfContent.EndText();

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 8);
                    pdfContent.SetTextMatrix(imglocX, imglocY - 1);  //(xPos, yPos)
                    pdfContent.ShowText(itemAccn);
                    pdfContent.EndText();

                    if (chkbFooter.Checked)
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 8);  //(xPos, yPos)
                        pdfContent.ShowText("*Don't remove this sticker.");
                        pdfContent.EndText();
                    }

                    barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);

                    barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 7);
                    barcodeJpg.ScalePercent(70f);
                    pdfToCreate.Add(barcodeJpg);

                    if (chkbQr.Checked)
                    {
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);
                        qrcodeJpg.SetAbsolutePosition(imglocX + 80, imglocY - 8);
                        qrcodeJpg.ScalePercent(20f);
                        pdfToCreate.Add(qrcodeJpg);
                    }

                    pdfToCreate.NewPage();
                    imglocX = 15;
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void PdfBarcodePrinter5x25_2x1(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
               
                var pgSize = new iTextSharp.text.Rectangle(404, 100);
                Document pdfToCreate = new Document(pgSize);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;
                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                float imglocX = 15.0f, imglocY = 52;
                int columnCount = 0;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    imgBarcode = barCode.Draw(itemAccn, 25);
                    imgQrcode = qrCode.Draw(itemAccn, 25);

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 6);
                    pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                    pdfContent.ShowText("CodeAchi LMS");
                    pdfContent.EndText();

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 6);
                    pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                    pdfContent.ShowText(DateTime.Now.ToShortDateString());
                    pdfContent.EndText();

                    pdfContent.BeginText();
                    pdfContent.SetFontAndSize(baseFont, 8);
                    pdfContent.SetTextMatrix(imglocX, imglocY - 1);  //(xPos, yPos)
                    pdfContent.ShowText(itemAccn);
                    pdfContent.EndText();

                    if (chkbFooter.Checked)
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 8);  //(xPos, yPos)
                        pdfContent.ShowText("*Don't remove this sticker.");
                        pdfContent.EndText();
                    }

                    barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);

                    barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 7);
                    barcodeJpg.ScalePercent(70f);
                    pdfToCreate.Add(barcodeJpg);

                    if (chkbQr.Checked)
                    {
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);
                        qrcodeJpg.SetAbsolutePosition(imglocX + 80, imglocY - 8);
                        qrcodeJpg.ScalePercent(20f);
                        pdfToCreate.Add(qrcodeJpg);
                    }
                    columnCount++;
                    if (columnCount == 1)
                    {
                        imglocX = 170.0f;
                    }
                    else
                    {
                        columnCount = 0;
                        pdfToCreate.NewPage();
                        imglocX = 15;
                    }
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void BarcodeFor3x8(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                Document pdfToCreate = new Document(PageSize.A4);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;

                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                float imglocX = 0, imglocY = 0;
                float imgqrlocX = 0, imgqrlocY = 0;
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                //................................leave barcode.........................
                imgBarcode = barCode.Draw("1234567", 25);
                imgQrcode = qrCode.Draw("1234567", 25);
                for (barcodeCount = 0; barcodeCount <= leaveBarcode - 1; barcodeCount++)
                {
                    if (columnCount == 0)
                    {
                        imglocX = ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 80.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }
                    else if (columnCount == 1)
                    {
                        imglocX = (pageWidth / 3) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 80.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }
                    else if (columnCount == 2)
                    {
                        imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 65.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }

                    imglocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);
                    imgqrlocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);

                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 25.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 1)
                        {
                            imglocY = imglocY - 20.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 2)
                        {
                            imglocY = imglocY - 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 3)
                        {
                            //imglocY = imglocY - 20.0f;
                            imgqrlocY = imglocY;
                        }

                        if (rowCount == 4)
                        {
                            imglocY = imglocY + 7.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 5)
                        {
                            imglocY = imglocY + 15.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 6)
                        {
                            imglocY = imglocY + 30.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 7)
                        {
                            imglocY = imglocY + 40.0f;
                            imgqrlocY = imglocY;
                        }
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 1)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 2)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 3)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 5)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 6)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 7)
                        {
                            imglocY = imglocY + 20.0f;
                            imgqrlocY = imglocY;
                        }
                    }

                    if (columnCount == 3)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                string borrowerId = "BRC-1234567";
                DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    borrowerId = dataRow.Cells[1].Value.ToString();
                    if (borrowerId != "")
                    {
                        imgBarcode = barCode.Draw(borrowerId, 25);
                        imgQrcode = qrCode.Draw(borrowerId, 25);
                        if (columnCount == 0)
                        {
                            imglocX = ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 65.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }

                        imglocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);
                        imgqrlocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);

                        if (rdbPrinter.Checked)
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY - 25.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 1)
                            {
                                imglocY = imglocY - 20.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 2)
                            {
                                imglocY = imglocY - 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 3)
                            {
                                //imglocY = imglocY - 20.0f;
                                imgqrlocY = imglocY;
                            }

                            if (rowCount == 4)
                            {
                                imglocY = imglocY + 7.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 5)
                            {
                                imglocY = imglocY + 15.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 6)
                            {
                                imglocY = imglocY + 30.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 7)
                            {
                                imglocY = imglocY + 40.0f;
                                imgqrlocY = imglocY;
                            }
                        }
                        else
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY - 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 1)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 2)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 3)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 5)
                            {
                                imglocY = imglocY + 5.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 6)
                            {
                                imglocY = imglocY + 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 7)
                            {
                                imglocY = imglocY + 20.0f;
                                imgqrlocY = imglocY;
                            }
                        }
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height);  //(xPos, yPos)
                        pdfContent.ShowText("CodeAchi LMS");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height);  //(xPos, yPos)
                        pdfContent.ShowText(DateTime.Now.ToShortDateString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10);  //(xPos, yPos)
                        pdfContent.ShowText(borrowerId);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 17);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();

                            //pdfContent.BeginText();
                            //pdfContent.SetFontAndSize(baseFont, 6);
                            //pdfContent.SetTextMatrix(imglocX, imglocY - 24);  //(xPos, yPos)
                            //pdfContent.ShowText("by order " + orderedBy);
                            //pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - 17);
                        barcodeJpg.ScalePercent(52f);
                        qrcodeJpg.ScalePercent(20f);
                        pdfToCreate.Add(barcodeJpg);
                        if (chkbQr.Checked)
                        {
                            pdfToCreate.Add(qrcodeJpg);
                        }
                        if (columnCount == 3)
                        {
                            rowCount++;
                            columnCount = 0;
                        }
                        barcodeCount++;
                        if (barcodeCount % 24 == 0)
                        {
                            rowCount = 0;
                            pdfToCreate.NewPage();
                            imglocX = 0; imglocY = 0;
                            imgqrlocX = 0; imgqrlocY = 0;
                        }
                    }
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void BarcodeFor3x10(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                Document pdfToCreate = new Document(PageSize.A4);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;

                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                float imglocX = 0, imglocY = 0;
                float imgqrlocX = 0, imgqrlocY = 0;
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                //................................leave barcode.........................
                imgBarcode = barCode.Draw("1234567", 25);
                imgQrcode = qrCode.Draw("1234567", 25);
                for (barcodeCount = 0; barcodeCount <= leaveBarcode - 1; barcodeCount++)
                {
                    if (columnCount == 0)
                    {
                        imglocX = ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 80.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }
                    else if (columnCount == 1)
                    {
                        imglocX = (pageWidth / 3) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 80.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }
                    else if (columnCount == 2)
                    {
                        imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                        imglocX = imglocX + 65.0f;
                        imgqrlocX = imglocX + 90.0f;
                        columnCount++;
                    }

                    imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                    imgqrlocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);

                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 4)
                        {
                            imglocY = imglocY + 2.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 5)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 6)
                        {
                            imglocY = imglocY + 8.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 7)
                        {
                            imglocY = imglocY + 11.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 8)
                        {
                            imglocY = imglocY + 15.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 9)
                        {
                            imglocY = imglocY + 18.0f;
                            imgqrlocY = imglocY;
                        }
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 1)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 2)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        if (rowCount == 3)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                    }

                    if (columnCount == 3)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                string borrowerId = "BRC-1234567";
                DataGridViewRow[] dgvCheckedRows = dgvBrrList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    borrowerId = dataRow.Cells[1].Value.ToString();
                    if (borrowerId != "")
                    {
                        imgBarcode = barCode.Draw(borrowerId, 25);
                        imgQrcode = qrCode.Draw(borrowerId, 25);
                        if (columnCount == 0)
                        {
                            imglocX = ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (imgBarcode.Width + imgQrcode.Width)) / 3;
                            imglocX = imglocX + 65.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }

                        imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                        imgqrlocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);

                        if (rdbPrinter.Checked)
                        {
                            if (rowCount == 4)
                            {
                                imglocY = imglocY + 2.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 5)
                            {
                                imglocY = imglocY + 5.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 6)
                            {
                                imglocY = imglocY + 8.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 7)
                            {
                                imglocY = imglocY + 11.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 8)
                            {
                                imglocY = imglocY + 15.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 9)
                            {
                                imglocY = imglocY + 18.0f;
                                imgqrlocY = imglocY;
                            }
                        }
                        else
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY + 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 1)
                            {
                                imglocY = imglocY + 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 2)
                            {
                                imglocY = imglocY + 10.0f;
                                imgqrlocY = imglocY;
                            }
                            if (rowCount == 3)
                            {
                                imglocY = imglocY + 5.0f;
                                imgqrlocY = imglocY;
                            }
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height);  //(xPos, yPos)
                        pdfContent.ShowText("CodeAchi LMS");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height);  //(xPos, yPos)
                        pdfContent.ShowText(DateTime.Now.ToShortDateString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10);  //(xPos, yPos)
                        pdfContent.ShowText(borrowerId);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 17);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();

                            //pdfContent.BeginText();
                            //pdfContent.SetFontAndSize(baseFont, 6);
                            //pdfContent.SetTextMatrix(imglocX, imglocY - 24);  //(xPos, yPos)
                            //pdfContent.ShowText("by order " + orderedBy);
                            //pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - 17);
                        barcodeJpg.ScalePercent(52f);
                        qrcodeJpg.ScalePercent(20f);
                        pdfToCreate.Add(barcodeJpg);
                        if (chkbQr.Checked)
                        {
                            pdfToCreate.Add(qrcodeJpg);
                        }
                        if (columnCount == 3)
                        {
                            rowCount++;
                            columnCount = 0;
                        }
                        barcodeCount++;
                        if (barcodeCount % 30 == 0)
                        {
                            rowCount = 0;
                            pdfToCreate.NewPage();
                            imglocX = 0; imglocY = 0;
                            imgqrlocX = 0; imgqrlocY = 0;
                        }
                    }
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void rdbPdf_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPdf.Checked)
            {
                rdbPrinter.Checked = false;
                btnBrowse.Enabled = true;
                btnPrinter.Enabled = false;
                txtbPrinterName.Clear();
                btnPrint.Text = "Save";
            }
        }

        private void rdbPrinter_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPrinter.Checked)
            {
                rdbPdf.Checked = false;
                btnBrowse.Enabled = false;
                btnPrinter.Enabled = true;
                txtbPath.Clear();
                btnPrint.Text = "Print";
                txtbPrinterName.Text = Properties.Settings.Default.icardPrinter;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileName = new SaveFileDialog();
            fileName.Filter = "Pdf Files (*.pdf)|*.pdf";
            fileName.DefaultExt = "pdf";
            fileName.AddExtension = true;
            if (fileName.ShowDialog() == DialogResult.OK)
            {
                txtbPath.Text = fileName.FileName;
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void btnPrinter_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                txtbPrinterName.Text = printDialog.PrinterSettings.PrinterName;
                Properties.Settings.Default.icardPrinter= printDialog.PrinterSettings.PrinterName;
                Properties.Settings.Default.Save();
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataRow in dgvBrrList.Rows)
            {
                if (chkbAll.Checked)
                {
                    dataRow.Cells[0].Value = true;
                    dgvBrrList.CurrentCell = dataRow.Cells[1];
                }
                else
                {
                    dataRow.Cells[0].Value = false;
                    dgvBrrList.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
        }

        private void dtpFrom_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblBlink.Visible = !lblBlink.Visible;
        }

        private void cmbPaper_SelectedIndexChanged(object sender, EventArgs e)
        {
            numUpBlock.Value = 0;
            if(cmbPaper.SelectedIndex!=-1)
            {
                timer2.Stop();
                lblBlink.Visible = false;
                lblMessage.Visible = false;
                if (cmbPaper.SelectedIndex == 0 || cmbPaper.SelectedIndex == 1)
                {
                    numUpBlock.Enabled = true;
                }
                else
                {
                    numUpBlock.Enabled = false;
                }
                Properties.Settings.Default.icardPaper = cmbPaper.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                numUpBlock.Enabled = false;
            }
        }

        private void cmbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbOption.Text== "Print Id Card")
            {
                cmbPaper.Enabled = false;
                cmbPaper.SelectedIndex = -1;
                chkbFooter.Checked = false;
                chkbFooter.Enabled = false;
                chkbQr.Checked = false;
                chkbQr.Enabled = true;
            }
            else if(cmbOption.Text== "Print Barcode")
            {
                cmbPaper.Text = Properties.Settings.Default.icardPaper;
                cmbPaper.Enabled = true;
                cmbPaper.SelectedIndex = 0;
                chkbFooter.Enabled = true;
                chkbFooter.Checked = true;
                chkbQr.Checked = false;
            }
        }

        private void dgvBrrList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlRecord.Text = dgvBrrList.Rows.Count.ToString();
            btnPrint.Enabled = true;
        }

        private void dgvBrrList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTtlRecord.Text = dgvBrrList.Rows.Count.ToString();
            btnPrint.Enabled = false;
        }

        private void btnBrowse_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnBrowse.Enabled == true)
            {
                btnBrowse.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnBrowse.BackColor = Color.DimGray;
            }
        }

        private void btnPrinter_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnPrinter.Enabled == true)
            {
                btnPrinter.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnPrinter.BackColor = Color.DimGray;
            }
        }

        private void btnPrint_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnPrint.Enabled == true)
            {
                btnPrint.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnPrint.BackColor = Color.DimGray;
            }
        }
    }
}
