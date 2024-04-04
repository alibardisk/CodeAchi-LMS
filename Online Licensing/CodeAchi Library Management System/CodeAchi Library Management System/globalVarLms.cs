using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    class globalVarLms
    {
        public static List<string> itemList = new List<string> { };
        public static List<string> memberList = new List<string> { };
        public static List<string> priviledgeList = new List<string> { };
        public static List<String> ItemDetails = new List<String> { };

        public static int itemLimit=0;
        public static int memberLimit = 0;
        public static int librarianLimit = 0;
        public static int backupHour;

        public static bool mailSending = false;
        public static bool productExpire;
        public static bool productBlocked;
        public static bool isAdmin=false;
        public static bool opacAvailable=false;
        public static bool issueReciept;
        public static bool reissueReciept;
        public static bool returnReciept;
        public static bool paymentReciept;
        public static bool backupRequired=false;
        public static bool sqliteData;
        public static bool reportGeneration=false;
        public static bool excelImport=false;
        public static bool isLicensed=false;

        public static string licenseName;
        public static String insertApi = "http://codeachi.com/Product/LMS/InsertData.php?Q=";
        public static String selectApi = "http://codeachi.com/Product/LMS/SelectData.php?Q=";
        public static String updateApi = "http://codeachi.com/Product/LMS/UpdateData.php?Q=";
        public static String dateApi = "http://codeachi.com/Product/LMS/serverdate.php";
        public static string instName;
        public static string instAddress;
        public static string currentPassword;
        public static string currentUserName;
        public static string currentUserId;
        public static string instShortName;
        public static String currSymbol;
        public static string defaultPrinter="";
        public static string backupPath;
        public static string addCateory;
        public static string brrId;
        public static string itemAccn;
        public static string userContact;
        public static string tempValue;
        public static string machineId;
        public static string recieptPrinter;
        public static string recieptPaper;
        public static string connectionString;
        public static string blockedReason;

        public static DateTime expiryDate;
        public static DateTime currentDate;
        public static DateTime lastChecked;

        public static Bitmap bimapImage;
    }
}
