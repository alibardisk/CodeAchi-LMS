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
    public partial class FormSearchLookup : Form
    {
        public FormSearchLookup()
        {
            InitializeComponent();
        }

        bool breakOperation = false;
        string fieldCaption = "";
        string[] fieldList = null;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        List<string> selctedColumnsList = new List<string> { };
        FormDetails viewDetails = new FormDetails();

        private void FormFind_Load(object sender, EventArgs e)
        {
            panelStatus.Visible = false;
            cmbType.SelectedIndex = 0;
            cmbCategory.Enabled = false;
            cmbSearch.Enabled = false;
            txtbSearch.Enabled = false;

            dgvDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbSearch.AutoCompleteCustomSource = autoCollData;
            addField();
        }

        private void addField()
        {
            viewDetails.dgvItemDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            viewDetails.dgvItemDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            viewDetails.dgvItemDetails.Rows.Clear();
            viewDetails.dgvItemDetails.ColumnHeadersVisible = false;
            viewDetails.dgvItemDetails.Rows.Add("Accession No. :");
            viewDetails.dgvItemDetails.Rows.Add("Title :");
            viewDetails.dgvItemDetails.Rows.Add("Author :");
            viewDetails.dgvItemDetails.Rows.Add("ISBN/ISSN :");
            viewDetails.dgvItemDetails.Rows.Add("Classification No. :");
            viewDetails.dgvItemDetails.Rows.Add("Subject :");
            viewDetails.dgvItemDetails.Rows.Add("Location :");
            viewDetails.dgvItemDetails.Rows.Add("No. of Pages :");
            viewDetails.dgvItemDetails.Rows.Add("Price :");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 1");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 2");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 3");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 4");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 5");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 6");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 7");
            viewDetails.dgvItemDetails.Rows.Add("Additinal Info 8");
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
                            viewDetails.dgvItemDetails.Rows[0].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblTitle")
                        {
                            viewDetails.dgvItemDetails.Rows[1].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblClassification")
                        {
                            viewDetails.dgvItemDetails.Rows[4].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblSubject")
                        {
                            viewDetails.dgvItemDetails.Rows[5].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            viewDetails.dgvItemDetails.Rows[2].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblLocation")
                        {
                            viewDetails.dgvItemDetails.Rows[6].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblIsbn")
                        {
                            viewDetails.dgvItemDetails.Rows[3].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblPages")
                        {
                            viewDetails.dgvItemDetails.Rows[7].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblPrice")
                        {
                            viewDetails.dgvItemDetails.Rows[8].Cells[0].Value = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void loadFieldValue()
        {
            if (cmbType.Text == "Item")
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
                                dgvDetails.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblTitle")
                            {
                                dgvDetails.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblCategory")
                            {
                                categoryLabel.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                            else if (fieldName == "lblAuthor")
                            {
                                dgvDetails.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }

                            if (cmbSearch.Items.Count > 0)
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
            else
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
                                dgvDetails.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrName")
                            {
                                dgvDetails.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblMailId")
                            {
                                dgvDetails.Columns[4].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblContact")
                            {
                                dgvDetails.Columns[5].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            else if (fieldName == "lblBrAddress")
                            {
                                dgvDetails.Columns[6].HeaderText = fieldValue.Replace(fieldName + "=", "");
                            }
                            if (cmbSearch.Items.Count > 0)
                            {
                                if (fieldName == "lblBrId")
                                {
                                    cmbSearch.Items[1] = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblBrName")
                                {
                                    cmbSearch.Items[2] = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblMailId")
                                {
                                    cmbSearch.Items[5] = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblContact")
                                {
                                    cmbSearch.Items[6] = fieldValue.Replace(fieldName + "=", "");
                                }
                                else if (fieldName == "lblBrAddress")
                                {
                                    cmbSearch.Items[3] = fieldValue.Replace(fieldName + "=", "");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FormFind_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                      this.DisplayRectangle);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex > 0)
            {
                if (cmbType.Text == "Member")
                {
                    cmbSearch.Items.Clear();
                    cmbSearch.Items.Add("Please choose a field name...");
                    if (cmbCategory.SelectedIndex > 0)
                    {
                        cmbSearch.Items.Add("Borrower Id");
                        cmbSearch.Items.Add("Name");
                        cmbSearch.Items.Add("Address");
                        cmbSearch.Items.Add("Gender");
                        cmbSearch.Items.Add("Email Id");
                        cmbSearch.Items.Add("Contact");
                        cmbSearch.Items.Add("Membership Plan");
                        if (globalVarLms.sqliteData)
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
                            string queryString = "select capInfo from borrower_settings where catName=@catName";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
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
                        cmbSearch.Enabled = true;
                    }
                    else
                    {
                        cmbSearch.Enabled = false;
                    }
                    //cmbSearch.SelectedIndex = 0;
                    selctedColumnsList.Clear();
                    selctedColumnsList.Add("brrId");
                    selctedColumnsList.Add("brrName");
                    selctedColumnsList.Add("brrGender");
                    selctedColumnsList.Add("brrMailId");
                    selctedColumnsList.Add("brrContact");
                    selctedColumnsList.Add("brrAddress");

                    dgvDetails.Columns.Clear();
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 65;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Id";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 100;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Name";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 200;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Gender";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 65;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Email";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 180;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Contact";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 100;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Address";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvColumn.Width = 158;
                    dgvDetails.Columns.Add(dgvColumn);
                }
                else if (cmbType.Text == "Item")
                {
                    cmbSearch.Items.Clear();
                    cmbSearch.Items.Add("Please choose a field name...");
                    if (cmbCategory.SelectedIndex > 0)
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

                        if (globalVarLms.sqliteData)
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
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
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
                        cmbSearch.Enabled = true;
                    }
                    else
                    {
                        cmbSearch.Enabled = false;

                    }

                    selctedColumnsList.Clear();
                    selctedColumnsList.Add("itemAccession");
                    selctedColumnsList.Add("itemTitle");
                    selctedColumnsList.Add("itemAuthor");
                    selctedColumnsList.Add("rackNo");
                    selctedColumnsList.Add("itemAvailable");

                    dgvDetails.Columns.Clear();
                    DataGridViewTextBoxColumn dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Sl No.";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvColumn.Width = 65;
                    dgvDetails.Columns.Add(dgvColumn);

                    DataGridViewImageColumn dgvIColumn = new DataGridViewImageColumn();
                    dgvIColumn.HeaderText = "Image";
                    dgvIColumn.Name = "Image";
                    dgvIColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvIColumn.Width = 75;
                    dgvIColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    dgvDetails.Columns.Add(dgvIColumn);

                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Accession No";
                    dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 120;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Title of Item";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 250;
                    dgvDetails.Columns.Add(dgvColumn);
                    dgvColumn = new DataGridViewTextBoxColumn();
                    dgvColumn.HeaderText = "Author";
                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvColumn.Width = 240;
                    dgvDetails.Columns.Add(dgvColumn);

                    DataGridViewButtonColumn uninstallButtonColumn = new DataGridViewButtonColumn();
                    uninstallButtonColumn.Name = "view_details";
                    uninstallButtonColumn.HeaderText = "Action";
                    uninstallButtonColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    uninstallButtonColumn.Width = 115;
                    dgvDetails.Columns.Add(uninstallButtonColumn);
                }

                loadFieldValue();
                cmbSearch.SelectedIndex = 0;
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvDetails.Columns.Clear();
            if (cmbType.Text == "Member")
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
                            if (fieldName == "lblBrCategory")
                            {
                                categoryLabel.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                        }
                    }
                }
                else
                {
                    categoryLabel.Text ="Borrower Category :";
                }
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("-------Select-------");
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd = sqltConn.CreateCommand();
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
                cmbCategory.SelectedIndex = 0;
                cmbCategory.Enabled = true;

            }
            else if (cmbType.Text == "Item")
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
                            if (fieldName == "lblCategory")
                            {
                                categoryLabel.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                            }
                        }
                    }
                }
                else
                {
                    categoryLabel.Text = "Items Category :";
                }
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("-------Select-------");
                if (globalVarLms.sqliteData)
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
                cmbCategory.SelectedIndex = 0;
                cmbCategory.Enabled = true;
            }
            else
            {
                cmbCategory.Items.Clear();
                cmbSearch.Items.Clear();
                txtbSearch.Clear();
                cmbCategory.Enabled = false;
                cmbSearch.Enabled = false;
                txtbSearch.Enabled = false;
            }
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.Text == "Member")
            {
                txtbSearch.Clear();
                if (cmbSearch.SelectedIndex > 0)
                {
                    lblFldName.Text = cmbSearch.Text + " :";
                    string queryString = "";
                    if (cmbSearch.SelectedIndex==1)
                    {
                        queryString = "select  brrId from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearch.SelectedIndex == 2)
                    {
                        queryString = "select distinct brrName from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        queryString = "select distinct brrAddress from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        autoCollData.Clear();
                        autoCollData.Add("Male");
                        autoCollData.Add("Female");
                        autoCollData.Add("Others");
                        txtbSearch.AutoCompleteMode = AutoCompleteMode.Suggest;
                        txtbSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        txtbSearch.AutoCompleteCustomSource = autoCollData;
                    }
                    else if (cmbSearch.SelectedIndex == 5)
                    {
                        queryString = "select brrMailId from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearch.SelectedIndex == 6)
                    {
                        queryString = "select distinct brrContact from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else if (cmbSearch.SelectedIndex == 7)
                    {
                        queryString = "select distinct memPlan from borrowerDetails where brrCategory=@brrCategory";
                    }
                    else
                    {
                        List<string> filedNameList = fieldList.ToList();
                        if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                        {
                            queryString = "select distinct addInfo1 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                        {
                            queryString = "select distinct addInfo2 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                        {
                            queryString = "select distinct addInfo3 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                        {
                            queryString = "select distinct addInfo4 from borrowerDetails where brrCategory=@brrCategory";
                        }
                        else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            queryString = "select distinct addInfo5 from borrowerDetails where brrCategory=@brrCategory";
                        }
                    }
                    if (queryString != "")
                    {
                        if (globalVarLms.sqliteData)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                autoCollData.Clear();
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
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
                            queryString = queryString.Replace("borrowerDetails", "borrower_details");
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                autoCollData.Clear();
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
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
            else if (cmbType.Text == "Item")
            {
                txtbSearch.Clear();
                if (cmbSearch.SelectedIndex > 0)
                {
                    lblFldName.Text = cmbSearch.Text + " :";
                    string queryString = "";
                    if (cmbSearch.SelectedIndex==1)
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
                        if (globalVarLms.sqliteData)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                autoCollData.Clear();
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
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
                            queryString = queryString.Replace("itemDetails", "item_details").Replace("itemSubCategory", "item_subcategory");
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                            mysqlCmd.CommandTimeout = 99999;
                            MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                autoCollData.Clear();
                                List<string> idList = (from IDataRecord r in dataReader
                                                       select (string)r[0]).ToList();
                                autoCollData.AddRange(idList.ToArray());
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
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (cmbType.Text == "Member")
            {
                backgroundWorkerBorrower.WorkerSupportsCancellation = true;
                backgroundWorkerBorrower.RunWorkerAsync();
            }
            else if (cmbType.Text == "Item")
            {
                backgroundWorkerItems.WorkerSupportsCancellation = true;
                backgroundWorkerItems.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please select which data you want to search.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0;
            dgvDetails.Rows.Clear();
        }

        private void pnlField_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Solid);
        }

        private void dgvDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (cmbType.Text == "Item")
            {
                if (e.ColumnIndex == dgvDetails.Columns["view_details"].Index)
                {
                    timer1.Start();
                    viewDetails.ShowDialog();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (globalVarLms.sqliteData)
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
                        if (dataReader["capInfo"].ToString() != "")
                        {
                            string[] captionList = dataReader["capInfo"].ToString().Split('$');
                            for (int i = 0; i < captionList.Length; i++)
                            {
                                if (i == 0)
                                {
                                    viewDetails.dgvItemDetails.Rows[9].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 1)
                                {
                                    viewDetails.dgvItemDetails.Rows[10].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 2)
                                {
                                    viewDetails.dgvItemDetails.Rows[11].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 3)
                                {
                                    viewDetails.dgvItemDetails.Rows[12].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 4)
                                {
                                    viewDetails.dgvItemDetails.Rows[13].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 5)
                                {
                                    viewDetails.dgvItemDetails.Rows[14].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 6)
                                {
                                    viewDetails.dgvItemDetails.Rows[15].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 7)
                                {
                                    viewDetails.dgvItemDetails.Rows[16].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                            }
                        }
                    }
                }
                dataReader.Close();

                queryString = "select * from itemDetails where itemAccession=@itemAccession";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvDetails.SelectedRows[0].Cells[2].Value.ToString());
                viewDetails.dgvItemDetails.ClearSelection();
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        viewDetails.dgvItemDetails.Rows[0].Cells[1].Value = dataReader["itemAccession"].ToString();
                        viewDetails.dgvItemDetails.Rows[1].Cells[1].Value = dataReader["itemTitle"].ToString();
                        viewDetails.dgvItemDetails.Rows[2].Cells[1].Value = dataReader["itemAuthor"].ToString();
                        viewDetails.dgvItemDetails.Rows[3].Cells[1].Value = dataReader["itemIsbn"].ToString();
                        viewDetails.dgvItemDetails.Rows[4].Cells[1].Value = dataReader["itemClassification"].ToString();
                        viewDetails.dgvItemDetails.Rows[5].Cells[1].Value = dataReader["itemSubject"].ToString();
                        viewDetails.dgvItemDetails.Rows[6].Cells[1].Value = dataReader["rackNo"].ToString();
                        viewDetails.dgvItemDetails.Rows[7].Cells[1].Value = dataReader["totalPages"].ToString();
                        viewDetails.dgvItemDetails.Rows[8].Cells[1].Value = dataReader["itemPrice"].ToString();
                        viewDetails.dgvItemDetails.Rows[9].Cells[1].Value = dataReader["addInfo1"].ToString();
                        viewDetails.dgvItemDetails.Rows[10].Cells[1].Value = dataReader["addInfo2"].ToString();
                        viewDetails.dgvItemDetails.Rows[11].Cells[1].Value = dataReader["addInfo3"].ToString();
                        viewDetails.dgvItemDetails.Rows[12].Cells[1].Value = dataReader["addInfo4"].ToString();
                        viewDetails.dgvItemDetails.Rows[13].Cells[1].Value = dataReader["addInfo5"].ToString();
                        viewDetails.dgvItemDetails.Rows[14].Cells[1].Value = dataReader["addInfo6"].ToString();
                        viewDetails.dgvItemDetails.Rows[15].Cells[1].Value = dataReader["addInfo7"].ToString();
                        viewDetails.dgvItemDetails.Rows[16].Cells[1].Value = dataReader["addInfo8"].ToString();
                    }
                }
                dataReader.Close();
                if (dgvDetails.SelectedRows[0].DefaultCellStyle.BackColor == Color.Red)
                {
                    string brrId = "";
                    sqltCommnd.CommandText = "select expectedReturnDate,brrId from issuedItem where itemAccession=@itemAccession and itemReturned='" + false + "'";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvDetails.SelectedRows[0].Cells[2].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            viewDetails.dgvItemDetails.Rows.Add("Expected Date :", dataReader["expectedReturnDate"].ToString());
                            brrId = dataReader["brrId"].ToString();
                        }
                    }
                    dataReader.Close();

                    sqltCommnd.CommandText = "select brrName from borrowerDetails where brrId=@brrId";
                    sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            viewDetails.dgvItemDetails.Rows[6].Cells[1].Value = dataReader["brrName"].ToString() + "(" + brrId + ")";
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
                string queryString = "select capInfo from item_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", cmbCategory.Text);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["capInfo"].ToString() != "")
                        {
                            string[] captionList = dataReader["capInfo"].ToString().Split('$');
                            for (int i = 0; i < captionList.Length; i++)
                            {
                                if (i == 0)
                                {
                                    viewDetails.dgvItemDetails.Rows[9].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 1)
                                {
                                    viewDetails.dgvItemDetails.Rows[10].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 2)
                                {
                                    viewDetails.dgvItemDetails.Rows[11].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 3)
                                {
                                    viewDetails.dgvItemDetails.Rows[12].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 4)
                                {
                                    viewDetails.dgvItemDetails.Rows[13].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 5)
                                {
                                    viewDetails.dgvItemDetails.Rows[14].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 6)
                                {
                                    viewDetails.dgvItemDetails.Rows[15].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                                else if (i == 7)
                                {
                                    viewDetails.dgvItemDetails.Rows[16].Cells[0].Value = captionList[i].Substring(captionList[i].IndexOf("_") + 1) + " :";
                                }
                            }
                        }
                    }
                }
                dataReader.Close();
                Application.DoEvents();

                queryString = "select * from item_details where itemAccession=@itemAccession";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvDetails.SelectedRows[0].Cells[2].Value.ToString());
                mysqlCmd.CommandTimeout = 99999;
                viewDetails.dgvItemDetails.ClearSelection();
                dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        viewDetails.dgvItemDetails.Rows[0].Cells[1].Value = dataReader["itemAccession"].ToString();
                        viewDetails.dgvItemDetails.Rows[1].Cells[1].Value = dataReader["itemTitle"].ToString();
                        viewDetails.dgvItemDetails.Rows[2].Cells[1].Value = dataReader["itemAuthor"].ToString();
                        viewDetails.dgvItemDetails.Rows[3].Cells[1].Value = dataReader["itemIsbn"].ToString();
                        viewDetails.dgvItemDetails.Rows[4].Cells[1].Value = dataReader["itemClassification"].ToString();
                        viewDetails.dgvItemDetails.Rows[5].Cells[1].Value = dataReader["itemSubject"].ToString();
                        viewDetails.dgvItemDetails.Rows[6].Cells[1].Value = dataReader["rackNo"].ToString();
                        viewDetails.dgvItemDetails.Rows[7].Cells[1].Value = dataReader["totalPages"].ToString();
                        viewDetails.dgvItemDetails.Rows[8].Cells[1].Value = dataReader["itemPrice"].ToString();
                        viewDetails.dgvItemDetails.Rows[9].Cells[1].Value = dataReader["addInfo1"].ToString();
                        viewDetails.dgvItemDetails.Rows[10].Cells[1].Value = dataReader["addInfo2"].ToString();
                        viewDetails.dgvItemDetails.Rows[11].Cells[1].Value = dataReader["addInfo3"].ToString();
                        viewDetails.dgvItemDetails.Rows[12].Cells[1].Value = dataReader["addInfo4"].ToString();
                        viewDetails.dgvItemDetails.Rows[13].Cells[1].Value = dataReader["addInfo5"].ToString();
                        viewDetails.dgvItemDetails.Rows[14].Cells[1].Value = dataReader["addInfo6"].ToString();
                        viewDetails.dgvItemDetails.Rows[15].Cells[1].Value = dataReader["addInfo7"].ToString();
                        viewDetails.dgvItemDetails.Rows[16].Cells[1].Value = dataReader["addInfo8"].ToString();
                    }
                }
                dataReader.Close();
                Application.DoEvents();

                if (dgvDetails.SelectedRows[0].DefaultCellStyle.BackColor == Color.Red)
                {
                    string brrId = "";
                    queryString = "select expectedReturnDate,brrId from issued_item where itemAccession=@itemAccession and itemReturned='" + false + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvDetails.SelectedRows[0].Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            viewDetails.dgvItemDetails.Rows.Add("Expected Date :", dataReader["expectedReturnDate"].ToString());
                            brrId = dataReader["brrId"].ToString();
                        }
                    }
                    dataReader.Close();
                    Application.DoEvents();
                    queryString = "select brrName from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            viewDetails.dgvItemDetails.Rows[6].Cells[1].Value = dataReader["brrName"].ToString() + "(" + brrId + ")";
                        }
                    }
                    dataReader.Close();
                }
                mysqlConn.Close();
            }
            Application.DoEvents();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                lblScanning.Text = "Please Wait...While Fetching Data...";
                panelStatus.Visible = true;
                Application.DoEvents();
                dgvDetails.Rows.Clear();
                if (txtbSearch.Text != "")
                {
                    string conditionString = "";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemAccession=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemAccession like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 2)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemSubCat=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemSubCat like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemIsbn=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemIsbn like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemTitle=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemTitle like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 5)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemSubject=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemSubject like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 6)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemAuthor=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemAuthor like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 7)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemClassification=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemClassification like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 8)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "rackNo=@fieldName";
                        }
                        else
                        {
                            conditionString = "rackNo like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 9)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "totalPages=@fieldName";
                        }
                        else
                        {
                            conditionString = "totalPages like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 10)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "itemPrice=@fieldName";
                        }
                        else
                        {
                            conditionString = "itemPrice like @fieldName";
                        }
                    }
                    else
                    {
                        List<string> filedNameList = fieldList.ToList();
                        if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo1=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo1 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo2=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo2 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo3=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo3 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo4=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo4 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo5=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo5 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("6_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo6=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo6 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("7_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo7=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo7 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("8_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo8=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo8 like @fieldName";
                            }
                        }
                    }
                    if (conditionString != "")
                    {
                        if (globalVarLms.sqliteData)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                            string queryString = "select * from itemDetails where " + conditionString + " and itemCat=@itemCat and isLost='" + false + "' and isDamage='" + false + "' collate nocase";
                            sqltCommnd.CommandText = queryString;
                            if (rdbFull.Checked)
                            {
                                sqltCommnd.Parameters.AddWithValue("@fieldName", txtbSearch.Text);
                            }
                            else
                            {
                                sqltCommnd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                            }
                            sqltCommnd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (breakOperation)
                                    {
                                        break;
                                    }
                                    try
                                    {
                                        if(dataReader["imagePath"].ToString()!="base64String")
                                        {
                                            pcbBook.Image = Image.FromFile(Properties.Settings.Default.databasePath+ @"\CatlogImage\" + dataReader["imagePath"].ToString());
                                        }
                                        else
                                        {
                                            pcbBook.Image = Properties.Resources.NoImageAvailable;
                                        }
                                    }
                                    catch
                                    {
                                        pcbBook.Image = Properties.Resources.NoImageAvailable;
                                    }
                                    dgvDetails.Rows.Add(dgvDetails.Rows.Count + 1, pcbBook.Image, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(), "View Details");
                                    if (dataReader["itemAvailable"].ToString() != "True")
                                    {
                                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                    Application.DoEvents();
                                }
                            }
                            dataReader.Close();
                            sqltConn.Close();
                        }
                        else
                        {
                            string columnName = "";
                            if (rdbFull.Checked)
                            {
                                columnName = conditionString.Substring(0, conditionString.IndexOf("="));
                            }
                            else
                            {
                                columnName = conditionString.Substring(0, conditionString.IndexOf(" "));
                            }
                            conditionString = conditionString.Replace(columnName, "lower(" + columnName + ")");
                            try
                            {
                                MySqlConnection mysqlConn;
                                mysqlConn = ConnectionClass.mysqlConnection();
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                MySqlCommand mysqlCmd;
                                string queryString = "select * from item_details where " + conditionString + " and itemCat=@itemCat and isLost='" + false + "' and isDamage='" + false + "'";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                if (rdbFull.Checked)
                                {
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbSearch.Text.ToLower());
                                }
                                else
                                {
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text.ToLower() + "%");
                                }
                                mysqlCmd.Parameters.AddWithValue("@itemCat", cmbCategory.Text);
                                mysqlCmd.CommandTimeout = 99999;
                                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
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
                                        dgvDetails.Rows.Add(dgvDetails.Rows.Count + 1, pcbBook.Image, dataReader["itemAccession"].ToString(), dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(), "View Details");
                                        if (dataReader["itemAvailable"].ToString() != "True")
                                        {
                                            dgvDetails.Rows[dgvDetails.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                        }
                                        Application.DoEvents();
                                    }
                                }
                                dataReader.Close();
                                mysqlConn.Close();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        dgvDetails.ClearSelection();
                    }
                }
                panelStatus.Visible = false;
                Application.DoEvents();
            }));
        }

        private void FormSearchLookup_FormClosing(object sender, FormClosingEventArgs e)
        {
            breakOperation = true;
        }

        private void backgroundWorkerBorrower_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (dgvDetails.Columns.Contains("Image"))
                {
                    dgvDetails.Columns.Remove("Image");
                }
                dgvDetails.Rows.Clear();
                if (txtbSearch.Text != "")
                {
                    string conditionString = "";
                    if (cmbSearch.SelectedIndex == 1)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrId=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrId like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 2)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrName=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrName like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 3)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrAddress=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrAddress like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 4)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrGender=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrGender like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 5)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrMailId=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrMailId like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 6)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "brrContact=@fieldName";
                        }
                        else
                        {
                            conditionString = "brrContact like @fieldName";
                        }
                    }
                    else if (cmbSearch.SelectedIndex == 7)
                    {
                        if (rdbFull.Checked)
                        {
                            conditionString = "memPlan=@fieldName";
                        }
                        else
                        {
                            conditionString = "memPlan like @fieldName";
                        }
                    }
                    else
                    {
                        List<string> filedNameList = fieldList.ToList();
                        if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo1=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo1 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo2=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo2 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo3=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo3 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo4=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo4 like @fieldName";
                            }
                        }
                        else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                        {
                            if (rdbFull.Checked)
                            {
                                conditionString = "addInfo5=@fieldName";
                            }
                            else
                            {
                                conditionString = "addInfo5 like @fieldName";
                            }
                        }
                    }
                    if (conditionString != "")
                    {
                        if (globalVarLms.sqliteData)
                        {
                            SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                            if (sqltConn.State == ConnectionState.Closed)
                            {
                                sqltConn.Open();
                            }
                            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                            string queryString = "select  " + string.Join(",", selctedColumnsList) + " from borrowerDetails where " + conditionString + " and brrCategory=@brrCategory collate nocase";
                            //string queryString = "select  brrId,brrName,brrGender,brrMailId,brrContact,brrAddress from borrowerDetails where " + conditionString + " collate nocase";
                            sqltCommnd.CommandText = queryString;
                            if (rdbFull.Checked)
                            {
                                sqltCommnd.Parameters.AddWithValue("@fieldName", txtbSearch.Text);
                            }
                            else
                            {
                                sqltCommnd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                            }
                            sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                DataTable dataTable = new DataTable();
                                dataTable.Load(dataReader);
                                dataReader.Close();
                                string combineString = "";
                                string[] columnValues;

                                DataGridViewImageColumn dgvColumn = new DataGridViewImageColumn();
                                dgvColumn.HeaderText = "Image";
                                dgvColumn.Name = "Image";
                                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                dgvColumn.Width = 75;
                                dgvColumn.Visible = false;
                                dgvColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                                dgvDetails.Columns.Add(dgvColumn);

                                foreach (DataRow dataRow in dataTable.Rows)
                                {
                                    sqltCommnd = sqltConn.CreateCommand();
                                    sqltCommnd.CommandText = "select imagePath from borrowerDetails where brrId=@brrId";
                                    sqltCommnd.Parameters.AddWithValue("@brrId", dataRow["brrId"].ToString());
                                    dataReader = sqltCommnd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            try
                                            {
                                                if (dataReader["imagePath"].ToString()!="base64String")
                                                {
                                                    //
                                                    pcbBook.Image = Image.FromFile(Properties.Settings.Default.databasePath + @"\BorrowerImage\" + dataReader["imagePath"].ToString());
                                                }
                                                else
                                                {
                                                    pcbBook.Image = Properties.Resources.blankBrrImage;
                                                }
                                            }
                                            catch
                                            {
                                                pcbBook.Image = Properties.Resources.blankBrrImage;
                                            }
                                        }
                                    }
                                    dataReader.Close();
                                    combineString = dgvDetails.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                    columnValues = combineString.Split('^');
                                    dgvDetails.Rows.Add(columnValues);
                                    dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[dgvDetails.Columns.Count - 1].Value = pcbBook.Image;
                                    Application.DoEvents();
                                }
                                dgvDetails.ClearSelection();
                                dgvDetails.Columns["Image"].DisplayIndex = 1;
                                dgvDetails.Columns["Image"].Visible = true;
                            }
                            sqltConn.Close();
                        }
                        else
                        {
                            string columnName = "";
                            if (rdbFull.Checked)
                            {
                                columnName = conditionString.Substring(0, conditionString.IndexOf("="));
                            }
                            else
                            {
                                columnName = conditionString.Substring(0, conditionString.IndexOf(" "));
                            }
                            conditionString = conditionString.Replace(columnName, "lower(" + columnName + ")");
                            try
                            {
                                MySqlConnection mysqlConn;
                                mysqlConn = ConnectionClass.mysqlConnection();
                                if (mysqlConn.State == ConnectionState.Closed)
                                {
                                    mysqlConn.Open();
                                }
                                MySqlCommand mysqlCmd;
                                string queryString = "select  " + string.Join(",", selctedColumnsList) + " from borrower_details where " + conditionString + " and brrCategory=@brrCategory";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                if (rdbFull.Checked)
                                {
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", txtbSearch.Text.ToLower());
                                }
                                else
                                {
                                    mysqlCmd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text.ToLower() + "%");
                                }
                                mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbCategory.Text);
                                mysqlCmd.CommandTimeout = 99999;
                                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    DataTable dataTable = new DataTable();
                                    dataTable.Load(dataReader);
                                    dataReader.Close();
                                    string combineString = "";
                                    string[] columnValues;

                                    DataGridViewImageColumn dgvColumn = new DataGridViewImageColumn();
                                    dgvColumn.HeaderText = "Image";
                                    dgvColumn.Name = "Image";
                                    dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                                    dgvColumn.Width = 75;
                                    dgvColumn.Visible = false;
                                    dgvColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                                    dgvDetails.Columns.Add(dgvColumn);

                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        if (breakOperation)
                                        {
                                            break;
                                        }
                                        queryString = "select brrImage from borrower_details where brrId=@brrId";
                                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                        mysqlCmd.Parameters.AddWithValue("@brrId", dataRow["brrId"].ToString());
                                        mysqlCmd.CommandTimeout = 99999;
                                        dataReader = mysqlCmd.ExecuteReader();
                                        if (dataReader.HasRows)
                                        {
                                            while (dataReader.Read())
                                            {
                                                try
                                                {
                                                    byte[] imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                                    pcbBook.Image = Image.FromStream(memoryStream, true);
                                                }
                                                catch
                                                {
                                                    pcbBook.Image = Properties.Resources.blankBrrImage;
                                                }
                                            }
                                        }
                                        dataReader.Close();
                                        combineString = dgvDetails.Rows.Count + 1 + "^" + string.Join("^", dataRow.ItemArray.Cast<string>().ToArray().ToArray());
                                        columnValues = combineString.Split('^');
                                        dgvDetails.Rows.Add(columnValues);
                                        dgvDetails.Rows[dgvDetails.Rows.Count - 1].Cells[dgvDetails.Columns.Count - 1].Value = pcbBook.Image;
                                        Application.DoEvents();
                                    }
                                    dgvDetails.ClearSelection();
                                    dgvDetails.Columns["Image"].DisplayIndex = 1;
                                    dgvDetails.Columns["Image"].Visible = true;
                                }
                                else
                                {
                                    dataReader.Close();
                                }
                                mysqlConn.Close();
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        dgvDetails.ClearSelection();
                    }
                }
            }));
        }
    }
}
