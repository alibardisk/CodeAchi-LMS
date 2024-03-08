using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_LMS_Search
{
    public partial class FormHistory : Form
    {
        public FormHistory()
        {
            InitializeComponent();
        }

        public string connectionString = "";
        double finePerDay = 0, totalFine = 0; int daysLate = 0; DateTime expectReturn, returnDate, issueDate, reissueDate;

        private void dgvBook_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void dgvBook_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnReturn.Enabled = true;
            btnReissue.Enabled = true;
            lblTtlRecord.Text = dgvBook.Rows.Count.ToString();
        }

        private void dgvBook_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (dgvBook.Rows.Count == 0)
            {
                btnReturn.Enabled = false;
                btnReissue.Enabled = false;
            }
            lblTtlRecord.Text = dgvBook.Rows.Count.ToString();
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAll.Checked)
            {
                foreach (DataGridViewRow dataRow in dgvBook.Rows)
                {
                    dataRow.Cells[0].Value = true;
                    dgvBook.CurrentCell = dataRow.Cells[1];
                }
            }
            else
            {
                foreach (DataGridViewRow dataRow in dgvBook.Rows)
                {
                    dataRow.Cells[0].Value = false;
                    dgvBook.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
            dgvBook.ClearSelection();
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReissue_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnReissue.Enabled == true)
            {
                btnReissue.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnReissue.BackColor = Color.DimGray;
            }
        }

        private void btnReturn_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnReturn.Enabled == true)
            {
                btnReturn.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnReturn.BackColor = Color.DimGray;
            }
        }

        private void btnReissue_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> dataRows = dgvBook.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals("True"));
            if(dataRows.Count()==0)
            {
                MessageBox.Show("Please check some item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            totalFine = 0;
            foreach (DataGridViewRow dataRow in dataRows)
            {
                totalFine = totalFine + Convert.ToDouble(dataRow.Cells[5].Value.ToString());
            }
            if(totalFine>0)
            {
                MessageBox.Show("You have some fine.\rPlease contact to your librarian.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string itemCat = "", itemSubCat = "", strRtnDate = "";
            string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            DateTime returnDate = DateTime.Now.Date;
            Cursor = Cursors.WaitCursor;
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = null;

                foreach (DataGridViewRow dataRow in dataRows)
                {
                    numDay.Value = 1;
                    queryString = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            itemCat = dataReader["itemCat"].ToString();
                            itemSubCat = dataReader["itemSubCat"].ToString();
                        }
                        dataReader.Close();

                        queryString = "select issueDays,notReference from itemSubCategory where catName=@catName and subCatName=@subCatName";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                        sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubCat);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                            }
                        }
                        dataReader.Close();
                        returnDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                        strRtnDate = returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000");
                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "update issuedItem set reissuedDate='" + issueDate + "',expectedReturnDate='" + strRtnDate + "',reissuedBy='Self'" +
                            " where brrId=@brrId and itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value);
                        sqltCommnd.ExecuteNonQuery();
                        dataRow.Cells[4].Value = strRtnDate;
                        dataRow.Cells[5].Value = 0.00;
                    }
                    dataReader.Close();
                }
                sqltConn.Close();
            }
            else
            {
                MySqlConnection mysqlConn=null;
                mysqlConn = new MySqlConnection(connectionString);

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
                MySqlDataReader dataReader;
                string queryString = "";
                foreach (DataGridViewRow dataRow in dataRows)
                {
                    numDay.Value = 1;
                    queryString = "select itemCat,itemSubCat from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            itemCat = dataReader["itemCat"].ToString();
                            itemSubCat = dataReader["itemSubCat"].ToString();
                        }
                        dataReader.Close();

                        queryString = "select issueDays,notReference from item_subcategory where catName=@catName and subCatName=@subCatName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                        mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubCat);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                numDay.Value = Convert.ToInt32(dataReader["issueDays"].ToString());
                            }
                        }
                        dataReader.Close();
                        returnDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                        strRtnDate = returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000");
                      
                        queryString = "update issued_item set reissuedDate='" + issueDate + "',expectedReturnDate='" + strRtnDate + "',reissuedBy='Self'" +
                            " where brrId=@brrId and itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        dataRow.Cells[4].Value = strRtnDate;
                        dataRow.Cells[5].Value = 0.00;
                    }
                    dataReader.Close();
                }
                mysqlConn.Close();
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
            MessageBox.Show("Item reissued successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            chkbAll.Checked = false;
            if (lblDesignation.Text == "Borrower")
            {
                txtbBrrId.Enabled = false;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> dataRows = dgvBook.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals("True"));
            if (dataRows.Count() == 0)
            {
                MessageBox.Show("Please check some item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            totalFine = 0;
            foreach (DataGridViewRow dataRow in dataRows)
            {
                totalFine = totalFine + Convert.ToDouble(dataRow.Cells[5].Value.ToString());
            }
            if (totalFine > 0)
            {
                MessageBox.Show("You have some fine.\rPlease contact to your librarian.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Cursor = Cursors.WaitCursor;
           
            string reserveId = "", brrId = "";
            string strRtnDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            FormReservation reserveItem = new FormReservation();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = null;
                foreach (DataGridViewRow dataRow in dataRows)
                {
                    queryString = "update itemDetails set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    queryString = "update issuedItem set itemReturned='" + true + "',returnDate='" + dataRow.Cells[7].Value.ToString() + "',returnedBy='Self' where " +
                            "brrId=@brrId and itemAccession=@itemAccession and itemReturned='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select id,brrId from reservationList where itemTitle=@itemTitle and itemAuthor=@itemAuthor and availableDate='' order by [id] asc limit 1"; ;
                    sqltCommnd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[2].Value.ToString());
                    sqltCommnd.Parameters.AddWithValue("@itemAuthor", dataRow.Cells[7].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                    reserveId = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            reserveId = dataReader["id"].ToString();
                            brrId = dataReader["brrId"].ToString();
                        }
                    }
                    dataReader.Close();

                    if (reserveId != "")
                    {
                        sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select brrName from borrowerDetails where brrId=@brrId";
                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                reserveItem.txtbName.Text = dataReader["brrName"].ToString();
                            }
                        }
                        dataReader.Close();
                        reserveItem.txtbTitle.Text = dataRow.Cells[3].Value.ToString();
                        reserveItem.txtbAuthor.Text = dataRow.Cells[8].Value.ToString();
                        reserveItem.txtbLocation.Text = dataRow.Cells[9].Value.ToString();
                        reserveItem.txtbLocation.Enabled = false;
                        reserveItem.ShowDialog();
                        sqltCommnd.CommandText = "update reservationList set reserveLocation='" + reserveItem.txtbLocation.Text + "'," +
                            " availableDate='" + dataRow.Cells[7].Value.ToString() + "',itemAccn=@itemAccn where id='" + reserveId + "'";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                    }
                }
                sqltConn.Close();
            }
            else
            {
                MySqlConnection mysqlConn;
                mysqlConn = new MySqlConnection(connectionString);

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
                MySqlDataReader dataReader;
                string queryString ="";
               
                foreach (DataGridViewRow dataRow in dataRows)
                {
                    queryString = "update item_details set itemAvailable='" + true + "' where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    queryString = "update issued_item set itemReturned='" + true + "',returnDate='" + dataRow.Cells[7].Value.ToString() + "',returnedBy='Self' where " +
                            "brrId=@brrId and itemAccession=@itemAccession and itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    queryString = "select id,brrId from reservation_list where itemTitle=@itemTitle and itemAuthor=@itemAuthor and availableDate='' order by [id] asc limit 1"; ;
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemTitle", dataRow.Cells[2].Value.ToString());
                    mysqlCmd.Parameters.AddWithValue("@itemAuthor", dataRow.Cells[7].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    reserveId = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            reserveId = dataReader["id"].ToString();
                            brrId = dataReader["brrId"].ToString();
                        }
                    }
                    dataReader.Close();

                    if (reserveId != "")
                    {
                        queryString = "select brrName from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                reserveItem.txtbName.Text = dataReader["brrName"].ToString();
                            }
                        }
                        dataReader.Close();
                        reserveItem.txtbTitle.Text = dataRow.Cells[3].Value.ToString();
                        reserveItem.txtbAuthor.Text = dataRow.Cells[8].Value.ToString();
                        reserveItem.txtbLocation.Text = dataRow.Cells[9].Value.ToString();
                        reserveItem.txtbLocation.Enabled = false;
                        reserveItem.ShowDialog();
                        queryString= "update reservation_list set reserveLocation='" + reserveItem.txtbLocation.Text + "'," +
                            " availableDate='" + dataRow.Cells[7].Value.ToString() + "',itemAccn=@itemAccn where id='" + reserveId + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[1].Value.ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                }
                mysqlConn.Close();
            }
            Cursor = Cursors.Default;
            Application.DoEvents();
            MessageBox.Show("Item retured successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            chkbAll.Checked = false;
            dgvBook.Rows.Clear();
            txtbBrrId.Clear();
            txtbName.Clear();
            if (lblDesignation.Text == "Borrower")
            {
                txtbBrrId.Enabled = false;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            if (txtbBrrId.Text != "")
            {
                string strReturnDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                dgvBook.Rows.Clear();
                bool avoidFine = false; string brrCategory = "";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    SQLiteDataReader dataReader;

                    sqltCommnd.CommandText = "select brrCategory from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrCategory = dataReader["brrCategory"].ToString();
                        }
                    }
                    dataReader.Close();
                    Cursor = Cursors.WaitCursor;
                    sqltCommnd.CommandText = "select avoidFine from borrowerSettings where catName=@catName";
                    sqltCommnd.Parameters.AddWithValue("@catName", brrCategory);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                        }
                    }
                    dataReader.Close();

                    string queryString = "select itemAccession,issueDate,expectedReturnDate from issuedItem where brrId=@brrId and [itemReturned]='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        string itemCat = "", itemSubcat = "";
                        DataTable tempTable = new DataTable();
                        tempTable.Columns.Add("chkb", typeof(Boolean));
                        tempTable.Columns.Add("accnNo", typeof(String));
                        tempTable.Columns.Add("itemTitle", typeof(String));
                        tempTable.Columns.Add("issueDate", typeof(String));
                        tempTable.Columns.Add("expDate", typeof(String));
                        tempTable.Columns.Add("fine", typeof(String));
                        tempTable.Columns.Add("location", typeof(String));
                        tempTable.Columns.Add("Author", typeof(String));
                        while (dataReader.Read())
                        {
                            returnDate = DateTime.Now.Date;
                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                            if (daysLate < 0)
                            {
                                daysLate = 0;
                            }
                            tempTable.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                "", dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                daysLate, "");
                        }
                        dataReader.Close();
                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            sqltCommnd = sqltConn.CreateCommand();
                            queryString = "select itemTitle,itemCat,itemSubCat,itemAuthor,rackNo from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow[1].ToString());
                            dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][2] = dataReader["itemTitle"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][6] = dataReader["rackNo"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = dataReader["itemAuthor"].ToString();
                                    itemCat = dataReader["itemCat"].ToString();
                                    itemSubcat = dataReader["itemSubCat"].ToString();
                                }
                                dataReader.Close();

                                sqltCommnd = sqltConn.CreateCommand();
                                queryString = "select subCatFine from itemSubCategory where catName=@catName and subCatName=@subCatName";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("@catName", itemCat);
                                sqltCommnd.Parameters.AddWithValue("@subCatName", itemSubcat);
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                    }
                                }
                                dataReader.Close();
                                daysLate = Convert.ToInt32(dataRow[5].ToString());
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                if (!avoidFine)
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][5] = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00");
                                }
                                else
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][5] = 0.00.ToString("0.00");
                                }
                            }
                            dgvBook.Rows.Add(false, dataRow[1].ToString(), dataRow[2].ToString(),
                                dataRow[3].ToString(), dataRow[4].ToString(), dataRow[5].ToString(), dataRow[6].ToString(),
                                strReturnDate, dataRow[7].ToString());
                        }
                        dgvBook.ClearSelection();
                    }
                    else
                    {
                        dgvBook.Rows.Clear();
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = new MySqlConnection(connectionString);

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
                    string queryString = "select brrCategory from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrCategory = dataReader["brrCategory"].ToString();
                        }
                    }
                    dataReader.Close();
                    Cursor = Cursors.WaitCursor;
                    queryString = "select avoidFine from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", brrCategory);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            avoidFine = Convert.ToBoolean(dataReader["avoidFine"].ToString());
                        }
                    }
                    dataReader.Close();

                    queryString = "select itemAccession,issueDate,expectedReturnDate from issued_item where brrId=@brrId and itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        string itemCat = "", itemSubcat = "";
                        DataTable tempTable = new DataTable();
                        tempTable.Columns.Add("chkb", typeof(Boolean));
                        tempTable.Columns.Add("accnNo", typeof(String));
                        tempTable.Columns.Add("itemTitle", typeof(String));
                        tempTable.Columns.Add("issueDate", typeof(String));
                        tempTable.Columns.Add("expDate", typeof(String));
                        tempTable.Columns.Add("fine", typeof(String));
                        tempTable.Columns.Add("location", typeof(String));
                        tempTable.Columns.Add("Author", typeof(String));
                        while (dataReader.Read())
                        {
                            returnDate = DateTime.Now.Date;
                            expectReturn = DateTime.ParseExact(dataReader["expectedReturnDate"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            daysLate = Convert.ToInt32((returnDate - expectReturn).TotalDays);
                            if (daysLate < 0)
                            {
                                daysLate = 0;
                            }
                            tempTable.Rows.Add(false, dataReader["itemAccession"].ToString(),
                                "", dataReader["issueDate"].ToString(), dataReader["expectedReturnDate"].ToString(),
                                daysLate, "");
                        }
                        dataReader.Close();
                        foreach (DataRow dataRow in tempTable.Rows)
                        {
                            queryString = "select itemTitle,itemCat,itemSubCat,itemAuthor,rackNo from item_details where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow[1].ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][2] = dataReader["itemTitle"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][6] = dataReader["rackNo"].ToString();
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][7] = dataReader["itemAuthor"].ToString();
                                    itemCat = dataReader["itemCat"].ToString();
                                    itemSubcat = dataReader["itemSubCat"].ToString();
                                }
                                dataReader.Close();

                                queryString = "select subCatFine from item_subcategory where catName=@catName and subCatName=@subCatName";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@catName", itemCat);
                                mysqlCmd.Parameters.AddWithValue("@subCatName", itemSubcat);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        finePerDay = Convert.ToDouble(dataReader["subCatFine"].ToString());
                                    }
                                }
                                dataReader.Close();
                                daysLate = Convert.ToInt32(dataRow[5].ToString());
                                if (daysLate < 0)
                                {
                                    daysLate = 0;
                                }
                                if (!avoidFine)
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][5] = Math.Round(daysLate * finePerDay, 2, MidpointRounding.ToEven).ToString("0.00");
                                }
                                else
                                {
                                    tempTable.Rows[tempTable.Rows.IndexOf(dataRow)][5] = 0.00.ToString("0.00");
                                }
                            }
                            dgvBook.Rows.Add(false, dataRow[1].ToString(), dataRow[2].ToString(),
                                dataRow[3].ToString(), dataRow[4].ToString(), dataRow[5].ToString(), dataRow[6].ToString(),
                                strReturnDate, dataRow[7].ToString());
                        }
                        dgvBook.ClearSelection();
                    }
                    else
                    {
                        dgvBook.Rows.Clear();
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
            else
            {
                dgvBook.Rows.Clear();
            }
            Cursor = Cursors.Default;
            dgvBook.ClearSelection();
        }

        private void txtbBrrId_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select brrName,brrCategory,brrAddress from borrowerDetails where brrId=@brrId";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbName.Text = dataReader["brrName"].ToString();
                        }
                        dataReader.Close();
                    }
                    sqltConn.Close();
                }
                else
                {
                    MySqlConnection mysqlConn;
                    mysqlConn = new MySqlConnection(connectionString);

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
                    string queryString = "select brrName,brrCategory,brrAddress from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbName.Text = dataReader["brrName"].ToString();
                        }
                        dataReader.Close();
                    }
                    mysqlConn.Close();
                }
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void FormHistory_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                    this.DisplayRectangle);
        }

        private void FormHistory_Load(object sender, EventArgs e)
        {
            if (txtbBrrId.Text != "")
            {
                backgroundWorker1.RunWorkerAsync();
            }
            dgvBook.ClearSelection();
            dgvBook.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvBook.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
           
            btnReturn.Enabled = false;
            btnReissue.Enabled = false;
        }
    }
}
