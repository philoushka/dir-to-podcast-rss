using System;
namespace DIYPodcastRss.Core.Model {
    public class UserFeed {

        public string Name { get; set; }

        /// <summary>
        /// A unique id for this feed.
        /// </summary>
        public string UniqueId { get; set; }

        public string RssDoc { get; set; }

        /// <summary>
        /// the url of this host. like http://diypodcastrss.azurewebsites.net/podcastfeed/
        /// </summary>
        public string BaseUrl { get; set; }


        public Uri FeedUri { get { return new Uri(BaseUrl + UniqueId); } }
    }
}
