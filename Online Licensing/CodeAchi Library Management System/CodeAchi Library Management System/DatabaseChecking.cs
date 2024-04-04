using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    class DatabaseChecking
    {
        public static void CreerBase()
        {
            if (globalVarLms.sqliteData)
            {
                SQLiteConnection sqltConn = ConnectionClass.sqliteConnection();
                if (sqltConn.State == ConnectionState.Closed)
                {
                    sqltConn.Open();
                }
                SQLiteCommand sqltCommnd = sqltConn.CreateCommand();
                SQLiteCommand sqltCommnd1 = sqltConn.CreateCommand();

                // Create table if it does not exist
                sqltCommnd.CommandText = "CREATE TABLE IF NOT EXISTS userActivity (id INTEGER PRIMARY KEY AUTOINCREMENT, userId varchar, itemAccn varchar, brrId varchar, taskDesc varchar,dateTime varchar);";
                sqltCommnd.ExecuteNonQuery();

                sqltCommnd.CommandText = "CREATE TABLE IF NOT EXISTS noticeTemplate (id INTEGER PRIMARY KEY AUTOINCREMENT, tempName varchar, senderId varchar, htmlBody boolean, bodyText varchar,noticeType varchar);";
                sqltCommnd.ExecuteNonQuery();

                sqltCommnd.CommandText = "CREATE TABLE IF NOT EXISTS wishList (id INTEGER PRIMARY KEY AUTOINCREMENT,itemType varchar, itemTitle varchar, itemAuthor varchar, itemPublication varchar, queryDate varchar,queryCount varchar);";
                sqltCommnd.ExecuteNonQuery();

                sqltCommnd.CommandText = "CREATE TABLE IF NOT EXISTS reservationList (id INTEGER PRIMARY KEY AUTOINCREMENT,brrId varchar, itemAccn varchar, itemTitle varchar, itemAuthor varchar, reserveLocation varchar,reserveDate varchar,availableDate varchar);";
                sqltCommnd.ExecuteNonQuery();

                //Delete table
                sqltCommnd.CommandText = "DROP TABLE IF EXISTS opakReservation;";
                sqltCommnd.ExecuteNonQuery();

                //Delete table
                sqltCommnd.CommandText = "DROP TABLE IF EXISTS reportSetting;";
                sqltCommnd.ExecuteNonQuery();

                bool isExist = columExsist("userActivity", "daeTime", sqltConn);
                if (isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE userActivity RENAME COLUMN daeTime TO dateTime;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "issudItems", sqltConn);
                if (isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "CREATE TABLE t1_backup AS SELECT id,brrId,brrName,brrCategory,brrAddress,brrGender,brrMailId,brrContact,mbershipDuration,addInfo1,addInfo2," +
                                            "addInfo3,addInfo4,addInfo5,entryDate,brrImage,opkPermission,renewDate FROM borrowerDetails;";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "DROP TABLE borrowerDetails";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "ALTER TABLE t1_backup RENAME TO borrowerDetails;";
                    sqltCommnd.CommandType = CommandType.Text;
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "wizardSuccess", sqltConn);
                if (isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings RENAME COLUMN wizardSuccess TO productBlocked;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "isActivate", sqltConn);
                if (isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings RENAME COLUMN isActivate TO productExpired;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "reserveSystemLimit", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN reserveSystemLimit INTEGER;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "opacAvailable", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN opacAvailable BOOL;";
                    sqltCommnd.ExecuteNonQuery();
                    sqltCommnd.CommandText = "update generalSettings set opacAvailable='" + false + "';";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "machineId", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN machineId varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "notificationData", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN notificationData varchar;";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update generalSettings set notificationData=:notificationData;";
                    sqltCommnd.Parameters.AddWithValue("notificationData", getNotificationData());
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "settingsData", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN settingsData text;";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update generalSettings set settingsData=:settingsData;";
                    sqltCommnd.Parameters.AddWithValue("settingsData", getSettingsData());
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerSettings", "avoidFine", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerSettings ADD COLUMN avoidFine BOOL;";
                    sqltCommnd.ExecuteNonQuery();
                    sqltCommnd.CommandText = "update borrowerSettings set avoidFine='" + false + "';";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("paymentDetails", "paymentMode", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE paymentDetails ADD COLUMN paymentMode varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("paymentDetails", "paymentRef", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE paymentDetails ADD COLUMN paymentRef varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("itemDetails", "digitalReference", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE itemDetails ADD COLUMN digitalReference varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerSettings", "defaultPass", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerSettings ADD COLUMN defaultPass varchar;";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update borrowerSettings set defaultPass='12345';";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "brrPass", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerDetails ADD COLUMN brrPass varchar;";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update borrowerDetails set brrPass='12345';";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "memPlan", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerDetails ADD COLUMN memPlan varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "memFreq", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerDetails ADD COLUMN memFreq varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("accnSetting", "noPrefix", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE accnSetting ADD COLUMN noPrefix BOOL;";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update accnSetting set noPrefix='" + false + "';";
                    sqltCommnd.ExecuteNonQuery();
                }

                sqltCommnd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='mbershipSetting';";
                if (sqltCommnd.ExecuteScalar().ToString() == "0")
                {
                    sqltCommnd.CommandText = "CREATE TABLE IF NOT EXISTS mbershipSetting (id INTEGER PRIMARY KEY AUTOINCREMENT,membrshpName varchar,planFreqncy varchar, membrDurtn varchar, membrFees varchar,issueLimit varchar,planDesc varchar);";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "insert into mbershipSetting (membrshpName,planFreqncy,membrDurtn, membrFees,issueLimit,planDesc) " +
                        "values ('Yearly','365_Yearly','365','0','6','default');";
                    sqltCommnd.ExecuteNonQuery();

                    sqltCommnd.CommandText = "update borrowerDetails set memPlan='Yearly',memFreq='1'";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("generalSettings", "mngmntMail", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE generalSettings ADD COLUMN mngmntMail varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "addnlMail", sqltConn);
                if (!isExist)
                {
                    // Create change column name
                    sqltCommnd.CommandText = "ALTER TABLE borrowerDetails ADD COLUMN addnlMail varchar;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("itemDetails", "itemImage", sqltConn);
                if (isExist)
                {
                    string catlogImagePath = Properties.Settings.Default.databasePath + @"\CatlogImage";
                    if (!Directory.Exists(catlogImagePath))
                    {
                        Directory.CreateDirectory(catlogImagePath);
                    }
                    sqltCommnd.CommandText = "SELECT itemAccession,itemImage from itemDetails where itemImage!='base64String' and itemImage!='';";
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    string imageName = "";
                    MemoryStream memoryStream;
                    byte[] imageBytes;
                    Image catlogImage;
                    while (dataReader.Read())
                    {
                        try
                        {
                            imageBytes = Convert.FromBase64String(dataReader["itemImage"].ToString());
                            imageName = dataReader["itemAccession"].ToString() + ".jpg";
                            memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            catlogImage = System.Drawing.Image.FromStream(memoryStream, true);
                            if (!File.Exists(catlogImagePath + @"\" + imageName))
                            {
                                catlogImage.Save(catlogImagePath + @"\" + imageName, ImageFormat.Jpeg);
                            }
                        }
                        catch
                        {
                            imageName = "";
                        }
                        sqltCommnd1.CommandText = "Update itemDetails SET itemImage='" + imageName + "' where itemAccession='" + dataReader["itemAccession"].ToString() + "'";
                        sqltCommnd1.ExecuteNonQuery();
                    }
                    dataReader.Close();

                    sqltCommnd.CommandText = "ALTER TABLE itemDetails RENAME COLUMN itemImage TO imagePath;";
                    sqltCommnd.ExecuteNonQuery();
                }

                isExist = columExsist("borrowerDetails", "brrImage", sqltConn);
                if (isExist)
                {
                    string catlogImagePath = Properties.Settings.Default.databasePath + @"\BorrowerImage";
                    if (!Directory.Exists(catlogImagePath))
                    {
                        Directory.CreateDirectory(catlogImagePath);
                    }
                    sqltCommnd.CommandText = "SELECT brrId,brrImage from borrowerDetails where brrImage!='base64String' and brrImage!='';";
                    SQLiteDataReader dataReader = sqltCommnd.ExecuteReader();
                    string imageName = "";
                    MemoryStream memoryStream;
                    byte[] imageBytes;
                    Image catlogImage;
                    while (dataReader.Read())
                    {
                        try
                        {
                            imageBytes = Convert.FromBase64String(dataReader["brrImage"].ToString());
                            imageName = dataReader["brrId"].ToString() + ".jpg";
                            memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            memoryStream.Write(imageBytes, 0, imageBytes.Length);
                            catlogImage = System.Drawing.Image.FromStream(memoryStream, true);
                            if (!File.Exists(catlogImagePath + @"\" + imageName))
                            {
                                catlogImage.Save(catlogImagePath + @"\" + imageName, ImageFormat.Jpeg);
                            }
                        }
                        catch
                        {
                            imageName = "";
                        }
                        sqltCommnd1.CommandText = "Update borrowerDetails SET brrImage='" + imageName + "' where brrId='" + dataReader["brrId"].ToString() + "'";
                        sqltCommnd1.ExecuteNonQuery();
                    }
                    dataReader.Close();
                    sqltCommnd.CommandText = "ALTER TABLE borrowerDetails RENAME COLUMN brrImage TO imagePath;";
                    sqltCommnd.ExecuteNonQuery();
                    try
                    {
                        sqltCommnd.CommandText = "VACUUM;";
                        sqltCommnd.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
                }

                sqltCommnd.Dispose();
                sqltConn.Close();
                sqltConn.Dispose();
            }
            else
            {
                string[] dataList = Properties.Settings.Default.databasePath.Split(';');
                MySqlConnection mysqlConn;
                mysqlConn = ConnectionClass.mysqlConnection();
                if (mysqlConn.State == ConnectionState.Closed)
                {
                    try
                    {
                        mysqlConn.Open();
                        string queryString = "SELECT count(*) AS TOTALNUMBEROFTABLES FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + dataList[1].Replace(" database=", "") + "'; ";
                        MySqlCommand mysqlCmd;
                        mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                        mysqlCmd.CommandTimeout = 99999;
                        if (Convert.ToInt32(mysqlCmd.ExecuteScalar().ToString()) < Properties.Settings.Default.tableCount)
                        {
                            mysqlConn.Close();
                            MessageBox.Show("Some table does not exist. Please check your connection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FormServerConnection serverConnection = new FormServerConnection();
                            serverConnection.connectionFail = true;
                            serverConnection.ShowDialog();
                        }
                        else
                        {
                            bool isExist = sqlColumExsist("borrower_details", "addnlMail", mysqlConn);
                            if (!isExist)
                            {
                                // Create change column name
                                queryString = "ALTER TABLE borrower_details ADD COLUMN addnlMail varchar(100);";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                mysqlCmd.ExecuteNonQuery();
                            }

                            isExist = sqlColumExsist("general_settings", "mngmntMail", mysqlConn);
                            if (!isExist)
                            {
                                // Create change column name
                                queryString = "ALTER TABLE general_settings ADD COLUMN mngmntMail varchar(100);";
                                mysqlCmd = new MySqlCommand(queryString, mysqlConn);
                                mysqlCmd.CommandTimeout = 99999;
                                mysqlCmd.ExecuteNonQuery();
                            }
                            mysqlConn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FormServerConnection serverConnection = new FormServerConnection();
                        serverConnection.connectionFail = true;
                        serverConnection.ShowDialog();
                    }
                }
            }
        }

        public static bool columExsist(string tableName, string columnName, SQLiteConnection sqltConn)
        {
            DataTable colsTable = sqltConn.GetSchema("Columns");
            var data = colsTable.Select("COLUMN_NAME='" + columnName + "' AND TABLE_NAME='" + tableName + "'");
            return data.Length == 1;
        }

        public static bool sqlColumExsist(string tableName, string columnName, MySqlConnection mysqlConn)
        {
            DataTable colsTable = mysqlConn.GetSchema("Columns");
            var data = colsTable.Select("COLUMN_NAME='" + columnName + "' AND TABLE_NAME='" + tableName + "'");
            return data.Length == 1;
        }

        private static string getNotificationData()
        {
            string notificationData = "";
            if (Properties.Settings.Default.notificationData == "")
            {
                notificationData = "{\"IssueMail\" : \"" + false + "\"," +
                                  "\"IssueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"IssueSms\" : \"" + false + "\"," +
                                  "\"IssueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReissueMail\" : \"" + false + "\"," +
                                  "\"ReissueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReissueSms\" : \"" + false + "\"," +
                                  "\"ReissueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReturnMail\" : \"" + false + "\"," +
                                  "\"ReturnMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReturnSms\" : \"" + false + "\"," +
                                  "\"ReturnSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReservedMail\" : \"" + false + "\"," +
                                  "\"ReservedMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ReservedSms\" : \"" + false + "\"," +
                                  "\"ReservedSmsTemplate\" : \"" + "Template" + "\"," +
                                   "\"ArrivedMail\" : \"" + false + "\"," +
                                  "\"ArrivedMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"ArrivedSms\" : \"" + false + "\"," +
                                  "\"ArrivedSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueMail\" : \"" + false + "\"," +
                                  "\"DueMailTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueSms\" : \"" + false + "\"," +
                                  "\"DueSmsTemplate\" : \"" + "Template" + "\"," +
                                  "\"DueNotificationDate\" : \"" + "Date" + "\"," +
                                  "\"DueNotificationCondition\" : \"" + "Condition" + "\"" + "}";
            }
            else
            {
                notificationData = Properties.Settings.Default.notificationData;
            }
            return notificationData;
        }

        private static string getSettingsData()
        {
            string settingsData = "{\"mailType\" : \"" + Properties.Settings.Default.mailType + "\"," +
                                   "\"mailId\" : \"" + Properties.Settings.Default.mailId + "\"," +
                                   "\"mailPassword\" : \"" + Properties.Settings.Default.mailPassword + "\"," +
                                   "\"mailHost\" : \"" + Properties.Settings.Default.mailHost + "\"," +
                                   "\"mailPort\" : \"" + Properties.Settings.Default.mailPort + "\"," +
                                   "\"mailSsl\" : \"" + Properties.Settings.Default.mailSsl + "\"," +
                                   "\"smsApi\" : \"" + Properties.Settings.Default.smsApi + "\"," +
                                    "\"blockedMail\" : \"" + Properties.Settings.Default.blockedMail + "\"," +
                                   "\"blockedContact\" : \"" + Properties.Settings.Default.blockedContact + "\"," +
                                   "\"reserveDay\" : \"" + Properties.Settings.Default.reserveDay + "\"," +
                                   "\"hostName\" : \"" + Properties.Settings.Default.hostName + "\"," +
                                   "\"databaseSeries\" : \"" + Properties.Settings.Default.databaseSeries + "\"" + "}";
            return settingsData;
        }
    }
}
