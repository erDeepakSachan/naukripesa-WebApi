using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Util
{
    internal class DateTimeUtils
    {
    }
    public static class DateTimeExtensions
    {
        public static DateTime ToIST(this DateTime dateTime)
        {
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), istZone);
        }
    }

}
