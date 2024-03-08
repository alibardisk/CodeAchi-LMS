using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CodeAchi_Library_Management_System
{
    public partial class FormReport : Form
    {
        public FormReport()
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

        string brrName = "",itemTitle="";
        int ttlIssued = 0, ttlReturned = 0, ttlReissue = 0,notIssued=0;
        double paidAmount = 0.0, remitedAmount = 0.0,dueAmount=0.0,discountAmount=0.0;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        List<string> selctedColumnsList = new List<string> { };
        List<string> brrIdList = new List<string> { };
        string[] fieldList = null;string filteredColumn="";

        private void FormReport_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            panelMostLeast.Visible = false;
            pnlField.Visible = false;
            rdbAll.Checked = true;
            dtpFrom.Enabled = false;
            dtpTo.Enabled = false;
            cmbCategory.Enabled = false;
            cmbSubcategory.Enabled = false;
            cmbSearchBy.Enabled = false;
            txtbValue.Enabled = false;
            lnklblFieldAdd.Visible = false;
            chkbFilter.Enabled = false;
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblInfo3.Visible = false;
            lblInfo4.Visible = false;
            panelStatus.Visible = false;
            dtpTo.CustomFormat = Properties.Settings.Default.dateFormat;
            dtpFrom.CustomFormat = Properties.Settings.Default.dateFormat;

            dgvReport.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            btnApply.Enabled = false;
            btnCsv.Enabled = false;
            btnPdf.Enabled = false;
            btnPrint.Enabled = false;
        }

        private void dtpFrom_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void dtpTo_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbAll.Checked)
            {
                dtpFrom.Value = DateTime.Now;
                dtpTo.Value = DateTime.Now;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
            }
            else
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
        }

        private void rdbDate_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbDate.Checked)
            {
                dtpFrom.Enabled = true;
                dtpTo.Enabled = true;
            }
        }

        private void dgvReport_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvReport.Rows)
                row.Cells[0].Value = (row.Index + 1).ToString();
        }

        private void lnklblReportSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormReportSetting reportSetting = new FormReportSetting();
            reportSetting.ShowDialog();
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                MessageBox.Show("This feature is not available in the trial version", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (globalVarLms.currentDate > globalVarLms.expiryDate)
            {
                MessageBox.Show("Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No datafound !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Pdf Files (*.pdf)|*.pdf";
            saveDialog.DefaultExt = "pdf";
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    GenerateReport(saveDialog.FileName);
                    MessageBox.Show("Report save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No datafound !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
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
                GenerateReport(fileName);//===========generate Report==========

                if (globalVarLms.licenseType == "Demo")
                {
                    string tempName = Path.GetFileName(fileName);
                    string tempFile = fileName.Replace(tempName, "tempPdf.pdf");
                    File.Move(fileName, tempFile);
                    byte[] pdfBytes = File.ReadAllBytes(tempFile);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, Encoding.ASCII.EncodingName, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    File.WriteAllBytes(fileName, AddWatermark(pdfBytes, baseFont));
                    File.Delete(tempFile);
                }

                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printDialog.PrinterSettings.PrinterName,
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
        }

        private void GenerateReportHeader(Document pdfToCreate, PdfWriter pdWriter, BaseFont baseFont, BaseFont baseFontBold)
        {
            bool isFullName = Properties.Settings.Default.reportFullName,
                isAddress = Properties.Settings.Default.reportAddress, 
                isContact = Properties.Settings.Default.reportContact,
                isMail = Properties.Settings.Default.reportMail,
                isWebsite = Properties.Settings.Default.reportSite;

            DateTime startDate = dtpFrom.Value, endDate = dtpTo.Value;
            string librarianName = globalVarLms.currentUserName;
            System.Drawing.Image instLogo = null;
            string instName = "", instShortName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "";

            if (Properties.Settings.Default.sqliteDatabase)
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
                        instShortName = dataReader["instShortName"].ToString();
                        instAddress = dataReader["instAddress"].ToString();
                        instContact = dataReader["instContact"].ToString();
                        instWebsite = dataReader["instWebsite"].ToString();
                        instMail = dataReader["instMail"].ToString();

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
                            instShortName = dataReader["instShortName"].ToString();
                            instAddress = dataReader["instAddress"].ToString();
                            instContact = dataReader["instContact"].ToString();
                            instWebsite = dataReader["instWebsite"].ToString();
                            instMail = dataReader["instMail"].ToString();

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
                    mysqlConn.Close();
                }
                catch
                {

                }
            }

            PdfContentByte pdfContent = pdWriter.DirectContent;

            iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
            instLogoJpg.SetAbsolutePosition(60, pdfToCreate.PageSize.Height - 70f);//================Institute Logo=====
            instLogoJpg.ScaleAbsolute(50f, 50f);
            pdfToCreate.Add(instLogoJpg);

            pdfContent.BeginText();//================Institute Name=====
            pdfContent.SetFontAndSize(baseFontBold, 12);
            pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 30f);
            if (isFullName)
            {
                pdfContent.ShowText(instName);
            }
            else
            {
                pdfContent.ShowText(instShortName);
            }
            pdfContent.EndText();

            if (isAddress)
            {
                pdfContent.BeginText();//================Institute Address=====
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 44f);
                pdfContent.ShowText(instAddress);
                pdfContent.EndText();
            }

            string contactDetails = "";
            if (isContact)
            {
                contactDetails = "Call : " + instContact;
                if (isWebsite)
                {
                    contactDetails = "Call/website : " + instContact + " | " + instWebsite;
                    if (isMail)
                    {
                        contactDetails = "Call/website/email : " + instContact + " | " + instWebsite + " | " + instMail;
                    }
                }
            }
            else if (isWebsite)
            {
                contactDetails = "Website : " + instWebsite;
                if (isMail)
                {
                    contactDetails = "Website/email : " + instWebsite + " | " + instMail;
                }
            }
            else if (isMail)
            {
                contactDetails = "Email : " + instMail;
            }
            if (contactDetails != "")
            {
                pdfContent.BeginText();//================Institute Contact=====
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 56f);
                pdfContent.ShowText(contactDetails);
                pdfContent.EndText();
            }

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
            pdfContent.MoveTo(60, pdfToCreate.PageSize.Height - 80f);
            pdfContent.LineTo(585, pdfToCreate.PageSize.Height - 80f);
            pdfContent.Stroke();

            if (lstbReportType.SelectedItem.ToString() != "Item Wise Data" && 
                lstbReportType.SelectedItem.ToString() != "Subject Wise Data")
            {
                pdfContent.BeginText();//========report Type==============
                pdfContent.SetFontAndSize(baseFontBold, 9);
                pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 100f);
                pdfContent.ShowText("Report Type : ");
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 100f);
                pdfContent.ShowText(lstbReportType.SelectedItem.ToString());
                pdfContent.EndText();

                pdfContent.BeginText();//========report preiod==============
                pdfContent.SetFontAndSize(baseFontBold, 9);
                pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 115f);
                pdfContent.ShowText("Preiod : ");
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 115f);
                string timepreiod = FormatDate.getUserFormat(startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000")) +
                    " to " + FormatDate.getUserFormat(endDate.Day.ToString("00") + "/" + endDate.Month.ToString("00") + "/" + endDate.Year.ToString("0000"));
                pdfContent.ShowText(timepreiod);
                pdfContent.EndText();

                pdfContent.BeginText();//===============Duration==============
                pdfContent.SetFontAndSize(baseFontBold, 9);
                pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 115f);
                pdfContent.ShowText("Duration of Report : ");
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 115f);
                pdfContent.ShowText((Convert.ToInt32((endDate - startDate).TotalDays) + 1).ToString() + " Days");
                pdfContent.EndText();

                pdfContent.BeginText();//========report generated by==============
                pdfContent.SetFontAndSize(baseFontBold, 9);
                pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 130f);
                pdfContent.ShowText("Report Created By : ");
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 130f);
                pdfContent.ShowText(librarianName);
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFont, 9);
                pdfContent.SetTextMatrix(260, pdfToCreate.PageSize.Height - 130f);
                pdfContent.ShowText("using ");
                pdfContent.EndText();

                pdfContent.BeginText();
                pdfContent.SetFontAndSize(baseFontBold, 9);
                pdfContent.SetTextMatrix(285, pdfToCreate.PageSize.Height - 130f);
                pdfContent.ShowText("CodeAchi LMS " + Application.ProductVersion.Substring(0, 3));
                pdfContent.EndText();
            }
        }

        private void GenerateReport(string fileName)
        {
            using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
            {
                Document pdfToCreate = new Document(PageSize.A4);
                pdfToCreate.SetMargins(40, 10, 20, 20);
                PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
               
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                {
                    baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                PdfPCell pdfCell = null;
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFontBold, 9, 0, BaseColor.WHITE);
                iTextSharp.text.Font bodyFont = new iTextSharp.text.Font(baseFont, 7);

                pdfToCreate.Open();

                if (lstbReportType.SelectedItem.ToString() == "Item Wise Data")
                {
                    PdfContentByte pdfContent = pdWriter.DirectContent;
                    PdfPTable pdfTable = new PdfPTable(dgvReport.ColumnCount);
                    int rowCount = 0, columnPos = 0;

                    float[] columnWidth = { 70f, 500f, 80f, 80f, 100f, 100f, 80f };
                    pdfTable.SetTotalWidth(columnWidth);
                    pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                    pdfTable.SpacingBefore = 0;
                    pdfTable.SpacingAfter = 0;

                    //Adding Column
                    columnAdding(pdfTable, pdfCell, headerFont);

                    foreach (DataGridViewRow dataRow in dgvReport.Rows)
                    {
                        columnPos = 0;
                        foreach (DataGridViewCell dgvCell in dataRow.Cells)
                        {
                            pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                            pdfCell.Border = 0;
                            if (rowCount % 2 == 0)
                            {
                                pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                            }
                            else
                            {
                                pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                            }
                            if (columnPos == 0)
                            {
                                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            if (columnPos == 2)
                            {
                                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            pdfTable.AddCell(pdfCell);
                            columnPos++;
                        }
                        rowCount++;
                    }
                    pdfToCreate.Add(pdfTable);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Subject Wise Data")
                {
                    PdfContentByte pdfContent = pdWriter.DirectContent;
                    PdfPTable pdfTable = new PdfPTable(dgvReport.ColumnCount);
                    int rowCount = 0, columnPos = 0;

                    float[] columnWidth = { 70f, 500f, 120f, 120f };
                    pdfTable.SetTotalWidth(columnWidth);
                    pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                    pdfTable.SpacingBefore = 0;
                    pdfTable.SpacingAfter = 0;

                    //Adding Column
                    columnAdding(pdfTable, pdfCell, headerFont);

                    foreach (DataGridViewRow dataRow in dgvReport.Rows)
                    {
                        columnPos = 0;
                        foreach (DataGridViewCell dgvCell in dataRow.Cells)
                        {
                            pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                            pdfCell.Border = 0;
                            if (rowCount % 2 == 0)
                            {
                                pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                            }
                            else
                            {
                                pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                            }
                            if (columnPos == 0)
                            {
                                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            if (columnPos == 2)
                            {
                                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            pdfTable.AddCell(pdfCell);
                            columnPos++;
                        }
                        rowCount++;
                    }
                    pdfToCreate.Add(pdfTable);
                }
                else
                {
                    GenerateReportHeader(pdfToCreate, pdWriter, baseFont,baseFontBold);

                    PdfContentByte pdfContent = pdWriter.DirectContent;
                    if (lstbReportType.SelectedItem.ToString() == "Borrower Data" || lstbReportType.SelectedItem.ToString() == "Items Data")   //============Borrower/Items Category=========
                    {
                        pdfContent.BeginText();//===============Report details==============
                        pdfContent.SetFontAndSize(baseFontBold, 9);
                        pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                        pdfContent.ShowText("Category : ");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 9);
                        pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                        pdfContent.ShowText(cmbCategory.Text);
                        pdfContent.EndText();

                        if (chkbFilter.Checked)
                        {
                            pdfContent.BeginText();//===============Filter details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 160f);
                            pdfContent.ShowText(cmbSearchBy.Text + " : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 160f);
                            pdfContent.ShowText(txtbValue.Text);
                            pdfContent.EndText();
                        }
                        if (lstbReportType.SelectedItem.ToString() == "Items Data" && cmbCategory.Text != "All Category")//============Items Category=========
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Subcategory : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(cmbSubcategory.Text);
                            pdfContent.EndText();
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History") //=========Items Data=======
                    {
                        if (cmbCategory.Text == "Particular Borrower")
                        {
                            pdfContent.BeginText();//===============User details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Member Id : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(txtbValue.Text);
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Member Name : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(brrName);
                            pdfContent.EndText();
                        }
                        else
                        {
                            pdfContent.BeginText();//===============Report details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Category : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(cmbCategory.Text);
                            pdfContent.EndText();
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History") //=========Items Data=======
                    {
                        if (cmbCategory.Text == "Particular Item")
                        {
                            pdfContent.BeginText();//===============Item details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Title of Item : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(itemTitle);
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Accession No. : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(txtbValue.Text);
                            pdfContent.EndText();
                        }
                        else
                        {
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Category : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(cmbCategory.Text);
                            pdfContent.EndText();

                            if (cmbCategory.Text != "All Items")
                            {
                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFontBold, 9);
                                pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                                pdfContent.ShowText("Subcategory : ");
                                pdfContent.EndText();

                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFont, 9);
                                pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                                pdfContent.ShowText(cmbSubcategory.Text);
                                pdfContent.EndText();
                            }
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                        lstbReportType.SelectedItem.ToString() == "Least Active Borrower" ||
                        lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                        lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                    {
                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFontBold, 9);
                        pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                        pdfContent.ShowText("Category : ");
                        pdfContent.EndText();

                        pdfContent.BeginText();
                        pdfContent.SetFontAndSize(baseFont, 9);
                        pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                        pdfContent.ShowText(cmbCategory.Text);
                        pdfContent.EndText();
                        if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                        lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                        {
                            if (cmbCategory.Text != "All Category")
                            {
                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFontBold, 9);
                                pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                                pdfContent.ShowText("Subcategory : ");
                                pdfContent.EndText();

                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFont, 9);
                                pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                                pdfContent.ShowText(cmbSubcategory.Text);
                                pdfContent.EndText();
                            }
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                    {
                        if (cmbCategory.Text != "All Data")
                        {
                            pdfContent.BeginText();//===============User details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Member Id : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(txtbValue.Text);
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Member Name : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(378, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(brrName);
                            pdfContent.EndText();
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Librarian Activity")
                    {
                        if (cmbCategory.Text != "All Librarian")
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select userName from userDetails where userMail=@userMail";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@userMail", txtbValue.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            string userName = "";
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    userName = dataReader["userName"].ToString();
                                }
                            }
                            dataReader.Close();
                            sqltConn.Close();
                            pdfContent.BeginText();//===============User details==============
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(60, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Librarian Id : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(148, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(txtbValue.Text);
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFontBold, 9);
                            pdfContent.SetTextMatrix(310, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText("Librarian Name : ");
                            pdfContent.EndText();

                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, 9);
                            pdfContent.SetTextMatrix(400, pdfToCreate.PageSize.Height - 145f);
                            pdfContent.ShowText(userName);
                            pdfContent.EndText();
                        }
                    }
                    ////========================Generate Pie Chart===============
                    GeneratePieChart();

                    var memoryStream = new MemoryStream();
                    pieChart.SaveImage(memoryStream, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                    iTextSharp.text.Image chartImg = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
                    chartImg.SetAbsolutePosition(90, pdfToCreate.PageSize.Height - 425f);
                    chartImg.ScalePercent(75f, 75f);
                    pdfToCreate.Add(chartImg);

                    ////========================Generate Line Chart===============
                    memoryStream = new MemoryStream();
                    barChart.SaveImage(memoryStream, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                    chartImg = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
                    chartImg.SetAbsolutePosition(60f, 125f);
                    chartImg.ScalePercent(75f, 70f);
                    pdfToCreate.Add(chartImg);

                    ////===========Table adding======================
                    pdfToCreate.NewPage();
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }

                    PdfPTable pdfTable = new PdfPTable(dgvReport.ColumnCount);

                    headerFont = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.WHITE);
                    bodyFont = new iTextSharp.text.Font(baseFont, 5);
                    int rowCount = 0;

                    if (lstbReportType.SelectedItem.ToString() == "Borrower Data" || lstbReportType.SelectedItem.ToString() == "Items Data")   //============Borrower/Items Category=========
                    {
                        pdfTable.TotalWidth = 500;
                        pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                        pdfTable.SpacingBefore = 0;
                        pdfTable.SpacingAfter = 0;
                        pdfTable.WidthPercentage = 100;

                        //Adding Column
                        columnAdding(pdfTable, pdfCell, headerFont);
                        //Adding DataRow
                        int columnPos = 0;
                        foreach (DataGridViewRow dataRow in dgvReport.Rows)
                        {
                            columnPos = 0;
                            foreach (DataGridViewCell dgvCell in dataRow.Cells)
                            {
                                pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                                pdfCell.Border = 0;
                                if (rowCount % 2 == 0)
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                                }
                                else
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                                }
                                if (columnPos == 0)
                                {
                                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                pdfTable.AddCell(pdfCell);
                                columnPos++;
                            }
                            rowCount++;
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                    {
                        if (dgvReport.ColumnCount == 8)
                        {
                            float[] columnWidth = { 80f, 120f, 120f, 120f, 120f, 120f, 200f, 200f };
                            pdfTable.SetTotalWidth(columnWidth);
                        }
                        else if (dgvReport.ColumnCount == 10)
                        {
                            float[] columnWidth = { 65f, 90f, 120f, 200f, 90f, 90f, 90f, 90f, 200f, 200f };
                            pdfTable.SetTotalWidth(columnWidth);
                            pdfTable.WidthPercentage = 100;
                        }

                        pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                        pdfTable.SpacingBefore = 0;
                        pdfTable.SpacingAfter = 0;

                        //Adding Column
                        columnAdding(pdfTable, pdfCell, headerFont);

                        //Adding DataRow
                        int columnPos = 0, columCount = dgvReport.Columns.Count;
                        DateTime tempDate;
                        foreach (DataGridViewRow dataRow in dgvReport.Rows)
                        {
                            columnPos = 0;
                            foreach (DataGridViewCell dgvCell in dataRow.Cells)
                            {
                                if (dgvReport.Columns[1].HeaderText == "Date" && dgvCell.ColumnIndex == 1)
                                {
                                    tempDate = Convert.ToDateTime(dgvCell.Value.ToString());
                                    pdfCell = new PdfPCell(new Phrase(tempDate.ToString("dd/MM/yyyy").Replace("-", "/"), bodyFont));
                                }
                                else
                                {
                                    pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                                }
                                pdfCell.Border = 0;
                                if (rowCount % 2 == 0)
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                                }
                                else
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                                }
                                if (columnPos == 0 || (columCount == 10 && (columnPos == 4 || columnPos == 5 || columnPos == 6 || columnPos == 7)) ||
                                    (columCount == 8 && (columnPos == 2 || columnPos == 3 || columnPos == 4 || columnPos == 5)))
                                {
                                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                }

                                pdfTable.AddCell(pdfCell);
                                columnPos++;
                            }
                            rowCount++;
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Librarian Activity" ||
                        lstbReportType.SelectedItem.ToString() == "Issued Items" ||
                        lstbReportType.SelectedItem.ToString() == "Overdue Items" ||
                        lstbReportType.SelectedItem.ToString() == "Borrower Circulation History" ||
                        lstbReportType.SelectedItem.ToString() == "Item Circulation History" ||
                        lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
                    {
                        if (dgvReport.ColumnCount == 3)
                        {
                            float[] columnWidth = { 70f, 100f, 200f };
                            pdfTable.SetTotalWidth(columnWidth);
                        }
                        else if (dgvReport.ColumnCount == 4)
                        {
                            float[] columnWidth = { 70f, 100f, 200f, 200f };
                            pdfTable.SetTotalWidth(columnWidth);
                        }
                        else if (dgvReport.ColumnCount == 5)
                        {
                            if (lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
                            {
                                float[] columnWidth = { 60f, 120f, 350f, 70f, 320f };
                                pdfTable.SetTotalWidth(columnWidth);
                                pdfTable.WidthPercentage = 100;
                            }
                        }
                        else if (dgvReport.ColumnCount == 6)
                        {
                            if (lstbReportType.SelectedItem.ToString() == "Issued Items")
                            {
                                float[] columnWidth = { 60f, 120f, 220f, 120f, 345f, 120f };
                                pdfTable.SetTotalWidth(columnWidth);
                                pdfTable.WidthPercentage = 100;
                            }
                            else if (lstbReportType.SelectedItem.ToString() == "Overdue Items")
                            {
                                float[] columnWidth = { 60f, 220f, 120f, 345f, 120f, 120f };
                                pdfTable.SetTotalWidth(columnWidth);
                                pdfTable.WidthPercentage = 100;
                            }
                            else
                            {
                                float[] columnWidth = { 70f, 90f, 150f, 520f, 80f, 175f };
                                pdfTable.SetTotalWidth(columnWidth);
                                pdfTable.WidthPercentage = 100;
                            }
                        }
                        else if (dgvReport.ColumnCount == 7)
                        {
                            float[] columnWidth = { 70f, 90f, 140f, 390f, 140f, 80f, 175f };
                            pdfTable.SetTotalWidth(columnWidth);
                            pdfTable.WidthPercentage = 100;
                        }

                        pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                        pdfTable.SpacingBefore = 0;
                        pdfTable.SpacingAfter = 0;

                        //Adding Column
                        columnAdding(pdfTable, pdfCell, headerFont);

                        //Adding DataRow
                        int columnPos = 0;
                        DateTime tempDate;
                        foreach (DataGridViewRow dataRow in dgvReport.Rows)
                        {
                            columnPos = 0;
                            foreach (DataGridViewCell dgvCell in dataRow.Cells)
                            {
                                if (dgvReport.Columns[1].HeaderText == "Date" && dgvCell.ColumnIndex == 1)
                                {
                                    tempDate = Convert.ToDateTime(dgvCell.Value.ToString());
                                    pdfCell = new PdfPCell(new Phrase(tempDate.ToString("dd/MM/yyyy").Replace("-", "/"), bodyFont));
                                }
                                else
                                {
                                    pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                                }

                                pdfCell.Border = 0;
                                if (rowCount % 2 == 0)
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                                }
                                else
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                                }
                                if (columnPos == 0 || (lstbReportType.SelectedItem.ToString() == "Overdue Items" && columnPos == 5))
                                {
                                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                pdfTable.AddCell(pdfCell);
                                columnPos++;
                            }
                            rowCount++;
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                        lstbReportType.SelectedItem.ToString() == "Least Active Borrower" ||
                        lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                        lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                    {
                        if (dgvReport.ColumnCount == 3)
                        {
                            float[] columnWidth = { 70f, 500f, 150f };
                            pdfTable.SetTotalWidth(columnWidth);
                        }
                        else if (dgvReport.ColumnCount == 4)
                        {
                            float[] columnWidth = { 70f, 300f, 150f, 150f };
                            pdfTable.SetTotalWidth(columnWidth);
                        }
                        pdfTable.WriteSelectedRows(0, -1, 0, 0, pdfContent);
                        pdfTable.SpacingBefore = 0;
                        pdfTable.SpacingAfter = 0;

                        //Adding Column
                        columnAdding(pdfTable, pdfCell, headerFont);

                        //Adding DataRow
                        int columnPos = 0, columnCount = dgvReport.Columns.Count;
                        foreach (DataGridViewRow dataRow in dgvReport.Rows)
                        {
                            columnPos = 0;
                            foreach (DataGridViewCell dgvCell in dataRow.Cells)
                            {
                                pdfCell = new PdfPCell(new Phrase(dgvCell.Value.ToString(), bodyFont));
                                pdfCell.Border = 0;
                                if (rowCount % 2 == 0)
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.AntiqueWhite);
                                }
                                else
                                {
                                    pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.White);
                                }
                                if (columnPos == 0 || (columnCount == 3 && columnPos == 2) || (columnCount == 4 && columnPos == 3))
                                {
                                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                pdfTable.AddCell(pdfCell);
                                columnPos++;
                            }
                            rowCount++;
                        }
                    }
                    pdfToCreate.Add(pdfTable);
                }
                pdfToCreate.Close();
                outputStream.Close();
            }
            //=========================Add Page Footer=====================
            AddPageFooter(fileName, globalVarLms.instName);
        }

        private void columnAdding(PdfPTable pdfTable, PdfPCell pdfCell, iTextSharp.text.Font headerFont)
        {
            foreach (DataGridViewColumn dgvColumn in dgvReport.Columns)
            {
                pdfCell = new PdfPCell(new Phrase(dgvColumn.HeaderText, headerFont));
                Color backColor = dgvReport.ColumnHeadersDefaultCellStyle.BackColor;
                pdfCell.BackgroundColor = new iTextSharp.text.BaseColor(backColor);
                pdfCell.BorderColor = BaseColor.WHITE;
                pdfCell.VerticalAlignment = Element.ALIGN_TOP;
                pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(pdfCell);
            }
        }

        private void AddPageFooter( string fileName,string instName)
        {
            byte[] pdfBytes = File.ReadAllBytes(fileName);
            BaseFont baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            if (File.Exists("c:/windows/Fonts/malgun.ttf"))
            {
                baseFont1 = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            iTextSharp.text.Font blackFont = new iTextSharp.text.Font(baseFont1, 9);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfReader pdfReader = new PdfReader(pdfBytes);
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, memoryStream))
                {
                    int totalpages = pdfReader.NumberOfPages;
                    for (int i = 1; i <= totalpages; i++)
                    {
                        //==============Page Footer======================
                        blackFont = new iTextSharp.text.Font(baseFont1, 9);
                        ColumnText.ShowTextAligned(pdfStamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase("Page " + i.ToString() + " of " + totalpages.ToString(), blackFont), 60f, 15f, 0);
                        ColumnText.ShowTextAligned(pdfStamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase("Report Generated using ", blackFont), 200f, 15f, 0);
                        baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                        if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                        {
                            baseFont1 = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        }
                        blackFont = new iTextSharp.text.Font(baseFont1, 9);
                        ColumnText.ShowTextAligned(pdfStamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase("CodeAchi LMS " + Application.ProductVersion.Substring(0, 3), blackFont), 299f, 15f, 0);
                        baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                        if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                        {
                            baseFont1 = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        }
                        blackFont = new iTextSharp.text.Font(baseFont1, 9);
                        ColumnText.ShowTextAligned(pdfStamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(" for ", blackFont), 378f, 15f, 0);
                        baseFont1 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                        if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                        {
                            baseFont1 = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        }
                        blackFont = new iTextSharp.text.Font(baseFont1, 9);
                        ColumnText.ShowTextAligned(pdfStamper.GetUnderContent(i), Element.ALIGN_LEFT, new Phrase(instName, blackFont), 395, 15f, 0);
                    }
                }
                pdfBytes = memoryStream.ToArray();
            }
            File.WriteAllBytes(fileName, pdfBytes);
        }

        private void GeneratePieChart()
        {
            barChart.Series.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                if (lstbReportType.SelectedItem.ToString() == "Borrower Data")   //============Borrower/Items Category=========
                {
                    if (cmbCategory.Text == "All Category")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title("All " + lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);

                        pieChart.Series["Series1"].Points.Clear();
                        barChart.Series.Clear();

                        int pointValue, pointPos = 0;
                        foreach (string catName in cmbCategory.Items)
                        {
                            if (catName != "All Category")
                            {
                                pointValue = 0;
                                if (rdbAll.Checked)
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", catName);
                                    pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                    barChart.Series.Add(catName + " - " + pointValue.ToString());
                                    barChart.Series[pointPos].Points.Add(pointValue);
                                }
                                else
                                {
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@brrCategory", catName);
                                        pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                    barChart.Series.Add(catName + " - " + pointValue.ToString());
                                    barChart.Series[pointPos].Points.Add(pointValue);
                                }
                                pointPos++;
                            }
                        }
                    }
                    else
                    {
                        barChart.Series.Clear();
                        if (chkbFilter.Checked)
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title(cmbCategory.Text, Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);
                            if (rdbAll.Checked)
                            {
                                int pointValue;
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory and " + filteredColumn + "=@fieldName;";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();

                                barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                pointValue = (Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - pointValue);
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others - " + pointValue.ToString();

                                barChart.Series.Add("Others - " + pointValue.ToString());
                                barChart.Series[1].Points.Add(pointValue);
                            }
                            else
                            {
                                int pointValue = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "';";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();
                                barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                int ttlValue = 0;
                                startDate = dtpFrom.Value.Date;
                                tempdate = dtpFrom.Value.Date;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    ttlValue = ttlValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others - " + (ttlValue - pointValue).ToString();
                                barChart.Series.Add("Others - " + (ttlValue - pointValue).ToString());
                                barChart.Series[1].Points.Add(ttlValue - pointValue);
                            }
                        }
                        else
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);
                            if (rdbAll.Checked)
                            {
                                int pointValue;
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory;";
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = "select count(id) from borrowerDetails";
                                sqltCommnd.CommandType = CommandType.Text;
                                pointValue = (Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - pointValue);
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                barChart.Series.Add("Others category - " + pointValue.ToString());
                                barChart.Series[1].Points.Add(pointValue);
                            }
                            else
                            {
                                int pointValue = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                int ttlValue = 0;
                                startDate = dtpFrom.Value.Date;
                                tempdate = dtpFrom.Value.Date;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from borrowerDetails where entryDate='" + filterDate + "';";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    ttlValue = ttlValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + (ttlValue - pointValue).ToString();
                                barChart.Series.Add("Others category - " + (ttlValue - pointValue).ToString());
                                barChart.Series[1].Points.Add(ttlValue - pointValue);
                            }
                        }
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Items Data")
                {
                    if (cmbCategory.Text == "All Category")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title("All " + lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);

                        pieChart.Series["Series1"].Points.Clear();
                        barChart.Series.Clear();

                        int pointValue, pointPos = 0;
                        foreach (string catName in cmbCategory.Items)
                        {
                            if (catName != "All Category")
                            {
                                pointValue = 0;
                                if (rdbAll.Checked)
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", catName);
                                    pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                    barChart.Series.Add(catName + " - " + pointValue.ToString());
                                    barChart.Series[pointPos].Points.Add(pointValue);
                                }
                                else
                                {
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", catName);
                                        pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                    barChart.Series.Add(catName + " - " + pointValue.ToString());
                                    barChart.Series[pointPos].Points.Add(pointValue);
                                }
                                pointPos++;
                            }
                        }
                    }
                    else
                    {
                        barChart.Series.Clear();
                        if (chkbFilter.Checked)
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title(cmbCategory.Text, Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);
                            if (rdbAll.Checked)
                            {
                                int pointValue;
                                sqltCommnd = sqltConn.CreateCommand();
                                if (cmbSubcategory.Text == "All Subcategory")
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and " + filteredColumn + "=@fieldName;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                }
                                else
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and itemSubCat=@itemSubCat and " + filteredColumn + "=@fieldName;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                }
                                pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();

                                barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                sqltCommnd = sqltConn.CreateCommand();
                                if (cmbSubcategory.Text == "All Subcategory")
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                }
                                else
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and itemSubCat=@itemSubCat";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                }
                                pointValue = (Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - pointValue);
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others - " + pointValue.ToString();

                                barChart.Series.Add("Others - " + pointValue.ToString());
                                barChart.Series[1].Points.Add(pointValue);
                            }
                            else
                            {
                                int pointValue = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    }
                                    else
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                    pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(pointValue);
                                pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();
                                barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                barChart.Series[0].Points.Add(pointValue);

                                int ttlValue = 0;
                                startDate = dtpFrom.Value.Date;
                                tempdate = dtpFrom.Value.Date;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    }
                                    else
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                    ttlValue = ttlValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                pieChart.Series["Series1"].Points[1].LegendText = "Others - " + (ttlValue - pointValue).ToString();
                                barChart.Series.Add("Others - " + (ttlValue - pointValue).ToString());
                                barChart.Series[1].Points.Add(ttlValue - pointValue);
                            }
                        }
                        else
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);
                            if (rdbAll.Checked)
                            {
                                int pointValue;
                                sqltCommnd = sqltConn.CreateCommand();
                                if (cmbSubcategory.Text == "All Subcategory")
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);

                                    pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from itemDetails";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    pointValue = (Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - pointValue);
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                    barChart.Series.Add("Others Category - " + pointValue.ToString());
                                    barChart.Series[1].Points.Add(pointValue);
                                }
                                else
                                {
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and itemSubCat=@itemSubCat;";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);

                                    pointValue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbSubcategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat";
                                    sqltCommnd.CommandType = CommandType.Text;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);

                                    pointValue = (Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - pointValue);
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                    barChart.Series.Add("Others Subcategory - " + pointValue.ToString());
                                    barChart.Series[1].Points.Add(pointValue);
                                }
                            }
                            else
                            {
                                int pointValue = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    }
                                    else
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        pointValue = pointValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    }
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                if (cmbSubcategory.Text == "All Subcategory")
                                {
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);
                                }
                                else
                                {
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbSubcategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);
                                }

                                int ttlValue = 0;
                                startDate = dtpFrom.Value.Date;
                                tempdate = dtpFrom.Value.Date;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    sqltCommnd = sqltConn.CreateCommand();
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        ttlValue = ttlValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    }
                                    else
                                    {
                                        sqltCommnd.CommandText = "select count(id) from itemDetails where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                        sqltCommnd.CommandType = CommandType.Text;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        ttlValue = ttlValue + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    }
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                                if (cmbSubcategory.Text == "All Subcategory")
                                {
                                    pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + (ttlValue - pointValue).ToString();
                                    barChart.Series.Add("Others category - " + (ttlValue - pointValue).ToString());
                                    barChart.Series[1].Points.Add(ttlValue - pointValue);
                                }
                                else
                                {
                                    pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others Subcategory - " + (ttlValue - pointValue).ToString();
                                    barChart.Series.Add("Others Subcategory - " + (ttlValue - pointValue).ToString());
                                    barChart.Series[1].Points.Add(ttlValue - pointValue);
                                }
                            }
                        }
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History" || lstbReportType.SelectedItem.ToString() == "Item Circulation History")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Issued - " + ttlIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(ttlReissue);
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Reissued - " + ttlReissue.ToString();
                    pieChart.Series["Series1"].Points.Add(ttlReturned);
                    pieChart.Series["Series1"].Points[2].LegendText = "Total Returned - " + ttlReturned.ToString();

                    barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                    barChart.Series[0].Points.Add(ttlIssued);
                    barChart.Series.Add("Total Reissue - " + ttlReissue.ToString());
                    barChart.Series[1].Points.Add(ttlReissue);
                    barChart.Series.Add("Total Returned - " + ttlReturned.ToString());
                    barChart.Series[2].Points.Add(ttlReturned);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                    lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Active Borrower - " + ttlIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(notIssued);
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Inactive Borrower - " + notIssued.ToString();

                    barChart.Series.Add("Total Active Borrower - " + ttlIssued.ToString());
                    barChart.Series[0].Points.Add(ttlIssued);
                    barChart.Series.Add("Total Inactive Borrower - " + notIssued.ToString());
                    barChart.Series[1].Points.Add(notIssued);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                   lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(notIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Item Type - " + notIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[1].LegendText = "Issued item Type - " + ttlIssued.ToString();

                    barChart.Series.Add("Total Item Type - " + notIssued.ToString());
                    barChart.Series[0].Points.Add(notIssued);
                    barChart.Series.Add("Issued Item Type - " + ttlIssued.ToString());
                    barChart.Series[1].Points.Add(ttlIssued);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(Convert.ToInt32(paidAmount));
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Paid - " + paidAmount.ToString();
                    pieChart.Series["Series1"].Points.Add(Convert.ToInt32(discountAmount));
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Discount - " + discountAmount.ToString();
                    pieChart.Series["Series1"].Points.Add(Convert.ToInt32(remitedAmount));
                    pieChart.Series["Series1"].Points[2].LegendText = "Total Remited - " + remitedAmount.ToString();
                    pieChart.Series["Series1"].Points.Add(Convert.ToInt32(dueAmount));
                    pieChart.Series["Series1"].Points[3].LegendText = "Total Due - " + dueAmount.ToString();

                    barChart.Series.Add("Total Paid - " + paidAmount.ToString());
                    barChart.Series[0].Points.Add(Convert.ToInt32(paidAmount));
                    barChart.Series.Add("Total Discount - " + discountAmount.ToString());
                    barChart.Series[1].Points.Add(Convert.ToInt32(discountAmount));
                    barChart.Series.Add("Total Remited - " + remitedAmount.ToString());
                    barChart.Series[2].Points.Add(Convert.ToInt32(remitedAmount));
                    barChart.Series.Add("Total Due - " + dueAmount.ToString());
                    barChart.Series[3].Points.Add(Convert.ToInt32(dueAmount));
                }
                else if (lstbReportType.SelectedItem.ToString() == "Issued Items")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(notIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Items - " + notIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Issued - " + ttlIssued.ToString();

                    barChart.Series.Add("Total Items - " + notIssued.ToString());
                    barChart.Series[0].Points.Add(notIssued);
                    barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                    barChart.Series[1].Points.Add(ttlIssued);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Overdue Items")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Issued - " + ttlIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(notIssued);
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Overdue - " + notIssued.ToString();

                    barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                    barChart.Series[0].Points.Add(ttlIssued);
                    barChart.Series.Add("Total Overdue - " + notIssued.ToString());
                    barChart.Series[1].Points.Add(notIssued);
                }
                else if (lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
                {
                    barChart.Titles.Clear();
                    pieChart.Titles.Clear();
                    Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                    pieChart.Titles.Add(chartTitle);
                    barChart.Titles.Add(chartTitle);
                    pieChart.Series["Series1"].Points.Clear();

                    pieChart.Series["Series1"].Points.Add(notIssued);
                    pieChart.Series["Series1"].Points[0].LegendText = "Total Items - " + notIssued.ToString();
                    pieChart.Series["Series1"].Points.Add(ttlIssued);
                    pieChart.Series["Series1"].Points[1].LegendText = "Total Lost/Damage - " + ttlIssued.ToString();

                    barChart.Series.Add("Total Items - " + notIssued.ToString());
                    barChart.Series[0].Points.Add(notIssued);
                    barChart.Series.Add("Total Lost/Damage - " + ttlIssued.ToString());
                    barChart.Series[1].Points.Add(ttlIssued);
                }
                pieChart.BorderlineWidth = 0;
                pieChart.Legends[0].Docking = Docking.Bottom;
                pieChart.Legends[0].Alignment = StringAlignment.Center;
                barChart.Legends[0].Docking = Docking.Bottom;
                barChart.Legends[0].Alignment = StringAlignment.Center;
                barChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
                barChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
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
                    MySqlCommand mysqlCmd = null;
                    string queryString = "";
                    if (lstbReportType.SelectedItem.ToString() == "Borrower Data")   //============Borrower/Items Category=========
                    {
                        if (cmbCategory.Text == "All Category")
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title("All " + lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);

                            pieChart.Series["Series1"].Points.Clear();
                            barChart.Series.Clear();

                            int pointValue, pointPos = 0;
                            foreach (string catName in cmbCategory.Items)
                            {
                                if (catName != "All Category")
                                {
                                    pointValue = 0;
                                    if (rdbAll.Checked)
                                    {
                                        queryString = "select count(id) from borrower_details where brrCategory = @brrCategory;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", catName);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                        barChart.Series.Add(catName + " - " + pointValue.ToString());
                                        barChart.Series[pointPos].Points.Add(pointValue);
                                    }
                                    else
                                    {
                                        DateTime startDate = dtpFrom.Value.Date;
                                        DateTime tempdate = dtpFrom.Value.Date;
                                        string filterDate = "";
                                        while (startDate <= dtpTo.Value.Date)
                                        {
                                            filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                            queryString = "select count(id) from borrower_details where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@brrCategory", catName);
                                            mysqlCmd.CommandTimeout = 99999;
                                            pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                            startDate = tempdate.AddDays(1);
                                            tempdate = startDate;
                                        }
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                        barChart.Series.Add(catName + " - " + pointValue.ToString());
                                        barChart.Series[pointPos].Points.Add(pointValue);
                                    }
                                    pointPos++;
                                }
                            }
                        }
                        else
                        {
                            barChart.Series.Clear();
                            if (chkbFilter.Checked)
                            {
                                barChart.Titles.Clear();
                                pieChart.Titles.Clear();
                                Title chartTitle = new Title(cmbCategory.Text, Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                                pieChart.Titles.Add(chartTitle);
                                barChart.Titles.Add(chartTitle);
                                if (rdbAll.Checked)
                                {
                                    int pointValue;
                                    queryString = "select count(id) from borrower_details where brrCategory = @brrCategory and " + filteredColumn + "=@fieldName;";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();

                                    barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    queryString = "select count(id) from borrower_details where brrCategory = @brrCategory";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - pointValue);
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others - " + pointValue.ToString();

                                    barChart.Series.Add("Others - " + pointValue.ToString());
                                    barChart.Series[1].Points.Add(pointValue);
                                }
                                else
                                {
                                    int pointValue = 0;
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        queryString = "select count(id) from borrower_details where brrCategory = @brrCategory and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "';";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    int ttlValue = 0;
                                    startDate = dtpFrom.Value.Date;
                                    tempdate = dtpFrom.Value.Date;
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        queryString = "select count(id) from borrower_details where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        ttlValue = ttlValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others - " + (ttlValue - pointValue).ToString();
                                    barChart.Series.Add("Others - " + (ttlValue - pointValue).ToString());
                                    barChart.Series[1].Points.Add(ttlValue - pointValue);
                                }
                            }
                            else
                            {
                                barChart.Titles.Clear();
                                pieChart.Titles.Clear();
                                Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                                pieChart.Titles.Add(chartTitle);
                                barChart.Titles.Add(chartTitle);
                                if (rdbAll.Checked)
                                {
                                    int pointValue;
                                    queryString = "select count(id) from borrower_details where brrCategory = @brrCategory;";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    queryString = "select count(id) from borrower_details";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - pointValue);
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                    barChart.Series.Add("Others category - " + pointValue.ToString());
                                    barChart.Series[1].Points.Add(pointValue);
                                }
                                else
                                {
                                    int pointValue = 0;
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        queryString = "select count(id) from borrower_details where brrCategory = @brrCategory and entryDate='" + filterDate + "';";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    int ttlValue = 0;
                                    startDate = dtpFrom.Value.Date;
                                    tempdate = dtpFrom.Value.Date;
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        queryString = "select count(id) from borrower_details where entryDate='" + filterDate + "';";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.CommandTimeout = 99999;
                                        ttlValue = ttlValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + (ttlValue - pointValue).ToString();
                                    barChart.Series.Add("Others category - " + (ttlValue - pointValue).ToString());
                                    barChart.Series[1].Points.Add(ttlValue - pointValue);
                                }
                            }
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Items Data")
                    {
                        if (cmbCategory.Text == "All Category")
                        {
                            barChart.Titles.Clear();
                            pieChart.Titles.Clear();
                            Title chartTitle = new Title("All " + lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                            pieChart.Titles.Add(chartTitle);
                            barChart.Titles.Add(chartTitle);

                            pieChart.Series["Series1"].Points.Clear();
                            barChart.Series.Clear();

                            int pointValue, pointPos = 0;
                            foreach (string catName in cmbCategory.Items)
                            {
                                if (catName != "All Category")
                                {
                                    pointValue = 0;
                                    if (rdbAll.Checked)
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", catName);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                        barChart.Series.Add(catName + " - " + pointValue.ToString());
                                        barChart.Series[pointPos].Points.Add(pointValue);
                                    }
                                    else
                                    {
                                        DateTime startDate = dtpFrom.Value.Date;
                                        DateTime tempdate = dtpFrom.Value.Date;
                                        string filterDate = "";
                                        while (startDate <= dtpTo.Value.Date)
                                        {
                                            filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", catName);
                                            mysqlCmd.CommandTimeout = 99999;
                                            pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                            startDate = tempdate.AddDays(1);
                                            tempdate = startDate;
                                        }
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[pointPos].LegendText = catName + " - " + pointValue.ToString();

                                        barChart.Series.Add(catName + " - " + pointValue.ToString());
                                        barChart.Series[pointPos].Points.Add(pointValue);
                                    }
                                    pointPos++;
                                }
                            }
                        }
                        else
                        {
                            barChart.Series.Clear();
                            if (chkbFilter.Checked)
                            {
                                barChart.Titles.Clear();
                                pieChart.Titles.Clear();
                                Title chartTitle = new Title(cmbCategory.Text, Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                                pieChart.Titles.Add(chartTitle);
                                barChart.Titles.Add(chartTitle);
                                if (rdbAll.Checked)
                                {
                                    int pointValue;
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat and " + filteredColumn + "=@fieldName;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    }
                                    else
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat and itemSubCat=@itemSubCat and " + filteredColumn + "=@fieldName;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    }
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();

                                    barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    }
                                    else
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat and itemSubCat=@itemSubCat";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                    mysqlCmd.CommandTimeout = 99999;
                                    pointValue = (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - pointValue);
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others - " + pointValue.ToString();

                                    barChart.Series.Add("Others - " + pointValue.ToString());
                                    barChart.Series[1].Points.Add(pointValue);
                                }
                                else
                                {
                                    int pointValue = 0;
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        if (cmbSubcategory.Text == "All Subcategory")
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                        }
                                        else
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and " + filteredColumn + "=@fieldName and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(pointValue);
                                    pieChart.Series["Series1"].Points[0].LegendText = txtbValue.Text + " - " + pointValue.ToString();
                                    barChart.Series.Add(txtbValue.Text + " - " + pointValue.ToString());
                                    barChart.Series[0].Points.Add(pointValue);

                                    int ttlValue = 0;
                                    startDate = dtpFrom.Value.Date;
                                    tempdate = dtpFrom.Value.Date;
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        if (cmbSubcategory.Text == "All Subcategory")
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        }
                                        else
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                        mysqlCmd.CommandTimeout = 99999;
                                        ttlValue = ttlValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                    pieChart.Series["Series1"].Points[1].LegendText = "Others - " + (ttlValue - pointValue).ToString();
                                    barChart.Series.Add("Others - " + (ttlValue - pointValue).ToString());
                                    barChart.Series[1].Points.Add(ttlValue - pointValue);
                                }
                            }
                            else
                            {
                                barChart.Titles.Clear();
                                pieChart.Titles.Clear();
                                Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                                pieChart.Titles.Add(chartTitle);
                                barChart.Titles.Add(chartTitle);
                                if (rdbAll.Checked)
                                {
                                    int pointValue;
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                        barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                        barChart.Series[0].Points.Add(pointValue);

                                        queryString = "select count(id) from item_details";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - pointValue);
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                        barChart.Series.Add("Others Category - " + pointValue.ToString());
                                        barChart.Series[1].Points.Add(pointValue);
                                    }
                                    else
                                    {
                                        queryString = "select count(id) from item_details where itemCat = @itemCat and itemSubCat=@itemSubCat;";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                        barChart.Series.Add(cmbSubcategory.Text + " - " + pointValue.ToString());
                                        barChart.Series[0].Points.Add(pointValue);

                                        queryString = "select count(id) from item_details where itemCat = @itemCat";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        mysqlCmd.CommandTimeout = 99999;
                                        pointValue = (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - pointValue);
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + pointValue.ToString();
                                        barChart.Series.Add("Others Subcategory - " + pointValue.ToString());
                                        barChart.Series[1].Points.Add(pointValue);
                                    }
                                }
                                else
                                {
                                    int pointValue = 0;
                                    DateTime startDate = dtpFrom.Value.Date;
                                    DateTime tempdate = dtpFrom.Value.Date;
                                    string filterDate = "";
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        if (cmbSubcategory.Text == "All Subcategory")
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.CommandTimeout = 99999;
                                            pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        }
                                        else
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "' and itemSubCat=@itemSubCat;";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                            mysqlCmd.CommandTimeout = 99999;
                                            pointValue = pointValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        }
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                        barChart.Series.Add(cmbCategory.Text + " - " + pointValue.ToString());
                                        barChart.Series[0].Points.Add(pointValue);
                                    }
                                    else
                                    {
                                        pieChart.Series["Series1"].Points.Add(pointValue);
                                        pieChart.Series["Series1"].Points[0].LegendText = cmbCategory.Text + " - " + pointValue.ToString();
                                        barChart.Series.Add(cmbSubcategory.Text + " - " + pointValue.ToString());
                                        barChart.Series[0].Points.Add(pointValue);
                                    }

                                    int ttlValue = 0;
                                    startDate = dtpFrom.Value.Date;
                                    tempdate = dtpFrom.Value.Date;
                                    while (startDate <= dtpTo.Value.Date)
                                    {
                                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                        if (cmbSubcategory.Text == "All Subcategory")
                                        {
                                            queryString = "select count(id) from item_details where entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.CommandTimeout = 99999;
                                            ttlValue = ttlValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        }
                                        else
                                        {
                                            queryString = "select count(id) from item_details where itemCat = @itemCat and entryDate='" + filterDate + "';";
                                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                            mysqlCmd.CommandTimeout = 99999;
                                            ttlValue = ttlValue + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                        }
                                        startDate = tempdate.AddDays(1);
                                        tempdate = startDate;
                                    }
                                    if (cmbSubcategory.Text == "All Subcategory")
                                    {
                                        pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                        pieChart.Series["Series1"].Points[1].LegendText = "Others category - " + (ttlValue - pointValue).ToString();
                                        barChart.Series.Add("Others category - " + (ttlValue - pointValue).ToString());
                                        barChart.Series[1].Points.Add(ttlValue - pointValue);
                                    }
                                    else
                                    {
                                        pieChart.Series["Series1"].Points.Add(ttlValue - pointValue);
                                        pieChart.Series["Series1"].Points[1].LegendText = "Others Subcategory - " + (ttlValue - pointValue).ToString();
                                        barChart.Series.Add("Others Subcategory - " + (ttlValue - pointValue).ToString());
                                        barChart.Series[1].Points.Add(ttlValue - pointValue);
                                    }
                                }
                            }
                        }
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History" || lstbReportType.SelectedItem.ToString() == "Item Circulation History")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Issued - " + ttlIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(ttlReissue);
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Reissued - " + ttlReissue.ToString();
                        pieChart.Series["Series1"].Points.Add(ttlReturned);
                        pieChart.Series["Series1"].Points[2].LegendText = "Total Returned - " + ttlReturned.ToString();

                        barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                        barChart.Series[0].Points.Add(ttlIssued);
                        barChart.Series.Add("Total Reissue - " + ttlReissue.ToString());
                        barChart.Series[1].Points.Add(ttlReissue);
                        barChart.Series.Add("Total Returned - " + ttlReturned.ToString());
                        barChart.Series[2].Points.Add(ttlReturned);
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                        lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Active Borrower - " + ttlIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(notIssued);
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Inactive Borrower - " + notIssued.ToString();

                        barChart.Series.Add("Total Active Borrower - " + ttlIssued.ToString());
                        barChart.Series[0].Points.Add(ttlIssued);
                        barChart.Series.Add("Total Inactive Borrower - " + notIssued.ToString());
                        barChart.Series[1].Points.Add(notIssued);
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                       lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(notIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Item Type - " + notIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[1].LegendText = "Issued item Type - " + ttlIssued.ToString();

                        barChart.Series.Add("Total Item Type - " + notIssued.ToString());
                        barChart.Series[0].Points.Add(notIssued);
                        barChart.Series.Add("Issued Item Type - " + ttlIssued.ToString());
                        barChart.Series[1].Points.Add(ttlIssued);
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(Convert.ToInt32(paidAmount));
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Paid - " + paidAmount.ToString();
                        pieChart.Series["Series1"].Points.Add(Convert.ToInt32(discountAmount));
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Discount - " + discountAmount.ToString();
                        pieChart.Series["Series1"].Points.Add(Convert.ToInt32(remitedAmount));
                        pieChart.Series["Series1"].Points[2].LegendText = "Total Remited - " + remitedAmount.ToString();
                        pieChart.Series["Series1"].Points.Add(Convert.ToInt32(dueAmount));
                        pieChart.Series["Series1"].Points[3].LegendText = "Total Due - " + dueAmount.ToString();

                        barChart.Series.Add("Total Paid - " + paidAmount.ToString());
                        barChart.Series[0].Points.Add(Convert.ToInt32(paidAmount));
                        barChart.Series.Add("Total Discount - " + discountAmount.ToString());
                        barChart.Series[1].Points.Add(Convert.ToInt32(discountAmount));
                        barChart.Series.Add("Total Remited - " + remitedAmount.ToString());
                        barChart.Series[2].Points.Add(Convert.ToInt32(remitedAmount));
                        barChart.Series.Add("Total Due - " + dueAmount.ToString());
                        barChart.Series[3].Points.Add(Convert.ToInt32(dueAmount));
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Issued Items")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(notIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Items - " + notIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Issued - " + ttlIssued.ToString();

                        barChart.Series.Add("Total Items - " + notIssued.ToString());
                        barChart.Series[0].Points.Add(notIssued);
                        barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                        barChart.Series[1].Points.Add(ttlIssued);
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Overdue Items")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Issued - " + ttlIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(notIssued);
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Overdue - " + notIssued.ToString();

                        barChart.Series.Add("Total Issued - " + ttlIssued.ToString());
                        barChart.Series[0].Points.Add(ttlIssued);
                        barChart.Series.Add("Total Overdue - " + notIssued.ToString());
                        barChart.Series[1].Points.Add(notIssued);
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
                    {
                        barChart.Titles.Clear();
                        pieChart.Titles.Clear();
                        Title chartTitle = new Title(lstbReportType.SelectedItem.ToString(), Docking.Top, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), Color.Black);
                        pieChart.Titles.Add(chartTitle);
                        barChart.Titles.Add(chartTitle);
                        pieChart.Series["Series1"].Points.Clear();

                        pieChart.Series["Series1"].Points.Add(notIssued);
                        pieChart.Series["Series1"].Points[0].LegendText = "Total Items - " + notIssued.ToString();
                        pieChart.Series["Series1"].Points.Add(ttlIssued);
                        pieChart.Series["Series1"].Points[1].LegendText = "Total Lost/Damage - " + ttlIssued.ToString();

                        barChart.Series.Add("Total Items - " + notIssued.ToString());
                        barChart.Series[0].Points.Add(notIssued);
                        barChart.Series.Add("Total Lost/Damage - " + ttlIssued.ToString());
                        barChart.Series[1].Points.Add(ttlIssued);
                    }
                    pieChart.BorderlineWidth = 0;
                    pieChart.Legends[0].Docking = Docking.Bottom;
                    pieChart.Legends[0].Alignment = StringAlignment.Center;
                    barChart.Legends[0].Docking = Docking.Bottom;
                    barChart.Legends[0].Alignment = StringAlignment.Center;
                    barChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
                    barChart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    mysqlConn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private static byte[] AddWatermark(byte[] pdfBytes, BaseFont baseFont)
        {
            using (var memoryStream = new MemoryStream(10 * 1024))
            {
                using (var pdfReader = new PdfReader(pdfBytes))
                using (var pdfStamper = new PdfStamper(pdfReader, memoryStream))
                {
                    int pageCount = pdfReader.NumberOfPages;
                    for (int i = 1; i <= pageCount; i++)
                    {
                        var contentBytes = pdfStamper.GetOverContent(i);
                        AddWaterMark(contentBytes, "www.CodeAchi.com", baseFont, 48, 35, new BaseColor(255, 0, 0), pdfReader.GetPageSizeWithRotation(i));
                    }
                    pdfStamper.Close();
                }
                return memoryStream.ToArray();
            }
        }

        public static void AddWaterMark(PdfContentByte contentBytes, string watermarkText, BaseFont baseFont, float fontSize, float angle, BaseColor color, iTextSharp.text.Rectangle realPageSize, iTextSharp.text.Rectangle rect = null)
        {
            var gstate = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.3f };
            contentBytes.SaveState();
            contentBytes.SetGState(gstate);
            contentBytes.SetColorFill(color);
            contentBytes.BeginText();
            contentBytes.SetFontAndSize(baseFont, fontSize);
            var ps = rect ?? realPageSize; /*dc.PdfDocument.PageSize is not always correct*/
            var x = (ps.Right + ps.Left) / 2;
            var y = (ps.Bottom + ps.Top) / 2;
            contentBytes.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, angle);
            contentBytes.EndText();
            contentBytes.RestoreState();
        }

        private void pnlSearch_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.Silver, ButtonBorderStyle.Solid);
        }

        private void pnlField_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Solid);
        }

        private void lstbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblInfo3.Visible = false;
            lblInfo4.Visible = false;
            if (lstbReportType.SelectedIndex != -1)
            {
                btnApply.Enabled = true;
                btnSave_Click(null, null);
                rdbAll.Checked = true;
                rdbDate.Enabled = true;
                dtpFrom.Enabled = false;
                dtpTo.Enabled = false;
                cmbCategory.Enabled = false;
                cmbSubcategory.Items.Clear();
                cmbSubcategory.Enabled = false;
                cmbSearchBy.Items.Clear();
                cmbSearchBy.Enabled = false;
                txtbValue.Enabled = false;
                lnklblFieldAdd.Visible = false;
                chkbFilter.Checked = false;
                chkbFilter.Enabled = false;
                dgvReport.Columns.Clear();
                dgvReport.Rows.Clear();
                selctedColumnsList.Clear();

                if (lstbReportType.SelectedItem.ToString() == "Borrower Data")   //============Borrower data=========
                {
                    panelMostLeast.Visible = false;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    lblValue.Text = "Filter Value :";
                    cmbCategory.Items.Clear();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from borrowerSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                            lnklblFieldAdd.Visible = true;
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
                        string queryString = "select catName from borrower_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                            lnklblFieldAdd.Visible = true;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    //dgvReport.Columns.Clear();
                    //dgvReport.Rows.Clear();
                    //lstbAddedField.Items.Clear();
                    //selctedColumnsList.Clear();
                    //lstbAddedField.Items.Add("Borrower Id");
                    //lstbAddedField.Items.Add("Name");
                    //lstbAddedField.Items.Add("Category");
                    //lstbAddedField.Items.Add("Address");
                    //lstbAddedField.Items.Add("Gender");
                    //lstbAddedField.Items.Add("Email Id");
                    //lstbAddedField.Items.Add("Contact");
                    //lstbAddedField.Items.Add("Membership Duration");
                    //lstbAddedField.Items.Add("Entry Date");

                    //if (dgvReport.Columns.Count == 0)
                    //{
                    //    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    //    dgvColumn.HeaderText = "Sl No.";
                    //    dgvColumn.ValueType = typeof(Int32);
                    //    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    //    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    //    dgvColumn.Width = 80;
                    //    dgvReport.Columns.Add(dgvColumn);
                    //}
                    //foreach (string selectedColumn in lstbAddedField.Items)
                    //{
                    //    dgvReport.Columns.Add(selectedColumn, selectedColumn);
                    //}

                    //selctedColumnsList.Add("brrId");
                    //selctedColumnsList.Add("brrName");
                    //selctedColumnsList.Add("brrCategory");
                    //selctedColumnsList.Add("brrAddress");
                    //selctedColumnsList.Add("brrGender");
                    //selctedColumnsList.Add("brrMailId");
                    //selctedColumnsList.Add("brrContact");
                    //selctedColumnsList.Add("mbershipDuration");
                    //selctedColumnsList.Add("entryDate");
                }
                else if (lstbReportType.SelectedItem.ToString() == "Items Data") //=========Items Data=======
                {
                    panelMostLeast.Visible = false;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    lblValue.Text = "Filter Value :";
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Category");
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from itemSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                        cmbCategory.Items.AddRange(catList.ToArray());
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
                        string queryString = "select catName from item_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                        cmbCategory.Items.AddRange(catList.ToArray());
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    cmbCategory.SelectedIndex = 0;
                    cmbCategory.Enabled = true;
                    lnklblFieldAdd.Visible = true;
                }
                else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History")
                {
                    panelMostLeast.Visible = false;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    lblValue.Text = "Borrower Id :";

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
                                    lblValue.Text = fieldValue.Replace(fieldName + "=", "")+" :";
                                }
                            }
                        }
                    }
                    cmbCategory.Items.Clear();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from borrowerSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Borrower");
                            cmbCategory.Items.Add("Particular Borrower");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
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
                        string queryString = "select catName from borrower_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Borrower");
                            cmbCategory.Items.Add("Particular Borrower");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Item Circulation History")
                {
                    panelMostLeast.Visible = false;
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    lblValue.Text = "Accession No :";
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
                                    lblValue.Text = fieldValue.Replace(fieldName + "=", "")+" :";
                                }
                            }
                        }
                    }
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Items");
                    cmbCategory.Items.Add("Particular Item");
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from itemSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                        dataReader.Close();
                        sqltConn.Close();
                        cmbCategory.Items.AddRange(catList.ToArray());
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
                        string queryString = "select catName from item_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                        dataReader.Close();
                        mysqlConn.Close();
                        cmbCategory.Items.AddRange(catList.ToArray());
                    }
                    cmbCategory.SelectedIndex = 0;
                    cmbCategory.Enabled = true;
                }
                else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                    lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                {
                    dgvReport.Columns.Clear();
                    panelMostLeast.Visible = true;
                    numUpto.Value = 10;
                    cmbCategory.Items.Clear();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from borrowerSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                        }
                        cmbTransaction.SelectedIndex = 0;
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
                        string queryString = "select catName from borrower_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                        }
                        cmbTransaction.SelectedIndex = 0;
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                    lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                {
                    dgvReport.Columns.Clear();
                    panelMostLeast.Visible = true;
                    numUpto.Value = 10;
                    cmbCategory.Items.Clear();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select catName from itemSettings";
                        sqltCommnd.CommandType = CommandType.Text;
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
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
                        string queryString = "select catName from item_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCategory.Items.Add("All Category");
                            List<string> catList = (from IDataRecord r in dataReader select (string)r["catName"]).ToList();
                            cmbCategory.Items.AddRange(catList.ToArray());
                            cmbCategory.SelectedIndex = 0;
                            cmbCategory.Enabled = true;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    cmbTransaction.SelectedIndex = 0;
                }
                else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                {
                    panelMostLeast.Visible = false;
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Data");
                    cmbCategory.Items.Add("Particular Borrower Data");
                    cmbCategory.Enabled = true;
                    cmbCategory.SelectedIndex = 0;
                }
                else if (lstbReportType.SelectedItem.ToString() == "Librarian Activity")
                {
                    panelMostLeast.Visible = false;
                    cmbCategory.Items.Clear();
                    cmbCategory.Items.Add("All Librarian");
                    cmbCategory.Items.Add("Particular Librarian");
                    cmbCategory.Enabled = true;
                    cmbCategory.SelectedIndex = 0;
                }
                else if (lstbReportType.SelectedItem.ToString() == "Issued Items")
                {
                    panelMostLeast.Visible = false;
                    rdbDate.Enabled = false;

                    dgvReport.Columns.Clear();
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 60;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Issue Date";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Member";
                    dgvColumn.Width = 220;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Accession No";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Item Title";
                    dgvColumn.Width = 345;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Expected Date";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
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
                                    dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblTitle")
                                {
                                    dgvReport.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }

                }
                else if (lstbReportType.SelectedItem.ToString() == "Overdue Items")
                {
                    panelMostLeast.Visible = false;
                    rdbDate.Enabled = false;

                    dgvReport.Columns.Clear();
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 60;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Member";
                    dgvColumn.Width = 220;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Accession No";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Item Title";
                    dgvColumn.Width = 345;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Expected Date";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Days Late";
                    dgvColumn.Width = 120;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
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
                                    dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblTitle")
                                {
                                    dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }

                }
                else if (lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
                {
                    panelMostLeast.Visible = false;
                    rdbDate.Enabled = false;

                    dgvReport.Columns.Clear();
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 60;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Accession No";
                    dgvColumn.Width = 120;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Item Title";
                    dgvColumn.Width = 350;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Status";
                    dgvColumn.Width = 70;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Lost/Damage Comment";
                    dgvColumn.Width = 320;
                    dgvReport.Columns.Add(dgvColumn);
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
                                    dgvReport.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblTitle")
                                {
                                    dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }

                }
                else if (lstbReportType.SelectedItem.ToString() == "Item Wise Data")
                {
                    panelMostLeast.Visible = false;
                    rdbDate.Enabled = false;

                    dgvReport.Columns.Clear();
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 60;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Item Title";
                    dgvColumn.Width = 500;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Issued";
                    dgvColumn.Width = 80;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Lost";
                    dgvColumn.Width = 80;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Damage";
                    dgvColumn.Width = 80;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Available";
                    dgvColumn.Width = 80;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Total";
                    dgvColumn.Width = 80;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    if (FieldSettings.Default.itemEntry != "")
                    {
                        string fieldName = "";
                        string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                        foreach (string fieldValue in valueList)
                        {
                            if (fieldValue != "" && fieldValue != null)
                            {
                                fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                                if (fieldName == "lblTitle")
                                {
                                    dgvReport.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }

                }
                else if (lstbReportType.SelectedItem.ToString() == "Subject Wise Data")
                {
                    panelMostLeast.Visible = false;
                    rdbDate.Enabled = false;

                    dgvReport.Columns.Clear();
                    dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 60;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Subject";
                    dgvColumn.Width = 500;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Total Title";
                    dgvColumn.Width = 120;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Total Accession";
                    dgvColumn.Width = 100;
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvReport.Columns.Add(dgvColumn);
                    if (FieldSettings.Default.itemEntry != "")
                    {
                        string fieldName = "";
                        string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                        foreach (string fieldValue in valueList)
                        {
                            if (fieldValue != "" && fieldValue != null)
                            {
                                fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                                if (fieldName == "lblSubject")
                                {
                                    dgvReport.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex != -1)
            {
                cmbSubcategory.Items.Clear();
                cmbSubcategory.Enabled = false;
                dgvReport.Rows.Clear();
                if (lstbReportType.SelectedItem.ToString() == "Borrower Data")
                {
                    BorrowerDataColumns();
                }
                else if (lstbReportType.SelectedItem.ToString() == "Items Data")
                {
                    if (cmbCategory.Text != "All Category")
                    {
                        cmbSearchBy.Items.Clear();
                        cmbSearchBy.Items.Add("Please select a type...");
                        cmbSearchBy.Items.Add("ISBN");
                        cmbSearchBy.Items.Add("Title");
                        cmbSearchBy.Items.Add("Subject");
                        cmbSearchBy.Items.Add("Author");
                        cmbSearchBy.Items.Add("Classification No");
                        cmbSearchBy.Items.Add("Rack No./Location");
                        cmbSearchBy.Items.Add("No of Pages");
                        cmbSearchBy.Items.Add("Price");

                        if(FieldSettings.Default.itemEntry != "")
                        {
                            string fieldName = "";
                            string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                            foreach (string fieldValue in valueList)
                            {
                                if (fieldValue != "" && fieldValue != null)
                                {
                                    fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                                    if (fieldName == "lblTitle")
                                    {
                                        cmbSearchBy.Items[2] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblSubject")
                                    {
                                        cmbSearchBy.Items[3] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblAuthor")
                                    {
                                        cmbSearchBy.Items[4] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblLocation")
                                    {
                                        cmbSearchBy.Items[6] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblIsbn")
                                    {
                                        cmbSearchBy.Items[1] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblClassification")
                                    {
                                        cmbSearchBy.Items[5] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblPages")
                                    {
                                        cmbSearchBy.Items[7] = fieldValue.Replace(fieldName + "=", "");
                                    }
                                    else if (fieldName == "lblPrice")
                                    {
                                        cmbSearchBy.Items[8] = fieldValue.Replace(fieldName + "=", "");
                                    }
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
                            string queryString = "select capInfo from itemSettings where catName=@catName";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    fieldList = dataReader["capInfo"].ToString().Split('$');
                                }
                                foreach (string fieldName in fieldList)
                                {
                                    if (fieldName != "")
                                    {
                                        cmbSearchBy.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
                                }
                            }
                            dataReader.Close();

                            queryString = "select subCatName from itemSubCategory where catName=@catName";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            cmbSubcategory.Items.Clear();
                            if (dataReader.HasRows)
                            {
                                cmbSubcategory.Items.Add("All Subcategory");
                                List<string> subcatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                                cmbSubcategory.Items.AddRange(subcatList.ToArray());
                                cmbSubcategory.SelectedIndex = 0;
                                cmbSubcategory.Enabled = true;
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
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    fieldList = dataReader["capInfo"].ToString().Split('$');
                                }
                                foreach (string fieldName in fieldList)
                                {
                                    if (fieldName != "")
                                    {
                                        cmbSearchBy.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
                                }
                            }
                            dataReader.Close();

                            queryString = "select subCatName from item_subcategory where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            cmbSubcategory.Items.Clear();
                            if (dataReader.HasRows)
                            {
                                cmbSubcategory.Items.Add("All Subcategory");
                                List<string> subcatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                                cmbSubcategory.Items.AddRange(subcatList.ToArray());
                                cmbSubcategory.SelectedIndex = 0;
                                cmbSubcategory.Enabled = true;
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                    }
                    else
                    {
                        ItemDataColumns();
                        chkbFilter.Checked = false;
                        chkbFilter.Enabled = false;
                        cmbSubcategory.Items.Clear();
                        cmbSubcategory.Enabled = false;
                        cmbSearchBy.Items.Clear();
                        cmbSearchBy.Enabled = false;
                        txtbValue.Clear();
                        txtbValue.Enabled = false;
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History") //=========Items Data=======
                {
                    BorrowerHistoryColumns();
                }
                else if (lstbReportType.SelectedItem.ToString() == "Item Circulation History") //=========Items Data=======
                {
                    ItemHistoryColumns();
                }
                else if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
                    lstbReportType.SelectedItem.ToString() == "Least Circulated Items") //=========Items Data=======
                {
                    if (cmbCategory.Text != "All Category")
                    {
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select subCatName from itemSubCategory where catName=@catName";
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            cmbSubcategory.Items.Clear();
                            if (dataReader.HasRows)
                            {
                                cmbSubcategory.Items.Add("All Subcategory");
                                List<string> subcatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                                cmbSubcategory.Items.AddRange(subcatList.ToArray());
                                cmbSubcategory.SelectedIndex = 0;
                                cmbSubcategory.Enabled = true;
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
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            cmbSubcategory.Items.Clear();
                            if (dataReader.HasRows)
                            {
                                cmbSubcategory.Items.Add("All Subcategory");
                                List<string> subcatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                                cmbSubcategory.Items.AddRange(subcatList.ToArray());
                                cmbSubcategory.SelectedIndex = 0;
                                cmbSubcategory.Enabled = true;
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                    }
                }
                else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
                {
                    PaymentDataColumns();
                }
                else if(lstbReportType.SelectedItem.ToString() == "Librarian Activity")
                {
                    LibrarianActivityColumns();
                }
            }
        }

        private void BorrowerDataColumns()
        {
            dgvReport.Columns.Clear();
            dgvReport.Rows.Clear();
            lstbAddedField.Items.Clear();
            selctedColumnsList.Clear();
            lstbAddedField.Items.Add("Borrower Id");
            lstbAddedField.Items.Add("Name");
            if (cmbCategory.Text == "All Category")
            {
                lstbAddedField.Items.Add("Category");
            }
            lstbAddedField.Items.Add("Address");
            lstbAddedField.Items.Add("Gender");
            lstbAddedField.Items.Add("Email Id");
            lstbAddedField.Items.Add("Contact");
            lstbAddedField.Items.Add("Membership Duration");
            lstbAddedField.Items.Add("Entry Date");

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
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Borrower Id")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            if (lstbAddedField.Items.IndexOf("Category") > -1)
                            {
                                lstbAddedField.Items[lstbAddedField.Items.IndexOf("Category")] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                        else if (fieldName == "lblBrName")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Name")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrAddress")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Address")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblMailId")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Email Id")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblContact")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Contact")] = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }

            if (dgvReport.Columns.Count == 0)
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.ValueType = typeof(Int32);
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
            }
            foreach (string selectedColumn in lstbAddedField.Items)
            {
                dgvReport.Columns.Add(selectedColumn, selectedColumn);
            }

            selctedColumnsList.Add("brrId");
            selctedColumnsList.Add("brrName");
            if (cmbCategory.Text == "All Category")
            {
                selctedColumnsList.Add("brrCategory");
            }
            selctedColumnsList.Add("brrAddress");
            selctedColumnsList.Add("brrGender");
            selctedColumnsList.Add("brrMailId");
            selctedColumnsList.Add("brrContact");
            selctedColumnsList.Add("mbershipDuration");
            selctedColumnsList.Add("entryDate");

            if (cmbCategory.Text != "All Category")
            {
                cmbSearchBy.Items.Clear();
                cmbSearchBy.Items.Add("Please select a type...");
                cmbSearchBy.Items.Add("Name");
                cmbSearchBy.Items.Add("Address");
                cmbSearchBy.Items.Add("Gender");
                cmbSearchBy.Items.Add("Email Id");
                cmbSearchBy.Items.Add("Contact");
                cmbSearchBy.Items.Add("Membership Duration");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select capInfo from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            fieldList = dataReader["capInfo"].ToString().Split('$');
                        }
                        foreach (string fieldName in fieldList)
                        {
                            if (fieldName != "")
                            {
                                cmbSearchBy.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
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
                    string queryString = "select capInfo from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            fieldList = dataReader["capInfo"].ToString().Split('$');
                        }
                        foreach (string fieldName in fieldList)
                        {
                            if (fieldName != "")
                            {
                                cmbSearchBy.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                chkbFilter.Checked = false;
                chkbFilter.Enabled = true;
                if (FieldSettings.Default.borrowerEntry != "")
                {
                    string fieldName = "";
                    string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                    foreach (string fieldValue in valueList)
                    {
                        if (fieldValue != "" && fieldValue != null)
                        {
                            fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                            if (fieldName == "lblBrName")
                            {
                                cmbSearchBy.Items[1] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrAddress")
                            {
                                cmbSearchBy.Items[2] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblMailId")
                            {
                                cmbSearchBy.Items[4] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblContact")
                            {
                                cmbSearchBy.Items[5] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }
            }
            else
            {
                chkbFilter.Checked = false;
                chkbFilter.Enabled = false;
                cmbSearchBy.Items.Clear();
                cmbSearchBy.Enabled = false;
                txtbValue.Clear();
                txtbValue.Enabled = false;
            }
        }

        private void ItemDataColumns()
        {
            lstbAddedField.Items.Clear();
            selctedColumnsList.Clear();
            dgvReport.Columns.Clear();
            lstbAddedField.Items.Add("Accession No");
            if (cmbCategory.Text == "All Category")
            {
                lstbAddedField.Items.Add("Category");
                lstbAddedField.Items.Add("Subcategory");
            }
            else
            {
                if (cmbSubcategory.Text == "All Subcategory")
                {
                    lstbAddedField.Items.Add("Subcategory");
                }
            }
            lstbAddedField.Items.Add("ISBN");
            lstbAddedField.Items.Add("Title");
            lstbAddedField.Items.Add("Subject");
            lstbAddedField.Items.Add("Author");
            lstbAddedField.Items.Add("Classification No");
            lstbAddedField.Items.Add("Rack No./Location");
            lstbAddedField.Items.Add("No of Pages");
            lstbAddedField.Items.Add("Price");
            lstbAddedField.Items.Add("Entry Date");
           
            if (dgvReport.Columns.Count == 0)
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.ValueType = typeof(Int32);
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
            }
            
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
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Accession No")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblCategory")
                        {
                            if (lstbAddedField.Items.IndexOf("Category") > -1)
                            {
                                lstbAddedField.Items[lstbAddedField.Items.IndexOf("Category")] = fieldValue.Replace(fieldName + "=", "");
                            }                                    
                        }
                        else if (fieldName == "lblSubCategory")
                        {
                            if (lstbAddedField.Items.IndexOf("Subcategory") > -1)
                            {
                                lstbAddedField.Items[lstbAddedField.Items.IndexOf("Subcategory")] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                        else if (fieldName == "lblTitle")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Title")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblSubject")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Subject")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Author")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Rack No./Location")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblIsbn")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("ISBN")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblClassification")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Classification No")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblPages")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("No of Pages")] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblPrice")
                        {
                            lstbAddedField.Items[lstbAddedField.Items.IndexOf("Price")] = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
           
            foreach (string selectedColumn in lstbAddedField.Items)
            {
                dgvReport.Columns.Add(selectedColumn, selectedColumn);
            }
            
            selctedColumnsList.Add("itemAccession");
            if (cmbCategory.Text == "All Category")
            {
                selctedColumnsList.Add("itemCat");
                selctedColumnsList.Add("itemSubCat");
            }
            else
            {
                if (cmbSubcategory.Text == "All Subcategory")
                {
                    selctedColumnsList.Add("itemSubCat");
                }
            }
            selctedColumnsList.Add("itemIsbn");
            selctedColumnsList.Add("itemTitle");
            selctedColumnsList.Add("itemSubject");
            selctedColumnsList.Add("itemAuthor");
            selctedColumnsList.Add("itemClassification");
            selctedColumnsList.Add("rackNo");
            selctedColumnsList.Add("totalPages");
            selctedColumnsList.Add("itemPrice");
            selctedColumnsList.Add("entryDate");
        }

        private void BorrowerHistoryColumns()
        {
            dgvReport.Columns.Clear();
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (cmbCategory.Text == "Particular Borrower")
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 70;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                //dgvColumn.DefaultCellStyle.Format = "d";
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Accession No";
                dgvColumn.Width = 140;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Name";
                dgvColumn.Width = 406;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Action";
                dgvColumn.Width = 90;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);

                if (FieldSettings.Default.borrowerEntry != "")
                {
                    string fieldName = "";
                    string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                    foreach (string fieldValue in valueList)
                    {
                        if (fieldValue != "" && fieldValue != null)
                        {
                            fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                            if (fieldName == "lblBrName")
                            {
                                dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }

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
                                dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
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

                    sqltCommnd.CommandText = "select brrId from borrowerDetails";
                    sqltCommnd.CommandType = CommandType.Text;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    brrIdList.Clear();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                        autoCollData.Clear();
                        autoCollData.AddRange(brrIdList.ToArray());
                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.Select();
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
                    string queryString = "select brrId from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    brrIdList.Clear();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                        autoCollData.Clear();
                        autoCollData.AddRange(brrIdList.ToArray());
                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.Select();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
            else
            {
                txtbValue.Clear();
                txtbValue.Enabled = false;
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 70;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower ID";
                dgvColumn.Width = 130;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Name";
                dgvColumn.Width = 286;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Accession No";
                dgvColumn.Width = 130;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Action";
                dgvColumn.Width = 90;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);

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
                                dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrName")
                            {
                                dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }

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
                                dgvReport.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }
                dgvReport.Refresh();
                Application.DoEvents();
            }
        }

        private void ItemHistoryColumns()
        {
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvReport.Columns.Clear();
            if (cmbCategory.Text == "Particular Item")
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 70;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Id";
                dgvColumn.Width = 140;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Name";
                dgvColumn.Width = 406;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Action";
                dgvColumn.Width = 90;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);
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
                                dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrName")
                            {
                                dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
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

                    sqltCommnd.CommandText = "select itemAccession from itemDetails";
                    sqltCommnd.CommandType = CommandType.Text;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    brrIdList.Clear();
                    if (dataReader.HasRows)
                    {
                        List<string> itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                        autoCollData.Clear();
                        autoCollData.AddRange(itemList.ToArray());
                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.Select();
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
                    brrIdList.Clear();
                    if (dataReader.HasRows)
                    {
                        List<string> itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                        autoCollData.Clear();
                        autoCollData.AddRange(itemList.ToArray());
                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.Select();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                cmbSubcategory.Items.Clear();
                cmbSubcategory.Enabled = false;
            }
            else
            {
                txtbValue.Clear();
                txtbValue.Enabled = false;
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 70;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Accession No";
                dgvColumn.Width = 130;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Item Title";
                dgvColumn.Width = 286;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Id";
                dgvColumn.Width = 130;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Action";
                dgvColumn.Width = 90;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);

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
                                dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblTitle")
                            {
                                dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
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
                                dgvReport.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }


                if (cmbCategory.Text!="All Items")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        sqltCommnd.CommandText = "select subCatName from itemSubCategory where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbSubcategory.Items.Clear();
                            cmbSubcategory.Items.Add("All Subcategory");
                            List<string> subCatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                            cmbSubcategory.Items.AddRange(subCatList.ToArray());
                            cmbSubcategory.SelectedIndex = 0;
                            cmbSubcategory.Enabled = true;
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
                        string queryString = "select subCatName from item_subCategory where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbSubcategory.Items.Clear();
                            cmbSubcategory.Items.Add("All Subcategory");
                            List<string> subCatList = (from IDataRecord r in dataReader select (string)r["subCatName"]).ToList();
                            cmbSubcategory.Items.AddRange(subCatList.ToArray());
                            cmbSubcategory.SelectedIndex = 0;
                            cmbSubcategory.Enabled = true;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                else
                {
                    cmbSubcategory.Items.Clear();
                    cmbSubcategory.Enabled = false;
                }
            }
        }

        private void PaymentDataColumns()
        {
            if (cmbCategory.Text == "All Data")
            {
                txtbValue.Clear();
                txtbValue.Enabled = false;

                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvReport.Columns.Clear();
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Id";
                dgvColumn.Width = 120;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Borrower Name";
                dgvColumn.Width = 180;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Total";
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Discount";
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Status";
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Remitted";
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Description";
                dgvColumn.Width = 150;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 156;
                dgvReport.Columns.Add(dgvColumn);
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
                                dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrName")
                            {
                                dgvReport.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }
            }
            else
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select memberId from paymentDetails where isPaid='" + true + "'";
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> memberList = (from IDataRecord r in dataReader
                                                   select (string)r["memberId"]).ToList();
                        autoCollData.AddRange(memberList.ToArray());

                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.SelectAll();
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
                    string queryString = "select memberId from payment_details where isPaid='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> memberList = (from IDataRecord r in dataReader
                                                   select (string)r["memberId"]).ToList();
                        autoCollData.AddRange(memberList.ToArray());

                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.SelectAll();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvReport.Columns.Clear();
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Total";
                dgvColumn.Width = 100;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Discount";
                dgvColumn.Width = 100;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Status";
                dgvColumn.Width = 100;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Remited";
                dgvColumn.Width = 100;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Description";
                dgvColumn.Width = 223;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 223;
                dgvReport.Columns.Add(dgvColumn);
            }
        }

        private void LibrarianActivityColumns()
        {
            if (cmbCategory.Text == "All Librarian")
            {
                txtbValue.Clear();
                txtbValue.Enabled = false;

                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvReport.Columns.Clear();
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 150;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Librarian Id";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Task Description";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);
            }
            else
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select userMail from userDetails";
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> memberList = (from IDataRecord r in dataReader
                                                   select (string)r["userMail"]).ToList();
                        autoCollData.AddRange(memberList.ToArray());

                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.SelectAll();
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
                    string queryString = "select userMail from user_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> memberList = (from IDataRecord r in dataReader
                                                   select (string)r["userMail"]).ToList();
                        autoCollData.AddRange(memberList.ToArray());

                        txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbValue.AutoCompleteCustomSource = autoCollData;
                        txtbValue.Enabled = true;
                        txtbValue.SelectAll();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvReport.Columns.Clear();
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 60;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Date";
                dgvColumn.Width = 150;
                dgvReport.Columns.Add(dgvColumn);
                dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Task Description";
                dgvColumn.Width = 200;
                dgvReport.Columns.Add(dgvColumn);
            }
        }
    
        private void lnklblFieldAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lstbAvailableField.Items.Clear();
            if (lstbReportType.SelectedItem.ToString() == "Borrower Data")
            {
                lstbAvailableField.Items.Add("Borrower Id");
                lstbAvailableField.Items.Add("Name");
                if (cmbCategory.SelectedIndex == 0)
                {
                    lstbAvailableField.Items.Add("Category");
                }
                lstbAvailableField.Items.Add("Address");
                lstbAvailableField.Items.Add("Gender");
                lstbAvailableField.Items.Add("Email Id");
                lstbAvailableField.Items.Add("Contact");
                lstbAvailableField.Items.Add("Membership Duration");
                lstbAvailableField.Items.Add("Entry Date");

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
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Borrower Id")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrCategory")
                            {
                                if (lstbAvailableField.Items.IndexOf("Category") > -1)
                                {
                                    lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Category")] = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                            else if (fieldName == "lblBrName")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Name")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrAddress")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Address")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblMailId")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Email Id")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblContact")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Contact")] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }

                if (cmbCategory.Text != "All Category")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select capInfo from borrowerSettings where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            string[] capinfoList = null;
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.Text != fieldName.Substring(fieldName.IndexOf("_") + 1))
                                        {
                                            lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                        }
                                    }
                                    else
                                    {
                                        lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
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
                        string queryString = "select capInfo from borrower_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string[] capinfoList = null;
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.Text != fieldName.Substring(fieldName.IndexOf("_") + 1))
                                        {
                                            lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                        }
                                    }
                                    else
                                    {
                                        lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
                                }
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            else if (lstbReportType.SelectedItem.ToString() == "Items Data")
            {
                lstbAvailableField.Items.Add("Accession No");
                if (cmbCategory.SelectedIndex == 0)
                {
                    lstbAvailableField.Items.Add("Category");
                    lstbAvailableField.Items.Add("Subcategory");
                }
                else
                {
                    if (cmbSubcategory.SelectedIndex == 0)
                    {
                        lstbAvailableField.Items.Add("Subcategory");
                    }
                }
                lstbAvailableField.Items.Add("ISBN");
                lstbAvailableField.Items.Add("Title");
                lstbAvailableField.Items.Add("Subject");
                lstbAvailableField.Items.Add("Author");
                lstbAvailableField.Items.Add("Classification No");
                lstbAvailableField.Items.Add("Rack No./Location");
                lstbAvailableField.Items.Add("No of Pages");
                lstbAvailableField.Items.Add("Price");
                lstbAvailableField.Items.Add("Entry Date");

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
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Accession No")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblCategory")
                            {
                                if (lstbAvailableField.Items.IndexOf("Category") > -1)
                                {
                                    lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Category")] = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                            else if (fieldName == "lblSubCategory")
                            {
                                if (lstbAvailableField.Items.IndexOf("Subcategory") > -1)
                                {
                                    lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Subcategory")] = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                            else if (fieldName == "lblTitle")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Title")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblSubject")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Subject")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblAuthor")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Author")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblLocation")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Rack No./Location")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblIsbn")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("ISBN")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblClassification")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Classification No")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPages")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("No of Pages")] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPrice")
                            {
                                lstbAvailableField.Items[lstbAvailableField.Items.IndexOf("Price")] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }

                if (cmbCategory.Text != "All Category")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select capInfo from itemSettings where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            string[] capinfoList = null;
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.Text != fieldName.Substring(fieldName.IndexOf("_") + 1))
                                        {
                                            lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                        }
                                    }
                                    else
                                    {
                                        lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
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
                        string queryString = "select capInfo from item_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string[] capinfoList = null;
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.Text != fieldName.Substring(fieldName.IndexOf("_") + 1))
                                        {
                                            lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                        }
                                    }
                                    else
                                    {
                                        lstbAvailableField.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                    }
                                }
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            
            pnlField.Visible = true;
            while(pnlField.Height<239)
            {
                pnlField.Height = pnlField.Height + 1;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            while (pnlField.Height>0)
            {
                pnlField.Height = pnlField.Height - 1;
            }
            pnlField.Visible = false;
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSearchBy.SelectedIndex != -1)
            {
                if (lstbReportType.SelectedItem.ToString() == "Borrower Data")
                {
                    string queryString="";
                    if (cmbSearchBy.SelectedIndex == 3)
                    {
                        queryString = "select distinct brrGender from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearchBy.SelectedIndex==1)
                    {
                        queryString = "select distinct brrName from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearchBy.SelectedIndex == 2)
                    {
                        queryString = "select distinct brrAddress from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearchBy.SelectedIndex == 4)
                    {
                        queryString = "select distinct brrMailId from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearchBy.SelectedIndex == 5)
                    {
                        queryString = "select distinct brrContact from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearchBy.SelectedIndex == 6)
                    {
                        queryString = "select distinct mbershipDuration from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else
                    {
                        List<string> filedNameList = fieldList.ToList();
                        if (filedNameList.IndexOf("1_" + cmbSearchBy.Text) != -1)
                        {
                            queryString = "select distinct addInfo1 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("2_" + cmbSearchBy.Text) != -1)
                        {
                            queryString = "select distinct addInfo2 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("3_" + cmbSearchBy.Text) != -1)
                        {
                            queryString = "select distinct addInfo3 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("4_" + cmbSearchBy.Text) != -1)
                        {
                            queryString = "select distinct addInfo4 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("5_" + cmbSearchBy.Text) != -1)
                        {
                            queryString = "select distinct addInfo5 from borrowerDetails where brrCategory=@brrCategory";
                        }
                    }
                    if(queryString!="")
                    {
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                List<string> valueList = (from IDataRecord r in dataReader select (string)r[0]).ToList();
                                autoCollData.Clear();
                                autoCollData.AddRange(valueList.ToArray());
                                txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                                txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                txtbValue.AutoCompleteCustomSource = autoCollData;
                                txtbValue.Enabled = true;
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
                            queryString =queryString.Replace("borrowerDetails","borrower_details");
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                List<string> valueList = (from IDataRecord r in dataReader select (string)r[0]).ToList();
                                autoCollData.Clear();
                                autoCollData.AddRange(valueList.ToArray());
                                txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                                txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                txtbValue.AutoCompleteCustomSource = autoCollData;
                                txtbValue.Enabled = true;
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                    }
                }
                else if(lstbReportType.SelectedItem.ToString() == "Items Data")
                {
                    if (cmbSubcategory.Text== "All Subcategory")
                    {
                        string queryString = "";
                        if (cmbSearchBy.SelectedIndex == 1)
                        {
                            queryString = "select distinct itemIsbn from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 2)
                        {
                            queryString = "select distinct itemTitle from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 3)
                        {
                            queryString = "select distinct itemSubject from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 4)
                        {
                            queryString = "select distinct itemAuthor from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 5)
                        {
                            queryString = "select distinct itemClassification from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 6)
                        {
                            queryString = "select distinct rackNo from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 7)
                        {
                            queryString = "select distinct totalPages from itemDetails where itemCat=@itemCat";
                        }
                        else if (cmbSearchBy.SelectedIndex == 8)
                        {
                            queryString = "select distinct itemPrice from itemDetails where itemCat=@itemCat";
                        }
                        else
                        {
                            List<string> filedNameList = fieldList.ToList();
                            if (filedNameList.IndexOf("1_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo1 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("2_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo2 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("3_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo3 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("4_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo4 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("5_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo5 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("6_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo6 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("7_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo7 from itemDetails where itemCat=@itemCat";
                            }
                            else if (filedNameList.IndexOf("8_" + cmbSearchBy.Text) != -1)
                            {
                                queryString = "select distinct addInfo8 from itemDetails where itemCat=@itemCat";
                            }
                        }
                        if (queryString != "")
                        {
                            if (Properties.Settings.Default.sqliteDatabase)
                            {
                                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                                if (sqltConn.State == ConnectionState.Closed)
                                {
                                    sqltConn.Open();
                                }
                                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.CommandType = CommandType.Text;
                                sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    List<string> valueList = (from IDataRecord r in dataReader select (string)r[0]).ToList();
                                    autoCollData.Clear();
                                    autoCollData.AddRange(valueList.ToArray());
                                    txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                                    txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                    txtbValue.AutoCompleteCustomSource = autoCollData;
                                    txtbValue.Enabled = true;
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
                                queryString = queryString.Replace("itemDetails", "item_details");
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                mysqlCmd.CommandTimeout = 99999;
                                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    List<string> valueList = (from IDataRecord r in dataReader select (string)r[0]).ToList();
                                    autoCollData.Clear();
                                    autoCollData.AddRange(valueList.ToArray());
                                    txtbValue.AutoCompleteMode = AutoCompleteMode.Suggest;
                                    txtbValue.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                    txtbValue.AutoCompleteCustomSource = autoCollData;
                                    txtbValue.Enabled = true;
                                }
                                dataReader.Close();
                                mysqlConn.Close();
                            }
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstbAvailableField.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a field name from the list.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstbAddedField.Items.IndexOf(lstbAvailableField.SelectedItem) != -1)
            {
                MessageBox.Show("Field already added.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string selectedField = lstbAvailableField.SelectedItem.ToString();
            lstbAddedField.Items.Add(lstbAvailableField.SelectedItem);
            if(dgvReport.Columns.Count==0)
            {
                DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                dgvColumn.HeaderText = "Sl No.";
                dgvColumn.ValueType = typeof(Int32);
                dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Width = 80;
                dgvReport.Columns.Add(dgvColumn);
            }
            dgvReport.Columns.Add(lstbAvailableField.SelectedItem.ToString(),lstbAvailableField.SelectedItem.ToString());
            btnRemove.Enabled = true;
            if (lstbReportType.SelectedItem.ToString() == "Borrower Data")   //============Borrower data=========
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
                            if (fieldValue.Replace(fieldName + "=", "") == selectedField)
                            {
                                if (fieldName == "lblBrId")
                                {
                                    selectedField = "Borrower Id";
                                }
                                else if (fieldName == "lblBrCategory")
                                {
                                    selectedField = "Category";
                                }
                                else if (fieldName == "lblBrName")
                                {
                                    selectedField = "Name";
                                }
                                else if (fieldName == "lblBrAddress")
                                {
                                    selectedField = "Address";
                                }
                                else if (fieldName == "lblMailId")
                                {
                                    selectedField = "Email Id";
                                }
                                else if (fieldName == "lblContact")
                                {
                                    selectedField = "Contact";
                                }
                            }
                        }
                    }
                }
                if (selectedField == "Borrower Id")
                {
                    selctedColumnsList.Add("brrId");
                }
                else if (selectedField == "Name")
                {
                    selctedColumnsList.Add("brrName");
                }
                else if (selectedField == "Category")
                {
                    selctedColumnsList.Add("brrCategory");
                }
                else if (selectedField == "Address")
                {
                    selctedColumnsList.Add("brrAddress");
                }
                else if (selectedField == "Gender")
                {
                    selctedColumnsList.Add("brrGender");
                }
                else if (selectedField == "Email Id")
                {
                    selctedColumnsList.Add("brrMailId");
                }
                else if (selectedField == "Contact")
                {
                    selctedColumnsList.Add("brrContact");
                }
                else if (selectedField == "Membership Duration")
                {
                    selctedColumnsList.Add("mbershipDuration");
                }
                else if (selectedField == "Entry Date")
                {
                    selctedColumnsList.Add("entryDate");
                }
                else
                {
                    if (cmbCategory.Text != "All Category")
                    {
                        string[] capnList = null;
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select capInfo from borrowerSettings where catName=@catName";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                    foreach (string capnName in capnList)
                                    {
                                        if (capnName != "")
                                        {
                                            if (selectedField == capnName.Replace("1_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo1");
                                            }
                                            else if (selectedField == capnName.Replace("2_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo2");
                                            }
                                            else if (selectedField == capnName.Replace("3_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo3");
                                            }
                                            else if (selectedField == capnName.Replace("4_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo4");
                                            }
                                            else if (selectedField == capnName.Replace("5_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo5");
                                            }
                                        }
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
                            string queryString = "select capInfo from borrower_settings where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                    foreach (string capnName in capnList)
                                    {
                                        if (capnName != "")
                                        {
                                            if (selectedField == capnName.Replace("1_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo1");
                                            }
                                            else if (selectedField == capnName.Replace("2_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo2");
                                            }
                                            else if (selectedField == capnName.Replace("3_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo3");
                                            }
                                            else if (selectedField == capnName.Replace("4_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo4");
                                            }
                                            else if (selectedField == capnName.Replace("5_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo5");
                                            }
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                    }
                }
            }
            else if (lstbReportType.SelectedItem.ToString() == "Items Data")
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
                            if (fieldValue.Replace(fieldName + "=", "") == selectedField)
                            {
                                if (fieldName == "lblAccession")
                                {
                                    selectedField = "Accession No";
                                }
                                else if (fieldName == "lblCategory")
                                {
                                    selectedField = "Category";
                                }
                                else if (fieldName == "lblSubCategory")
                                {
                                    selectedField = "Subcategory";
                                }
                                else if (fieldName == "lblTitle")
                                {
                                    selectedField = "Title";
                                }
                                else if (fieldName == "lblSubject")
                                {
                                    selectedField = "Subject";
                                }
                                else if (fieldName == "lblAuthor")
                                {
                                    selectedField = "Author";
                                }
                                else if (fieldName == "lblLocation")
                                {
                                    selectedField = "Rack No./Location";
                                }
                                else if (fieldName == "lblIsbn")
                                {
                                    selectedField = "ISBN";
                                }
                                else if (fieldName == "lblClassification")
                                {
                                    selectedField = "Classification No";
                                }
                                else if (fieldName == "lblPages")
                                {
                                    selectedField = "No of Pages";
                                }
                                else if (fieldName == "lblPrice")
                                {
                                    selectedField = "Price";
                                }
                            }
                        }
                    }
                }

                if (selectedField == "Accession No")
                {
                    selctedColumnsList.Add("itemAccession");
                }
                else if (selectedField == "Category")
                {
                    selctedColumnsList.Add("itemCat");
                }
                else if (selectedField == "Subcategory")
                {
                    selctedColumnsList.Add("itemSubCat");
                }
                else if (selectedField == "ISBN")
                {
                    selctedColumnsList.Add("itemIsbn");
                }
                else if (selectedField == "Title")
                {
                    selctedColumnsList.Add("itemTitle");
                }
                else if (selectedField == "Subject")
                {
                    selctedColumnsList.Add("itemSubject");
                }
                else if (selectedField == "Author")
                {
                    selctedColumnsList.Add("itemAuthor");
                }
                else if (selectedField == "Classification No")
                {
                    selctedColumnsList.Add("itemClassification");
                }
                else if (selectedField == "Rack No./Location")
                {
                    selctedColumnsList.Add("rackNo");
                }
                else if (selectedField == "No of Pages")
                {
                    selctedColumnsList.Add("totalPages");
                }
                else if (selectedField == "Price")
                {
                    selctedColumnsList.Add("itemPrice");
                }
                else if (selectedField == "Entry Date")
                {
                    selctedColumnsList.Add("entryDate");
                }
                else
                {
                    if (cmbCategory.Text != "All Category")
                    {
                        string[] capnList = null;
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "select capInfo from itemSettings where catName=@catName";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                    foreach (string capnName in capnList)
                                    {
                                        if (capnName != "")
                                        {
                                            if (selectedField == capnName.Replace("1_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo1");
                                            }
                                            else if (selectedField == capnName.Replace("2_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo2");
                                            }
                                            else if (selectedField == capnName.Replace("3_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo3");
                                            }
                                            else if (selectedField == capnName.Replace("4_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo4");
                                            }
                                            else if (selectedField == capnName.Replace("5_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo5");
                                            }
                                            else if (selectedField == capnName.Replace("6_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo6");
                                            }
                                            else if (selectedField == capnName.Replace("7_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo7");
                                            }
                                            else if (selectedField == capnName.Replace("8_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo8");
                                            }
                                        }
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
                            string queryString = "select capInfo from item_settings where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                    foreach (string capnName in capnList)
                                    {
                                        if (capnName != "")
                                        {
                                            if (selectedField == capnName.Replace("1_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo1");
                                            }
                                            else if (selectedField == capnName.Replace("2_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo2");
                                            }
                                            else if (selectedField == capnName.Replace("3_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo3");
                                            }
                                            else if (selectedField == capnName.Replace("4_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo4");
                                            }
                                            else if (selectedField == capnName.Replace("5_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo5");
                                            }
                                            else if (selectedField == capnName.Replace("6_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo6");
                                            }
                                            else if (selectedField == capnName.Replace("7_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo7");
                                            }
                                            else if (selectedField == capnName.Replace("8_", ""))
                                            {
                                                selctedColumnsList.Add("addInfo8");
                                            }
                                        }
                                    }
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstbAddedField.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a field name from the list.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dgvReport.Columns.Remove(lstbAddedField.SelectedItem.ToString());
            string removedField = lstbAddedField.SelectedItem.ToString();
            if (dgvReport.Columns.Count==1)
            {
                dgvReport.Columns.Clear();
            }
            if (lstbReportType.SelectedItem.ToString() == "Borrower Data")
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
                            if (fieldValue.Replace(fieldName + "=", "") == removedField)
                            {
                                if (fieldName == "lblBrId")
                                {
                                    removedField = "Borrower Id";
                                }
                                else if (fieldName == "lblBrCategory")
                                {
                                    removedField = "Category";
                                }
                                else if (fieldName == "lblBrName")
                                {
                                    removedField = "Name";
                                }
                                else if (fieldName == "lblBrAddress")
                                {
                                    removedField = "Address";
                                }
                                else if (fieldName == "lblMailId")
                                {
                                    removedField = "Email Id";
                                }
                                else if (fieldName == "lblContact")
                                {
                                    removedField = "Contact";
                                }
                            }
                        }
                    }
                }
               
                if (removedField == "Borrower Id")
                {
                    selctedColumnsList.Remove("brrId");
                }
                else if (removedField == "Name")
                {
                    selctedColumnsList.Remove("brrName");
                }
                else if (removedField == "Category")
                {
                    selctedColumnsList.Remove("brrCategory");
                }
                else if (removedField == "Address")
                {
                    selctedColumnsList.Remove("brrAddress");
                }
                else if (removedField == "Gender")
                {
                    selctedColumnsList.Remove("brrGender");
                }
                else if (removedField == "Email Id")
                {
                    selctedColumnsList.Remove("brrMailId");
                }
                else if (removedField == "Contact")
                {
                    selctedColumnsList.Remove("brrContact");
                }
                else if (removedField == "Membership Duration")
                {
                    selctedColumnsList.Remove("mbershipDuration");
                }
                else if (removedField == "Entry Date")
                {
                    selctedColumnsList.Remove("entryDate");
                }
                else
                {
                    string[] capinfoList = null;
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select capInfo from borrowerSettings where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (lstbAddedField.Items.IndexOf(fieldName.Substring(fieldName.IndexOf("_") + 1)) != -1)
                                    {
                                        if (fieldName.Contains("1_"))
                                        {
                                            selctedColumnsList.Remove("addInfo1");
                                        }
                                        else if (fieldName.Contains("2_"))
                                        {
                                            selctedColumnsList.Remove("addInfo2");
                                        }
                                        else if (fieldName.Contains("3_"))
                                        {
                                            selctedColumnsList.Remove("addInfo3");
                                        }
                                        else if (fieldName.Contains("4_"))
                                        {
                                            selctedColumnsList.Remove("addInfo4");
                                        }
                                        else if (fieldName.Contains("5_"))
                                        {
                                            selctedColumnsList.Remove("addInfo5");
                                        }
                                    }
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
                        string queryString = "select capInfo from borrower_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (lstbAddedField.Items.IndexOf(fieldName.Substring(fieldName.IndexOf("_") + 1)) != -1)
                                    {
                                        if (fieldName.Contains("1_"))
                                        {
                                            selctedColumnsList.Remove("addInfo1");
                                        }
                                        else if (fieldName.Contains("2_"))
                                        {
                                            selctedColumnsList.Remove("addInfo2");
                                        }
                                        else if (fieldName.Contains("3_"))
                                        {
                                            selctedColumnsList.Remove("addInfo3");
                                        }
                                        else if (fieldName.Contains("4_"))
                                        {
                                            selctedColumnsList.Remove("addInfo4");
                                        }
                                        else if (fieldName.Contains("5_"))
                                        {
                                            selctedColumnsList.Remove("addInfo5");
                                        }
                                    }
                                }
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            else if (lstbReportType.SelectedItem.ToString() == "Items Data")
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
                            if (fieldValue.Replace(fieldName + "=", "") == removedField)
                            {
                                if (fieldName == "lblAccession")
                                {
                                    removedField = "Accession No";
                                }
                                else if (fieldName == "lblCategory")
                                {
                                    removedField = "Category";
                                }
                                else if (fieldName == "lblSubCategory")
                                {
                                    removedField = "Subcategory";
                                }
                                else if (fieldName == "lblTitle")
                                {
                                    removedField = "Title";
                                }
                                else if (fieldName == "lblSubject")
                                {
                                    removedField = "Subject";
                                }
                                else if (fieldName == "lblAuthor")
                                {
                                    removedField = "Author";
                                }
                                else if (fieldName == "lblLocation")
                                {
                                    removedField = "Rack No./Location";
                                }
                                else if (fieldName == "lblIsbn")
                                {
                                    removedField = "ISBN";
                                }
                                else if (fieldName == "lblClassification")
                                {
                                    removedField = "Classification No";
                                }
                                else if (fieldName == "lblPages")
                                {
                                    removedField = "No of Pages";
                                }
                                else if (fieldName == "lblPrice")
                                {
                                    removedField = "Price";
                                }
                            }
                        }
                    }
                }

                if (removedField == "Accession No")
                {
                    selctedColumnsList.Remove("itemAccession");
                }
                else if (removedField == "Category")
                {
                    selctedColumnsList.Remove("itemCat");
                }
                else if (removedField == "Subcategory")
                {
                    selctedColumnsList.Remove("itemSubCat");
                }
                else if (removedField == "ISBN")
                {
                    selctedColumnsList.Remove("itemIsbn");
                }
                else if (removedField == "Title")
                {
                    selctedColumnsList.Remove("itemTitle");
                }
                else if (removedField == "Subject")
                {
                    selctedColumnsList.Remove("itemSubject");
                }
                else if (removedField == "Author")
                {
                    selctedColumnsList.Remove("itemAuthor");
                }
                else if (removedField == "Classification No")
                {
                    selctedColumnsList.Remove("itemClassification");
                }
                else if (removedField == "Rack No./Location")
                {
                    selctedColumnsList.Remove("rackNo");
                }
                else if (removedField == "No of Pages")
                {
                    selctedColumnsList.Remove("totalPages");
                }
                else if (removedField == "Price")
                {
                    selctedColumnsList.Remove("itemPrice");
                }
                else if (lstbAddedField.SelectedItem.ToString() == "Entry Date")
                {
                    selctedColumnsList.Remove("entryDate");
                }
                else
                {
                    string[] capinfoList = null;
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select capInfo from itemSettings where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (lstbAddedField.Items.IndexOf(fieldName.Substring(fieldName.IndexOf("_") + 1)) != -1)
                                    {
                                        if (fieldName.Contains("1_"))
                                        {
                                            selctedColumnsList.Remove("addInfo1");
                                        }
                                        else if (fieldName.Contains("2_"))
                                        {
                                            selctedColumnsList.Remove("addInfo2");
                                        }
                                        else if (fieldName.Contains("3_"))
                                        {
                                            selctedColumnsList.Remove("addInfo3");
                                        }
                                        else if (fieldName.Contains("4_"))
                                        {
                                            selctedColumnsList.Remove("addInfo4");
                                        }
                                        else if (fieldName.Contains("5_"))
                                        {
                                            selctedColumnsList.Remove("addInfo5");
                                        }
                                        else if (fieldName.Contains("6_"))
                                        {
                                            selctedColumnsList.Remove("addInfo6");
                                        }
                                        else if (fieldName.Contains("7_"))
                                        {
                                            selctedColumnsList.Remove("addInfo7");
                                        }
                                        else if (fieldName.Contains("8_"))
                                        {
                                            selctedColumnsList.Remove("addInfo8");
                                        }
                                    }
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
                        string queryString = "select capInfo from item_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                capinfoList = dataReader["capInfo"].ToString().Split('$');
                            }
                            foreach (string fieldName in capinfoList)
                            {
                                if (fieldName != "")
                                {
                                    if (lstbAddedField.Items.IndexOf(fieldName.Substring(fieldName.IndexOf("_") + 1)) != -1)
                                    {
                                        if (fieldName.Contains("1_"))
                                        {
                                            selctedColumnsList.Remove("addInfo1");
                                        }
                                        else if (fieldName.Contains("2_"))
                                        {
                                            selctedColumnsList.Remove("addInfo2");
                                        }
                                        else if (fieldName.Contains("3_"))
                                        {
                                            selctedColumnsList.Remove("addInfo3");
                                        }
                                        else if (fieldName.Contains("4_"))
                                        {
                                            selctedColumnsList.Remove("addInfo4");
                                        }
                                        else if (fieldName.Contains("5_"))
                                        {
                                            selctedColumnsList.Remove("addInfo5");
                                        }
                                        else if (fieldName.Contains("6_"))
                                        {
                                            selctedColumnsList.Remove("addInfo6");
                                        }
                                        else if (fieldName.Contains("7_"))
                                        {
                                            selctedColumnsList.Remove("addInfo7");
                                        }
                                        else if (fieldName.Contains("8_"))
                                        {
                                            selctedColumnsList.Remove("addInfo8");
                                        }
                                    }
                                }
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            lstbAddedField.Items.Remove(lstbAddedField.SelectedItem);
            if(lstbAddedField.Items.Count==0)
            {
                btnRemove.Enabled = false;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            panelStatus.Visible = true;
            btnApply.Enabled = false;
            Application.DoEvents();
            if (lstbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                panelStatus.Visible = false;
                btnApply.Enabled = true;
                return;
            }
            dgvReport.Rows.Clear();
            if (lstbReportType.SelectedItem.ToString() == "Borrower Data")   //============Borrower data=========
            {
                if (selctedColumnsList.Count == 0)
                {
                    MessageBox.Show("Please add some field for report.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelStatus.Visible = false;
                    btnApply.Enabled = true;
                    return;
                }
                if (chkbFilter.Checked)
                {
                    if (cmbSearchBy.SelectedIndex == -1 || cmbSearchBy.SelectedIndex == 0)
                    {
                        MessageBox.Show("please select filter type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter a filterin value.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                AllBorrowerData();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Items Data")   //============Borrower data=========
            {
                if (selctedColumnsList.Count == 0)
                {
                    MessageBox.Show("Please add some field for report.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelStatus.Visible = false;
                    btnApply.Enabled = true;
                    return;
                }
                if (chkbFilter.Checked)
                {
                    if (cmbSearchBy.SelectedIndex == -1 || cmbSearchBy.SelectedIndex == 0)
                    {
                        MessageBox.Show("please select filter type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter a filterin value.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                AllItemsData();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Borrower Circulation History") //=========Items Data=======
            {
                if (cmbCategory.Text == "All Borrower")
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    AllBorrowerHistory();
                }
                else if (cmbCategory.Text == "Particular Borrower")
                {
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter a borrower id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    if (autoCollData.IndexOf(txtbValue.Text) == -1)
                    {
                        MessageBox.Show("Borrower doesn,t exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    PrticularBorrowerHistory();
                }
                else
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    BorrowerCategoryHistory();
                }
            }
            else if (lstbReportType.SelectedItem.ToString() == "Item Circulation History") //=========Items Data=======
            {
                if (cmbCategory.Text == "All Items")
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    AllItemsHistory();
                }
                else if (cmbCategory.Text == "Particular Item")
                {
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter an item accession no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    if (autoCollData.IndexOf(txtbValue.Text) == -1)
                    {
                        MessageBox.Show("Item doesn,t exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    PrticularItemHistory();
                }
                else
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                    btnPrint.Enabled = false;
                    btnPdf.Enabled = false;
                    btnCsv.Enabled = false;
                    Cursor = Cursors.WaitCursor;
                    ItemsCategoryHistory();
                }
            }
            else if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower" ||
                lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
            {
                if (numUpto.Value.ToString() == "")
                {
                    MessageBox.Show("Please enter a maximum limit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelStatus.Visible = false;
                    btnApply.Enabled = true;
                    return;
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                MostOrLeastActiveBorrower();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items" ||
               lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
            {
                if (numUpto.Value.ToString() == "")
                {
                    MessageBox.Show("Please enter a maximum limit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    panelStatus.Visible = false;
                    btnApply.Enabled = true;
                    return;
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                MostOrLeastCirculatedItems();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Payment Data")
            {
                if (cmbCategory.Text != "All Data")
                {
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter a borrower id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                PaymentHistory();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Librarian Activity")
            {
                if (cmbCategory.Text != "All Librarian")
                {
                    if (txtbValue.Text == "")
                    {
                        MessageBox.Show("Please enter a librarian id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panelStatus.Visible = false;
                        btnApply.Enabled = true;
                        return;
                    }
                }
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                LibrarianActivities();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Issued Items")
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                IssuedItems();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Overdue Items")
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                OverDueItems();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Lost/Damage Report")
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                LostDamageReport();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Item Wise Data")
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                ItemWiseData();
            }
            else if (lstbReportType.SelectedItem.ToString() == "Subject Wise Data")
            {
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                btnPrint.Enabled = false;
                btnPdf.Enabled = false;
                btnCsv.Enabled = false;
                Cursor = Cursors.WaitCursor;
                SubjectWiseData();
            }
            ttlRecord.Text = 0.ToString();
            Cursor = Cursors.Default;
            Application.DoEvents();
            panelStatus.Visible = false;
            EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
            btnPrint.Enabled = true;
            btnPdf.Enabled = true;
            btnCsv.Enabled = true;
            btnApply.Enabled = true;
        }

        private void chkbFilter_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbFilter.Checked)
            {
                cmbSearchBy.Enabled = true;
                cmbSearchBy.SelectedIndex = 0;
                txtbValue.Enabled = true;
            }
            else
            {
                cmbSearchBy.Enabled = false;
                cmbSearchBy.SelectedIndex = -1;
                txtbValue.Clear();
                txtbValue.Enabled = false;
            }
        }

        private void AllBorrowerData()
        {
            if (selctedColumnsList.Count > 0)
            {
                string selectedColumns = "", combineString;
                string[] columnValues;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    if (cmbCategory.Text == "All Category")//========All category===============
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string queryString = "select " + selectedColumns + " from borrowerDetails";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                SQLiteDataReader dataReader;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from borrowerDetails";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " where entryDate='" + filterDate + "'";
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = queryString;
                                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    else//=============Single categpry===========
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string[] capnList = null;
                            sqltCommnd.CommandText = "select capInfo from borrowerSettings where catName=@catName";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                }
                            }
                            dataReader.Close();

                            string queryString = "select " + selectedColumns + " from borrowerDetails where brrCategory=@brrCategory";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from borrowerDetails where brrCategory=@brrCategory";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " and entryDate='" + filterDate + "'";

                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.SelectedIndex == 1)
                                        {
                                            filteredColumn = "brrName";
                                            queryString = queryString + " and brrName=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 2)
                                        {
                                            filteredColumn = "brrAddress";
                                            queryString = queryString + " and brrAddress=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 3)
                                        {
                                            filteredColumn = "brrGender";
                                            queryString = queryString + " and brrGender=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 4)
                                        {
                                            filteredColumn = "brrMailId";
                                            queryString = queryString + " and brrMailId=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 5)
                                        {
                                            filteredColumn = "brrContact";
                                            queryString = queryString + " and brrContact=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 6)
                                        {
                                            filteredColumn = "mbershipDuration";
                                            queryString = queryString + " and mbershipDuration=@fieldName collate nocase";
                                        }
                                        else
                                        {
                                            foreach (string fieldName in capnList)
                                            {
                                                if (fieldName != "")
                                                {
                                                    if ("1_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo1";
                                                        queryString = queryString + " and addInfo1=@fieldName collate nocase";
                                                    }
                                                    else if ("2_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo2";
                                                        queryString = queryString + " and addInfo2=@fieldName collate nocase";
                                                    }
                                                    else if ("3_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo3";
                                                        queryString = queryString + " and addInfo3=@fieldName collate nocase";
                                                    }
                                                    else if ("4_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo4";
                                                        queryString = queryString + " and addInfo4=@fieldName collate nocase";
                                                    }
                                                    else if ("5_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo5";
                                                        queryString = queryString + " and addInfo5=@fieldName collate nocase";
                                                    }
                                                }
                                            }
                                        }
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                        sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                    }
                                    else
                                    {
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    }
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                if (chkbFilter.Checked)
                                {
                                    if (cmbSearchBy.SelectedIndex == 1)
                                    {
                                        filteredColumn = "brrName";
                                        queryString = queryString + " and brrName=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 2)
                                    {
                                        filteredColumn = "brrAddress";
                                        queryString = queryString + " and brrAddress=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 3)
                                    {
                                        filteredColumn = "brrGender";
                                        queryString = queryString + " and brrGender=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 4)
                                    {
                                        filteredColumn = "brrMailId";
                                        queryString = queryString + " and brrMailId=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 5)
                                    {
                                        filteredColumn = "brrContact";
                                        queryString = queryString + " and brrContact=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 6)
                                    {
                                        filteredColumn = "mbershipDuration";
                                        queryString = queryString + " and mbershipDuration=@fieldName collate nocase";
                                    }
                                    else
                                    {
                                        foreach (string fieldName in capnList)
                                        {
                                            if (fieldName != "")
                                            {
                                                if ("1_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo1";
                                                    queryString = queryString + " and addInfo1=@fieldName collate nocase";
                                                }
                                                else if ("2_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo2";
                                                    queryString = queryString + " and addInfo2=@fieldName collate nocase";
                                                }
                                                else if ("3_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo3";
                                                    queryString = queryString + " and addInfo3=@fieldName collate nocase";
                                                }
                                                else if ("4_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo4";
                                                    queryString = queryString + " and addInfo4=@fieldName collate nocase";
                                                }
                                                else if ("5_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo5";
                                                    queryString = queryString + " and addInfo5=@fieldName collate nocase";
                                                }
                                            }
                                        }
                                    }
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                }
                                else
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                }
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    if (rdbAll.Checked)
                    {
                        sqltCommnd.CommandText = "select [entryDate] from borrowerDetails order by [id] asc limit 1";
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();

                        dtpTo.Value = DateTime.Now.Date;
                    }
                    sqltConn.Close();
                }
                else
                {
                    string queryString = "";
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
                    if (cmbCategory.Text == "All Category")//========All category===============
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            queryString = "select " + selectedColumns + " from borrower_details";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from borrower_details";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " where entryDate='" + filterDate + "'";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    else//=============Single categpry===========
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string[] capnList = null;
                            queryString = "select capInfo from borrower_settings where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                }
                            }
                            dataReader.Close();

                            queryString = "select " + selectedColumns + " from borrower_details where brrCategory=@brrCategory";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from borrower_details where brrCategory=@brrCategory";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " and entryDate='" + filterDate + "'";

                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.SelectedIndex == 1)
                                        {
                                            filteredColumn = "brrName";
                                            queryString = queryString + " and lower(brrName)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 2)
                                        {
                                            filteredColumn = "brrAddress";
                                            queryString = queryString + " and lower(brrAddress)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 3)
                                        {
                                            filteredColumn = "brrGender";
                                            queryString = queryString + " and lower(brrGender)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 4)
                                        {
                                            filteredColumn = "brrMailId";
                                            queryString = queryString + " and lower(brrMailId)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 5)
                                        {
                                            filteredColumn = "brrContact";
                                            queryString = queryString + " and lower(brrContact)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 6)
                                        {
                                            filteredColumn = "mbershipDuration";
                                            queryString = queryString + " and lower(mbershipDuration)=@fieldName";
                                        }
                                        else
                                        {
                                            foreach (string fieldName in capnList)
                                            {
                                                if (fieldName != "")
                                                {
                                                    if ("1_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo1";
                                                        queryString = queryString + " and lower(addInfo1)=@fieldName";
                                                    }
                                                    else if ("2_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo2";
                                                        queryString = queryString + " and lower(addInfo2)=@fieldName";
                                                    }
                                                    else if ("3_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo3";
                                                        queryString = queryString + " and lower(addInfo3)=@fieldName";
                                                    }
                                                    else if ("4_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo4";
                                                        queryString = queryString + " and lower(addInfo4)=@fieldName";
                                                    }
                                                    else if ("5_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo5";
                                                        queryString = queryString + " and lower(addInfo5)=@fieldName";
                                                    }
                                                }
                                            }
                                        }
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text.ToLower());
                                    }
                                    else
                                    {
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    }
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                if (chkbFilter.Checked)
                                {
                                    if (cmbSearchBy.SelectedIndex == 1)
                                    {
                                        filteredColumn = "brrName";
                                        queryString = queryString + " and lower(brrName)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 2)
                                    {
                                        filteredColumn = "brrAddress";
                                        queryString = queryString + " and lower(brrAddress)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 3)
                                    {
                                        filteredColumn = "brrGender";
                                        queryString = queryString + " and lower(brrGender)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 4)
                                    {
                                        filteredColumn = "brrMailId";
                                        queryString = queryString + " and lower(brrMailId)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 5)
                                    {
                                        filteredColumn = "brrContact";
                                        queryString = queryString + " and lower(brrContact)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 6)
                                    {
                                        filteredColumn = "mbershipDuration";
                                        queryString = queryString + " and lower(mbershipDuration)=@fieldName";
                                    }
                                    else
                                    {
                                        foreach (string fieldName in capnList)
                                        {
                                            if (fieldName != "")
                                            {
                                                if ("1_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo1";
                                                    queryString = queryString + " and lower(addInfo1)=@fieldName";
                                                }
                                                else if ("2_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo2";
                                                    queryString = queryString + " and lower(addInfo2)=@fieldName";
                                                }
                                                else if ("3_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo3";
                                                    queryString = queryString + " and lower(addInfo3)=@fieldName";
                                                }
                                                else if ("4_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo4";
                                                    queryString = queryString + " and lower(addInfo4)=@fieldName";
                                                }
                                                else if ("5_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo5";
                                                    queryString = queryString + " and lower(addInfo5)=@fieldName";
                                                }
                                            }
                                        }
                                    }
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text.ToLower());
                                }
                                else
                                {
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                }
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    if (rdbAll.Checked)
                    {
                        queryString = "select entryDate from borrower_details order by id asc limit 1";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();

                        dtpTo.Value = DateTime.Now.Date;
                    }
                    mysqlConn.Close();
                }
                lblInfo1.Visible = true;
                lblInfo2.Visible = false;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;
                lblInfo1.Text = cmbCategory.Text + " : " + dgvReport.Rows.Count.ToString();
                dgvReport.ClearSelection();
            }
            else
            {
                MessageBox.Show("Please add some field name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AllItemsData()
        {
            if (selctedColumnsList.Count > 0)
            {
                string selectedColumns = "", combineString;
                string[] columnValues;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    if (cmbCategory.Text == "All Category")//========All category===============
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string queryString = "select " + selectedColumns + " from itemDetails";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                SQLiteDataReader dataReader;
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from itemDetails";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " where entryDate='" + filterDate + "'";
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = queryString;
                                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    else//=============Single categpry===========
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string[] capnList = null;
                            sqltCommnd.CommandText = "select capInfo from itemSettings where catName=@catName";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                }
                            }
                            dataReader.Close();

                            string queryString = "select " + selectedColumns + " from itemDetails where itemCat=@itemCat";
                            if (cmbSubcategory.Text != "All Subcategory")
                            {
                                queryString = "select " + selectedColumns + " from itemDetails where itemCat=@itemCat and itemSubCat=@itemSubCat";
                            }
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from itemDetails where itemCat=@itemCat";
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        queryString = "select " + selectedColumns + " from itemDetails where itemCat=@itemCat and itemSubCat=@itemSubCat";
                                    }
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " and entryDate='" + filterDate + "'";

                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.SelectedIndex == 1)
                                        {
                                            filteredColumn = "itemIsbn";
                                            queryString = queryString + " and itemIsbn=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 2)
                                        {
                                            filteredColumn = "itemTitle";
                                            queryString = queryString + " and itemTitle=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 3)
                                        {
                                            filteredColumn = "itemSubject";
                                            queryString = queryString + " and itemSubject=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 4)
                                        {
                                            filteredColumn = "itemAuthor";
                                            queryString = queryString + " and itemAuthor=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 5)
                                        {
                                            filteredColumn = "itemClassification";
                                            queryString = queryString + " and itemClassification=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 6)
                                        {
                                            filteredColumn = "rackNo";
                                            queryString = queryString + " and rackNo=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 7)
                                        {
                                            filteredColumn = "totalPages";
                                            queryString = queryString + " and totalPages=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 8)
                                        {
                                            filteredColumn = "itemPrice";
                                            queryString = queryString + " and itemPrice=@fieldName collate nocase";
                                        }
                                        else if (cmbSearchBy.Text == "Entry Date")
                                        {
                                            filteredColumn = "entryDate";
                                            queryString = queryString + " and entryDate=@fieldName collate nocase";
                                        }
                                        else
                                        {
                                            foreach (string fieldName in capnList)
                                            {
                                                if (fieldName != "")
                                                {
                                                    if ("1_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo1";
                                                        queryString = queryString + " and addInfo1=@fieldName collate nocase";
                                                    }
                                                    else if ("2_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo2";
                                                        queryString = queryString + " and addInfo2=@fieldName collate nocase";
                                                    }
                                                    else if ("3_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo3";
                                                        queryString = queryString + " and addInfo3=@fieldName collate nocase";
                                                    }
                                                    else if ("4_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo4";
                                                        queryString = queryString + " and addInfo4=@fieldName collate nocase";
                                                    }
                                                    else if ("5_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo5";
                                                        queryString = queryString + " and addInfo5=@fieldName collate nocase";
                                                    }
                                                    else if ("6_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo6";
                                                        queryString = queryString + " and addInfo6=@fieldName collate nocase";
                                                    }
                                                    else if ("7_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo7";
                                                        queryString = queryString + " and addInfo7=@fieldName collate nocase";
                                                    }
                                                    else if ("8_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo8";
                                                        queryString = queryString + " and addInfo8=@fieldName collate nocase";
                                                    }
                                                }
                                            }
                                        }
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        if (cmbSubcategory.Text != "All Subcategory")
                                        {
                                            sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                        else
                                        {
                                            sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                        }
                                    }
                                    else
                                    {
                                        sqltCommnd = sqltConn.CreateCommand();
                                        sqltCommnd.CommandText = queryString;
                                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        if (cmbSubcategory.Text != "All Subcategory")
                                        {
                                            sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                    }
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                if (chkbFilter.Checked)
                                {
                                    if (cmbSearchBy.SelectedIndex == 1)
                                    {
                                        filteredColumn = "itemIsbn";
                                        queryString = queryString + " and itemIsbn=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 2)
                                    {
                                        filteredColumn = "itemTitle";
                                        queryString = queryString + " and itemTitle=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 3)
                                    {
                                        filteredColumn = "itemSubject";
                                        queryString = queryString + " and itemSubject=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 4)
                                    {
                                        filteredColumn = "itemAuthor";
                                        queryString = queryString + " and itemAuthor=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 5)
                                    {
                                        filteredColumn = "itemClassification";
                                        queryString = queryString + " and itemClassification=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 6)
                                    {
                                        filteredColumn = "rackNo";
                                        queryString = queryString + " and rackNo=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 7)
                                    {
                                        filteredColumn = "totalPages";
                                        queryString = queryString + " and totalPages=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 8)
                                    {
                                        filteredColumn = "itemPrice";
                                        queryString = queryString + " and itemPrice=@fieldName collate nocase";
                                    }
                                    else if (cmbSearchBy.Text == "Entry Date")
                                    {
                                        filteredColumn = "entryDate";
                                        queryString = queryString + " and entryDate=@fieldName collate nocase";
                                    }
                                    else
                                    {
                                        foreach (string fieldName in capnList)
                                        {
                                            if (fieldName != "")
                                            {
                                                if ("1_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo1";
                                                    queryString = queryString + " and addInfo1=@fieldName collate nocase";
                                                }
                                                else if ("2_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo2";
                                                    queryString = queryString + " and addInfo2=@fieldName collate nocase";
                                                }
                                                else if ("3_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo3";
                                                    queryString = queryString + " and addInfo3=@fieldName collate nocase";
                                                }
                                                else if ("4_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo4";
                                                    queryString = queryString + " and addInfo4=@fieldName collate nocase";
                                                }
                                                else if ("5_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo5";
                                                    queryString = queryString + " and addInfo5=@fieldName collate nocase";
                                                }
                                                else if ("6_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo6";
                                                    queryString = queryString + " and addInfo6=@fieldName collate nocase";
                                                }
                                                else if ("7_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo7";
                                                    queryString = queryString + " and addInfo7=@fieldName collate nocase";
                                                }
                                                else if ("8_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo8";
                                                    queryString = queryString + " and addInfo8=@fieldName collate nocase";
                                                }
                                            }
                                        }
                                    }
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                                }
                                else
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = queryString;
                                    sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                }
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    if (rdbAll.Checked)
                    {
                        sqltCommnd.CommandText = "select [entryDate] from itemDetails order by [id] asc limit 1";
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();

                        dtpTo.Value = DateTime.Now.Date;
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
                    string queryString = "";
                    MySqlDataReader dataReader = null;
                    if (cmbCategory.Text == "All Category")//========All category===============
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            queryString = "select " + selectedColumns + " from item_details";
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from item_details";
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " where entryDate='" + filterDate + "'";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    else//=============Single categpry===========
                    {
                        selectedColumns = string.Join(",", selctedColumnsList.ToArray());
                        if (selectedColumns != "")
                        {
                            string[] capnList = null;
                            queryString = "select capInfo from item_settings where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    capnList = dataReader["capInfo"].ToString().Split('$');
                                }
                            }
                            dataReader.Close();

                            queryString = "select " + selectedColumns + " from item_details where itemCat=@itemCat";
                            if (cmbSubcategory.Text != "All Subcategory")
                            {
                                queryString = "select " + selectedColumns + " from item_details where itemCat=@itemCat and itemSubCat=@itemSubCat";
                            }
                            if (rdbDate.Checked)
                            {
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    queryString = "select " + selectedColumns + " from item_details where itemCat=@itemCat";
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        queryString = "select " + selectedColumns + " from item_details where itemCat=@itemCat and itemSubCat=@itemSubCat";
                                    }
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    queryString = queryString + " and entryDate='" + filterDate + "'";

                                    if (chkbFilter.Checked)
                                    {
                                        if (cmbSearchBy.SelectedIndex == 1)
                                        {
                                            filteredColumn = "itemIsbn";
                                            queryString = queryString + " and lower(itemIsbn)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 2)
                                        {
                                            filteredColumn = "itemTitle";
                                            queryString = queryString + " and lower(itemTitle)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 3)
                                        {
                                            filteredColumn = "itemSubject";
                                            queryString = queryString + " and lower(itemSubject)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 4)
                                        {
                                            filteredColumn = "itemAuthor";
                                            queryString = queryString + " and lower(itemAuthor)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 5)
                                        {
                                            filteredColumn = "itemClassification";
                                            queryString = queryString + " and lower(itemClassification)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 6)
                                        {
                                            filteredColumn = "rackNo";
                                            queryString = queryString + " and lower(rackNo)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 7)
                                        {
                                            filteredColumn = "totalPages";
                                            queryString = queryString + " and lower(totalPages)=@fieldName";
                                        }
                                        else if (cmbSearchBy.SelectedIndex == 8)
                                        {
                                            filteredColumn = "itemPrice";
                                            queryString = queryString + " and lower(itemPrice)=@fieldName";
                                        }
                                        else if (cmbSearchBy.Text == "Entry Date")
                                        {
                                            filteredColumn = "entryDate";
                                            queryString = queryString + " and lower(entryDate)=@fieldName";
                                        }
                                        else
                                        {
                                            foreach (string fieldName in capnList)
                                            {
                                                if (fieldName != "")
                                                {
                                                    if ("1_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo1";
                                                        queryString = queryString + " and lower(addInfo1)=@fieldName";
                                                    }
                                                    else if ("2_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo2";
                                                        queryString = queryString + " and lower(addInfo2)=@fieldName";
                                                    }
                                                    else if ("3_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo3";
                                                        queryString = queryString + " and lower(addInfo3)=@fieldName";
                                                    }
                                                    else if ("4_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo4";
                                                        queryString = queryString + " and lower(addInfo4)=@fieldName";
                                                    }
                                                    else if ("5_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo5";
                                                        queryString = queryString + " and lower(addInfo5)=@fieldName";
                                                    }
                                                    else if ("6_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo6";
                                                        queryString = queryString + " and lower(addInfo6)=@fieldName";
                                                    }
                                                    else if ("7_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo7";
                                                        queryString = queryString + " and lower(addInfo7)=@fieldName";
                                                    }
                                                    else if ("8_" + cmbSearchBy.Text == fieldName)
                                                    {
                                                        filteredColumn = "addInfo8";
                                                        queryString = queryString + " and lower(addInfo8)=@fieldName";
                                                    }
                                                }
                                            }
                                        }
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        if (cmbSubcategory.Text != "All Subcategory")
                                        {
                                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                        else
                                        {
                                            mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text.ToLower());
                                        }
                                    }
                                    else
                                    {
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                        if (cmbSubcategory.Text != "All Subcategory")
                                        {
                                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                        }
                                    }
                                    mysqlCmd.CommandTimeout = 99999;
                                    dataReader = mysqlCmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        DataTable dataTable = new DataTable();
                                        dataTable.Load(dataReader);
                                        foreach (DataRow dataRow in dataTable.Rows)
                                        {
                                            combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                            columnValues = combineString.Split('^');
                                            dgvReport.Rows.Add(columnValues);
                                            Application.DoEvents();
                                        }
                                    }
                                    dataReader.Close();
                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }
                            else
                            {
                                if (chkbFilter.Checked)
                                {
                                    if (cmbSearchBy.SelectedIndex == 1)
                                    {
                                        filteredColumn = "itemIsbn";
                                        queryString = queryString + " and lower(itemIsbn)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 2)
                                    {
                                        filteredColumn = "itemTitle";
                                        queryString = queryString + " and lower(itemTitle)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 3)
                                    {
                                        filteredColumn = "itemSubject";
                                        queryString = queryString + " and lower(itemSubject)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 4)
                                    {
                                        filteredColumn = "itemAuthor";
                                        queryString = queryString + " and lower(itemAuthor)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 5)
                                    {
                                        filteredColumn = "itemClassification";
                                        queryString = queryString + " and lower(itemClassification)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 6)
                                    {
                                        filteredColumn = "rackNo";
                                        queryString = queryString + " and lower(rackNo)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 7)
                                    {
                                        filteredColumn = "totalPages";
                                        queryString = queryString + " and lower(totalPages)=@fieldName";
                                    }
                                    else if (cmbSearchBy.SelectedIndex == 8)
                                    {
                                        filteredColumn = "itemPrice";
                                        queryString = queryString + " and lower(itemPrice)=@fieldName";
                                    }
                                    else if (cmbSearchBy.Text == "Entry Date")
                                    {
                                        filteredColumn = "entryDate";
                                        queryString = queryString + " and lower(entryDate)=@fieldName";
                                    }
                                    else
                                    {
                                        foreach (string fieldName in capnList)
                                        {
                                            if (fieldName != "")
                                            {
                                                if ("1_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo1";
                                                    queryString = queryString + " and lower(addInfo1)=@fieldName";
                                                }
                                                else if ("2_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo2";
                                                    queryString = queryString + " and lower(addInfo2)=@fieldName";
                                                }
                                                else if ("3_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo3";
                                                    queryString = queryString + " and lower(addInfo3)=@fieldName";
                                                }
                                                else if ("4_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo4";
                                                    queryString = queryString + " and lower(addInfo4)=@fieldName";
                                                }
                                                else if ("5_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo5";
                                                    queryString = queryString + " and lower(addInfo5)=@fieldName";
                                                }
                                                else if ("6_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo6";
                                                    queryString = queryString + " and lower(addInfo6)=@fieldName";
                                                }
                                                else if ("7_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo7";
                                                    queryString = queryString + " and lower(addInfo7)=@fieldName";
                                                }
                                                else if ("8_" + cmbSearchBy.Text == fieldName)
                                                {
                                                    filteredColumn = "addInfo8";
                                                    queryString = queryString + " and lower(addInfo8)=@fieldName";
                                                }
                                            }
                                        }
                                    }
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text.ToLower());
                                }
                                else
                                {
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                    if (cmbSubcategory.Text != "All Subcategory")
                                    {
                                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                                    }
                                }
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        combineString = dgvReport.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvReport.Rows.Add(columnValues);
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                            }
                        }
                    }
                    if (rdbAll.Checked)
                    {
                        queryString = "select entryDate from item_details order by id asc limit 1";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();

                        dtpTo.Value = DateTime.Now.Date;
                    }
                    mysqlConn.Close();
                }
                lblInfo1.Visible = true;
                lblInfo2.Visible = false;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;
                if (cmbCategory.Text=="All Category")
                {
                    lblInfo1.Text = cmbCategory.Text+" : " + dgvReport.Rows.Count.ToString();
                }
                else
                {
                    if(cmbSubcategory.Text!="All Subcategory")
                    {
                        lblInfo1.Text = cmbSubcategory.Text + " : " + dgvReport.Rows.Count.ToString();
                    }
                    else
                    {
                        lblInfo1.Text = cmbCategory.Text + " : " + dgvReport.Rows.Count.ToString();
                    }
                }
                dgvReport.ClearSelection();
            }
            else
            {
                MessageBox.Show("Please add some field name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AllBorrowerHistory()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                ttlReturned = 0; ttlIssued = 0; ttlReissue = 0;
                SQLiteDataReader dataReader;
                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                            "where issuedItem.brrId=borrowerDetails.brrId";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1,FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                           "where issuedItem.brrId=borrowerDetails.brrId " +
                           "and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.Sort(dgvReport.Columns[1], ListSortDirection.Ascending);
                dgvReport.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";//
                dgvReport.ClearSelection();
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
                ttlReturned = 0; ttlIssued = 0; ttlReissue = 0;
                MySqlDataReader dataReader=null;
                string queryString = "";
                if (rdbAll.Checked)
                {
                    queryString = "select * from issued_item inner join borrower_details " +
                            "where issued_item.brrId=borrower_details.brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        queryString = "select * from issued_item inner join borrower_details " +
                           "where issued_item.brrId=borrower_details.brrId " +
                           "and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issued_item order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.Sort(dgvReport.Columns[1], ListSortDirection.Ascending);
                dgvReport.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy";//
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void BorrowerCategoryHistory()
        {
            ttlReturned = 0; ttlIssued = 0; ttlReissue = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader;
                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                            "where issuedItem.brrId=borrowerDetails.brrId and borrowerDetails.brrCategory=@brrCategory";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                           "where issuedItem.brrId=borrowerDetails.brrId and borrowerDetails.brrCategory=@brrCategory " +
                           "and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
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
                string queryString = "";
                MySqlDataReader dataReader;
                if (rdbAll.Checked)
                {
                    queryString = "select * from issued_item inner join borrower_details " +
                            "where issued_item.brrId=borrower_details.brrId and borrower_details.brrCategory=@brrCategory";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        queryString = "select * from issued_item inner join borrower_details " +
                           "where issued_item.brrId=borrower_details.brrId and borrower_details.brrCategory=@brrCategory " +
                           "and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issued_item order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void PrticularBorrowerHistory()
        {
            string queryString = "";
            dgvReport.Rows.Clear();
            var tempDate = DateTime.Now.Date;
            ttlIssued = 0; ttlReissue = 0; ttlReturned = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                            "where issuedItem.brrId=borrowerDetails.brrId and borrowerDetails.brrId=@brrId";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        sqltCommnd.CommandText = "select * from issuedItem inner join borrowerDetails " +
                           "where issuedItem.brrId=borrowerDetails.brrId and borrowerDetails.brrId=@brrId " +
                           "and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                if (rdbAll.Checked)
                {
                    queryString = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Return : " + ttlReturned.ToString();
                dgvReport.ClearSelection();
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
                if (rdbAll.Checked)
                {
                    queryString = "select * from issued_item inner join borrower_details " +
                            "where issued_item.brrId=borrower_details.brrId and borrower_details.brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        queryString = "select * from issued_item inner join borrower_details " +
                           "where issued_item.brrId=borrower_details.brrId and borrower_details.brrId=@brrId " +
                           "and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                            dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                            "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["itemAccession"].ToString(),
                                        "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issued_item order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Return : " + ttlReturned.ToString();
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void AllItemsHistory()
        {
            ttlReturned = 0; ttlIssued = 0; ttlReissue = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader;
                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                            "where issuedItem.itemAccession=itemDetails.itemAccession";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                           "where issuedItem.itemAccession=itemDetails.itemAccession " +
                           "and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
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
                string queryString = "";
                MySqlDataReader dataReader=null;
                if (rdbAll.Checked)
                {
                    queryString = "select * from issued_item inner join item_details " +
                            "where issued_item.itemAccession=item_details.itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        queryString = "select * from issued_item inner join item_details " +
                           "where issued_item.itemAccession=item_details.itemAccession " +
                           "and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issued_item order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void ItemsCategoryHistory()
        {
            ttlReturned = 0; ttlIssued = 0; ttlReissue = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader;
                if (rdbAll.Checked)
                {
                    if (cmbSubcategory.Text == "All Subcategory")
                    {
                        sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                           "where issuedItem.itemAccession=itemDetails.itemAccession and itemDetails.itemCat=@itemCat";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                    }
                    else
                    {
                        sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                          "where issuedItem.itemAccession=itemDetails.itemAccession and itemDetails.itemCat=@itemCat and itemDetails.itemSubCat=@itemSubCat";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                    }

                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbSubcategory.Text == "All Subcategory")
                        {
                            sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                                "where issuedItem.itemAccession=itemDetails.itemAccession " +
                                "and itemDetails.itemCat=@itemCat (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        }
                        else
                        {
                            sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                                "where issuedItem.itemAccession=itemDetails.itemAccession " +
                                "and itemDetails.itemCat=@itemCat and itemDetails.itemSubCat=@itemSubCat and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                        }

                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
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
                string queryString = "";
                MySqlDataReader dataReader=null;
                if (rdbAll.Checked)
                {
                    if (cmbSubcategory.Text == "All Subcategory")
                    {
                            queryString = "select * from issued_item inner join item_details " +
                           "where issued_item.itemAccession=item_details.itemAccession and item_details.itemCat=@itemCat";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                    }
                    else
                    {
                        queryString = "select * from issued_item inner join item_details " +
                          "where issued_item.itemAccession=item_details.itemAccession and item_details.itemCat=@itemCat and item_details.itemSubCat=@itemSubCat";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                    }
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbSubcategory.Text == "All Subcategory")
                        {
                            queryString = "select * from issued_item inner join item_details " +
                                "where issued_item.itemAccession=item_details.itemAccession " +
                                "and item_details.itemCat=@itemCat (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        }
                        else
                        {
                            queryString = "select * from issued_item inner join item_details " +
                                "where issued_item.itemAccession=item_details.itemAccession " +
                                "and item_details.itemCat=@itemCat and item_details.itemSubCat=@itemSubCat and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                        }
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Returned : " + ttlReturned.ToString();

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issuedItem order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void PrticularItemHistory()
        {
            dgvReport.Rows.Clear();
            ttlIssued = 0; ttlReissue = 0; ttlReturned = 0;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "";
                SQLiteDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                            "where issuedItem.itemAccession=itemDetails.itemAccession and itemDetails.itemAccession=@itemAccession";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbValue.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails " +
                           "where issuedItem.itemAccession=itemDetails.itemAccession " +
                           "and itemDetails.itemAccession=@itemAccession and (issuedItem.issueDate='" + filterDate + "' or issuedItem.reissuedDate='" + filterDate + "' or issuedItem.returnDate='" + filterDate + "')";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                if (rdbAll.Checked)
                {
                    queryString = "select [issueDate] from issuedItem order by [id] asc limit 1";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Return : " + ttlReturned.ToString();
                dgvReport.ClearSelection();
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
                string queryString = "";
                MySqlDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    queryString = "select * from issued_item inner join item_details " +
                            "where issued_item.itemAccession=item_details.itemAccession and item_details.itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbValue.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["issueDate"].ToString() != "" && dataReader["issueDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Issue", dataReader["issuedBy"].ToString());
                                ttlIssued++;
                            }

                            if (dataReader["reissuedDate"].ToString() != "" && dataReader["reissuedDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Reissue", dataReader["reissuedBy"].ToString());
                                ttlReissue++;
                            }

                            if (dataReader["returnDate"].ToString() != "" && dataReader["returnDate"].ToString() != null)
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                    dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                "Return", dataReader["returnedBy"].ToString());
                                ttlReturned++;
                            }
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        queryString = "select * from issued_item inner join item_details " +
                           "where issued_item.itemAccession=item_details.itemAccession " +
                           "and item_details.itemAccession=@itemAccession and (issued_item.issueDate='" + filterDate + "' or issued_item.reissuedDate='" + filterDate + "' or issued_item.returnDate='" + filterDate + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issueDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Issue", dataReader["issuedBy"].ToString());
                                    ttlIssued++;
                                }

                                if (dataReader["reissuedDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Reissue", dataReader["reissuedBy"].ToString());
                                    ttlReissue++;
                                }

                                if (dataReader["returnDate"].ToString() == filterDate)
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()),
                                        dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["brrId"].ToString(),
                                    "Return", dataReader["returnedBy"].ToString());
                                    ttlReturned++;
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                        Application.DoEvents();
                    }
                }

                if (rdbAll.Checked)
                {
                    queryString = "select issueDate from issued_item order by id asc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = true;
                lblInfo4.Visible = false;

                lblInfo1.Text = "Total Issued : " + ttlIssued.ToString();
                lblInfo2.Text = "Total Reissue : " + ttlReissue.ToString();
                lblInfo3.Text = "Total Return : " + ttlReturned.ToString();
                dgvReport.ClearSelection();
                mysqlConn.Close();
            }
        }

        private void MostOrLeastActiveBorrower()
        {
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblInfo3.Visible = false;
            lblInfo4.Visible = false;
            dgvReport.Columns.Clear();
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
            dgvColumn.HeaderText = "Rank";
            dgvColumn.ValueType = typeof(Int32);
            dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.Width = 100;
            dgvReport.Columns.Add(dgvColumn);
            dgvColumn = new DataGridViewTextBoxColumn();
            dgvColumn.HeaderText = "Borrower Name";
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.Width = 300;
            dgvReport.Columns.Add(dgvColumn);
            dgvColumn = new DataGridViewTextBoxColumn();
            dgvColumn.HeaderText = "Borrower Id";
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.Width = 150;
            dgvReport.Columns.Add(dgvColumn);
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
                            dgvReport.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrName")
                        {
                            dgvReport.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
            dgvColumn = new DataGridViewTextBoxColumn();
            if (cmbTransaction.Text == "All Transactions")
            {
                dgvColumn.HeaderText = "Total Transactions";
            }
            else if (cmbTransaction.Text == "Issue")
            {
                dgvColumn.HeaderText = "Total Issue";
            }
            else if (cmbTransaction.Text == "Reissue")
            {
                dgvColumn.HeaderText = "Total Reissue";
            }
            else if (cmbTransaction.Text == "Return")
            {
                dgvColumn.HeaderText = "Total Return";
            }
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvColumn.Width = 150;
            dgvReport.Columns.Add(dgvColumn);
            DataTable tempTable = new DataTable();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbTransaction.Text == "All Transactions")
                    {
                        sqltCommnd.CommandText = "SELECT brrId, COUNT(*) FROM issuedItem GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Issue")
                    {
                        sqltCommnd.CommandText = "SELECT brrId, COUNT(*) FROM issuedItem GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Reissue")
                    {
                        sqltCommnd.CommandText = "SELECT brrId, COUNT(*) FROM issuedItem where reissuedDate is not null GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Return")
                    {
                        sqltCommnd.CommandText = "SELECT brrId, COUNT(*) FROM issuedItem where returnDate is not null GROUP BY brrId";
                    }

                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        tempTable.Load(dataReader);
                    }
                    dataReader.Close();
                    if (tempTable.Columns.Count == 0)
                    {
                        MessageBox.Show("No " + cmbTransaction.Text + " data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (cmbCategory.Text == "All Category")
                    {
                        if (cmbTransaction.Text == "All Transactions")
                        {
                            int ttlTransaction = 0;
                            foreach (DataRow dataRow in tempTable.Rows)
                            {
                                ttlTransaction = 0;
                                ttlTransaction = Convert.ToInt32(dataRow[1].ToString());
                                sqltCommnd = sqltConn.CreateCommand();
                                sqltCommnd.CommandText = "SELECT COUNT(id) FROM issuedItem where brrId='" + dataRow[0].ToString() + "' and reissuedDate is not null";
                                ttlTransaction = ttlTransaction + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                sqltCommnd.CommandText = "SELECT COUNT(id) FROM issuedItem where brrId='" + dataRow[0].ToString() + "' and returnDate is not null";
                                ttlTransaction = ttlTransaction + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                dataRow[1] = ttlTransaction;
                            }
                        }
                        if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "COUNT(*) desc";
                        }
                        else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "COUNT(*) asc";
                        }
                        tempTable = tempTable.DefaultView.ToTable();
                        int upTo = Convert.ToInt32(numUpto.Value);
                        int rankos = 0;
                        string brrName = "";
                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            if (upTo > 0 && rankos == upTo)
                            {
                                break;
                            }
                            sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "SELECT brrName from borrowerDetails where brrId='" + dataRow[0].ToString() + "'";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    brrName = dataReader["brrName"].ToString();
                                }
                            }
                            dataReader.Close();
                            rankos++;
                            dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), dataRow[1].ToString());
                            Application.DoEvents();
                        }
                        dgvReport.ClearSelection();
                        ttlIssued = tempTable.Rows.Count;
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select count(id) from borrowerDetails;";
                        sqltCommnd.CommandType = CommandType.Text;
                        notIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - ttlIssued;
                    }
                    else
                    {
                        sqltCommnd.CommandText = "SELECT brrId FROM borrowerDetails where brrCategory=@brrCategory";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        List<string> brrIdList = new List<string> { };
                        if (dataReader.HasRows)
                        {
                            brrIdList = (from IDataRecord r in dataReader
                                         select (string)r["brrId"]
                            ).ToList();
                        }
                        dataReader.Close();

                        string combineString = "", filterQuery = "", brrName = "";
                        DataView dataView = new DataView();
                        DataTable filteredDataTable = new DataTable();
                        combineString = string.Join("','", brrIdList);
                        filterQuery = "brrId IN ('" + combineString + "')";
                        dataView = tempTable.DefaultView;
                        dataView.RowFilter = filterQuery;
                        filteredDataTable = dataView.ToTable();

                        if (filteredDataTable.Rows.Count > 0)
                        {
                            if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                            {
                                filteredDataTable.DefaultView.Sort = "COUNT(*) desc";
                            }
                            else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                            {
                                filteredDataTable.DefaultView.Sort = "COUNT(*) asc";
                            }
                            filteredDataTable = filteredDataTable.DefaultView.ToTable();
                            int upTo = Convert.ToInt32(numUpto.Value);
                            int ttlTransaction = 0, rankos = 0;
                            foreach (DataRow dataRow in filteredDataTable.Rows)
                            {
                                if (upTo > 0 && rankos == upTo)
                                {
                                    break;
                                }
                                ttlTransaction = Convert.ToInt32(dataRow[1].ToString());
                                if (cmbTransaction.Text == "All Transactions")
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "SELECT COUNT(id) FROM issuedItem where brrId='" + dataRow[0].ToString() + "' and reissuedDate is not null";
                                    ttlTransaction = ttlTransaction + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                    sqltCommnd.CommandText = "SELECT COUNT(id) FROM issuedItem where brrId='" + dataRow[0].ToString() + "' and returnDate is not null";
                                    ttlTransaction = ttlTransaction + Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                                }
                                sqltCommnd.CommandText = "SELECT brrName FROM borrowerDetails where brrId='" + dataRow[0].ToString() + "'";
                                sqltCommnd.CommandType = CommandType.Text;
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        brrName = dataReader[0].ToString();
                                    }
                                }
                                dataReader.Close();
                                rankos++;
                                dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), ttlTransaction);
                                Application.DoEvents();
                            }
                        }
                        dgvReport.ClearSelection();
                        ttlIssued = filteredDataTable.Rows.Count;
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select count(id) from borrowerDetails where brrCategory=@brrCategory;";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        notIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString()) - ttlIssued;
                    }
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DataColumn dtColumn;
                    // Create id column  
                    dtColumn = new DataColumn();
                    dtColumn.ColumnName = "brrId";
                    dtColumn.Caption = "brrId";
                    tempTable.Columns.Add(dtColumn);
                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Int32);
                    dtColumn.ColumnName = "tranCount";
                    dtColumn.Caption = "tranCount";
                    tempTable.Columns.Add(dtColumn);

                    List<string> brrIdList = new List<string> { };
                    if (cmbCategory.Text == "All Category")
                    {
                        sqltCommnd.CommandText = "select brrId from borrowerDetails";
                        sqltCommnd.CommandType = CommandType.Text;
                    }
                    else
                    {
                        sqltCommnd.CommandText = "select brrId from borrowerDetails where brrCategory=@brrCategory";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    }
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                    }
                    dataReader.Close();

                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    DataRow[] selectedRow;
                    int transCount = 0;
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbTransaction.Text == "All Transactions")
                        {
                            sqltCommnd.CommandText = "SELECT brrId,returnDate,reissuedDate FROM issuedItem where issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Issue")
                        {
                            sqltCommnd.CommandText = "SELECT brrId FROM issuedItem where issueDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Reissue")
                        {
                            sqltCommnd.CommandText = "SELECT brrId FROM issuedItem where reissuedDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Return")
                        {
                            sqltCommnd.CommandText = "SELECT brrId FROM issuedItem where returnDate='" + filterDate + "'";
                        }
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (brrIdList.IndexOf(dataReader["brrId"].ToString()) != -1)
                                {
                                    selectedRow = tempTable.Select("brrId='" + dataReader["brrId"].ToString() + "'");
                                    if (selectedRow.Count() > 0)
                                    {
                                        transCount = Convert.ToInt32(selectedRow[0][1].ToString());
                                        transCount++;
                                        selectedRow[0][1] = transCount;
                                    }
                                    else
                                    {
                                        tempTable.Rows.Add(dataReader["brrId"].ToString(), 1);
                                    }
                                }
                            }
                        }
                        dataReader.Close();

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                    }

                    if (tempTable.Rows.Count > 0)
                    {
                        if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "tranCount desc";
                        }
                        else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "tranCount asc";
                        }
                        tempTable = tempTable.DefaultView.ToTable();
                        int upTo = Convert.ToInt32(numUpto.Value);
                        int rankos = 0;

                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            if (upTo > 0 && rankos == upTo)
                            {
                                break;
                            }
                            sqltCommnd.CommandText = "SELECT brrName FROM borrowerDetails where brrId='" + dataRow[0].ToString() + "'";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    brrName = dataReader[0].ToString();
                                }
                            }
                            dataReader.Close();
                            rankos++;
                            dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), dataRow[1].ToString());
                            Application.DoEvents();
                        }
                    }
                    dgvReport.ClearSelection();
                    ttlIssued = tempTable.Rows.Count;
                    notIssued = brrIdList.Count - ttlIssued;
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
                string queryString = "";
                MySqlDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbTransaction.Text == "All Transactions")
                    {
                        queryString = "SELECT brrId, COUNT(*) FROM issued_item GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Issue")
                    {
                        queryString = "SELECT brrId, COUNT(*) FROM issued_item GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Reissue")
                    {
                        queryString = "SELECT brrId, COUNT(*) FROM issued_item where reissuedDate is not null GROUP BY brrId";
                    }
                    else if (cmbTransaction.Text == "Return")
                    {
                        queryString = "SELECT brrId, COUNT(*) FROM issued_item where returnDate is not null GROUP BY brrId";
                    }

                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        tempTable.Load(dataReader);
                    }
                    dataReader.Close();
                    if (tempTable.Columns.Count == 0)
                    {
                        MessageBox.Show("No " + cmbTransaction.Text + " data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (cmbCategory.Text == "All Category")
                    {
                        if (cmbTransaction.Text == "All Transactions")
                        {
                            int ttlTransaction = 0;
                            foreach (DataRow dataRow in tempTable.Rows)
                            {
                                ttlTransaction = 0;
                                ttlTransaction = Convert.ToInt32(dataRow[1].ToString());
                                
                                queryString = "SELECT COUNT(id) FROM issued_item where brrId='" + dataRow[0].ToString() + "' and reissuedDate is not null";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                ttlTransaction = ttlTransaction + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                queryString = "SELECT COUNT(id) FROM issued_item where brrId='" + dataRow[0].ToString() + "' and returnDate is not null";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                ttlTransaction = ttlTransaction + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                dataRow[1] = ttlTransaction;
                            }
                        }
                        if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "COUNT(*) desc";
                        }
                        else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "COUNT(*) asc";
                        }
                        tempTable = tempTable.DefaultView.ToTable();
                        int upTo = Convert.ToInt32(numUpto.Value);
                        int rankos = 0;
                        string brrName = "";
                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            if (upTo > 0 && rankos == upTo)
                            {
                                break;
                            }
                            
                            queryString = "SELECT brrName from borrower_details where brrId='" + dataRow[0].ToString() + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
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
                            rankos++;
                            dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), dataRow[1].ToString());
                            Application.DoEvents();
                        }
                        dgvReport.ClearSelection();
                        ttlIssued = tempTable.Rows.Count;
                        
                        queryString = "select count(id) from borrower_details;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        notIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - ttlIssued;
                    }
                    else
                    {
                        queryString = "SELECT brrId FROM borrower_details where brrCategory=@brrCategory";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        List<string> brrIdList = new List<string> { };
                        if (dataReader.HasRows)
                        {
                            brrIdList = (from IDataRecord r in dataReader
                                         select (string)r["brrId"]
                            ).ToList();
                        }
                        dataReader.Close();

                        string combineString = "", filterQuery = "", brrName = "";
                        DataView dataView = new DataView();
                        DataTable filteredDataTable = new DataTable();
                        combineString = string.Join("','", brrIdList);
                        filterQuery = "brrId IN ('" + combineString + "')";
                        dataView = tempTable.DefaultView;
                        dataView.RowFilter = filterQuery;
                        filteredDataTable = dataView.ToTable();

                        if (filteredDataTable.Rows.Count > 0)
                        {
                            if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                            {
                                filteredDataTable.DefaultView.Sort = "COUNT(*) desc";
                            }
                            else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                            {
                                filteredDataTable.DefaultView.Sort = "COUNT(*) asc";
                            }
                            filteredDataTable = filteredDataTable.DefaultView.ToTable();
                            int upTo = Convert.ToInt32(numUpto.Value);
                            int ttlTransaction = 0, rankos = 0;
                            foreach (DataRow dataRow in filteredDataTable.Rows)
                            {
                                if (upTo > 0 && rankos == upTo)
                                {
                                    break;
                                }
                                ttlTransaction = Convert.ToInt32(dataRow[1].ToString());
                                if (cmbTransaction.Text == "All Transactions")
                                {
                                    queryString = "SELECT COUNT(id) FROM issued_item where brrId='" + dataRow[0].ToString() + "' and reissuedDate is not null";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    ttlTransaction = ttlTransaction + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                    queryString = "SELECT COUNT(id) FROM issued_item where brrId='" + dataRow[0].ToString() + "' and returnDate is not null";
                                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                    mysqlCmd.CommandTimeout = 99999;
                                    ttlTransaction = ttlTransaction + Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                                }
                                queryString = "SELECT brrName FROM borrower_details where brrId='" + dataRow[0].ToString() + "'";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        brrName = dataReader[0].ToString();
                                    }
                                }
                                dataReader.Close();
                                rankos++;
                                dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), ttlTransaction);
                                Application.DoEvents();
                            }
                        }
                        dgvReport.ClearSelection();
                        ttlIssued = filteredDataTable.Rows.Count;
                        
                        queryString = "select count(id) from borrower_details where brrCategory=@brrCategory;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        notIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) - ttlIssued;
                    }
                    queryString = "select issueDate from issued_item order by id asc limit 1"; ;
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DataColumn dtColumn;
                    // Create id column  
                    dtColumn = new DataColumn();
                    dtColumn.ColumnName = "brrId";
                    dtColumn.Caption = "brrId";
                    tempTable.Columns.Add(dtColumn);
                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Int32);
                    dtColumn.ColumnName = "tranCount";
                    dtColumn.Caption = "tranCount";
                    tempTable.Columns.Add(dtColumn);

                    List<string> brrIdList = new List<string> { };
                    if (cmbCategory.Text == "All Category")
                    {
                        queryString = "select brrId from borrower_details";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    }
                    else
                    {
                        queryString = "select brrId from borrower_details where brrCategory=@brrCategory";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                    }
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                    }
                    dataReader.Close();

                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    DataRow[] selectedRow;
                    int transCount = 0;
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbTransaction.Text == "All Transactions")
                        {
                            queryString = "SELECT brrId,returnDate,reissuedDate FROM issued_item where issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Issue")
                        {
                            queryString = "SELECT brrId FROM issued_item where issueDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Reissue")
                        {
                            queryString = "SELECT brrId FROM issued_item where reissuedDate='" + filterDate + "'";
                        }
                        else if (cmbTransaction.Text == "Return")
                        {
                            queryString = "SELECT brrId FROM issued_item where returnDate='" + filterDate + "'";
                        }
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (brrIdList.IndexOf(dataReader["brrId"].ToString()) != -1)
                                {
                                    selectedRow = tempTable.Select("brrId='" + dataReader["brrId"].ToString() + "'");
                                    if (selectedRow.Count() > 0)
                                    {
                                        transCount = Convert.ToInt32(selectedRow[0][1].ToString());
                                        transCount++;
                                        selectedRow[0][1] = transCount;
                                    }
                                    else
                                    {
                                        tempTable.Rows.Add(dataReader["brrId"].ToString(), 1);
                                    }
                                }
                            }
                        }
                        dataReader.Close();

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                    }

                    if (tempTable.Rows.Count > 0)
                    {
                        if (lstbReportType.SelectedItem.ToString() == "Most Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "tranCount desc";
                        }
                        else if (lstbReportType.SelectedItem.ToString() == "Least Active Borrower")
                        {
                            tempTable.DefaultView.Sort = "tranCount asc";
                        }
                        tempTable = tempTable.DefaultView.ToTable();
                        int upTo = Convert.ToInt32(numUpto.Value);
                        int rankos = 0;

                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            if (upTo > 0 && rankos == upTo)
                            {
                                break;
                            }
                            queryString = "SELECT brrName FROM borrower_details where brrId='" + dataRow[0].ToString() + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    brrName = dataReader[0].ToString();
                                }
                            }
                            dataReader.Close();
                            rankos++;
                            dgvReport.Rows.Add(rankos, brrName, dataRow[0].ToString(), dataRow[1].ToString());
                            Application.DoEvents();
                        }
                    }
                    dgvReport.ClearSelection();
                    ttlIssued = tempTable.Rows.Count;
                    notIssued = brrIdList.Count - ttlIssued;
                }
                mysqlConn.Close();
            }
        }

        private void MostOrLeastCirculatedItems()
        {
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblInfo3.Visible = false;
            lblInfo4.Visible = false;
            dgvReport.Columns.Clear();
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
            dgvColumn.HeaderText = "Rank";
            dgvColumn.ValueType = typeof(Int32);
            dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.Width = 100;
            dgvReport.Columns.Add(dgvColumn);
            dgvColumn = new DataGridViewTextBoxColumn();
            dgvColumn.HeaderText = "Title of Item";
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.Width = 500;
            dgvReport.Columns.Add(dgvColumn);
            dgvColumn = new DataGridViewTextBoxColumn();
            if (cmbTransaction.Text == "All Transactions")
            {
                dgvColumn.HeaderText = "Total Transactions";
            }
            else if (cmbTransaction.Text == "Issue")
            {
                dgvColumn.HeaderText = "Total Issue";
            }
            else if (cmbTransaction.Text == "Reissue")
            {
                dgvColumn.HeaderText = "Total Reissue";
            }
            else if (cmbTransaction.Text == "Return")
            {
                dgvColumn.HeaderText = "Total Return";
            }
            dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvColumn.Width = 150;
            dgvReport.Columns.Add(dgvColumn);

            if (FieldSettings.Default.itemEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        if (fieldName == "lblTitle")
                        {
                            dgvReport.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
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

                if (cmbCategory.Text == "All Category")
                {
                    sqltCommnd.CommandText = "select distinct itemTitle from itemDetails";
                }
                else
                {
                    if (cmbSubcategory.Text == "All Subcategory")
                    {
                        sqltCommnd.CommandText = "select distinct itemTitle from itemDetails where itemCat=@itemCat";
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                    }
                    else
                    {
                        sqltCommnd.CommandText = "select distinct itemTitle from itemDetails where itemCat=@itemCat and itemSubCat=@itemSubCat";
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                    }
                }

                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                List<string> itemNameList = new List<string> { };
                if (dataReader.HasRows)
                {
                    itemNameList = (from IDataRecord r in dataReader select (string)r["itemTitle"]).ToList();
                }
                dataReader.Close();
                List<string> itemAccnList = new List<string> { };
                DataTable allDataTable = new DataTable();
                sqltCommnd.CommandText = "select * from issuedItem";
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    allDataTable.Load(dataReader);
                    dataReader.Close();

                    string combineString = "", filterQuery = "";
                    DataView dataView = new DataView();
                    DataTable filteredDataTable = new DataTable();
                    DataTable finalDataTable = new DataTable();
                    DataColumn dtColumn;

                    // Create id column  
                    dtColumn = new DataColumn();
                    dtColumn.ColumnName = "Title";
                    dtColumn.Caption = "Title";
                    finalDataTable.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Int32);
                    dtColumn.ColumnName = "Total";
                    dtColumn.Caption = "Total";
                    finalDataTable.Columns.Add(dtColumn);
                    foreach (string itemTitle in itemNameList)
                    {
                        itemAccnList.Clear();
                        sqltCommnd.CommandText = "select itemAccession from itemDetails where itemTitle=@itemTitle";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemTitle", itemTitle);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            itemAccnList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                        }
                        dataReader.Close();
                        combineString = string.Join("','", itemAccnList);
                        filterQuery = "itemAccession IN ('" + combineString + "')";

                        dataView = allDataTable.DefaultView;
                        dataView.RowFilter = filterQuery;
                        filteredDataTable = dataView.ToTable();
                        if (filteredDataTable.Rows.Count > 0)
                        {
                            if (rdbAll.Checked)
                            {
                                ttlIssued = filteredDataTable.Rows.Count;
                                ttlReissue = filteredDataTable.AsEnumerable().Where(x => x["reissuedDate"].ToString() != "").ToList().Count();
                                ttlReturned = filteredDataTable.AsEnumerable().Where(x => x["returnDate"].ToString() != "").ToList().Count();
                            }
                            else
                            {
                                ttlIssued = 0; ttlReissue = 0; ttlReturned = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    ttlIssued = ttlIssued + filteredDataTable.AsEnumerable().Where(x => x["issueDate"].ToString() == filterDate).ToList().Count();
                                    ttlReissue = ttlReissue + filteredDataTable.AsEnumerable().Where(x => x["reissuedDate"].ToString() == filterDate).ToList().Count();
                                    ttlReturned = ttlReturned + filteredDataTable.AsEnumerable().Where(x => x["returnDate"].ToString() == filterDate).ToList().Count();

                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }

                            if (cmbTransaction.Text == "All Transactions")
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlIssued + ttlReissue + ttlReturned);
                            }
                            else if (cmbTransaction.Text == "Issue" && ttlIssued > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlIssued);
                            }
                            else if (cmbTransaction.Text == "Reissue" && ttlReissue > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlReissue);
                            }
                            else if (cmbTransaction.Text == "Return" && ttlReturned > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlReturned);
                            }
                        }
                    }

                    if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items")
                    {
                        finalDataTable.DefaultView.Sort = "Total desc";
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                    {
                        finalDataTable.DefaultView.Sort = "Total asc";
                    }
                    finalDataTable = finalDataTable.DefaultView.ToTable();
                    int upTo = Convert.ToInt32(numUpto.Value);
                    int rankos = 0;
                    foreach (DataRow dataRow in finalDataTable.Rows)
                    {
                        if (upTo > 0 && rankos == upTo)
                        {
                            break;
                        }
                        rankos++;
                        dgvReport.Rows.Add(rankos, dataRow[0].ToString(), dataRow[1].ToString());
                        Application.DoEvents();
                    }
                    dgvReport.ClearSelection();
                    ttlIssued = finalDataTable.Rows.Count;
                    if (cmbCategory.Text == "All Category")
                    {
                        sqltCommnd.CommandText = "select count(distinct itemTitle) from itemDetails";
                    }
                    else
                    {
                        if (cmbSubcategory.Text == "All Subcategory")
                        {
                            sqltCommnd.CommandText = "select count(distinct itemTitle) from itemDetails where itemCat=@itemCat";
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        }
                        else
                        {
                            sqltCommnd.CommandText = "select count(distinct itemTitle) from itemDetails where itemCat=@itemCat and itemSubCat=@itemSubCat";
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                        }
                    }
                    sqltCommnd.CommandType = CommandType.Text;
                    notIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                    if (rdbAll.Checked)
                    {
                        sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();
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
                MySqlCommand mysqlCmd=null;
                string queryString = "";
                MySqlDataReader dataReader = null;

                if (cmbCategory.Text == "All Category")
                {
                    queryString = "select distinct itemTitle from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                }
                else
                {
                    if (cmbSubcategory.Text == "All Subcategory")
                    {
                        queryString = "select distinct itemTitle from item_details where itemCat=@itemCat";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                    }
                    else
                    {
                        queryString = "select distinct itemTitle from item_details where itemCat=@itemCat and itemSubCat=@itemSubCat";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                    }
                }

                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                List<string> itemNameList = new List<string> { };
                if (dataReader.HasRows)
                {
                    itemNameList = (from IDataRecord r in dataReader select (string)r["itemTitle"]).ToList();
                }
                dataReader.Close();
                List<string> itemAccnList = new List<string> { };
                DataTable allDataTable = new DataTable();
                queryString = "select * from issued_item";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    allDataTable.Load(dataReader);
                    dataReader.Close();

                    string combineString = "", filterQuery = "";
                    DataView dataView = new DataView();
                    DataTable filteredDataTable = new DataTable();
                    DataTable finalDataTable = new DataTable();
                    DataColumn dtColumn;

                    // Create id column  
                    dtColumn = new DataColumn();
                    dtColumn.ColumnName = "Title";
                    dtColumn.Caption = "Title";
                    finalDataTable.Columns.Add(dtColumn);

                    dtColumn = new DataColumn();
                    dtColumn.DataType = typeof(Int32);
                    dtColumn.ColumnName = "Total";
                    dtColumn.Caption = "Total";
                    finalDataTable.Columns.Add(dtColumn);
                    foreach (string itemTitle in itemNameList)
                    {
                        itemAccnList.Clear();
                        queryString = "select itemAccession from item_details where itemTitle=@itemTitle";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", itemTitle);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            itemAccnList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                        }
                        dataReader.Close();
                        combineString = string.Join("','", itemAccnList);
                        filterQuery = "itemAccession IN ('" + combineString + "')";

                        dataView = allDataTable.DefaultView;
                        dataView.RowFilter = filterQuery;
                        filteredDataTable = dataView.ToTable();
                        if (filteredDataTable.Rows.Count > 0)
                        {
                            if (rdbAll.Checked)
                            {
                                ttlIssued = filteredDataTable.Rows.Count;
                                ttlReissue = filteredDataTable.AsEnumerable().Where(x => x["reissuedDate"].ToString() != "").ToList().Count();
                                ttlReturned = filteredDataTable.AsEnumerable().Where(x => x["returnDate"].ToString() != "").ToList().Count();
                            }
                            else
                            {
                                ttlIssued = 0; ttlReissue = 0; ttlReturned = 0;
                                DateTime startDate = dtpFrom.Value.Date;
                                DateTime tempdate = dtpFrom.Value.Date;
                                string filterDate = "";
                                while (startDate <= dtpTo.Value.Date)
                                {
                                    filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                                    ttlIssued = ttlIssued + filteredDataTable.AsEnumerable().Where(x => x["issueDate"].ToString() == filterDate).ToList().Count();
                                    ttlReissue = ttlReissue + filteredDataTable.AsEnumerable().Where(x => x["reissuedDate"].ToString() == filterDate).ToList().Count();
                                    ttlReturned = ttlReturned + filteredDataTable.AsEnumerable().Where(x => x["returnDate"].ToString() == filterDate).ToList().Count();

                                    startDate = tempdate.AddDays(1);
                                    tempdate = startDate;
                                }
                            }

                            if (cmbTransaction.Text == "All Transactions")
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlIssued + ttlReissue + ttlReturned);
                            }
                            else if (cmbTransaction.Text == "Issue" && ttlIssued > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlIssued);
                            }
                            else if (cmbTransaction.Text == "Reissue" && ttlReissue > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlReissue);
                            }
                            else if (cmbTransaction.Text == "Return" && ttlReturned > 0)
                            {
                                finalDataTable.Rows.Add(itemTitle, ttlReturned);
                            }
                        }
                    }

                    if (lstbReportType.SelectedItem.ToString() == "Most Circulated Items")
                    {
                        finalDataTable.DefaultView.Sort = "Total desc";
                    }
                    else if (lstbReportType.SelectedItem.ToString() == "Least Circulated Items")
                    {
                        finalDataTable.DefaultView.Sort = "Total asc";
                    }
                    finalDataTable = finalDataTable.DefaultView.ToTable();
                    int upTo = Convert.ToInt32(numUpto.Value);
                    int rankos = 0;
                    foreach (DataRow dataRow in finalDataTable.Rows)
                    {
                        if (upTo > 0 && rankos == upTo)
                        {
                            break;
                        }
                        rankos++;
                        dgvReport.Rows.Add(rankos, dataRow[0].ToString(), dataRow[1].ToString());
                        Application.DoEvents();
                    }
                    dgvReport.ClearSelection();
                    ttlIssued = finalDataTable.Rows.Count;
                    if (cmbCategory.Text == "All Category")
                    {
                        queryString = "select count(distinct itemTitle) from item_details";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    }
                    else
                    {
                        if (cmbSubcategory.Text == "All Subcategory")
                        {
                            queryString = "select count(distinct itemTitle) from item_details where itemCat=@itemCat";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        }
                        else
                        {
                            queryString = "select count(distinct itemTitle) from item_details where itemCat=@itemCat and itemSubCat=@itemSubCat";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                        }
                    }
                    mysqlCmd.CommandTimeout = 99999;
                    notIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                    if (rdbAll.Checked)
                    {
                        queryString = "select issueDate from issued_item order by id asc limit 1"; ;
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {

                                }
                            }
                        }
                        dataReader.Close();
                    }
                }
                mysqlConn.Close();
            }
        }

        private void PaymentHistory()
        {
            paidAmount = 0.0;dueAmount = 0.0;remitedAmount = 0.00;discountAmount = 0.0;
            var tempDate = DateTime.Now.Date;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbCategory.Text == "All Data")
                    {
                        sqltCommnd.CommandText = "Select * from paymentDetails inner join borrowerDetails where borrowerDetails.brrId=paymentDetails.memberId";
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (dataReader["isPaid"].ToString() == "True")
                                {
                                    if (dataReader["isRemited"].ToString() == "True")
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                            dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                            dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                    }
                                }
                                else
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString())
                                        , dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                    dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    else
                    {
                        sqltCommnd.CommandText = "Select * from paymentDetails where paymentDetails.memberId=@memberId collate nocase";
                        sqltCommnd.Parameters.AddWithValue("@memberId", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (dataReader["isPaid"].ToString() == "True")
                                {
                                    if (dataReader["isRemited"].ToString() == "True")
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                            dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                    }
                                }
                                else
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                    dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                }
                                Application.DoEvents();
                            }
                            dgvReport.ClearSelection();
                        }
                        dataReader.Close();
                        sqltCommnd.CommandText = "Select brrName from borrowerDetails where brrId=@brrId";
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                brrName = dataReader["brrName"].ToString();
                            }
                        }
                        dataReader.Close();
                    }
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbCategory.Text == "All Data")
                        {
                            sqltCommnd.CommandText = "Select * from paymentDetails inner join borrowerDetails where borrowerDetails.brrId=paymentDetails.memberId and feesDate='" + filterDate + "'";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if (dataReader["isPaid"].ToString() == "True")
                                    {
                                        if (dataReader["isRemited"].ToString() == "True")
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                                dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                                dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        }
                                        else
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                            discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }
                        else
                        {
                            sqltCommnd.CommandText = "Select * from paymentDetails where memberId=@memberId and feesDate='" + filterDate + "' collate nocase";
                            sqltCommnd.Parameters.AddWithValue("@memberId", txtbValue.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if (dataReader["isPaid"].ToString() == "True")
                                    {
                                        if (dataReader["isRemited"].ToString() == "True")
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                                dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        }
                                        else
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                            discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    Application.DoEvents();
                                }
                                dgvReport.ClearSelection();
                            }
                            dataReader.Close();
                        }

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                    }
                    if (cmbCategory.Text != "All Data")
                    {
                        sqltCommnd.CommandText = "Select brrName from borrowerDetails where brrId=@brrId";
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                brrName = dataReader["brrName"].ToString();
                            }
                        }
                        dataReader.Close();
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
                MySqlCommand mysqlCmd=null;
                string queryString = "";
                MySqlDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbCategory.Text == "All Data")
                    {
                        queryString = "Select * from payment_details inner join borrower_details where borrower_details.brrId=payment_details.memberId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (dataReader["isPaid"].ToString() == "True")
                                {
                                    if (dataReader["isRemited"].ToString() == "True")
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                            dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                            dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                    }
                                }
                                else
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                        dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                    dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    else
                    {
                        queryString = "Select * from payment_details where lower(memberId)=@memberId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbValue.Text.ToLower());
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                if (dataReader["isPaid"].ToString() == "True")
                                {
                                    if (dataReader["isRemited"].ToString() == "True")
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                            dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                    }
                                }
                                else
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                           dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                           dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                    dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                }
                                Application.DoEvents();
                            }
                            dgvReport.ClearSelection();
                        }
                        dataReader.Close();

                        queryString = "Select brrName from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbValue.Text);
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
                    }

                    queryString = "select issueDate from issued_item order by id asc limit 1"; ;
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbCategory.Text == "All Data")
                        {
                            queryString = "Select * from payment_details inner join borrower_details where borrower_details.brrId=payment_details.memberId and feesDate='" + filterDate + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if (dataReader["isPaid"].ToString() == "True")
                                    {
                                        if (dataReader["isRemited"].ToString() == "True")
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                                dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                                dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        }
                                        else
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                            discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                            dataReader["memberId"].ToString(), dataReader["brrName"].ToString(),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }
                        else
                        {
                            queryString = "Select * from payment_details where memberId=@memberId and feesDate='" + filterDate + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", txtbValue.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempDate = DateTime.ParseExact(dataReader["feesDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    if (dataReader["isPaid"].ToString() == "True")
                                    {
                                        if (dataReader["isRemited"].ToString() == "True")
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                                dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "True",
                                                dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            remitedAmount = remitedAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                        }
                                        else
                                        {
                                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Paid", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                            paidAmount = paidAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                            discountAmount = discountAmount + Convert.ToDouble(dataReader["discountAmnt"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["feesDate"].ToString()),
                                               dataReader["dueAmount"].ToString(), dataReader["discountAmnt"].ToString(), "Due", "False",
                                               dataReader["feesDesc"].ToString(), dataReader["collctedBy"].ToString());
                                        dueAmount = dueAmount + Convert.ToDouble(dataReader["dueAmount"].ToString());
                                    }
                                    Application.DoEvents();
                                }
                                dgvReport.ClearSelection();
                            }
                            dataReader.Close();
                        }

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                    }
                    if (cmbCategory.Text != "All Data")
                    {
                        queryString = "Select brrName from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbValue.Text);
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
                    }
                }
                mysqlConn.Close();
            }
            lblInfo1.Visible = true;
            lblInfo2.Visible = true;
            lblInfo3.Visible = true;
            lblInfo4.Visible = true;
            lblInfo1.Text ="Total Paid : "+ paidAmount.ToString();
            lblInfo2.Text = "Total Discount : " + discountAmount.ToString();
            lblInfo3.Text = "Total Remited : " + remitedAmount.ToString();
            lblInfo4.Text = "Total Due : " + dueAmount.ToString();
            dgvReport.ClearSelection();
        }

        private void LibrarianActivities()
        {
            lblInfo1.Visible = false;
            lblInfo2.Visible = false;
            lblInfo3.Visible = false;
            lblInfo4.Visible = false;
            paidAmount = 0.0; dueAmount = 0.0; remitedAmount = 0.00; discountAmount = 0.0;
            var tempDate = DateTime.Now.Date;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbCategory.Text == "All Librarian")
                    {
                        sqltCommnd.CommandText = "Select issueDate,returnDate,reissuedDate,issuedBy,reissuedBy,returnedBy from issuedItem";
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), dataReader["issuedBy"].ToString(), "Item Issue");
                                if (dataReader["reissuedDate"].ToString() != "")
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), dataReader["reissuedBy"].ToString(), "Item Reissue");
                                }
                                else if (dataReader["returnDate"].ToString() != "")
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), dataReader["returnedBy"].ToString(), "Item Return");
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "Select payDate,collctedBy from paymentDetails where isPaid='" + true + "'";
                        sqltCommnd.CommandType = CommandType.Text;
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    else
                    {
                        sqltCommnd.CommandText = "Select * from issuedItem where issuedBy=@issuedBy or reissuedBy=@reissuedBy or returnedBy=@returnedBy collate nocase";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@issuedBy", txtbValue.Text);
                        sqltCommnd.Parameters.AddWithValue("@reissuedBy", txtbValue.Text);
                        sqltCommnd.Parameters.AddWithValue("@returnedBy", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), "Item Issue");
                                }
                                if (dataReader["reissuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), "Item Reissue");
                                }
                                if (dataReader["returnedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), "Item Return");
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "Select payDate,collctedBy from paymentDetails where collctedBy=@collctedBy and isPaid='" + true + "' collate nocase";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@collctedBy", txtbValue.Text);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                    sqltCommnd.CommandType = CommandType.Text;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbCategory.Text == "All Librarian")
                        {
                            sqltCommnd.CommandText = "Select * from issuedItem where issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "'";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["issueDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), dataReader["issuedBy"].ToString(), "Item Issue");
                                    }
                                    if (dataReader["reissuedDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), dataReader["reissuedBy"].ToString(), "Item Reissue");
                                    }
                                    if (dataReader["returnDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), dataReader["returnedBy"].ToString(), "Item Return");
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();

                            sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "Select payDate,collctedBy from paymentDetails where payDate='" + filterDate + "' and isPaid='" + true + "'";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }
                        else
                        {
                            sqltCommnd.CommandText = "Select * from issuedItem where (issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "') collate nocase";
                            sqltCommnd.CommandType = CommandType.Text;
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["issuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), "Item Issue");
                                    }
                                    if (dataReader["reissuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), "Item Reissue");
                                    }
                                    if (dataReader["returnedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), "Item Return");
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();

                            sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = "Select payDate,collctedBy from paymentDetails where collctedBy=@collctedBy and payDate='" + filterDate + "' and isPaid='" + true + "' collate nocase";
                            sqltCommnd.CommandType = CommandType.Text;
                            sqltCommnd.Parameters.AddWithValue("@collctedBy", txtbValue.Text);
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
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
                MySqlCommand mysqlCmd=null;
                string queryString = "";
                MySqlDataReader dataReader = null;
                if (rdbAll.Checked)
                {
                    if (cmbCategory.Text == "All Librarian")
                    {
                        queryString = "Select issueDate,returnDate,reissuedDate,issuedBy,reissuedBy,returnedBy from issued_item";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), dataReader["issuedBy"].ToString(), "Item Issue");
                                if (dataReader["reissuedDate"].ToString() != "")
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), dataReader["reissuedBy"].ToString(), "Item Reissue");
                                }
                                else if (dataReader["returnDate"].ToString() != "")
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), dataReader["returnedBy"].ToString(), "Item Return");
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                       
                        queryString = "Select payDate,collctedBy from payment_details where isPaid='" + true + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    else
                    {
                        queryString = "Select * from issued_item where lower(issuedBy)=@issuedBy or lower(reissuedBy)=@reissuedBy or lower(returnedBy)=@returnedBy";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@issuedBy", txtbValue.Text.ToLower());
                        mysqlCmd.Parameters.AddWithValue("@reissuedBy", txtbValue.Text.ToLower());
                        mysqlCmd.Parameters.AddWithValue("@returnedBy", txtbValue.Text.ToLower());
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["issuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), "Item Issue");
                                }
                                if (dataReader["reissuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), "Item Reissue");
                                }
                                if (dataReader["returnedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), "Item Return");
                                }
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        
                        queryString = "Select payDate,collctedBy from payment_details where lower(collctedBy)=@collctedBy and isPaid='" + true + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@collctedBy", txtbValue.Text.ToLower()) ;
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        dgvReport.ClearSelection();
                    }
                    queryString = "select issueDate from issued_item order by id asc limit 1"; ;
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {

                            }
                        }
                    }
                    dataReader.Close();
                }
                else
                {
                    DateTime startDate = dtpFrom.Value.Date;
                    DateTime tempdate = dtpFrom.Value.Date;
                    string filterDate = "";
                    while (startDate <= dtpTo.Value.Date)
                    {
                        filterDate = startDate.Day.ToString("00") + "/" + startDate.Month.ToString("00") + "/" + startDate.Year.ToString("0000");
                        if (cmbCategory.Text == "All Librarian")
                        {
                            queryString = "Select * from issued_item where issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["issueDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), dataReader["issuedBy"].ToString(), "Item Issue");
                                    }
                                    if (dataReader["reissuedDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), dataReader["reissuedBy"].ToString(), "Item Reissue");
                                    }
                                    if (dataReader["returnDate"].ToString() == filterDate)
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), dataReader["returnedBy"].ToString(), "Item Return");
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();

                            queryString = "Select payDate,collctedBy from payment_details where payDate='" + filterDate + "' and isPaid='" + true + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }
                        else
                        {
                            queryString = "Select * from issued_item where (issueDate='" + filterDate + "' or reissuedDate='" + filterDate + "' or returnDate='" + filterDate + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["issuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()), "Item Issue");
                                    }
                                    if (dataReader["reissuedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["reissuedDate"].ToString()), "Item Reissue");
                                    }
                                    if (dataReader["returnedBy"].ToString().ToLower() == txtbValue.Text.ToLower())
                                    {
                                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["returnDate"].ToString()), "Item Return");
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();

                            queryString = "Select payDate,collctedBy from payment_details where lower(collctedBy)=@collctedBy and payDate='" + filterDate + "' and isPaid='" + true + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@collctedBy", txtbValue.Text.ToLower());
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["payDate"].ToString()), dataReader["collctedBy"].ToString(), "Payment Collection");
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            dgvReport.ClearSelection();
                        }

                        startDate = tempdate.AddDays(1);
                        tempdate = startDate;
                    }
                }
                mysqlConn.Close();
            }
            dgvReport.ClearSelection();
        }

        private void OverDueItems()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails inner join borrowerDetails " +
                        "where issuedItem.itemAccession=itemDetails.itemAccession and issuedItem.brrId=borrowerDetails.brrId and issuedItem.itemReturned='" + false + "'";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                DateTime currentDate = DateTime.Now.Date, expDate = DateTime.Now.Date;
                if (dataReader.HasRows)
                {
                    int daysLate = 0;
                    while (dataReader.Read())
                    {
                        expDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        daysLate = Convert.ToInt32((currentDate - expDate).TotalDays);
                        if (daysLate > 0)
                        {
                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                       dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()),
                                       daysLate.ToString());
                            Application.DoEvents();
                        }
                    }
                }
                dataReader.Close();

                sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from issuedItem where itemReturned = '" + false + "';";
                sqltCommnd.CommandType = CommandType.Text;
                lblInfo1.Text = "Total Issued : " + sqltCommnd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Overdue : " + dgvReport.Rows.Count.ToString();
                ttlIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                notIssued = dgvReport.Rows.Count;
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
                string queryString = "select * from issued_item inner join item_details inner join borrower_details " +
                        "where issued_item.itemAccession=item_details.itemAccession and issued_item.brrId=borrower_details.brrId and issued_item.itemReturned='" + false + "'";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                DateTime currentDate = DateTime.Now.Date, expDate = DateTime.Now.Date;
                if (dataReader.HasRows)
                {
                    int daysLate = 0;
                    while (dataReader.Read())
                    {
                        expDate = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        daysLate = Convert.ToInt32((currentDate - expDate).TotalDays);
                        if (daysLate > 0)
                        {
                            dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                       dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()),
                                       daysLate.ToString());
                            Application.DoEvents();
                        }
                    }
                }
                dataReader.Close();

                queryString = "select issueDate from issued_item order by id asc limit 1"; ;
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                queryString = "select count(id) from issued_item where itemReturned = '" + false + "';";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                lblInfo1.Text = "Total Issued : " + mysqlCmd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Overdue : " + dgvReport.Rows.Count.ToString();
                ttlIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                notIssued = dgvReport.Rows.Count;
                mysqlConn.Close();
            }
            dgvReport.ClearSelection();
        }

        private void IssuedItems()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from issuedItem inner join itemDetails inner join borrowerDetails " +
                       "where issuedItem.itemAccession=itemDetails.itemAccession and issuedItem.brrId=borrowerDetails.brrId and issuedItem.itemReturned='" + false + "'";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                            dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                      dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()),
                                      FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()));
                        Application.DoEvents();
                    }
                }
                dataReader.Close();

                sqltCommnd.CommandText = "select [issueDate] from issuedItem order by [id] asc limit 1"; ;
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from itemDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                lblInfo1.Text = "Total Items : " + sqltCommnd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Issued : " + dgvReport.Rows.Count.ToString();
                notIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                ttlIssued = dgvReport.Rows.Count;
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
                string queryString = "select * from issued_item inner join item_details inner join borrower_details " +
                       "where issued_item.itemAccession=item_details.itemAccession and issued_item.brrId=borrower_details.brrId and issued_item.itemReturned='" + false + "'";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, FormatDate.getUserFormat(dataReader["issueDate"].ToString()),
                            dataReader["brrName"].ToString() + "(" + dataReader["brrId"].ToString() + ")",
                                      dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()),
                                      FormatDate.getUserFormat(dataReader["expectedReturnDate"].ToString()));
                        Application.DoEvents();
                    }
                }
                dataReader.Close();

                queryString= "select issueDate from issued_item order by id asc limit 1"; ;
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                queryString = "select count(id) from item_details;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                lblInfo1.Text = "Total Items : " + mysqlCmd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Issued : " + dgvReport.Rows.Count.ToString();
                notIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                ttlIssued = dgvReport.Rows.Count;
                mysqlConn.Close();
            }
            dgvReport.ClearSelection();
        }

        private void LostDamageReport()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from lostDamage inner join itemDetails where itemDetails.itemAccession=lostDamage.itemAccn";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    var tempDate = DateTime.Now.Date;
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["itemAccn"].ToString(),
                            dataReader["itemTitle"].ToString(), dataReader["itemStatus"].ToString(), dataReader["statusComment"].ToString());
                        Application.DoEvents();
                    }
                }
                dataReader.Close();

                sqltCommnd.CommandText = "select entryDate from lostDamage order by [id] asc limit 1"; ;
                sqltCommnd.CommandType = CommandType.Text;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(id) from itemDetails;";
                sqltCommnd.CommandType = CommandType.Text;
                lblInfo1.Text = "Total Items : " + sqltCommnd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Lost/Damage : " + dgvReport.Rows.Count.ToString();
                notIssued = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                ttlIssued = dgvReport.Rows.Count;
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
                string queryString = "select * from lost_damage inner join item_details where item_details.itemAccession=lost_damage.itemAccn";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    var tempDate = DateTime.Now.Date;
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["itemAccn"].ToString(),
                            dataReader["itemTitle"].ToString(), dataReader["itemStatus"].ToString(), dataReader["statusComment"].ToString());
                        Application.DoEvents();
                    }
                }
                dataReader.Close();

                queryString = "select entryDate from lost_damage order by id asc limit 1"; ;
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            dtpFrom.Value = DateTime.ParseExact(dataReader["issueDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {

                        }
                    }
                }
                dataReader.Close();
                lblInfo1.Visible = true;
                lblInfo2.Visible = true;
                lblInfo3.Visible = false;
                lblInfo4.Visible = false;

                queryString = "select count(id) from item_details;";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                lblInfo1.Text = "Total Items : " + mysqlCmd.ExecuteScalar().ToString();
                lblInfo2.Text = "Total Lost/Damage : " + dgvReport.Rows.Count.ToString();
                notIssued = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                ttlIssued = dgvReport.Rows.Count;
                mysqlConn.Close();
            }
            dgvReport.ClearSelection();
        }

        private void ItemWiseData()
        {
            List<string> itemAuthor = new List<string> { };
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select count(itemTitle) as ttlCount,itemTitle,itemAuthor from itemDetails group by itemTitle,itemAuthor";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader1 = null;
                string queryString = "";
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    SQLiteCommand sqltCommnd1 = sqltConn.CreateCommand();
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["itemTitle"].ToString(),"","","","", dataReader["ttlCount"].ToString());
                        //itemAuthor.Add(dataReader["itemAuthor"].ToString());
                        queryString = "select count(itemTitle) as ttlCount from itemDetails where itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemAvailable='" + false + "'";
                        sqltCommnd1.CommandText = queryString;
                        sqltCommnd1.CommandType = CommandType.Text;
                        sqltCommnd1.Parameters.AddWithValue("@itemTitle", dataReader["itemTitle"].ToString());
                        sqltCommnd1.Parameters.AddWithValue("@itemAuthor", dataReader["itemAuthor"].ToString());
                        dataReader1 = sqltCommnd1.ExecuteReader();
                        if (dataReader1.HasRows)
                        {
                            while (dataReader1.Read())
                            {
                                dgvReport.Rows[dgvReport.Rows.Count-1].Cells[2].Value = dataReader1["ttlCount"].ToString();
                            }
                        }
                        dataReader1.Close();

                        queryString = "select count(itemTitle) as ttlCount from itemDetails where itemTitle=@itemTitle and itemAuthor=@itemAuthor and isLost='" + true + "'";
                        sqltCommnd1.CommandText = queryString;
                        sqltCommnd1.CommandType = CommandType.Text;
                        sqltCommnd1.Parameters.AddWithValue("@itemTitle", dataReader["itemTitle"].ToString());
                        sqltCommnd1.Parameters.AddWithValue("@itemAuthor", dataReader["itemAuthor"].ToString());
                        dataReader1 = sqltCommnd1.ExecuteReader();
                        if (dataReader1.HasRows)
                        {
                            while (dataReader1.Read())
                            {
                                dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[3].Value = dataReader1["ttlCount"].ToString();
                            }
                        }
                        dataReader1.Close();

                        queryString = "select count(itemTitle) as ttlCount from itemDetails where itemTitle=@itemTitle and itemAuthor=@itemAuthor and isDamage='" + true + "'";
                        sqltCommnd1.CommandText = queryString;
                        sqltCommnd1.CommandType = CommandType.Text;
                        sqltCommnd1.Parameters.AddWithValue("@itemTitle", dataReader["itemTitle"].ToString());
                        sqltCommnd1.Parameters.AddWithValue("@itemAuthor", dataReader["itemAuthor"].ToString());
                        dataReader1 = sqltCommnd1.ExecuteReader();
                        if (dataReader1.HasRows)
                        {
                            while (dataReader1.Read())
                            {
                                dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[4].Value = dataReader1["ttlCount"].ToString();
                            }
                        }
                        dataReader1.Close();

                        queryString = "select count(itemTitle) as ttlCount from itemDetails where itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemAvailable='" + true + "'";
                        sqltCommnd1.CommandText = queryString;
                        sqltCommnd1.CommandType = CommandType.Text;
                        sqltCommnd1.Parameters.AddWithValue("@itemTitle", dataReader["itemTitle"].ToString());
                        sqltCommnd1.Parameters.AddWithValue("@itemAuthor", dataReader["itemAuthor"].ToString());
                        dataReader1 = sqltCommnd1.ExecuteReader();
                        if (dataReader1.HasRows)
                        {
                            while (dataReader1.Read())
                            {
                                dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[5].Value =(Convert.ToInt32(dataReader1["ttlCount"].ToString())- Convert.ToInt32(dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[3].Value.ToString()));
                            }
                        }
                        dataReader1.Close();
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
                string queryString = "select count(itemTitle) as ttlCount,itemTitle,itemAuthor from item_details group by itemTitle,itemAuthor";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvReport.Rows.Add(dgvReport.Rows.Count + 1, dataReader["itemTitle"].ToString(),"","","","", dataReader["ttlCount"].ToString());
                        itemAuthor.Add(dataReader["itemAuthor"].ToString());
                        Application.DoEvents();
                    }
                }
                dataReader.Close();
                foreach (DataGridViewRow dataRow in dgvReport.Rows)
                {
                    queryString = "select count(itemTitle) as ttlCount from item_details where itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemAvailable='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor[dataRow.Index]);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dataRow.Cells[2].Value = dataReader["ttlCount"].ToString();
                        }
                    }
                    dataReader.Close();

                    queryString = "select count(itemTitle) as ttlCount from item_details where itemTitle=@itemTitle and itemAuthor=@itemAuthor and isLost='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor[dataRow.Index]);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dataRow.Cells[3].Value = dataReader["ttlCount"].ToString();
                        }
                    }
                    dataReader.Close();

                    queryString = "select count(itemTitle) as ttlCount from item_details where itemTitle=@itemTitle and itemAuthor=@itemAuthor and isDamage='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor[dataRow.Index]);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dataRow.Cells[4].Value = dataReader["ttlCount"].ToString();
                        }
                    }
                    dataReader.Close();

                    queryString = "select count(itemTitle) as ttlCount from item_details where itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemAvailable='" + true + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", itemAuthor[dataRow.Index]);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dataRow.Cells[5].Value = (Convert.ToInt32(dataReader["ttlCount"].ToString()) - Convert.ToInt32(dataRow.Cells[3].Value.ToString()));
                        }
                    }
                    dataReader.Close();
                    Application.DoEvents();
                }
                mysqlConn.Close();
            }
        }

        private void SubjectWiseData()
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select distinct itemSubject from itemDetails";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                List<string> subjectList = new List<string> { };
                if (dataReader.HasRows)
                {
                    subjectList = (from IDataRecord r in dataReader
                                   select (string)r["itemSubject"]
                                           ).ToList();
                }
                dataReader.Close();

                int typeofTitle = 0;
                int totalTypeofTitle = 0;
                int totalItems = 0;
                int totalTitle = 0;
                foreach (string subjectName in subjectList)
                {
                    typeofTitle = 0;
                    totalTitle = 0;
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count(distinct itemTitle) from itemDetails where itemSubject=@itemSubject";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemSubject", subjectName);
                    typeofTitle = Convert.ToInt32(sqltCommnd.ExecuteScalar());
                    totalTypeofTitle = totalTypeofTitle + typeofTitle;

                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select count (itemTitle) from itemDetails where itemSubject=@itemSubject";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemSubject", subjectName);
                    totalTitle = Convert.ToInt32(sqltCommnd.ExecuteScalar());
                    totalItems = totalItems + totalTitle;
                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, subjectName, typeofTitle, totalTitle);
                    Application.DoEvents();
                }
                sqltConn.Close();
                lblInfo1.Text = "Total type of title : " + totalTypeofTitle.ToString();
                lblInfo2.Text = "Total items : " + totalItems.ToString();
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
                string queryString = "select distinct itemSubject from item_details";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                List<string> subjectList = new List<string> { };
                if (dataReader.HasRows)
                {
                    subjectList = (from IDataRecord r in dataReader
                                   select (string)r["itemSubject"]
                                           ).ToList();
                }
                dataReader.Close();

                int typeofTitle = 0;
                int totalTypeofTitle = 0;
                int totalItems = 0;
                int totalTitle = 0;
                foreach (string subjectName in subjectList)
                {
                    queryString = "select count(distinct itemTitle) from item_details where itemSubject=@itemSubject";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemSubject", subjectName);
                    mysqlCmd.CommandTimeout = 99999;
                    typeofTitle = Convert.ToInt32(mysqlCmd.ExecuteScalar());
                    totalTypeofTitle = totalTypeofTitle + typeofTitle;

                    queryString = "select count(itemTitle) from item_details where itemSubject=@itemSubject";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemSubject", subjectName);
                    mysqlCmd.CommandTimeout = 99999;
                    totalTitle = Convert.ToInt32(mysqlCmd.ExecuteScalar());
                    totalItems = totalItems + totalTitle;
                    dgvReport.Rows.Add(dgvReport.Rows.Count + 1, subjectName, typeofTitle, totalTitle);
                    Application.DoEvents();
                }
                
                mysqlConn.Close();
                lblInfo1.Text = "Total type of title : " + totalTypeofTitle.ToString();
                lblInfo2.Text = "Total items : " + totalItems.ToString();
            }
            dgvReport.ClearSelection();
            lblInfo1.Visible = true;
            lblInfo2.Visible = true;
        }

        private void dgvReport_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ttlRecord.Text = dgvReport.Rows.Count.ToString();
        }

        private void dgvReport_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            ttlRecord.Text = dgvReport.Rows.Count.ToString();
        }

        private void btnCsv_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnCsv.Enabled == true)
            {
                btnCsv.BackColor = Color.FromArgb(45,58,66);
            }
            else
            {
                btnCsv.BackColor = Color.DimGray;
            }
        }

        private void btnPdf_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnPdf.Enabled == true)
            {
                btnPdf.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnPdf.BackColor = Color.DimGray;
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

        private void pcbFullView_Click(object sender, EventArgs e)
        {
            pnlView.Location = new Point(5, 0);
            pnlView.Height = 506;
            pcbFullView.Visible = false;
        }

        private void pcbNormalView_Click(object sender, EventArgs e)
        {
            pnlView.Height = 321;
            pnlView.Location = new Point(5, 185);
            pcbFullView.Visible = true;
        }

        private void cmbSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkbFilter.Checked = false;
            chkbFilter.Enabled = false;
            if (lstbReportType.SelectedItem.ToString() == "Items Data")// || lstbReportType.SelectedItem.ToString()=="Item Circulation History"
            {
                if (cmbCategory.Text != "All Category")
                {
                    ItemDataColumns();
                    if (cmbSubcategory.Text == "All Subcategory")
                    {
                        chkbFilter.Checked = true;
                        chkbFilter.Enabled = true;
                    }
                    else
                    {
                        chkbFilter.Checked = false;
                        chkbFilter.Enabled = false;
                    }
                }
            }
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            if (globalVarLms.licenseType == "Demo")
            {
                MessageBox.Show("This feature is not available in the trial version", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (globalVarLms.currentDate > globalVarLms.expiryDate)
            {
                MessageBox.Show("Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No datafound !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Csv Files (*.csv)|*.csv";
            saveDialog.DefaultExt = "csv";
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                // Choose whether to write header. Use EnableWithoutHeaderText instead to omit header.
                dgvReport.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                // Select all the cells
                dgvReport.SelectAll();
                // Copy selected cells to DataObject
                DataObject dataObject = dgvReport.GetClipboardContent();
                // Get the text of the DataObject, and serialize it to a file
                File.WriteAllText(fileName, dataObject.GetText(TextDataFormat.CommaSeparatedValue),Encoding.UTF8);
                dgvReport.ClearSelection();
                MessageBox.Show("Report save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pcbPanelClose_MouseEnter(object sender, EventArgs e)
        {
            pcbPanelClose.BackColor = Color.Red;
        }

        private void pcbPanelClose_MouseLeave(object sender, EventArgs e)
        {
            pcbPanelClose.BackColor = Color.DimGray;
        }

        private void pcbPanelClose_Click(object sender, EventArgs e)
        {
            btnSave_Click(null, null);
        }
    }
}
