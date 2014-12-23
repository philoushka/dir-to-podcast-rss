using System.Web;
using System.Web.Mvc;
using DIY_PodcastRss.Utils;
namespace DIY_PodcastRss {
    public class LogConfig {
        public static void EnsureLoggingSetup() {
            Logger.EnsureLogDirectoryExists();
        }
    }
}
