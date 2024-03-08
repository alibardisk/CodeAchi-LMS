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
    public partial class FormLostDamage : Form
    {
        public FormLostDamage()
        {
            InitializeComponent();
        }

        int selectCount = 0;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormLostDamage_Load(object sender, EventArgs e)
        {
            cmbSearchBy.SelectedIndex = 0;
            dgvItem.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvItem.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            timer1.Start();
            txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbSearch.AutoCompleteCustomSource = autoCollData;
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
                            dgvItem.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            cmbSearchBy.Items[1] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvItem.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvItem.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            cmbSearchBy.Items[3] = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }
        }

        private void FormLostDamage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvItem.Rows.Clear();
            txtbComment.Clear();
            txtbSearch.Clear();
            rdbLost.Checked = true;
            string queryString = "";
            if (cmbSearchBy.SelectedIndex == 1)
            {
                queryString = "select itemAccession from itemDetails";
            }
            if (cmbSearchBy.SelectedIndex==2)
            {
                queryString = "select brrId from borrowerDetails";
            }
            else if(cmbSearchBy.SelectedIndex==3)
            {
                queryString = "select distinct rackNo from itemDetails";
            }
            if(queryString!="")
            {
                if(Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> locationList = (from IDataRecord r in dataReader
                                                     select (string)r[0]
                            ).ToList();
                        autoCollData.AddRange(locationList.ToArray());
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    queryString = queryString.Replace("itemDetails", "item_details").Replace("borrowerDetails", "borrower_details");
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
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        List<string> locationList = (from IDataRecord r in dataReader
                                                     select (string)r[0]
                            ).ToList();
                        autoCollData.AddRange(locationList.ToArray());
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void txtbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string itemStatus = "";
            if(e.KeyCode==Keys.Enter)
            {
                if(txtbSearch.Text!="")
                {
                    if (cmbSearchBy.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select a search option.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (cmbSearchBy.SelectedIndex == 1)
                    {
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select itemTitle,itemAuthor,isLost,isDamage from itemDetails where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", txtbSearch.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                chkbAll.Checked = true;
                                while (dataReader.Read())
                                {
                                    itemStatus = "Available";
                                    if (Convert.ToBoolean(dataReader["isLost"].ToString())==true)
                                    {
                                        itemStatus = "Lost";
                                    }
                                    else if(Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                    {
                                        itemStatus = "Damaged";
                                    }
                                    dgvItem.Rows.Add(true, txtbSearch.Text, dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
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
                            string queryString = "select itemTitle,itemAuthor,isLost,isDamage from item_details where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", txtbSearch.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                chkbAll.Checked = true;
                                while (dataReader.Read())
                                {
                                    itemStatus = "Available";
                                    if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                    {
                                        itemStatus = "Lost";
                                    }
                                    else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                    {
                                        itemStatus = "Damaged";
                                    }
                                    dgvItem.Rows.Add(true, txtbSearch.Text, dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                        txtbSearch.SelectAll();
                    }
                    else if (cmbSearchBy.SelectedIndex == 2)
                    {
                        List<string> accnList = new List<string> { };
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select itemAccession from issuedItem where brrId=@brrId and itemReturned='" + false + "'";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrId", txtbSearch.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    accnList.Add(dataReader["itemAccession"].ToString());
                                }
                            }
                            dataReader.Close();

                            foreach (string accnNo in accnList)
                            {
                                queryString = "select itemTitle,itemAuthor,isLost,isDamage from itemDetails where itemAccession=@itemAccession";
                                sqltCommnd.CommandText = queryString;
                                sqltCommnd.Parameters.AddWithValue("@itemAccession", accnNo);
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    chkbAll.Checked = true;
                                    itemStatus = "Available";
                                    if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                    {
                                        itemStatus = "Lost";
                                    }
                                    else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                    {
                                        itemStatus = "Damaged";
                                    }
                                    while (dataReader.Read())
                                    {
                                        dgvItem.Rows.Add(true, accnNo, dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
                                    }
                                }
                                dataReader.Close();
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
                            string queryString = "select itemAccession from issued_item where brrId=@brrId and itemReturned='" + false + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", txtbSearch.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    accnList.Add(dataReader["itemAccession"].ToString());
                                }
                            }
                            dataReader.Close();

                            foreach (string accnNo in accnList)
                            {
                                queryString = "select itemTitle,itemAuthor,isLost,isDamage from item_details where itemAccession=@itemAccession";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", accnNo);
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    chkbAll.Checked = true;
                                    while (dataReader.Read())
                                    {
                                        itemStatus = "Available";
                                        if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                        {
                                            itemStatus = "Lost";
                                        }
                                        else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                        {
                                            itemStatus = "Damaged";
                                        }
                                        dgvItem.Rows.Add(true, accnNo, dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
                                    }
                                }
                                dataReader.Close();
                            }
                            mysqlConn.Close();
                        }
                        dgvItem.ClearSelection();
                    }
                    else if (cmbSearchBy.SelectedIndex == 3)
                    {
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select itemAccession,itemTitle,itemAuthor,isLost,isDamage from itemDetails where rackNo=@rackNo and itemAvailable='" + true + "' and isLost='" + false + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@rackNo", txtbSearch.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                chkbAll.Checked = true;
                                while (dataReader.Read())
                                {
                                    itemStatus = "Available";
                                    if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                    {
                                        itemStatus = "Lost";
                                    }
                                    else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                    {
                                        itemStatus = "Damaged";
                                    }
                                    dgvItem.Rows.Add(true, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
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
                            string queryString = "select itemAccession,itemTitle,itemAuthor,isLost,isDamage from item_details where rackNo=@rackNo and itemAvailable='" + true + "' and isLost='" + false + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@rackNo", txtbSearch.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                chkbAll.Checked = true;
                                while (dataReader.Read())
                                {
                                    itemStatus = "Available";
                                    if (Convert.ToBoolean(dataReader["isLost"].ToString()) == true)
                                    {
                                        itemStatus = "Lost";
                                    }
                                    else if (Convert.ToBoolean(dataReader["isDamage"].ToString()) == true)
                                    {
                                        itemStatus = "Damaged";
                                    }
                                    dgvItem.Rows.Add(true, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),itemStatus);
                                }
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                        dgvItem.ClearSelection();
                    }
                    dgvItem.ClearSelection();
                }
            }
        }

        private void txtbSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtbSearch.Text=="")
            {
                dgvItem.Rows.Clear();
                txtbComment.Clear();
                txtbSearch.Clear();
                rdbLost.Checked = true;
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataRow in dgvItem.Rows)
            {
                if (chkbAll.Checked)
                {
                    dataRow.Cells[0].Value = true;
                    dgvItem.CurrentCell = dataRow.Cells[1];
                }
                else
                {
                    dataRow.Cells[0].Value = false;
                    dgvItem.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
        }

        private void dgvItem_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(dgvItem.Rows.Count==0)
            {
                MessageBox.Show("Please add some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach(DataGridViewRow dataRow in dgvItem.Rows)
            {
                if(dataRow.Cells[0].Value.ToString()=="True")
                {
                    selectCount++;
                }
            }

            if(selectCount==0)
            {
                MessageBox.Show("Please check some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            string currentUser = Properties.Settings.Default.currentUserId;
            string lostBy = "";
            if(cmbSearchBy.SelectedIndex==1)
            {
                lostBy = txtbSearch.Text;
            }
            if (Properties.Settings.Default.sqliteDatabase)
            {
                string queryString = "";
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                foreach (DataGridViewRow dataRow in dgvItem.Rows)
                {
                    if (dataRow.Cells[0].Value.ToString() == "True")
                    {
                        if (rdbPerfect.Checked)
                        {
                            queryString = "update itemDetails set isLost='" + false + "',isDamage='" + false + "' where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();

                            queryString = "delete from lostDamage where itemAccn=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                        }
                        else if (rdbLost.Checked)
                        {
                            queryString = "update itemDetails set isLost='" + true + "' where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();

                            queryString = "insert into lostDamage (itemAccn,itemStatus,statusComment,brrId,entryDate,entryBy) values (@itemAccession,'" + "Lost" + "',@statusComment,@brrId,'" + currentDate + "',@entryBy)";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@statusComment", txtbComment.Text);
                            sqltCommnd.Parameters.AddWithValue("@brrId", lostBy);
                            sqltCommnd.Parameters.AddWithValue("@entryBy", currentUser);
                            sqltCommnd.ExecuteNonQuery();

                            sqltCommnd.CommandText = "update issuedItem set itemReturned='" + true + "',returnDate='" + currentDate + "',returnedBy='" + currentUser + "' where " +
                              "itemAccession=@itemAccession and itemReturned='" + false + "'"; ;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                        }
                        else
                        {
                            queryString = "update itemDetails set isDamage='" + true + "' where itemAccession=@itemAccession";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();

                            queryString = "insert into lostDamage (itemAccn,itemStatus,statusComment,brrId,entryDate,entryBy) values (@itemAccession,'" + "Damage" + "',@statusComment,@brrId,'" + currentDate + "',@entryBy)";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@statusComment", txtbComment.Text);
                            sqltCommnd.Parameters.AddWithValue("@brrId", lostBy);
                            sqltCommnd.Parameters.AddWithValue("@entryBy", currentUser);
                            sqltCommnd.ExecuteNonQuery();
                        }
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
                MySqlCommand mysqlCmd;
                string queryString = "";
                foreach (DataGridViewRow dataRow in dgvItem.Rows)
                {
                    if (dataRow.Cells[0].Value.ToString() == "True")
                    {
                        if (rdbPerfect.Checked)
                        {
                            queryString = "update item_details set isLost='" + false + "',isDamage='" + false + "' where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "delete from lost_damage where itemAccn=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else if (rdbLost.Checked)
                        {
                            queryString = "update item_details set isLost='" + true + "' where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "insert into lost_damage (itemAccn,itemStatus,statusComment,brrId,entryDate,entryBy) values (@itemAccession,'" + "Lost" + "',@statusComment,@brrId,'" + currentDate + "',@entryBy)";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@statusComment", txtbComment.Text);
                            mysqlCmd.Parameters.AddWithValue("@brrId", lostBy);
                            mysqlCmd.Parameters.AddWithValue("@entryBy", currentUser);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "update issued_item set itemReturned='" + true + "',returnDate='" + currentDate + "',returnedBy='" + currentUser + "' where " +
                              "itemAccession=@itemAccession and itemReturned='" + false + "'"; ;
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            queryString = "update item_details set isDamage='" + true + "' where itemAccession=@itemAccession";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();

                            queryString = "insert into lost_damage (itemAccn,itemStatus,statusComment,brrId,entryDate,entryBy) values (@itemAccession,'" + "Damage" + "',@statusComment,@brrId,'" + currentDate + "',@entryBy)";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@statusComment", txtbComment.Text);
                            mysqlCmd.Parameters.AddWithValue("@brrId", lostBy);
                            mysqlCmd.Parameters.AddWithValue("@entryBy", currentUser);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                    }
                }
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            cmbSearchBy.SelectedIndex = 0;
            dgvItem.Rows.Clear();
            txtbComment.Clear();
            txtbSearch.Clear();
            MessageBox.Show("Item status updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
