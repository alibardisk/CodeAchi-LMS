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
    public partial class FormItemEdit : Form
    {
        public FormItemEdit()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        string[] fieldList=null;

        private void FormItemEdit_Load(object sender, EventArgs e)
        {
            lblNotification.Visible = false;
            lblMessage.Visible = false;
            cmbValue.Visible = false;
            txtbAccn.Visible = false;
            cmbSubcategory.Visible = false;
            lblField.Visible = false;
            cmbSearch.SelectedIndex = 0;
            dgvItem.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvItem.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("-------Select---------");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catName from itemSettings order by catName asc";
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
                    string queryString = "select catName from item_settings order by catName asc";
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
                            cmbSearch.Items[1] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvItem.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            cmbSearch.Items[3] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblCategory")
                        {
                            categoryLabel.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            //if (cmbCategory.Items.Count > 0)
                            //{
                            //    cmbCategory.Items[0] = "Please select a " + fieldValue.Replace(fieldName + "=", "") + "...";
                            //}
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvItem.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblSubCategory")
                        {
                            cmbSearch.Items[2] = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblLocation")
                        {
                            cmbSearch.Items[4] = fieldValue.Replace(fieldName + "=", "");
                        }
                        if(lstbFieldList.Items.Count>0)
                        {
                            if (fieldName == "lblSubCategory")
                            {
                                lstbFieldList.Items[0] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblIsbn")
                            {
                                lstbFieldList.Items[1] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblTitle")
                            {
                                lstbFieldList.Items[2] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblSubject")
                            {
                                lstbFieldList.Items[3] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblAuthor")
                            {
                                lstbFieldList.Items[4] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblClassification")
                            {
                                lstbFieldList.Items[5] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblLocation")
                            {
                                lstbFieldList.Items[6] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPages")
                            {
                                lstbFieldList.Items[7] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPrice")
                            {
                                lstbFieldList.Items[8] = fieldValue.Replace(fieldName + "=", "");
                            }
                        }
                    }
                }
            }
        }

        private void FormItemEdit_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer1.Stop();
            lblNotification.Visible = false;
            lblMessage.Visible = false;
            dgvItem.Rows.Clear();
            lstbFieldList.Items.Clear();
            cmbSearch.SelectedIndex = 0;
            if (cmbCategory.SelectedIndex > 0)
            {
                lstbFieldList.Items.Add("Subcategory");
                lstbFieldList.Items.Add("ISBN");
                lstbFieldList.Items.Add("Title");
                lstbFieldList.Items.Add("Subject");
                lstbFieldList.Items.Add("Author");
                lstbFieldList.Items.Add("Classification No");
                lstbFieldList.Items.Add("Rack No./Location");
                lstbFieldList.Items.Add("No of Pages");
                lstbFieldList.Items.Add("Price");

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
                                lstbFieldList.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                cmbSearch.Enabled = true;
                loadFieldValue();
            }
            else
            {
                cmbSearch.Enabled = false;
            }
        }

        private void cmbSubcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbCategory.SelectedIndex==0)
            {
                MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dgvItem.Rows.Clear();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select  itemTitle,itemAccession,itemAuthor from itemDetails where itemCat=@itemCat and itemSubCat=@itemSubCat";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                sqltCommnd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvItem.Rows.Add(false, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString());
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
                string queryString = "select  itemTitle,itemAccession,itemAuthor from item_details where itemCat=@itemCat and itemSubCat=@itemSubCat";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                mysqlCmd.Parameters.AddWithValue("@itemSubCat", cmbSubcategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvItem.Rows.Add(false, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString());
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            dgvItem.ClearSelection();
        }

        private void txtbAccn_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (cmbCategory.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select a category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
                DataGridViewRow[] dgvRow = dgvItem.Rows.OfType<DataGridViewRow>()
                .Where(x => (string)x.Cells["accnNo"].Value == txtbAccn.Text)
                .ToArray<DataGridViewRow>();
                if (dgvRow.Count() != 0)
                {
                    MessageBox.Show("Already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select itemTitle,itemAuthor from itemDetails where itemAccession=@fieldName";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from itemDetails where itemAccession=@fieldName";
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from itemDetails where itemTitle=@fieldName collate nocase";
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from itemDetails where rackNo=@fieldName";
                    }
                    
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@fieldName", txtbAccn.Text.TrimEnd());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvItem.Rows.Add(true, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString());
                        }
                    }
                    dataReader.Close();
                    sqltConn.Close();
                }
                else
                {
                    string queryString = "select itemTitle,itemAuthor from item_details where itemAccession=@fieldName";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from item_details where itemAccession=@fieldName";
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from item_details where itemTitle=@fieldName";
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        queryString = "select itemAccession,itemTitle,itemAuthor from item_details where rackNo=@fieldName";
                    }

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
                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbAccn.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvItem.Rows.Add(true, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
               
                dgvItem.ClearSelection();
                txtbAccn.Clear();
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(dgvItem.Rows.Count==0)
            {
                MessageBox.Show("Please add some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow[] dgvCheckedRows = dgvItem.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();

            if (dgvCheckedRows.Count() == 0)
            {
                MessageBox.Show("Please check some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if(lstbFieldList.SelectedIndex==-1)
            {
                MessageBox.Show("Please select a field name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (lstbFieldList.SelectedIndex == 7)
            {
                if(txtbValue.Text=="")
                {
                    txtbValue.Text = 0.ToString();
                }
            }
            else if (lstbFieldList.SelectedIndex == 8)
            {
                if (txtbValue.Text == "")
                {
                    txtbValue.Text = 0.00.ToString("0.00");
                }
            }
            string columnName = "";
            if (lstbFieldList.SelectedIndex==0)
            {
                if(cmbValue.SelectedIndex==0)
                {
                    MessageBox.Show("Please select a subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                txtbValue.Text = cmbValue.Text;
                columnName = "itemSubCat";
            }
            else if (lstbFieldList.SelectedIndex == 1)
            {
                columnName = "itemIsbn";
            }
            else if (lstbFieldList.SelectedIndex == 2)
            {
                columnName = "itemTitle";
            }
            else if (lstbFieldList.SelectedIndex == 3)
            {
                columnName = "itemSubject";
            }
            else if (lstbFieldList.SelectedIndex == 4)
            {
                columnName = "itemAuthor";
            }
            else if (lstbFieldList.SelectedIndex == 5)
            {
                columnName = "itemClassification";
            }
            else if (lstbFieldList.SelectedIndex == 6)
            {
                columnName = "rackNo";
            }
            else if (lstbFieldList.SelectedIndex == 7)
            {
                columnName = "totalPages";
            }
            else if (lstbFieldList.SelectedIndex == 8)
            {
                columnName = "itemPrice";
            }
            else
            {
                List<string> filedNameList = fieldList.ToList();
                if(filedNameList.IndexOf("1_"+lstbFieldList.SelectedItem.ToString())!=-1)
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
                else if (filedNameList.IndexOf("6_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo6";
                }
                else if (filedNameList.IndexOf("7_" + lstbFieldList.SelectedItem.ToString()) != -1)
                {
                    columnName = "addInfo7";
                }
                else if (filedNameList.IndexOf("8_" + lstbFieldList.SelectedItem.ToString()) >-1)
                {
                    columnName = "addInfo8";
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
                string queryString = "";
                string itemAccns = "";
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    queryString = "update itemDetails set " + columnName + "=:fieldName where itemAccession=@itemAccession";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                    sqltCommnd.Parameters.AddWithValue("fieldName", txtbValue.Text);
                    sqltCommnd.ExecuteNonQuery();
                    if (itemAccns == "")
                    {
                        itemAccns = dataRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        itemAccns = itemAccns + "$" + dataRow.Cells[1].Value.ToString();
                    }
                }
                Cursor = Cursors.Default;
                Application.DoEvents();
                string taskDesc = lstbFieldList.SelectedItem.ToString() + " update of items";
                string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + itemAccns + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
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
                MySqlCommand mysqlCmd=new MySqlCommand();
                string queryString = "";
                string itemAccns = "",tempAccession="";
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
               
                foreach (DataGridViewRow dataRow in dgvCheckedRows)
                {
                    if(dataRow.Cells[1].Value.ToString().Contains(","))
                    {
                        queryString = "update item_details set " + columnName + "=@fieldName where itemAccession=@itemAccession;";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                        mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        if (tempAccession == "")
                        {
                            tempAccession = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            tempAccession = tempAccession + "," + dataRow.Cells[1].Value.ToString();
                        }
                    }
                    if (itemAccns == "")
                    {
                        itemAccns = dataRow.Cells[1].Value.ToString();
                    }
                    else
                    {
                        itemAccns = itemAccns + "$" + dataRow.Cells[1].Value.ToString();
                    }
                }
                if (tempAccession != "")
                {
                    queryString = "update item_details set " + columnName + "=@fieldName where find_in_set(itemAccession,@itemAccession);";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", tempAccession);
                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbValue.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                }

                Cursor = Cursors.Default;
                Application.DoEvents();
                string taskDesc = lstbFieldList.SelectedItem.ToString() + " update of items";
                string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + itemAccns + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            txtbValue.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbValue.Visible = false;
            MessageBox.Show("Selected items updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lstbFieldList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstbFieldList.SelectedIndex == 0)
            {
                cmbValue.Items.Clear();
                cmbValue.Items.Add("Please select a "+lstbFieldList.Items[0].ToString()+"...");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select subCatName from itemSubCategory where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbValue.Items.Add(dataReader["subCatName"].ToString());
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
                    string queryString = "select subCatName from item_subcategory where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbValue.Items.Add(dataReader["subCatName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                cmbValue.SelectedIndex = 0;
                cmbValue.Visible = true;
            }
            else
            {
                cmbValue.Visible = false;
                if (lstbFieldList.SelectedIndex == 7)
                {
                    txtbValue.Text = 0.ToString();
                }
                else if (lstbFieldList.SelectedIndex == 8)
                {
                    txtbValue.Text = 0.00.ToString("0.00");
                }
            }
        }

        private void txtbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(lstbFieldList.SelectedIndex==7)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            else if (lstbFieldList.SelectedIndex == 8)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if (e.KeyChar == '.'
                    && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvItem.Rows.Count == 0)
            {
                MessageBox.Show("Please add some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow[] dgvCheckedRows = dgvItem.Rows.OfType<DataGridViewRow>().Where(x => (bool)x.Cells[0].Value == true).ToArray<DataGridViewRow>();

            if (dgvCheckedRows.Count()==0)
            {
                MessageBox.Show("Please check some items.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure watnt to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "";
                    sqltCommnd.CommandText = queryString;
                    string itemAccns = "";
                    Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    foreach (DataGridViewRow dataRow in dgvCheckedRows)
                    {
                        queryString = "delete from itemDetails  where itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                        if (Directory.Exists(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                        {
                            foreach (string digitalFile in Directory.GetFiles(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                            {
                                if (digitalFile.Contains("(" + StringExtention.replacrCharacter(dataRow.Cells[1].Value.ToString()) + ")"))
                                {
                                    try
                                    {
                                        File.Delete(digitalFile);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        if (itemAccns == "")
                        {
                            itemAccns = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            itemAccns = itemAccns + "$" + dataRow.Cells[1].Value.ToString();
                        }
                    }
                    Cursor = Cursors.Default;
                    Application.DoEvents();
                    string taskDesc = "Items delete";
                    string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + itemAccns + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
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
                    string queryString = "";
                    string itemAccns = "",tempAccession="";
                    Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    foreach (DataGridViewRow dataRow in dgvCheckedRows)
                    {
                        if (Directory.Exists(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                        {
                            foreach (string digitalFile in Directory.GetFiles(Properties.Settings.Default.databasePath + @"\Digital Reference"))
                            {
                                if (digitalFile.Contains("(" + StringExtention.replacrCharacter(dataRow.Cells[1].Value.ToString()) + ")"))
                                {
                                    try
                                    {
                                        File.Delete(digitalFile);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                        if(dataRow.Cells[1].Value.ToString().Contains(","))
                        {
                            queryString = "delete from item_details where itemAccession=@itemAccession;"; ;
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[1].Value.ToString());
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            if (tempAccession == "")
                            {
                                tempAccession = dataRow.Cells[1].Value.ToString();
                            }
                            else
                            {
                                tempAccession = tempAccession + "," + dataRow.Cells[1].Value.ToString();
                            }
                        }
                        if (itemAccns == "")
                        {
                            itemAccns = dataRow.Cells[1].Value.ToString();
                        }
                        else
                        {
                            itemAccns = itemAccns + "$" + dataRow.Cells[1].Value.ToString();
                        }
                    }
                    if (tempAccession != "")
                    {
                        queryString = "delete from item_details where find_in_set(itemAccession,@itemAccession);"; ;
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", tempAccession);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                    }

                    Cursor = Cursors.Default;
                    Application.DoEvents();
                    string taskDesc = "Items delete";
                    string currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + itemAccns + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                dgvItem.Rows.Clear();
                cmbCategory.SelectedIndex = 0;
                MessageBox.Show("Selected items deleted successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvItem.Rows.Clear();
            timer1.Stop();
            lblMessage.Visible = false;
            lblNotification.Visible = false;
            if (cmbSearch.SelectedIndex >0)
            {
                lblField.Text = cmbSearch.Text + " :";
                lblField.Visible = true;
                Application.DoEvents();
                if (cmbSearch.SelectedIndex == 2)
                {
                    cmbSubcategory.Visible = true;
                    Application.DoEvents();
                    cmbSubcategory.Items.Clear();
                    cmbSubcategory.Items.Add("Please select a "+ cmbSearch.Text + "...");
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select subCatName from itemSubCategory where catName=@catName order by subCatName asc";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                cmbSubcategory.Items.Add(dataReader["subCatName"].ToString());
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
                        string queryString = "select subCatName from item_subcategory where catName=@catName order by subCatName asc";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                cmbSubcategory.Items.Add(dataReader["subCatName"].ToString());
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    cmbSubcategory.SelectedIndex = 0;
                }
                else
                {
                    timer1.Start();
                    lblMessage.Visible = true;
                    txtbAccn.Visible = true;
                    cmbSubcategory.Visible = false;
                    lblField.Visible = true;
                    string queryString = "select  itemTitle,itemAccession,rackNo from itemDetails where itemCat=@itemCat";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        queryString = "select itemAccession from itemDetails where itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        queryString = "select distinct(itemTitle) from itemDetails where itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        queryString = "select  distinct(rackNo) from itemDetails where itemCat=@itemCat";
                    }
                    autoCollData.Clear();
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }

                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            List<string> idList = (from IDataRecord r in dataReader
                                                    select (string)r[0]
                            ).ToList();
                            autoCollData.AddRange(idList.ToArray());
                           
                            txtbAccn.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbAccn.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbAccn.AutoCompleteCustomSource = autoCollData;
                            txtbAccn.Enabled = true;
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
                            List<string> idList = (from IDataRecord r in dataReader
                                                   select (string)r[0]
                               ).ToList();
                            autoCollData.AddRange(idList.ToArray());

                            txtbAccn.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbAccn.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbAccn.AutoCompleteCustomSource = autoCollData;
                            txtbAccn.Enabled = true;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            else
            {
                txtbAccn.Clear();
                txtbAccn.Visible = false;
                cmbSubcategory.Visible = false;
                lblField.Visible = false;
            }
        }

        private void txtbValue_Enter(object sender, EventArgs e)
        {
            txtbValue.Clear();
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
