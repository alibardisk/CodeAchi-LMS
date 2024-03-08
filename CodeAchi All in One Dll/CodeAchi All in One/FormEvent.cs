using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public partial class FormEvent : Form
    {
        public FormEvent()
        {
            InitializeComponent();
        }

        private void FormEvent_Load(object sender, EventArgs e)
        {
            timer1.Interval = 10000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pcbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            pcbClose.BackColor = Color.Transparent;
        }
    }
}
