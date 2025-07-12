using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DateTimeExtension
    {
        private static TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        public static DateTime ConvertUtcToVNTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, vnTimeZone);
        }
        public static string FormatDateTime(DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
