using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormIsbnItems : Form
    {
        public FormIsbnItems()
        {
            InitializeComponent();
        }

        public string operationName = "";

        private void FormIsbnItems_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void dgvAccnList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(dgvAccnList.SelectedRows.Count>0)
            {
                issueItem_Click(null, null);
            }
        }

        private void dgvAccnList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvAccnList.HitTest(e.X, e.Y);
                dgvAccnList.ClearSelection();
                if (hti.RowIndex >= 0)
                {
                    dgvAccnList.Rows[hti.RowIndex].Selected = true;
                    contextMenuStrip1.Show(dgvAccnList, new Point(e.X, e.Y));
                }
            }
        }

        private void issueItem_Click(object sender, EventArgs e)
        {
            if (dgvAccnList.SelectedRows[0].Cells[2].Value.ToString() == "Yes")
            {
                this.Hide();
            }
            else if (dgvAccnList.SelectedRows[0].Cells[2].Value.ToString() == "No")
            {
                MessageBox.Show("Item already issued.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.Hide();
            }
        }

        private void FormIsbnItems_Load(object sender, EventArgs e)
        {
            dgvAccnList.ClearSelection();
            loadFieldValue();
            if(operationName=="Return")
            {
                issueItem.Text = "Return Item";
            }
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
                            dgvAccnList.Columns[1].HeaderText = fieldValue.Replace(fieldName + "=", "");
                        }
                        else if (fieldName == "lblTitle")
                        {
                            lblTitle_.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblAuthor")
                        {
                            lblAuthor_.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                        else if (fieldName == "lblIsbn")
                        {
                            lblIsbn_.Text = fieldValue.Replace(fieldName + "=", "") + " :";
                        }
                    }
                }
            }
        }
    }
}
