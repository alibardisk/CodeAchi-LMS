using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SetupPreChecking
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
            //bool createdNew;
            //myMutex = new Mutex(true, Application.ProductName, out createdNew);
            //if (createdNew)
            //{
                Application.Run(new FormChecking());
            //}
            //else
            //    MessageBox.Show("The application is already running.", Application.ProductName,
            //      MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
