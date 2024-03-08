using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    public partial class FormUpdate : Form
    {
        public FormUpdate()
        {
            InitializeComponent();
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            using (ZipFile zip = ZipFile.Read(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+@"\"+Application.ProductName + @"\Setup files.zip"))
            {
                if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files"))
                {
                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files", true);
                }
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files");
                zip.ExtractAll(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files\");
            }
            string[] fileList = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files");
            foreach(string fileName in fileList)
            {
                File.Copy(fileName, Application.StartupPath + @"\" + Path.GetFileName(fileName),true);
            }
            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files", true);
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Application.ProductName + @"\Setup files.zip");
            MessageBox.Show("Updated successfully.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
            Process.Start(Application.StartupPath + @"\" + Application.ProductName + ".exe");
            Application.Exit();
        }
    }
}
