using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace CodeAchi_Library_Management_System
{
    public partial class UcItemSettings : UserControl
    {
        public UcItemSettings()
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

        string catName = "";

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvItemSubcat.Rows.Count==0)
            {
                MessageBox.Show("Please add atleast one subcategory.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItemSubcat.Select();
                return;
            }
            string addInfo = null; bool isConsume = false;
            if(chkbConsumable.Checked)
            {
                isConsume = true;
            }
            if (chkbAdditional1.Checked)
            {
                addInfo = "1_" + txtbAdditional1.Text;
            }
            if (chkbAdditional2.Checked)
            {
                addInfo = addInfo+"$" + "2_" + txtbAdditional2.Text;
            }
            if (chkbAdditional3.Checked)
            {
                addInfo = addInfo + "$" + "3_" + txtbAdditional3.Text;
            }
            if (chkbAdditional4.Checked)
            {
                addInfo = addInfo + "$" + "4_" + txtbAdditional4.Text;
            }
            if (chkbAdditional5.Checked)
            {
                addInfo = addInfo + "$" + "5_" + txtbAdditional5.Text;
            }
            if (chkbAdditional6.Checked)
            {
                addInfo = addInfo + "$" + "6_" + txtbAdditional6.Text;
            }
            if (chkbAdditional7.Checked)
            {
                addInfo = addInfo + "$" + "7_" + txtbAdditional7.Text;
            }
            if (chkbAdditional8.Checked)
            {
                addInfo = addInfo + "$" + "8_" + txtbAdditional8.Text;
            }
            if (btnSave.Text == "Sa&ve")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }

                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select catName from itemSettings where  catName =@catName";
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Category already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "INSERT INTO itemSettings (catName,catDesc,capInfo,isConsume) VALUES (@catName,@catDesc,@capInfo,'" + isConsume + "');";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                    sqltCommnd.Parameters.AddWithValue("@capInfo", addInfo);
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
                    string queryString = "select catName from item_settings where  catName =@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Category already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    dataReader.Close();

                    queryString = "INSERT INTO item_settings (catName,catDesc,capInfo,isConsume) VALUES (@catName,@catDesc,@capInfo,'" + isConsume + "');";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                    mysqlCmd.Parameters.AddWithValue("@capInfo", addInfo);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvItemCategory.Rows.Add(txtbCategoryName.Text);
                dgvItemCategory.ClearSelection();
                dgvItemSubcat.Rows.Clear();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category added successfully !";
                showNotification();
                clearData();
                enableDisable();
                contextMenuStrip2.Enabled = true;
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
                    string queryString = "update itemSettings set catDesc=:catDesc,capInfo=:capInfo,[isConsume]='" + isConsume + "' where  catName = @catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("catDesc", txtbCategoryDesc.Text);
                    sqltCommnd.Parameters.AddWithValue("capInfo", addInfo);
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
                    string queryString = "update item_settings set catDesc=@catDesc,capInfo=@capInfo,isConsume='" + isConsume + "' where  catName = @catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                    mysqlCmd.Parameters.AddWithValue("@capInfo", addInfo);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvItemCategory.ClearSelection();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category updated successfully !";
                showNotification();
                clearData();
                enableDisable();
                btnSave.Text = "Sa&ve";
                contextMenuStrip2.Enabled = true;
                txtbCategoryName.Enabled = true;
            }
            chkbAdditional2.TabIndex = 4;
            grpbSubcategory.TabIndex = 5;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtbCategoryName.Text == "")
            {
                MessageBox.Show("Please add category name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbCategoryName.Select();
                return;
            }
            if (txtbSubCatName.Text=="")
            {
                MessageBox.Show("Please add subcategory name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbSubCatName.Select();
                return;
            }
            if (txtbShortname.Text == "")
            {
                MessageBox.Show("Please add short name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbShortname.Select();
                return;
            }
            
            bool canIssue=true;
            if(chkbNotUsed.Checked)
            {
                canIssue = false;
            }
            if (btnAdd.Text == "Add")
            {
                foreach (DataGridViewRow dataRow in dgvItemSubcat.Rows)
                {
                    if (dataRow.Cells[0].Value.ToString() == txtbSubCatName.Text)
                    {
                        MessageBox.Show("Subcategory already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbSubCatName.Select();
                        return;
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
                    string queryString = "INSERT INTO itemSubCategory (catName,subCatName,shortName,issueDays,subCatDesc" +
                        ",notReference,subCatFine) VALUES (@catName,@subCatName,'" + txtbShortname.Text + "'" +
                        ",'" + txtbIssueDays.Text + "',@subCatDesc,'" + canIssue + "','" + txtbFine.Text + "');";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@subCatName", txtbSubCatName.Text);
                    sqltCommnd.Parameters.AddWithValue("@subCatDesc", txtbSubCatDesc.Text);
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
                    string queryString = queryString = "INSERT INTO item_subcategory (catName,subCatName,shortName,issueDays,subCatDesc" +
                        ",notReference,subCatFine) VALUES (@catName,@subCatName,'" + txtbShortname.Text + "'" +
                        ",'" + txtbIssueDays.Text + "',@subCatDesc,'" + canIssue + "','" + txtbFine.Text + "');";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@subCatName", txtbSubCatName.Text);
                    mysqlCmd.Parameters.AddWithValue("@subCatDesc", txtbSubCatDesc.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvItemSubcat.Rows.Add(txtbSubCatName.Text);
                dgvItemSubcat.ClearSelection();
                txtbSubCatName.Clear();
                txtbSubCatDesc.Clear();
                chkbNotUsed.Checked = false;
                txtbShortname.Clear();
                txtbFine.Text = 0.00.ToString("0.00");
                txtbIssueDays.Text = 10.ToString();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Subcategory added successfully !";
                showNotification();
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
                    string queryString = "update itemSubCategory set issueDays='" + txtbIssueDays.Text + "',subCatFine='" + txtbFine.Text + "'," +
                        "subCatDesc=:subCatDesc,notReference='" + canIssue + "' where catName=@catName and subCatName=@subCatName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@subCatName", txtbSubCatName.Text);
                    sqltCommnd.Parameters.AddWithValue("subCatDesc", txtbSubCatDesc.Text);
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
                    string queryString = "update item_subcategory set issueDays = '" + txtbIssueDays.Text + "',subCatFine = '" + txtbFine.Text + "'," +
                        "subCatDesc=@subCatDesc,notReference='" + canIssue + "' where catName=@catName and subCatName=@subCatName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@subCatName", txtbSubCatName.Text);
                    mysqlCmd.Parameters.AddWithValue("@subCatDesc", txtbSubCatDesc.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvItemSubcat.ClearSelection();
                dgvItemSubcat.ClearSelection();
                txtbSubCatName.Clear();
                txtbSubCatDesc.Clear();
                chkbNotUsed.Checked = false;
                chkbNotUsed.Enabled = true;
                txtbShortname.Clear();
                txtbFine.Text = 0.00.ToString("0.00");
                txtbIssueDays.Text = 10.ToString();
                txtbSubCatName.Enabled = true;
                txtbShortname.Enabled = true;
                btnAdd.Text = "Add";
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Subcategory updated successfully !";
                showNotification();
                btnSave.Enabled = true;
                txtbCategoryName.Enabled = true;
            }
            txtbSubCatName.Select();
        }

        public void UcItemSettings_Load(object sender, EventArgs e)
        {
            enableDisable();
            clearData();
            txtbCategoryName.Select();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgvItemCategory.SelectedRows.Count == 1)
            {
                dgvItemSubcat.Rows.Clear();
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from itemSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbCategoryName.Enabled = false;
                            txtbCategoryName.Text = dataReader["catName"].ToString();
                            txtbCategoryDesc.Text = dataReader["catDesc"].ToString();
                            string capInfo = dataReader["capInfo"].ToString();
                            if (dataReader["isConsume"].ToString() == "True")
                            {
                                chkbConsumable.Checked = true;
                            }
                            else
                            {
                                chkbConsumable.Checked = true;
                            }

                            if (capInfo != "")
                            {
                                string[] addInfoList = capInfo.Split('$');
                                foreach (string addInfo in addInfoList)
                                {
                                    if (addInfo.Contains("1_"))
                                    {
                                        chkbAdditional1.Checked = true;
                                        chkbAdditional1.Enabled = false;
                                        txtbAdditional1.Text = addInfo.Replace("1_", "");
                                        txtbAdditional1.Enabled = false;
                                    }
                                    else if (addInfo.Contains("2_"))
                                    {
                                        chkbAdditional2.Checked = true;
                                        chkbAdditional2.Enabled = false;
                                        txtbAdditional2.Text = addInfo.Replace("2_", "");
                                        txtbAdditional2.Enabled = false;
                                    }
                                    else if (addInfo.Contains("3_"))
                                    {
                                        chkbAdditional3.Checked = true;
                                        chkbAdditional3.Enabled = false;
                                        txtbAdditional3.Text = addInfo.Replace("3_", "");
                                        txtbAdditional3.Enabled = false;
                                    }
                                    else if (addInfo.Contains("4_"))
                                    {
                                        chkbAdditional4.Checked = true;
                                        chkbAdditional4.Enabled = false;
                                        txtbAdditional4.Text = addInfo.Replace("4_", "");
                                        txtbAdditional4.Enabled = false;
                                    }
                                    else if (addInfo.Contains("5_"))
                                    {
                                        chkbAdditional5.Checked = true;
                                        chkbAdditional5.Enabled = false;
                                        txtbAdditional5.Text = addInfo.Replace("5_", "");
                                        txtbAdditional5.Enabled = false;
                                    }
                                    else if (addInfo.Contains("6_"))
                                    {
                                        chkbAdditional6.Checked = true;
                                        chkbAdditional6.Enabled = false;
                                        txtbAdditional6.Text = addInfo.Replace("6_", "");
                                        txtbAdditional6.Enabled = false;
                                    }
                                    else if (addInfo.Contains("7_"))
                                    {
                                        chkbAdditional7.Checked = true;
                                        chkbAdditional7.Enabled = false;
                                        txtbAdditional7.Text = addInfo.Replace("7_", "");
                                        txtbAdditional7.Enabled = false;
                                    }
                                    else if (addInfo.Contains("8_"))
                                    {
                                        chkbAdditional8.Checked = true;
                                        chkbAdditional8.Enabled = false;
                                        txtbAdditional8.Text = addInfo.Replace("8_", "");
                                        txtbAdditional8.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    dataReader.Close();

                    queryString = "select subCatName from itemSubCategory where catName=@catName order by subCatName asc";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                   
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvItemSubcat.Rows.Add(dataReader["subCatName"].ToString());
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
                    string queryString = "select * from item_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbCategoryName.Enabled = false;
                            txtbCategoryName.Text = dataReader["catName"].ToString();
                            txtbCategoryDesc.Text = dataReader["catDesc"].ToString();
                            string capInfo = dataReader["capInfo"].ToString();
                            if (dataReader["isConsume"].ToString() == "True")
                            {
                                chkbConsumable.Checked = true;
                            }
                            else
                            {
                                chkbConsumable.Checked = true;
                            }

                            if (capInfo != "")
                            {
                                string[] addInfoList = capInfo.Split('$');
                                foreach (string addInfo in addInfoList)
                                {
                                    if (addInfo.Contains("1_"))
                                    {
                                        chkbAdditional1.Checked = true;
                                        chkbAdditional1.Enabled = false;
                                        txtbAdditional1.Text = addInfo.Replace("1_", "");
                                        txtbAdditional1.Enabled = false;
                                    }
                                    else if (addInfo.Contains("2_"))
                                    {
                                        chkbAdditional2.Checked = true;
                                        chkbAdditional2.Enabled = false;
                                        txtbAdditional2.Text = addInfo.Replace("2_", "");
                                        txtbAdditional2.Enabled = false;
                                    }
                                    else if (addInfo.Contains("3_"))
                                    {
                                        chkbAdditional3.Checked = true;
                                        chkbAdditional3.Enabled = false;
                                        txtbAdditional3.Text = addInfo.Replace("3_", "");
                                        txtbAdditional3.Enabled = false;
                                    }
                                    else if (addInfo.Contains("4_"))
                                    {
                                        chkbAdditional4.Checked = true;
                                        chkbAdditional4.Enabled = false;
                                        txtbAdditional4.Text = addInfo.Replace("4_", "");
                                        txtbAdditional4.Enabled = false;
                                    }
                                    else if (addInfo.Contains("5_"))
                                    {
                                        chkbAdditional5.Checked = true;
                                        chkbAdditional5.Enabled = false;
                                        txtbAdditional5.Text = addInfo.Replace("5_", "");
                                        txtbAdditional5.Enabled = false;
                                    }
                                    else if (addInfo.Contains("6_"))
                                    {
                                        chkbAdditional6.Checked = true;
                                        chkbAdditional6.Enabled = false;
                                        txtbAdditional6.Text = addInfo.Replace("6_", "");
                                        txtbAdditional6.Enabled = false;
                                    }
                                    else if (addInfo.Contains("7_"))
                                    {
                                        chkbAdditional7.Checked = true;
                                        chkbAdditional7.Enabled = false;
                                        txtbAdditional7.Text = addInfo.Replace("7_", "");
                                        txtbAdditional7.Enabled = false;
                                    }
                                    else if (addInfo.Contains("8_"))
                                    {
                                        chkbAdditional8.Checked = true;
                                        chkbAdditional8.Enabled = false;
                                        txtbAdditional8.Text = addInfo.Replace("8_", "");
                                        txtbAdditional8.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    dataReader.Close();

                    queryString = "select subCatName from item_subcategory where catName=@catName order by subCatName asc";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvItemSubcat.Rows.Add(dataReader["subCatName"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                dgvItemSubcat.ClearSelection();
                contextMenuStrip2.Enabled = false;
                txtbSubCatName.Clear();
                txtbSubCatDesc.Clear();
                txtbShortname.Clear();
                txtbIssueDays.Text = 10.ToString();
                txtbFine.Text = 0.00.ToString("0.00");
                chkbConsumable.Checked = false;
                btnAdd.Text = "Add";
                btnSave.Text = "Update";
                this.Refresh();
            }
        }

        public void clearData()
        {
            txtbCategoryName.Clear();
            txtbCategoryDesc.Clear();
            txtbAdditional1.Clear();
            txtbAdditional2.Clear();
            txtbAdditional3.Clear();
            txtbAdditional4.Clear();
            txtbAdditional5.Clear();
            txtbAdditional6.Clear();
            txtbAdditional7.Clear();
            txtbAdditional8.Clear();
            txtbSubCatName.Clear();
            txtbSubCatDesc.Clear();
            chkbNotUsed.Checked = false;
            txtbShortname.Clear();
            txtbFine.Text = 0.00.ToString("0.00");
            txtbIssueDays.Text = 10.ToString();
        }

        public void enableDisable()
        {
            chkbAdditional1.Checked = false;
            chkbAdditional2.Checked = false;
            chkbAdditional3.Checked = false;
            chkbAdditional4.Checked = false;
            chkbAdditional5.Checked = false;
            chkbAdditional6.Checked = false;
            chkbAdditional7.Checked = false;
            chkbAdditional8.Checked = false;
            chkbConsumable.Checked = false;

            chkbAdditional1.Enabled = true;
            chkbAdditional2.Enabled = false;
            chkbAdditional3.Enabled = false;
            chkbAdditional4.Enabled = false;
            chkbAdditional5.Enabled = false;
            chkbAdditional6.Enabled = false;
            chkbAdditional7.Enabled = false;
            chkbAdditional8.Enabled = false;

            txtbAdditional1.Enabled = false;
            txtbAdditional2.Enabled = false;
            txtbAdditional3.Enabled = false;
            txtbAdditional4.Enabled = false;
            txtbAdditional5.Enabled = false;
            txtbAdditional6.Enabled = false;
            txtbAdditional7.Enabled = false;
            txtbAdditional8.Enabled = false;
        }

        private void chkbAdditional1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional1.Checked)
            {
                txtbAdditional1.Enabled = true;
                txtbAdditional1.Select();
                chkbAdditional2.TabIndex = 4;
                grpbSubcategory.TabIndex = 5;
            }
            else
            {
                txtbAdditional1.Enabled = false;
            }
        }

        private void chkbAdditional2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional2.Checked)
            {
                txtbAdditional2.Enabled = true;
                txtbAdditional2.Select();
                chkbAdditional3.TabIndex = 5;
                grpbSubcategory.TabIndex = 6;
            }
            else
            {
                txtbAdditional2.Enabled = false;
            }
        }

        private void chkbAdditional3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional3.Checked)
            {
                txtbAdditional3.Enabled = true;
                txtbAdditional3.Select();
                chkbAdditional4.TabIndex = 6;
                grpbSubcategory.TabIndex = 7;
            }
            else
            {
                txtbAdditional3.Enabled = false;
            }
        }

        private void chkbAdditional4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional4.Checked)
            {
                txtbAdditional4.Enabled = true;
                txtbAdditional4.Select();
                chkbAdditional5.TabIndex = 7;
                grpbSubcategory.TabIndex = 8;
            }
            else
            {
                txtbAdditional4.Enabled = false;
            }
        }

        private void chkbAdditional5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional5.Checked)
            {
                txtbAdditional5.Enabled = true;
                txtbAdditional5.Select();
                chkbAdditional6.TabIndex = 8;
                grpbSubcategory.TabIndex = 9;
            }
            else
            {
                txtbAdditional5.Enabled = false;
            }
        }

        private void chkbAdditional6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional6.Checked)
            {
                txtbAdditional6.Enabled = true;
                txtbAdditional6.Select();
                chkbAdditional7.TabIndex = 9;
                grpbSubcategory.TabIndex = 10;
            }
            else
            {
                txtbAdditional6.Enabled = false;
            }
        }

        private void chkbAdditional7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional7.Checked)
            {
                txtbAdditional7.Enabled = true;
                txtbAdditional7.Select();
                chkbAdditional8.TabIndex = 10;
                grpbSubcategory.TabIndex = 11;
            }
            else
            {
                txtbAdditional7.Enabled = false;
            }
        }

        private void chkbAdditional8_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAdditional8.Checked)
            {
                txtbAdditional8.Enabled = true;
                txtbAdditional8.Select();
                grpbSubcategory.TabIndex = 11;
            }
            else
            {
                txtbAdditional8.Enabled = false;
            }
        }

        private void txtbAdditional1_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional1.Text == "")
            {
                txtbAdditional1.Focus();
                return;
            }
            chkbAdditional2.Select();
        }

        private void txtbAdditional2_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional2.Text == "")
            {
                txtbAdditional2.Focus();
                return;
            }
            chkbAdditional3.Select();
        }

        private void txtbAdditional3_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional3.Text == "")
            {
                txtbAdditional3.Focus();
                return;
            }
            chkbAdditional4.Select();
        }

        private void txtbAdditional4_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional4.Text == "")
            {
                txtbAdditional4.Focus();
                return;
            }
            chkbAdditional5.Select();
        }

        private void txtbAdditional5_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional5.Text == "")
            {
                txtbAdditional5.Focus();
                return;
            }
            chkbAdditional6.Select();
        }

        private void txtbAdditional6_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional6.Text == "")
            {
                txtbAdditional6.Focus();
                return;
            }
            chkbAdditional7.Select();
        }

        private void txtbAdditional7_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional7.Text == "")
            {
                txtbAdditional7.Focus();
                return;
            }
            chkbAdditional8.Select();
        }

        private void txtbAdditional8_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional8.Text == "")
            {
                txtbAdditional8.Focus();
                return;
            }
            btnSave.Select();
        }

        private void txtbAdditional1_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional1.Text != "")
            {
                chkbAdditional2.Enabled = true;
            }
            else
            {
                chkbAdditional2.Enabled = false;
            }
        }

        private void txtbAdditional2_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional2.Text != "")
            {
                chkbAdditional3.Enabled = true;
            }
            else
            {
                chkbAdditional3.Enabled = false;
            }
        }

        private void txtbAdditional3_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional3.Text != "")
            {
                chkbAdditional4.Enabled = true;
            }
            else
            {
                chkbAdditional4.Enabled = false;
            }
        }

        private void txtbAdditional4_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional4.Text != "")
            {
                chkbAdditional5.Enabled = true;
            }
            else
            {
                chkbAdditional5.Enabled = false;
            }
        }

        private void txtbAdditional5_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional5.Text != "")
            {
                chkbAdditional6.Enabled = true;
            }
            else
            {
                chkbAdditional6.Enabled = false;
            }
        }

        private void txtbAdditional6_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional6.Text != "")
            {
                chkbAdditional7.Enabled = true;
            }
            else
            {
                chkbAdditional7.Enabled = false;
            }
        }

        private void txtbAdditional7_TextChanged(object sender, EventArgs e)
        {
            if (txtbAdditional7.Text != "")
            {
                chkbAdditional8.Enabled = true;
            }
            else
            {
                chkbAdditional8.Enabled = false;
            }
        }

        private void chkbNotUsed_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbNotUsed.Checked)
            {
                txtbIssueDays.Enabled = false;
                txtbFine.Enabled = false;
            }
            else
            {
                txtbIssueDays.Enabled = true;
                txtbFine.Enabled = true;
            }
        }

        private void txtbIssueDays_Leave(object sender, EventArgs e)
        {
            if(txtbIssueDays.Text=="")
            {
                txtbIssueDays.Text = 10.ToString();
            }
        }

        private void txtbIssueDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbShortname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                if (!char.IsControl(e.KeyChar))
                {
                    if (txtbShortname.Text.Length > 2)
                    {
                        e.Handled = true;
                        MessageBox.Show("only three character accepted.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void txtbFine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&(e.KeyChar != '.'))
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

        private void txtbFine_Leave(object sender, EventArgs e)
        {
            if (txtbFine.Text == "")
            {
                txtbFine.Text = 0.00.ToString("0.00");
            }
        }

        private void dgvItemCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvItemCategory.HitTest(e.X, e.Y);
                dgvItemCategory.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvItemCategory.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvItemCategory, new Point(e.X, e.Y));
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
            enableDisable();
            btnSave.Text = "Sa&ve";
            txtbCategoryName.Enabled = true;
            txtbCategoryDesc.Enabled = true;
            btnAdd.Text = "Add";
            txtbSubCatName.Enabled = true;
            txtbShortname.Enabled = true;
            dgvItemCategory.ClearSelection();
            dgvItemSubcat.ClearSelection();
            chkbAdditional2.TabIndex = 4;
            grpbSubcategory.TabIndex = 5;
            chkbNotUsed.Enabled = true;
            dgvItemSubcat.Rows.Clear();
        }

        private void dgvItemSubcat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvItemSubcat.HitTest(e.X, e.Y);
                dgvItemSubcat.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvItemSubcat.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip2.Show(dgvItemSubcat, new Point(e.X, e.Y));
                }
            }
        }

        private void updateSubcategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            catName= dgvItemCategory.SelectedCells[0].Value.ToString();
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
                sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                dgvItemSubcat.Rows.Clear();
                txtbCategoryName.Text = dgvItemCategory.SelectedCells[0].Value.ToString();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvItemSubcat.Rows.Add(dataReader["subCatName"].ToString());
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
                string queryString = "select subCatName from item_subcategory where catName=@catName  order by subCatName asc"; ;
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                dgvItemSubcat.Rows.Clear();
                txtbCategoryName.Text = dgvItemCategory.SelectedCells[0].Value.ToString();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvItemSubcat.Rows.Add(dataReader["subCatName"].ToString());
                    }
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            dgvItemSubcat.ClearSelection();
            contextMenuStrip2.Enabled = true;
            btnSave.Text = "Sa&ve";
            btnSave.Enabled = false;
            txtbCategoryName.Enabled = false;
        }

        private void updatetoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from itemSubCategory where subCatName=@subCatName and catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                sqltCommnd.Parameters.AddWithValue("@subCatName", dgvItemSubcat.SelectedCells[0].Value.ToString());
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbSubCatName.Text = dataReader["subCatName"].ToString();
                        txtbSubCatDesc.Text = dataReader["subCatDesc"].ToString();
                        txtbShortname.Text = dataReader["shortName"].ToString();
                        txtbIssueDays.Text = dataReader["issueDays"].ToString();
                        txtbFine.Text = dataReader["subCatFine"].ToString();
                        if (dataReader["notReference"].ToString() == "True")
                        {
                            chkbNotUsed.Checked = false;
                            chkbNotUsed.Enabled = false;
                        }
                        else
                        {
                            chkbNotUsed.Checked = true;
                            chkbNotUsed.Enabled = true;
                        }
                        txtbSubCatName.Enabled = false;
                        txtbShortname.Enabled = false;
                    }
                    btnAdd.Text = "Update";
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
                string queryString = "select * from item_subcategory where subCatName=@subCatName and catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                mysqlCmd.Parameters.AddWithValue("@subCatName", dgvItemSubcat.SelectedCells[0].Value.ToString());
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbSubCatName.Text = dataReader["subCatName"].ToString();
                        txtbSubCatDesc.Text = dataReader["subCatDesc"].ToString();
                        txtbShortname.Text = dataReader["shortName"].ToString();
                        txtbIssueDays.Text = dataReader["issueDays"].ToString();
                        txtbFine.Text = dataReader["subCatFine"].ToString();
                        if (dataReader["notReference"].ToString() == "True")
                        {
                            chkbNotUsed.Checked = false;
                            chkbNotUsed.Enabled = false;
                        }
                        else
                        {
                            chkbNotUsed.Checked = true;
                            chkbNotUsed.Enabled = true;
                        }
                        txtbSubCatName.Enabled = false;
                        txtbShortname.Enabled = false;
                    }
                    btnAdd.Text = "Update";
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void deleteCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("if you delete the category all data associated with that Category will get deleted." + Environment.NewLine +
               "Are you want to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IntPtr hMenu = GetSystemMenu(this.Parent.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
                List<string> itemList = new List<string> { };
                string currentDateTime = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("hhmmss");
               
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    string sourceFile = Properties.Settings.Default.databasePath + @"\LMS.sl3";
                    string destFile = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvItemCategory.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Items_category_" + currentDateTime + ".sl3";
                    File.Copy(sourceFile, destFile);
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    string queryString = "select itemAccession from itemDetails where itemCat=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();
                    ///Delete from items details
                    queryString = "delete from itemDetails where itemCat=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    foreach (string itemId in itemList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issuedItem where itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from paymentDetails where itemAccn=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lostDamage where itemAccn=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        ////=========delete from Opak Details========
                        //queryString = "delete from opakReservation where itemAccn=@itemAccession";
                        //sqltCommnd.CommandText = queryString;
                        //sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        //sqltCommnd.ExecuteNonQuery();
                    }

                    queryString = "delete from itemSubCategory where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    queryString = "delete from itemSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    string combindedString = string.Join("$", itemList.ToArray());
                    string taskDesc = dgvItemCategory.SelectedCells[0].Value.ToString() + " items category delete";
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + combindedString + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
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

                    mysqlCmd = new MySqlCommand();
                    mysqlCmd.Connection = mysqlConn;
                    MySqlBackup mb = new MySqlBackup(mysqlCmd);
                    string backupName = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvItemCategory.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Items_category_" + currentDateTime + ".sql";
                    mb.ExportToFile(backupName);
                    mb.Dispose();

                    string queryString = "select itemAccession from item_details where itemCat=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();

                    ///Delete from items details
                    queryString = "delete from item_details where itemCat=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    foreach (string itemId in itemList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issued_item where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from payment_details where itemAccn=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lost_damage where itemAccn=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        ////=========delete from Opak Details========
                        //queryString = "delete from opak_reservation where itemAccn=@itemAccession";
                        //mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        //mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        //mysqlCmd.ExecuteNonQuery();
                    }

                    queryString = "delete from item_subcategory where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    queryString = "delete from item_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvItemCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    string combindedString = string.Join("$", itemList.ToArray());
                    string taskDesc = dgvItemCategory.SelectedCells[0].Value.ToString() + " items category delete";
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + combindedString + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvItemCategory.Rows.RemoveAt(dgvItemCategory.SelectedRows[0].Index);
                dgvItemSubcat.Rows.Clear();
                clearData();
                enableDisable();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category deleted successfully !";
                showNotification();
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
            }
        }

        private void deleteSubcategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(catName=="")
            {
                return;
            }
            if (MessageBox.Show("if you delete the subcategory all data associated with that subcategory will get deleted." + Environment.NewLine +
              "Are you want to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IntPtr hMenu = GetSystemMenu(this.Parent.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);

                List<string> itemList = new List<string> { };
                string currentDateTime = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("hhmmss");
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    string sourceFile = Properties.Settings.Default.databasePath + @"\LMS.sl3";
                    string destFile = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvItemSubcat.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Items_subcategory_" + currentDateTime + ".sl3";
                    File.Copy(sourceFile, destFile);
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    string queryString = "select itemAccession from itemDetails where itemCat=@catName and itemSubCat=@itemSubCat";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", catName);
                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();
                    ///Delete from items details
                    queryString = "delete from itemDetails where itemCat=@catName and itemSubCat=@itemSubCat";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", catName);
                    sqltCommnd.Parameters.AddWithValue("@itemSubCat", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    foreach (string itemId in itemList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issuedItem where itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from paymentDetails where itemAccn=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lostDamage where itemAccn=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        sqltCommnd.ExecuteNonQuery();

                        ////=========delete from Opak Details========
                        //queryString = "delete from opakReservation where itemAccn=@itemAccession";
                        //sqltCommnd.CommandText = queryString;
                        //sqltCommnd.Parameters.AddWithValue("@itemAccession", itemId);
                        //sqltCommnd.ExecuteNonQuery();
                    }

                    string catlogAccess = catName + "(" + dgvItemSubcat.SelectedCells[0].Value.ToString() + ")";
                    List<string> catIdList = new List<string> { };
                    List<string> catLogList = new List<string> { };
                    queryString = "select categoryId from borrowerSettings";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        catIdList = (from IDataRecord r in dataReader select r["categoryId"].ToString()).ToList();
                    }
                    dataReader.Close();

                    queryString = "select catlogAccess from borrowerSettings";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        catLogList = (from IDataRecord r in dataReader select (string)r["catlogAccess"]).ToList();
                    }
                    dataReader.Close();
                    int i = 0;
                    foreach (string catId in catIdList)
                    {
                        if (catLogList[i].IndexOf("$" + catlogAccess) != -1)
                        {
                            queryString = "update borrowerSettings set catlogAccess=:catlogAccess where categoryId='" + catId + "' ";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("catlogAccess", catLogList[i].Replace("$" + catlogAccess, ""));
                            sqltCommnd.ExecuteNonQuery();
                        }
                        else if (catLogList[i].IndexOf(catlogAccess + "$") != -1)
                        {
                            queryString = "update borrowerSettings set catlogAccess=:catlogAccess where categoryId='" + catId + "' ";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("catlogAccess", catLogList[i].Replace(catlogAccess + "$", ""));
                            sqltCommnd.ExecuteNonQuery();
                        }
                        i++;
                    }
                    queryString = "delete from itemSubCategory where catName=@catName and subCatName=@subCatName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", catName);
                    sqltCommnd.Parameters.AddWithValue("@subCatName", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    string combindedString = string.Join("$", itemList.ToArray());
                    string taskDesc = dgvItemSubcat.SelectedCells[0].Value.ToString() + " items subcategory delete";
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + combindedString + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
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

                    mysqlCmd = new MySqlCommand();
                    mysqlCmd.Connection = mysqlConn;
                    MySqlBackup mb = new MySqlBackup(mysqlCmd);
                    string backupName = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvItemSubcat.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Items_subcategory_" + currentDateTime + ".sql";
                    mb.ExportToFile(backupName);
                    mb.Dispose();

                    string queryString = "select itemAccession from item_details where itemCat=@catName and itemSubCat=@itemSubCat";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", catName);
                    mysqlCmd.Parameters.AddWithValue("@itemSubCat", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        itemList = (from IDataRecord r in dataReader select (string)r["itemAccession"]).ToList();
                    }
                    dataReader.Close();

                    ///Delete from items details
                    queryString = "delete from item_details where itemCat=@catName and itemSubCat=@itemSubCat";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", catName);
                    mysqlCmd.Parameters.AddWithValue("@itemSubCat", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    foreach (string itemId in itemList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issued_item where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from payment_details where itemAccn=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lost_damage where itemAccn=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        mysqlCmd.ExecuteNonQuery();

                        ////=========delete from Opak Details========
                        //queryString = "delete from opak_reservation where itemAccn=@itemAccession";
                        //mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        //mysqlCmd.Parameters.AddWithValue("@itemAccession", itemId);
                        //mysqlCmd.ExecuteNonQuery();
                    }

                    string catlogAccess = catName + "(" + dgvItemSubcat.SelectedCells[0].Value.ToString() + ")";
                    List<string> catIdList = new List<string> { };
                    List<string> catLogList = new List<string> { };

                    queryString = "select categoryId from borrower_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        catIdList = (from IDataRecord r in dataReader select r["categoryId"].ToString()).ToList();
                    }
                    dataReader.Close();

                    queryString = "select catlogAccess from borrower_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        catLogList = (from IDataRecord r in dataReader select (string)r["catlogAccess"]).ToList();
                    }
                    dataReader.Close();

                    int i = 0;
                    foreach (string catId in catIdList)
                    {
                        if (catLogList[i].IndexOf("$" + catlogAccess) != -1)
                        {
                            queryString = "update borrower_settings set catlogAccess=@catlogAccess where categoryId='" + catId + "' ";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.Parameters.AddWithValue("@catlogAccess", catLogList[i].Replace("$" + catlogAccess, ""));
                            mysqlCmd.ExecuteNonQuery();
                        }
                        else if (catLogList[i].IndexOf(catlogAccess + "$") != -1)
                        {
                            queryString = "update borrower_settings set catlogAccess=@catlogAccess where categoryId='" + catId + "' ";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.CommandTimeout = 99999;
                            mysqlCmd.Parameters.AddWithValue("@catlogAccess", catLogList[i].Replace(catlogAccess + "$", ""));
                            mysqlCmd.ExecuteNonQuery();
                        }
                        i++;
                    }

                    queryString = "delete from item_subcategory where catName=@catName and subCatName=@subCatName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.Parameters.AddWithValue("@catName", catName);
                    mysqlCmd.Parameters.AddWithValue("@subCatName", dgvItemSubcat.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    string combindedString = string.Join("$", itemList.ToArray());
                    string taskDesc = dgvItemSubcat.SelectedCells[0].Value.ToString() + " items subcategory delete";
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + combindedString + "', '" + "" + "','" + taskDesc + "','" + currentDateTime + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }

                dgvItemSubcat.Rows.RemoveAt(dgvItemSubcat.SelectedRows[0].Index);
                txtbSubCatName.Clear();
                txtbSubCatDesc.Clear();
                chkbNotUsed.Checked = false;
                txtbShortname.Clear();
                txtbFine.Text = 0.00.ToString("0.00");
                txtbIssueDays.Text = 10.ToString();
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Subcategory deleted successfully !";
                showNotification();
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnClose_Click(null, null);
            timer1.Stop();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            while (panelNotification.Width > 1)
            {
                panelNotification.Location = new Point(panelNotification.Location.X + 1, panelNotification.Location.Y);
                panelNotification.Width = panelNotification.Width - 1;
                Application.DoEvents();
            }
            panelNotification.Visible = false;
        }

        private void showNotification()
        {
            lblProductName.Text = Application.ProductName;
            panelNotification.Visible = true;
            while (panelNotification.Width < 278)
            {
                panelNotification.Location = new Point(panelNotification.Location.X - 1, panelNotification.Location.Y);
                panelNotification.Width = panelNotification.Width + 1;
                Application.DoEvents();
            }
            timer1.Start();
        }

        private void updateToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.WhiteSmoke;
            updateToolStripMenuItem.ForeColor = Color.Black;
        }

        private void updateToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            updateToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void updateSubcategoryToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            updateSubcategoryToolStripMenuItem.BackColor = Color.WhiteSmoke;
            updateSubcategoryToolStripMenuItem.ForeColor = Color.Black;
        }

        private void updateSubcategoryToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            updateSubcategoryToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            updateSubcategoryToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void deleteCategoryToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            deleteCategoryToolStripMenuItem.BackColor = Color.WhiteSmoke;
            deleteCategoryToolStripMenuItem.ForeColor = Color.Black;
        }

        private void deleteCategoryToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            deleteCategoryToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            deleteCategoryToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void updatetoolStripMenuItem1_MouseEnter(object sender, EventArgs e)
        {
            updatetoolStripMenuItem1.BackColor = Color.WhiteSmoke;
            updatetoolStripMenuItem1.ForeColor = Color.Black;
        }

        private void updatetoolStripMenuItem1_MouseLeave(object sender, EventArgs e)
        {
            updatetoolStripMenuItem1.BackColor = Color.FromArgb(76, 82, 90);
            updatetoolStripMenuItem1.ForeColor = Color.WhiteSmoke;
        }

        private void deleteSubcategoryToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            deleteSubcategoryToolStripMenuItem.BackColor = Color.WhiteSmoke;
            deleteSubcategoryToolStripMenuItem.ForeColor = Color.Black;
        }

        private void deleteSubcategoryToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            deleteSubcategoryToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            deleteSubcategoryToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void btnSave_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnSave.Enabled == true)
            {
                btnSave.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnSave.BackColor = Color.DimGray;
            }
        }
    }
}
