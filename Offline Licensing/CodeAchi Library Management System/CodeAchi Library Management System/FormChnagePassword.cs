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
    public partial class FormChnagePassword : Form
    {
        public FormChnagePassword()
        {
            InitializeComponent();
        }

        private void FormChnagePassword_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(txtbCurrent.Text=="")
            {
                MessageBox.Show("Please enter the current password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbCurrent.Select();
                return;
            }
            if (txtbCurrent.Text != globalVarLms.currentPassword)
            {
                MessageBox.Show("Please enter the right password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbCurrent.SelectAll();
                return;
            }
            if (txtbNew.Text == "")
            {
                MessageBox.Show("Please enter the new password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbNew.Select();
                return;
            }
            if (txtbConfirm.Text == "")
            {
                MessageBox.Show("Please confirm the password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbConfirm.Select();
                return;
            }
            if(txtbNew.Text!=txtbConfirm.Text)
            {
                MessageBox.Show("Please confirm the password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbConfirm.SelectAll();
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
                string queryString = "update userDetails set userPassword=:userPassword where userMail=@userMail";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@userMail", Properties.Settings.Default.currentUserId);
                sqltCommnd.Parameters.AddWithValue("userPassword", txtbConfirm.Text);
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
                string queryString = "update user_details set userPassword=@userPassword where userMail=@userMail";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                mysqlCmd.Parameters.AddWithValue("@userMail", Properties.Settings.Default.currentUserId);
                mysqlCmd.Parameters.AddWithValue("@userPassword", txtbConfirm.Text);
                mysqlCmd.ExecuteNonQuery();
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            globalVarLms.currentPassword = txtbConfirm.Text;
            txtbConfirm.Clear();
            txtbCurrent.Clear();
            txtbNew.Clear();
            MessageBox.Show("Password changed successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pcbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtbCurrent_TextChanged(object sender, EventArgs e)
        {
            txtbCurrent.UseSystemPasswordChar = true;
        }

        private void txtbNew_TextChanged(object sender, EventArgs e)
        {
            txtbNew.UseSystemPasswordChar = true;
        }

        private void txtbConfirm_TextChanged(object sender, EventArgs e)
        {
            txtbConfirm.UseSystemPasswordChar = true;
        }
    }
}
