
using DiyPodcastRss.Core.Model;
using System.Collections.Generic;


namespace DiyPodcastRss.Web.ViewModels
{
    public class AllUserFeedsViewModel
    {
        public IEnumerable<UserFeed> Feeds { get; set; }
    }
}
