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
    public partial class UcBorrewerSetting : UserControl
    {
        public UcBorrewerSetting()
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

        string brrPassword = "";

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtbCategoryName.Text=="")
            {
                MessageBox.Show("Please add category name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbCategoryName.Select();
                return;
            }
            string catAccess = null;
            foreach (Control pnlControl in pnlChkb.Controls)
            {
                if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name != "All" && ((CheckBox)(pnlControl)).Checked)
                {
                    if (catAccess == null)
                    {
                        catAccess = ((CheckBox)(pnlControl)).Text;
                    }
                    else
                    {
                        catAccess = catAccess + "$" + ((CheckBox)(pnlControl)).Text;
                    }
                }
            }
            if (catAccess == null)
            {
                MessageBox.Show("Please give some category access.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string addInfo = null;
            if (chkbAdditional1.Checked)
            {
                addInfo = "1_" + txtbAdditional1.Text;
            }
            if (chkbAdditional2.Checked)
            {
                addInfo = addInfo + "$2_" + txtbAdditional2.Text;
            }
            if (chkbAdditional3.Checked)
            {
                addInfo = addInfo + "$3_" + txtbAdditional3.Text;
            }
            if (chkbAdditional4.Checked)
            {
                addInfo = addInfo + "$4_" + txtbAdditional4.Text;
            }
            if (chkbAdditional5.Checked)
            {
                addInfo = addInfo + "$5_" + txtbAdditional5.Text;
            }
            bool avoidFine = false;
            if(chkbAvdFine.Checked)
            {
                avoidFine = true;
            }
            if (btnSave.Text == "Sa&ve")// =======================Borrower Category Add=================
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select catName from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        MessageBox.Show("Category already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbCategoryName.Select();
                        return;
                    }
                    dataReader.Close();
                    queryString = "INSERT INTO borrowerSettings (catName,catDesc,catlogAccess,capInfo,maxDue,avoidFine,defaultPass) VALUES (@catName,@catDesc,@catlogAccess,@capInfo,'" + txtbDueLimit.Text + "','" + avoidFine + "',@defaultPass);";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                    sqltCommnd.Parameters.AddWithValue("@catlogAccess", catAccess);
                    sqltCommnd.Parameters.AddWithValue("@capInfo", addInfo);
                    sqltCommnd.Parameters.AddWithValue("@defaultPass", txtbPassword.Text);
                    sqltCommnd.ExecuteNonQuery();
                    sqltConn.Close();
                }
                else
                {
                    bool settingExist = false;
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
                    string queryString = "select catName from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        settingExist = true;
                    }
                    dataReader.Close();
                    if(!settingExist)
                    {
                        queryString = "INSERT INTO borrower_settings (catName,catDesc,catlogAccess,capInfo,maxDue,avoidFine,defaultPass) VALUES (@catName,@catDesc,@catlogAccess,@capInfo,'" + txtbDueLimit.Text + "','" + avoidFine + "',@defaultPass);";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                        mysqlCmd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                        mysqlCmd.Parameters.AddWithValue("@catlogAccess", catAccess);
                        mysqlCmd.Parameters.AddWithValue("@capInfo", addInfo);
                        mysqlCmd.Parameters.AddWithValue("@defaultPass", txtbPassword.Text);
                        mysqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Category already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbCategoryName.Select();
                    }
                    mysqlConn.Close();
                }
                dgvBrrCategory.Rows.Add(txtbCategoryName.Text);
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category added successfully !";
                showNotification();
                clearData();
                enableDisable();
            }
            else//============================Category update========================
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "update borrowerSettings set catDesc=:catDesc,catlogAccess=:catlogAccess," +
                        "capInfo=:capInfo,maxDue='" + txtbDueLimit.Text + "',avoidFine='" + avoidFine + "',defaultPass=:defaultPass where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("catDesc", txtbCategoryDesc.Text);
                    sqltCommnd.Parameters.AddWithValue("catlogAccess", catAccess);
                    sqltCommnd.Parameters.AddWithValue("capInfo", addInfo);
                    sqltCommnd.Parameters.AddWithValue("defaultPass", txtbPassword.Text);
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update borrowerDetails set brrPass=:brrPass where brrPass=@brrPass and brrCategory=@brrCategory;";
                    sqltCommnd.Parameters.AddWithValue("@brrPass", brrPassword);
                    sqltCommnd.Parameters.AddWithValue("@brrCategory", txtbCategoryName.Text);
                    sqltCommnd.Parameters.AddWithValue("brrPass", txtbPassword.Text);
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
                    string queryString = "update borrower_settings set catDesc=@catDesc,catlogAccess=@catlogAccess," +
                        "capInfo=@capInfo,maxDue='" + txtbDueLimit.Text + "',avoidFine='" + avoidFine + "',defaultPass=@defaultPass where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@catDesc", txtbCategoryDesc.Text);
                    mysqlCmd.Parameters.AddWithValue("@catlogAccess", catAccess);
                    mysqlCmd.Parameters.AddWithValue("@capInfo", addInfo);
                    mysqlCmd.Parameters.AddWithValue("@defaultPass", txtbPassword.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    queryString= "update borrower_details set brrPass=@brrPass where brrPass=@brrPass1 and brrCategory=@brrCategory;";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrPass", brrPassword);
                    mysqlCmd.Parameters.AddWithValue("@brrCategory", txtbCategoryName.Text);
                    mysqlCmd.Parameters.AddWithValue("@brrPass1", txtbPassword.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category updated successfully !";
                showNotification();
                clearData();
                enableDisable();
                foreach (Control pnlControl in pnlChkb.Controls)
                {
                    if (pnlControl is CheckBox)
                    {
                        ((CheckBox)(pnlControl)).Enabled = true;
                        ((CheckBox)(pnlControl)).Checked = false;
                    }
                }
                btnSave.Text = "Sa&ve";
            }
            chkbAdditional2.TabIndex = 5;
            groupBox2.TabIndex = 6;
            pnlChkb.TabIndex = 7;
            btnSave.TabIndex = 8;
            btnReset.TabIndex = 9;
            chkbAvdFine.Checked = false;
        }

        public void UcBorrewerSetting_Load(object sender, EventArgs e)
        {
            enableDisable();
            clearData();
            txtbCategoryName.Select();
        }

        public void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkbButton = sender as CheckBox;
            if (chkbButton == null)
            {
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (chkbButton.Checked)
            {
                if (chkbButton.Name == "All")
                {
                    foreach (Control pnlControl in pnlChkb.Controls)
                    {
                        if (pnlControl is CheckBox)
                        {
                            ((CheckBox)(pnlControl)).Checked = true;
                        }
                    }
                }
                else
                {
                    int checkboxCount = 0;
                    foreach (Control pnlControl in pnlChkb.Controls)
                    {
                        if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name != "All" )
                        {
                            checkboxCount++;
                        }
                    }

                    int checkCount = 0;
                    foreach (Control pnlControl in pnlChkb.Controls)
                    {
                        if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name != "All" &&
                            ((CheckBox)(pnlControl)).Checked)
                        {
                            checkCount++;
                        }
                    }

                    if(checkboxCount==checkCount)
                    {
                        foreach (Control pnlControl in pnlChkb.Controls)
                        {
                            if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                            {
                                ((CheckBox)(pnlControl)).Checked = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (chkbButton.Name == "All")
                {
                    int checkCount = 0;
                    foreach (Control pnlControl in pnlChkb.Controls)
                    {
                        if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name != "All" && 
                            !((CheckBox)(pnlControl)).Checked)
                        {
                            checkCount++;
                        }
                    }
                    if (checkCount == 0)
                    {
                        foreach (Control pnlControl in pnlChkb.Controls)
                        {
                            if (pnlControl is CheckBox)
                            {
                                if (((CheckBox)(pnlControl)).Enabled == true)
                                {
                                    ((CheckBox)(pnlControl)).Checked = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Control pnlControl in pnlChkb.Controls)
                    {
                        if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name=="All")
                        {
                            ((CheckBox)(pnlControl)).Checked = false;
                        }
                    }
                }
            }
        }

        private void chkbAdditional1_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAdditional1.Checked)
            {
                txtbAdditional1.Enabled = true;
                txtbAdditional1.Select();
                chkbAdditional2.TabIndex = 6;
                groupBox2.TabIndex = 7;
                pnlChkb.TabIndex = 8;
                btnSave.TabIndex = 9;
                btnReset.TabIndex = 10;
            }
            else
            {
                txtbAdditional1.Enabled = false;
            }
        }

        private void chkbAdditional2_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAdditional2.Checked)
            {
                txtbAdditional2.Enabled = true;
                txtbAdditional2.Select();
                chkbAdditional3.TabIndex = 7;
                groupBox2.TabIndex = 8;
                pnlChkb.TabIndex = 9;
                btnSave.TabIndex = 10;
                btnReset.TabIndex = 11;
            }
            else
            {
                txtbAdditional2.Enabled = false;
            }
        }

        private void chkbAdditional3_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAdditional3.Checked)
            {
                txtbAdditional3.Enabled=true;
                txtbAdditional3.Select();
                chkbAdditional4.TabIndex = 8;
                groupBox2.TabIndex = 9;
                pnlChkb.TabIndex = 10;
                btnSave.TabIndex = 11;
                btnReset.TabIndex = 12;
            }
            else
            {
                txtbAdditional3.Enabled = false;
            }
        }

        private void chkbAdditional4_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAdditional4.Checked)
            {
                txtbAdditional4.Enabled = true;
                txtbAdditional4.Select();
                chkbAdditional5.TabIndex = 9;
                groupBox2.TabIndex = 10;
                pnlChkb.TabIndex = 11;
                btnSave.TabIndex = 12;
                btnReset.TabIndex = 13;
            }
            else
            {
                txtbAdditional4.Enabled = false;
            }
        }

        private void chkbAdditional5_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAdditional5.Checked)
            {
                txtbAdditional5.Enabled = true;
                txtbAdditional5.Select();
                groupBox2.TabIndex = 10;
                pnlChkb.TabIndex = 11;
                btnSave.TabIndex = 12;
                btnReset.TabIndex = 13;
            }
            else
            {
                txtbAdditional5.Enabled = false;
            }
        }

        private void txtbAdditional1_Leave(object sender, EventArgs e)
        {
            if(txtbAdditional1.Text=="")
            {
                txtbAdditional1.Select();
                return;
            }
            chkbAdditional2.Enabled = true;
            chkbAdditional2.Select();
        }

        private void txtbAdditional2_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional2.Text == "")
            {
                txtbAdditional2.Select();
                return;
            }
            chkbAdditional3.Enabled = true;
            chkbAdditional3.Select();
        }

        private void txtbAdditional3_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional3.Text == "")
            {
                txtbAdditional3.Select();
                return;
            }
            chkbAdditional4.Enabled = true;
            chkbAdditional4.Select();
        }

        private void txtbAdditional4_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional4.Text == "")
            {
                txtbAdditional4.Select();
                return;
            }
            chkbAdditional5.Enabled = true;
            chkbAdditional5.Select();
        }

        private void txtbAdditional5_Leave(object sender, EventArgs e)
        {
            if (txtbAdditional5.Text == "")
            {
                txtbAdditional5.Select();
                return;
            }
        }

        public void clearData()
        {
            txtbCategoryName.Clear();
            txtbCategoryDesc.Clear();
            txtbFees.Text=0.00.ToString("0.00");
            txtbDueLimit.Text = 0.00.ToString("0.00");
            txtbMaxitems.Text=0.ToString();
            txtbAdditional1.Clear();
            txtbAdditional2.Clear();
            txtbAdditional3.Clear();
            txtbAdditional4.Clear();
            txtbAdditional5.Clear();
            chkbAdditional1.Checked = false;
            for (int i = 0; i < pnlChkb.Controls.Count; i++)
            {
                Control pnlControl = pnlChkb.Controls[i];
                if (pnlControl is CheckBox)
                {
                    if (((CheckBox)(pnlControl)).Name == "All")
                    {
                        ((CheckBox)(pnlControl)).Checked = false;
                    }
                }
            }
        }

        public void enableDisable()
        {
            chkbAdditional1.Checked = false;
            chkbAdditional2.Checked = false;
            chkbAdditional3.Checked = false;
            chkbAdditional4.Checked = false;
            chkbAdditional5.Checked = false;

            chkbAdditional1.Enabled = true;
            chkbAdditional2.Enabled = false;
            chkbAdditional3.Enabled = false;
            chkbAdditional4.Enabled = false;
            chkbAdditional5.Enabled = false;

            txtbAdditional1.Enabled = false;
            txtbAdditional2.Enabled = false;
            txtbAdditional3.Enabled = false;
            txtbAdditional4.Enabled = false;
            txtbAdditional5.Enabled = false;
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

        private void txtbFine_Leave(object sender, EventArgs e)
        {
            if(txtbFees.Text=="")
            {
                txtbFees.Text = 0.00.ToString("0.00");
            }
        }

        private void txtbFine_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtbMaxitems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvBrrCategory_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvBrrCategory.HitTest(e.X, e.Y);
                if (hti.RowIndex >= 0)
                {
                    dgvBrrCategory.ClearSelection();
                    dgvBrrCategory.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvBrrCategory, new Point(e.X, e.Y));
                }
            }
        }

        private void txtbDueLimit_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtbDueLimit_Leave(object sender, EventArgs e)
        {
            if (txtbDueLimit.Text == "")
            {
                txtbDueLimit.Text = 0.00.ToString("0.00");
            }
        }

        private void txtbMaxitems_Leave(object sender, EventArgs e)
        {
            if (txtbMaxitems.Text == "")
            {
                txtbMaxitems.Text = 0.ToString();
            }
        }

        private void txtbFees_Enter(object sender, EventArgs e)
        {
            txtbFees.Clear();
        }

        private void txtbMaxitems_Enter(object sender, EventArgs e)
        {
            txtbMaxitems.Clear();
        }

        private void txtbDueLimit_Enter(object sender, EventArgs e)
        {
            txtbDueLimit.Clear();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData();
            enableDisable();
            txtbCategoryName.Enabled = true;
            btnSave.Text = "Sa&ve";
            foreach (Control pnlControl in pnlChkb.Controls)
            {
                if (pnlControl is CheckBox)
                {
                    ((CheckBox)(pnlControl)).Enabled = true;
                    ((CheckBox)(pnlControl)).Checked = false;
                }
            }
            dgvBrrCategory.ClearSelection();
            chkbAdditional2.TabIndex = 5;
            groupBox2.TabIndex = 6;
            pnlChkb.TabIndex = 7;
            btnSave.TabIndex = 8;
            btnReset.TabIndex = 9;
            chkbAvdFine.Checked = false;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnClose_Click(null, null);
            timer1.Stop();
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

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from borrowerSettings where catName=@catName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbCategoryName.Enabled = false;
                        txtbCategoryName.Text = dataReader["catName"].ToString();
                        txtbCategoryDesc.Text = dataReader["catDesc"].ToString();
                        //txtbMaxitems.Text = dataReader["maxCheckin"].ToString();
                        //txtbFees.Text = dataReader["membershipFees"].ToString();
                        txtbDueLimit.Text = dataReader["maxDue"].ToString();
                        txtbPassword.Text = dataReader["defaultPass"].ToString();
                        brrPassword = dataReader["defaultPass"].ToString();
                        string catlogAccess = dataReader["catlogAccess"].ToString();
                        string capInfo = dataReader["capInfo"].ToString();
                        if (Convert.ToBoolean(dataReader["avoidFine"].ToString()))
                        {
                            chkbAvdFine.Checked = true;
                        }
                        else
                        {
                            chkbAvdFine.Checked = false;
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
                            }
                        }
                        else
                        {
                            chkbAdditional1.Checked = false;
                            chkbAdditional2.Checked = false;
                            chkbAdditional3.Checked = false;
                            chkbAdditional4.Checked = false;
                            chkbAdditional5.Checked = false;

                            chkbAdditional1.Enabled = true;
                            chkbAdditional2.Enabled = false;
                            chkbAdditional3.Enabled = false;
                            chkbAdditional4.Enabled = false;
                            chkbAdditional5.Enabled = false;

                            txtbAdditional1.Clear();
                            txtbAdditional2.Clear();
                            txtbAdditional3.Clear();
                            txtbAdditional4.Clear();
                            txtbAdditional5.Clear();

                            txtbAdditional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                        }
                        if (catlogAccess != "")
                        {
                            int checkboxCount = 0;
                            int checkCount = 0;
                            List<string> catlogLists = catlogAccess.Split('$').ToList();
                            foreach (Control pnlControl in pnlChkb.Controls)
                            {
                                if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                {
                                    //((CheckBox)(pnlControl)).Enabled = false;
                                    ((CheckBox)(pnlControl)).Checked = false;
                                }
                                else
                                {
                                    checkboxCount++;
                                    ((CheckBox)(pnlControl)).Checked = false;
                                    ((CheckBox)(pnlControl)).Enabled = true;
                                    if (catlogLists.IndexOf(((CheckBox)(pnlControl)).Text) != -1)
                                    {
                                        ((CheckBox)(pnlControl)).Checked = true;
                                        ((CheckBox)(pnlControl)).Enabled = false;
                                        checkCount++;
                                    }
                                }
                            }
                            if(checkCount==checkboxCount)
                            {
                                foreach (Control pnlControl in pnlChkb.Controls)
                                {
                                    if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                    {
                                        ((CheckBox)(pnlControl)).Enabled = false;
                                        ((CheckBox)(pnlControl)).Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                foreach (Control pnlControl in pnlChkb.Controls)
                                {
                                    if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                    {
                                        ((CheckBox)(pnlControl)).Enabled = true;
                                    }
                                }
                            }
                        }
                    }
                    btnSave.Text = "Update";
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
                string queryString = "select * from borrower_settings where catName=@catName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbCategoryName.Enabled = false;
                        txtbCategoryName.Text = dataReader["catName"].ToString();
                        txtbCategoryDesc.Text = dataReader["catDesc"].ToString();
                        txtbDueLimit.Text = dataReader["maxDue"].ToString();
                        txtbPassword.Text = dataReader["defaultPass"].ToString();
                        brrPassword = dataReader["defaultPass"].ToString();
                        string catlogAccess = dataReader["catlogAccess"].ToString();
                        string capInfo = dataReader["capInfo"].ToString();
                        if (Convert.ToBoolean(dataReader["avoidFine"].ToString()))
                        {
                            chkbAvdFine.Checked = true;
                        }
                        else
                        {
                            chkbAvdFine.Checked = false;
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
                            }
                        }
                        else
                        {
                            chkbAdditional1.Checked = false;
                            chkbAdditional2.Checked = false;
                            chkbAdditional3.Checked = false;
                            chkbAdditional4.Checked = false;
                            chkbAdditional5.Checked = false;

                            chkbAdditional1.Enabled = true;
                            chkbAdditional2.Enabled = false;
                            chkbAdditional3.Enabled = false;
                            chkbAdditional4.Enabled = false;
                            chkbAdditional5.Enabled = false;

                            txtbAdditional1.Clear();
                            txtbAdditional2.Clear();
                            txtbAdditional3.Clear();
                            txtbAdditional4.Clear();
                            txtbAdditional5.Clear();

                            txtbAdditional1.Enabled = false;
                            txtbAdditional2.Enabled = false;
                            txtbAdditional3.Enabled = false;
                            txtbAdditional4.Enabled = false;
                            txtbAdditional5.Enabled = false;
                        }
                        if (catlogAccess != "")
                        {
                            int checkboxCount = 0;
                            int checkCount = 0;
                            List<string> catlogLists = catlogAccess.Split('$').ToList();
                            foreach (Control pnlControl in pnlChkb.Controls)
                            {
                                if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                {
                                    //((CheckBox)(pnlControl)).Enabled = false;
                                    ((CheckBox)(pnlControl)).Checked = false;
                                }
                                else
                                {
                                    checkboxCount++;
                                    ((CheckBox)(pnlControl)).Checked = false;
                                    ((CheckBox)(pnlControl)).Enabled = true;
                                    if (catlogLists.IndexOf(((CheckBox)(pnlControl)).Text) != -1)
                                    {
                                        ((CheckBox)(pnlControl)).Checked = true;
                                        ((CheckBox)(pnlControl)).Enabled = false;
                                        checkCount++;
                                    }
                                }
                            }
                            if (checkCount == checkboxCount)
                            {
                                foreach (Control pnlControl in pnlChkb.Controls)
                                {
                                    if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                    {
                                        ((CheckBox)(pnlControl)).Enabled = false;
                                        ((CheckBox)(pnlControl)).Checked = true;
                                    }
                                }
                            }
                            else
                            {
                                foreach (Control pnlControl in pnlChkb.Controls)
                                {
                                    if (pnlControl is CheckBox && ((CheckBox)(pnlControl)).Name == "All")
                                    {
                                        ((CheckBox)(pnlControl)).Enabled = true;
                                    }
                                }
                            }
                        }
                    }
                    btnSave.Text = "Update";
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("if you delete the category all data associated with that Category will get deleted." + Environment.NewLine +
              "Are you want to delete ?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IntPtr hMenu = GetSystemMenu(this.Parent.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);

                List<string> brrIdList = new List<string> { };
                string currentDateTime = DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("hhmmss");
                if (globalVarLms.sqliteData)
                {
                    string sourceFile = Properties.Settings.Default.databasePath + @"\LMS.sl3";
                    string destFile = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvBrrCategory.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Borrower_category_" + currentDateTime + ".sl3";
                    File.Copy(sourceFile, destFile);

                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();

                    string queryString = "select brrId from borrowerDetails where brrCategory=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                    }
                    dataReader.Close();
                    ///Delete from borrower details
                    queryString = "delete from borrowerDetails where brrCategory=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    foreach (string brrId in brrIdList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issuedItem where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from paymentDetails where memberId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lostDamage where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                        sqltCommnd.ExecuteNonQuery();

                        //=========delete from Opak Details========
                        queryString = "delete from reservationList where brrId=@brrId";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@brrId", brrId);
                        sqltCommnd.ExecuteNonQuery();
                    }

                    queryString = "delete from borrowerSettings where catName=@catName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    sqltCommnd.ExecuteNonQuery();

                    string taskDesc = dgvBrrCategory.SelectedCells[0].Value.ToString() + " borrower category delete";
                    string combindedString = string.Join("$", brrIdList.ToArray());
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into userActivity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + combindedString + "','" + taskDesc + "','" + currentDateTime + "')";
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
                    string backupName = globalVarLms.backupPath + @"\Backup_before_delete_" + dgvBrrCategory.SelectedCells[0].Value.ToString().Replace("/", "_").Replace(@"\", "_") + "_Borrower_category_" + currentDateTime + ".sql";
                    mb.ExportToFile(backupName);
                    mb.Dispose();

                    string queryString = "select brrId from borrower_details where brrCategory=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        brrIdList = (from IDataRecord r in dataReader select (string)r["brrId"]).ToList();
                    }
                    dataReader.Close();

                    ///Delete from borrower details
                    queryString = "delete from borrower_details where brrCategory=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    foreach (string brrId in brrIdList)
                    {
                        //=========delete from issue Details========
                        queryString = "delete from issued_item where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from Payment Details========
                        queryString = "delete from payment_details where memberId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from lost/damage Details========
                        queryString = "delete from lost_damage where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                        mysqlCmd.ExecuteNonQuery();

                        //=========delete from Opak Details========
                        queryString = "delete from reservation_list where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.Parameters.AddWithValue("@brrId", brrId);
                        mysqlCmd.ExecuteNonQuery();
                    }

                    queryString = "delete from borrower_settings where catName=@catName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.Parameters.AddWithValue("@catName", dgvBrrCategory.SelectedCells[0].Value.ToString());
                    mysqlCmd.ExecuteNonQuery();

                    string taskDesc = dgvBrrCategory.SelectedCells[0].Value.ToString() + " borrower category delete";
                    string combindedString = string.Join("$", brrIdList.ToArray());
                    currentDateTime = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000") + " " + DateTime.Now.ToString("hh:mm:ss tt");
                    queryString = "insert into user_activity (userId,itemAccn,brrId,taskDesc,dateTime) values ('" + globalVarLms.currentUserId + "','" + "" + "', '" + combindedString + "','" + taskDesc + "','" + currentDateTime + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();

                    mysqlConn.Close();
                }
                dgvBrrCategory.Rows.RemoveAt(dgvBrrCategory.SelectedRows[0].Index);
                globalVarLms.backupRequired = true;
                lblUserMessage.Text = "Category deleted successfully !";
                showNotification();
                
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
            }
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

        private void deleteToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.WhiteSmoke;
            deleteToolStripMenuItem.ForeColor = Color.Black;
        }

        private void deleteToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            deleteToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }
    }
}
