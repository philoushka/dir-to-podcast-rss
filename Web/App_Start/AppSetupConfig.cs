using System.Web;
using System.Web.Mvc;
using DiyPodcastRss.Web.Utils;
using DiyPodcastRss.Web.Repositories;
namespace DiyPodcastRss.Web {
    public class AppSetupConfig {
        public static void EnsureLoggingSetup() {
            Logger.EnsureLogDirectoryExists();
        }
        public static void EnsureFeedStorageSetup()
        {
            FeedRepo.EnsureFeedDirectoryExists();
        }
    }
}
