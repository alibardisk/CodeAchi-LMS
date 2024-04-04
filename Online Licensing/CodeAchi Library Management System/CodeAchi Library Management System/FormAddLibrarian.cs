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
    public partial class FormAddLibrarian : Form
    {
        public FormAddLibrarian()
        {
            InitializeComponent();
        }

        private void FormAdminSetting_Load(object sender, EventArgs e)
        {
            chkbActive.Checked = true;
            dgvUserList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvUserList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select userName,userDesig,userMail,userContact,isActive,userPassword from userDetails where userMail !=@userMail";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");//
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (dataReader["isActive"].ToString() == "True")
                        {
                            dgvUserList.Rows.Add(dataReader["userName"].ToString(), dataReader["userDesig"].ToString(),
                                dataReader["userContact"].ToString(), dataReader["userMail"].ToString(), "Active");
                        }
                        else
                        {
                            dgvUserList.Rows.Add(dataReader["userName"].ToString(), dataReader["userDesig"].ToString(),
                               dataReader["userContact"].ToString(), dataReader["userMail"].ToString(), "Blocked");
                        }
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
                    string queryString = "select userName,userDesig,userMail,userContact,isActive,userPassword from user_details where userMail !=@userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", "lmssl@codeachi.com");//
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (dataReader["isActive"].ToString() == "True")
                            {
                                dgvUserList.Rows.Add(dataReader["userName"].ToString(), dataReader["userDesig"].ToString(),
                                    dataReader["userContact"].ToString(), dataReader["userMail"].ToString(), "Active");
                            }
                            else
                            {
                                dgvUserList.Rows.Add(dataReader["userName"].ToString(), dataReader["userDesig"].ToString(),
                                   dataReader["userContact"].ToString(), dataReader["userMail"].ToString(), "Blocked");
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            dgvUserList.ClearSelection();
        }

        private void FormAdminSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void txtbContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            globalVarLms.bimapImage = Properties.Resources.blankBrrImage;
            FormPicture takePicture = new FormPicture();
            takePicture.ShowDialog();
            pcbUser.Image = globalVarLms.bimapImage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNotification.Visible = !lblNotification.Visible;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtbName.Text=="")
            {
                MessageBox.Show("Please enter a user name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbName.Select();
                return;
            }
            if (txtbDesignation.Text == "")
            {
                MessageBox.Show("Please enter the designation.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbDesignation.Select();
                return;
            }
            if (txtbMailId.Text == "")
            {
                MessageBox.Show("Please enter a mail id.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbMailId.Select();
                return;
            }
            if (txtbContact.Text == "")
            {
                MessageBox.Show("Please enter the contact no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbContact.Select();
                return;
            }
            if (txtbPassword.Text == "")
            {
                MessageBox.Show("Please enter a password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbPassword.Select();
                return;
            }
            if (txtbAddress.Text == "")
            {
                MessageBox.Show("Please enter the address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbAddress.Select();
                return;
            }

            String base64String = "base64String";
            if (pcbUser.Image != Properties.Resources.blankBrrImage && pcbUser.Image != Properties.Resources.uploadingFail)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pcbUser.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = memoryStream.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            bool isActive = false;
            if(chkbActive.Checked)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
            string userPrivilg = "";
            if(chkbEntryItems.Checked)
            {
                if(userPrivilg=="")
                {
                    userPrivilg = "EI";
                }
                else
                {
                    userPrivilg = userPrivilg+ "$EI";
                }
            }
            if(chkbEntryMember.Checked)
            {
                if (userPrivilg == "")
                {
                    userPrivilg = "EM";
                }
                else
                {
                    userPrivilg = userPrivilg + "$EM";
                }
            }
            if(chkbIssueReturn.Checked)
            {
                if (userPrivilg == "")
                {
                    userPrivilg = "IR";
                }
                else
                {
                    userPrivilg = userPrivilg + "$IR";
                }
            }
            if(chkbRemit.Checked)
            {
                if (userPrivilg == "")
                {
                    userPrivilg = "RP";
                }
                else
                {
                    userPrivilg = userPrivilg + "$RP";
                }
            }
           
            string queryString = "";
            if (btnAdd.Text=="Add")
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    queryString = "Select userMail from userDetails where userMail=@userMail";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        MessageBox.Show("User id already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbName.SelectAll();
                        dataReader.Close();
                        return;
                    }
                    else
                    {
                        dataReader.Close();
                    }
                    sqltCommnd = sqltConn.CreateCommand();
                    queryString = "insert into userDetails (userName,userDesig,userMail,userContact,userPassword,userAddress,userPriviledge,isActive,isAdmin,userImage)" +
                        " values (@userName,@userDesig,@userMail,'" + txtbContact.Text + "',@userPassword,@userAddress,'" + userPrivilg + "','" + isActive + "','" + false + "','" + base64String + "')";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@userName", txtbName.Text);
                    sqltCommnd.Parameters.AddWithValue("@userDesig", txtbDesignation.Text);
                    sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    sqltCommnd.Parameters.AddWithValue("@userPassword", txtbPassword.Text);
                    sqltCommnd.Parameters.AddWithValue("@userAddress", txtbAddress.Text);
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
                    queryString = "Select userMail from user_details where userMail=@userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        MessageBox.Show("User id already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtbName.SelectAll();
                        dataReader.Close();
                        return;
                    }
                    else
                    {
                        dataReader.Close();
                    }
                    
                    queryString = "insert into user_details (userName,userDesig,userMail,userContact,userPassword,userAddress,userPriviledge,isActive,isAdmin,userImage)" +
                        " values (@userName,@userDesig,@userMail,'" + txtbContact.Text + "',@userPassword,@userAddress,'" + userPrivilg + "','" + isActive + "','" + false + "','" + base64String + "')";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userName", txtbName.Text);
                    mysqlCmd.Parameters.AddWithValue("@userDesig", txtbDesignation.Text);
                    mysqlCmd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    mysqlCmd.Parameters.AddWithValue("@userPassword", txtbPassword.Text);
                    mysqlCmd.Parameters.AddWithValue("@userAddress", txtbAddress.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                if (chkbActive.Checked)
                {
                    dgvUserList.Rows.Add(txtbName.Text, txtbDesignation.Text, txtbContact.Text, txtbMailId.Text,"Active");
                }
                else
                {
                    dgvUserList.Rows.Add(txtbName.Text, txtbDesignation.Text, txtbContact.Text, txtbMailId.Text,"Blocked");
                }
                dgvUserList.ClearSelection();
                clesrField();
                MessageBox.Show("User added successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    queryString = "update userDetails set userDesig=:userDesig,userName=:userName" +
                        ",userContact='" + txtbContact.Text + "',userPassword=:userPassword" +
                        ",userAddress=:userAddress,userPriviledge='" + userPrivilg + "',isActive='" + isActive + "'" +
                        ",userImage='" + base64String + "' where userMail=@userMail";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    sqltCommnd.Parameters.AddWithValue("userName", txtbName.Text);
                    sqltCommnd.Parameters.AddWithValue("userDesig", txtbDesignation.Text);
                    sqltCommnd.Parameters.AddWithValue("userName", txtbMailId.Text);
                    sqltCommnd.Parameters.AddWithValue("userPassword", txtbPassword.Text);
                    sqltCommnd.Parameters.AddWithValue("userAddress", txtbAddress.Text);
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
                    queryString = "update user_details set userDesig=@userDesig,userName=@userName" +
                       ",userContact='" + txtbContact.Text + "',userPassword=@userPassword" +
                       ",userAddress=@userAddress,userPriviledge='" + userPrivilg + "',isActive='" + isActive + "'" +
                       ",userImage='" + base64String + "' where userMail=@userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", txtbMailId.Text);
                    mysqlCmd.Parameters.AddWithValue("@userName", txtbName.Text);
                    mysqlCmd.Parameters.AddWithValue("@userDesig", txtbDesignation.Text);
                    mysqlCmd.Parameters.AddWithValue("@userPassword", txtbPassword.Text);
                    mysqlCmd.Parameters.AddWithValue("@userAddress", txtbAddress.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                dgvUserList.SelectedRows[0].Cells[1].Value = txtbDesignation.Text;
                dgvUserList.SelectedRows[0].Cells[2].Value = txtbContact.Text;
                dgvUserList.SelectedRows[0].Cells[0].Value = txtbName.Text;
                if (isActive)
                {
                    dgvUserList.SelectedRows[0].Cells[4].Value = "Active";
                }
                else
                {
                    dgvUserList.SelectedRows[0].Cells[4].Value = "Blocked";
                }
                dgvUserList.ClearSelection();
                clesrField();
                btnAdd.Text = "Add";
                txtbMailId.Enabled = true;
                MessageBox.Show("User updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            if(chkbAll.Checked)
            {
                chkbEntryItems.Checked = true;
                chkbEntryMember.Checked = true;
                chkbIssueReturn.Checked = true;
                chkbRemit.Checked = true;
            }
            else
            {
                chkbEntryItems.Checked = false;
                chkbEntryMember.Checked = false;
                chkbIssueReturn.Checked = false;
                chkbRemit.Checked = false;
            }
        }

        private void clesrField()
        {
            txtbName.Clear();
            txtbAddress.Clear();
            txtbContact.Clear();
            txtbDesignation.Clear();
            txtbMailId.Clear();
            txtbPassword.Clear();
            chkbAll.Checked = false;
            chkbEntryItems.Checked = false;
            chkbEntryMember.Checked = false;
            chkbIssueReturn.Checked = false;
            chkbRemit.Checked = false;
            chkbActive.Checked = true;
            pcbUser.Image = Properties.Resources.blankBrrImage;
        }

        private void dgvUserList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvUserList.HitTest(e.X, e.Y);
                dgvUserList.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvUserList.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvUserList, new Point(e.X, e.Y));
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUserList.SelectedRows.Count == 1)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select * from userDetails where userMail=@userMail";
                    sqltCommnd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                    sqltCommnd.CommandText = queryString;
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbMailId.Enabled = false;
                            txtbName.Text = dataReader["userName"].ToString();
                            txtbDesignation.Text = dataReader["userDesig"].ToString();
                            txtbContact.Text = dataReader["userContact"].ToString();
                            txtbMailId.Text = dataReader["userMail"].ToString();
                            txtbPassword.Text = dataReader["userPassword"].ToString();
                            txtbAddress.Text = dataReader["userAddress"].ToString();
                            if (dataReader["isActive"].ToString() == "True")
                            {
                                chkbActive.Checked = true;
                            }
                            else
                            {
                                chkbActive.Checked = false;
                            }

                            string[] prevList = dataReader["userPriviledge"].ToString().Split('$');
                            foreach (string userPrev in prevList)
                            {
                                if (userPrev == "EM")
                                {
                                    chkbEntryMember.Checked = true;
                                }
                                if (userPrev == "EI")
                                {
                                    chkbEntryItems.Checked = true;
                                }
                                if (userPrev == "IR")
                                {
                                    chkbIssueReturn.Checked = true;
                                }
                                if (userPrev == "RP")
                                {
                                    chkbRemit.Checked = true;
                                }
                            }

                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["userImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbUser.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbUser.Image = Properties.Resources.blankBrrImage;
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
                    string queryString = "select * from user_details where userMail=@userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbMailId.Enabled = false;
                            txtbName.Text = dataReader["userName"].ToString();
                            txtbDesignation.Text = dataReader["userDesig"].ToString();
                            txtbContact.Text = dataReader["userContact"].ToString();
                            txtbMailId.Text = dataReader["userMail"].ToString();
                            txtbPassword.Text = dataReader["userPassword"].ToString();
                            txtbAddress.Text = dataReader["userAddress"].ToString();
                            if (dataReader["isActive"].ToString() == "True")
                            {
                                chkbActive.Checked = true;
                            }
                            else
                            {
                                chkbActive.Checked = false;
                            }

                            string[] prevList = dataReader["userPriviledge"].ToString().Split('$');
                            foreach (string userPrev in prevList)
                            {
                                if (userPrev == "EM")
                                {
                                    chkbEntryMember.Checked = true;
                                }
                                if (userPrev == "EI")
                                {
                                    chkbEntryItems.Checked = true;
                                }
                                if (userPrev == "IR")
                                {
                                    chkbIssueReturn.Checked = true;
                                }
                                if (userPrev == "RP")
                                {
                                    chkbRemit.Checked = true;
                                }
                            }

                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["userImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbUser.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbUser.Image = Properties.Resources.blankBrrImage;
                            }
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                btnAdd.Text = "Update";
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clesrField();
            txtbName.Enabled = true;
            btnAdd.Text = "Add";
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUserList.Rows.Count > 0)
            {
                if (globalVarLms.sqliteData)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    sqltCommnd.CommandText = "select isAdmin from userDetails where userMail= @userMail";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (Convert.ToBoolean(dataReader["isAdmin"].ToString()))
                            {
                                dataReader.Close();
                                MessageBox.Show("You can't delete an admin.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                    dataReader.Close();
                    string queryString = "";
                    if (MessageBox.Show("Are youe sure want to delete?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        queryString = "delete from userDetails where userMail= @userMail";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                        globalVarLms.backupRequired = true;
                        dgvUserList.Rows.RemoveAt(dgvUserList.SelectedRows[0].Index);
                        MessageBox.Show("User deleted Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string queryString = "select isAdmin from user_details where userMail= @userMail";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            if (Convert.ToBoolean(dataReader["isAdmin"].ToString()))
                            {
                                dataReader.Close();
                                mysqlConn.Close();
                                MessageBox.Show("You can't delete an admin.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        dataReader.Close();
                    }
                    else
                    {
                        dataReader.Close();
                    }

                    if (MessageBox.Show("Are youe sure want to delete?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        queryString = "delete from user_details where userMail= @userMail";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@userMail", dgvUserList.SelectedRows[0].Cells[3].Value.ToString());
                        mysqlCmd.CommandTimeout = 99999;
                        mysqlCmd.ExecuteNonQuery();
                        globalVarLms.backupRequired = true;
                        dgvUserList.Rows.RemoveAt(dgvUserList.SelectedRows[0].Index);
                        MessageBox.Show("User deleted Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    mysqlConn.Close();
                }
                dgvUserList.ClearSelection();
                clesrField();
            }
        }

        private void txtbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbPassword_TextChanged(object sender, EventArgs e)
        {
            txtbPassword.UseSystemPasswordChar = true;
        }

        private void updateToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            updateToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void deleteToolStripMenuItem_MouseLeave(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.FromArgb(76, 82, 90);
            deleteToolStripMenuItem.ForeColor = Color.WhiteSmoke;
        }

        private void updateToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            updateToolStripMenuItem.BackColor = Color.WhiteSmoke;
            updateToolStripMenuItem.ForeColor = Color.Black;
        }

        private void deleteToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            deleteToolStripMenuItem.BackColor = Color.WhiteSmoke;
            deleteToolStripMenuItem.ForeColor = Color.Black;
        }

        private void lnkLblFingerPrint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("No fingerprint scanner detected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
