using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace DIYPodcastRss.Core.Model
{
    public class UserFeed
    {

        public string ImgUrl { get; set; }
        public string Name { get; set; }


        public string RssDoc { get; set; }

        /// <summary>
        /// the url of this host. like http://diypodcastrss.azurewebsites.net/podcastfeed/
        /// </summary>
        public string BaseUrl { get; set; }


        public Uri FeedUri { get { return new Uri(BaseUrl + FeedToken); } }

        public DateTime CreatedOnUtc { get; set; }
        public string FeedDocument { get; set; }
        public string CreatedFromIpHost { get; set; }
        public IEnumerable<string> Files { get; set; }
        /// <summary>
        /// A unique id for this feed.
        /// </summary>
        public string FeedToken { get; set; }

        public DateTime? DeletedOnUtc { get; set; }

    }
}
