using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_AllinOne
{
    public class DatabaseInformation
    {
        public static bool setDatabasePath(string databasePath)
        {
            bool databaseSaved = false;
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    byteData = Encoding.UTF8.GetBytes(databasePath);
                    regKey.SetValue("Data9", Convert.ToBase64String(byteData));
                    databaseSaved = true;
                    //MessageBox.Show("Save Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            return databaseSaved;
        }

        public static bool setConnectionString( string connectionString)
        {
            bool connectionStringSaved = false;
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    byteData = Encoding.UTF8.GetBytes(connectionString);
                    regKey.SetValue("Data10", Convert.ToBase64String(byteData));
                    connectionStringSaved = true;
                    //MessageBox.Show("Save Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            return connectionStringSaved;
        }

        public static string getDatabasePath()
        {
            string databasePath = "Blank";
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    object o1 = regKey.GetValue("Data9");
                    if (o1 != null)
                    {
                        byteData = Convert.FromBase64String(o1.ToString());
                        databasePath = Encoding.UTF8.GetString(byteData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return databasePath;
        }

        public static string getConnectionString()
        {
            string connectionString = "Blank";
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    object o1 = regKey.GetValue("Data10");
                    if (o1 != null)
                    {
                        byteData = Convert.FromBase64String(o1.ToString());
                        connectionString = Encoding.UTF8.GetString(byteData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            return connectionString;
        }

        public static bool setHostAddress(string hostIp)
        {
            bool databaseSaved = false;
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    byteData = Encoding.UTF8.GetBytes(hostIp);
                    regKey.SetValue("Data11", Convert.ToBase64String(byteData));
                    databaseSaved = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            return databaseSaved;
        }

        public static string gethostAddress()
        {
            string connectionString = "Blank";
            byte[] compName = Encoding.UTF8.GetBytes("CodeAchi");
            byte[] byteData = Encoding.UTF8.GetBytes(Application.ProductName);
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\" + Convert.ToBase64String(compName) + @"\" + Convert.ToBase64String(byteData), true);
            if (regKey != null)
            {
                try
                {
                    object o1 = regKey.GetValue("Data11");
                    if (o1 != null)
                    {
                        byteData = Convert.FromBase64String(o1.ToString());
                        connectionString = Encoding.UTF8.GetString(byteData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Not Exist", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            return connectionString;
        }
    }
}
