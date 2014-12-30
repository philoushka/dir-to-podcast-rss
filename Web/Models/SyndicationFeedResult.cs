using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace DiyPodcastRss.Web.Models
{
    public class SyndicationFeedResult : ContentResult
    {
        public SyndicationFeedResult(SyndicationFeed feed)
            : base()
        {
            string rssXml = new DiyPodcastRss.Core.SyndicationFeedResult().GenerateRssXml(feed);
            Content = rssXml;
            ContentType = "application/rss+xml";
        }
    }
}