using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeAchi_Library_Management_System
{
    class FormatDate
    {
        public static string getUserFormat(string inputDate)
        {
            string outputDate = "";
            if (inputDate != "")
            {
                DateTime dateTime = DateTime.ParseExact(inputDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (Properties.Settings.Default.dateFormat == "dd/MM/yyyy")
                {
                    outputDate = inputDate;
                }
                else if (Properties.Settings.Default.dateFormat == "dd/MM/yy")
                {
                    outputDate = dateTime.Day.ToString("00") + "/" + dateTime.Month.ToString("00") + "/" + dateTime.Year.ToString().Substring(2, 2);
                }
                else if (Properties.Settings.Default.dateFormat == "MM/dd/yy")
                {
                    outputDate = dateTime.Month.ToString("00") + "/" + dateTime.Day.ToString("00") + "/" + dateTime.Year.ToString().Substring(2, 2);
                }
                else if (Properties.Settings.Default.dateFormat == "yy/MM/dd")
                {
                    outputDate = dateTime.Year.ToString().Substring(2, 2) + "/" + dateTime.Month.ToString("00") + "/" + dateTime.Day.ToString("00");
                }
            }
            return outputDate;
        }

        public static string getAppFormat(string inputDate)
        {
            string outputDate = "";
            if (inputDate != "")
            {
                DateTime dateTime = DateTime.ParseExact(inputDate, Properties.Settings.Default.dateFormat, CultureInfo.InvariantCulture);
                outputDate = dateTime.Day.ToString("00") + "/" + dateTime.Month.ToString("00") + "/" + dateTime.Year.ToString("0000");
            }
            return outputDate;
        }
    }
}
