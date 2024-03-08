using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SetupPreChecking
{
    public partial class FormChecking : Form
    {
        public FormChecking()
        {
            InitializeComponent();
        }

        private void FormChecking_Load(object sender, EventArgs e)
        {
            Thread newInstallThread = new Thread(InstallThread);
            newInstallThread.Start();
        }

        public void InstallThread()
        {
            int versionCounter = 0;
            bool startInstall = false;
            // Opens the registry key for the .NET Framework entry. 
            RegistryKey frameWorkKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
            if (frameWorkKey != null)
            {
                int releaseKey = (int)frameWorkKey.GetValue("Release");
                if (releaseKey >= 378389)
                {
                    versionCounter++;
                }
            }
            if (versionCounter == 0)
            {
                string tempPath = Path.GetTempPath() + Application.ProductName;
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                Directory.CreateDirectory(tempPath);
                File.WriteAllBytes(tempPath + @"\" + "NDP451-KB2859818-Web.exe", Properties.Resources.NDP451_KB2859818_Web);
                try
                {
                    Process InstallProcess = new Process();
                    InstallProcess.StartInfo.FileName = tempPath + @"\" + "NDP451-KB2859818-Web.exe";
                    InstallProcess.StartInfo.Arguments = "File";
                    InstallProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    InstallProcess.Start();
                    WindowHelper.BringProcessToFront(InstallProcess);
                    InstallProcess.WaitForExit();
                    frameWorkKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
                    if (frameWorkKey != null)
                    {
                        startInstall = true;
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                catch
                {
                    Application.Exit();
                }
            }
            else
            {
                startInstall = true;
            }
            if (startInstall)
            {
                string tempPath = Path.GetTempPath() + Application.ProductName;
                if (Directory.Exists(tempPath))
                {
                    try
                    {
                        Directory.Delete(tempPath, true);
                    }
                    catch(Exception ex)
                    {
                        if(ex.Message== "Access to the path 'codeachi-library-setup.exe' is denied.")
                        {
                            MessageBox.Show("The setup application is already running.", Application.ProductName,
                              MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Application.Exit();
                        }
                    }
                }
               
                try
                {
                    Directory.CreateDirectory(tempPath);
                    File.WriteAllBytes(tempPath + @"\" + Application.ProductName + ".exe", Properties.Resources.CodeAchi_LMS_Search);
                    Process InstallProcess = new Process();
                    InstallProcess.StartInfo.FileName = tempPath + @"\" + Application.ProductName + ".exe";
                    InstallProcess.StartInfo.Arguments = "File";
                    InstallProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    InstallProcess.Start();
                    WindowHelper.BringProcessToFront(InstallProcess);
                }
                catch
                {

                }
                Application.Exit();
            }
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
