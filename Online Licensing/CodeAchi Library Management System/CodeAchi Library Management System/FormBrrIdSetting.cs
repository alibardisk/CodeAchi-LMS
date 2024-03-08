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
    public partial class FormBrrIdSetting : Form
    {
        public FormBrrIdSetting()
        {
            InitializeComponent();
        }

        private void FormBrrIdSetting_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from brrIdSetting";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["isAutoGenerate"].ToString() == "True")
                        {
                            timer1.Interval = 450;
                            timer1.Start();
                            rdbAuto.Checked = true;
                            grpbAuto.Enabled = true;
                            if (dataReader["isManualPrefix"].ToString() == "True")
                            {
                                rdbPrefixText.Checked = true;
                                txtbPrefix.Enabled = true;
                                txtbPrefix.Text = dataReader["prefixText"].ToString();
                            }
                            else
                            {
                                rdbCategory.Checked = true;
                                txtbPrefix.Enabled = false;
                            }
                            txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                        }
                        else
                        {
                            rdbManual.Checked = true;
                            grpbAuto.Enabled = false;
                        }
                    }
                    btnSave.Text = "Update";
                }
                else
                {
                    btnSave.Text = "Save";
                }
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
                    string queryString = "select * from brrid_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["isAutoGenerate"].ToString() == "True")
                            {
                                timer1.Interval = 450;
                                timer1.Start();
                                rdbAuto.Checked = true;
                                grpbAuto.Enabled = true;
                                if (dataReader["isManualPrefix"].ToString() == "True")
                                {
                                    rdbPrefixText.Checked = true;
                                    txtbPrefix.Enabled = true;
                                    txtbPrefix.Text = dataReader["prefixText"].ToString();
                                }
                                else
                                {
                                    rdbCategory.Checked = true;
                                    txtbPrefix.Enabled = false;
                                }
                                txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                            }
                            else
                            {
                                rdbManual.Checked = true;
                                grpbAuto.Enabled = false;
                            }
                        }
                        btnSave.Text = "Update";
                    }
                    else
                    {
                        btnSave.Text = "Save";
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void FormBrrIdSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAuto.Checked)
            {
                grpbAuto.Enabled = true;
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from brrIdSetting";
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    timer1.Interval = 450;
                    timer1.Start();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["isManualPrefix"].ToString() == "True")
                            {
                                rdbPrefixText.Checked = true;
                                txtbPrefix.Enabled = true;
                                txtbPrefix.Select();
                            }
                            else
                            {
                                rdbCategory.Checked = true;
                                txtbPrefix.Enabled = false;
                            }
                            txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                        }
                    }
                    else
                    {
                        rdbCategory.Checked = true;
                        txtbPrefix.Enabled = false;
                        txtbJoinChar.Text = "-";
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
                    string queryString = "select * from brrid_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    timer1.Interval = 450;
                    timer1.Start();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["isManualPrefix"].ToString() == "True")
                            {
                                rdbPrefixText.Checked = true;
                                txtbPrefix.Enabled = true;
                                txtbPrefix.Select();
                            }
                            else
                            {
                                rdbCategory.Checked = true;
                                txtbPrefix.Enabled = false;
                            }
                            txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                        }
                    }
                    else
                    {
                        rdbCategory.Checked = true;
                        txtbPrefix.Enabled = false;
                        txtbJoinChar.Text = "-";
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
            else
            {
                grpbAuto.Enabled = false;
            }
        }

        private void txtbJoinChar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '/' && e.KeyChar != '\\' && e.KeyChar != '-' &&
               e.KeyChar != '_' && e.KeyChar != ' ' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    if (!char.IsControl(e.KeyChar) && txtbJoinChar.Text.Length > 0)
                    {
                        e.Handled = true;
                    }
                }
                catch
                {

                }
            }
        }

        private void rdbCategory_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCategory.Checked)
            {
                txtbPrefix.Enabled = false;
                txtbPrefix.Clear();
            }
            else
            {
                txtbPrefix.Enabled = true;
                txtbPrefix.Select();
            }
        }

        private void txtbPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && txtbPrefix.Text.Count(char.IsLetter) > 2)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string queryString = "";
            bool isAutoGenerate = true, isManualPrefix = true;
            if (rdbManual.Checked)
            {
                isAutoGenerate = false;
            }
            else
            {
                if (rdbCategory.Checked)
                {
                    isManualPrefix = false;
                }
                else
                {
                    if (txtbPrefix.Text == "")
                    {
                        MessageBox.Show("Please enter the prefix text.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbPrefix.Select();
                        return;
                    }
                }
            }
            if (btnSave.Text == "Save")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd;
                    queryString = "insert into brrIdSetting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + isAutoGenerate + "','" + isManualPrefix + "','" + txtbPrefix.Text + "','" + txtbJoinChar.Text + "','" + 0 + "');";
                    sqltCommnd = sqltConn.CreateCommand();
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
                    queryString = "insert into brrid_setting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber) values ('" + isAutoGenerate + "','" + isManualPrefix + "','" + txtbPrefix.Text + "','" + txtbJoinChar.Text + "','" + 0 + "');";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Borrower id setting saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Text = "Update";
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
                    SQLiteCommand sqltCommnd;
                    queryString = "update brrIdSetting  set isAutoGenerate='" + isAutoGenerate + "' ,isManualPrefix='" + isManualPrefix + "' ,prefixText='" + txtbPrefix.Text + "',joiningChar='" + txtbJoinChar.Text + "'";
                    sqltCommnd = sqltConn.CreateCommand();
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
                    queryString = "update brrid_setting  set isAutoGenerate='" + isAutoGenerate + "' ,isManualPrefix='" + isManualPrefix + "' ,prefixText='" + txtbPrefix.Text + "',joiningChar='" + txtbJoinChar.Text + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Borrower id setting updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
      
    }
}
