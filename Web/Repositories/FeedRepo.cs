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
                if (userFeed.DeletedOnUtc.HasValue == false)
                {
                    yield return userFeed;
                }
            }
        }

        public UserFeed GetFeed(string token)
        {
            string filePath = Path.Combine(FeedsDir, token + ".json");
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
    }
}