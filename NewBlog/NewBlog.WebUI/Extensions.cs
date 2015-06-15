using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewBlog.WebUI
{
    public static class Extensions
    {
        public static string ToConfigLocalTime(this DateTime utcDT)
        {
            var istTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
            DateTime dTime = DateTime.SpecifyKind(utcDT, DateTimeKind.Unspecified);
            return String.Format("{0} ({1})", TimeZoneInfo.ConvertTimeFromUtc(dTime, istTZ).ToShortDateString(), ConfigurationManager.AppSettings["TimezoneAbbr"]);
        }
    }

}