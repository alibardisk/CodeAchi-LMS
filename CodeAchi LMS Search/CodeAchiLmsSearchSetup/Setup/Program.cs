using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Setup
{
    static class Program
    {
        //private static Mutex myMutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            //bool createdNew;
            //myMutex = new Mutex(true, Application.ProductName, out createdNew);
            //if (createdNew)
            //{
                Application.Run(new FormInstall());
            //}
            //else
            //    MessageBox.Show("This Setup Programm is already running.", Application.ProductName,
            //      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Setup.DotNetZip.dll"))
            {
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
            throw new NotImplementedException();
        }
    }
}
