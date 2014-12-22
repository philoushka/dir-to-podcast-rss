using System.Linq;
using DIYPodcastRss.Core.Model;

namespace DIY_PodcastRss.Models
{
    public class FeedNotification
    {
        public UserFeed UserFeed { get; set; }
        public string SendTo { get; set; }
        public bool IsEmailAddress
        {
            get { return this.SendTo.Contains('@'); }
        }
    }
}