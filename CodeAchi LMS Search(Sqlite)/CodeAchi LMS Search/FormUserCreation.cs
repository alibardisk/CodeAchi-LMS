using Microsoft.Win32;
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
    public partial class FormUserCreation : Form
    {
        public FormUserCreation()
        {
            InitializeComponent();
            txtbBrrId.LostFocus += new EventHandler(txtbBrrId_LostFocus);
        }

        string databasePath = "", hostIp = "", connectionString = "", brrMail = "",brrName="",brrAddress="",brrContact="";

        private void txtbBrrId_LostFocus(object sender, EventArgs e)
        {
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            sqltCommnd.CommandText = "select brrName,brrMailId,brrContact,brrAddress from borrowerDetails where brrId=@brrId collate nocase";
            sqltCommnd.CommandType = CommandType.Text;
            sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    brrName = dataReader["brrName"].ToString();
                    brrAddress = dataReader["brrAddress"].ToString();
                    brrContact = dataReader["brrContact"].ToString();
                    brrMail = dataReader["brrMailId"].ToString();
                }
                dataReader.Close();
                txtbUserId.Text = txtbBrrId.Text;
            }
            else
            {
                brrName = "";
                
            }
            sqltConn.Close();
            txtbBrrId.Select(0, 0);
        }

        private void txtbBrrMail_Enter(object sender, EventArgs e)
        {
            if (brrName == "")
            {
                MessageBox.Show("Borrower doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrId.Focus();
            }
        }

        private void FormUserCreation_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                    this.DisplayRectangle);
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void txtbConfirm_TextChanged(object sender, EventArgs e)
        {
            txtbConfirm.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtbUserId.Text=="")
            {
                MessageBox.Show("Please enter your Id", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrId.Select();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter a password", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }
            if (brrMail.ToLower() != txtbBrrMail.Text.ToLower())
            {
                MessageBox.Show("Email mismatch please contact your librarian.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbBrrMail.Select();
                return;
            }

            if (txtbPassword.Text != txtbConfirm.Text)
            {
                MessageBox.Show("Password mismatch please enter the correct password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbConfirm.Select();
                return;
            }

            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            string queryString = "Select userMail from userDetails where userMail=@userMail";
            sqltCommnd.CommandText = queryString;
            sqltCommnd.Parameters.AddWithValue("@userMail", txtbBrrId.Text);
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            if (dataReader.HasRows)
            {
                MessageBox.Show("Password of this user already registered!"+Environment.NewLine+"Please contact librarian if you have forgot your password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataReader.Close();
                sqltConn.Close();
                return;
            }
            else
            {
                dataReader.Close();
            }

            sqltCommnd = sqltConn.CreateCommand();
            queryString = "insert into userDetails (userName,userDesig,userMail,userContact,userPassword,userAddress,isActive,isAdmin)" +
                " values (@userName,@userDesig,@userMail,'" + brrContact + "',@userPassword,@userAddress,'" + true + "','" + false + "')";
            sqltCommnd.CommandText = queryString;
            sqltCommnd.Parameters.AddWithValue("@userName", brrName);
            sqltCommnd.Parameters.AddWithValue("@userDesig", "Borrower");
            sqltCommnd.Parameters.AddWithValue("@userMail", txtbBrrId.Text);
            sqltCommnd.Parameters.AddWithValue("@userPassword", txtbPassword.Text);
            sqltCommnd.Parameters.AddWithValue("@userAddress", brrAddress);
            sqltCommnd.ExecuteNonQuery();
            sqltConn.Close();
            MessageBox.Show("Password saved successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {
            if(txtbBrrId.Text=="")
            {
                brrName = "";
                brrAddress = "";
                brrContact = "";
                brrMail = "";
                txtbUserId.Text = txtbBrrId.Text;
            }
        }

        private void txtbBrrId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
            }
        }

        private void FormUserCreation_Load(object sender, EventArgs e)
        {
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                object o1 = regKey.GetValue("Data2");
                if (o1 != null)
                {
                    byteData = Convert.FromBase64String(o1.ToString());
                    databasePath = Encoding.UTF8.GetString(byteData);
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
                    txtbBrrId.Select();
                }
            }
        }
    }
}
