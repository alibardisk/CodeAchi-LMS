using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormBorrowerEdit : Form
    {
        public FormBorrowerEdit()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        string[] fieldList = null;

        private void FormEditBorrower_Load(object sender, EventArgs e)
        {
            pnlStatus.Visible = false;
            cmbValue.Visible = false;
            dgvBorrower.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvBorrower.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("-------select--------");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catName from borrowerSettings order by catName asc";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbCategory.Items.Add(dataReader["catName"].ToString());
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
                    string queryString = "select catName from borrower_settings order by catName asc";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbCategory.Items.Add(dataReader["catName"].ToString());
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
            cmbCategory.SelectedIndex = 0;
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
                            dgvBorrower.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrName")
                        {
                            dgvBorrower.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrAddress")
                        {
                            dgvBorrower.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            lblBrCategory.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }

                        if (lstbFieldList.Items.Count > 0)
                        {
                            if (fieldName == "lblBrName")
                            {
                                lstbFieldList.Items[0] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblMailId")
                            {
                                lstbFieldList.Items[4] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblContact")
                            {
                                lstbFieldList.Items[5] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrAddress")
                            {
                                lstbFieldList.Items[2] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrCategory")
                            {
                                lstbFieldList.Items[1] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }
            }
        }

        private void FormEditBorrower_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvBorrower.Rows.Clear();
            lstbFieldList.Items.Clear();
            if (cmbCategory.SelectedIndex != 0)
            {
                dgvBorrower.ClearSelection();
                lstbFieldList.Items.Add("Name");
                lstbFieldList.Items.Add("Category");
                lstbFieldList.Items.Add("Address");
                lstbFieldList.Items.Add("Gender");
                lstbFieldList.Items.Add("Email Id 1");
                lstbFieldList.Items.Add("Contact");
                lstbFieldList.Items.Add("Membership Plan");
                lstbFieldList.Items.Add("Email Id 2");
                cmbSearch.Items.Clear();
                cmbSearch.Items.Add("Please select a search option...");
                cmbSearch.Items.Add("All Borrower");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select capInfo from borrowerSettings where catName=@catName";
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
                                cmbSearch.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                lstbFieldList.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
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
                                cmbSearch.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                                lstbFieldList.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                loadFieldValue();
                cmbSearch.SelectedIndex = 0;
                cmbSearch.Enabled = true;
            }
            else
            {
                cmbSearch.Enabled = false;
                cmbSearch.Items.Clear();
                txtbAccn.Clear();
                txtbAccn.Enabled = false;
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataRow in dgvBorrower.Rows)
            {
                if (chkbAll.Checked)
                {
                    dataRow.Cells[0].Value = true;
                    dgvBorrower.CurrentCell = dataRow.Cells[1];
                }
                else
                {
                    dataRow.Cells[0].Value = false;
                    dgvBorrower.CurrentCell = dataRow.Cells[1];
                }
            }
            Application.DoEvents();
        }

        private void dgvBorrower_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBorrower.Rows.Count == 0)
            {
                MessageBox.Show("Please add some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow[] dgvCheckedRows = dgvBorrower.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();

            if (dgvCheckedRows.Count() == 0)
            {
                MessageBox.Show("Please check some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure watnt to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string queryString = "";
                string brrIds = "",tempIds="";
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                string taskDesc = "borrower delete";
                string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");

                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;

                    foreach (DataGridViewRow dataRow in dgvCheckedRows)
                    {
                        queryString = "delete from borrowerDetails  where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                        if (brrIds == "")
                        {
                            brrIds = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            brrIds = brrIds + "$" + dataRow.Cells[1].Value.ToString();
                        }
                    }

                    queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + brrIds + "','" + taskDesc + "','" + currentDateTime + "')";
                    sqltCommnd.CommandText = queryString;
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
                    foreach (DataGridViewRow dataRow in dgvCheckedRows)
                    {
                        if(dataRow.Cells[1].Value.ToString().Contains(","))
                        {
                            queryString = "delete from borrower_details where brrId= @brrId";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            if (tempIds == "")
                            {
                                tempIds = dataRow.Cells[1].Value.ToString();
                            }
                            else
                            {
                                tempIds = tempIds + "," + dataRow.Cells[1].Value.ToString();
                            }
                        }
                        if (brrIds == "")
                        {
                            brrIds = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            brrIds = brrIds + "$" + dataRow.Cells[1].Value.ToString();
                        }
                    }
                    if (tempIds != "")
                    {
                        queryString = "delete from borrower_details  where find_in_set(brrId, @brrId)";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", tempIds);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                   
                    queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + brrIds + "','" + taskDesc + "','" + currentDateTime + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                Cursor = Cursors.Default;
                Application.DoEvents();
                globalVarLms.backupRequired = true;
                dgvBorrower.Rows.Clear();
                cmbCategory.SelectedIndex = 0;
                MessageBox.Show("selected borrower deleted successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstbFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtbBrrRnwDate.Enabled = false;
            txtbFreqncy.Enabled = false;
            txtbBrrRnwDate.Text = 0.ToString();
            txtbFreqncy.Text = 1.ToString();
            if (lstbFieldList.SelectedIndex == 1)
            {
                cmbValue.Visible = true;
                cmbValue.Items.Clear();
                cmbValue.Items.AddRange(cmbCategory.Items.Cast<Object>().ToArray());
                cmbValue.SelectedIndex = 0;
            }
            if (lstbFieldList.SelectedIndex == 6)
            {
                cmbValue.Items.Clear();
                cmbValue.Visible = true;
                cmbValue.Items.Add("--Select--");
                string queryString = "select membrshpName from mbershipSetting";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    //======================Item Category add to combobox============
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbValue.Items.Add(dataReader["membrshpName"].ToString());
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
                    queryString = queryString.Replace("mbershipSetting", "mbership_setting");
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbValue.Items.Add(dataReader["membrshpName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                cmbValue.SelectedIndex = 0;
                txtbBrrRnwDate.Enabled = true;
                txtbFreqncy.Enabled = true;
            }
            else if (lstbFieldList.SelectedIndex == 3)
            {
                cmbValue.Visible = true;
                cmbValue.Items.Clear();
                cmbValue.Items.Add("Please select a gender...");
                cmbValue.Items.Add("Male");
                cmbValue.Items.Add("Female");
                cmbValue.Items.Add("Others");
                cmbValue.SelectedIndex = 0;
            }
            else
            {
                cmbValue.Visible = false;
                if (lstbFieldList.SelectedIndex == 5)
                {
                    txtbValue.Text = 0.ToString();
                }
            }
        }

        private void txtbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lstbFieldList.SelectedIndex == 5)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void txtbValue_Enter(object sender, EventArgs e)
        {
            txtbValue.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBorrower.Rows.Count == 0)
            {
                MessageBox.Show("Please add some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow[] dgvCheckedRows = dgvBorrower.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();
            if (dgvCheckedRows.Count() == 0)
            {
                MessageBox.Show("Please check some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lstbFieldList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a field name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstbFieldList.SelectedIndex == 5)
            {
                if (txtbValue.Text == "")
                {
                    txtbValue.Text = 0.ToString();
                }
            }
          
            string columnName = "";
            if (lstbFieldList.SelectedIndex==3)
            {
                if (cmbValue.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a gender.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtbValue.Text = cmbValue.Text;
                columnName = "brrGender";
            }
            else if (lstbFieldList.SelectedIndex == 1)
            {
                if (cmbValue.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtbValue.Text = cmbValue.Text;
                columnName = "brrCategory";
            }
            else if (lstbFieldList.SelectedIndex == 0)
            {
                columnName = "brrName";
            }
            else if (lstbFieldList.SelectedIndex == 2)
            {
                columnName = "brrAddress";
            }
            else if (lstbFieldList.SelectedIndex == 4)
            {
                columnName = "brrMailId";
            }
            else if (lstbFieldList.SelectedIndex == 5)
            {
                columnName = "brrContact";
            }
            else if (lstbFieldList.SelectedIndex == 6)
            {
                columnName = "mbershipDuration,memPlan,memFreq";
            }
            else if (lstbFieldList.SelectedIndex == 7)
            {
                columnName = "addnlMail";
            }
            else
            {
                List<string> filedNameList = fieldList.ToList();
                if (filedNameList.IndexOf("1_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo1";
                }
                else if (filedNameList.IndexOf("2_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo2";
                }
                else if (filedNameList.IndexOf("3_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo3";
                }
                else if (filedNameList.IndexOf("4_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo4";
                }
                else if (filedNameList.IndexOf("5_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo5";
                }
            }

            string brrIds = "", tempIds="";
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            string queryString = "";
            string taskDesc = lstbFieldList.SelectedItem.ToString() + " update of borrower";
            string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    if (lstbFieldList.SelectedIndex != 6)
                    {
                        queryString = "update borrowerDetails set " + columnName + "=:fieldName where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("fieldName", txtbValue.Text);
                        sqltCommnd.ExecuteNonQuery();
                    }
                    else
                    {
                        queryString = "update borrowerDetails set mbershipDuration='" + txtbBrrRnwDate.Text + "',memFreq='" + txtbFreqncy.Text + "',memPlan=:fieldName where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("fieldName", cmbValue.Text);
                        sqltCommnd.ExecuteNonQuery();
                    }
                    if (brrIds == "")
                    {
                        brrIds = dataRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        brrIds = brrIds + "$" + dataRow.Cells[1].Value.ToString();
                    }
                    Application.DoEvents();
                }

                queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + brrIds + "','" + taskDesc + "','" + currentDateTime + "')";
                sqltCommnd.CommandText = queryString;
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
               
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    if (dataRow.Cells[1].Value.ToString().Contains(","))
                    {
                        if (lstbFieldList.SelectedIndex != 6)
                        {
                            queryString = "update borrower_details set " + columnName + "=@fieldName where brrId=@brrId;";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            queryString = "update borrower_details set mbershipDuration='" + txtbBrrRnwDate.Text + "',memFreq='" + txtbFreqncy.Text + "',memPlan=@fieldName where brrId=@brrId";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrId", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@fieldName", cmbValue.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (tempIds == "")
                        {
                            tempIds = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            tempIds = tempIds + "," + dataRow.Cells[1].Value.ToString();
                        }
                    }
                    if (brrIds == "")
                    {
                        brrIds = dataRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        brrIds = brrIds + "$" + dataRow.Cells[1].Value.ToString();
                    }
                    Application.DoEvents();
                }
                if (tempIds != "")
                {
                    if (lstbFieldList.SelectedIndex != 6)
                    {
                        queryString = "update borrower_details set " + columnName + "=@fieldName where find_in_set(brrId,@brrId);";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrIds.Replace("$", ","));
                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        queryString = "update borrower_details set mbershipDuration='" + txtbBrrRnwDate.Text + "',memFreq='" + txtbFreqncy.Text + "',memPlan=@fieldName where find_in_set(brrId,@brrId)";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrIds.Replace("$", ","));
                        mysqlCmd.Parameters.AddWithValue("@fieldName", cmbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                }

                queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + brrIds + "','" + taskDesc + "','" + currentDateTime + "')";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            Cursor = Cursors.Default;
            Application.DoEvents();
            txtbValue.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbValue.Visible = false;
            MessageBox.Show("Selected borrower updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        int validDays = 0;

        private void txtbFreqncy_TextChanged(object sender, EventArgs e)
        {
            if (txtbFreqncy.Text != "")
            {
                txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
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

        private void cmbValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lstbFieldList.SelectedIndex==6)
            {
                validDays = 0;
                if (cmbValue.SelectedIndex != 0)
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = "select membrDurtn,issueLimit from mbershipSetting where membrshpName=@membrshpName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@membrshpName", cmbValue.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
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
                        string queryString = "select membrDurtn,issueLimit from mbership_setting where membrshpName=@membrshpName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@membrshpName", cmbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                validDays = Convert.ToInt32(dataReader["membrDurtn"].ToString());
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    txtbBrrRnwDate.Text = (validDays * Convert.ToInt32(txtbFreqncy.Text)).ToString();
                }
            }
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbSearch.SelectedIndex>0)
            {
                dgvBorrower.Rows.Clear();
                if(cmbSearch.Text=="All Borrower")
                {
                    txtbAccn.Clear();
                    txtbAccn.Enabled = false;
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select brrId,brrName,brrAddress from borrowerDetails where brrCategory=@catName";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBorrower.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["brrAddress"].ToString());
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
                        string queryString = "select brrId,brrName,brrAddress from borrower_details where brrCategory=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvBorrower.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["brrAddress"].ToString());
                                Application.DoEvents();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                else
                {
                    List<string> filedNameList = fieldList.ToList();
                    string columnName = "";
                    if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                    {
                        columnName = "addInfo1";
                    }
                    else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                    {
                        columnName = "addInfo2";
                    }
                    else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                    {
                        columnName = "addInfo3";
                    }
                    else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                    {
                        columnName = "addInfo4";
                    }
                    else if (filedNameList.IndexOf("5_" + cmbSearch.Text) != -1)
                    {
                        columnName = "addInfo5";
                    }
                    List<string> columnData = new List<string> { };
                    autoCollData.Clear();
                    if(Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select distinct " + columnName + " from borrowerDetails where brrCategory=@catName";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            columnData = (from IDataRecord r in dataReader
                                          select (string)r[0]
                               ).ToList();
                            autoCollData.AddRange(columnData.ToArray());
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
                        string queryString = "select distinct " + columnName + " from borrower_details where brrCategory=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            columnData = (from IDataRecord r in dataReader
                                          select (string)r[0]
                               ).ToList();
                            autoCollData.AddRange(columnData.ToArray());
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }

                    txtbAccn.AutoCompleteMode = AutoCompleteMode.Suggest;
                    txtbAccn.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    txtbAccn.AutoCompleteCustomSource = autoCollData;
                    txtbAccn.Enabled = true;
                }
            }
            else
            {
                txtbAccn.Clear();
                txtbAccn.Enabled = false;
            }
        }

        private void txtbAccn_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                dgvBorrower.Rows.Clear();
                List<string> filedNameList = fieldList.ToList();
                string columnName = "";
                if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                {
                    columnName = "addInfo1";
                }
                else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                {
                    columnName = "addInfo2";
                }
                else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                {
                    columnName = "addInfo3";
                }
                else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                {
                    columnName = "addInfo4";
                }
                else if (filedNameList.IndexOf("5_" + cmbSearch.Text) != -1)
                {
                    columnName = "addInfo5";
                }
               
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select brrId,brrName,brrAddress from borrowerDetails where brrCategory=@catName and " + columnName + " = @fieldValue collate nocase";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    sqltCommnd.Parameters.AddWithValue("@fieldValue", txtbAccn.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBorrower.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["brrAddress"].ToString());
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
                    string queryString = "select brrId,brrName,brrAddress from borrower_details where brrCategory=@catName and lower(" + columnName + ") = @fieldValue";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    mysqlCmd.Parameters.AddWithValue("@fieldValue", txtbAccn.Text.ToLower());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvBorrower.Rows.Add(false, dataReader["brrId"].ToString(), dataReader["brrName"].ToString(), dataReader["brrAddress"].ToString());
                            Application.DoEvents();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
        }
    }
}
