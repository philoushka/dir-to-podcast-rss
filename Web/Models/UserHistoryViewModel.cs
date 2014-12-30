using DiyPodcastRss.Core.Model;
using System.Collections.Generic;

namespace DiyPodcastRss.Web.ViewModels
{
    public class UserHistoryViewModel
    {
        public IEnumerable<UserFeed> Feeds { get; set; }

    }
}
