using DIY_PodcastRss.Extensions;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DIY_PodcastRss.Utils
{
    public static class Logger
    {
        public static bool LogEnabled { get { return new[] { "true", "1" }.Contains(ConfigurationManager.AppSettings["LogEnabled"].ToString()); } }
        public static string LogDir { get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogStorageDir"].ToString()); } }
        public static string TodayLogFile { get { return Path.Combine(LogDir, DateTime.UtcNow.ToString("yyyyMMdd") + ".log"); } }
        public static string NowTimestamp { get { return DateTime.UtcNow.ToString("HH:mm:ss"); } }
        public static void LogMsg(params object[] logMsgParams)
        {
            if (LogEnabled == false) { return; }
            string message = string.Join(" ", logMsgParams).Trim();
            string logMsg = "\r\n{0}\t{1}".FormatWith(NowTimestamp, message );
            File.AppendAllText(TodayLogFile, logMsg);
        }

        public static void EnsureLogDirectoryExists()
        {
            if (Directory.Exists(LogDir) == false)
            {
                Directory.CreateDirectory(LogDir);
            }
        }
    }
}