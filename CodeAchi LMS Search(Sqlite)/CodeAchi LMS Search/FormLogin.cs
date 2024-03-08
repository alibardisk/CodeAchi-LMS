using Microsoft.Win32;
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

namespace CodeAchi_LMS_Search
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        string databasePath = "", hostIp = "", connectionString = "";

        private void FormLogin_Load(object sender, EventArgs e)
        {
            lblCompany1.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
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
                    txtbMailId.Select();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtbMailId.Text == "")
            {
                MessageBox.Show("Please enter your userid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter your password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            if (sqltConn.State == ConnectionState.Closed)
            {
                sqltConn.Open();
            }
            SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
            string queryString = "select userName,userPassword,userImage,userPriviledge,isActive,isAdmin,userContact,userDesig from userDetails where userMail=@userMail";
            sqltCommnd.CommandText = queryString;
            sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text.TrimEnd());
            SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
            string userName = "", userPassword = "", userPriviledge = "";
            bool isActive = false, isAdmin = false;
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    userName = dataReader["userName"].ToString();
                    userPassword = dataReader["userPassword"].ToString();
                    isActive = Convert.ToBoolean(dataReader["isActive"].ToString());
                    isAdmin = Convert.ToBoolean(dataReader["isAdmin"].ToString());
                    userPriviledge = dataReader["userPriviledge"].ToString();
                    lblDesignation.Text= dataReader["userDesig"].ToString();
                }
            }
            else
            {
                dataReader.Close();
                queryString = "select * from borrowerDetails where brrId=@brrId";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbMailId.Text.TrimEnd());
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        userName = dataReader["brrName"].ToString();
                        userPassword = dataReader["brrPass"].ToString();
                        isActive = true;
                        isAdmin = false;
                        userPriviledge = "IR";
                        lblDesignation.Text = "Borrower";
                    }
                }
                else
                {
                    MessageBox.Show("Userid doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbMailId.Select();
                    return;
                }
            }
            dataReader.Close();
            
            if (!isActive)
            {
                MessageBox.Show("Userid doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            dataReader.Close();
            sqltConn.Close();
            if (txtbPassword.Text == userPassword)
            {
                this.Hide();
            }
            else
            {
                MessageBox.Show("Your password is wrong.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }
        }

        private void txtbMailId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(null, null);
            }
        }

        private void txtbMailId_Enter(object sender, EventArgs e)
        {
            txtbMailId.SelectAll();
        }

        private void txtbPassword_Enter(object sender, EventArgs e)
        {
            txtbPassword.SelectAll();
        }

        private void txtbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
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

        private void FormLogin_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                     this.DisplayRectangle);
        }

        private void lblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormUserCreation createUser = new FormUserCreation();
            createUser.ShowDialog();
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void txtbMailId_TextChanged(object sender, EventArgs e)
        {
            if (txtbMailId.Text != "")
            {
                SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select userImage from userDetails where userMail=@userMail";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            pcbUsrImage.BorderStyle = BorderStyle.Fixed3D;
                            byte[] imageBytes = Convert.FromBase64String(dataReader["userImage"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            pcbUsrImage.Image = System.Drawing.Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            pcbUsrImage.BorderStyle = BorderStyle.None;
                            pcbUsrImage.Image = Properties.Resources.blankBrrImage;
                        }
                    }
                }
                else
                {
                    pcbUsrImage.BorderStyle = BorderStyle.None;
                    pcbUsrImage.Image = Properties.Resources.blankBrrImage;
                }
                sqltConn.Close();
            }
            else
            {
                pcbUsrImage.BorderStyle = BorderStyle.None;
                pcbUsrImage.Image = Properties.Resources.blankBrrImage;
            }
        }
    }
}
