using System.Web;
using System.Web.Mvc;
using DIY_PodcastRss.Utils;
using DIY_PodcastRss.Repositories;
namespace DIY_PodcastRss {
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
