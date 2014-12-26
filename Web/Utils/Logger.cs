using DIY_PodcastRss.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace DIY_PodcastRss.Utils
{
    public static class Logger
    {
        public const string LogNameDateFormat = "yyyyMMdd";
        private const string LogExtension = ".log";

        public static bool LogEnabled { get { return new[] { "true", "1" }.Contains(ConfigurationManager.AppSettings["LogEnabled"].ToString()); } }
        public static string LogDir { get { return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["LogStorageDir"].ToString()); } }
        public static string TodayLogFile { get { return Path.Combine(LogDir, DateTime.UtcNow.ToString(LogNameDateFormat) + LogExtension); } }
        public static string NowTimestamp { get { return DateTime.UtcNow.ToString("HH:mm:ss"); } }
        public static void LogMsg(params object[] logMsgParams)
        {
            if (LogEnabled == false) { return; }
            string message = string.Join(" ", logMsgParams).Trim();
            string logMsg = "\r\n{0}\t{1}".FormatWith(NowTimestamp, message);
            File.AppendAllText(TodayLogFile, logMsg);
        }

        public static void EnsureLogDirectoryExists()
        {
            if (Directory.Exists(LogDir) == false)
            {
                Directory.CreateDirectory(LogDir);
            }
        }

        public static IEnumerable<string> ReadAllForDay(string logDate)
        {
            try
            {
                return File.ReadLines(Path.Combine(LogDir, logDate + LogExtension));
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Gets the names of the files, without file extensions, of logs on disk. 
        /// i.e. 20090827, 20101119
        /// </summary>        
        public static Dictionary<string, int> ListLogs()
        {
            var logs = new Dictionary<string, int>();
            foreach (var filePath in Directory.GetFiles(LogDir, "*" + LogExtension))
            {
                int linesCount = File.ReadLines(filePath).Count();
                string file = Path.GetFileNameWithoutExtension(filePath);
                logs.Add(file, linesCount);
            }
            return logs;
        }
    }
}