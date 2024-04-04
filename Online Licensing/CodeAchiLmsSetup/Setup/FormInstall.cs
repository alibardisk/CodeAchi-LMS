using Ionic.Zip;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Setup
{
    public partial class FormInstall : Form
    {
        public FormInstall()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        string installFolder = Application.CompanyName + @"\" + Application.ProductName;
        public static string stepName;
        string installPath = "", startMenuPath = "", macAddress="";
        bool deskShortcut = false, startMenu = false;

        private void FormInstall_Load(object sender, EventArgs e)
        {
            this.Text = "Setup - " + Application.ProductName + " " + Application.ProductVersion;
            lblWlcmeHeadr.Text = "Welcome to the " + Application.ProductName + " Setup Wizard";
            lblHeader.Visible = false;
            lblInfo.Visible = false;
            btnFinish.Text = "Cancel";
            btnBack.Visible = false;
            stepName = "Welcome";
            RegistryKey regKey = (Registry.LocalMachine).OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\" + Application.ProductName, true);
            if(regKey!=null)
            {
                if(MessageBox.Show(Application.ProductName+" is already installed"+Environment.NewLine+"Do you want to continue?",Application.ProductName+" Setup",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    Process[] localByName = Process.GetProcessesByName(Application.ProductName);
                    if(localByName.Length>=1)
                    {
                        if(MessageBox.Show(Application.ProductName+" is already running.."+Environment.NewLine+"Do you want to continue?",Application.ProductName+ " Setup",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                        {
                            foreach (Process currProcess in localByName)
                            {
                                if (currProcess.MainWindowTitle != "Setup - " + Application.ProductName + " " + Application.ProductVersion)
                                {
                                    currProcess.Kill();
                                    currProcess.WaitForExit();
                                }
                            }
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                    string installLocation = regKey.GetValue("InstallLocation").ToString();
                    if(Directory.Exists(installLocation))
                    {
                        Directory.Delete(installLocation, true);
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void pnlLicense_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (btnNext.Text == "<< Back")
            {
                //panelProgress.Visible = false;
                btnNext.Text = "Next >>";
                btnBack.Visible = true;
                stepName = "Task";
                btnFinish.Text = "Cancel";
            }
            else
            {
                if (stepName == "Welcome")
                {
                    lblHeader.Visible = true;
                    lblInfo.Visible = true;
                    lblHeader.Text = "License Agreement";
                    lblInfo.Text = "Read the following important information before continuing.";
                    if (System.IO.File.Exists(Path.GetTempPath() + @"\Software License Agreement.rtf"))
                    {
                        System.IO.File.Delete(Path.GetTempPath() + @"\\Software License Agreement.rtf");
                    }
                    System.IO.File.WriteAllBytes(Path.GetTempPath() + @"\License.zip", Properties.Resources.License);
                    using (ZipFile zip = ZipFile.Read(Path.GetTempPath() + @"License.zip"))
                    {
                        zip.ExtractAll(Path.GetTempPath());
                    }
                    pnlFinish.Visible = false;
                    pnlAppFolder.Visible = false;
                    pnlHome.Visible = false;
                    txtbLicense.LoadFile(Path.GetTempPath() + @"\Software License Agreement.rtf");
                    rdbNot.Checked = true;
                    btnBack.Visible = true;
                    btnNext.Enabled = false;
                    stepName = "License";
                }
                else if (stepName == "License")
                {
                    lblHeader.Visible = true;
                    lblInfo.Visible = true;
                    lblHeader.Text = "Select Application Folder";
                    lblInfo.Text = "Please choose the directory for the installation.";

                    pnlAppFolder.Visible = true;
                    pnlFinish.Visible = false;
                    pnlTask.Visible = false;
                    pnlHome.Visible = false;

                    btnBack.Visible = true;
                    lblAppText.Text = "Setup will install " + Application.ProductName.Replace("&", "&&") + " in the folder shown below.";
                    txtbPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + installFolder;// Application.ProductName;
                    System.IO.DriveInfo cdrive;

                    cdrive = new System.IO.DriveInfo(@"C:\");
                    string SizeType = "";

                    Double theSize = cdrive.TotalFreeSpace;
                    if (theSize < 1000)
                    {
                        SizeType = "B";
                    }
                    else if (theSize < 1000000000)
                    {
                        if (theSize < 1000000)
                        {
                            SizeType = "KB";
                            theSize = theSize / 1024;
                        }
                        else
                        {
                            SizeType = "MB";
                            theSize = theSize / (1024 * 1024);
                        }
                    }
                    else
                    {
                        theSize = theSize / (1024 * 1024 * 1024);
                        SizeType = "GB";
                    }
                    Label6.Text = Label6.Text + (Math.Round(theSize, 2)).ToString() + " " + SizeType;
                    stepName = "AppFolder";
                }
                else if (stepName == "AppFolder")
                {
                    lblHeader.Text = "Select Additional Tasks";
                    lblInfo.Text = "Which additional tasks should be performed?";
                    installPath = txtbPath.Text;
                    pnlTask.Visible = true;
                    pnlFinish.Visible = false;
                    pnlProgress.Visible = false;
                    pnlHome.Visible = false;
                    btnBack.Visible = true;
                    lblTask.Text = "Select the additional tasks you would like setup to perform while installing " + Application.ProductName.Replace("&", "&&") + ". then click Next.";
                    stepName = "Task";
                    string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
                    if (!Directory.Exists(databasePath))
                    {
                        chkbDatabase.Checked = false;
                        chkbDatabase.Enabled = false;
                    }
                    else
                    {
                        if (!System.IO.File.Exists(databasePath + @"\LMS.sl3"))
                        {
                            chkbDatabase.Checked = false;
                            chkbDatabase.Enabled = false;
                        }
                        else
                        {
                            chkbDatabase.Checked = true;
                            chkbDatabase.Enabled = true;
                        }
                    }
                }
                else if (stepName == "Task")
                {
                    lblHeader.Text = "Install";
                    lblInfo.Text = "Please wait while Setup installs on your computer.";
                    if (chkbDesktop.Checked == true)
                    {
                        deskShortcut = true;
                    }
                    if (chkbStartMenu.Checked == true)
                    {
                        startMenu = true;
                        startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu) + @"\" + Application.ProductName;
                    }
                    pnlProgress.Visible = true;
                    pnlFinish.Visible = false;
                    pnlHome.Visible = false;
                    //btnBack.Visible = false;
                    //btnNext.Text = "Install";
                    btnFinish.Enabled = false;
                    if (!System.IO.Directory.Exists(installPath))
                    {
                        System.IO.Directory.CreateDirectory(installPath);
                    }
                    //stepName = "Progress";
                    //label9.Text = "Please click on Install to continue.";
                //}
                //else if (stepName == "Progress")
                //{
                    //////lblWelcome_text.Text = "Installing..";
                    label9.Text = "Configuring...";
                    Application.DoEvents();
                    btnNext.Enabled = false;
                    if (System.IO.File.Exists(installPath + @"\UNINSTALL.exe"))
                    {
                        System.IO.File.Delete(installPath + @"\UNINSTALL.exe");
                    }
                    if (System.IO.Directory.Exists(installPath))
                    {
                        DirectoryInfo installedDirectory = new DirectoryInfo(installPath);
                        foreach (System.IO.FileInfo eachFile in installedDirectory.GetFiles()) eachFile.Delete();
                    }
                    ProgressBar1.Value = 10;
                    System.IO.File.WriteAllBytes(installPath + @"\Setup files.zip", Properties.Resources.Setup_files);
                    using (ZipFile zip = ZipFile.Read(installPath + @"\Setup files.zip"))
                    {
                        zip.ExtractAll(installPath + @"\");
                    }
                    //ZipFile.ExtractToDirectory(globalVar.installPath + @"\Setup files.zip", globalVar.installPath + @"\");
                    System.IO.File.Delete(installPath + @"\Setup files.zip");
                    ProgressBar1.Value = 60;
                    WshShell WshShell = new WshShell();

                    if (deskShortcut)
                    {
                        //Creating Desktop shortcut 
                        string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                        IWshShortcut desktopShortcut = (IWshShortcut)(WshShell.CreateShortcut(Path.Combine(shortcutPath, Application.ProductName) + ".lnk"));
                        desktopShortcut.TargetPath = installPath + @"\" + Application.ProductName + ".exe";
                        desktopShortcut.IconLocation = installPath + @"\icon.ico";
                        desktopShortcut.WorkingDirectory = installPath;
                        desktopShortcut.Save();
                    }
                    ProgressBar1.Value = 70;
                    if (startMenu)
                    {
                        //Creating start menu shortcut 
                        if (!System.IO.Directory.Exists(startMenuPath))
                            System.IO.Directory.CreateDirectory(startMenuPath);

                        string shortcutPath = startMenuPath;
                        IWshShortcut startmenuShortcut = (IWshShortcut)(WshShell.CreateShortcut(System.IO.Path.Combine(shortcutPath, Application.ProductName) + ".lnk"));
                        startmenuShortcut.TargetPath = installPath + @"\" + Application.ProductName + ".exe";
                        startmenuShortcut.IconLocation = installPath + @"\icon.ico";
                        startmenuShortcut.WorkingDirectory = installPath;
                        startmenuShortcut.Save();
                    }
                    ProgressBar1.Value = 80;
                    string databasePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName;
                   
                    if (!chkbDatabase.Checked)
                    {
                        Directory.CreateDirectory(databasePath);
                        System.IO.File.WriteAllBytes(databasePath + @"\LMS.sl3", Properties.Resources.LMS);
                    }
                    else
                    {
                        databasePath = "";
                    }

                    if (databasePath != "")
                    {
                        GrantAccess(databasePath);
                    }
                    //////string installTime = DateTime.Now.ToString("hh:mm:ss");
                    //////string installedDate = DateTime.Now.Day.ToString("00") + ":" + DateTime.Now.Month.ToString("00") + ":"+installTime+":" + DateTime.Now.Year.ToString("0000");
                    //////macAddress = GetMACAddress()+":"+installedDate;

                    //////byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
                    //////byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);

                    //////RegistryKey oldregKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                    //////RegistryKey regKey = null;
                    //////if(Environment.Is64BitProcess)
                    //////{
                    //////    regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                    //////    if (regKey == null)
                    //////    {
                    //////        regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                    //////        if (oldregKey != null)
                    //////        {
                    //////            foreach (var name in oldregKey.GetValueNames())
                    //////            {
                    //////                regKey.SetValue(name, oldregKey.GetValue(name), oldregKey.GetValueKind(name));
                    //////            }
                    //////            Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                    //////        }
                    //////        else
                    //////        {
                    //////            byteData = Encoding.UTF8.GetBytes(macAddress);
                    //////            regKey.SetValue("Data1", Convert.ToBase64String(byteData));
                    //////            byteData = Encoding.UTF8.GetBytes(databasePath);
                    //////            regKey.SetValue("Data2", Convert.ToBase64String(byteData));
                    //////        }
                    //////    }
                    //////    else
                    //////    {
                    //////        if (databasePath != "")
                    //////        {
                    //////            byteData = Encoding.UTF8.GetBytes(databasePath);
                    //////            regKey.SetValue("Data2", Convert.ToBase64String(byteData));
                    //////        }
                    //////    }
                    //////}
                    //////else
                    //////{
                    //////    regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
                    //////    if (regKey == null)
                    //////    {
                    //////        regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                    //////        if (oldregKey != null)
                    //////        {
                    //////            foreach (var name in oldregKey.GetValueNames())
                    //////            {
                    //////                regKey.SetValue(name, oldregKey.GetValue(name), oldregKey.GetValueKind(name));
                    //////            }
                    //////            Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData));
                    //////        }
                    //////        else
                    //////        {
                    //////            byteData = Encoding.UTF8.GetBytes(macAddress);
                    //////            regKey.SetValue("Data1", Convert.ToBase64String(byteData));
                    //////            byteData = Encoding.UTF8.GetBytes(databasePath);
                    //////            regKey.SetValue("Data2", Convert.ToBase64String(byteData));
                    //////        }
                    //////    }
                    //////    else
                    //////    {
                    //////        if (databasePath != "")
                    //////        {
                    //////            byteData = Encoding.UTF8.GetBytes(databasePath);
                    //////            regKey.SetValue("Data2", Convert.ToBase64String(byteData));
                    //////        }
                    //////    }
                    //////}

                    //////if (regKey != null)
                    //////{
                    //////    object o1 = regKey.GetValue("Data1");
                    //////    byteData = Convert.FromBase64String(o1.ToString());
                    //////    macAddress = Encoding.UTF8.GetString(byteData);
                    //////}
                    
                    string installReglocation = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

                    RegistryKey key = (Registry.LocalMachine).OpenSubKey(installReglocation, true);

                    //Creating my AppRegistryKey            
                    RegistryKey AppKey = key.CreateSubKey(Application.ProductName);

                    //Adding my values to my AppRegistryKey
                    AppKey.SetValue("DisplayName", Application.ProductName, Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("DisplayVersion", Application.ProductVersion, Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("DisplayIcon", installPath + @"\icon.ico", Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("Publisher", Application.ProductName, Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("UninstallString", installPath + @"\UNINSTALL.exe", Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("UninstallPath", installPath + @"\UNINSTALL.exe", Microsoft.Win32.RegistryValueKind.String);
                    AppKey.SetValue("InstallLocation", installPath + @"\", Microsoft.Win32.RegistryValueKind.String);
                    stepName = "Finish";
                    pnlHome.Visible = false;
                    pnlFinish.Visible = true;

                    lblFinish.Text = "Completing the " + Application.ProductName.Replace("&", "&&") + " Setup Wizard";
                    lblFinishInfo.Text = "Setup has finished installing " + Application.ProductName.Replace("&", "&&") + " on your computer. Click Finish to exit Setup.";
                    chkbLaunch.Text = "Launch " + Application.ProductName;
                    chkbLaunch.Checked = true;
                    btnNext.Visible = false;
                    btnFinish.Text = "Finish";
                    btnFinish.Enabled = true;
                    btnBack.Visible = false;
                    stepName = "Finish";
                }
            }
        }

        private bool GrantAccess(string fullPath)
        {
            DirectoryInfo dInfo = new DirectoryInfo(fullPath);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                FileSystemRights.FullControl,
                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.NoPropagateInherit,
                AccessControlType.Allow));

            dInfo.SetAccessControl(dSecurity);
            return true;
        }

        //public static string StringToBinary(string data)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (char c in data.ToCharArray())
        //    {
        //        sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        //    }
        //    return sb.ToString();
        //}

        //public static string BinaryToString(string data)
        //{
        //    List<Byte> byteList = new List<Byte>();

        //    for (int i = 0; i < data.Length; i += 8)
        //    {
        //        byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
        //    }
        //    return Encoding.ASCII.GetString(byteList.ToArray());
        //}

        private void rdbAccept_CheckedChanged(object sender, EventArgs e)
        {
            if (stepName == "License")
            {
                if (rdbAccept.Checked == true)
                {
                    btnNext.Enabled = true;
                }
                else
                {
                    btnNext.Enabled = false;
                }
            }
        }

        private void pnlAppFolder_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog flderDialog = new FolderBrowserDialog();
            flderDialog.Description = Application.ProductName + " - Select Intall Folder";
            if (flderDialog.ShowDialog() == DialogResult.OK)
            {
                txtbPath.Text = flderDialog.SelectedPath + @"\" + installFolder;//Application.ProductName;
            }
            else
            {
                txtbPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\" + installFolder;//Application.ProductName;
            }
        }

        private void pnlTask_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void pnlProgress_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void pnlFinish_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, p.DisplayRectangle, Color.DimGray, ButtonBorderStyle.Dashed);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (stepName == "License")
            {
                pnlHome.Visible = true;
                stepName = "Welcome";
                btnBack.Visible = false;
                btnNext.Enabled = true;
            }
            else if (stepName == "AppFolder")
            {
                pnlAppFolder.Visible = false;
                stepName = "License";
            }
            else if (stepName == "Task")
            {
                pnlTask.Visible = false;
                stepName = "AppFolder";
            }
            else if (stepName == "Progress")
            {
                pnlProgress.Visible = false;
                stepName = "Task";
                btnFinish.Enabled = true;
                btnNext.Text = "Next >>";
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (btnFinish.Text == "Finish")
            {
                btnFinish.Enabled = false;
                if(System.IO.File.Exists(installPath + @"\sourcefile.txt"))
                {
                    System.IO.File.Delete(installPath + @"\sourcefile.txt");
                }
                System.IO.File.WriteAllText(installPath + @"\sourcefile.txt", typeof(Program).Assembly.GetName().Name);
                //string installedDate = DateTime.Now.Day.ToString("00") + "/" + DateTime.Now.Month.ToString("00") + "/" + DateTime.Now.Year.ToString("0000");
                //if (checkNetConnection() == true)
                //{
                //    try
                //    {
                //        string queryToCheck = "SELECT productName FROM installationDetails WHERE mac = '" + macAddress + "' and productName='" + Application.ProductName + "'";
                //        //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //        WebRequest webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/SelectData.php?Q=" + queryToCheck);
                //        webRequest.Timeout = 8000;
                //        WebResponse webResponse = webRequest.GetResponse();
                //        Stream dataStream = webResponse.GetResponseStream();
                //        StreamReader strmReader = new StreamReader(dataStream);
                //        string requestResult = strmReader.ReadLine();
                //        if (requestResult == null)
                //        {
                //            queryToCheck = "SELECT productName FROM installationDetails WHERE mac = '" + macAddress + "' and productName='" + Application.ProductName + "'";
                //            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //            webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/SelectData.php?Q=" + queryToCheck);
                //            webRequest.Timeout = 8000;
                //            webResponse = webRequest.GetResponse();
                //            dataStream = webResponse.GetResponseStream();
                //            strmReader = new StreamReader(dataStream);
                //            requestResult = strmReader.ReadLine();
                //            if (requestResult == null)
                //            {
                //                string installTime = DateTime.Now.ToString("hh:mm:ss tt");
                //                string queryToInsert = "INSERT INTO installationDetails (mac,productName,isBlocked,licenseKey,installDate,installTime) VALUES('" + macAddress + "', '" + Application.ProductName + "','" + false + "','" + "Demo" + "','" + installedDate + "','" + installTime + "')";
                //                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //                webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/InsertData.php?Q=" + queryToInsert);
                //                webRequest.Timeout = 8000;
                //                webResponse = webRequest.GetResponse();
                //            }
                //        }
                //        else
                //        {
                //           //string queryToUpdate = "update installationDetails set mac='"+macAddress+"' WHERE mac = '" + GetMACAddress() + "' and productName='" + Application.ProductName + "'";
                //           // //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //           // webRequest = WebRequest.Create("http://codeachi.com/Product/LMS/UpdateData.php?Q=" + queryToUpdate);
                //           // webRequest.Timeout = 8000;
                //           // webResponse = webRequest.GetResponse();
                //           // dataStream = webResponse.GetResponseStream();
                //           // strmReader = new StreamReader(dataStream);
                //           // requestResult = strmReader.ReadLine();
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}

                if (chkbLaunch.Checked == true)
                {
                    Process.Start(installPath + @"\" + Application.ProductName + ".exe");
                }
                //try
                //{
                //    string queryToSelect = "SELECT installUrl1,installUrl2 FROM installUninstallUrl WHERE productName='" + Application.ProductName + "'";
                //    //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //    WebRequest webRequest1 = WebRequest.Create("http://codeachi.com/Product/LMS/SelectData.php?Q=" + queryToSelect);
                //    webRequest1.Timeout = 8000;
                //    WebResponse webResponse1 = webRequest1.GetResponse();
                //    Stream dataStream1 = webResponse1.GetResponseStream();
                //    StreamReader strmReader1 = new StreamReader(dataStream1);
                //    string requestResult1 = strmReader1.ReadLine();
                //    if (requestResult1 != null && requestResult1 != "")
                //    {
                //        string[] splitData = requestResult1.Split('$');
                //        try
                //        {
                //            Process.Start(splitData[0]);
                //        }
                //        catch
                //        {

                //        }
                //        try
                //        {
                //            Process.Start(splitData[1]);
                //        }
                //        catch
                //        {

                //        }
                //    }
                //}
                //catch
                //{
                //}
                Application.Exit();
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to exit the setup?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        //public string GetMACAddress()
        //{
        //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //    String sMacAddress = string.Empty;
        //    foreach (NetworkInterface adapter in nics)
        //    {
        //        if (sMacAddress == String.Empty)// only return MAC Address from first card  
        //        {
        //            IPInterfaceProperties properties = adapter.GetIPProperties();
        //            sMacAddress = adapter.GetPhysicalAddress().ToString();
        //        }
        //    }
        //    sMacAddress = Regex.Replace(sMacAddress, ".{2}", "$0:");
        //    sMacAddress = sMacAddress.Remove(sMacAddress.Length - 1, 1);
        //    return sMacAddress;
        //}

        public static bool checkNetConnection()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}
