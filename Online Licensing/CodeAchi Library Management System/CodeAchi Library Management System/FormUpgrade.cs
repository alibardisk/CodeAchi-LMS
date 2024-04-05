using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormUpgrade : Form
    {
        public FormUpgrade()
        {
            InitializeComponent();
        }

        private const uint SC_CLOSE = 0xf060;
        private const uint MF_GRAYED = 0x01;
        private const int MF_ENABLED = 0x00000000;
        private const int MF_DISABLED = 0x00000002;

        [DllImport("user32.dll")]
        private static extern int EnableMenuItem(IntPtr hMenu, uint wIDEnableItem, uint wEnable);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        private void FormUpgrade_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                         this.DisplayRectangle);
        }

        private void FormUpgrade_Load(object sender, EventArgs e)
        {
            bWorkerGetDetails.RunWorkerAsync();
            btnCancel.Enabled = false;
            btnUpdate.Enabled = false;
            //IntPtr hMenu = GetSystemMenu(this.Handle, false);
            //EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }

        private void bWorkerGetDetails_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
        }

        private void bWorkerGetDetails_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string[] splitData = globalVarLms.tempValue.Split('$');
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new System.Uri(splitData[0]),
                 Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\"+Application.ProductName + @"\"+splitData[1]);
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblProgress.Text ="Downloaded "+ e.ProgressPercentage.ToString()+"%";
            Application.DoEvents();
            if(e.ProgressPercentage==100)
            {
                btnCancel.Enabled = true;
                btnUpdate.Enabled = true;
                //IntPtr hMenu = GetSystemMenu(this.Handle, false);
                //EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                //btnUpdate_Click(null, null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Updater.exe");
            Application.Exit();
        }
    }
}
