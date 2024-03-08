using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public partial class FormAdobeReader : Form
    {
        public FormAdobeReader()
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
        string fileName ="";

        private void FormAdobeReader_Load(object sender, EventArgs e)
        {
          
        }

        private void FormAdobeReader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                       this.DisplayRectangle);
        }

        public bool IsConnectedToInternet()
        {
            //int desc;
            //return InternetGetConnectedState(out desc, 0);
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblProgress.Text = "Downloaded " + e.ProgressPercentage.ToString() + "%";
            Application.DoEvents();
            if (e.ProgressPercentage == 100)
            {
                
                //string tempPath = Path.GetTempPath() + Application.ProductName;
                //string fileName = tempPath + @"\AcroRdrDC1801120040_en_US.exe";
                Process InstallProcess = new Process();
                InstallProcess.StartInfo.FileName = fileName;
                InstallProcess.StartInfo.Arguments = "File";
                InstallProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                InstallProcess.Start();
                WindowHelper.BringProcessToFront(InstallProcess);
                InstallProcess.WaitForExit();
                lblProgress.Text = "Installation Complete Successfully.";
                btnSkip.Text = "Close";
                btnSkip.Enabled = true;
                Application.DoEvents();

                IntPtr hMenu = GetSystemMenu(this.Handle, false);
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                Cursor = Cursors.Default;
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            Timer2.Enabled = false;
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = false;
            btnSkip.Enabled = false;
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
            if (IsConnectedToInternet() == true)
            {
                try
                {
                    string tempPath = Path.GetTempPath() + Application.ProductName;
                    if (Directory.Exists(tempPath))
                    {
                        Directory.Delete(tempPath, true);
                    }
                    Directory.CreateDirectory(tempPath);
                    //string fileName = tempPath + @"\AcroRdrDC1801120040_en_US.exe";
                    Cursor = Cursors.WaitCursor;
                    Timer2.Enabled = true;
                    string queryToCheck = "SELECT pdfUrl FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                    ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    WebRequest webRequest = WebRequest.Create(CheckProductStatus.selectApi + queryToCheck);
                    webRequest.Timeout = 8000;
                    WebResponse webResponse = webRequest.GetResponse();
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader strmReader = new StreamReader(dataStream);
                    string requestResult = strmReader.ReadLine();
                    if (requestResult != "")
                    {
                        requestResult = requestResult.Replace("$", "");
                        fileName = tempPath + requestResult.Substring(requestResult.LastIndexOf("/"));
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                            wc.DownloadFileAsync(new System.Uri(requestResult), fileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    btnSkip.Enabled = true;
                    btnContinue.Enabled = true;
                    EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                    Cursor = Cursors.Default;
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                btnSkip.Enabled = true;
                btnContinue.Enabled = true;
                EnableMenuItem(hMenu, SC_CLOSE, MF_ENABLED);
                Cursor = Cursors.Default;
                MessageBox.Show("Please check your internet connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
