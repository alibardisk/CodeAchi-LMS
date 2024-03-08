using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_LMS_Search
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public int reserveLimit = 0, maxCheckin=0,ttlIssue=0;
        string[] fieldList = null;
        DateTime currentDate = DateTime.Now.Date;
        public string databasePath = "",hostIp="", connectionString="", fieldCaption = "",
            queryString = "";
        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();
        List<string> catgoryAccessList = new List<string> { };
        List<string> fieldValueList = new List<string> { };
        bool logIn = false;

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            lblCompany1.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            lblCompany.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            MenuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColorTable());
            timer1.Start();
            lstbFilterData.Visible = false;
            panelStatus.Visible = false;
            backgroundWorker1.RunWorkerAsync();

            dgvItemDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvItemDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            settingToolStripMenuItem.Enabled = false;
            viewHistoryToolStripMenuItem.Enabled = false;
            changePasswordToolStripMenuItem.Enabled = false;
            logOutToolStripMenuItem.Enabled = false;
            contextMenuStrip1.Enabled = false;
        }

        const int WM_PARENTNOTIFY = 0x210;
        const int WM_LBUTTONDOWN = 0x201;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN || (m.Msg == WM_PARENTNOTIFY &&
                (int)m.WParam == WM_LBUTTONDOWN))
                if (!lstbFilterData.ClientRectangle.Contains(
                                 lstbFilterData.PointToClient(Cursor.Position)))
                    lstbFilterData.Visible=false;
            base.WndProc(ref m);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = currentDate.Day.ToString("00") + "-" + currentDate.Month.ToString("00") + "-" + currentDate.Year +
                " " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSetting databaseSetting = new FormSetting();
            databaseSetting.ShowDialog();
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                object o1 = regKey.GetValue("Data2");
                if(o1!=null)
                {
                    byteData = Convert.FromBase64String(o1.ToString());
                    databasePath = Encoding.UTF8.GetString(byteData);
                    o1 = regKey.GetValue("Data2");
                    if (o1 != null)
                    {
                        byteData = Convert.FromBase64String(o1.ToString());
                        databasePath = Encoding.UTF8.GetString(byteData);
                        if (Properties.Settings.Default.sqliteDatabase)
                        {
                            o1 = regKey.GetValue("Data3");
                            if (o1 != null)
                            {
                                byteData = Convert.FromBase64String(o1.ToString());
                                hostIp = Encoding.UTF8.GetString(byteData);
                                if (hostIp != "Local")
                                {
                                    var hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                                    databasePath = databasePath.Replace(hostName, hostIp).Replace("\\", "/");
                                }
                            }
                            connectionString = @"Data Source=" + databasePath + "/LMS.sl3;Version=3;Password=codeachi@lmssl;";
                        }
                        else
                        {
                            connectionString = databasePath;
                        }
                    }
                }

            }
        }

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtbSearch.Clear();
            queryString = "";
            if (cmbSearch.SelectedIndex>0)
            {
                Cursor = Cursors.WaitCursor;
                if(cmbSearch.Text.Length>14)
                {
                    lblFldName.Text = cmbSearch.Text.Substring(0, 14) + Environment.NewLine + cmbSearch.Text.Substring(14, (cmbSearch.Text.Length -14)) + " :";
                }
                else
                {
                    lblFldName.Text = cmbSearch.Text + " :";
                }
                
                if (cmbSearch.Text == "Accession No")
                {
                    //queryString = "select itemAccession from itemDetails where itemCat=@catName and itemAccession like @fieldValue collate nocase";
                    queryString = "select itemAccession from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Subcategory")
                {
                    //queryString = "select itemSubCat from itemSubCategory where catName=@catName and subCatName like @fieldValue collate nocase";
                    queryString = "select distinct itemSubCat from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "ISBN/ISSN")
                {
                    //queryString = "select distinct itemIsbn from itemDetails where itemCat=@catName and itemIsbn like @fieldValue collate nocase";
                    queryString = "select distinct itemIsbn from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Title")
                {
                    //queryString = "select distinct itemTitle from itemDetails where itemCat=@catName and itemTitle like @fieldValue collate nocase";
                    queryString = "select distinct itemTitle from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Subject")
                {
                    //queryString = "select distinct itemSubject from itemDetails where itemCat=@catName and itemSubject like @fieldValue collate nocase";
                    queryString = "select distinct itemSubject from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Author")
                {
                    //queryString = "select distinct itemAuthor from itemDetails where itemCat=@catName and itemAuthor like @fieldValue collate nocase";
                    queryString = "select distinct itemAuthor from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Classification No")
                {
                    //queryString = "select distinct itemClassification from itemDetails where itemCat=@catName and itemClassification like @fieldValue collate nocase";
                    queryString = "select distinct itemClassification from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Location")
                {
                    //queryString = "select distinct rackNo from itemDetails where itemCat=@catName and rackNo like @fieldValue collate nocase";
                    queryString = "select distinct rackNo from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "No of Pages")
                {
                    //queryString = "select distinct totalPages from itemDetails where itemCat=@catName and totalPages like @fieldValue collate nocase";
                    queryString = "select distinct totalPages from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Price")
                {
                    //queryString = "select distinct itemPrice from itemDetails where itemCat=@catName and itemPrice like @fieldValue collate nocase";
                    queryString = "select distinct itemPrice from itemDetails where itemCat=@catName collate nocase";
                }
                else if (cmbSearch.Text == "Price")
                {
                    //queryString = "select distinct itemPrice from itemDetails where itemCat=@catName and itemPrice like @fieldValue collate nocase";
                    queryString = "select distinct itemPrice from itemDetails where itemCat=@catName collate nocase";
                }
                else
                {
                    List<string> filedNameList = fieldList.ToList();
                    if (filedNameList.IndexOf("1_" + cmbSearch.Text) != -1)
                    {
                        //queryString = "select distinct addInfo1 from itemDetails where itemCat=@catName and addInfo1 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo1 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("2_" + cmbSearch.Text) != -1)
                    {
                        //queryString = "select distinct addInfo2 from itemDetails where itemCat=@catName and addInfo2 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo2 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("3_" + cmbSearch.Text) != -1)
                    {
                        //queryString = "select distinct addInfo3 from itemDetails where itemCat=@catName and addInfo3 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo3 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("4_" + cmbSearch.Text) != -1)
                    {
                        //queryString = "select distinct addInfo4 from itemDetails where itemCat=@catName and addInfo4 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo4 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("5_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        //queryString = "select distinct addInfo5 from itemDetails where itemCat=@catName and addInfo5 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo5 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("6_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        //queryString = "select distinct addInfo6 from itemDetails where itemCat=@catName and addInfo6 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo6 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("7_" + cmbSearch.SelectedItem.ToString()) != -1)
                    {
                        //queryString = "select distinct addInfo7 from itemDetails where itemCat=@catName and addInfo7 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo7 from itemDetails where itemCat=@catName collate nocase";
                    }
                    else if (filedNameList.IndexOf("8_" + cmbSearch.SelectedItem) != -1)
                    {
                        //queryString = "select distinct addInfo8 from itemDetails where itemCat=@catName and addInfo8 like @fieldValue collate nocase";
                        queryString = "select distinct addInfo8 from itemDetails where itemCat=@catName collate nocase";
                    }
                }
                
                if (queryString != "")
                {
                    if (Properties.Settings.Default.sqliteDatabase)
                    {
                        //===============================================================
                        SQLiteConnection sqltConn = new SQLiteConnection(connectionString);

                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                        //sqltCommnd.Parameters.AddWithValue("@fieldValue", "%" + txtbSearch.Text + "%");
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        lstbFilterData.Items.Clear();
                        fieldValueList.Clear();
                        if (dataReader.HasRows)
                        {
                            fieldValueList = (from IDataRecord r in dataReader
                                              select (string)r[0]
                                ).ToList();
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
                        mysqlCmd = new MySqlCommand(queryString.Replace(" collate nocase","").Replace("itemDetails", "item_details"), mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        lstbFilterData.Items.Clear();
                        fieldValueList.Clear();
                        if (dataReader.HasRows)
                        {
                            fieldValueList = (from IDataRecord r in dataReader
                                              select (string)r[0]
                                ).ToList();
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                    Cursor = Cursors.Default;
                    //=================================================================
                    txtbSearch.Enabled = true;
                    txtbSearch.Select();
                }
                Cursor = Cursors.Default;
            }
            else
            {
                txtbSearch.Clear();
                txtbSearch.Enabled = false;
            }
            Cursor = Cursors.Default;
        }

        private void lstbFilterData_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                txtbSearch.Text = lstbFilterData.SelectedItem.ToString();
                lstbFilterData.Visible = false;
            }
        }

        private void lstbFilterData_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.Location.X,e.Location.Y);
            int a = lstbFilterData.IndexFromPoint(pt);
            if(a>-1)
            {
                lstbFilterData.SelectedIndex = a;
            }
        }

        private void txtbSearch_Enter(object sender, EventArgs e)
        {
            txtbSearch.SelectAll();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            lblScanning.Text = "Please Wait...While Fetching Data...";
            panelStatus.Visible = true;
            dgvItemDetails.Rows.Clear();
            if (txtbSearch.Text != "")
            {
                string conditionString = "";
                if (cmbSearch.Text == "Accession No")
                {
                    conditionString = "itemAccession like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Subcategory")
                {
                    conditionString = "itemSubCat like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "ISBN/ISSN")
                {
                    conditionString = "itemIsbn like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Title")
                {
                    conditionString = "itemTitle like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Subject")
                {
                    conditionString = "itemSubject like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Author")
                {
                    conditionString = "itemAuthor like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Classification No")
                {
                    conditionString = "itemClassification like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Rack No./Location")
                {
                    conditionString = "rackNo like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "No of Pages")
                {
                    conditionString = "totalPages like @fieldName and itemCat=@itemCat";
                }
                else if (cmbSearch.Text == "Price")
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
                        SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                        if (sqltConn.State == ConnectionState.Closed)
                        {
                            sqltConn.Open();
                        }
                        SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                        string queryString = "select itemAccession,itemTitle,itemAuthor,rackNo,itemAvailable,digitalReference,itemImage " +
                                            "from itemDetails where " + conditionString + " and isLost='" + false + "' and isDamage='" + false + "' collate nocase";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                        sqltCommnd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                        SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
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
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                        dataReader["itemAvailable"].ToString(), dataReader["rackNo"].ToString(), "", dataReader["digitalReference"].ToString());
                                Application.DoEvents();
                            }
                            dataReader.Close();
                            lblScanning.Text = "Please Wait...While Updating Data...";
                            Application.DoEvents();
                            IEnumerable<DataGridViewRow> dataRows = dgvItemDetails.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[5].Value.ToString().Equals("False"));
                            foreach (DataGridViewRow dataRow in dataRows)
                            {
                                sqltCommnd.CommandText = "select expectedReturnDate from issuedItem where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                sqltCommnd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                                dataReader = sqltCommnd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        dataRow.Cells[7].Value = dataReader["expectedReturnDate"].ToString();
                                    }
                                }
                                dataReader.Close();
                                dataRow.DefaultCellStyle.BackColor = Color.Red;
                                Application.DoEvents();
                            }
                            dgvItemDetails.ClearSelection();
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
                        string queryString = "select itemAccession,itemTitle,itemAuthor,rackNo,itemAvailable,digitalReference,itemImage " +
                                            "from item_details where " + conditionString + " and isLost='" + false + "' and isDamage='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@fieldName", "%" + txtbSearch.Text + "%");
                        mysqlCmd.Parameters.AddWithValue("@itemCat", cmbItemCategory.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
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
                                        dataReader["itemTitle"].ToString(), dataReader["itemAuthor"].ToString(),
                                        dataReader["itemAvailable"].ToString(), dataReader["rackNo"].ToString(), "", dataReader["digitalReference"].ToString());
                                Application.DoEvents();
                            }
                            dataReader.Close();
                            lblScanning.Text = "Please Wait...While Updating Data...";
                            Application.DoEvents();
                            IEnumerable<DataGridViewRow> dataRows = dgvItemDetails.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[5].Value.ToString().Equals("False"));
                            foreach (DataGridViewRow dataRow in dataRows)
                            {
                                queryString = "select expectedReturnDate from issued_item where itemAccession=@itemAccession and itemReturned='" + false + "'";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.Parameters.AddWithValue("@itemAccession", dataRow.Cells[2].Value.ToString());
                                mysqlCmd.CommandTimeout = 99999;
                                dataReader = mysqlCmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        dataRow.Cells[7].Value = dataReader["expectedReturnDate"].ToString();
                                    }
                                }
                                dataReader.Close();
                                dataRow.DefaultCellStyle.BackColor = Color.Red;
                                Application.DoEvents();
                            }
                            dgvItemDetails.ClearSelection();
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                    }
                }
            }
            Cursor = Cursors.Default;
            panelStatus.Visible = false;
        }

        private void dgvItemDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lblTtlRecord.Text = dgvItemDetails.Rows.Count.ToString();
        }

        private void dgvItemDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            lblTtlRecord.Text = dgvItemDetails.Rows.Count.ToString();
        }

        private void dgvItemDetails_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvItemDetails.SelectedRows.Count > 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(dgvItemDetails, new Point(e.X, e.Y));
                }
            }
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
                }
            }
        }

        private void reserveItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reserveLimit == 0)
            {
                MessageBox.Show("You have no permission to use this feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor != Color.Red)
            {
                MessageBox.Show("You can,t reserve an available item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItemDetails.ClearSelection();
                return;
            }
            else
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select itemCat,itemSubCat from itemDetails where itemAccession=@itemAccession";
                    sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    string catAccess = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            catAccess = dataReader["itemCat"].ToString() + "(" + dataReader["itemSubCat"].ToString() + ")";
                        }
                    }
                    dataReader.Close();
                    if (catgoryAccessList.IndexOf(catAccess) != -1)
                    {
                        string reserveDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        sqltCommnd.CommandText = "insert into reservationList (brrId,itemAccn,itemTitle,itemAuthor,reserveLocation,reserveDate,availableDate)" +
                            " values ('" + txtbBrrId.Text + "','',@itemTitle,@itemAuthor,'','" + reserveDate + "','')";
                        sqltCommnd.Parameters.AddWithValue("@itemTitle", dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("@itemAuthor", dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                        MessageBox.Show("Item reserved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string queryString = "select itemCat,itemSubCat from item_details where itemAccession=@itemAccession";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    string catAccess = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            catAccess = dataReader["itemCat"].ToString() + "(" + dataReader["itemSubCat"].ToString() + ")";
                        }
                    }
                    dataReader.Close();
                    if (catgoryAccessList.IndexOf(catAccess) != -1)
                    {
                        string reserveDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        queryString = "insert into reservation_list (brrId,itemAccn,itemTitle,itemAuthor,reserveLocation,reserveDate,availableDate)" +
                            " values ('" + txtbBrrId.Text + "','',@itemTitle,@itemAuthor,'','" + reserveDate + "','')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemTitle", dgvItemDetails.SelectedRows[0].Cells[3].Value.ToString());
                        mysqlCmd.Parameters.AddWithValue("@itemAuthor", dgvItemDetails.SelectedRows[0].Cells[4].Value.ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Item reserved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvItemDetails.Rows.Clear();
            if (txtbBrrId.Enabled == true)
            {
                txtbBrrId.Clear();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("Are you sure you want to close the application ? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    /* Cancel the Closing event from closing the form. */
                    e.Cancel = true;
                }
                else
                {
                    /* Closing the form. */
                    e.Cancel = false;
                    Application.Exit();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //FormLogin loginForm = new FormLogin();
            //loginForm.ShowDialog();
            //lblLoginTime.Text = "Login Time : " + DateTime.Now.ToString("HH:mm:ss tt");
            //lblDesignation.Text = loginForm.lblDesignation.Text;
            //lblUserId.Text = loginForm.txtbMailId.Text;
            //if(lblDesignation.Text=="Borrower")
            //{
            //    settingToolStripMenuItem.Enabled = false;
            //    txtbBrrId.Text = loginForm.txtbMailId.Text;
            //    txtbBrrId.Enabled = false;
            //}
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtbAddress.Clear();
            txtbBrrId.Clear();
            txtbName.Clear();
            txtbSearch.Clear();
            lblTtlRecord.Text = 0.ToString();
            dgvItemDetails.Rows.Clear();
            cmbItemCategory.Items.Clear();
            cmbSearch.Items.Clear();
            FormMain_Load(null, null);
        }

        private void viewHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHistory issueHistory = new FormHistory();
            issueHistory.connectionString = connectionString;
            issueHistory.txtbBrrId.Text = txtbBrrId.Text;
            issueHistory.txtbName.Text = txtbName.Text;
            issueHistory.lblDesignation.Text = lblDesignation.Text;
            issueHistory.txtbBrrId.AutoCompleteMode = AutoCompleteMode.Suggest;
            issueHistory.txtbBrrId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            issueHistory.txtbBrrId.AutoCompleteCustomSource = txtbBrrId.AutoCompleteCustomSource;
            if (lblDesignation.Text=="Borrower")
            {
                issueHistory.txtbBrrId.Enabled = false;
            }
            issueHistory.ShowDialog();
        }

        private void txtbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lstbFilterData.SelectedIndex == lstbFilterData.Items.Count - 1)
                {
                    lstbFilterData.SelectedIndex = 0;
                }
                else
                {
                    lstbFilterData.SelectedIndex = lstbFilterData.SelectedIndex + 1;
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (lstbFilterData.SelectedIndex == 0)
                {
                    lstbFilterData.SelectedIndex = lstbFilterData.Items.Count - 1;
                }
                else
                {
                    lstbFilterData.SelectedIndex = lstbFilterData.SelectedIndex - 1;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if(lstbFilterData.Visible)
                {
                    txtbSearch.Text = lstbFilterData.SelectedItem.ToString();
                    lstbFilterData.Visible = false;
                    txtbSearch.SelectionStart = txtbSearch.Text.Length;
                }
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChangePassword changePassword = new FormChangePassword();
            changePassword.connectionString = connectionString;
            changePassword.userName = lblUserId.Text;
            changePassword.ShowDialog();
        }

        private void logInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin loginForm = new FormLogin();
            loginForm.ShowDialog();
            logIn = loginForm.logIn;
            if (logIn)
            {
                lblLoginTime.Text = "Login Time : " + DateTime.Now.ToString("HH:mm:ss tt");
                lblDesignation.Text = loginForm.lblDesignation.Text;
                lblUserId.Text = loginForm.txtbMailId.Text;
                settingToolStripMenuItem.Enabled = true;
                viewHistoryToolStripMenuItem.Enabled = true;
                changePasswordToolStripMenuItem.Enabled = true;
                logOutToolStripMenuItem.Enabled = true;
                contextMenuStrip1.Enabled = true;
                if (lblDesignation.Text == "Borrower")
                {
                    settingToolStripMenuItem.Enabled = false;
                    txtbBrrId.Text = loginForm.txtbMailId.Text;
                    txtbBrrId.Enabled = false;
                }
            }
            else
            {

            }
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                     this.DisplayRectangle);
        }

        private void issueItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string itemCat = "", itemSubCat = "";
            bool referenceCopy = false;
            if (dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor == Color.Red)
            {
                MessageBox.Show("Item already issued.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvItemDetails.ClearSelection();
                Cursor = Cursors.Default;
                return;
            }
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
                sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
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
                            referenceCopy = Convert.ToBoolean(dataReader["notReference"].ToString());
                        }
                    }
                    dataReader.Close();

                    if (catgoryAccessList.IndexOf(itemCat + "(" + itemSubCat + ")") != -1)
                    {
                        if (!referenceCopy)
                        {
                            MessageBox.Show("This is a reference copy, you can't issue this item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            Cursor = Cursors.Default;
                            return;
                        }
                        if (ttlIssue == maxCheckin)
                        {
                            MessageBox.Show("You can't issue more than " + maxCheckin.ToString() + " items at a time for this borrower", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            Cursor = Cursors.Default;
                            sqltConn.Close();
                            return;
                        }
                        sqltCommnd.CommandText = "select itemAccn from reservationList where itemAccn=@itemAccn and brrId!='" + txtbBrrId.Text + "'";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            MessageBox.Show("This item reserved by someone.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            dataReader.Close();
                            Cursor = Cursors.Default;
                            return;
                        }
                        dataReader.Close();
                        string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        DateTime returnDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                        string strRtnDate = returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000");

                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "update itemDetails set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        sqltCommnd.ExecuteNonQuery();

                        sqltCommnd = sqltConn.CreateCommand();
                        queryString = "INSERT INTO issuedItem (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issuedBy) " +
                         " values (@brrId,@itemAccession" + ",'" + issueDate + "','" + strRtnDate + "'" + ",'" + false + "','Self')";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        sqltCommnd.ExecuteNonQuery();

                        sqltCommnd.CommandText = "delete from reservationList where itemAccn=@itemAccn";
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        sqltCommnd.ExecuteNonQuery();
                        dgvItemDetails.SelectedRows[0].Cells[5].Value = "False";
                        dgvItemDetails.SelectedRows[0].Cells[6].Value = txtbBrrId.Text;
                        dgvItemDetails.SelectedRows[0].Cells[7].Value = strRtnDate;
                        dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor = Color.Red;
                        ttlIssue++;
                        Cursor = Cursors.Default;
                        MessageBox.Show("Item issued successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
                        sqltConn.Close();
                    }
                    else
                    {
                        MessageBox.Show("You can't issue this item for this borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
                        sqltConn.Close();
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    numDay.Value = 1;
                }
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
                string queryString = "select itemCat,itemSubCat from item_details where itemAccession=@itemAccession";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
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
                            referenceCopy = Convert.ToBoolean(dataReader["notReference"].ToString());
                        }
                    }
                    dataReader.Close();

                    if (catgoryAccessList.IndexOf(itemCat + "(" + itemSubCat + ")") != -1)
                    {
                        if (!referenceCopy)
                        {
                            MessageBox.Show("This is a reference copy, you can't issue this item.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            Cursor = Cursors.Default;
                            return;
                        }
                        if (ttlIssue == maxCheckin)
                        {
                            MessageBox.Show("You can't issue more than " + maxCheckin.ToString() + " items at a time for this borrower", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            Cursor = Cursors.Default;
                            mysqlConn.Close();
                            return;
                        }
                        queryString = "select itemAccn from reservation_list where itemAccn=@itemAccn and brrId!='" + txtbBrrId.Text + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            MessageBox.Show("This item reserved by someone.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dgvItemDetails.ClearSelection();
                            dataReader.Close();
                            Cursor = Cursors.Default;
                            return;
                        }
                        dataReader.Close();
                        string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        DateTime returnDate = DateTime.Now.AddDays(Convert.ToDouble(numDay.Value));
                        string strRtnDate = returnDate.Day.ToString("00") + "/" + returnDate.Month.ToString("00") + "/" + returnDate.Year.ToString("0000");

                        queryString = "update item_details set itemAvailable='" + false + "' where itemAccession=@itemAccession";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();

                        queryString = "INSERT INTO issued_item (brrId,itemAccession,issueDate,expectedReturnDate,itemReturned,issuedBy) " +
                         " values (@brrId,@itemAccession" + ",'" + issueDate + "','" + strRtnDate + "'" + ",'" + false + "','Self')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAccession", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();

                        queryString = "delete from reservation_list where itemAccn=@itemAccn";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dgvItemDetails.SelectedRows[0].Cells[2].Value);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        dgvItemDetails.SelectedRows[0].Cells[5].Value = "False";
                        dgvItemDetails.SelectedRows[0].Cells[6].Value = txtbBrrId.Text;
                        dgvItemDetails.SelectedRows[0].Cells[7].Value = strRtnDate;
                        dgvItemDetails.SelectedRows[0].DefaultCellStyle.BackColor = Color.Red;
                        ttlIssue++;
                        Cursor = Cursors.Default;
                        MessageBox.Show("Item issued successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
                        mysqlConn.Close();
                    }
                    else
                    {
                        MessageBox.Show("You can't issue this item for this borrower.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvItemDetails.ClearSelection();
                        mysqlConn.Close();
                        Cursor = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    numDay.Value = 1;
                }
            }
            Cursor = Cursors.Default;
        }

        private void readEBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgvItemDetails.SelectedRows[0].Cells["digitalReference"].Value.ToString()!="")
            {
                string[] fileList = dgvItemDetails.SelectedRows[0].Cells["digitalReference"].Value.ToString().Split('?');
                if(Directory.Exists(Path.GetTempPath()+"digitalReference"))
                {
                    Directory.Delete(Path.GetTempPath() + "digitalReference", true);
                }
                Directory.CreateDirectory(Path.GetTempPath() + "digitalReference");
                foreach (string digitalFile in fileList)
                {
                    if (File.Exists(Properties.Settings.Default.databasePath.TrimEnd() + @"\" + digitalFile))
                    {
                        //byte[] fileByte = File.ReadAllBytes(Properties.Settings.Default.databasePath.TrimEnd() + @"\" + digitalFile);
                        File.Copy(Properties.Settings.Default.databasePath.TrimEnd() + @"\" + digitalFile,
                            Path.GetTempPath() + "digitalReference" + @"\" + digitalFile, true);
                        //File.WriteAllBytes(Path.GetTempPath() + "digitalReference" + @"\" + digitalFile, fileByte);
                    }
                }
                FormEbook readEBook = new FormEbook();
                readEBook.ShowDialog();
            }
            else
            {
                MessageBox.Show("No E-book avilable.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtbSearch_TextChanged(object sender, EventArgs e)
        {
            var itemList = fieldValueList;
            if (itemList.Count > 0)
            {
                //clear the items from the list
                lstbFilterData.Items.Clear();

                //filter the items and add them to the list
                lstbFilterData.Items.AddRange(
                    itemList.Where(i => i.ToLower().Contains(txtbSearch.Text.ToLower())).ToArray());

                lstbFilterData.Sorted = true;
                lstbFilterData.Visible = true;
                lstbFilterData.Size = lstbFilterData.PreferredSize;
                try
                {
                    lstbFilterData.SelectedIndex = 0;
                }
                catch
                {

                }
            }
            if (txtbSearch.Text != "")
            {
                //if (queryString != "")
                //{
                //    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                //    if (sqltConn.State == ConnectionState.Closed)
                //    {
                //        sqltConn.Open();
                //    }
                //    SQLiteDataAdapter da = new SQLiteDataAdapter(queryString, sqltConn);
                //    da.SelectCommand.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                //    da.SelectCommand.Parameters.AddWithValue("@fieldValue", "%" + txtbSearch.Text + "%");
                //    DataTable dt = new DataTable();
                //    da.Fill(dt); comboBox1.DataSource = dt;
                //    //lstbFilterData.DataSource = dt;
                //    //SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //    //sqltCommnd.CommandText = queryString;
                //    //sqltCommnd.Parameters.AddWithValue("@catName", cmbItemCategory.Text);
                //    //sqltCommnd.Parameters.AddWithValue("@fieldValue", "%" + txtbSearch.Text + "%");
                //    //SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                //    //lstbFilterData.Items.Clear();
                //    //if (dataReader.HasRows)
                //    //{

                //    //    List<string> idList = (from IDataRecord r in dataReader
                //    //                           select (string)r[0]
                //    //        ).ToList();
                //    //    lstbFilterData.Items.AddRange(idList.ToArray());
                //    //lstbFilterData.Visible = true;
                //    ////lstbFilterData.Height = lstbFilterData.PreferredHeight;
                //    //lstbFilterData.Size = lstbFilterData.PreferredSize;
                //    //lstbFilterData.SelectedIndex = 0;
                //    //}
                //    //dataReader.Close();
                //    sqltConn.Close();
                //}
                //else
                //{
                //    lstbFilterData.Items.Clear();
                //}
            }
            else
            {
                lstbFilterData.Items.Clear();
            }
            if (lstbFilterData.Items.Count == 0)
            {
                lstbFilterData.Visible = false;

            }
        }

        private void cmbItemCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSearch.Items.Clear();
            cmbSearch.Items.Add("Please choose a field name...");
            if (cmbItemCategory.SelectedIndex > 0)
            {
                cmbSearch.Items.Add("Accession No");
                cmbSearch.Items.Add("Subcategory");
                cmbSearch.Items.Add("ISBN/ISSN");
                cmbSearch.Items.Add("Title");
                cmbSearch.Items.Add("Subject");
                cmbSearch.Items.Add("Author");
                cmbSearch.Items.Add("Classification No");
                cmbSearch.Items.Add("Location");
                cmbSearch.Items.Add("No of Pages");
                cmbSearch.Items.Add("Price");

                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
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
                cmbSearch.Enabled = true;
                cmbSearch.SelectedIndex = 0;
            }
            else
            {
                cmbSearch.Items.Clear();
                txtbSearch.Clear();
                cmbSearch.Enabled = false;
            }
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {
            if(txtbBrrId.Text!="")
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
                    string brrCategory = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbName.Text = dataReader["brrName"].ToString();
                            txtbAddress.Text = dataReader["brrAddress"].ToString();
                            brrCategory = dataReader["brrCategory"].ToString();
                        }
                        dataReader.Close();

                        sqltCommnd.CommandText = "select catlogAccess,maxCheckin from borrowerSettings where catName=@catName";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@catName", brrCategory);
                        dataReader = sqltCommnd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                catgoryAccessList = dataReader["catlogAccess"].ToString().Split('$').ToList();
                                maxCheckin = Convert.ToInt32(dataReader["maxCheckin"].ToString());
                            }
                        }
                        dataReader.Close();
                        string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        sqltCommnd.CommandText = "select count(id) from issuedItem where brrId=@brrId and issueDate='" + issueDate + "'";
                        sqltCommnd.CommandType = CommandType.Text;
                        sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        ttlIssue = Convert.ToInt32(sqltCommnd.ExecuteScalar().ToString());
                        sqltConn.Close();

                        cmbItemCategory.Items.Clear();
                        cmbItemCategory.Items.Add("Please select a category");
                        foreach (string catAccess in catgoryAccessList)
                        {
                            if (cmbItemCategory.Items.IndexOf(catAccess.Substring(0, catAccess.IndexOf("("))) == -1)
                            {
                                cmbItemCategory.Items.Add(catAccess.Substring(0, catAccess.IndexOf("(")));
                            }
                        }
                        cmbItemCategory.SelectedIndex = 0;
                        cmbItemCategory.Enabled = true;
                    }
                    else
                    {
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
                    string brrCategory = "";
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbName.Text = dataReader["brrName"].ToString();
                            txtbAddress.Text = dataReader["brrAddress"].ToString();
                            brrCategory = dataReader["brrCategory"].ToString();
                        }
                        dataReader.Close();

                        queryString = "select catlogAccess,maxCheckin from borrower_settings where catName=@catName";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@catName", brrCategory);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                catgoryAccessList = dataReader["catlogAccess"].ToString().Split('$').ToList();
                                maxCheckin = Convert.ToInt32(dataReader["maxCheckin"].ToString());
                            }
                        }
                        dataReader.Close();
                        string issueDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                        queryString = "select count(id) from issued_item where brrId=@brrId and issueDate='" + issueDate + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        ttlIssue = Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString());
                        mysqlConn.Close();

                        cmbItemCategory.Items.Clear();
                        cmbItemCategory.Items.Add("Please select a category");
                        foreach (string catAccess in catgoryAccessList)
                        {
                            if (cmbItemCategory.Items.IndexOf(catAccess.Substring(0, catAccess.IndexOf("("))) == -1)
                            {
                                cmbItemCategory.Items.Add(catAccess.Substring(0, catAccess.IndexOf("(")));
                            }
                        }
                        cmbItemCategory.SelectedIndex = 0;
                        cmbItemCategory.Enabled = true;
                    }
                    else
                    {
                        dataReader.Close();
                    }
                    mysqlConn.Close();
                }
            }
            else
            {
                txtbName.Clear();
                txtbAddress.Clear();
                cmbItemCategory.Items.Clear();
                cmbItemCategory.Enabled = false;
            }
        }

    }

    class MenuColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Red; }
        }

        //public override Color MenuBorder  //Change color according your Need
        //{
        //    get { return Color.DimGray; }
        //}

        public override Color MenuItemBorder
        {
            get
            {
                return Color.DimGray;
            }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Red; }
        }
    }
}
