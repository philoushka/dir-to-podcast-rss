using System;

namespace DiyPodcastRss.Web.Extensions
{
    public static class DateTimes
    {
        public static string FriendlyFormat(this DateTime? input)
        {
            if (input.HasValue)
            {
                return input.Value.FriendlyFormat();
            }
            return "";
        }

        public static string FriendlyFormat(this DateTime input)
        {
            return input.ToString("dd-MMM-yyyy HH:mm");
        }

        public static string FriendlyFormatDateOnly(this DateTime input)
        {
            return input.ToString("dd-MMM-yyyy");
        }
    }
}