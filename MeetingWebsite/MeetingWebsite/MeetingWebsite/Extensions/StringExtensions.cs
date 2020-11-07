using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetingWebsite.Extensions
{
    public static class StringExtensions
    {
        public static string Normilized(this string value)
        {
            return value.Trim().Replace(" ", "").ToUpper();
        }
    }
}
