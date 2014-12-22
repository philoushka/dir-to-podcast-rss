using DIY_PodcastRss.Extensions;
using DIYPodcastRss.Core.Model;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace DIY_PodcastRss.Repositories
{
    public class FeedRepo
    {
        public string FeedsDir
        {
            get
            {
                return HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FeedStorageDir"].ToString());
            }
        }
        public IEnumerable<UserFeed> AllFeeds()
        {
            foreach (var serializedFile in Directory.GetFiles(FeedsDir, "*.json"))
            {
                string userFeedJson = File.ReadAllText(serializedFile);
                UserFeed userFeed = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<UserFeed>(userFeedJson);
                yield return userFeed;
            }
        }

        public UserFeed GetFeed(string feedToken)
        {
            string filePath = BuildFilePath(feedToken);
            if (File.Exists(filePath))
            {
                string userFeedJson = File.ReadAllText(filePath);
                return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<UserFeed>(userFeedJson);
            }
            return null;
        }


        public void SaveFeed(UserFeed userFeed)
        {
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(userFeed);
            File.WriteAllText(Path.Combine(FeedsDir, userFeed.FeedToken + ".json"), json);
        }

        public bool ReserveEmptyFeed(string feedToken)
        {
            if (feedToken.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (File.Exists(BuildFilePath(feedToken)) == false)
            {
                File.WriteAllText(Path.Combine(FeedsDir, feedToken + ".json"), "");
                return true;
            }
            return false;
        }

        public bool DeleteFeed(string feedToken)
        {
            string filePath = BuildFilePath(feedToken);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        private string BuildFilePath(string feedToken)
        {
            return Path.Combine(FeedsDir, feedToken + ".json");
        }
    }
}