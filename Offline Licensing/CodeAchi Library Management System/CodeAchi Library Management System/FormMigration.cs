using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
    public partial class FormMigration : Form
    {
        public FormMigration()
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

        private void FormMigration_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (rdbIssue.Checked)
            {
                IssuedDataMigration(txtbFileName.Text);
            }
            else if (rdbReissue.Checked)
            {
                ReissuedDataMigration(txtbFileName.Text);
            }
            else if (rdbReturn.Checked)
            {
                ReturnDataMigration(txtbFileName.Text);
            }
        }

        private void IssuedDataMigration(string fileName)
        {
            try
            {
                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(txtbFileName.Text);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                int dataFound = xlRange.Rows.Count - 1;
                if (dataFound == 0)
                {
                    MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    btnImport.Enabled = false;
                    IntPtr hMenu = GetSystemMenu(this.Handle, false);
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    lblTotal.Text = dataFound.ToString();
                    progressBar1.Maximum = dataFound;
                    int itemCount = 0;
                    DateTime tempDate;
                    string brrId, itemAccession, issueDate, expectedReturnDate, issueComment, issuedBy;
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = null;//iterate over the rows and columns and print to the console as it appears in the file
                                                        //excel is not zero based!!
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            brrId = xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            itemAccession = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                            if (brrId != "" && itemAccession != "")
                            {
                                try
                                {
                                    tempDate = xlRange.Cells[i, "C"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "C"].Value2) : DateTime.Now.Date;
                                    issueDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    tempDate = xlRange.Cells[i, "D"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "D"].Value2) : DateTime.Now.Date;
                                    expectedReturnDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    issueComment = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                    issuedBy = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : "";

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "insert into issuedItem (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issueComment,issuedBy)" +
                                        "values (@brrId,@itemAccession,'" + issueDate + "','" + expectedReturnDate + "','" + false + "',@issueComment,@issuedBy)";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    sqltCommnd.Parameters.AddWithValue("@issueComment", issueComment);
                                    sqltCommnd.Parameters.AddWithValue("@issuedBy", issuedBy);
                                    sqltCommnd.ExecuteNonQuery();

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "update itemDetails set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    sqltCommnd.ExecuteNonQuery();

                                    itemCount++;
                                    progressBar1.Value = itemCount;
                                    lblItem.Text = itemCount.ToString();
                                }
                                catch
                                {

                                }
                            }
                        }
                        sqltConn.Close();
                    }
                    else
                    {
                        MySqlConnection mysqlConn = null;
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
                                btnImport.Enabled = true;
                                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                                txtbFileName.Clear();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                                xlWorkbook.Close(true);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                                xlApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                                return;
                            }
                        }
                        MySqlCommand mysqlCmd = null;
                        string queryString = "";

                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            brrId = xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            itemAccession = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                            if (brrId != "" && itemAccession != "")
                            {
                                try
                                {
                                    tempDate = xlRange.Cells[i, "C"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "C"].Value2) : DateTime.Now.Date;
                                    issueDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    tempDate = xlRange.Cells[i, "D"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "D"].Value2) : DateTime.Now.Date;
                                    expectedReturnDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    issueComment = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";
                                    issuedBy = xlRange.Cells[i, "F"].Value2 != null ? xlRange.Cells[i, "F"].Value2.ToString() : "";

                                    queryString = "insert into issued_item (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issueComment,issuedBy)" +
                                        "values (@brrId,@itemAccession,'" + issueDate + "','" + expectedReturnDate + "','" + false + "',@issueComment,@issuedBy)";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    mysqlCmd.Parameters.AddWithValue("@issueComment", issueComment);
                                    mysqlCmd.Parameters.AddWithValue("@issuedBy", issuedBy);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();

                                    queryString = "update item_details set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();

                                    itemCount++;
                                    progressBar1.Value = itemCount;
                                    lblItem.Text = itemCount.ToString();
                                }
                                catch
                                {

                                }
                            }
                        }
                        mysqlConn.Close();
                    }
                    btnImport.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                    txtbFileName.Clear();
                    globalVarLms.backupRequired = true;
                    MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                    xlWorkbook.Close(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                }
            }
            catch
            {
                MessageBox.Show("Please close the csv file and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnImport.Enabled = true;
            }
        }

        private void ReissuedDataMigration(string fileName)
        {
            try
            { 
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = null;
                if (File.ReadAllLines(fileName).Length == 1)
                {
                    MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                btnImport.Enabled = false;
                IntPtr hMenu = GetSystemMenu(this.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                int ttlData = File.ReadAllLines(fileName).Length - 1;
                lblTotal.Text =ttlData.ToString();
                progressBar1.Maximum = ttlData;
                DateTime tempDate;
                string brrId, itemAccession, reissueDate, expectedReturnDate, reissueComment, reissuedBy;
                //using (TextReader fileReader = File.OpenText(fileName))
                //{
                //    var csvReader = new CsvReader(fileReader);
                //    csvReader.Configuration.HasHeaderRecord = true;

                //    int itemCount = 0;
                //    while (csvReader.Read())
                //    {

                //        csvReader.TryGetField<string>(0, out brrId);
                //        csvReader.TryGetField<string>(1, out itemAccession);
                //        if (brrId != "Member Id" && itemAccession != "Item Accession")
                //        {
                //            try
                //            {
                //                csvReader.TryGetField<DateTime>(2, out tempDate);
                //                //tempDate = Convert.ToDateTime(reissueDate);
                //                reissueDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                //                csvReader.TryGetField<DateTime>(3, out tempDate);
                //                //tempDate = Convert.ToDateTime(expectedReturnDate);
                //                expectedReturnDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                //                csvReader.TryGetField<string>(4, out reissueComment);
                //                csvReader.TryGetField<string>(5, out reissuedBy);
                //                Application.DoEvents();

                //                sqltCommnd = sqltConn.CreateCommand();
                //                sqltCommnd.CommandText = "update issuedItem set reissuedDate='"+ reissueDate + "',expectedReturnDate='"+ expectedReturnDate+ "',"+
                //                    "reissuedComment=:reissuedComment,reissuedBy=:reissuedBy where brrId=@brrId and itemAccession=@itemAccession";
                //                sqltCommnd.CommandType = CommandType.Text;
                //                sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                //                sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccession);
                //                sqltCommnd.Parameters.AddWithValue("reissuedComment", reissueComment);
                //                sqltCommnd.Parameters.AddWithValue("reissuedBy", reissuedBy);
                //                sqltCommnd.ExecuteNonQuery();

                //                itemCount++;
                //                progressBar1.Value = itemCount;
                //                lblItem.Text = itemCount.ToString();
                //            }
                //            catch
                //            {

                //            }
                //        }
                //    }
                //}
                btnImport.Enabled = true;
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                txtbFileName.Clear();
                MessageBox.Show("save Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Please close the csv file and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnImport.Enabled = true;
            }
        }

        private void ReturnDataMigration(string fileName)
        {
            try
            {
                //Create COM Objects. Create a COM object for everything that is referenced
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(txtbFileName.Text);
                Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
                int dataFound = xlRange.Rows.Count - 1;
                if (dataFound == 0)
                {
                    MessageBox.Show("No data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    IntPtr hMenu = GetSystemMenu(this.Handle, false);
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnImport.Enabled = false;
                    lblTotal.Text = dataFound.ToString();
                    progressBar1.Maximum = dataFound;
                    int itemCount = 0;
                    DateTime tempDate;
                    string brrId, itemAccession, returnDate, returnComment, returnBy;

                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = null;
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            brrId = xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            itemAccession = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                            if (brrId != "" && itemAccession != "")
                            {
                                try
                                {
                                    tempDate = xlRange.Cells[i, "C"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "C"].Value2) : DateTime.Now.Date;
                                    returnDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    returnComment = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "";
                                    returnBy = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "update issuedItem set returnDate='" + returnDate + "',itemReturned='" + true + "'," +
                                            "returnedComment=:returnedComment,returnedBy=:returnedBy where brrId=@brrId and itemAccession=@itemAccession";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    sqltCommnd.Parameters.AddWithValue("returnedComment", returnComment);
                                    sqltCommnd.Parameters.AddWithValue("returnedBy", returnBy);
                                    sqltCommnd.ExecuteNonQuery();

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "update itemDetails set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    sqltCommnd.ExecuteNonQuery();

                                    itemCount++;
                                    progressBar1.Value = itemCount;
                                    lblItem.Text = itemCount.ToString();
                                }
                                catch
                                {

                                }
                            }
                        }
                        sqltConn.Close();
                    }
                    else
                    {
                        MySqlConnection mysqlConn=null;
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
                                btnImport.Enabled = true;
                                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                                txtbFileName.Clear();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                                xlWorkbook.Close(true);
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                                xlApp.Quit();
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                                return;
                            }
                        }
                        MySqlCommand mysqlCmd=null;
                        string queryString = "";
                        for (int i = 2; i <= xlRange.Rows.Count; i++)
                        {
                            brrId = xlRange.Cells[i, "A"].Value2 != null ? xlRange.Cells[i, "A"].Value2.ToString() : "";
                            itemAccession = xlRange.Cells[i, "B"].Value2 != null ? xlRange.Cells[i, "B"].Value2.ToString() : "";
                            if (brrId != "" && itemAccession != "")
                            {
                                try
                                {
                                    tempDate = xlRange.Cells[i, "C"].Value2 != null ? DateTime.FromOADate(xlRange.Cells[i, "C"].Value2) : DateTime.Now.Date;
                                    returnDate = tempDate.Day.ToString("00") + "/" + tempDate.Month.ToString("00") + "/" + tempDate.Year.ToString("0000");
                                    returnComment = xlRange.Cells[i, "D"].Value2 != null ? xlRange.Cells[i, "D"].Value2.ToString() : "";
                                    returnBy = xlRange.Cells[i, "E"].Value2 != null ? xlRange.Cells[i, "E"].Value2.ToString() : "";

                                    queryString = "update issued_item set returnDate='" + returnDate + "',itemReturned='" + true + "'," +
                                            "returnedComment=@returnedComment,returnedBy=@returnedBy where brrId=@brrId and itemAccession=@itemAccession";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    mysqlCmd.Parameters.AddWithValue("@returnedComment", returnComment);
                                    mysqlCmd.Parameters.AddWithValue("@returnedBy", returnBy);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();

                                    queryString = "update item_details set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemAccession", itemAccession);
                                    mysqlCmd.CommandTimeout = 99999;
                                    mysqlCmd.ExecuteNonQuery();

                                    itemCount++;
                                    progressBar1.Value = itemCount;
                                    lblItem.Text = itemCount.ToString();
                                }
                                catch
                                {

                                }
                            }
                        }
                        mysqlConn.Close();
                    }

                    btnImport.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                    txtbFileName.Clear();
                    globalVarLms.backupRequired = true;
                    MessageBox.Show("Save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                    xlWorkbook.Close(true);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                }
            }
            catch
            {
                MessageBox.Show("Please close the csv file and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnImport.Enabled = true;
            }
}

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            btnGenerate.Enabled = false;
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            string tempPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
            Directory.CreateDirectory(tempPath);
            string fileName = "";
            string columnHeader = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = Application.ProductName + "enter file name.";
            saveDialog.InitialDirectory = tempPath;
            saveDialog.DefaultExt = "xlsx";
            saveDialog.Filter = "Excel file (*.xlsx)|*.xlsx";

            if (rdbIssue.Checked)
            {
                saveDialog.FileName = "issueData";
                columnHeader = "Member Id,Item Accession,Issue Date,Expected Return Date,Issue Comment, Issue By";
            }
            else if (rdbReissue.Checked)
            {
                saveDialog.FileName = "ReissueData";
                columnHeader = "Member Id,Item Accession,Re-issue Date,Expected Return Date,Re-issue Comment,Re-issue By";
            }
            else if (rdbReturn.Checked)
            {
                saveDialog.FileName = "ReturnData";
                columnHeader = "Member Id,Item Accession,Return Date,Return Comment, Return By";
            }
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveDialog.FileName;
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

        private void FormMigration_Load(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
            string columnHeader = "";
            if (rdbIssue.Checked)
            {
                columnHeader = "Member Id,Item Accession,Issue Date,Expected Return Date,Issue Comment, Issue By";
            }
            else if (rdbReissue.Checked)
            {
                columnHeader = "Member Id,Item Accession,Re-issue Date,Expected Return Date,Re-issue Comment,Re-issue By";
            }
            else if (rdbReturn.Checked)
            {
                columnHeader = "Member Id,Item Accession,Return Date,Return Comment, Return By";
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
        }

        private void txtbFileName_TextChanged(object sender, EventArgs e)
        {
            if(txtbFileName.Text!="")
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }
        }

        private void rdbReturn_CheckedChanged(object sender, EventArgs e)
        {
            txtbFileName.Clear();
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
        }

        private void rdbReissue_CheckedChanged(object sender, EventArgs e)
        {
            txtbFileName.Clear();
            progressBar1.Maximum = 0;
            lblItem.Text = 0.ToString();
            lblTotal.Text = 0.ToString();
        }

        private void rdbIssue_CheckedChanged(object sender, EventArgs e)
        {
            txtbFileName.Clear();
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
    }
}
