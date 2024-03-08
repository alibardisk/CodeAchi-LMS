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
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                object o1 = regKey.GetValue("Data2");
                if (o1 == null)
                {
                    btnUpdate.Text = "Save";
                    chkbServerIp.Checked = false;
                    txtbIp.Enabled = false;
                }
                else
                {
                    byteData = Convert.FromBase64String(o1.ToString());
                    txtbDatabasePath.Text = Encoding.UTF8.GetString(byteData);
                    o1 = regKey.GetValue("Data3");
                    if (o1 != null)
                    {
                        byteData = Convert.FromBase64String(o1.ToString());
                        if (Encoding.UTF8.GetString(byteData) == "Local")
                        {
                            chkbServerIp.Checked = false;
                        }
                        else
                        {
                            txtbIp.Text = Encoding.UTF8.GetString(byteData);
                            chkbServerIp.Checked = true;
                        }
                    }
                    else
                    {
                        chkbServerIp.Checked = false;
                    }
                }
            }
        }

        private void FormSetting_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        private void btnBrowsePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            browseFolder.Description = Application.ProductName + " select database path.";
            if (browseFolder.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(browseFolder.SelectedPath + @"\LMS.sl3"))
                {
                    txtbDatabasePath.Text = browseFolder.SelectedPath;
                }
                else
                {
                    MessageBox.Show("No databse found in the selected location.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (chkbServerIp.Checked)
            {
                if (txtbIp.Text == "")
                {
                    MessageBox.Show("Please enter the server ip.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbIp.Select();
                    return;
                }
            }
            if (txtbDatabasePath.Text == "")
            {
                MessageBox.Show("Please enter the database path.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string databasePath = txtbDatabasePath.Text.TrimEnd();
            if (chkbServerIp.Checked)
            {
                var hostName = databasePath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                databasePath = databasePath.Replace(hostName, txtbIp.Text).Replace("\\", "/");
            }
            string connectionString = @"Data Source="+databasePath+"/LMS.sl3;Version=3;Password=codeachi@lmssl;";
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString);
            try
            {
                sqltConn.Open();
            }
            catch
            {

            }
            if (sqltConn.State.ToString() == "Closed")
            {
                MessageBox.Show("Connection not open.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Connection opened.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqltConn.Close();
                btnUpdate.Enabled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (chkbServerIp.Checked)
            {
                if (txtbIp.Text == "")
                {
                    MessageBox.Show("Please enter the server ip.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbIp.Select();
                    return;
                }
            }
            if (txtbDatabasePath.Text == "")
            {
                MessageBox.Show("Please enter the database path.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                byteData = Encoding.UTF8.GetBytes(txtbDatabasePath.Text);
                regKey.SetValue("Data2", Convert.ToBase64String(byteData));

                if (chkbServerIp.Checked)
                {
                    byteData = Encoding.UTF8.GetBytes(txtbIp.Text);
                    regKey.SetValue("Data3", Convert.ToBase64String(byteData));
                }
                else
                {
                    byteData = Encoding.UTF8.GetBytes("Local");
                    regKey.SetValue("Data3", Convert.ToBase64String(byteData));
                }

                MessageBox.Show("Database save successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
        }

        private void FormSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
                byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                if (regKey != null)
                {
                    object o1 = regKey.GetValue("Data2");
                    if (o1 == null)
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
            }
        }

        private void btnUpdate_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnUpdate.Enabled == true)
            {
                btnUpdate.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnUpdate.BackColor = Color.DimGray;
            }
        }

        private void chkbServerIp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbServerIp.Checked == true)
            {
                txtbIp.Enabled = true;
            }
            else
            {
                txtbIp.Clear();
                txtbIp.Enabled = false;
            }
        }
    }
}
