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
    public partial class FormMemberLookup : Form
    {
        public FormMemberLookup()
        {
            InitializeComponent();
        }

        string fieldCaption = "";
        string[] fieldList = null;
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormMemberLookup_Load(object sender, EventArgs e)
        {
            dgvBrrDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvBrrDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            cmbBrrCategory.Items.Clear();
            cmbBrrCategory.Items.Add("---------select----------");
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select catName from borrowerSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
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
                    string queryString = "select catName from borrower_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            cmbBrrCategory.Items.Add(dataReader["catName"].ToString());
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
            cmbBrrCategory.SelectedIndex = 0;
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
                            dgvBrrDetails.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrName")
                        {
                            dgvBrrDetails.Columns[3].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblMailId")
                        {
                            dgvBrrDetails.Columns[5].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblContact")
                        {
                            dgvBrrDetails.Columns[6].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrAddress")
                        {
                            dgvBrrDetails.Columns[7].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblBrCategory")
                        {
                            lblBrCategory.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        if(cmbSearch.Items.Count>0)
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

        private void FormMemberLookup_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void cmbBrrCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvBrrDetails.DataSource = null;
            dgvBrrDetails.Refresh();
            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("Please choose a field name...");
            if (cmbBrrCategory.SelectedIndex>0)
            {
                cmbSearch.Items.Add("Borrower Id");
                cmbSearch.Items.Add("Name");
                cmbSearch.Items.Add("Address");
                cmbSearch.Items.Add("Gender");
                cmbSearch.Items.Add("Email Id");
                cmbSearch.Items.Add("Contact");
                cmbSearch.Items.Add("Membership Duration");

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
                    sqltCommnd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
                    mysqlCmd.Parameters.AddWithValue("@catName", cmbBrrCategory.Text);
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
                loadFieldValue();
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
                else if (cmbSearch.Text == "Gender")
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
                    queryString = "select distinct mbershipDuration from borrowerDetails where brrCategory=@brrCategory";
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
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
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
                        queryString = queryString.Replace("borrowerDetails","borrower_details");
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
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

        private void dgvBrrDetails_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvBrrDetails.HitTest(e.X, e.Y);
                dgvBrrDetails.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    if (!globalVarLms.mailSending)
                    {
                        dgvBrrDetails.Rows[hti.RowIndex].Selected = true;
                        contextMenuStrip1.Show(dgvBrrDetails, new Point(e.X, e.Y));
                    }
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            globalVarLms.brrId = dgvBrrDetails.SelectedRows[0].Cells[2].Value.ToString();
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(dgvBrrDetails.Rows.Count==0)
            {
                MessageBox.Show("Please add some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvBrrDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select some borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string combineString = "";
            foreach(DataGridViewRow dgvRow in dgvBrrDetails.SelectedRows)
            {
                combineString = dgvRow.Cells[3].Value.ToString();
                combineString = combineString+"$"+ dgvRow.Cells[5].Value.ToString();
                combineString = combineString + "$" + dgvRow.Cells[6].Value.ToString();
                globalVarLms.memberList.Add(combineString);
            }
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvBrrDetails.Rows.Clear();
            if (txtbSearch.Text != "")
            {
                string conditionString = "";
                if (cmbSearch.SelectedIndex == 1)
                {
                    conditionString = "brrId like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 2)
                {
                    conditionString = "brrName like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 3)
                {
                    conditionString = "brrAddress like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 4)
                {
                    conditionString = "brrGender like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 5)
                {
                    conditionString = "brrMailId like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 6)
                {
                    conditionString = "brrContact like @fieldName and brrCategory=@brrCategory";
                }
                else if (cmbSearch.SelectedIndex == 7)
                {
                    conditionString = "mbershipDuration like @fieldName and brrCategory=@brrCategory";
                }
                else
                {
                    List<string> filedNameList = fieldList.ToList();
                    if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                    {
                        conditionString = "addInfo1 like @fieldName and brrCategory=@brrCategory";
                    }
                    else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                    {
                        conditionString = "addInfo2 like @fieldName and brrCategory=@brrCategory";
                    }
                    else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                    {
                        conditionString = "addInfo3 like @fieldName and brrCategory=@brrCategory";
                    }
                    else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                    {
                        conditionString = "addInfo4 like @fieldName and brrCategory=@brrCategory";
                    }
                    else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        conditionString = "addInfo5 like @fieldName and brrCategory=@brrCategory";
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
                        string queryString = "select brrId,brrName,brrGender,brrMailId,brrContact,brrAddress,imagePath from borrowerDetails where " + conditionString + " collate nocase";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                        sqltCommnd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    if(dataReader["imagePath"].ToString()!="base64String")
                                    {
                                        pcbBook.Image = System.Drawing.Image.FromFile(Properties.Settings.Default.databasePath + @"\BorrowerImage\" + dataReader["imagePath"].ToString());
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

                                dgvBrrDetails.Rows.Add(dgvBrrDetails.Rows.Count + 1, pcbBook.Image, dataReader["brrId"].ToString(),
                                    dataReader["brrName"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrMailId"].ToString(),
                                    dataReader["brrContact"].ToString(), dataReader["brrAddress"].ToString());
                                Application.DoEvents();
                            }
                            dgvBrrDetails.ClearSelection();
                        }
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
                        string queryString = "select brrId,brrName,brrGender,brrMailId,brrContact,brrAddress,brrImage from borrower_details where " + conditionString + "";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text.ToLower() + "%") ;
                        mysqlCmd.Parameters.AddWithValue("@brrCategory", cmbBrrCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBook.Image = System.Drawing.Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBook.Image = Properties.Resources.NoImageAvailable;
                                }

                                dgvBrrDetails.Rows.Add(dgvBrrDetails.Rows.Count + 1, pcbBook.Image, dataReader["brrId"].ToString(),
                                    dataReader["brrName"].ToString(), dataReader["brrGender"].ToString(), dataReader["brrMailId"].ToString(),
                                    dataReader["brrContact"].ToString(), dataReader["brrAddress"].ToString());
                                Application.DoEvents();
                            }
                            dgvBrrDetails.ClearSelection();
                        }
                        mysqlConn.Close();
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbBrrCategory.SelectedIndex = 0;
            dgvBrrDetails.Rows.Clear();
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
    }
}
