using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormGeneralSetting : Form
    {
        public FormGeneralSetting()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormGeneralSetting_Load(object sender, EventArgs e)
        {
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
                        txtbFullName.Text = dataReader["instName"].ToString();
                        txtbShortName.Text = dataReader["instShortName"].ToString();
                        txtbLibraryName.Text = dataReader["libraryName"].ToString();
                        txtbAddress.Text = dataReader["instAddress"].ToString();
                        txtbContact.Text = dataReader["instContact"].ToString();
                        txtbMailId.Text = dataReader["instMail"].ToString();
                        TxtbWebsite.Text = dataReader["instWebsite"].ToString();
                        txtbCode.Text = dataReader["dialCode"].ToString();
                        cmbCurrName.Items.Add(dataReader["currencyName"].ToString());
                        cmbCurrName.SelectedIndex = 0;
                        cmbCurrShort.Items.Add(dataReader["currShortName"].ToString());
                        cmbCurrShort.SelectedIndex = 0;
                        cmbCurrSymbol.Items.Add(dataReader["currSymbol"].ToString());
                        cmbCurrSymbol.SelectedIndex = 0;
                        txtbCountry.Text = dataReader["countryName"].ToString();
                        txtbPrinter.Text = dataReader["printerName"].ToString();
                        txtbMngmnt.Text = dataReader["mngmntMail"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            pcbLogo.Image = System.Drawing.Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            pcbLogo.Image = Properties.Resources.your_logo;
                        }
                    }
                }
                else
                {
                    btnUpdate.Text = "Insert";
                }
                dataReader.Close();

                queryString = "select [countryName] from countryDetails";
                sqltCommnd.CommandText = queryString;
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollData.Clear();
                    List<string> idList = (from IDataRecord r in dataReader
                                           select (string)r["countryName"]
                        ).ToList();
                    autoCollData.AddRange(idList.ToArray());
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
                    string queryString = "select * from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbFullName.Text = dataReader["instName"].ToString();
                            txtbShortName.Text = dataReader["instShortName"].ToString();
                            txtbLibraryName.Text = dataReader["libraryName"].ToString();
                            txtbAddress.Text = dataReader["instAddress"].ToString();
                            txtbContact.Text = dataReader["instContact"].ToString();
                            txtbMailId.Text = dataReader["instMail"].ToString();
                            TxtbWebsite.Text = dataReader["instWebsite"].ToString();
                            txtbCode.Text = dataReader["dialCode"].ToString();
                            cmbCurrName.Items.Add(dataReader["currencyName"].ToString());
                            cmbCurrName.SelectedIndex = 0;
                            cmbCurrShort.Items.Add(dataReader["currShortName"].ToString());
                            cmbCurrShort.SelectedIndex = 0;
                            cmbCurrSymbol.Items.Add(dataReader["currSymbol"].ToString());
                            cmbCurrSymbol.SelectedIndex = 0;
                            txtbCountry.Text = dataReader["countryName"].ToString();
                            txtbPrinter.Text = dataReader["printerName"].ToString();
                            txtbMngmnt.Text = dataReader["mngmntMail"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbLogo.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbLogo.Image = Properties.Resources.your_logo;
                            }
                        }
                    }
                    else
                    {
                        btnUpdate.Text = "Insert";
                    }
                    dataReader.Close();

                    queryString = "select countryName from country_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> idList = (from IDataRecord r in dataReader
                                               select (string)r["countryName"]
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
            if(txtbLibraryName.Text=="")
            {
                txtbLibraryName.Text = txtbFullName.Text;
            }
            
            if (txtbCode.Text == "")
            {
                txtbCode.ForeColor = Color.Gray;
                txtbCode.Text = "EX- +1 for USA";
            }

            txtbCountry.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbCountry.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbCountry.AutoCompleteCustomSource = autoCollData;
        }

        private void FormGeneralSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                          this.DisplayRectangle);
        }

        private void txtbCode_Enter(object sender, EventArgs e)
        {
            if(txtbCode.Text== "EX- +1 for USA".ToUpper())
            {
                txtbCode.Clear();
                txtbCode.ForeColor = Color.Black;
            }
        }

        private void txtbCode_Leave(object sender, EventArgs e)
        {
            if (txtbCode.Text == "")
            {
                txtbCode.ForeColor = Color.Gray;
                txtbCode.Text = "EX- +1 for USA";
            }
        }

        private void txtbContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.NoImageAvailable;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbLogo.Image = globalVarLms.bimapImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(txtbPrinter.Text=="")
            {
                MessageBox.Show("Please select your default printer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtbFullName.Text == "")
            {
                MessageBox.Show("Please enter your organisation name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbFullName.Select();
                return;
            }
            if (txtbShortName.Text == "")
            {
                MessageBox.Show("Please enter your organisation short name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbShortName.Select();
                return;
            }
            String base64String = "base64String";
            if (pcbLogo.Image != Properties.Resources.blankBrrImage || pcbLogo.Image != Properties.Resources.uploadingFail ||
                pcbLogo.Image != Properties.Resources.your_logo)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pcbLogo.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = memoryStream.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
                
            if (btnUpdate.Text == "Update")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "update generalSettings set instName=?,instShortName=?,instAddress=?," +
                        "instLogo='" + base64String + "',instContact='" + txtbContact.Text + "',instWebsite=?," +
                        "instMail=?,libraryName=?,countryName=?,dialCode=?,currencyName=?,currShortName=?,currSymbol=?,printerName=?,mngmntMail=?";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@instName", txtbFullName.Text);
                    sqltCommnd.Parameters.AddWithValue("@instShortName", txtbShortName.Text);
                    sqltCommnd.Parameters.AddWithValue("@instAddress", txtbAddress.Text.Replace(Environment.NewLine, ""));
                    sqltCommnd.Parameters.AddWithValue("@instWebsite", TxtbWebsite.Text);
                    sqltCommnd.Parameters.AddWithValue("@instMail", txtbMailId.Text);
                    sqltCommnd.Parameters.AddWithValue("@libraryName", txtbLibraryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                    sqltCommnd.Parameters.AddWithValue("@dialCode", txtbCode.Text);
                    sqltCommnd.Parameters.AddWithValue("@currencyName", cmbCurrName.Text);
                    sqltCommnd.Parameters.AddWithValue("@currShortName", cmbCurrShort.Text);
                    sqltCommnd.Parameters.AddWithValue("@currSymbol", cmbCurrSymbol.Text);
                    sqltCommnd.Parameters.AddWithValue("@printerName", txtbPrinter.Text);
                    sqltCommnd.Parameters.AddWithValue("@mngmntMail", txtbMngmnt.Text);
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
                    string queryString = "update general_settings set instName=@instName,instShortName=@instShortName,instAddress=@instAddress," +
                        "instLogo='" + base64String + "',instContact='" + txtbContact.Text + "',instWebsite=@instWebsite," +
                        "instMail=@instMail,libraryName=@libraryName,countryName=@countryName,dialCode=@dialCode," +
                        "currencyName=@currencyName,currShortName=@currShortName,currSymbol=@currSymbol,printerName=@printerName,mngmntMail=@mngmntMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@instName", txtbFullName.Text);
                    mysqlCmd.Parameters.AddWithValue("@instShortName", txtbShortName.Text);
                    mysqlCmd.Parameters.AddWithValue("@instAddress", txtbAddress.Text.Replace(Environment.NewLine, ""));
                    mysqlCmd.Parameters.AddWithValue("@instWebsite", TxtbWebsite.Text);
                    mysqlCmd.Parameters.AddWithValue("@instMail", txtbMailId.Text);
                    mysqlCmd.Parameters.AddWithValue("@libraryName", txtbLibraryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                    mysqlCmd.Parameters.AddWithValue("@dialCode", txtbCode.Text);
                    mysqlCmd.Parameters.AddWithValue("@currencyName", cmbCurrName.Text);
                    mysqlCmd.Parameters.AddWithValue("@currShortName", cmbCurrShort.Text);
                    mysqlCmd.Parameters.AddWithValue("@currSymbol", cmbCurrSymbol.Text);
                    mysqlCmd.Parameters.AddWithValue("@printerName", txtbPrinter.Text);
                    mysqlCmd.Parameters.AddWithValue("@mngmntMail", txtbMngmnt.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
               
                MessageBox.Show("Updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string queryString = "Insert into generalSettings (instName,instShortName,instAddress," +
                        "instLogo,instContact,instWebsite,instMail,libraryName,countryName,dialCode,currencyName," +
                        "currShortName,currSymbol,printerName,mngmntMail) values (@instName,@instShortName,@instAddress,'" + base64String + "','" + txtbContact.Text + "',@instWebsite,@instMail," +
                        "@libraryName,@countryName,@dialCode,@currencyName,@currShortName,@currSymbol,@printerName,@mngmntMail);";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@instName", txtbFullName.Text);
                    sqltCommnd.Parameters.AddWithValue("@instShortName", txtbShortName.Text);
                    sqltCommnd.Parameters.AddWithValue("@instAddress", txtbAddress.Text);
                    sqltCommnd.Parameters.AddWithValue("@instWebsite", TxtbWebsite.Text);
                    sqltCommnd.Parameters.AddWithValue("@instMail", txtbMailId.Text);
                    sqltCommnd.Parameters.AddWithValue("@libraryName", txtbLibraryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                    sqltCommnd.Parameters.AddWithValue("@dialCode", txtbCode.Text);
                    sqltCommnd.Parameters.AddWithValue("@currencyName", cmbCurrName.Text);
                    sqltCommnd.Parameters.AddWithValue("@currShortName", cmbCurrShort.Text);
                    sqltCommnd.Parameters.AddWithValue("@currSymbol", cmbCurrSymbol.Text);
                    sqltCommnd.Parameters.AddWithValue("@printerName", txtbPrinter.Text);
                    sqltCommnd.Parameters.AddWithValue("@mngmntMail", txtbMngmnt.Text);
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
                    string queryString = "Insert into general_settings (instName,instShortName,instAddress," +
                        "instLogo,instContact,instWebsite,instMail,libraryName,countryName,dialCode,currencyName," +
                        "currShortName,currSymbol,printerName,mngmntMail) values (@instName,@instShortName,@instAddress,'" + base64String + "','" + txtbContact.Text + "',@instWebsite,@instMail," +
                        "@libraryName,@countryName,@dialCode,@currencyName,@currShortName,@currSymbol,@printerName,@mngmntMail);";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@instName", txtbFullName.Text);
                    mysqlCmd.Parameters.AddWithValue("@instShortName", txtbShortName.Text);
                    mysqlCmd.Parameters.AddWithValue("@instAddress", txtbAddress.Text);
                    mysqlCmd.Parameters.AddWithValue("@instWebsite", TxtbWebsite.Text);
                    mysqlCmd.Parameters.AddWithValue("@instMail", txtbMailId.Text);
                    mysqlCmd.Parameters.AddWithValue("@libraryName", txtbLibraryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                    mysqlCmd.Parameters.AddWithValue("@dialCode", txtbCode.Text);
                    mysqlCmd.Parameters.AddWithValue("@currencyName", cmbCurrName.Text);
                    mysqlCmd.Parameters.AddWithValue("@currShortName", cmbCurrShort.Text);
                    mysqlCmd.Parameters.AddWithValue("@currSymbol", cmbCurrSymbol.Text);
                    mysqlCmd.Parameters.AddWithValue("@printerName", txtbPrinter.Text);
                    mysqlCmd.Parameters.AddWithValue("@mngmntMail", txtbMngmnt.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                MessageBox.Show("Inserted successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            globalVarLms.instName = txtbFullName.Text;
            globalVarLms.instShortName = txtbShortName.Text;
            globalVarLms.instAddress = txtbAddress.Text;
            globalVarLms.currSymbol = cmbCurrSymbol.Text;
            globalVarLms.defaultPrinter = txtbPrinter.Text;
            globalVarLms.backupRequired = true;
            Properties.Settings.Default.Save();
        }

        private void txtbCountry_TextChanged(object sender, EventArgs e)
        {
            if (txtbCountry.Text != "")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select currencyName,cuurShort,currSymbol,dialCode from countryDetails where countryName=@countryName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        cmbCurrName.Items.Clear();
                        cmbCurrShort.Items.Clear();
                        cmbCurrSymbol.Items.Clear();
                        while (dataReader.Read())
                        {
                            txtbCode.Text = dataReader["dialCode"].ToString();
                            string[] currnList = dataReader["currencyName"].ToString().Split(',');
                            foreach (string curName in currnList)
                            {
                                cmbCurrName.Items.Add(curName);
                            }
                            cmbCurrName.SelectedIndex = 0;
                            currnList = dataReader["cuurShort"].ToString().Split(',');
                            foreach (string curName in currnList)
                            {
                                cmbCurrShort.Items.Add(curName);
                            }
                            cmbCurrShort.SelectedIndex = 0;
                            currnList = dataReader["currSymbol"].ToString().Split(',');
                            foreach (string curName in currnList)
                            {
                                cmbCurrSymbol.Items.Add(curName);
                            }
                            cmbCurrSymbol.SelectedIndex = 0;
                        }
                    }
                    dataReader.Close();
                    queryString = "select currencyName,currShortName,currSymbol from generalSettings";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["currencyName"].ToString() != "")
                            {
                                if (cmbCurrName.Items.IndexOf(dataReader["currencyName"].ToString()) >= 0)
                                {
                                    //cmbCurrName.Items.Add(dataReader["currencyName"].ToString());
                                    cmbCurrName.Text = dataReader["currencyName"].ToString();
                                }
                            }

                            if (dataReader["currShortName"].ToString() != "")
                            {
                                if (cmbCurrShort.Items.IndexOf(dataReader["currShortName"].ToString()) >= 0)
                                {
                                    cmbCurrShort.Text = dataReader["currShortName"].ToString();
                                }
                            }

                            if (dataReader["currSymbol"].ToString() != "")
                            {
                                if (cmbCurrSymbol.Items.IndexOf(dataReader["currSymbol"].ToString()) >= 0)
                                {
                                    cmbCurrSymbol.Text = dataReader["currSymbol"].ToString();
                                }
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
                        string queryString = "select currencyName,cuurShort,currSymbol,dialCode from country_details where countryName=@countryName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@countryName", txtbCountry.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            cmbCurrName.Items.Clear();
                            cmbCurrShort.Items.Clear();
                            cmbCurrSymbol.Items.Clear();
                            while (dataReader.Read())
                            {
                                txtbCode.Text = dataReader["dialCode"].ToString();
                                string[] currnList = dataReader["currencyName"].ToString().Split(',');
                                foreach (string curName in currnList)
                                {
                                    cmbCurrName.Items.Add(curName);
                                }
                                cmbCurrName.SelectedIndex = 0;
                                currnList = dataReader["cuurShort"].ToString().Split(',');
                                foreach (string curName in currnList)
                                {
                                    cmbCurrShort.Items.Add(curName);
                                }
                                cmbCurrShort.SelectedIndex = 0;
                                currnList = dataReader["currSymbol"].ToString().Split(',');
                                foreach (string curName in currnList)
                                {
                                    cmbCurrSymbol.Items.Add(curName);
                                }
                                cmbCurrSymbol.SelectedIndex = 0;
                            }
                        }
                        dataReader.Close();

                        queryString = "select currencyName,currShortName,currSymbol from general_settings";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["currencyName"].ToString() != "")
                                {
                                    if (cmbCurrName.Items.IndexOf(dataReader["currencyName"].ToString()) >= 0)
                                    {
                                        //cmbCurrName.Items.Add(dataReader["currencyName"].ToString());
                                        cmbCurrName.Text = dataReader["currencyName"].ToString();
                                    }
                                }

                                if (dataReader["currShortName"].ToString() != "")
                                {
                                    if (cmbCurrShort.Items.IndexOf(dataReader["currShortName"].ToString()) >= 0)
                                    {
                                        cmbCurrShort.Text = dataReader["currShortName"].ToString();
                                    }
                                }

                                if (dataReader["currSymbol"].ToString() != "")
                                {
                                    if (cmbCurrSymbol.Items.IndexOf(dataReader["currSymbol"].ToString()) >= 0)
                                    {
                                        cmbCurrSymbol.Text = dataReader["currSymbol"].ToString();
                                    }
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
            }
        }

        private void btnPrinter_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                txtbPrinter.Text = printDialog.PrinterSettings.PrinterName;
            }
        }

        private void txtbMailId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbCountry_Enter(object sender, EventArgs e)
        {
            txtbCountry.SelectAll();
        }

        private void txtbMngmnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
