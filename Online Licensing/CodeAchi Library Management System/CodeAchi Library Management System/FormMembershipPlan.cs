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
    public partial class FormMembershipPlan : Form
    {
        public FormMembershipPlan()
        {
            InitializeComponent();
        }

        string planName = "";

        private void FormMembershipPlan_Load(object sender, EventArgs e)
        {
            cmbFrqncy.SelectedIndex = 0;
            dgvPlan.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvPlan.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from mbershipSetting";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        dgvPlan.Rows.Add(dgvPlan.Rows.Count + 1, dataReader["membrshpName"].ToString(), dataReader["membrDurtn"].ToString(),
                               dataReader["membrFees"].ToString(), dataReader["issueLimit"].ToString());
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
                    string queryString = "select * from mbership_setting";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvPlan.Rows.Add(dgvPlan.Rows.Count + 1, dataReader["membrshpName"].ToString(), dataReader["membrDurtn"].ToString(),
                                   dataReader["membrFees"].ToString(), dataReader["issueLimit"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            dgvPlan.ClearSelection();
        }

        private void FormMembershipPlan_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
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

        private void txtbFees_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtbFees_Leave(object sender, EventArgs e)
        {
            if (txtbFees.Text == "")
            {
                txtbFees.Text = 0.00.ToString("0.00");
            }
        }

        private void txtbMaxitems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtbMaxitems_Leave(object sender, EventArgs e)
        {
            if (txtbMaxitems.Text == "")
            {
                txtbMaxitems.Text = 0.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtbName.Text == "")
            {
                MessageBox.Show("Please enter a plan name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbName.Select();
                return;
            }
            if (cmbFrqncy.SelectedIndex==0)
            {
                MessageBox.Show("Please select a plan frequency.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbFrqncy.Select();
                return;
            }
            if (txtbMaxitems.Text == "0")
            {
                MessageBox.Show("Please enter the issue limit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbName.Select();
                return;
            }
            string planValidity = cmbFrqncy.Text.Substring(0, cmbFrqncy.Text.IndexOf("_"));
            
            string queryString = "";
            if (btnSave.Text == "Save")
            {
                DataGridViewRow[] dgvCheckedRows = dgvPlan.Rows.OfType<DataGridViewRow>().Where(x => (string)x.Cells[1].Value == txtbName.Text).ToArray<DataGridViewRow>();
                if (dgvCheckedRows.Count() != 0)
                {
                    MessageBox.Show("Plan already exist.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtbName.SelectAll();
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
                    queryString = "insert into mbershipSetting (membrshpName,planFreqncy,membrDurtn,membrFees,issueLimit,planDesc)" +
                    " values (@membrshpName,'" + cmbFrqncy.Text + "','" + planValidity + "','" + txtbFees.Text + "','" + txtbMaxitems.Text + "',@planDesc)";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@membrshpName", txtbName.Text);
                    sqltCommnd.Parameters.AddWithValue("@planDesc", txtbDesc.Text);
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
                    queryString = "insert into mbership_setting (membrshpName,planFreqncy,membrDurtn,membrFees,issueLimit,planDesc)" +
                   " values (@membrshpName,'" + cmbFrqncy.Text + "','" + planValidity + "','" + txtbFees.Text + "','" + txtbMaxitems.Text + "',@planDesc)";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@membrshpName", txtbName.Text);
                    mysqlCmd.Parameters.AddWithValue("@planDesc", txtbDesc.Text);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                globalVarLms.backupRequired = true;
                dgvPlan.Rows.Add(dgvPlan.Rows.Count + 1, txtbName.Text, planValidity, txtbFees.Text, txtbMaxitems.Text);
                MessageBox.Show("Plan added successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    queryString = "update mbershipSetting set membrshpName=:membrshpName, planFreqncy='" + cmbFrqncy.Text + "',membrDurtn='" + planValidity + "',membrFees='" + txtbFees.Text + "',issueLimit='" + txtbMaxitems.Text + "',planDesc=:planDesc where membrshpName=@membrshpName";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@membrshpName", planName);
                    sqltCommnd.Parameters.AddWithValue("membrshpName", txtbName.Text);
                    sqltCommnd.Parameters.AddWithValue("planDesc", txtbDesc.Text);
                    sqltCommnd.ExecuteNonQuery();
                    
                    sqltCommnd.CommandText = "update borrowerDetails set memPlan=:memPlan where memPlan=@memPlan";
                    sqltCommnd.Parameters.AddWithValue("@memPlan", planName);
                    sqltCommnd.Parameters.AddWithValue("memPlan", txtbName.Text);
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
                    queryString = "update mbership_setting set membrshpName=@membrshpName1, planFreqncy='" + cmbFrqncy.Text + "',membrDurtn='" + planValidity + "',membrFees='" + txtbFees.Text + "',issueLimit='" + txtbMaxitems.Text + "',planDesc=@planDesc where membrshpName=@membrshpName";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@membrshpName", planName);
                    mysqlCmd.Parameters.AddWithValue("@membrshpName1", txtbName.Text);
                    mysqlCmd.Parameters.AddWithValue("@planDesc", txtbDesc.Text);
                    mysqlCmd.ExecuteNonQuery();

                    queryString = "update borrower_details set memPlan=@memPlan1 where memPlan=@memPlan";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@memPlan", planName);
                    mysqlCmd.Parameters.AddWithValue("@memPlan1", txtbName.Text);
                    mysqlCmd.ExecuteNonQuery();
                    mysqlConn.Close();
                }
                dgvPlan.SelectedRows[0].Cells[2].Value = planValidity;
                dgvPlan.SelectedRows[0].Cells[3].Value = txtbFees.Text;
                dgvPlan.SelectedRows[0].Cells[4].Value = txtbMaxitems.Text;
                globalVarLms.backupRequired = true;
                MessageBox.Show("Plan updated successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            btnReset_Click(null, null);
            dgvPlan.ClearSelection();
            Application.DoEvents();
        }

        private void dgvPlan_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvPlan.HitTest(e.X, e.Y);
                dgvPlan.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvPlan.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvPlan, new Point(e.X, e.Y));
                }
            }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select * from mbershipSetting where membrshpName=@membrshpName";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@membrshpName", dgvPlan.SelectedRows[0].Cells[1].Value.ToString());
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbName.Text = dataReader["membrshpName"].ToString();
                        cmbFrqncy.Text = dataReader["planFreqncy"].ToString();
                        txtbFees.Text = dataReader["membrFees"].ToString();
                        txtbMaxitems.Text = dataReader["issueLimit"].ToString();
                        txtbDesc.Text = dataReader["planDesc"].ToString();
                        planName = txtbName.Text;
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
                string queryString = "select * from mbership_setting where membrshpName=@membrshpName";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.Parameters.AddWithValue("@membrshpName", dgvPlan.SelectedRows[0].Cells[1].Value.ToString());
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        txtbName.Text = dataReader["membrshpName"].ToString();
                        cmbFrqncy.Text = dataReader["planFreqncy"].ToString();
                        txtbFees.Text = dataReader["membrFees"].ToString();
                        txtbMaxitems.Text = dataReader["issueLimit"].ToString();
                        txtbDesc.Text = dataReader["planDesc"].ToString();
                        planName = txtbName.Text;
                    }
                    btnSave.Text = "Update";
                }
                dataReader.Close();
                mysqlConn.Close();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtbName.ReadOnly = false;
            btnSave.Text = "Save";
            txtbName.Clear();
            txtbFees.Text = 0.ToString();
            cmbFrqncy.SelectedIndex = 0;
            txtbMaxitems.Text = 0.ToString();
            txtbName.Focus();
        }
    }
}
