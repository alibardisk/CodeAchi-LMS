using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    public partial class FormFont : Form
    {
        public FormFont()
        {
            InitializeComponent();
        }

        private void FormFont_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Gray, 2),
                        this.DisplayRectangle);
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            Timer2.Stop();
            string tempPath = Path.GetTempPath() + Application.ProductName;
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
            Directory.CreateDirectory(tempPath);
            string fileName = tempPath + @"\malgunbd.ttf";
            try
            {
                Cursor = Cursors.WaitCursor;
                string queryToCheck = "SELECT simHeiUrl FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                WebRequest webRequest = WebRequest.Create(globalVarLms.selectApi + queryToCheck);
                webRequest.Timeout = 8000;
                WebResponse webResponse = webRequest.GetResponse();
                Stream dataStream = webResponse.GetResponseStream();
                StreamReader strmReader = new StreamReader(dataStream);
                string requestResult = strmReader.ReadLine();
                if (requestResult != "")
                {
                    requestResult = requestResult.Replace("$", "");
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                        wc.DownloadFileAsync(new System.Uri(requestResult), fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblProgress.Text = "Downloaded " + e.ProgressPercentage.ToString() + "%";
            Application.DoEvents();
            if (e.ProgressPercentage == 100)
            {
                try
                {
                    string tempPath = Path.GetTempPath() + Application.ProductName;
                    string fileName = tempPath + @"\malgunbd.ttf";
                    Process InstallProcess = new Process();
                    InstallProcess.StartInfo.FileName = fileName;
                    InstallProcess.StartInfo.Arguments = "File";
                    InstallProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    InstallProcess.Start();
                    WindowHelper.BringProcessToFront(InstallProcess);
                    InstallProcess.WaitForExit();
                    File.Delete(fileName);
                }
                catch
                {

                }
                Cursor = Cursors.Default;
                this.Hide();  
            }
        }

        private void FormFont_Load(object sender, EventArgs e)
        {
            Timer2.Start();
        }
    }

    public static class WindowHelper
    {
        public static void BringProcessToFront(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            if (IsIconic(handle))
            {
                ShowWindow(handle, SW_RESTORE);
            }

            SetForegroundWindow(handle);
        }

        const int SW_RESTORE = 9;

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);
    }
}
