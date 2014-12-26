using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DIY_PodcastRss.Extensions
{
    public static class Html
    {
        public static HtmlString FormatLogLine(this HtmlHelper html, string logLine)
        {
            string lineTimestamp = GetLineStartsWithTimeStamp(logLine);
            if (lineTimestamp != "")
            {
                return new HtmlString(logLine.Trim().Replace(lineTimestamp, "<span class='logTimeStamp'>{0}</span>".FormatWith(lineTimestamp)));
            }
            return new HtmlString(logLine.Trim());
        }

        private static string GetLineStartsWithTimeStamp(string line)
        {
            return (Regex.Match(line, @"^\d{2}:\d{2}\:\d{2}").Value);
        }
    }
}