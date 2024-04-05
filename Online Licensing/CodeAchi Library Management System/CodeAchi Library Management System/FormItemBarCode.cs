
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTextSharp.awt.geom.Point2D;
using Color = System.Drawing.Color;
using Document = iTextSharp.text.Document;
using PageSize = iTextSharp.text.PageSize;
using Rectangle = iTextSharp.text.Rectangle;

namespace CodeAchi_Library_Management_System
{
    public partial class FormItemBarCode : Form
    {

        private System.Drawing.Font printFont;
        private System.Drawing.Font printFontBig;
        private string itemLocation;
        private string itemAccn = "BRC-1234567";
        public FormItemBarCode()
        {
            InitializeComponent();
        }

        public static AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormItemBarCode_Load(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            timer1.Interval = 450;
            timer1.Start();
            btnBrowse.Enabled = false;
            panelDate.Visible = false;
            lblMessage.Visible = false;
            lblBlink.Visible = false;
            chkbFooter.Checked = true;
            chkbOn.Checked = true;
            chkbQr.Checked = true;
            dtpFrom.CustomFormat = Properties.Settings.Default.dateFormat;
            dtpTo.CustomFormat = Properties.Settings.Default.dateFormat;

            dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            GetAccession();
            txtbInfo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbInfo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbInfo.AutoCompleteCustomSource = autoCollData;
            txtbPrinterName.Text = Properties.Settings.Default.barcodePrinter;
            cmbPaper.Text = Properties.Settings.Default.barcodePaper;
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
                            lblInfo.Text ="Enter "+ fieldValue.Replace(fieldName + "=", "")+" :";
                            dgvAccnList.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvAccnList.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbTitle.Text = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            dgvAccnList.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbLocation.Text = fieldValue.Replace(fieldName + "=", "");
                            rdbFLocation.Text = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblClassification")
                        {
                            dgvAccnList.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            rdbClassi.Text = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
        }

        private void FormItemBarCode_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
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
                txtbPrinterName.Text = Properties.Settings.Default.barcodePrinter;
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
                lblInfo.Text = "Enter " + dgvAccnList.Columns[3].HeaderText + " :";
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
                Properties.Settings.Default.barcodePrinter = printDialog.PrinterSettings.PrinterName;
                Properties.Settings.Default.Save();
                timer2.Stop();
                lblMessage.Visible = false;
                lblBlink.Visible = false;
            }
        }

