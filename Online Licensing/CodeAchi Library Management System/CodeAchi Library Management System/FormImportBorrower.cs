using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormImportBorrower : Form
    {
        public FormImportBorrower()
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

        int lastNumber = 0, validDays = 0, issueLimit = 0;
        double memFees = 0.00, totalFees = 0.00;
        bool isAutoGenerate = false, isManualPrefix = false; string prefixText = "", joiningChar = "";
        List<string> brrIdList = new List<string> { };

        private void FormImportExcelBorrower_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void FormImportBorrower_Load(object sender, EventArgs e)
        {
            cmbBrrCategory.Items.Clear();
            cmbBrrCategory.Items.Add("-------Select-------");
            cmbPlan.Items.Add("--Select--");
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //======================Item Category add to combobox============
                string queryString = "select catName from borrowerSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
                    }
                }
                dataReader.Close();

                queryString = "select membrshpName from mbershipSetting";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbPlan.Items.Add(dataReader["membrshpName"].ToString());
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
                            cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
                        }
                    }
                    dataReader.Close();

                    queryString = "select membrshpName from mbership_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbPlan.Items.Add(dataReader["membrshpName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            cmbPlan.SelectedIndex = 0;
            cmbBrrCategory.SelectedIndex = 0;
            rdbAuto.Checked = false;
            rdbManual.Checked = false;
            btnImport.Enabled = false;
            if (FieldSettings.Default.borrowerEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        if (fieldName == "lblBrCategory")
                        {
                            lblBrCategory.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            if (cmbBrrCategory.SelectedIndex == -1 || cmbBrrCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbBrrCategory.Select();
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
                saveDialog.FileName = cmbBrrCategory.Text + "_" + rdbAuto.Text;
            }
            else
            {
                saveDialog.FileName = cmbBrrCategory.Text + "_" + rdbManual.Text;
            }

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                string capInfo = "";
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select [capInfo] from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
                    string queryString = "select capInfo from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
                string columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days),Membership Fees,Opak Permission(True/False),Image Name";
                if (capInfo != "" && capInfo != null)
                {
                    if (addInfoList.Length == 1)
                    {
                        columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") +
                        ",Membership Fees,Opak Permission(True/False),Image Name";
                    }
                    else if (addInfoList.Length == 2)
                    {
                        columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                       addInfoList[1].Replace("2_", "") + ",Membership Fees,Opak Permission(True/False),Image Name";
                    }
                    else if (addInfoList.Length == 3)
                    {
                        columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                        addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + ",Membership Fees,Opak Permission(True/False),Image Name";
                    }
                    else if (addInfoList.Length == 4)
                    {
                        columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                        addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") + ",Membership Fees,Opak Permission(True/False),Image Name";
                    }
                    else if (addInfoList.Length == 5)
                    {
                        columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                        addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") + "," + addInfoList[4].Replace("5_", "") +
                        ",Membership Fees,Opak Permission(True/False),Image Name";
                    }
                }
                if (!rdbAuto.Checked)
                {
                    columnHeader = "Member Id," + columnHeader;
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
            if (cmbBrrCategory.SelectedIndex == -1 || cmbBrrCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbBrrCategory.Select();
                return;
            }
            if (!rdbManual.Checked && !rdbAuto.Checked)
            {
                MessageBox.Show("Please select id type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string capInfo = "";
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select [capInfo] from borrowerSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
                string queryString = "select capInfo from borrower_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
            string columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days),Membership Fees,Opak Permission(True/False),Image Name";
            if (capInfo != "" && capInfo != null)
            {
                if (addInfoList.Length == 1)
                {
                    columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") +
                    ",Membership Fees,Opak Permission(True/False),Image Name";
                }
                else if (addInfoList.Length == 2)
                {
                    columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                   addInfoList[1].Replace("2_", "") + ",Membership Fees,Opak Permission(True/False),Image Name";
                }
                else if (addInfoList.Length == 3)
                {
                    columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                    addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + ",Membership Fees,Opak Permission(True/False),Image Name";
                }
                else if (addInfoList.Length == 4)
                {
                    columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                    addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") +
                    ",Membership Fees,Opak Permission(True/False),Image Name";
                }
                else if (addInfoList.Length == 5)
                {
                    columnHeader = "Name,Address,Gender,Mail Id,Contact No,Membership Duration (days)," + addInfoList[0].Replace("1_", "") + "," +
                    addInfoList[1].Replace("2_", "") + "," + addInfoList[2].Replace("3_", "") + "," + addInfoList[3].Replace("4_", "") + "," + addInfoList[4].Replace("5_", "") + "," +
                    ",Membership Fees,Opak Permission(True/False),Image Name";
                }
            }
            if (!rdbAuto.Checked)
            {
                columnHeader = "Member Id," + columnHeader;
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

        private void rdbManual_CheckedChanged(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
        }

        private void cmbBrrCategory_SelectedIndexChanged(object sender, EventArgs e)
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

        private void txtbFreqncy_TextChanged(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text != "")
            {
                txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                totalFees = memFees * Convert.ToInt32(txtbFreqncy.Text);
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

        private void cmbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                totalFees = memFees * Convert.ToInt32(txtbFreqncy.Text);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Properties.Settings.Default.databasePath + @"\BorrowerImage"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.databasePath + @"\BorrowerImage");
                }
                if (cmbPlan.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a membership plan.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string defaultPass = "";
                string fileName = txtbFileName.Text;
                btnImport.Enabled = false;
                IntPtr hMenu = GetSystemMenu(this.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                int rowCount = 0;
                string capInfo = "", queryString = "";
                SQLiteConnection sqltConn = null;
                SQLiteCommand sqltCommnd = null;
                SQLiteDataReader dataReader = null;
                MySqlConnection mysqlConn = null;
                MySqlCommand mysqlCmd = null;
                MySqlDataReader sqldataReader = null;

                if (globalVarLms.sqliteData)
                {
                    sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "select capInfo,defaultPass from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            defaultPass = dataReader["defaultPass"].ToString();
                            capInfo = dataReader["capInfo"].ToString();
                        }
                    }
                    dataReader.Close();

                    queryString = "select [invidCount] from paymentDetails order by [invidCount] desc limit 1";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                        }
                    }
                    dataReader.Close();
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
                    queryString = "select capInfo,defaultPass from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    sqldataReader = mysqlCmd.ExecuteReader();
                    if (sqldataReader.HasRows)
                    {
                        while (sqldataReader.Read())
                        {
                            defaultPass = sqldataReader["defaultPass"].ToString();
                            capInfo = sqldataReader["capInfo"].ToString();
                        }
                    }
                    sqldataReader.Close();

                    queryString = "select invidCount from payment_details order by invidCount desc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    sqldataReader = mysqlCmd.ExecuteReader();
                    if (sqldataReader.HasRows)
                    {
                        while (sqldataReader.Read())
                        {
                            rowCount = Convert.ToInt32(sqldataReader["invidCount"].ToString());
                        }
                    }
                    sqldataReader.Close();
                    mysqlConn.Close();
                }
                string brrId = "", brrName, brrCategory = cmbBrrCategory.Text, brrAddress, brrGender, brrMail, brrContact, entryDate,
                   memDuration, addInfo1 = "", addInfo2 = "", addInfo3 = "", addInfo4 = "", addInfo5 = "",
                    membershipFees = "", imgPath = "", opakPermission = "", memPlan = cmbPlan.Text;
                entryDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");

                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(txtbFileName.Text);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                int dataFound = xlRange.Rows.Count - 1;
                if (dataFound == 0)
                {
                    MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnImport.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                }
                else
                {
                    lblTotal.Text = dataFound.ToString();
                    progressBar1.Maximum = dataFound;
                    string[] addInfoList = capInfo.Split('$');
                    List<string> existData = new List<string> { };
                    int itemCount = 0;
                    if (rdbAuto.Checked)
                    {
                        if (!isManualPrefix) //Manual prefix
                        {
                            if (brrCategory.Length < 3)
                            {
                                prefixText = brrCategory.ToUpper();
                            }
                            else
                            {
                                prefixText = brrCategory.ToUpper();
                                prefixText = prefixText.Substring(0, 3);
                            }
                        }
                        int existCount = 0;
                        //iterate over the rows and columns and print to the console as it appears in the file
                        //excel is not zero based!!
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            brrName = xlRange.Cells[i, "A"].Value2.ToString();
                            existCount = 0;
                            if (globalVarLms.sqliteData)
                            {
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
                                brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                                brrId = brrId.Replace(" ", "");

                                sqltCommnd.CommandText = "select count(brrId) from borrowerDetails where brrId=@brrId";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                                existCount = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                            }
                            else
                            {
                                mysqlConn = ConnectionClass.mysqlConnection();
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                queryString = "select brrId from borrower_details where brrCategory=@catName and brrId like @brrId order by id desc limit 1";    //  
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                                mysqlCmd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                                mysqlCmd.CommandTimeout = 99999;
                                sqldataReader = mysqlCmd.ExecuteReader();
                                if (sqldataReader.HasRows)
                                {
                                    while (sqldataReader.Read())
                                    {
                                        brrId = sqldataReader["brrId"].ToString();
                                    }
                                    sqldataReader.Close();

                                    queryString = "select brrId from borrower_details where brrCategory=@catName and brrId like @brrId";    //  
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", prefixText + joiningChar + "%");
                                    mysqlCmd.CommandTimeout = 99999;
                                    sqldataReader = mysqlCmd.ExecuteReader();
                                    if (sqldataReader.HasRows)
                                    {
                                        brrIdList = (from IDataRecord r in sqldataReader
                                                     select (string)r["brrId"]).ToList();
                                    }
                                    sqldataReader.Close();

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
                                    sqldataReader.Close();
                                    lastNumber = 1;
                                }
                                brrId = prefixText + joiningChar + lastNumber.ToString("00000");
                                brrId = brrId.Replace(" ", "");

                                queryString = "select count(brrId) from borrower_details where brrId=@brrId";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                                mysqlCmd.CommandTimeout = 99999;
                                existCount = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                            }
                            if (existCount > 0)
                            {
                                existData.Add("Borrower Id : " + brrId + " Name : " + brrName + " [ not added due to duplicate borrower id]");
                            }
                            else
                            {
                                brrAddress = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                                brrGender = xlRange.Cells[i, "C"].Value2 != null ? xlRange.Cells[i, "C"].Value2.ToString() : "Others";
                                brrMail = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "";
                                brrContact = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                memDuration = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : 0.ToString();
                                if (capInfo != "" && capInfo != null)
                                {
                                    if (addInfoList.Length == 1)
                                    {
                                        addInfo1 = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                        membershipFees = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 2)
                                    {
                                        addInfo1 = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                        membershipFees = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 3)
                                    {
                                        addInfo1 = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        membershipFees = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 4)
                                    {
                                        addInfo1 = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        membershipFees = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : null;
                                    }
                                    else if (addInfoList.Length == 5)
                                    {
                                        addInfo1 = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : "";
                                        addInfo2 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                        addInfo3 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                        addInfo4 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                        addInfo5 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                        membershipFees = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : null;
                                    }
                                }
                                else
                                {
                                    membershipFees = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : 0.ToString();
                                    opakPermission = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : false.ToString();
                                    imgPath = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : null;
                                }



                                membershipFees = totalFees.ToString();
                                memDuration = txtbBrrRnwDate.Text;
                                if (globalVarLms.sqliteData)
                                {
                                    Image brrImage = null;
                                    String imgName = "";
                                    if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                    {
                                        brrImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                        imgName = Properties.Settings.Default.databasePath + @"\BorrowerImage\" + brrId + ".jpg";
                                        if (!File.Exists(imgName))
                                        {
                                            brrImage.Save(imgName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        imgName = brrId + ".jpg";
                                    }
                                    //====================Insert Borrower details======================
                                    sqltCommnd = sqltConn.CreateCommand();
                                    queryString = "INSERT INTO borrowerDetails (brrId,brrName,brrCategory,brrAddress,brrGender," +
                                        "brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                                        "entryDate,opkPermission,imagePath,brrPass,memPlan,memFreq) VALUES (@brrId,@brrName,@brrCategory," +
                                        "@brrAddress,'" + brrGender + "',@brrMailId,@brrContact" +
                                        ",'" + memDuration + "',@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5" +
                                        ",'" + entryDate + "','" + opakPermission + "','" + imgName + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "');";
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                                    sqltCommnd.Parameters.AddWithValue("@brrName", brrName);
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", brrCategory);
                                    sqltCommnd.Parameters.AddWithValue("@brrAddress", brrAddress);
                                    sqltCommnd.Parameters.AddWithValue("@brrMailId", brrMail);
                                    sqltCommnd.Parameters.AddWithValue("@brrContact", brrContact);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                    sqltCommnd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                    sqltCommnd.Parameters.AddWithValue("@brrPass", defaultPass);
                                    sqltCommnd.Parameters.AddWithValue("@memPlan", memPlan);
                                    sqltCommnd.ExecuteNonQuery();
                                }
                                else
                                {
                                    Image brrImage = null;
                                    String base64String = "base64String";
                                    try
                                    {
                                        if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                        {
                                            brrImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                            using (MemoryStream memoryStream = new MemoryStream())
                                            {
                                                brrImage.Save(memoryStream, brrImage.RawFormat);
                                                byte[] imageBytes = memoryStream.ToArray();
                                                base64String = Convert.ToBase64String(imageBytes);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        base64String = "base64String";
                                    }
                                    queryString = "INSERT INTO borrower_details (brrId,brrName,brrCategory,brrAddress,brrGender," +
                                        "brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                                        "entryDate,opkPermission,brrImage,brrPass,memPlan,memFreq) VALUES (@brrId,@brrName,@brrCategory," +
                                        "@brrAddress,'" + brrGender + "',@brrMailId,@brrContact" +
                                        ",'" + memDuration + "',@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5" +
                                        ",'" + entryDate + "','" + opakPermission + "','" + base64String + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "');";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                                    mysqlCmd.Parameters.AddWithValue("@brrName", brrName);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", brrCategory);
                                    mysqlCmd.Parameters.AddWithValue("@brrAddress", brrAddress);
                                    mysqlCmd.Parameters.AddWithValue("@brrMailId", brrMail);
                                    mysqlCmd.Parameters.AddWithValue("@brrContact", brrContact);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                    mysqlCmd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                    mysqlCmd.Parameters.AddWithValue("@brrPass", defaultPass);
                                    mysqlCmd.Parameters.AddWithValue("@memPlan", memPlan);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();
                                    mysqlConn.Close();
                                }
                                if (Convert.ToDouble(membershipFees) > 0)
                                {
                                    rowCount++;
                                    string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                                    queryString = "Insert Into paymentDetails (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt)" +
                                            "values ('" + entryDate + "','" + invId + "','" + brrId + "','" + "Membership fees" + "','" + membershipFees + "','" + true + "'," +
                                            "'" + entryDate + "','" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + 0 + "')";
                                }

                                itemCount++;
                                progressBar1.Value = itemCount;
                                lblItem.Text = itemCount.ToString();
                                Application.DoEvents();
                            }
                        }

                        if (lastNumber > 0)
                        {
                            if (globalVarLms.sqliteData)
                            {
                                queryString = "update brrIdSetting  set lastNumber='" + lastNumber + "'";
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
                                queryString = "update brrid_setting  set lastNumber='" + lastNumber + "'";
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
                        //iterate over the rows and columns and print to the console as it appears in the file
                        //excel is not zero based!!
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            existCount = 0;
                            brrId = xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            if (brrId != "")
                            {
                                brrName = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                                if (globalVarLms.sqliteData)
                                {
                                    sqltCommnd.CommandText = "select count(brrId) from borrowerDetails where brrId=@brrId";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", brrId.ToUpper());
                                    existCount = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                }
                                else
                                {
                                    if (mysqlConn.State == ConnectionState.Closed)
                                    {
                                        mysqlConn.Open();
                                    }
                                    queryString = "select count(brrId) from borrower_details where brrId=@brrId";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId.ToUpper());
                                    mysqlCmd.CommandTimeout = 99999;
                                    existCount = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                }

                                if (existCount > 0)
                                {
                                    existData.Add("Borrower Id : " + brrId + " Name : " + brrName + " [ not added due to duplicate borrower id]");
                                }
                                else
                                {
                                    brrName = xlRange.Cells[i, "B"].Value2.ToString();
                                    brrAddress = xlRange.Cells[i, "C"].Value2 != null ? xlRange.Cells[i, "C"].Value2.ToString() : "";
                                    brrGender = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "Others";
                                    brrMail = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                    brrContact = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : "";
                                    memDuration = xlRange.Cells[i, "G"].Value2 != null ? xlRange.Cells[i, "G"].Value2.ToString() : 0.ToString();
                                    if (capInfo != "" && capInfo != null)
                                    {
                                        if (addInfoList.Length == 1)
                                        {
                                            addInfo1 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                            membershipFees = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : 0.ToString();
                                            opakPermission = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : false.ToString();
                                            imgPath = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 2)
                                        {
                                            addInfo1 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                            membershipFees = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : 0.ToString();
                                            opakPermission = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : false.ToString();
                                            imgPath = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 3)
                                        {
                                            addInfo1 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            membershipFees = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : 0.ToString();
                                            opakPermission = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : false.ToString();
                                            imgPath = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 4)
                                        {
                                            addInfo1 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            membershipFees = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : 0.ToString();
                                            opakPermission = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : false.ToString();
                                            imgPath = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : null;
                                        }
                                        else if (addInfoList.Length == 5)
                                        {
                                            addInfo1 = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : "";
                                            addInfo2 = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : "";
                                            addInfo3 = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : "";
                                            addInfo4 = xlRange.Cells[i, "K"].Value2 != null ? xlRange.Cells[i, "K"].Value2.ToString() : "";
                                            addInfo5 = xlRange.Cells[i, "L"].Value2 != null ? xlRange.Cells[i, "L"].Value2.ToString() : "";
                                            membershipFees = xlRange.Cells[i, "M"].Value2 != null ? xlRange.Cells[i, "M"].Value2.ToString() : 0.ToString();
                                            opakPermission = xlRange.Cells[i, "N"].Value2 != null ? xlRange.Cells[i, "N"].Value2.ToString() : false.ToString();
                                            imgPath = xlRange.Cells[i, "O"].Value2 != null ? xlRange.Cells[i, "O"].Value2.ToString() : null;
                                        }
                                    }
                                    else
                                    {
                                        membershipFees = xlRange.Cells[i, "H"].Value2 != null ? xlRange.Cells[i, "H"].Value2.ToString() : 0.ToString();
                                        opakPermission = xlRange.Cells[i, "I"].Value2 != null ? xlRange.Cells[i, "I"].Value2.ToString() : false.ToString();
                                        imgPath = xlRange.Cells[i, "J"].Value2 != null ? xlRange.Cells[i, "J"].Value2.ToString() : null;
                                    }

                                    membershipFees = totalFees.ToString();
                                    memDuration = txtbBrrRnwDate.Text;
                                    if (globalVarLms.sqliteData)
                                    {
                                        Image brrImage = null;
                                        String imgName = "";
                                        if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                        {
                                            brrImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                            imgName = Properties.Settings.Default.databasePath + @"\BorrowerImage\" + brrId.ToUpper() + ".jpg";
                                            if (!File.Exists(imgName))
                                            {
                                                brrImage.Save(imgName, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            imgName = brrId.ToUpper() + ".jpg";
                                        }
                                        //====================Insert Borrower details======================
                                        sqltCommnd = sqltConn.CreateCommand();
                                        queryString = "INSERT INTO borrowerDetails (brrId,brrName,brrCategory,brrAddress,brrGender," +
                                            "brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                                            "entryDate,opkPermission,imagePath,brrPass,memPlan,memFreq) VALUES (@brrId,@brrName,@brrCategory," +
                                            "@brrAddress,'" + brrGender + "',@brrMailId,@brrContact" +
                                            ",'" + memDuration + "',@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5" +
                                            ",'" + entryDate + "','" + opakPermission + "','" + imgName + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "');";
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId.ToUpper());
                                        sqltCommnd.Parameters.AddWithValue("@brrName", brrName);
                                        sqltCommnd.Parameters.AddWithValue("@brrCategory", brrCategory);
                                        sqltCommnd.Parameters.AddWithValue("@brrAddress", brrAddress);
                                        sqltCommnd.Parameters.AddWithValue("@brrMailId", brrMail);
                                        sqltCommnd.Parameters.AddWithValue("@brrContact", brrContact);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                        sqltCommnd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                        sqltCommnd.Parameters.AddWithValue("@brrPass", defaultPass);
                                        sqltCommnd.Parameters.AddWithValue("@memPlan", memPlan);
                                        sqltCommnd.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        Image brrImage = null;
                                        String base64String = "base64String";
                                        try
                                        {
                                            if (File.Exists(txtbImage.Text + @"\" + imgPath))
                                            {
                                                brrImage = Image.FromFile(txtbImage.Text + @"\" + imgPath);
                                                using (MemoryStream memoryStream = new MemoryStream())
                                                {
                                                    brrImage.Save(memoryStream, brrImage.RawFormat);
                                                    byte[] imageBytes = memoryStream.ToArray();
                                                    base64String = Convert.ToBase64String(imageBytes);
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            base64String = "base64String";
                                        }
                                        queryString = "INSERT INTO borrower_details (brrId,brrName,brrCategory,brrAddress,brrGender," +
                                            "brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2,addInfo3,addInfo4,addInfo5," +
                                            "entryDate,opkPermission,brrImage,brrPass,memPlan,memFreq) VALUES (@brrId,@brrName,@brrCategory," +
                                            "@brrAddress,'" + brrGender + "',@brrMailId,@brrContact" +
                                            ",'" + memDuration + "',@addInfo1,@addInfo2,@addInfo3,@addInfo4,@addInfo5" +
                                            ",'" + entryDate + "','" + opakPermission + "','" + base64String + "',@brrPass,@memPlan,'" + txtbFreqncy.Text + "');";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId.ToUpper());
                                        mysqlCmd.Parameters.AddWithValue("@brrName", brrName);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", brrCategory);
                                        mysqlCmd.Parameters.AddWithValue("@brrAddress", brrAddress);
                                        mysqlCmd.Parameters.AddWithValue("@brrMailId", brrMail);
                                        mysqlCmd.Parameters.AddWithValue("@brrContact", brrContact);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo1", addInfo1);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo2", addInfo2);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo3", addInfo3);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo4", addInfo4);
                                        mysqlCmd.Parameters.AddWithValue("@addInfo5", addInfo5);
                                        mysqlCmd.Parameters.AddWithValue("@brrPass", defaultPass);
                                        mysqlCmd.Parameters.AddWithValue("@memPlan", memPlan);
                                        mysqlCmd.CommandTimeout = 99999;
                                        mysqlCmd.ExecuteNonQuery();
                                        mysqlConn.Close();
                                    }
                                    if (Convert.ToDouble(membershipFees) > 0)
                                    {
                                        rowCount++;
                                        string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                                        queryString = "Insert Into paymentDetails (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt)" +
                                                "values ('" + entryDate + "','" + invId + "','" + brrId + "','" + "Membership fees" + "','" + membershipFees + "','" + true + "'," +
                                                "'" + entryDate + "','" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + 0 + "')";
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
                    MessageBox.Show("Borrower successfully imported.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (existData.Count > 0)
                    {
                        File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for import borrower.txt", existData);
                        try
                        {
                            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CodeAchi LMS log for import borrower.txt");
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
                //MessageBox.Show("Please close the excel file and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnImport.Enabled = true;
            }
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAuto.Checked)
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
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
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
                    string queryString = "select * from brrid_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            isAutoGenerate = Convert.ToBoolean(dataReader["isAutoGenerate"].ToString());
                            isManualPrefix = Convert.ToBoolean(dataReader["isManualPrefix"].ToString());
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
                    MessageBox.Show("Please change borrower id setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (globalVarLms.isAdmin)
                    {
                        FormBrrIdSetting idSetting = new FormBrrIdSetting();
                        idSetting.grpbAuto.Enabled = false;
                        idSetting.rdbAuto.Enabled = true;
                        idSetting.ShowDialog();
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
    }
}
