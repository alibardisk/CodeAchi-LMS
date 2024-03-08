using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormItemLookup : Form
    {
        public FormItemLookup()
        {
            InitializeComponent();
        }

        JObject jsonObj;
        bool htmlBody = false, breakOperation = false;
        string fieldCaption = "", jsonString = "", sederId = "", mailBody, reciverId, reciverAddress,
            reciverName;
        string[] fieldList = null;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        List<string> blockList = new List<string> { };

        private void FormItemLookup_Load(object sender, EventArgs e)
        {
            panelStatus.Visible = false;
            dgvItemDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvItemDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            cmbItemCategory.Items.Clear();
            cmbItemCategory.Items.Add("-------Select-------");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catName from itemSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbItemCategory.Items.Add(dataReader["catName"].ToString());
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
                    string queryString = "select catName from item_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbItemCategory.Items.Add(dataReader["catName"].ToString());
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
            cmbItemCategory.SelectedIndex = 0;
            jsonString = Properties.Settings.Default.notificationData;
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
                            dgvItemDetails.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            dgvItemDetails.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblCategory")
                        {
                            categoryLabel.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            dgvItemDetails.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                       
                        if (cmbSearch.Items.Count>0)
                        {
                            if (fieldName == "lblAccession")
                            {
                                cmbSearch.Items[1] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblSubCategory")
                            {
                                cmbSearch.Items[2] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblIsbn")
                            {
                                cmbSearch.Items[3] = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                            else if (fieldName == "lblTitle")
                            {
                                cmbSearch.Items[4] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblSubject")
                            {
                                cmbSearch.Items[5] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblAuthor")
                            {
                                cmbSearch.Items[6] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblClassification")
                            {
                                cmbSearch.Items[7] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblLocation")
                            {
                                cmbSearch.Items[8] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPages")
                            {
                                cmbSearch.Items[9] = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblPrice")
                            {
                                cmbSearch.Items[10] = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                        }
                    }
                }
            }
        }

        private void FormItemLookup_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("Please choose a field name...");
            if(cmbItemCategory.SelectedIndex>0)
            {
                cmbSearch.Items.Add("Accession No");
                cmbSearch.Items.Add("Subcategory");
                cmbSearch.Items.Add("ISBN/ISSN");
                cmbSearch.Items.Add("Title");
                cmbSearch.Items.Add("Subject");
                cmbSearch.Items.Add("Author");
                cmbSearch.Items.Add("Classification No");
                cmbSearch.Items.Add("Rack No./Location");
                cmbSearch.Items.Add("No of Pages");
                cmbSearch.Items.Add("Price");

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
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            fieldCaption = dataReader["capInfo"].ToString();
                            fieldList = dataReader["capInfo"].ToString().Split('$');
                        }
                        foreach (string fieldName in fieldList)
                        {
                            if (fieldName != "")
                            {
                                cmbSearch.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
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
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            fieldCaption = dataReader["capInfo"].ToString();
                            fieldList = dataReader["capInfo"].ToString().Split('$');
                        }
                        foreach (string fieldName in fieldList)
                        {
                            if (fieldName != "")
                            {
                                cmbSearch.Items.Add(fieldName.Substring(fieldName.IndexOf("_") + 1));
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                loadFieldValue();
                cmbSearch.Enabled = true;
            }
            else
            {
                cmbSearch.Enabled = false;
            }
            cmbSearch.SelectedIndex = 0;
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtbSearch.Clear();
            if(cmbSearch.SelectedIndex>0)
            {
                lblFldName.Text = cmbSearch.Text + " :";
                string queryString = "";
                if (cmbSearch.SelectedIndex == 1)
                {
                    queryString = "select itemAccession from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 2)
                {
                    queryString = "select subCatName from itemSubCategory where catName=@catName";
                }
                else if (cmbSearch.SelectedIndex == 3)
                {
                    queryString = "select distinct itemIsbn from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 4)
                {
                    queryString = "select distinct itemTitle from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 5)
                {
                    queryString = "select distinct itemSubject from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 6)
                {
                    queryString = "select itemAuthor from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 7)
                {
                    queryString = "select distinct itemClassification from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 8)
                {
                    queryString = "select distinct rackNo from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 9)
                {
                    queryString = "select distinct totalPages from itemDetails where itemCat=@catName";
                }
                else if (cmbSearch.SelectedIndex == 10)
                {
                    queryString = "select distinct itemPrice from itemDetails where itemCat=@catName";
                }
                else
                {
                    List<string> filedNameList = fieldList.ToList();
                    if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                    {
                        queryString = "select distinct addInfo1 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                    {
                        queryString = "select distinct addInfo2 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                    {
                        queryString = "select distinct addInfo3 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                    {
                        queryString = "select distinct addInfo4 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        queryString = "select distinct addInfo5 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("6_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        queryString = "select distinct addInfo6 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("7_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        queryString = "select distinct addInfo7 from itemDetails where itemCat=@catName";
                    }
                    else if (filedNameList.IndexOf("8_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        queryString = "select distinct addInfo8 from itemDetails where itemCat=@catName";
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
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            autoCollData.Clear();
                            List<string> idList = (from IDataRecord r in dataReader
                                                   select (string)r[0]).ToList();
                            autoCollData.AddRange(idList.ToArray());
                            txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbSearch.AutoCompleteCustomSource = autoCollData;
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
                        queryString = queryString.Replace("itemDetails","item_details").Replace("itemSubCategory", "item_subcategory");
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            autoCollData.Clear();
                            List<string> idList = (from IDataRecord r in dataReader
                                                   select (string)r[0]).ToList();
                            autoCollData.AddRange(idList.ToArray());
                            txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                            txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            txtbSearch.AutoCompleteCustomSource = autoCollData;
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
                txtbSearch.Enabled = true;
                txtbSearch.Select();
            }
            else
            {
                txtbSearch.Enabled = false;
            }
        }

        private void loadData(DataTable dataTable)
        {
            BindingSource SBind = new BindingSource();
            SBind.DataSource = dataTable;

            dataTable.Columns[0].ColumnName = "Accession";
            dataTable.Columns[1].ColumnName = "Title";
            dataTable.Columns[2].ColumnName = "ISBN/ISSN";
            dataTable.Columns[3].ColumnName = "Category";
            dataTable.Columns[4].ColumnName = "Subcategory";
            dataTable.Columns[5].ColumnName = "Subject";
            dataTable.Columns[6].ColumnName = "Author";
            dataTable.Columns[7].ColumnName = "Classification No";
            dataTable.Columns[8].ColumnName = "Pages";
            dataTable.Columns[9].ColumnName = "Price";
            if (fieldCaption == "")
            {
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns.RemoveAt(10);
                dataTable.Columns[10].ColumnName = "Entry Date";
                dataTable.Columns[11].ColumnName = "Rack No";
                dataTable.Columns[12].ColumnName = "Available";
                dataTable.Columns[13].ColumnName = "Lost";
                dataTable.Columns[14].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 0)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns.RemoveAt(11);
                dataTable.Columns[11].ColumnName = "Entry Date";
                dataTable.Columns[12].ColumnName = "Rack No";
                dataTable.Columns[13].ColumnName = "Available";
                dataTable.Columns[14].ColumnName = "Lost";
                dataTable.Columns[15].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 1)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns.RemoveAt(12);
                dataTable.Columns[12].ColumnName = "Entry Date";
                dataTable.Columns[13].ColumnName = "Rack No";
                dataTable.Columns[14].ColumnName = "Available";
                dataTable.Columns[15].ColumnName = "Lost";
                dataTable.Columns[16].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 2)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(13);
                dataTable.Columns.RemoveAt(13);
                dataTable.Columns.RemoveAt(13);
                dataTable.Columns.RemoveAt(13);
                dataTable.Columns.RemoveAt(13);
                dataTable.Columns[13].ColumnName = "Entry Date";
                dataTable.Columns[14].ColumnName = "Rack No";
                dataTable.Columns[15].ColumnName = "Available";
                dataTable.Columns[16].ColumnName = "Lost";
                dataTable.Columns[17].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 3)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns[13].ColumnName = fieldList[3].Substring(fieldList[3].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(14);
                dataTable.Columns.RemoveAt(14);
                dataTable.Columns.RemoveAt(14);
                dataTable.Columns.RemoveAt(14);
                dataTable.Columns[14].ColumnName = "Entry Date";
                dataTable.Columns[15].ColumnName = "Rack No";
                dataTable.Columns[16].ColumnName = "Available";
                dataTable.Columns[17].ColumnName = "Lost";
                dataTable.Columns[18].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 4)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns[13].ColumnName = fieldList[3].Substring(fieldList[3].IndexOf("_") + 1);
                dataTable.Columns[14].ColumnName = fieldList[4].Substring(fieldList[4].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(15);
                dataTable.Columns.RemoveAt(15);
                dataTable.Columns.RemoveAt(15);
                dataTable.Columns[15].ColumnName = "Entry Date";
                dataTable.Columns[16].ColumnName = "Rack No";
                dataTable.Columns[17].ColumnName = "Available";
                dataTable.Columns[18].ColumnName = "Lost";
                dataTable.Columns[19].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 5)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns[13].ColumnName = fieldList[3].Substring(fieldList[3].IndexOf("_") + 1);
                dataTable.Columns[14].ColumnName = fieldList[4].Substring(fieldList[4].IndexOf("_") + 1);
                dataTable.Columns[15].ColumnName = fieldList[5].Substring(fieldList[5].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(16);
                dataTable.Columns.RemoveAt(16);
                dataTable.Columns[16].ColumnName = "Entry Date";
                dataTable.Columns[17].ColumnName = "Rack No";
                dataTable.Columns[18].ColumnName = "Available";
                dataTable.Columns[19].ColumnName = "Lost";
                dataTable.Columns[20].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 6)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns[13].ColumnName = fieldList[3].Substring(fieldList[3].IndexOf("_") + 1);
                dataTable.Columns[14].ColumnName = fieldList[4].Substring(fieldList[4].IndexOf("_") + 1);
                dataTable.Columns[15].ColumnName = fieldList[5].Substring(fieldList[5].IndexOf("_") + 1);
                dataTable.Columns[16].ColumnName = fieldList[6].Substring(fieldList[6].IndexOf("_") + 1);
                dataTable.Columns.RemoveAt(17);
                dataTable.Columns[17].ColumnName = "Entry Date";
                dataTable.Columns[18].ColumnName = "Rack No";
                dataTable.Columns[19].ColumnName = "Available";
                dataTable.Columns[20].ColumnName = "Lost";
                dataTable.Columns[21].ColumnName = "Damage";
            }
            else if (fieldCaption.ToCharArray().Count(c => c == '$') == 7)
            {
                dataTable.Columns[10].ColumnName = fieldList[0].Substring(fieldList[0].IndexOf("_") + 1);
                dataTable.Columns[11].ColumnName = fieldList[1].Substring(fieldList[1].IndexOf("_") + 1);
                dataTable.Columns[12].ColumnName = fieldList[2].Substring(fieldList[2].IndexOf("_") + 1);
                dataTable.Columns[13].ColumnName = fieldList[3].Substring(fieldList[3].IndexOf("_") + 1);
                dataTable.Columns[14].ColumnName = fieldList[4].Substring(fieldList[4].IndexOf("_") + 1);
                dataTable.Columns[15].ColumnName = fieldList[5].Substring(fieldList[5].IndexOf("_") + 1);
                dataTable.Columns[16].ColumnName = fieldList[6].Substring(fieldList[6].IndexOf("_") + 1);
                dataTable.Columns[17].ColumnName = fieldList[7].Substring(fieldList[7].IndexOf("_") + 1);
                dataTable.Columns[18].ColumnName = "Entry Date";
                dataTable.Columns[19].ColumnName = "Rack No";
                dataTable.Columns[20].ColumnName = "Available";
                dataTable.Columns[21].ColumnName = "Lost";
                dataTable.Columns[22].ColumnName = "Damage";
            }
            dgvItemDetails.DataSource = SBind;
            dgvItemDetails.Refresh();
        }

        private void dgvItemDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvItemDetails.HitTest(e.X, e.Y);
                dgvItemDetails.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvItemDetails.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvItemDetails, new Point(e.X, e.Y));
                }
            }
        }

        private void issueItem_MouseEnter(object sender, EventArgs e)
        {
            issueItem.BackColor = Color.WhiteSmoke;
            issueItem.ForeColor = Color.Black;
        }

        private void issueItem_MouseLeave(object sender, EventArgs e)
        {
            issueItem.BackColor = Color.FromArgb(76, 82, 90);
            issueItem.ForeColor = Color.WhiteSmoke;
        }

        private void reserveItemToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            issueItem.BackColor = Color.WhiteSmoke;
            issueItem.ForeColor = Color.Black;
        }

        private void FormItemLookup_FormClosing(object sender, FormClosingEventArgs e)
        {
            breakOperation = true;
        }

        private void backgroundWorkerItems_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lblScanning.Text = "Please Wait...While Fetching Data...";
                panelStatus.Visible = true;
                Application.DoEvents();
                dgvItemDetails.Rows.Clear();
                if (txtbSearch.Text != "")
                {
                    string conditionString = "";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        conditionString = "itemAccession like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 2)
                    {
                        conditionString = "itemSubCat like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        conditionString = "itemIsbn like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        conditionString = "itemTitle like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 5)
                    {
                        conditionString = "itemSubject like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 6)
                    {
                        conditionString = "itemAuthor like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 7)
                    {
                        conditionString = "itemClassification like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 8)
                    {
                        conditionString = "rackNo like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 9)
                    {
                        conditionString = "totalPages like @fieldName and itemCat=@itemCat";
                    }
                    else if (cmbSearch.SelectedIndex == 10)
                    {
                        conditionString = "itemPrice like @fieldName and itemCat=@itemCat";
                    }
                    else
                    {
                        List<string> filedNameList = fieldList.ToList();
                        if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo1 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo2 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo3 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo4 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            conditionString = "addInfo5 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("6_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo6 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("7_" + cmbSearch.Text) != -1)
                        {
                            conditionString = "addInfo7 like @fieldName and itemCat=@itemCat";
                        }
                        else if (filedNameList.IndexOf("8_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            conditionString = "addInfo8 like @fieldName and itemCat=@itemCat";
                        }
                    }
                    if (conditionString != "")
                    {
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select itemAccession,itemTitle,itemAuthor,itemAvailable " +
                                                "from itemDetails where " + conditionString + " and isLost='" + false + "' and isDamage='" + false + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                SQLiteCommand sqltCmd1 = sqltConn.CreateCommand();
                                SQLiteDataReader dataReader1 = null;
                                while (dataReader.Read())
                                {
                                    if (breakOperation)
                                    {
                                        break;
                                    }
                                    try
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                        pcbBook.Image = Image.FromStream(memoryStream, true);
                                    }
                                    catch
                                    {
                                        pcbBook.Image = Properties.Resources.NoImageAvailable;
                                    }
                                    dgvItemDetails.Rows.Add(dgvItemDetails.Rows.Count + 1, pcbBook.Image, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["itemAvailable"].ToString());

                                    if (dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].Cells[5].Value.ToString().Equals("False"))
                                    {
                                        sqltCmd1.CommandText = "select expectedReturnDate from issuedItem where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                        sqltCmd1.Parameters.AddWithValue("@itemAccession", dataReader["itemAccession"].ToString());
                                        dataReader1 = sqltCmd1.ExecuteReader();
                                        if (dataReader1.HasRows)
                                        {
                                            while (dataReader1.Read())
                                            {
                                                dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].Cells[6].Value =FormatDate.getUserFormat(dataReader1["expectedReturnDate"].ToString());
                                            }
                                            Application.DoEvents();
                                        }
                                        dataReader1.Close();
                                        dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                            dataReader.Close();
                            sqltConn.Close();
                        }
                        else
                        {
                            string columnName = conditionString.Substring(0, conditionString.IndexOf(" "));
                            conditionString = conditionString.Replace(columnName, "lower(" + columnName + ")");
                           
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
                            string queryString = "select itemAccession,itemTitle,itemAuthor,itemAvailable " +
                                                "from item_details where " + conditionString + " and isLost='" + false + "' and isDamage='" + false + "'";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text.ToLower() + "%");
                            mysqlCmd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();

                            if (dataReader.HasRows)
                            {
                                MySqlConnection mysqlConn1;
                                mysqlConn1 = ConnectionClass.mysqlConnection();
                                if (mysqlConn1.State == ConnectionState.Closed)
                                {
                                    mysqlConn1.Open();
                                }
                                MySqlCommand mysqlCmd1 = null;
                                MySqlDataReader dataReader1 = null;
                                while (dataReader.Read())
                                {
                                    if (breakOperation)
                                    {
                                        break;
                                    }
                                    try
                                    {
                                        byte[] imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                                        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                        memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                        pcbBook.Image = Image.FromStream(memoryStream, true);
                                    }
                                    catch
                                    {
                                        pcbBook.Image = Properties.Resources.NoImageAvailable;
                                    }
                                    dgvItemDetails.Rows.Add(dgvItemDetails.Rows.Count + 1, pcbBook.Image, dataReader["itemAccession"].ToString(),
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(), dataReader["itemAvailable"].ToString());

                                    if (dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].Cells[5].Value.ToString().Equals("False"))
                                    {
                                        queryString = "select expectedReturnDate from issued_item where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                        mysqlCmd1 = new MySqlCommand(queryString, mysqlConn1);
                                        mysqlCmd1.Parameters.AddWithValue("@itemAccession", dataReader["itemAccession"].ToString());
                                        mysqlCmd1.CommandTimeout = 99999;
                                        dataReader1 = mysqlCmd1.ExecuteReader();
                                        if (dataReader1.HasRows)
                                        {
                                            while (dataReader1.Read())
                                            {
                                                dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].Cells[6].Value =FormatDate.getUserFormat(dataReader1["expectedReturnDate"].ToString());
                                            }
                                            Application.DoEvents();
                                        }
                                        dataReader1.Close();
                                        dgvItemDetails.Rows[dgvItemDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                                mysqlConn1.Close();
                            }
                            dataReader.Close();
                            mysqlConn.Close();
                        }
                        dgvItemDetails.ClearSelection();
                    }
                }
                panelStatus.Visible = false;
                Application.DoEvents();
            }));
        }

        private void reserveItemToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            issueItem.BackColor = Color.FromArgb(76, 82, 90);
            issueItem.ForeColor = Color.WhiteSmoke;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            backgroundWorkerItems.WorkerSupportsCancellation = true;
            if (!backgroundWorkerItems.IsBusy)
            {
                backgroundWorkerItems.RunWorkerAsync();
            }

            else
            {
                backgroundWorkerItems.CancelAsync();
                backgroundWorkerItems.RunWorkerAsync();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbItemCategory.SelectedIndex = 0;
            dgvItemDetails.Rows.Clear();
        }

        private void issueItem_Click(object sender, EventArgs e)
        {
            if (dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor == Color.Red)
            {
                MessageBox.Show("You can,t issue a already issued item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItemDetails.ClearSelection();
            }
            else
            {
                globalVarLms.itemAccn = dgvItemDetails.SelectedRows[0].Cells[2].Value.ToString();
                breakOperation = true;
                this.Close();
            }
        }

        private void reserveItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor != Color.Red)
            {
                MessageBox.Show("You can,t reserve an available item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItemDetails.ClearSelection();
            }
            else
            {
                List<string> catList = globalVarLms.tempValue.Split('$').ToList();
                string catAccess = "";
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            catAccess = dataReader["itemCat"].ToString() + "(" + dataReader["itemSubCat"].ToString() + ")";
                        }
                    }
                    dataReader.Close();
                    if (catList.IndexOf(catAccess) != -1)
                    {
                        string reserveDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        sqltCommnd.CommandText = "insert into reservationList (brrId,itemAccn,itemTitle,itemAuthor,reserveLocation,reserveDate,availableDate)" +
                            " values ('" + globalVarLms.brrId + "','',@itemTitle,@itemAuthor,'','" + reserveDate + "','')";
                        sqltCommnd.Parameters.AddWithValue("@itemTitle", dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("@itemAuthor", dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                        globalVarLms.backupRequired = true;
                        MessageBox.Show("Item reserved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ItemReservedNotification(globalVarLms.brrId, dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString(),
                            dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString(), dgvItemDetails.SelectedRows[0].Cells[6].Value.ToString());
                        dgvItemDetails.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("You can,t reserve this item for this borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
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
                    string queryString = "select itemCat,itemSubCat from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            catAccess = dataReader["itemCat"].ToString() + "(" + dataReader["itemSubCat"].ToString() + ")";
                        }
                    }
                    dataReader.Close();

                    if (catList.IndexOf(catAccess) != -1)
                    {
                        string reserveDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        queryString = "insert into reservation_list (brrId,itemAccn,itemTitle,itemAuthor,reserveLocation,reserveDate,availableDate)" +
                            " values ('" + globalVarLms.brrId + "','',@itemTitle,@itemAuthor,'','" + reserveDate + "','')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString());
                        mysqlCmd.Parameters.AddWithValue("@itemAuthor", dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        globalVarLms.backupRequired = true;
                        MessageBox.Show("Item reserved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ItemReservedNotification(globalVarLms.brrId, dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString(),
                            dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString(), dgvItemDetails.SelectedRows[0].Cells[6].Value.ToString());
                        dgvItemDetails.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("You can,t reserve this item for this borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
                    }
                    mysqlConn.Close();
                }
            }
        }

        private void ItemReservedNotification(string issuedBorrower, string itemTitle,string itemAuthor,string expDate)
        {
            htmlBody = false;
            jsonObj = JObject.Parse(jsonString);
            if (Convert.ToBoolean(jsonObj["ReservedMail"].ToString()))
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Mail' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReservedMailTemplate"].ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            sederId = dataReader["senderId"].ToString();
                            mailBody = dataReader["bodyText"].ToString();
                            if (Convert.ToBoolean(dataReader["htmlBody"].ToString()))
                            {
                                htmlBody = true;
                            }
                        }
                    }
                    dataReader.Close();
                    sqltCommnd.CommandText = "select brrName,brrAddress,brrMailId,brrContact from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", issuedBorrower);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            reciverName = dataReader["brrName"].ToString();
                            reciverAddress = dataReader["brrAddress"].ToString();
                            reciverId = dataReader["brrMailId"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Clone();
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
                    string queryString = "select * from notice_template where noticeType='Mail' and tempName=@tempName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReservedMailTemplate"].ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            sederId = dataReader["senderId"].ToString();
                            mailBody = dataReader["bodyText"].ToString();
                            if (Convert.ToBoolean(dataReader["htmlBody"].ToString()))
                            {
                                htmlBody = true;
                            }
                        }
                    }
                    dataReader.Close();

                    queryString = "select brrName,brrAddress,brrMailId,brrContact from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", issuedBorrower);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            reciverName = dataReader["brrName"].ToString();
                            reciverAddress = dataReader["brrAddress"].ToString();
                            reciverId = dataReader["brrMailId"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Clone();
                }
                blockList = Properties.Settings.Default.blockedMail.Split('$').ToList();

                if (blockList.IndexOf(reciverId) == -1)
                {
                    SmtpClient smtpServer = new SmtpClient(Properties.Settings.Default.mailHost, Convert.ToInt32(Properties.Settings.Default.mailPort));
                    MailMessage mailMessage = new MailMessage();
                    //======================================SMTP SETTINGS===================
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Credentials = new NetworkCredential(Properties.Settings.Default.mailId, Properties.Settings.Default.mailPassword);
                    smtpServer.EnableSsl = Convert.ToBoolean(Properties.Settings.Default.mailSsl);

                    try
                    {
                        mailMessage.From = new MailAddress(sederId);
                        mailMessage.To.Add(reciverId);
                        mailMessage.Subject = jsonObj["ReservedMailTemplate"].ToString();
                        mailMessage.IsBodyHtml = htmlBody;
                        mailMessage.Body = mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                            Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                            Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", "").
                             Replace("[$ItemTitle$]", itemTitle).Replace("[$ExpectedDate$]", expDate).Replace("[$ItemAuthor$]", itemAuthor);

                        smtpServer.Send(mailMessage);
                    }
                    catch
                    {

                    }
                }
            }
            if (Convert.ToBoolean(jsonObj["ReservedSms"].ToString()))
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select * from noticeTemplate where noticeType='Sms' and tempName=@tempName";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@tempName", jsonObj["ReservedSmsTemplate"].ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            mailBody = dataReader["bodyText"].ToString();
                        }
                    }
                    dataReader.Close();

                    sqltCommnd.CommandText = "select brrName,brrAddress,brrMailId,brrContact from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", issuedBorrower);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                        while (dataReader.Read())
                        {
                            reciverName = dataReader["brrName"].ToString();
                            reciverAddress = dataReader["brrAddress"].ToString();
                            reciverId = dataReader["brrContact"].ToString();
                        }
                    }
                    dataReader.Close();
                    sqltConn.Clone();
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
                        string queryString = "select * from notice_template where noticeType='Sms' and tempName=@tempName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@tempName", jsonObj["ReservedSmsTemplate"].ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                mailBody = dataReader["bodyText"].ToString();
                            }
                        }
                        dataReader.Close();

                        queryString = "select brrName,brrAddress,brrMailId,brrContact from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", issuedBorrower);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            DateTime entrydate = DateTime.Now, renewDate = DateTime.Now;
                            while (dataReader.Read())
                            {
                                reciverName = dataReader["brrName"].ToString();
                                reciverAddress = dataReader["brrAddress"].ToString();
                                reciverId = dataReader["brrMailId"].ToString();
                            }
                        }
                        dataReader.Close();
                        mysqlConn.Clone();
                    }
                    catch
                    {

                    }
                }
                blockList = Properties.Settings.Default.blockedContact.Split('$').ToList();
                if (blockList.IndexOf(reciverId) == -1)
                {
                    try
                    {
                        string smsApi = Properties.Settings.Default.smsApi.Replace("[$Message$]", mailBody.TrimEnd().Replace("[$BorrowerName$]", reciverName).
                            Replace("[$BorrowerId$]", issuedBorrower).Replace("[$Address$]", reciverAddress).
                            Replace("[$EmilId$]", reciverId).Replace("[$ItemAccession$]", "").
                             Replace("[$ItemTitle$]", itemTitle).Replace("[$ExpectedDate$]", expDate).Replace("[$ItemAuthor$]", itemAuthor));
                        WebRequest webRequest = WebRequest.Create(smsApi.Replace("[$ContactNumber$]", reciverId).Replace("[$BorrowerName$]", reciverName));
                        webRequest.Timeout = 8000;
                        WebResponse webResponse = webRequest.GetResponse();
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void dgvItemDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlRecord.Text = dgvItemDetails.Rows.Count.ToString();
        }

        private void dgvItemDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTtlRecord.Text = dgvItemDetails.Rows.Count.ToString();
        }
    }
}
