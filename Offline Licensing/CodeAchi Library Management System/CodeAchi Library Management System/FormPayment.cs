using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormPayment : Form
    {
        public FormPayment()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection autoCollData = new AutoCompleteStringCollection();

        private void FormPayment_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void FormPayment_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            dgvPaymentDetails.RowsDefaultCellStyle.BackColor = Color.AntiqueWhite;
            dgvPaymentDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //==============Borrower Id add to autocomplete=============
                string queryString = "select [brrId] from borrowerDetails";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    autoCollData.Clear();
                    while (dataReader.Read())
                    {
                        autoCollData.Add(dataReader["brrId"].ToString());
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
                    string queryString = "select brrId from borrower_details";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        autoCollData.Clear();
                        while (dataReader.Read())
                        {
                            autoCollData.Add(dataReader["brrId"].ToString());
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }

            txtbBrrId.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtbBrrId.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtbBrrId.AutoCompleteCustomSource = autoCollData;
            label6.Text = globalVarLms.currSymbol;
            label11.Text = globalVarLms.currSymbol;
            label12.Text = globalVarLms.currSymbol;
            label13.Text = globalVarLms.currSymbol;
            cmbPaymentMode.SelectedIndex = 0;
            loadFieldValue();
        }

        private void loadFieldValue()
        {
            if (FieldSettings.Default.itemEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.itemEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        if (fieldName == "lblAccession")
                        {
                            dgvPaymentDetails.Columns[2].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                    }
                }
            }

            if (FieldSettings.Default.borrowerEntry != "")
            {
                string fieldName = "";
                string[] valueList = FieldSettings.Default.borrowerEntry.Split('|');
                foreach (string fieldValue in valueList)
                {
                    if (fieldValue != "" && fieldValue != null)
                    {
                        fieldName = fieldValue.Substring(0, fieldValue.IndexOf("="));
                        if (fieldName == "lblBrId")
                        {
                            lblBrId.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrName")
                        {
                            lblBrName.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblBrAddress")
                        {
                            lblBrAddress.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }

        private void txtbBrrId_TextChanged(object sender, EventArgs e)
        {
            if(txtbBrrId.Text!="")
            {
                if (Properties.Settings.Default.sqliteDatabase)
                {
                    SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                    if (sqltConn.State == ConnectionState.Closed)
                    {
                        sqltConn.Open();
                    }
                    SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                    string queryString = "select brrName,brrAddress,brrImage from borrowerDetails where brrId=@brrId";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            txtbBrrName.Text = dataReader["brrName"].ToString();
                            txtbBrrAddress.Text = dataReader["brrAddress"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                pcbBrrImage.Image = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                            }
                        }
                    }
                    dataReader.Close();
                    queryString = "select feesDate,itemAccn,feesDesc,dueAmount from paymentDetails where memberId=@memberId and isPaid='" + false + "'";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            dgvPaymentDetails.Rows.Add(false,FormatDate.getUserFormat(dataReader["feesDate"].ToString()), dataReader["itemAccn"].ToString(), dataReader["feesDesc"].ToString(), dataReader["dueAmount"].ToString(), false);
                        }
                    }
                    else
                    {
                        dgvPaymentDetails.Rows.Clear();
                    }
                    dataReader.Close();
                    sqltConn.Close();
                    dgvPaymentDetails.ClearSelection();
                    btnAdd.Enabled = true;
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
                        string queryString = "select brrName,brrAddress,brrImage from borrower_details where brrId=@brrId";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                txtbBrrName.Text = dataReader["brrName"].ToString();
                                txtbBrrAddress.Text = dataReader["brrAddress"].ToString();
                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                    pcbBrrImage.Image = System.Drawing.Image.FromStream(memoryStream, true);
                                }
                                catch
                                {
                                    pcbBrrImage.Image = Properties.Resources.blankBrrImage;
                                }
                            }
                        }
                        dataReader.Close();

                        queryString = "select feesDate,itemAccn,feesDesc,dueAmount from payment_details where memberId=@memberId and isPaid='" + false + "'";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                        mysqlCmd.CommandTimeout = 99999;
                        dataReader = mysqlCmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                dgvPaymentDetails.Rows.Add(false, FormatDate.getUserFormat(dataReader["feesDate"].ToString()), dataReader["itemAccn"].ToString(), dataReader["feesDesc"].ToString(), dataReader["dueAmount"].ToString(), false);
                            }
                        }
                        else
                        {
                            dgvPaymentDetails.Rows.Clear();
                        }
                        dataReader.Close();
                        mysqlConn.Close();
                        dgvPaymentDetails.ClearSelection();
                        btnAdd.Enabled = true;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                dgvPaymentDetails.Rows.Clear();
                btnAdd.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtbDesc.Text=="")
            {
                MessageBox.Show("Please add the description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbDesc.SelectAll();
                return;
            }
            string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            dgvPaymentDetails.Rows.Add(true, FormatDate.getUserFormat(currentDate), "", txtbDesc.Text, txtbAmount.Text,false);
            double totalFine = 0;
            foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
            {
                if (dataRow.Cells[0].Value.ToString() == "True")
                {
                    totalFine = totalFine + Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                }
            }
            lblTotal.Text = totalFine.ToString("0.00");
            lblPay.Text = Math.Round((totalFine - Convert.ToDouble(txtbDiscount.Text)), 2, MidpointRounding.AwayFromZero).ToString("0.00");
            txtbDesc.Clear();
            txtbAmount.Text = 0.00.ToString("0.00");
            dgvPaymentDetails.ClearSelection(); 
        }

        private void txtbAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
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

        private void txtbAmount_Leave(object sender, EventArgs e)
        {
            if(txtbAmount.Text=="")
            {
                txtbAmount.Text = 0.00.ToString("0.00");
            }
        }

        private void dgvPaymentDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) 
            {
                double totalFine = 0;
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    if (dataRow.Cells[0].Value.ToString() == "True")
                    {
                        btnSave.Enabled = true;
                        totalFine = totalFine + Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                    }
                }
                lblTotal.Text = totalFine.ToString("0.00");
                lblPay.Text = Math.Round((totalFine-Convert.ToDouble(txtbDiscount.Text)), 2, MidpointRounding.AwayFromZero).ToString("0.00");
            }
            else if(e.ColumnIndex == 5)
            {
                double totalFine = Convert.ToDouble(lblTotal.Text);
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    if (dataRow.Cells[0].Value.ToString() == "True" && dataRow.Cells[5].Value.ToString() == "True")
                    {
                        totalFine = totalFine - Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                    }
                }
                lblPay.Text = Math.Round((totalFine - Convert.ToDouble(txtbDiscount.Text)), 2, MidpointRounding.AwayFromZero).ToString("0.00");
            }
        }

        private void chkbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbAll.Checked)
            {
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    dataRow.Cells[0].Value = true;
                    dgvPaymentDetails.CurrentCell = dataRow.Cells[1];
                }
                btnSave.Enabled = true;
            }
            else
            {
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    dataRow.Cells[0].Value = false;
                    dgvPaymentDetails.CurrentCell = dataRow.Cells[1];
                }
                btnSave.Enabled = false;
            }
            Application.DoEvents();
        }

        private void dgvPaymentDetails_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 5)
            {
                SendKeys.Send("{Tab}");
            }
            if (e.ColumnIndex == 0)
            {
                btnSave.Enabled = true;
            }
        }

        private void txtbAmount_Enter(object sender, EventArgs e)
        {
            txtbAmount.Clear();
        }

        private void txtbDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                // only allow one decimal point
                if (e.KeyChar == '.'
                    && (sender as TextBox).Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }

                if (!char.IsControl(e.KeyChar) && e.KeyChar.ToString() != ".")
                {
                    try
                    {
                        if (Convert.ToDouble(lblTotal.Text) < (Convert.ToDouble(txtbDiscount.Text + e.KeyChar.ToString())))
                        {
                            MessageBox.Show("Please enter right amount.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            e.Handled = true;
                        }
                    }
                    catch
                    {

                    }
                }
                
            }
        }

        private void txtbDiscount_Leave(object sender, EventArgs e)
        {
            if(txtbDiscount.Text=="")
            {
                txtbDiscount.Text = 0.00.ToString("0.00");
            }
        }

        private void txtbDiscount_Enter(object sender, EventArgs e)
        {
            txtbDiscount.Clear();
        }

        private void txtbDiscount_TextChanged(object sender, EventArgs e)
        {
            if(txtbDiscount.Text!="" && txtbDiscount.Text!=".")
            {
                lblPay.Text = Math.Round((Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(txtbDiscount.Text)), 2, MidpointRounding.AwayFromZero).ToString("0.00");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(lblPay.Text) > 0)
            {
                if (cmbPaymentMode.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select payment mode.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            int rowCount = 0;
            string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
            bool isPaymentDone = false;
            string invId = "";
            bool isRemit = false;

            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select invidCount from paymentDetails where invidCount!=0 order by id desc limit 1";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                    }
                }
                dataReader.Close();

                rowCount++;
                invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    isRemit = false;
                    if (dataRow.Cells[5].Value.ToString() == "True")
                    {
                        isRemit = true;
                    }
                    queryString = "select * from paymentDetails where memberId=@memberId and [feesDate]='" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            queryString = "update paymentDetails set invId='" + invId + "',isPaid='" + true + "'," +
                            "isRemited='" + isRemit + "',discountAmnt='" + txtbDiscount.Text + "',payDate=" +
                            "'" + currentDate + "',collctedBy='" + Properties.Settings.Default.currentUserId + "'," +
                            "invidCount='" + rowCount + "',paymentMode='" + cmbPaymentMode.Text + "'," +
                            "paymentRef='" + txtbReference.Text + "' where memberId=@memberId and [feesDate]=" +
                            "'" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                            globalVarLms.itemList.Add(dataRow.Cells[2].Value.ToString());
                            isPaymentDone = true;
                        }
                    }
                    else
                    {
                        dataReader.Close();
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            queryString = "insert into paymentDetails (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                                "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount,paymentMode,paymentRef) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                                "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                                ",'" + dataRow.Cells[4].Value.ToString() + "','" + true + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                                "'" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + cmbPaymentMode.Text + "','" + txtbReference.Text + "')";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
                            globalVarLms.itemList.Add(dataRow.Cells[2].Value.ToString());
                            isPaymentDone = true;
                        }
                        else
                        {
                            queryString = "insert into paymentDetails (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                               "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                               "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                               ",'" + dataRow.Cells[4].Value.ToString() + "','" + false + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                               "'" + Properties.Settings.Default.currentUserId + "','" + 0 + "')";
                            sqltCommnd.CommandText = queryString;
                            sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            sqltCommnd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                            sqltCommnd.ExecuteNonQuery();
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
                MySqlCommand mysqlCmd;
                string queryString = "select invidCount from payment_details where invidCount!=0 order by id desc limit 1";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                    }
                }
                dataReader.Close();

                rowCount++;
                invId = globalVarLms.instShortName + "-" + rowCount.ToString("00000");
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    isRemit = false;
                    if (dataRow.Cells[5].Value.ToString() == "True")
                    {
                        isRemit = true;
                    }
                    queryString = "select * from payment_details where memberId=@memberId and feesDate='" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        dataReader.Close();
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            queryString = "update payment_details set invId='" + invId + "',isPaid='" + true + "'," +
                            "isRemited='" + isRemit + "',discountAmnt='" + txtbDiscount.Text + "',payDate=" +
                            "'" + currentDate + "',collctedBy='" + Properties.Settings.Default.currentUserId + "'," +
                            "invidCount='" + rowCount + "',paymentMode='" + cmbPaymentMode.Text + "'," +
                            "paymentRef='" + txtbReference.Text + "' where memberId=@memberId and feesDate=" +
                            "'" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();
                            globalVarLms.itemList.Add(dataRow.Cells[2].Value.ToString());
                            isPaymentDone = true;
                        }
                    }
                    else
                    {
                        dataReader.Close();
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            queryString = "insert into payment_details (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                                "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount,paymentMode,paymentRef) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                                "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                                ",'" + dataRow.Cells[4].Value.ToString() + "','" + true + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                                "'" + Properties.Settings.Default.currentUserId + "','" + rowCount + "','" + cmbPaymentMode.Text + "','" + txtbReference.Text + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();
                            globalVarLms.itemList.Add(dataRow.Cells[2].Value.ToString());
                            isPaymentDone = true;
                        }
                        else
                        {
                            queryString = "insert into payment_details (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                               "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                               "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                               ",'" + dataRow.Cells[4].Value.ToString() + "','" + false + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                               "'" + Properties.Settings.Default.currentUserId + "','" + 0 + "')";
                            mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                            mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                            mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                            mysqlCmd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                            mysqlCmd.ExecuteNonQuery();
                        }
                    }
                }
                mysqlConn.Close();
            }
            globalVarLms.backupRequired = true;
            if(isPaymentDone && Convert.ToDouble(lblPay.Text)>0)
            {
                if(globalVarLms.paymentReciept)
                {
                    generateMembershipReciept(invId, currentDate);
                }
            }
            this.Close();
        }

        private void generateMembershipReciept(string invId, string currentDate)
        {
            System.Drawing.Image instLogo = null;
            string instName = "", instAddress = "", instContact = "", instWebsite = "", instMail = "", cuurShort = "";
            string brrContact = "";
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                //====================check for Borrower id exist======================
                string queryString = "select * from generalSettings";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        instName = dataReader["instName"].ToString();
                        instAddress = dataReader["instAddress"].ToString();
                        instContact = dataReader["instContact"].ToString();
                        instWebsite = dataReader["instWebsite"].ToString();
                        instMail = dataReader["instMail"].ToString();
                        cuurShort = dataReader["currShortName"].ToString();
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                            MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                        }
                        catch
                        {
                            instLogo = Properties.Resources.NoImageAvailable;
                        }
                    }
                }
                dataReader.Close();

                queryString = "select brrContact from borrowerDetails where brrId=@brrId";
                sqltCommnd.CommandText = queryString;
                sqltCommnd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                dataReader = sqltCommnd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        brrContact = dataReader["brrContact"].ToString();
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
                    string queryString = "select * from general_settings";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.CommandTimeout = 99999;
                    MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            instName = dataReader["instName"].ToString();
                            instAddress = dataReader["instAddress"].ToString();
                            instContact = dataReader["instContact"].ToString();
                            instWebsite = dataReader["instWebsite"].ToString();
                            instMail = dataReader["instMail"].ToString();
                            cuurShort = dataReader["currShortName"].ToString();
                            try
                            {
                                byte[] imageBytes = Convert.FromBase64String(dataReader["instLogo"].ToString());
                                MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                                instLogo = System.Drawing.Image.FromStream(memoryStream, true);
                            }
                            catch
                            {
                                instLogo = Properties.Resources.NoImageAvailable;
                            }
                        }
                    }
                    dataReader.Close();

                    queryString = "select brrContact from borrower_details where brrId=@brrId";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@brrId", txtbBrrId.Text);
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            brrContact = dataReader["brrContact"].ToString();
                        }
                    }
                    dataReader.Close();
                    mysqlConn.Close();
                }
                catch
                {

                }
            }
            string folderPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch
                {

                }
            }
            Directory.CreateDirectory(folderPath);
            string fileName = folderPath + @"\tempInvoice" + ".pdf";
            if (globalVarLms.recieptPaper == "A4")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    Document pdfToCreate = new Document(PageSize.A4);
                    pdfToCreate.SetMargins(40, 10, 160, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(60, pdfToCreate.PageSize.Height - 70f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(50f, 50f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 12);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 44f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = "Call/website/email : " + instContact + " | " + instWebsite + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 9);
                    pdfContent.SetTextMatrix(115, pdfToCreate.PageSize.Height - 56f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 9);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(410, pdfToCreate.PageSize.Height - 68f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(410, pdfToCreate.PageSize.Height - 58f);
                    }

                    pdfContent.ShowText("Prepared Date : " + FormatDate.getUserFormat(currentDate) + " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(40, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.LineTo(585, pdfToCreate.PageSize.Height - 80f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 545;
                    PdfPCell pdfCell = null;

                    PdfPTable tempTable = new PdfPTable(4);
                    float[] columnWidth = { 80f, 225f, 80f, 160f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(2);
                    columnWidth = new float[] { 80f, 465f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    columnWidth = new float[] { 70f, 360, 120f, 100f, 120f };
                    tempTable.SetTotalWidth(columnWidth);

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Accession No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Remitted Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    columnWidth = new float[] { 70f, 360, 120f, 100f, 120f };
                    tempTable.SetTotalWidth(columnWidth);

                    double ttlRemit = 0.00;
                    int rowCount = 0;
                    //Adding DataRow
                    foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                    {
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            pdfCell = new PdfPCell(new Phrase((rowCount + 1).ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            if (dataRow.Cells[5].Value.ToString() == "True")
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                                ttlRemit = ttlRemit + Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                            }
                            else
                            {
                                pdfCell = new PdfPCell(new Phrase(0.00.ToString("0.00"), smallFont));
                            }
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.BorderWidth = 0;
                            tempTable.AddCell(pdfCell);
                            rowCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    columnWidth = new float[] { 70f, 360, 120f, 100f, 120f };
                    tempTable.SetTotalWidth(columnWidth);

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Subtotal    :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblTotal.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + ttlRemit.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        pdfCell.BorderColorTop = new iTextSharp.text.BaseColor(Color.DimGray);
                        tempTable.AddCell(pdfCell);
                    }

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Discount   :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            double discount = Convert.ToDouble(txtbDiscount.Text);
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + discount.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    columnWidth = new float[] { 70f, 360, 120f, 100f, 120f };
                    tempTable.SetTotalWidth(columnWidth);

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Grand Total :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblPay.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 40, pdfToCreate.PageSize.Height - 85, pdfContent);

                    pdfToCreate.Close();
                    outputStream.Close();
                }

                if (globalVarLms.licenseType == "Demo")
                {
                    string tempName = Path.GetFileName(fileName);
                    string tempFile = fileName.Replace(tempName, "tempPdf.pdf");
                    File.Move(fileName, tempFile);
                    byte[] pdfBytes = File.ReadAllBytes(tempFile);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, Encoding.ASCII.EncodingName, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    File.WriteAllBytes(fileName, AddWatermark(pdfBytes, baseFont));
                    File.Delete(tempFile);
                }
            }
            else if (globalVarLms.recieptPaper == "Roll Paper 80 x 297 mm")
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(315f, 1169f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(175, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(175, pdfToCreate.PageSize.Height - 46f);
                    }
                    pdfContent.ShowText("Prepared Date : " + FormatDate.getUserFormat(currentDate) + " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(305, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 295;

                    PdfPTable tempTable = new PdfPTable(2);
                    float[] columnWidth = { 65, 230 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    pdfTable.TotalWidth = 295;

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Accession No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Remitted Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    pdfTable.TotalWidth = 295;

                    double ttlRemit = 0.00;
                    int rowCount = 0;
                    //Adding DataRow
                    foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                    {
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            pdfCell = new PdfPCell(new Phrase((rowCount + 1).ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            if (dataRow.Cells[5].Value.ToString() == "True")
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                                ttlRemit = ttlRemit + Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                            }
                            else
                            {
                                pdfCell = new PdfPCell(new Phrase(0.00.ToString("0.00"), smallFont));
                            }
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.BorderWidth = 0;
                            tempTable.AddCell(pdfCell);
                            rowCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    tempTable.TotalWidth = 295;

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Subtotal    :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblTotal.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + ttlRemit.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        pdfCell.BorderColorTop = new iTextSharp.text.BaseColor(Color.DimGray);
                        tempTable.AddCell(pdfCell);
                    }

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Discount   :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            double discount = Convert.ToDouble(txtbDiscount.Text);
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + discount.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    tempTable.TotalWidth = 295;

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Grand Total :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblPay.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }
            else
            {
                using (FileStream outputStream = new FileStream(fileName, FileMode.Create))
                {
                    var pageSize = new iTextSharp.text.Rectangle(189f, 12897f);
                    Document pdfToCreate = new Document(pageSize);
                    pdfToCreate.SetMargins(0, 0, 20, 20);
                    PdfWriter pdWriter = PdfWriter.GetInstance(pdfToCreate, outputStream);
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    BaseFont baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
                    if (File.Exists("c:/windows/Fonts/malgun.ttf"))
                    {
                        baseFont = BaseFont.CreateFont("c:/windows/Fonts/malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        baseFontBold = BaseFont.CreateFont("c:/windows/Fonts/malgunbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                    iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 7, 0, BaseColor.BLACK);
                    iTextSharp.text.Font smallFontBold = new iTextSharp.text.Font(baseFontBold, 7, 0, BaseColor.BLACK);
                    pdfToCreate.Open();

                    PdfContentByte pdfContent = pdWriter.DirectContent;

                    iTextSharp.text.Image instLogoJpg = iTextSharp.text.Image.GetInstance(instLogo, BaseColor.WHITE);
                    instLogoJpg.SetAbsolutePosition(10, pdfToCreate.PageSize.Height - 45f);//================Institute Logo=====
                    instLogoJpg.ScaleAbsolute(25f, 25f);
                    pdfToCreate.Add(instLogoJpg);

                    pdfContent.BeginText();//================Institute Name=====
                    pdfContent.SetFontAndSize(baseFontBold, 9);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 30f);
                    pdfContent.ShowText(instName);
                    pdfContent.EndText();

                    pdfContent.BeginText();//================Institute Address=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 38f);
                    pdfContent.ShowText(instAddress);
                    pdfContent.EndText();

                    string contactDetails = instContact + " | " + instMail;
                    pdfContent.BeginText();//================Institute Contact=====
                    pdfContent.SetFontAndSize(baseFont, 7);
                    pdfContent.SetTextMatrix(33, pdfToCreate.PageSize.Height - 46f);
                    pdfContent.ShowText(contactDetails);
                    pdfContent.EndText();

                    pdfContent.BeginText();//===============date Time
                    pdfContent.SetFontAndSize(baseFont, 7);
                    if (contactDetails != "")
                    {
                        pdfContent.SetTextMatrix(50, pdfToCreate.PageSize.Height - 54f);
                    }
                    else
                    {
                        pdfContent.SetTextMatrix(50, pdfToCreate.PageSize.Height - 46f);
                    }
                    pdfContent.ShowText("Prepared Date : " + FormatDate.getUserFormat(currentDate) + " " + DateTime.Now.ToString("hh:mm:ss tt"));
                    pdfContent.EndText();

                    pdfContent.SetLineWidth(1.0f);//================Draw Black Line=====
                    pdfContent.SetColorFill(BaseColor.DARK_GRAY);
                    pdfContent.MoveTo(10, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.LineTo(179, pdfToCreate.PageSize.Height - 60f);
                    pdfContent.Stroke();

                    PdfPTable pdfTable = new PdfPTable(1);
                    pdfTable.TotalWidth = 169;

                    PdfPTable tempTable = new PdfPTable(2);
                    float[] columnWidth = { 65, 104 };
                    tempTable.SetTotalWidth(columnWidth);

                    PdfPCell pdfCell = new PdfPCell(new Phrase("Invoice No :", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(invId, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Id : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrId.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Member Name : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(txtbBrrName.Text, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Contact No : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(brrContact, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Bill Created By : ", smallFontBold));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase(globalVarLms.currentUserName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 5;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    pdfTable.TotalWidth = 169;

                    pdfCell = new PdfPCell(new Phrase("Sl No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Description", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Accession No.", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(new Phrase("Remitted Amount", smallFontBold));
                    pdfCell.Border = 0;
                    pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tempTable.AddCell(pdfCell);

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    pdfTable.TotalWidth = 169;

                    double ttlRemit = 0.00;
                    int rowCount = 0;
                    //Adding DataRow
                    foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                    {
                        if (dataRow.Cells[0].Value.ToString() == "True")
                        {
                            pdfCell = new PdfPCell(new Phrase((rowCount + 1).ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[3].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[2].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.Border = 0;
                            tempTable.AddCell(pdfCell);

                            if (dataRow.Cells[5].Value.ToString() == "True")
                            {
                                pdfCell = new PdfPCell(new Phrase(dataRow.Cells[4].Value.ToString(), smallFont));
                                ttlRemit = ttlRemit + Convert.ToDouble(dataRow.Cells[4].Value.ToString());
                            }
                            else
                            {
                                pdfCell = new PdfPCell(new Phrase(0.00.ToString("0.00"), smallFont));
                            }
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            pdfCell.BorderWidth = 0;
                            tempTable.AddCell(pdfCell);
                            rowCount++;
                        }
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.PaddingBottom = 2;
                    pdfCell.BorderWidthTop = 1f;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    tempTable.TotalWidth = 169;

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Subtotal    :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblTotal.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + ttlRemit.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        pdfCell.BorderColorTop = new iTextSharp.text.BaseColor(Color.DimGray);
                        tempTable.AddCell(pdfCell);
                    }

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Discount   :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            double discount = Convert.ToDouble(txtbDiscount.Text);
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + discount.ToString("0.00"), smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderWidthBottom = 1f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    tempTable = new PdfPTable(5);
                    tempTable.TotalWidth = 169;

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i == 2)
                        {
                            pdfCell = new PdfPCell(new Phrase("Grand Total :", smallFontBold));
                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        else if (i == 3)
                        {
                            pdfCell = new PdfPCell(new Phrase(cuurShort + " " + lblPay.Text, smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else if (i == 4)
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        else
                        {
                            pdfCell = new PdfPCell(new Phrase(" ", smallFont));
                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        pdfCell.BorderWidth = 0;
                        pdfCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                        tempTable.AddCell(pdfCell);
                    }

                    pdfCell = new PdfPCell(tempTable);
                    pdfCell.BorderWidth = 0;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfTable.AddCell(pdfCell);

                    smallFont = new iTextSharp.text.Font(baseFont, 6, 0, BaseColor.BLACK);
                    pdfCell = new PdfPCell(new Phrase("The reciept generated by " + Application.ProductName, smallFont));
                    pdfCell.BorderWidth = 0; //1.5f;
                    pdfCell.BorderColor = BaseColor.DARK_GRAY;
                    pdfCell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    pdfTable.AddCell(pdfCell);

                    pdfTable.WriteSelectedRows(0, -1, 10, pdfToCreate.PageSize.Height - 63f, pdfContent);

                    pdfToCreate.Close();
                    pdWriter.Close();
                    outputStream.Close();
                }
            }

            // Create the printer settings for our printer
            var printerSettings = new PrinterSettings
            {
                PrinterName = globalVarLms.recieptPrinter,
                Copies = (short)1,
                FromPage = (short)1,
            };

            // Create our page settings for the paper size selected
            var pageSettings = new PageSettings(printerSettings)
            {
                Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0),
            };
            foreach (PaperSize paperSize in printerSettings.PaperSizes)
            {
                //MessageBox.Show(paperSize.PaperName + "," + paperSize.Width + "," + paperSize.Height);
                if (paperSize.PaperName == "A4")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
                else if (paperSize.PaperName == "Roll Paper 80 x 297 mm")
                {
                    pageSettings.PaperSize = paperSize;
                    break;
                }
            }
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(fileName))
            {
                using (var printDocument = pdfDocument.CreatePrintDocument())
                {
                    printDocument.PrinterSettings = printerSettings;
                    printDocument.DefaultPageSettings = pageSettings;
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.Print();
                    //https://stackoverflow.com/questions/6103705/how-can-i-send-a-file-document-to-the-printer-and-have-it-print
                }
            }
        }

        private static byte[] AddWatermark(byte[] pdfBytes, BaseFont baseFont)
        {
            using (var memoryStream = new MemoryStream(10 * 1024))
            {
                using (var pdfReader = new PdfReader(pdfBytes))
                using (var pdfStamper = new PdfStamper(pdfReader, memoryStream))
                {
                    int pageCount = pdfReader.NumberOfPages;
                    for (int i = 1; i <= pageCount; i++)
                    {
                        var contentBytes = pdfStamper.GetOverContent(i);
                        AddWaterMark(contentBytes, "www.CodeAchi.com", baseFont, 48, 35, new BaseColor(255, 0, 0), pdfReader.GetPageSizeWithRotation(i));
                    }
                    pdfStamper.Close();
                }
                return memoryStream.ToArray();
            }
        }

        public static void AddWaterMark(PdfContentByte contentBytes, string watermarkText, BaseFont baseFont, float fontSize, float angle, BaseColor color, iTextSharp.text.Rectangle realPageSize, iTextSharp.text.Rectangle rect = null)
        {
            var gstate = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.3f };
            contentBytes.SaveState();
            contentBytes.SetGState(gstate);
            contentBytes.SetColorFill(color);
            contentBytes.BeginText();
            contentBytes.SetFontAndSize(baseFont, fontSize);
            var ps = rect ?? realPageSize; /*dc.PdfDocument.PageSize is not always correct*/
            var x = (ps.Right + ps.Left) / 2;
            var y = (ps.Bottom + ps.Top) / 2;
            contentBytes.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, angle);
            contentBytes.EndText();
            contentBytes.RestoreState();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.sqliteDatabase)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                string queryString = "select invidCount from paymentDetails where invidCount!=0 order by id desc limit 1";
                sqltCommnd.CommandText = queryString;
                SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                int rowCount = 0;
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                    }
                }
                dataReader.Close();
                string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                globalVarLms.itemList.Clear();
                string invId = globalVarLms.instShortName + "-" + (rowCount + 1).ToString("00000");
                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    bool isRemit = false;
                    if (dataRow.Cells[5].Value.ToString() == "True")
                    {
                        isRemit = true;
                    }
                    queryString = "select * from paymentDetails where memberId=@memberId and [feesDate]='" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                    sqltCommnd.CommandText = queryString;
                    sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                    sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                    dataReader = sqltCommnd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        dataReader.Close();
                        queryString = "insert into paymentDetails (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                            "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                            "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                            ",'" + dataRow.Cells[4].Value.ToString() + "','" + false + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                            "'" + Properties.Settings.Default.currentUserId + "','" + rowCount + 1 + "')";
                        sqltCommnd.CommandText = queryString;
                        sqltCommnd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                        sqltCommnd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                        sqltCommnd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                        sqltCommnd.ExecuteNonQuery();
                    }
                    dataReader.Close();
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
                string queryString = "select invidCount from payment_details where invidCount!=0 order by id desc limit 1";
                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                mysqlCmd.CommandTimeout = 99999;
                MySqlDataReader dataReader = mysqlCmd.ExecuteReader();
                int rowCount = 0;
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        rowCount = Convert.ToInt32(dataReader["invidCount"].ToString());
                    }
                }
                dataReader.Close();
                string currentDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                globalVarLms.itemList.Clear();
                string invId = globalVarLms.instShortName + "-" + (rowCount + 1).ToString("00000");

                foreach (DataGridViewRow dataRow in dgvPaymentDetails.Rows)
                {
                    bool isRemit = false;
                    if (dataRow.Cells[5].Value.ToString() == "True")
                    {
                        isRemit = true;
                    }
                    queryString = "select * from payment_details where memberId=@memberId and feesDate='" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "' and itemAccn=@itemAccn";
                    mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                    mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                    mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                    mysqlCmd.CommandTimeout = 99999;
                    dataReader = mysqlCmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        dataReader.Close();
                        queryString = "insert into payment_details (feesDate,invId,memberId,itemAccn,feesDesc,dueAmount," +
                            "isPaid,isRemited,discountAmnt,payDate,collctedBy,invidCount) values('" + FormatDate.getAppFormat(dataRow.Cells[1].Value.ToString()) + "'," +
                            "'" + invId + "',@memberId,@itemAccn,@feesDesc" +
                            ",'" + dataRow.Cells[4].Value.ToString() + "','" + false + "','" + isRemit + "','" + txtbDiscount.Text + "','" + currentDate + "'," +
                            "'" + Properties.Settings.Default.currentUserId + "','" + rowCount + 1 + "')";
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.Parameters.AddWithValue("@memberId", txtbBrrId.Text);
                        mysqlCmd.Parameters.AddWithValue("@itemAccn", dataRow.Cells[2].Value.ToString());
                        mysqlCmd.Parameters.AddWithValue("@feesDesc", dataRow.Cells[3].Value.ToString());
                        mysqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dataReader.Close();
                    }
                }
                mysqlConn.Close();
            }
            this.Close();
        }

        private void dgvPaymentDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            btnSave.Enabled = true;
            btnSkip.Enabled = true;
        }

        private void dgvPaymentDetails_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(dgvPaymentDetails.Rows.Count==0)
            {
                btnSave.Enabled = false;
                btnSkip.Enabled = false;
            }
        }

        private void btnAdd_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnAdd.Enabled == true)
            {
                btnAdd.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnAdd.BackColor = Color.DimGray;
            }
        }

        private void btnSave_EnabledChanged(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            if (btnSave.Enabled == true)
            {
                btnSave.BackColor = Color.FromArgb(45, 58, 66);
            }
            else
            {
                btnSave.BackColor = Color.DimGray;
            }
        }

        private void lnkLvlRctSetting_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRecieptSetting recieptSetting = new FormRecieptSetting();
            recieptSetting.ShowDialog();
            globalVarLms.issueReciept = Properties.Settings.Default.issueReciept;
            globalVarLms.reissueReciept = Properties.Settings.Default.reissueReciept;
            globalVarLms.returnReciept = Properties.Settings.Default.returnReciept;
            globalVarLms.paymentReciept = Properties.Settings.Default.paymentReciept;
            globalVarLms.recieptPrinter = Properties.Settings.Default.recieptPrinter;
            globalVarLms.recieptPaper = Properties.Settings.Default.recieptPaper;
        }
    }
}
