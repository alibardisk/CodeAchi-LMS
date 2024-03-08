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

namespace CodeAchi_LMS_Search
{
    public partial class FormChangePassword : Form
    {
        public FormChangePassword()
        {
            InitializeComponent();
        }

        public string connectionString = "", userName = "",userPassword="";

        private void txtbOld_TextChanged(object sender, EventArgs e)
        {
            txtbOld.UseSystemPasswordChar = true;
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void txtbConfirm_TextChanged(object sender, EventArgs e)
        {
            txtbConfirm.UseSystemPasswordChar = true;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(txtbOld.Text=="")
            {
                MessageBox.Show("Please enter your current password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbOld.Focus();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter the new password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Focus();
                return;
            }
            if (txtbConfirm.Text == "")
            {
                MessageBox.Show("Please confirm your password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbConfirm.Focus();
                return;
            }
            if (txtbOld.Text != userPassword)
            {
                MessageBox.Show("Password missmatch.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbOld.SelectAll();
                return;
            }
            if (txtbPassword.Text != txtbConfirm.Text)
            {
                MessageBox.Show("Password missmatch.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbConfirm.SelectAll();
                return;
            }

            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            sqltCommnd.CommandText = "update userDetails set userPassword='"+txtbPassword.Text.TrimEnd()+"' where userMail=@userMail";
            sqltCommnd.CommandType = CommandType.Text;
            sqltCommnd.Parameters.AddWithValue("@userMail", userName);

            sqltCommnd.CommandText = "update borrowerDetails set brrPass='" + txtbPassword.Text.TrimEnd() + "' where brrId=@brrId";
            sqltCommnd.CommandType = CommandType.Text;
            sqltCommnd.Parameters.AddWithValue("@brrId", userName);

            sqltCommnd.ExecuteNonQuery();
            sqltConn.Close();
            MessageBox.Show("Password change successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        private void FormChangePassword_Load(object sender, EventArgs e)
        {
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            sqltCommnd.CommandText = "select userPassword from userDetails where userMail=@userMail collate nocase";
            sqltCommnd.CommandType = CommandType.Text;
            sqltCommnd.Parameters.AddWithValue("@userMail", userName);
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    userPassword = dataReader["userPassword"].ToString();
                }
                dataReader.Close();
            }
            else
            {
                dataReader.Close();
                sqltCommnd.CommandText = "select brrPass from borrowerDetails where brrId=@brrId collate nocase";
                sqltCommnd.CommandType = CommandType.Text;
                sqltCommnd.Parameters.AddWithValue("@brrId", userName);
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        userPassword = dataReader["brrPass"].ToString();
                    }
                    dataReader.Close();
                }
            }
            dataReader.Close();
            sqltConn.Close();
        }

        private void FormChangePassword_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                    this.DisplayRectangle);
        }
    }
}
