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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormRenewMember : Form
    {
        public ToolTip tooTip = new ToolTip();
        public FormRenewMember()
        {
            InitializeComponent();
            tooTip.OwnerDraw = true;
            tooTip.Draw += new DrawToolTipEventHandler(tooTip_Draw);
        }

        DateTime currentDate = DateTime.Now.Date;
        AutoCompleteStringCollection autoCollBorrower = new AutoCompleteStringCollection();

        private void FormRenew_Load(object sender, EventArgs e)
        {
            txtbFees.Text =  0.00.ToString("0.00");
            txtbDuration.Text = 0.ToString();
            lblCurrency.Text = globalVarLms.currSymbol;
            txtbBrrId.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbBrrId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbBrrId.AutoCompleteCustomSource = autoCollBorrower;
            cmbPaymentMode.SelectedIndex = 0;
            cmbPlan.Items.Add("--Select--");
            dtpIssue.CustomFormat = Properties.Settings.Default.dateFormat;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select brrId,mbershipDuration,entryDate,renewDate from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                    while (dataReader.Read())
                    {
                        if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                        {
                            entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        try
                        {
                            renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                            if (renewDate.Date <= DateTime.Now.Date)
                            {
                                autoCollBorrower.Add(dataReader["brrId"].ToString());
                            }
                        }
                        catch
                        {

                        }
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
                    string queryString = "select brrId,mbershipDuration,entryDate,renewDate from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            if (dataReader["renewDate"].ToString() != null && dataReader["renewDate"].ToString() != "")
                            {
                                entrydate = DateTime.ParseExact(dataReader["renewDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                entrydate = DateTime.ParseExact(dataReader["entryDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                            try
                            {
                                renewDate = entrydate.AddDays(Convert.ToInt32(dataReader["mbershipDuration"].ToString()));
                                if (renewDate.Date <= DateTime.Now.Date)
                                {
                                    autoCollBorrower.Add(dataReader["brrId"].ToString());
                                }
                            }
                            catch
                            {

                            }
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
                        if (fieldName == "lblBrId")
                        {
                            lblBrId.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrName")
                        {
                            lblBrName.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            lblBrCategory.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void FormRenew_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                          this.DisplayRectangle);
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {
            if (txtbBrrId.Text != "")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select brrName,mbershipDuration,brrCategory,brrImage,memPlan,memFreq from borrowerDetails where brrId=@brrId";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        tooTip.Hide(txtbBrrId);
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbCategory.Text = dataReader["brrCategory"].ToString();
                            cmbPlan.Text = dataReader["memPlan"].ToString();
                            txtbFreqncy.Text = dataReader["memFreq"].ToString();
                            txtbDuration.Text = dataReader["mbershipDuration"].ToString();
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
                        dataReader.Close();
                    }
                    else
                    {
                        tooTip.AutomaticDelay = 1000;
                        tooTip.Show("Borrower doesn't exist.", txtbBrrId);
                        txtbBrrName.Clear();
                        txtbCategory.Clear();
                        txtbDuration.Text = 0.ToString();
                        txtbFees.Text = 0.00.ToString("0.00");
                        pcbBrrImage.Image = Properties.Resources.blankBrrImage;
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
                        string queryString = "select brrName,mbershipDuration,brrCategory,brrImage,memPlan,memFreq from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            tooTip.Hide(txtbBrrId);
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                txtbBrrName.Text = dataReader["brrName"].ToString();
                                txtbCategory.Text = dataReader["brrCategory"].ToString();
                                cmbPlan.Text = dataReader["memPlan"].ToString();
                                txtbFreqncy.Text = dataReader["memFreq"].ToString();
                                txtbDuration.Text = dataReader["mbershipDuration"].ToString();
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
                            dataReader.Close();
                        }
                        else
                        {
                            tooTip.AutomaticDelay = 1000;
                            tooTip.Show("Borrower doesn't exist.", txtbBrrId);
                            txtbBrrName.Clear();
                            txtbCategory.Clear();
                            txtbDuration.Text = 0.ToString();
                            txtbFees.Text = 0.00.ToString("0.00");
                            pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                tooTip.Hide(txtbBrrId);
                txtbBrrName.Clear();
                txtbCategory.Clear();
                txtbDuration.Text = 0.ToString();
                txtbFees.Text =0.00.ToString("0.00");
                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
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

        private void txtbDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbDuration_Leave(object sender, EventArgs e)
        {
            if(txtbDuration.Text=="")
            {
                txtbDuration.Text = 0.ToString();
            }
        }

        private void txtbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
      (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbFees_Enter(object sender, EventArgs e)
        {
            txtbFees.Clear();
        }

        private void txtbDuration_Enter(object sender, EventArgs e)
        {
            txtbDuration.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtbBrrId.Text=="")
            {
                MessageBox.Show("Please enter the member id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrId.Select();
                return;
            }
            if (Convert.ToDouble(txtbFees.Text) > 0 && cmbPaymentMode.SelectedIndex == 0)
            {
                MessageBox.Show("Please select payment mode.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbPlan.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a membership plan.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbPlan.Select();
                return;
            }
            string renewDate = dtpIssue.Value.Day.ToString("00") + "/" + dtpIssue.Value.Month.ToString("00") + "/" + dtpIssue.Value.Year.ToString("0000");
            string renewlFees = txtbFees.Text.Replace(globalVarLms.currSymbol, "");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "update borrowerDetails set mbershipDuration='" + txtbDuration.Text + "',renewDate='" + renewDate + "',memPlan=:memPlan,memFreq='" + txtbFreqncy.Text + "' where brrId=@brrId";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                sqltCommnd.Parameters.AddWithValue("memPlan", cmbPlan.Text);
                sqltCommnd.ExecuteNonQuery();
                if (Convert.ToDouble(renewlFees) > 0)
                {
                    queryString = "select invidCount from paymentDetails where invidCount!=0 order by id desc limit 1";
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    int rowCount = 0;
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                        }
                    }
                    dataReader.Close();
                    rowCount++;
                    string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                    queryString = "Insert Into paymentDetails (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt,paymentMode,paymentRef)" +
                        "values ('" + renewDate + "','" + invId + "','" + txtbBrrId.Text + "','" + "Renewal fees" + "','" + renewlFees + "','" + true + "'," +
                        "'" + renewDate + "','" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + 0 + "','" + cmbPaymentMode.Text + "','" + txtbReference.Text + "')";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                    if (globalVarLms.paymentReciept)
                    {
                        generateMembershipReciept(invId, renewDate, renewlFees);
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
                MySqlCommand mysqlCmd;
                string queryString = queryString = "update borrower_details set mbershipDuration=" +
                    "'" + txtbDuration.Text + "',renewDate='" + renewDate + "',memPlan=@memPlan," +
                    "memFreq='" + txtbFreqncy.Text + "' where brrId=@brrId";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                mysqlCmd.Parameters.AddWithValue("@memPlan", cmbPlan.Text);
                mysqlCmd.CommandTimeout = 99999;
                mysqlCmd.ExecuteNonQuery();
                if (Convert.ToDouble(renewlFees) > 0)
                {
                    queryString = "select invidCount from payment_details where invidCount!=0 order by id desc limit 1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    int rowCount = 0;
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                        }
                    }
                    dataReader.Close();
                    rowCount++;
                    string invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                    queryString = "Insert Into payment_details (feesDate,invId,memberId,feesDesc,dueAmount,isPaid,payDate,collctedBy,invidCount,discountAmnt,paymentMode,paymentRef)" +
                        "values ('" + renewDate + "','" + invId + "','" + txtbBrrId.Text + "','" + "Renewal fees" + "','" + renewlFees + "','" + true + "'," +
                        "'" + renewDate + "','" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + 0 + "','" + cmbPaymentMode.Text + "','" + txtbReference.Text + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                    if (globalVarLms.paymentReciept)
                    {
                        generateMembershipReciept(invId, renewDate, renewlFees);
                    }
                }
            }
            globalVarLms.backupRequired = true;
            MessageBox.Show("Renew successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            autoCollBorrower.Remove(txtbBrrId.Text);
            txtbBrrId.Clear();
            txtbBrrName.Clear();
            txtbCategory.Clear();
            txtbDuration.Text = 0.ToString();
            txtbFees.Text = 0.00.ToString("0.00");
           
        }

        private void generateMembershipReciept(string invId, string currentDate,string renewalFees)
        {
            string brrContact = "";
            System.Drawing.Image instLogo = null;
            string instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "";
            if (Properties.Settings.Default.sqliteDatabase)
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

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase("Membership Renewal Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbDuration.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(renewalFees.ToString(), smallFont));
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

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase("Membership Renewal Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbDuration.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(renewalFees.ToString(), smallFont));
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

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
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

                    pdfCell = new PdfPCell(new Phrase("Membership Renewal Fees", smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbDuration.Text, smallFont));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(renewalFees.ToString(), smallFont));
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

        int validDays = 0;
        double memFees = 0.0;
        private void cmbPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlan.SelectedIndex != 0)
            {
                validDays = 0;
                memFees = 0.0;
                if (Properties.Settings.Default.sqliteDatabase)
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
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                txtbDuration.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                txtbFees.Text = (memFees * Convert.ToInt32(txtbFreqncy.Text)).ToString();
            }
        }

        private void txtbFreqncy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbFreqncy_TextChanged(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text != "")
            {
                txtbDuration.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                txtbFees.Text = (memFees * Convert.ToInt32(txtbFreqncy.Text)).ToString();
            }
        }

        private void txtbFreqncy_Leave(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text == "")
            {
                txtbFreqncy.Text = 1.ToString();
            }
        }

        private void lnkLvlRctSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}
