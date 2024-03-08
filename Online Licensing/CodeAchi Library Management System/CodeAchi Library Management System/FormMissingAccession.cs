using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormMissingAccession : Form
    {
        public FormMissingAccession()
        {
            InitializeComponent();
        }

        bool stopSearching = false;
        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void FormMissingAccession_Load(object sender, EventArgs e)
        {
            pcbLoad.Visible = false;
            rdbAll.Checked = true;
            dgvAccnList.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;//FromArgb(255, 255, 192);
            dgvAccnList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvAccnList.Rows.Clear();
            pcbLoad.Visible = true;
            Application.DoEvents();
            timer1.Start();
        }

        private void FormMissingAccession_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                          this.DisplayRectangle);
        }

        private void FormMissingAccession_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopSearching = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============================Add all Items======================
                string queryString = "select itemAccession from itemDetails";
                SQLiteDataReader dataReader;
                if (rdbAll.Checked)
                {
                    queryString = "select itemAccession from itemDetails";
                    sqltCommnd.CommandText = queryString;
                    dataReader = sqltCommnd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();
                    //sqltConn.Close();


                    List<int> numberAccession = new List<int> { };
                    List<string> prefixList = new List<string> { };
                    //List<string> prefixAccessionList = new List<string> { };
                    string prefixText = "";
                    foreach (string accesionNo in accessionList)
                    {
                        if(accesionNo!="")
                        {
                            if (stopSearching)
                            {
                                break;
                            }
                            if (accesionNo.All(char.IsDigit) == true)
                            {
                                numberAccession.Add(Convert.ToInt32(accesionNo));
                            }
                            else
                            {
                                if (char.IsDigit(accesionNo[0]) == false)
                                {
                                    prefixText = new string(accesionNo.TakeWhile(c => !char.IsDigit(c)).ToArray());
                                    if (accesionNo.Replace(prefixText, "").All(char.IsDigit) == true)
                                    {
                                        if (prefixList.IndexOf(prefixText) < 0)
                                        {
                                            prefixList.Add(prefixText);
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                        Application.DoEvents();
                    }

                    //===================Numeric Accession===========================
                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {
                            if (stopSearching)
                            {
                                break;
                            }
                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, i.ToString());
                            }
                            Application.DoEvents();
                        }
                    }

                    ///============================================================
                    foreach (string accnPrefix in prefixList)
                    {
                        if (stopSearching)
                        {
                            break;
                        }
                        numberAccession.Clear();
                        foreach (string accesionNo in accessionList)
                        {
                            if (stopSearching)
                            {
                                break;
                            }
                            if (accesionNo.StartsWith(accnPrefix))
                            {
                                if (accesionNo.Replace(accnPrefix, "").All(char.IsDigit) == true)
                                {
                                    try
                                    {
                                        if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(accnPrefix, ""))) < 0)
                                        {
                                            numberAccession.Add(Convert.ToInt32(accesionNo.Replace(accnPrefix, "")));
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            Application.DoEvents();
                        }

                        numberAccession.Sort();
                        if (numberAccession.Count > 0)
                        {
                            for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                            {
                                if (stopSearching)
                                {
                                    break;
                                }
                                if (numberAccession.IndexOf(i) < 0)
                                {
                                    dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, accnPrefix + i.ToString());
                                }
                                Application.DoEvents();
                            }
                        }
                        Application.DoEvents();
                    }
                }
                else if (rdbPrefix.Checked)
                {
                    List<int> numberAccession = new List<int> { };
                    queryString = "select itemAccession from itemDetails where itemAccession like @p1";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@p1", txtbPrefix.Text + "%");
                    dataReader = sqltCommnd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();
                    foreach (string accesionNo in accessionList)
                    {
                        if (accesionNo.Replace(txtbPrefix.Text, "").All(char.IsDigit) == true)
                        {
                            try
                            {
                                if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, ""))) < 0)
                                {
                                    numberAccession.Add(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, "")));
                                }
                            }
                            catch
                            {

                            }
                        }
                        Application.DoEvents();
                    }
                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {

                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, txtbPrefix.Text + i.ToString());
                            }
                            Application.DoEvents();
                        }
                    }
                }
                else if (rdbSuffix.Checked)
                {
                    List<int> numberAccession = new List<int> { };
                    queryString = "select itemAccession from itemDetails where itemAccession like @p1";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@p1", "%" + txtbSuffix.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();
                    foreach (string accesionNo in accessionList)
                    {
                        if (accesionNo.Replace(txtbSuffix.Text, "").All(char.IsDigit) == true)
                        {
                            try
                            {
                                if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, ""))) < 0)
                                {
                                    numberAccession.Add(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, "")));
                                }
                            }
                            catch
                            {

                            }
                        }
                        Application.DoEvents();
                    }

                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {

                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, i.ToString() + txtbSuffix.Text);
                            }
                            Application.DoEvents();
                        }
                    }
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
                MySqlCommand mysqlCmd=null;
                //==============================Add all Items======================
                string queryString = "select itemAccession from item_details";
                MySqlDataReader dataReader=null;
                if (rdbAll.Checked)
                {
                    queryString = "select itemAccession from item_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();

                    List<int> numberAccession = new List<int> { };
                    List<string> prefixList = new List<string> { };
                    string prefixText = "";
                    foreach (string accesionNo in accessionList)
                    {
                        if (stopSearching)
                        {
                            break;
                        }
                        if (accesionNo.All(char.IsDigit) == true)
                        {
                            numberAccession.Add(Convert.ToInt32(accesionNo));
                        }
                        else
                        {
                            if (char.IsDigit(accesionNo[0]) == false)
                            {
                                prefixText = new string(accesionNo.TakeWhile(c => !char.IsDigit(c)).ToArray());
                                if (accesionNo.Replace(prefixText, "").All(char.IsDigit) == true)
                                {
                                    if (prefixList.IndexOf(prefixText) < 0)
                                    {
                                        prefixList.Add(prefixText);
                                    }
                                }
                            }
                            else
                            {

                            }
                        }
                        Application.DoEvents();
                    }

                    //===================Numeric Accession===========================
                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {
                            if (stopSearching)
                            {
                                break;
                            }
                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, i.ToString());
                            }
                            Application.DoEvents();
                        }
                    }

                    ///============================================================
                    foreach (string accnPrefix in prefixList)
                    {
                        if (stopSearching)
                        {
                            break;
                        }
                        numberAccession.Clear();
                        foreach (string accesionNo in accessionList)
                        {
                            if (stopSearching)
                            {
                                break;
                            }
                            if (accesionNo.StartsWith(accnPrefix))
                            {
                                if (accesionNo.Replace(accnPrefix, "").All(char.IsDigit) == true)
                                {
                                    try
                                    {
                                        if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(accnPrefix, ""))) < 0)
                                        {
                                            numberAccession.Add(Convert.ToInt32(accesionNo.Replace(accnPrefix, "")));
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            Application.DoEvents();
                        }

                        numberAccession.Sort();
                        if (numberAccession.Count > 0)
                        {
                            for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                            {
                                if (stopSearching)
                                {
                                    break;
                                }
                                if (numberAccession.IndexOf(i) < 0)
                                {
                                    dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, accnPrefix + i.ToString());
                                }
                                Application.DoEvents();
                            }
                        }
                        Application.DoEvents();
                    }
                }
                else if (rdbPrefix.Checked)
                {
                    List<int> numberAccession = new List<int> { };
                    queryString = "select itemAccession from item_details where itemAccession like @p1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@p1", txtbPrefix.Text + "%");
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();
                    foreach (string accesionNo in accessionList)
                    {
                        if (accesionNo.Replace(txtbPrefix.Text, "").All(char.IsDigit) == true)
                        {
                            try
                            {
                                if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, ""))) < 0)
                                {
                                    numberAccession.Add(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, "")));
                                }
                            }
                            catch
                            {

                            }
                        }
                        Application.DoEvents();
                    }
                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {

                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, txtbPrefix.Text + i.ToString());
                            }
                            Application.DoEvents();
                        }
                    }
                }
                else if (rdbSuffix.Checked)
                {
                    List<int> numberAccession = new List<int> { };
                    queryString = "select itemAccession from item_details where itemAccession like @p1";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@p1", "%" + txtbSuffix.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    List<string> accessionList = new List<string> { };
                    if (dataReader.HasRows)
                    {
                        accessionList = (from IDataRecord r in dataReader
                                         select (string)r["itemAccession"]
                                               ).ToList();
                    }
                    dataReader.Close();
                    foreach (string accesionNo in accessionList)
                    {
                        if (accesionNo.Replace(txtbSuffix.Text, "").All(char.IsDigit) == true)
                        {
                            try
                            {
                                if (numberAccession.IndexOf(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, ""))) < 0)
                                {
                                    numberAccession.Add(Convert.ToInt32(accesionNo.Replace(txtbPrefix.Text, "")));
                                }
                            }
                            catch
                            {

                            }
                        }
                        Application.DoEvents();
                    }

                    numberAccession.Sort();
                    if (numberAccession.Count > 0)
                    {
                        for (int i = numberAccession[0]; i <= numberAccession[numberAccession.Count - 1]; i++)
                        {

                            if (numberAccession.IndexOf(i) < 0)
                            {
                                dgvAccnList.Rows.Add(dgvAccnList.Rows.Count + 1, i.ToString() + txtbSuffix.Text);
                            }
                            Application.DoEvents();
                        }
                    }
                }
                mysqlConn.Close();
            }
            pcbLoad.Visible = false;
            dgvAccnList.ClearSelection();
            lblTtlRecord.Text = dgvAccnList.Rows.Count.ToString();
        }
    }
}
