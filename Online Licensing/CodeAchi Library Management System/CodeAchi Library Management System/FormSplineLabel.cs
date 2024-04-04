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
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormSplineLabel : Form
    {

        public static AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        public FormSplineLabel()
        {
            InitializeComponent();
        }

        private void FormSplineLabel_Load(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            timer1.Interval = 450;
            timer1.Start();
            btnBrowse.Enabled = false;
            cmbPaper.SelectedIndex = 0;
            panelDate.Visible = false;
            lblMessage.Visible = false;
            lblBlink.Visible = false;
            chkbFooter.Checked = true;
            dtpFrom.CustomFormat = Properties.Settings.Default.dateFormat;
            dtpTo.CustomFormat = Properties.Settings.Default.dateFormat;

            dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            GetAccession();
            txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbInfo.AutoCompleteCustomSource = autoCollData;
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
                            lblInfo.Text = "Enter " + fieldValue.Replace(fieldName + "=", "") + " :";
                            dgvAccnList.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvAccnList.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvAccnList.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            dgvAccnList.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbLocation.Text = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblClassification")
                        {
                            dgvAccnList.Columns[5].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
        }

        public static void GetAccession()
        {
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============================Add all Items======================
                string queryString = "select itemAccession from itemDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                autoCollData.Clear();
                if (dataReader.HasRows)
                {
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["itemAccession"]
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
                        mysqlConn.Open();
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select itemAccession from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    autoCollData.Clear();
                    if (dataReader.HasRows)
                    {
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["itemAccession"]
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

        private void FormSplineLabel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
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
            }
        }

        private void rdbManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbManual.Checked)
            {
                dgvAccnList.Rows.Clear();
                panelDate.Visible = false;
                lblInfo.Text = "Enter " + dgvAccnList.Columns[1].HeaderText + " :";
                txtbInfo.Clear();
                GetAccession();
                txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbInfo.AutoCompleteCustomSource = autoCollData;
            }
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDate.Checked)
            {
                dgvAccnList.Rows.Clear();
                panelDate.Visible = true;
                lblInfo.Text = "Enter " + dgvAccnList.Columns[1].HeaderText + " :";
                Application.DoEvents();
            }
        }

        private void rdbLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLocation.Checked)
            {
                dgvAccnList.Rows.Clear();
                panelDate.Visible = false;
                lblInfo.Text = "Enter " + dgvAccnList.Columns[4].HeaderText + " :";
                txtbInfo.Clear();
                Application.DoEvents();

                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    //==============================Add all Items======================
                    string queryString = "select distinct [rackNo] from itemDetails";
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    autoCollData.Clear();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            autoCollData.Add(dataReader["rackNo"].ToString());
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
                    string queryString = "select rackNo from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    autoCollData.Clear();
                    if (dataReader.HasRows)
                    {
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["rackNo"]
                                               ).ToList();
                        autoCollData.AddRange(idList.ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                txtbInfo.AutoCompleteCustomSource = autoCollData;
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
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void cmbPaper_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaper.Text == "A4_21L (3x7)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 20;
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataRow in dgvAccnList.Rows)
            {
                if (chkbAll.Checked)
                {
                    dataRow.Cells[0].Value = true;
                    dgvAccnList.CurrentCell = dataRow.Cells[1];
                }
                else
                {
                    dataRow.Cells[0].Value = false;
                    dgvAccnList.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
        }

        private void txtbInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtbInfo.Text != "")
                {
                    if (globalVarLms.sqliteData)
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
                        if (rdbManual.Checked)
                        {
                            queryString = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbInfo.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                         dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
                                }
                            }
                            dgvAccnList.ClearSelection();
                        }
                        else
                        {
                            dgvAccnList.Rows.Clear();
                            queryString = "select itemAccession,itemTitle,itemClassification,itemAuthor from itemDetails where [rackNo]='" + txtbInfo.Text + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                        txtbInfo.Text, dataReader["itemClassification"].ToString());
                                    Application.DoEvents();
                                }
                            }
                            dgvAccnList.ClearSelection();
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
                        MySqlCommand mysqlCmd=null;
                        MySqlDataReader dataReader = null;
                        string queryString = "select * from reciept_setting";
                        if (rdbManual.Checked)
                        {
                            queryString = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from item_details where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbInfo.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                         dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
                                }
                            }
                            dgvAccnList.ClearSelection();
                        }
                        else
                        {
                            dgvAccnList.Rows.Clear();
                            queryString = "select itemAccession,itemTitle,itemClassification,itemAuthor from item_details where lower(rackNo)='" + txtbInfo.Text.ToLower() + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                        txtbInfo.Text, dataReader["itemClassification"].ToString());
                                    Application.DoEvents();
                                }
                            }
                            dgvAccnList.ClearSelection();
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    autoCollData.Remove(txtbInfo.Text);
                    txtbInfo.Clear();
                }
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dtpFrom.Value.Date;
            DateTime tempdate = dtpFrom.Value.Date;
            string filterDate = "";
            dgvAccnList.Rows.Clear();

            if (globalVarLms.sqliteData)
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
                    sqltCommnd.CommandText = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from itemDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                       dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                        dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
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
                MySqlCommand mysqlCmd;
                MySqlDataReader dataReader = null;
                string queryString = "";
                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    queryString = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from item_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                       dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                       dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
                }
                mysqlConn.Close();
            }
            dgvAccnList.ClearSelection();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dtpFrom.Value.Date;
            DateTime tempdate = dtpFrom.Value.Date;
            string filterDate = "";
            dgvAccnList.Rows.Clear();

            if (globalVarLms.sqliteData)
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
                    sqltCommnd.CommandText = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from itemDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                       dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                       dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
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
                MySqlCommand mysqlCmd;
                MySqlDataReader dataReader = null;
                string queryString = "";
                while (startDate <= dtpTo.Value.Date)
                {
                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                    queryString = "select itemAccession,itemTitle,rackNo,itemClassification,itemAuthor from item_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                       dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                       dataReader["rackNo"].ToString(), dataReader["itemClassification"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();

                    startDate = tempdate.AddDays(1);
                    tempdate = startDate;
                }
                mysqlConn.Close();
            }
            dgvAccnList.ClearSelection();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            lblBlink.Visible = !lblBlink.Visible;
        }

        private void dgvAccnList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlRecord.Text = dgvAccnList.Rows.Count.ToString();
            timer2.Stop();
            lblMessage.Visible = false;
            lblBlink.Visible = false;
            btnPrint.Enabled = true;
        }

        private void dgvAccnList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dgvAccnList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void dtpFrom_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dtpTo_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dgvAccnList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTtlRecord.Text = dgvAccnList.Rows.Count.ToString();
            btnPrint.Enabled = false;
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
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
            if (dgvAccnList.Rows.Count == 0)
            {
                lblMessage.Text = "Please add some item";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                return;
            }
            DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            if (dgvCheckedRows.Count() == 0)
            {
                lblMessage.Text = "Please check some items";
                lblMessage.Visible = true;
                timer2.Interval = 250;
                timer2.Start();
                return;
            }
            string orderedBy = "";
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select instShortName from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        orderedBy = dataReader["instShortName"].ToString();
                    }
                    orderedBy = "by order " + orderedBy;
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
                    string queryString = "select instShortName from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            orderedBy = dataReader["instShortName"].ToString();
                        }
                        orderedBy = "by order " + orderedBy;
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            //======================Barcode Generate================
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
            string fileName = "spineLabel";
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

            if (cmbPaper.Text == "A4_21L (3x7)")
            {
                try
                {
                    PdfBarcodeFor3x7(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items spline label generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if(cmbPaper.Text== "A4_40L (4x10)")
            {
                try
                {
                    PdfBarcodeFor3x7(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items spline label generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (cmbPaper.Text == "Barcode Printer (5x2 Cm)")//Not used
                {
                    //if (paperSize.PaperName == "58(48) x 210 mm")
                    //{
                    //    pageSettings.PaperSize = paperSize;
                    //    break;
                    //}
                }
                else   //used
                {
                    if (paperSize.PaperName == "A4")
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }
            }
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(fileName))
            {
                using (var printDocument = pdfDocument.CreatePrintDocument())
                {
                    printDocument.DefaultPageSettings.Landscape = false;
                    printDocument.PrinterSettings = printerSettings;
                    printDocument.DefaultPageSettings = pageSettings;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                    //https://stackoverflow.com/questions/6103705/how-can-i-send-a-file-document-to-the-printer-and-have-it-print
                }
            }
        }

        private void PdfBarcodeFor3x7(string fileName, string orderedBy)
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

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                PdfPTable pdfTable = new PdfPTable(1);
                pdfTable.TotalWidth = 150;
                PdfPCell pdfCell = new PdfPCell();
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 5);
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

                    imglocY = ((pageHeight - pageHeight / 7) + ((pageHeight / 7) - imgBarcode.Height) / 2) - ((pageHeight / 7) * rowCount);
                    imgqrlocY = ((pageHeight - pageHeight / 7) + ((pageHeight / 7) - imgBarcode.Height) / 2) - ((pageHeight / 7) * rowCount);

                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY + 6;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY + 9;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imglocY = imglocY + 14;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 4)
                        {
                            imglocY = imglocY + 18;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 22;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 27;
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
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY + 6;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY + 9;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imglocY = imglocY + 14;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 4)
                        {
                            imglocY = imglocY + 18;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 22;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 27;
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
                string itemAccn = "BRC-1234567";
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        imgBarcode = barCode.Draw(itemAccn, 25);
                        imgQrcode = qrCode.Draw(itemAccn, 25);
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

                        imglocY = ((pageHeight - pageHeight / 7) + ((pageHeight / 7) - imgBarcode.Height) / 2) - ((pageHeight / 7) * rowCount);
                        imgqrlocY = ((pageHeight - pageHeight / 7) + ((pageHeight / 7) - imgBarcode.Height) / 2) - ((pageHeight / 7) * rowCount);

                        if (rdbPrinter.Checked)
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY - 10.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY + 6;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY + 9;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 3)
                            {
                                imglocY = imglocY + 14;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 4)
                            {
                                imglocY = imglocY + 18;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 22;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 27;
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
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY + 6;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY + 9;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 3)
                            {
                                imglocY = imglocY + 14;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 4)
                            {
                                imglocY = imglocY + 18;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 22;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 27;
                                imgqrlocY = imglocY;
                            }
                        }
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                        pdfContent.ShowText("CodeAchi LMS");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                        pdfContent.ShowText(DateTime.Now.ToShortDateString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[2].HeaderText+" : " + dataRow.Cells[2].Value.ToString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height-8);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[3].HeaderText + " : " + dataRow.Cells[3].Value.ToString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height-16);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[5].HeaderText + " : " + dataRow.Cells[5].Value.ToString());
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 7);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height-24);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[4].HeaderText + " : " + dataRow.Cells[4].Value.ToString());
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height - 32);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        if (columnCount == 3)
                        {
                            rowCount++;
                            columnCount = 0;
                        }
                        barcodeCount++;
                        if (barcodeCount % 21 == 0)
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

        private void PdfBarcodeFor4x10(string fileName, string orderedBy)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                Document pdfToCreate = new Document(PageSize.A4);

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), rowsPerPage = 10, columnsPerPage = 4;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                PdfPTable pdfTable = new PdfPTable(columnsPerPage);
                float[] columnWidth = new float[columnsPerPage];
                for (int i = 0; i < columnsPerPage; i++)
                {
                    columnWidth[i] = 595 / columnsPerPage;
                }
                pdfTable.SetTotalWidth(columnWidth);
                PdfPCell pdfCell = null;

                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 8);
                iTextSharp.text.Font bodyFont = new iTextSharp.text.Font(baseFont, 6);
                //................................leave barcode.........................
                int columnCount = 0;
                for (int barcodeCount = 0; barcodeCount <= leaveBarcode - 1; barcodeCount++)
                {
                    pdfCell = new PdfPCell(new Phrase(" ", headerFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfCell.FixedHeight = 84.2f;
                    pdfTable.AddCell(pdfCell);
                    columnCount++;
                    if (columnCount == columnsPerPage)
                    {
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                PdfPTable pdfCellContent = new PdfPTable(1);
                pdfCellContent.TotalWidth = 525 / columnsPerPage;
                pdfCellContent.SpacingAfter = 10f;
                pdfCellContent.SpacingBefore = 10f;
                pdfCellContent.PaddingTop = 0;

                string itemAccn = "BRC-1234567";
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), bodyFont));
                        pdfCell.PaddingTop = 0;
                        pdfCell.Border = 0;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemAccn, headerFont));
                        pdfCell.Border = 0;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        columnCount++;
                        pdfCell = new PdfPCell(pdfCellContent);
                        pdfCell.Border = 0;
                        pdfCell.FixedHeight = 84.2f;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        if (columnsPerPage == columnCount)
                        {
                            pdfCell.PaddingLeft = 10;
                            columnCount = 0;
                        }
                        pdfTable.AddCell(pdfCell);

                        //Redeclare table
                        pdfCellContent = new PdfPTable(1);
                        pdfCellContent.TotalWidth = 525 / columnsPerPage;
                        pdfCellContent.SpacingAfter = 10f;
                        pdfCellContent.SpacingBefore = 10f;
                        pdfCellContent.PaddingTop = 0;
                    }
                }
                WriteTableData(pdfToCreate, pdfContent, pdfTable, rowsPerPage);
                //pdfTable.WriteSelectedRows(0, -1, 0, 842, pdfContent);
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void WriteTableData(Document pdfToCreate, PdfContentByte pdfContent, PdfPTable pdfTable, int rowsPerPage)
        {
            int totalRows = pdfTable.Rows.Count;
            int startRow = 0;
            int endRow = 0;
            int currentPage = 1;
            float posY = 842; // Initial position at the top of the page

            while (startRow < totalRows)
            {
                // Determine the end row for the current page
                if (totalRows - endRow > rowsPerPage)
                {
                    endRow += rowsPerPage;
                }
                else
                {
                    endRow = totalRows;
                }

                // Write the selected rows for the current page
                pdfTable.WriteSelectedRows(startRow, endRow, 0, posY, pdfContent);

                // Move to the next page if there are more rows remaining
                if (endRow < totalRows)
                {
                    pdfToCreate.NewPage();
                    currentPage++;
                    posY = 842; // Reset posY for the new page
                }

                // Update startRow and posY for the next iteration
                startRow = endRow;
                posY = 842;// - pdfTable.TotalHeight;
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


