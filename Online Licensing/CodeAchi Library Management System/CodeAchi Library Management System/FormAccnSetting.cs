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
    public partial class FormAccnSetting : Form
    {
        public FormAccnSetting()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void FormAccnSetting_Load(object sender, EventArgs e)
        {
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from accnSetting";
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
                            else if (dataReader["noPrefix"].ToString() == "True")
                            {
                                rdbnoPrefix.Checked = true;
                            }
                            else
                            {
                                rdbSubcategory.Checked = true;
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
                    string queryString = "select * from accn_setting";
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
                                else if (dataReader["noPrefix"].ToString() == "True")
                                {
                                    rdbnoPrefix.Checked = true;
                                }
                                else
                                {
                                    rdbSubcategory.Checked = true;
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
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
        }

        private void FormAccnSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                          this.DisplayRectangle);
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbAuto.Checked)
            {
                grpbAuto.Enabled = true;
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from accnSetting";
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
                                rdbSubcategory.Checked = true;
                                txtbPrefix.Enabled = false;
                            }
                            txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                        }
                    }
                    else
                    {
                        rdbSubcategory.Checked = true;
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
                    string queryString = "select * from accn_setting";
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
                                rdbSubcategory.Checked = true;
                                txtbPrefix.Enabled = false;
                            }
                            txtbJoinChar.Text = dataReader["joiningChar"].ToString();
                        }
                    }
                    else
                    {
                        rdbSubcategory.Checked = true;
                        txtbPrefix.Enabled = false;
                        txtbJoinChar.Text = "-";
                    }
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
            if(e.KeyChar!='/' && e.KeyChar != '\\' && e.KeyChar != '-' &&
                e.KeyChar != '_' && e.KeyChar != '.' && e.KeyChar != '~' &&
                !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                try
                {
                    if(!char.IsControl(e.KeyChar) && txtbJoinChar.Text.Length>0)
                    {
                        e.Handled = true;
                    }
                }
                catch
                {

                }
            }
        }

        private void rdbSubcategory_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbSubcategory.Checked)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            string queryString = "";
            bool isAutoGenerate = true, isManualPrefix = true, isnoPrefix = false ;
            if(rdbManual.Checked)
            {
                isAutoGenerate = false;
            }
            else
            {
                if(rdbSubcategory.Checked)
                {
                    isManualPrefix = false;
                }
                else if(rdbnoPrefix.Checked)
                {
                    isnoPrefix = true;
                    isManualPrefix = false;
                }
                else
                {
                    if(txtbPrefix.Text=="")
                    {
                        MessageBox.Show("Please enter the prefix text.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbPrefix.Select();
                        return;
                    }
                }
            }
            if(btnSave.Text=="Save")
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd;
                    queryString = "insert into accnSetting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber,noPrefix) values ('" + isAutoGenerate + "','" + isManualPrefix + "','" + txtbPrefix.Text + "','" + txtbJoinChar.Text + "','" + 0 + "','" + isnoPrefix + "');";
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
                    queryString = "insert into accn_setting (isAutoGenerate,isManualPrefix,prefixText,joiningChar,lastNumber,noPrefix) values ('" + isAutoGenerate + "','" + isManualPrefix + "','" + txtbPrefix.Text + "','" + txtbJoinChar.Text + "','" + 0 + "','" + isnoPrefix + "');";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Accession settiing saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Text = "Update";
            }
            else
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd;
                    queryString = "update accnSetting  set isAutoGenerate='" + isAutoGenerate + "' ,isManualPrefix='" + isManualPrefix + "' ,prefixText='" + txtbPrefix.Text + "',joiningChar='" + txtbJoinChar.Text + "',noPrefix='" + isnoPrefix + "'";
                    sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.ExecuteNonQuery();
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
                    queryString = "update accn_setting  set isAutoGenerate='" + isAutoGenerate + "' ,isManualPrefix='" + isManualPrefix + "' ,prefixText='" + txtbPrefix.Text + "',joiningChar='" + txtbJoinChar.Text + "',noPrefix='" + isnoPrefix + "'";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                MessageBox.Show("Accession setting update successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtbPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                if (!char.IsControl(e.KeyChar) && txtbPrefix.Text.Count(char.IsLetter)>2)
                {
                    e.Handled = true;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("1. Accession Number is unique ID for each copy of the Books & Items you will add." + Environment.NewLine +
                "2. Set \"Manual Accession\" if you want to enter Accession ID Manually for Your Books & Items." + Environment.NewLine +
                "3. Set \"Auto Generate\" if you want to generate Accession ID automatically." + Environment.NewLine +
                "4. Accession ID will be printed as Barcode if you want to use Barcode feature.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rdbnoPrefix_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbnoPrefix.Checked)
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
    }
}
