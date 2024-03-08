using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    static class Program
    {
        private static Mutex myMutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool createdNew;
            myMutex = new Mutex(true, Application.ProductName, out createdNew);
            if (createdNew)
            {
                Application.Run(new FormSplash());
            }
            else
                MessageBox.Show("The application is already running.", Application.ProductName,
                  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
    }
}
