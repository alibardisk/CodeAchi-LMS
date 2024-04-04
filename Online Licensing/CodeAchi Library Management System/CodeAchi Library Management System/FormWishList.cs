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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormWishList : Form
    {
        public FormWishList()
        {
            InitializeComponent();
        }

        private void FormWishList_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void FormWishList_Load(object sender, EventArgs e)
        {
            dgvWishList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvWishList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                sqltCommnd.CommandText = "select * from wishList";
                sqltCommnd.CommandType = CommandType.Text;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvWishList.Rows.Add(dgvWishList.Rows.Count + 1, dataReader["itemTitle"].ToString(), dataReader["itemType"].ToString(),
                        dataReader["itemAuthor"].ToString(), dataReader["itemPublication"].ToString(), dataReader["queryCount"].ToString());
                        dgvWishList.ClearSelection();
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
                    string queryString = "select * from wish_list";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvWishList.Rows.Add(dgvWishList.Rows.Count + 1, dataReader["itemTitle"].ToString(), dataReader["itemType"].ToString(),
                            dataReader["itemAuthor"].ToString(), dataReader["itemPublication"].ToString(), dataReader["queryCount"].ToString());
                            dgvWishList.ClearSelection();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
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
                        if (fieldName == "lblTitle")
                        {
                            dgvWishList.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            lblTitle.Text= fieldValue.Replace(fieldName + "=", "")+" :";
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvWishList.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            lblAuthor.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtbType.Clear();
            txtbTitle.Clear();
            txtbAuthor.Clear();
            txtbPublication.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtbType.Text=="")
            {
                MessageBox.Show("Please enter the item type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbType.Select();
                return;
            }
            if (txtbTitle.Text == "")
            {
                MessageBox.Show("Please enter the item title.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbTitle.Select();
                return;
            }
            if (txtbAuthor.Text == "")
            {
                MessageBox.Show("Please enter the item author.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbAuthor.Select();
                return;
            }
            if (txtbPublication.Text == "")
            {
                MessageBox.Show("Please enter the item publication.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPublication.Select();
                return;
            }
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                sqltCommnd.CommandText = "select queryCount from wishList where itemType=@itemType and itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemPublication=@itemPublication";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.Parameters.AddWithValue("@itemType", txtbType.Text);
                sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                sqltCommnd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    int queryCount = 0;
                    while (dataReader.Read())
                    {
                        queryCount = Convert.ToInt32(dataReader["queryCount"].ToString());
                    }
                    dataReader.Close();
                    queryCount++;
                    sqltCommnd.CommandText = "update wishList set queryCount='" + queryCount + "' where itemType=@itemType and itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemPublication=@itemPublication";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemType", txtbType.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                    sqltCommnd.ExecuteNonQuery();

                    DataGridViewRow[] dgvCheckedRows = dgvWishList.Rows.OfType<DataGridViewRow>().Where(x =>
                                    (string)x.Cells[1].Value == txtbTitle.Text && (string)x.Cells[2].Value == txtbType.Text
                                    && (string)x.Cells[3].Value == txtbAuthor.Text && (string)x.Cells[4].Value == txtbPublication.Text).ToArray();
                    dgvCheckedRows[0].Cells[5].Value = queryCount;
                }
                else
                {
                    dataReader.Close();
                    string queryDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                    sqltCommnd.CommandText = "insert into wishList (itemType,itemTitle,itemAuthor,itemPublication,queryDate,queryCount)" +
                        " values (@itemType,@itemTitle,@itemAuthor,@itemPublication,'" + queryDate + "', '" + 1 + "')";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@itemType", txtbType.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                    sqltCommnd.ExecuteNonQuery();
                    dgvWishList.Rows.Add(dgvWishList.Rows.Count + 1, txtbTitle.Text, txtbType.Text,
                        txtbAuthor.Text, txtbPublication.Text, 1.ToString());
                    dgvWishList.ClearSelection();
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
                string queryString = "select queryCount from wish_list where itemType=@itemType and itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemPublication=@itemPublication";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@itemType", txtbType.Text);
                mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                mysqlCmd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    int queryCount = 0;
                    while (dataReader.Read())
                    {
                        queryCount = Convert.ToInt32(dataReader["queryCount"].ToString());
                    }
                    dataReader.Close();

                    queryCount++;
                    queryString = "update wish_list set queryCount='" + queryCount + "' where itemType=@itemType and itemTitle=@itemTitle and itemAuthor=@itemAuthor and itemPublication=@itemPublication";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemType", txtbType.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    DataGridViewRow[] dgvCheckedRows = dgvWishList.Rows.OfType<DataGridViewRow>().Where(x =>
                                    (string)x.Cells[1].Value == txtbTitle.Text && (string)x.Cells[2].Value == txtbType.Text
                                    && (string)x.Cells[3].Value == txtbAuthor.Text && (string)x.Cells[4].Value == txtbPublication.Text).ToArray();
                    dgvCheckedRows[0].Cells[5].Value = queryCount;
                }
                else
                {
                    dataReader.Close();
                    string queryDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                    queryString = "insert into wish_list (itemType,itemTitle,itemAuthor,itemPublication,queryDate,queryCount)" +
                        " values (@itemType,@itemTitle,@itemAuthor,@itemPublication,'" + queryDate + "', '" + 1 + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemType", txtbType.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", txtbTitle.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", txtbAuthor.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemPublication", txtbPublication.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    dgvWishList.Rows.Add(dgvWishList.Rows.Count + 1, txtbTitle.Text, txtbType.Text,
                        txtbAuthor.Text, txtbPublication.Text, 1.ToString());
                    dgvWishList.ClearSelection();
                }
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            btnReset_Click(null, null);
            txtbType.Select();
        }

        private void dgvWishList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTotal.Text = dgvWishList.Rows.Count.ToString();
        }

        private void btnCsv_Click(object sender, EventArgs e)
        {
            if (!globalVarLms.isLicensed)
            {
                MessageBox.Show("This feature is not available in the trial version", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (globalVarLms.currentDate > globalVarLms.expiryDate)
            {
                MessageBox.Show("Your License has expired!" + Environment.NewLine + "Please renew to continue enjoying the services.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvWishList.Rows.Count == 0)
            {
                MessageBox.Show("No datafound !", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Csv Files (*.csv)|*.csv";
            saveDialog.DefaultExt = "csv";
            saveDialog.AddExtension = true;
            saveDialog.FileName = "WishList";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                // Choose whether to write header. Use EnableWithoutHeaderText instead to omit header.
                dgvWishList.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                // Select all the cells
                dgvWishList.SelectAll();
                // Copy selected cells to DataObject
                DataObject dataObject = dgvWishList.GetClipboardContent();
                // Get the text of the DataObject, and serialize it to a file
                File.WriteAllText(fileName, dataObject.GetText(TextDataFormat.CommaSeparatedValue));
               
                dgvWishList.ClearSelection();
                MessageBox.Show("Data save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
    }
}
