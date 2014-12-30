using System.Linq;
using DiyPodcastRss.Core.Model;

namespace DiyPodcastRss.Web.Models
{
    public class FeedNotification
    {
        public UserFeed UserFeed { get; set; }
        public string SendTo { get; set; }
        public bool IsVaguelyAnEmailAddress
        {
            get { return this.SendTo.Contains('@'); }
        }
    }
}