using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    class ConnectionClass
    {
        public static SQLiteConnection sqliteConnection()
        {
            
            string connectionString = "Data Source=" + Properties.Settings.Default.databasePath + @"\LMS.sl3;Version=3;Password=codeachi@lmssl;";  //";Password=codeachi@lmssl;"
            SQLiteConnection sqltConn = new SQLiteConnection(connectionString,true);
            //sqltConn.Open();
            //sqltConn.ChangePassword("codeachi@lmssl");
            //sqltConn.Close();
            return sqltConn;
        }
       
        public static MySqlConnection mysqlConnection()
        {
            MySqlConnection mysqlConn;
            mysqlConn = new MySqlConnection(Properties.Settings.Default.databasePath);
            return mysqlConn;
            //set global max_allowed_packet=1024*1024*1024;
        }
    }
}
