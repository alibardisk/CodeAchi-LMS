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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormLogin : Form
    {
        public ToolTip tooTip = new ToolTip();
        MySqlConnection mysqlConn;

        public FormLogin()
        {
            InitializeComponent();

            tooTip.OwnerDraw = true;
            tooTip.Draw += new DrawToolTipEventHandler(tooTip_Draw);
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            txtbMailId.Text = Properties.Settings.Default.currentUserId;
            if (txtbMailId.Text == "")
            {
                txtbMailId.Select();
            }
            else
            {
                txtbPassword.Select();
            }
            lblCompany1.Text = "© 2012-" + DateTime.Now.Year.ToString() + " | Developed by CodeAchi Technologies Pvt. Ltd.";
            pcbHide.Image = Properties.Resources.show_password;
            pcbHide.Name = "Show";
            txtbPassword.UseSystemPasswordChar = true;
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void txtbMailId_TextChanged(object sender, EventArgs e)
        {
            if (txtbMailId.Text != "")
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
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
                    mysqlConn = ConnectionClass.mysqlConnection();
                    if (mysqlConn.State == ConnectionState.Closed)
                    {
                        try
                        {
                            mysqlConn.Open();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    MySqlCommand mysqlCmd;
                    string queryString = "select userImage from user_details where userMail=@userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
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
                    dataReader.Close();
                    mysqlConn.Close();
                }
            }
            else
            {
                pcbUsrImage.BorderStyle = BorderStyle.None;
                pcbUsrImage.Image = Properties.Resources.blankBrrImage;
            }
        }

        void tooTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Segoe UI", 9.0f);
            Brush b = new SolidBrush(Color.WhiteSmoke);
            e.Graphics.FillRectangle(b, e.Bounds);
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.Red, new PointF(2, 2));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(txtbMailId.Text=="")
            {
                MessageBox.Show("Please enter your userid.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter your password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }

            string userName = "", userPassword = "", userPriviledge = "";
            bool isActive = false, isAdmin = false;
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select userName,userPassword,userImage,userPriviledge,isActive,isAdmin,userContact from userDetails where userMail=@userMail";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text.TrimEnd());
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        userName = dataReader["userName"].ToString();
                        userPassword = dataReader["userPassword"].ToString();
                        isActive = Convert.ToBoolean(dataReader["isActive"].ToString());
                        isAdmin = Convert.ToBoolean(dataReader["isAdmin"].ToString());
                        userPriviledge = dataReader["userPriviledge"].ToString();
                        globalVarLms.userContact = dataReader["userContact"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Userid doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbMailId.Select();
                    return;
                }
                dataReader.Close();
                sqltConn.Close();
            }
            else
            {
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
                string queryString = "select userName,userPassword,userImage,userPriviledge,isActive,isAdmin,userContact from user_details where userMail=@userMail";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        userName = dataReader["userName"].ToString();
                        userPassword = dataReader["userPassword"].ToString();
                        isActive = Convert.ToBoolean(dataReader["isActive"].ToString());
                        isAdmin = Convert.ToBoolean(dataReader["isAdmin"].ToString());
                        userPriviledge = dataReader["userPriviledge"].ToString();
                        globalVarLms.userContact = dataReader["userContact"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Userid doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbMailId.Select();
                    return;
                }
                dataReader.Close();
                mysqlConn.Close();
            }
            if(!isActive)
            {
                MessageBox.Show("Userid doesn't exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            
            if(txtbPassword.Text==userPassword)
            {
                globalVarLms.currentUserName = userName;
                globalVarLms.currentPassword = userPassword;
                globalVarLms.isAdmin = isAdmin;
                globalVarLms.priviledgeList = userPriviledge.Split('&').ToList();
                Properties.Settings.Default.currentUserId = txtbMailId.Text.TrimEnd();
                globalVarLms.currentUserId = txtbMailId.Text.TrimEnd();
                Properties.Settings.Default.Save();
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
            if(e.KeyCode==Keys.Enter)
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

        private void txtbMailId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void lblRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormAbout aboutForm = new FormAbout();
            aboutForm.ShowDialog();
        }

        private void pcbHide_Click(object sender, EventArgs e)
        {
            if(pcbHide.Name=="Show")
            {
                txtbPassword.UseSystemPasswordChar = false;
                pcbHide.Image = Properties.Resources.hide_password;
                pcbHide.Name = "Hide";
                Application.DoEvents();
            }
            else
            {
                txtbPassword.UseSystemPasswordChar = true;
                pcbHide.Image = Properties.Resources.show_password;
                pcbHide.Name = "Show";
                Application.DoEvents();
            }
        }
    }
}