        private void cmbPaper_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkbQr.Enabled = true;
            chkbOn.Enabled = true;
            rdbClassi.Enabled = true;
            rdbTitle.Enabled = true;
            rdbFLocation.Enabled = true;
            chkbFooter.Enabled = true;
            Properties.Settings.Default.barcodePaper = cmbPaper.Text;
            Properties.Settings.Default.Save();
            if (cmbPaper.Text == "A4_21L (3x7)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 20;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "A4_24L (3x8)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 23;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "A4_30L (3x10)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 29;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "Avery_5160")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 29;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "5927_30UP")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 29;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "A4_40L (4x10)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 39;
                chkbQr.Enabled = false;
            }
            else if (cmbPaper.Text == "A4_40L (4x10) Barcode Only")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 39;
                chkbQr.Enabled = false;
                chkbOn.Enabled = false;
                rdbClassi.Enabled = false;
                rdbTitle.Enabled = false;
                rdbFLocation.Enabled = false;
                chkbFooter.Enabled = false;
            }
            else if (cmbPaper.Text == "A4_65L (5x13)")
            {
                panelMargin.Visible = false;
                numUpBlock.Maximum = 64;
                chkbQr.Checked = false;
                chkbQr.Enabled = false;
            }
            else if (cmbPaper.Text == "Barcode Printer (5x2 Cm_1x1)")
            {
                panelMargin.Visible = true;
                chkbQr.Checked = false;
                chkbQr.Enabled = false;
            }
            else if (cmbPaper.Text == "Barcode Printer (5x2.5 Cm_1x1)")
            {
                panelMargin.Visible = true;
                chkbQr.Checked = true;
                chkbQr.Enabled = true;
            }
            else if (cmbPaper.Text == "Barcode Printer (5x2.5 Cm_2x1)")
            {
                panelMargin.Visible = true;
                chkbQr.Checked = true;
                chkbQr.Enabled = true;
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
                    string queryString = "";
                    if (globalVarLms.sqliteData)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        //==============================Add all Items======================
                        SQLiteDataReader dataReader;
                        if (rdbManual.Checked)
                        {
                            queryString = "select itemAccession,itemTitle,rackNo,itemClassification from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbInfo.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                        dataReader["itemClassification"].ToString());
                                }
                            }
                            dgvAccnList.ClearSelection();
                        }
                        else
                        {
                            dgvAccnList.Rows.Clear();
                            queryString = "select itemAccession,itemTitle,itemClassification from itemDetails where [rackNo]='" + txtbInfo.Text + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), txtbInfo.Text,
                                        dataReader["itemClassification"].ToString());
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
                        MySqlCommand mysqlCmd;
                        MySqlDataReader dataReader = null;
                        if(rdbManual.Checked)
                        {
                            queryString = "select itemAccession,itemTitle,rackNo,itemClassification from item_details where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbInfo.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(true, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                        dataReader["itemClassification"].ToString());
                                }
                            }
                            dgvAccnList.ClearSelection();
                        }
                        else
                        {
                            dgvAccnList.Rows.Clear();
                            queryString = "select itemAccession,itemTitle,itemClassification from item_details where lower(rackNo)='" + txtbInfo.Text.ToLower() + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), txtbInfo.Text,
                                        dataReader["itemClassification"].ToString());
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
                    sqltCommnd.CommandText = "select itemAccession,itemTitle,rackNo,itemClassification from itemDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                dataReader["itemClassification"].ToString());
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
                    queryString = "select itemAccession,itemTitle,rackNo,itemClassification from item_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                dataReader["itemClassification"].ToString());
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
                    sqltCommnd.CommandText = "select itemAccession,itemTitle,rackNo,itemClassification from itemDetails where entryDate='" + filterDate + "'";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                dataReader["itemClassification"].ToString());
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
                    queryString = "select itemAccession,itemTitle,rackNo,itemClassification from item_details where entryDate='" + filterDate + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvAccnList.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                dataReader["itemTitle"].ToString(), dataReader["rackNo"].ToString(),
                                dataReader["itemClassification"].ToString());
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
            string fileName = "accessionBarcode";
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
                    PdfBarcodeFor3x7L(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "A4_24L (3x8)")
            {
                try
                {
                    PdfBarcodeFor3x8(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    PdfBarcodeFor3x10(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "A4_40L (4x10)")
            {
                try
                {
                    PdfBarcodeFor4x10(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if(cmbPaper.Text== "A4_40L (4x10) Barcode Only")
            {
                try
                {
                    PdfBarcodeOnlyFor4x10(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "A4_65L (5x13)")
            {
                try
                {
                    PdfBarcodeFor5x13(fileName);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "Avery_5160")
            {
                try
                {
                    Avery_5160(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "5927_30UP")
            {
                try
                {
                    A5927_30UP(fileName, orderedBy);

                    if (rdbPrinter.Checked)//============Print Barcode===============
                    {
                        printA4Paper(fileName);
                    }
                    else
                    {
                        MessageBox.Show("Items accession generated Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            else if (cmbPaper.Text == "Barcode Printer (5x2 Cm_1x1)")
            {
                if (rdbPrinter.Checked)//============Print Barcode===============
                {
                    printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                    printFontBig = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        printFont = new System.Drawing.Font("Malgun Gothic", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                        printFontBig = new System.Drawing.Font("Malgun Gothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                    }

                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrinterSettings.PrinterName = txtbPrinterName.Text;
                    printDocument.DefaultPageSettings.Landscape = false;

                    printDocument.PrintPage += new PrintPageEventHandler(this.BarcodePrinter5x2Cm_1x1);
                    printDocument.Print();
                }
                else
                {
                    try
                    {
                        PdfBarcodeForPOS5x2_1x1(fileName, orderedBy);
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
            else if (cmbPaper.Text == "Barcode Printer (5x2.5 Cm_1x1)")
            {
                if (rdbPrinter.Checked)
                {
                    printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                    printFontBig = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        printFont = new System.Drawing.Font("Malgun Gothic", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                        printFontBig = new System.Drawing.Font("Malgun Gothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                    }
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrinterSettings.PrinterName = txtbPrinterName.Text;
                    dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
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
                        PdfBarcodePrinter5x25_1x1(fileName, orderedBy);
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
                if (rdbPrinter.Checked)
                {
                    printFont = new System.Drawing.Font("Arial", 6, FontStyle.Bold);
                    printFontBig = new System.Drawing.Font("Arial", 7, FontStyle.Bold);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        printFont = new System.Drawing.Font("Malgun Gothic", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                        printFontBig = new System.Drawing.Font("Malgun Gothic", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(129)));
                    }
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrinterSettings.PrinterName = txtbPrinterName.Text;
                    dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
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
                        PdfBarcodePrinter5x25_2x1(fileName, orderedBy);
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
            dgvAccnList.Rows.Clear();
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

        private void BarcodePrinter5x2Cm_1x1(object sender, PrintPageEventArgs ev)
        {
            float yPos = 0;
            float leftMargin = 20f;
            float topMargin = 0;

            Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            System.Drawing.Image imgBarcode;

            DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            foreach (DataGridViewRow dataRow in dgvCheckedRows)
            {
                itemAccn = dataRow.Cells[1].Value.ToString();
                itemLocation = dataRow.Cells[3].Value.ToString();
                imgBarcode = barCode.Draw(itemAccn, 25);

                topMargin = Convert.ToInt32(numMargin.Value);

                // Print each line of the file.
                yPos = yPos + topMargin + (printFont.GetHeight(ev.Graphics));

                ev.Graphics.DrawString("CodeAchi LMS", printFont, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                ev.Graphics.DrawString(DateTime.Now.ToShortDateString(), printFont, Brushes.Black,
                        leftMargin + 100, yPos, new StringFormat());

                if (chkbOn.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString(dgvAccnList.Columns[3].HeaderText + " : " + itemLocation, printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }

                yPos = yPos + 10;
                ev.Graphics.DrawImage(imgBarcode, leftMargin, yPos);

                yPos = yPos + imgBarcode.Height + 5;
                ev.Graphics.DrawString(itemAccn, printFontBig, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                if (chkbFooter.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString("*Dont tamper this sticker.", printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }

                yPos = yPos + 10;
            }
            //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument?view=netframework-4.8
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

            DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            yPos = printFont.GetHeight(ev.Graphics);
            foreach (DataGridViewRow dataRow in dgvCheckedRows)
            {
                itemAccn = dataRow.Cells[1].Value.ToString();
                itemLocation = dataRow.Cells[3].Value.ToString();
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);

                // Print each line of the file.
                ev.Graphics.DrawString("CodeAchi LMS", printFont, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                ev.Graphics.DrawString(DateTime.Now.ToShortDateString(), printFont, Brushes.Black,
                        leftMargin + 100, yPos, new StringFormat());

                if (chkbOn.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString(dgvAccnList.Columns[3].HeaderText + " : " + itemLocation, printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }

                yPos = yPos + 10;
                ev.Graphics.DrawImage(imgBarcode, leftMargin, yPos);

                yPos = yPos + imgBarcode.Height + 5;
                ev.Graphics.DrawString(itemAccn, printFontBig, Brushes.Black,
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
                    ev.Graphics.DrawImage(bmImage, leftMargin + 110, yPos - 50);
                }
                yPos = yPos + 49.42f;
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

            DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            yPos = printFont.GetHeight(ev.Graphics);
            tempYpos = yPos;

            foreach (DataGridViewRow dataRow in dgvCheckedRows)
            {
                itemAccn = dataRow.Cells[1].Value.ToString();
                itemLocation = dataRow.Cells[3].Value.ToString();
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);

                // Print each line of the file.
                ev.Graphics.DrawString("CodeAchi LMS", printFont, Brushes.Black,
                        leftMargin, yPos, new StringFormat());

                ev.Graphics.DrawString(DateTime.Now.ToShortDateString(), printFont, Brushes.Black,
                        leftMargin + 100, yPos, new StringFormat());

                if (chkbOn.Checked)
                {
                    yPos = yPos + 10;
                    ev.Graphics.DrawString(dgvAccnList.Columns[3].HeaderText + " : " + itemLocation, printFont, Brushes.Black,
                            leftMargin, yPos, new StringFormat());
                }

                yPos = yPos + 10;
                ev.Graphics.DrawImage(imgBarcode, leftMargin, yPos);

                yPos = yPos + imgBarcode.Height + 5;
                ev.Graphics.DrawString(itemAccn, printFontBig, Brushes.Black,
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
                    ev.Graphics.DrawImage(bmImage, leftMargin + 110, yPos - 50);
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
                    yPos = yPos + 49.42f;
                    tempYpos = yPos;
                }
            }
            //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.printing.printdocument?view=netframework-4.8
        }

        private void PdfBarcodeForPOS5x2_1x1(string fileName, string orderedBy)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                var pgSize = new iTextSharp.text.Rectangle(141.7f, 56.6f);
                Document pdfToCreate = new Document(pgSize);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                float imglocX = 0, imglocY = 0;
                iTextSharp.text.Image barcodeJpg;

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
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    imgBarcode = barCode.Draw(itemAccn, 25);

                    imglocY = ((pageHeight - pageHeight / 13) + ((pageHeight / 13) - imgBarcode.Height) / 2) - ((pageHeight / 13) * 0 + (7f * 0));

                    imglocY = imglocY - 25.0f;
                    imglocX = imglocX + 20.0f;

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

                    if (chkbOn.Checked)
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 2);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[3].HeaderText + " : " + dataRow.Cells[3].Value.ToString());
                        pdfContent.EndText();
                    }

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

                    pdfToCreate.NewPage();
                    imglocX = 0;
                    imglocY = 0;
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
        }

        private void PdfBarcodePrinter5x25_1x1(string fileName, string orderedBy)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                int paperHeight = 0;
                if (dgvCheckedRows.Length % 2 == 0)
                {
                    paperHeight = Convert.ToInt32(108.5 * (dgvCheckedRows.Length / 2));
                }
                else
                {
                    paperHeight = Convert.ToInt32(108.5 * ((dgvCheckedRows.Length + 1) / 2));
                }
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

                    if (chkbOn.Checked)
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 2);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[3].HeaderText + " : " + dataRow.Cells[3].Value.ToString());
                        pdfContent.EndText();
                    }

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

        private void PdfBarcodePrinter5x25_2x1(string fileName, string orderedBy)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                int paperHeight = 0;
                if (dgvCheckedRows.Length % 2 == 0)
                {
                    paperHeight = Convert.ToInt32(108.5 * (dgvCheckedRows.Length / 2));
                }
                else
                {
                    paperHeight = Convert.ToInt32(108.5 * ((dgvCheckedRows.Length + 1) / 2));
                }
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

                    if (chkbOn.Checked)
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 2);  //(xPos, yPos)
                        pdfContent.ShowText(dgvAccnList.Columns[3].HeaderText + " : " + dataRow.Cells[3].Value.ToString());
                        pdfContent.EndText();
                    }

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

        private void PdfBarcodeFor3x7L(string fileName, string orderedBy)
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
                //baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Nirmala UI.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
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
                            imglocX = ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
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
                                imglocY = imglocY - 20;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY-10;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 2)
                            {
                                //imglocY = imglocY + 9;
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
                                imglocY = imglocY + 38;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 43;
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
                                imglocY = imglocY - 6;
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
                                imglocY = imglocY + 28;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 33;
                                imgqrlocY = imglocY;
                            }
                        }
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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 150;

                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }

                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 2, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10 + 15);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 27);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - (17 - 20));
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

        private void PdfBarcodeFor3x8(string fileName, string orderedBy)
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

                    imglocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);
                    imgqrlocY = ((pageHeight - pageHeight / 8) + ((pageHeight / 8) - imgBarcode.Height) / 2) - ((pageHeight / 8) * rowCount);

                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 25.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY - 20.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY - 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 4)
                        {
                            imglocY = imglocY + 7.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 15.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 30.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 7)
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
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imglocY = imglocY - 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 7)
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
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
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
                            imglocX = ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
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
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY - 20.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY - 10.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 3)
                            {
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 4)
                            {
                                imglocY = imglocY + 7.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 15.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 30.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 7)
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
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 3)
                            {
                                imglocY = imglocY - 5.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 5.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 10.0f;
                                imgqrlocY = imglocY;
                            }
                            else if (rowCount == 7)
                            {
                                imglocY = imglocY + 20.0f;
                                imgqrlocY = imglocY;
                            }
                        }
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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 150;
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 2, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10 + 15);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 27);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - (17 - 20));
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

        private void PdfBarcodeFor3x10(string fileName, string orderedBy)
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
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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

                    imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                    imgqrlocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);

                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 4)
                        {
                            imglocY = imglocY + 2.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 8.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 7)
                        {
                            imglocY = imglocY + 11.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 8)
                        {
                            imglocY = imglocY + 15.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 9)
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
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY + 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
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
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
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
                            imglocX = ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
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
                        pdfContent.SetTextMatrix(imglocX, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                        pdfContent.ShowText("CodeAchi LMS");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 6);
                        pdfContent.SetTextMatrix(imglocX + 50, imglocY + imgBarcode.Height + 8);  //(xPos, yPos)
                        pdfContent.ShowText(DateTime.Now.ToShortDateString());
                        pdfContent.EndText();

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 150;
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 2, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10 + 15);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 27);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - (17 - 20));
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

        private void Avery_5160(string fileName, string orderedBy)
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
                //pageHeight = pageHeight -90f;
                float imglocX = 0, imglocY = 0;
                float imgqrlocX = 0, imgqrlocY = 0;
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                        imglocX = imglocX + 85.0f;
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

                    if (rowCount == 0)
                    {
                        imglocY = imglocY - 75.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 1)
                    {
                        imglocY = imglocY - 61.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 2)
                    {
                        imglocY = imglocY - 45.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 3)
                    {
                        imglocY = imglocY - 30.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 4)
                    {
                        imglocY = imglocY - 10.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 5)
                    {
                        imglocY = imglocY + 5.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 6)
                    {
                        imglocY = imglocY + 22.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 7)
                    {
                        imglocY = imglocY + 35.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 8)
                    {
                        imglocY = imglocY + 53.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 9)
                    {
                        imglocY = imglocY + 73.0f;
                        imgqrlocY = imglocY;
                    }

                    if (columnCount == 3)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
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
                            imglocX = ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 85.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 65.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }

                        imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                        imgqrlocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                       
                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 75.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY - 61.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY -45.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imglocY = imglocY - 30.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 4)
                        {
                            imglocY = imglocY - 10.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 5.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY +22.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 7)
                        {
                            imglocY = imglocY +35.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 8)
                        {
                            imglocY = imglocY +53.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 9)
                        {
                            imglocY = imglocY + 73.0f;
                            imgqrlocY = imglocY;
                        }

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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 150;
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY+3, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10 + 15);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY-10);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - (17 - 20));
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

        private void A5927_30UP(string fileName, string orderedBy)
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
                //pageHeight = pageHeight -90f;
                float imglocX = 0, imglocY = 0;
                float imgqrlocX = 0, imgqrlocY = 0;
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;
                iTextSharp.text.Image qrcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                        imglocX = imglocX + 85.0f;
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

                    if (rowCount == 0)
                    {
                        imglocY = imglocY - 33.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 1)
                    {
                        imglocY = imglocY - 18.5f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 2)
                    {
                        imglocY = imglocY - 6.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 3)
                    {
                        imglocY = imglocY + 3.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 4)
                    {
                        imglocY = imglocY + 13.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 5)
                    {
                        imglocY = imglocY + 25.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 6)
                    {
                        imglocY = imglocY + 38.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 7)
                    {
                        imglocY = imglocY + 53.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 8)
                    {
                        imglocY = imglocY + 63.0f;
                        imgqrlocY = imglocY;
                    }
                    else if (rowCount == 9)
                    {
                        imglocY = imglocY + 78.0f;
                        imgqrlocY = imglocY;
                    }

                    if (columnCount == 3)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
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
                            imglocX = ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 85.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 3) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 80.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            imglocX = ((pageWidth / 3) * 2) + ((pageWidth / 3) - (barcodeWidth + qrcodeWidth)) / 3;
                            imglocX = imglocX + 65.0f;
                            imgqrlocX = imglocX + 90.0f;
                            columnCount++;
                        }

                        imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);
                        imgqrlocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount);

                        if (rowCount == 0)
                        {
                            imglocY = imglocY - 33.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 1)
                        {
                            imglocY = imglocY - 18.5f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 2)
                        {
                            imglocY = imglocY - 6.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 3)
                        {
                            imglocY = imglocY + 3.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 4)
                        {
                            imglocY = imglocY + 13.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 5)
                        {
                            imglocY = imglocY + 25.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 6)
                        {
                            imglocY = imglocY + 38.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 7)
                        {
                            imglocY = imglocY + 53.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 8)
                        {
                            imglocY = imglocY + 63.0f;
                            imgqrlocY = imglocY;
                        }
                        else if (rowCount == 9)
                        {
                            imglocY = imglocY + 78.0f;
                            imgqrlocY = imglocY;
                        }

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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 150;
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 3, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY - 10 + 15);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 10);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        qrcodeJpg = iTextSharp.text.Image.GetInstance(imgQrcode, BaseColor.WHITE);

                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        qrcodeJpg.SetAbsolutePosition(imgqrlocX, imgqrlocY - (17 - 20));
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

        private void PdfBarcodeFor4x10Old(string fileName, string orderedBy)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {

                // Define page size in millimeters
                float widthInMM = iTextSharp.text.Utilities.MillimetersToPoints(210) ; // A4 width
                float heightInMM = iTextSharp.text.Utilities.MillimetersToPoints(297) ; // A4 height// Create a new document with defined page size
                //Document document = new Document(new Rectangle(widthInMM, heightInMM), 50, 50, 50, 50);
                //Document pdfToCreate = new Document(PageSize.A4);
                Document pdfToCreate = new Document(new Rectangle(widthInMM, heightInMM), 0, 0, 0, 0);

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;

                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                MessageBox.Show(pageWidth.ToString() + "-" + pageHeight.ToString());
                float imglocX = 0, imglocY = 0;
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                        imglocX = ((pageWidth / 4) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 10.0f;
                        columnCount++;
                    }
                    else if (columnCount == 1)
                    {
                        imglocX = (pageWidth / 4) + ((pageWidth / 4) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 20.0f;
                        columnCount++;
                    }
                    else if (columnCount == 2)
                    {
                        if (rdbPrinter.Checked)
                        {
                            imglocX = ((pageWidth / 4) * 2) + ((pageWidth / 4) - imgBarcode.Width) / 2;
                            imglocX = imglocX + 10.0f;
                        }
                        else
                        {
                            imglocX = ((pageWidth / 4) * 2) + ((pageWidth / 4) - imgBarcode.Width) / 2;
                            imglocX = imglocX + 30.0f;
                        }
                        columnCount++;
                    }
                    else if (columnCount == 3)
                    {
                        if (rdbPrinter.Checked)
                        {
                            imglocX = ((pageWidth / 4) * 3) + ((pageWidth / 4) - imgBarcode.Width) / 2;
                            imglocX = imglocX + 10.0f;
                        }
                        else
                        {
                            imglocX = ((pageWidth / 4) * 3) + ((pageWidth / 4) - imgBarcode.Width) / 2;
                            imglocX = imglocX + 40.0f;
                        }
                        columnCount++;
                    }

                    imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount + (1f * rowCount));
                    if (rdbPrinter.Checked)
                    {
                        if (rowCount == 8)
                        {
                            imglocY = imglocY + 10.0f;
                        }
                        else if (rowCount == 9)
                        {
                            imglocY = imglocY + 17.0f;
                        }
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            imglocY = imglocY + 10.0f;
                        }
                    }

                    if (columnCount == 4)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        imgBarcode = barCode.Draw(itemAccn, 25);
                        //imgQrcode = qrCode.Draw(itemAccn, 25);

                        if (columnCount == 0)
                        {
                            imglocX = ((pageWidth / 4) - barcodeWidth) / 2;
                            imglocX = imglocX + 10.0f;
                            columnCount++;
                        }
                        else if (columnCount == 1)
                        {
                            imglocX = (pageWidth / 4) + ((pageWidth / 4) - barcodeWidth) / 2;
                            imglocX = imglocX + 20.0f;
                            columnCount++;
                        }
                        else if (columnCount == 2)
                        {
                            if (rdbPrinter.Checked)
                            {
                                imglocX = ((pageWidth / 4) * 2) + ((pageWidth / 4) - barcodeWidth) / 2;
                                imglocX = imglocX + 10.0f;
                            }
                            else
                            {
                                imglocX = ((pageWidth / 4) * 2) + ((pageWidth / 4) - barcodeWidth) / 2;
                                imglocX = imglocX + 30.0f;
                            }
                            columnCount++;
                        }
                        else if (columnCount == 3)
                        {
                            if (rdbPrinter.Checked)
                            {
                                imglocX = ((pageWidth / 4) * 3) + ((pageWidth / 4) - barcodeWidth) / 2;
                                imglocX = imglocX + 10.0f;
                            }
                            else
                            {
                                imglocX = ((pageWidth / 4) * 3) + ((pageWidth / 4) - barcodeWidth) / 2;
                                imglocX = imglocX + 40.0f;
                            }
                            columnCount++;
                        }
                        imglocY = ((pageHeight - pageHeight / 10) + ((pageHeight / 10) - imgBarcode.Height) / 2) - ((pageHeight / 10) * rowCount + (1f * rowCount));

                        if (rdbPrinter.Checked)
                        {
                            if (rowCount == 8)
                            {
                                imglocY = imglocY + 10.0f;
                            }
                            else if (rowCount == 9)
                            {
                                imglocY = imglocY + 17.0f;
                            }
                        }
                        else
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY + 10.0f;
                            }
                        }

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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 110;
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), headerFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), headerFont));
                            }
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 5, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY + 5);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 21);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        barcodeJpg.ScalePercent(55f);
                        pdfToCreate.Add(barcodeJpg);

                        if (columnCount == 4)
                        {
                            rowCount++;
                            columnCount = 0;
                        }
                        barcodeCount++;
                        if (barcodeCount % 40 == 0)
                        {
                            rowCount = 0;
                            pdfToCreate.NewPage();
                            imglocX = 0; imglocY = 0;
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

                Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image imgBarcode;

                Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                System.Drawing.Image imgQrcode;

                float pageWidth = pdfToCreate.PageSize.Width;
                float pageHeight = pdfToCreate.PageSize.Height;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), rowsPerPage = 10, columnsPerPage=4;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                iTextSharp.text.Image barcodeJpg;
                //................................leave barcode.........................
                imgBarcode = barCode.Draw("1234567", 25);
                imgQrcode = qrCode.Draw("1234567", 25);
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
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        if (chkbOn.Checked)
                        {
                            if (rdbFLocation.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString().ToUpper(), bodyFont));
                            }
                            else if (rdbTitle.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString().ToUpper(), bodyFont));
                            }
                            else if (rdbClassi.Checked)
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString().ToUpper(), bodyFont));
                            }
                            pdfCell.PaddingTop = 0;
                            pdfCell.Border = 0;
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCellContent.AddCell(pdfCell);
                        }

                        imgBarcode = barCode.Draw(itemAccn, 25);
                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        barcodeJpg.ScalePercent(99f);
                        pdfCell.AddElement(barcodeJpg);
                        pdfCell.Border = 0;
                        pdfCell.PaddingLeft = 10f;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemAccn, headerFont));
                        pdfCell.Border = 0;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        if (chkbFooter.Checked)
                        {
                            pdfCell = new PdfPCell(new Phrase("*Don't remove this sticker.", bodyFont));
                            pdfCell.Border = 0;
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCellContent.AddCell(pdfCell);
                        }

                        columnCount++;
                        pdfCell = new PdfPCell(pdfCellContent);
                        pdfCell.Border = 0;
                        pdfCell.FixedHeight = 84.2f;
                        pdfCell.HorizontalAlignment=Element.ALIGN_CENTER;
                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        if (columnsPerPage == columnCount)
                        {
                            pdfCell.PaddingLeft = 10;
                            columnCount = 0;
                        }
                        pdfTable.AddCell(pdfCell);

                        //Redeclare table
                        pdfCellContent = new PdfPTable(1);
                        pdfCellContent.TotalWidth = 525/columnsPerPage;
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

        private void PdfBarcodeOnlyFor4x10(string fileName, string orderedBy)
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
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), rowsPerPage = 10,columnsPerPage=4;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                pdfToCreate.Open();
                PdfContentByte pdfContent = pdWriter.DirectContent;

                PdfPTable pdfTable = new PdfPTable(columnsPerPage);
                float[] columnWidth=new float[columnsPerPage];
                for (int i = 0; i < columnsPerPage; i++)
                {
                    columnWidth[i] = 595 / columnsPerPage;
                }
                pdfTable.SetTotalWidth(columnWidth);
                PdfPCell pdfCell = null;

                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont,12);
                iTextSharp.text.Image barcodeJpg;
                
                //................................leave barcode.........................
                imgBarcode = barCode.Draw("1234567", 25);
                imgQrcode = qrCode.Draw("1234567", 25);
                for (int barcodeCount = 0; barcodeCount <= leaveBarcode - 1; barcodeCount++)
                {
                    pdfCell = new PdfPCell(new Phrase(" ", headerFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfCell.FixedHeight = 84.2f;
                    pdfTable.AddCell(pdfCell);
                }

                //........................print barcode.....................................
                PdfPTable pdfCellContent = new PdfPTable(1);
                pdfCellContent.TotalWidth = columnWidth[0];
                pdfCellContent.SpacingAfter = 10f;
                pdfCellContent.SpacingBefore =10f;
                pdfCellContent.PaddingTop = 0;

                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        pdfCell = new PdfPCell(new Phrase(orderedBy.Replace("by order ",""), headerFont));
                        pdfCell.Border = 0;
                        pdfCell.HorizontalAlignment=Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        imgBarcode = barCode.Draw(itemAccn, 25);
                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        barcodeJpg.ScalePercent(98f);
                        pdfCell.AddElement(barcodeJpg);
                        pdfCell.Border = 0;
                        pdfCell.PaddingLeft = 10f;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        pdfCell = new PdfPCell(new Phrase(itemAccn, headerFont));
                        pdfCell.Border = 0;
                        pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfCellContent.AddCell(pdfCell);

                        pdfCell = new PdfPCell(pdfCellContent);
                        pdfCell.Border = 0;
                        pdfCell.FixedHeight = 84.2f;
                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(pdfCell);

                        //Redeclare table
                        pdfCellContent = new PdfPTable(1);
                        pdfCellContent.TotalWidth = columnWidth[0];
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

        private void PdfBarcodeFor5x13(string fileName)
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
                int columnCount = 0, rowCount = 0;
                int leaveBarcode = Convert.ToInt32(numUpBlock.Value), barcodeCount = 1;
                iTextSharp.text.Image barcodeJpg;

                pdfToCreate.SetMargins(0, 0, 0, 0);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                //{
                //    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //}
                if (File.Exists("c:/windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else if (File.Exists("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf"))
                {
                    baseFont = BaseFont.CreateFont("C:/Users/codea/AppData/Local/Microsoft/Windows/Fonts/Arial Unicode Font.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
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
                    if (columnCount == 0)                     //conditions for row.................
                    {
                        imglocX = ((pageWidth / 5) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 30.0f;
                        columnCount++;
                    }
                    else if (columnCount == 1)
                    {
                        imglocX = (pageWidth / 5) + ((pageWidth / 5) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 30.0f;
                        columnCount++;
                    }
                    else if (columnCount == 2)
                    {
                        imglocX = ((pageWidth / 5) * 2) + ((pageWidth / 5) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 25.0f;
                        columnCount++;
                    }
                    else if (columnCount == 3)
                    {
                        imglocX = ((pageWidth / 5) * 3) + ((pageWidth / 5) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 20.0f;
                        columnCount++;
                    }
                    else if (columnCount == 4)
                    {
                        imglocX = ((pageWidth / 5) * 4) + ((pageWidth / 5) - imgBarcode.Width) / 2;
                        imglocX = imglocX + 20f;
                        columnCount++;
                    }
                    imglocY = ((pageHeight - pageHeight / 13) + ((pageHeight / 13) - imgBarcode.Height) / 2) - ((pageHeight / 13) * rowCount + (7f * rowCount));
                    if (rowCount == 0)
                    {
                        imglocY = imglocY - 20.0f;
                    }
                    else if (rowCount == 1)
                    {
                        imglocY = imglocY - 10.0f;
                    }
                    else if (rowCount == 3)
                    {
                        imglocY = imglocY + 8.0f;
                    }
                    else if (rowCount == 4)
                    {
                        imglocY = imglocY + 19.0f;
                    }
                    else if (rowCount == 5)
                    {
                        imglocY = imglocY + 27.0f;
                    }
                    else if (rowCount == 6)
                    {
                        imglocY = imglocY + 38.0f;
                    }
                    else if (rowCount == 7)
                    {
                        imglocY = imglocY + 49.0f;
                    }
                    else if (rowCount == 8)
                    {
                        imglocY = imglocY + 59.0f;
                    }
                    else if (rowCount == 9)
                    {
                        imglocY = imglocY + 68.0f;
                    }
                    else if (rowCount == 10)
                    {
                        imglocY = imglocY + 79.0f;
                    }
                    else if (rowCount == 11)
                    {
                        imglocY = imglocY + 89.0f;
                    }
                    else if (rowCount == 12)
                    {
                        imglocY = imglocY + 99.0f;
                    }
                    else if (rowCount == 13)
                    {
                        imglocY = imglocY + 109.0f;
                    }

                    if (columnCount == 5)
                    {
                        rowCount++;
                        columnCount = 0;
                    }
                }

                string strValue = "";
                //........................print barcode.....................................
                string itemAccn = "BRC-1234567";
                imgBarcode = barCode.Draw(itemAccn, 25);
                imgQrcode = qrCode.Draw(itemAccn, 25);
                float barcodeWidth = imgBarcode.Width;
                float qrcodeWidth = imgQrcode.Width;
                DataGridViewRow[] dgvCheckedRows = dgvAccnList.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    itemAccn = dataRow.Cells[1].Value.ToString();
                    if (itemAccn != "")
                    {
                        imgBarcode = barCode.Draw(itemAccn, 25);
                        if (rdbPrinter.Checked)
                        {
                            if (columnCount == 0)                     //conditions for row.................
                            {
                                imglocX = ((pageWidth / 5) - barcodeWidth) / 2;
                                if (rdbPrinter.Checked)
                                {
                                    imglocX = imglocX + 35.0f;
                                }
                                else
                                {
                                    imglocX = imglocX + 30.0f;
                                }
                                columnCount++;
                            }
                            else if (columnCount == 1)
                            {
                                imglocX = (pageWidth / 5) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 25.0f;
                                columnCount++;
                            }
                            else if (columnCount == 2)
                            {
                                imglocX = ((pageWidth / 5) * 2) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 20.0f;
                                columnCount++;
                            }
                            else if (columnCount == 3)
                            {
                                imglocX = ((pageWidth / 5) * 3) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 15.0f;
                                columnCount++;
                            }
                            else if (columnCount == 4)
                            {
                                imglocX = ((pageWidth / 5) * 4) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 10.0f;
                                columnCount++;
                            }
                        }
                        else
                        {
                            if (columnCount == 0)                     //conditions for row.................
                            {
                                imglocX = ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 20.0f;
                                columnCount++;
                            }
                            else if (columnCount == 1)
                            {
                                imglocX = (pageWidth / 5) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 20.0f;
                                columnCount++;
                            }
                            else if (columnCount == 2)
                            {
                                imglocX = ((pageWidth / 5) * 2) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 18.0f;
                                columnCount++;
                            }
                            else if (columnCount == 3)
                            {
                                imglocX = ((pageWidth / 5) * 3) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 15.0f;
                                columnCount++;
                            }
                            else if (columnCount == 4)
                            {
                                imglocX = ((pageWidth / 5) * 4) + ((pageWidth / 5) - barcodeWidth) / 2;
                                imglocX = imglocX + 15.0f;
                                columnCount++;
                            }
                        }
                        

                        imglocY = ((pageHeight - pageHeight / 13) + ((pageHeight / 13) - imgBarcode.Height) / 2) - ((pageHeight / 13) * rowCount + (7f * rowCount));

                        if (rdbPrinter.Checked)
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY - 30.0f;
                            }
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY - 15.5f;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY - 5.0f;
                            }
                            else if (rowCount == 3)
                            {
                                imglocY = imglocY + 10.0f;
                            }
                            else if (rowCount == 4)
                            {
                                imglocY = imglocY + 22.0f;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 31.5f;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 45.5f;
                            }
                            else if (rowCount == 7)
                            {
                                imglocY = imglocY + 57.0f;
                            }
                            else if (rowCount == 8)
                            {
                                imglocY = imglocY + 70.0f;
                            }
                            else if (rowCount == 9)
                            {
                                imglocY = imglocY + 83.0f;
                            }
                            else if (rowCount == 10)
                            {
                                imglocY = imglocY + 95.0f;
                            }
                            else if (rowCount == 11)
                            {
                                imglocY = imglocY + 105.0f;
                            }
                            else if (rowCount == 12)
                            {
                                imglocY = imglocY + 120.0f;
                            }
                        }
                        else
                        {
                            if (rowCount == 0)
                            {
                                imglocY = imglocY - 15.0f;
                            }
                            else if (rowCount == 1)
                            {
                                imglocY = imglocY - 3.0f;
                            }
                            else if (rowCount == 2)
                            {
                                imglocY = imglocY + 5.0f;
                            }
                            else if (rowCount == 3)
                            {
                                imglocY = imglocY + 15.0f;
                            }
                            else if (rowCount == 4)
                            {
                                imglocY = imglocY + 25.0f;
                            }
                            else if (rowCount == 5)
                            {
                                imglocY = imglocY + 39.0f;
                            }
                            else if (rowCount == 6)
                            {
                                imglocY = imglocY + 47.0f;
                            }
                            else if (rowCount == 7)
                            {
                                imglocY = imglocY + 57.0f;
                            }
                            else if (rowCount == 8)
                            {
                                imglocY = imglocY + 67.0f;
                            }
                            else if (rowCount == 9)
                            {
                                imglocY = imglocY + 77.0f;
                            }
                            else if (rowCount == 10)
                            {
                                imglocY = imglocY + 87.0f;
                            }
                            else if (rowCount == 11)
                            {
                                imglocY = imglocY + 97.0f;
                            }
                            else if (rowCount == 12)
                            {
                                imglocY = imglocY + 109.0f;
                            }
                        }

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

                        if (chkbOn.Checked)
                        {
                            pdfTable = new PdfPTable(1);
                            pdfTable.TotalWidth = 90;

                            if (rdbFLocation.Checked)
                            {
                                strValue = dataRow.Cells[3].Value.ToString().ToUpper();
                            }
                            else if (rdbTitle.Checked)
                            {
                                strValue = dataRow.Cells[2].Value.ToString().ToUpper();
                            }
                            else if (rdbClassi.Checked)
                            {
                                strValue = dataRow.Cells[4].Value.ToString().ToUpper();
                            }
                            if (strValue.Length > 50)
                            {
                                strValue = strValue.Substring(0, 47) + "...";
                            }

                            pdfCell = new PdfPCell(new Phrase(strValue, headerFont));
                            pdfCell.BorderWidth = 0;
                            pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            pdfTable.AddCell(pdfCell);

                            pdfTable.WriteSelectedRows(0, -1, imglocX, imglocY + 7, pdfContent);
                        }

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 8);
                        pdfContent.SetTextMatrix(imglocX, imglocY + 7);  //(xPos, yPos)
                        pdfContent.ShowText(itemAccn);
                        pdfContent.EndText();

                        if (chkbFooter.Checked)
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 6);
                            pdfContent.SetTextMatrix(imglocX, imglocY - 12);  //(xPos, yPos)
                            pdfContent.ShowText("*Don't remove this sticker.");
                            pdfContent.EndText();
                        }

                        barcodeJpg = iTextSharp.text.Image.GetInstance(imgBarcode, BaseColor.WHITE);
                        barcodeJpg.SetAbsolutePosition(imglocX, imglocY + 15);
                        barcodeJpg.ScalePercent(55f);

                        pdfToCreate.Add(barcodeJpg);
                        if (columnCount == 5)
                        {
                            rowCount++;
                            columnCount = 0;
                        }
                        barcodeCount++;
                        if (barcodeCount % 65 == 0)
                        {
                            rowCount = 0;
                            pdfToCreate.NewPage();
                            imglocX = 0; imglocY = 0;
                        }
                    }
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
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

        private void chkbOn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbOn.Checked)
            {
                rdbTitle.Checked = true;
                rdbClassi.Enabled = true;
                rdbTitle.Enabled = true;
                rdbFLocation.Enabled = true;
            }
            else
            {
                rdbClassi.Checked = false;
                rdbTitle.Checked = false;
                rdbFLocation.Checked = false;
                rdbClassi.Enabled = false;
                rdbTitle.Enabled = false;
                rdbFLocation.Enabled = false;
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

namespace SafeHandles
{
    public class PrinterSafeHandle : global::Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        public PrinterSafeHandle(string PrinterName) : base(true)
        {
            if (!OpenPrinter(PrinterName, out this.handle, IntPtr.Zero))
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        protected override bool ReleaseHandle()
        {
            return ClosePrinter(this.handle);
        }
    }
}
