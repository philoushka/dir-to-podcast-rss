using System.ServiceModel.Syndication;
using System.Web.Mvc;

namespace DIY_PodcastRss.Models
{
    public class SyndicationFeedResult : ContentResult
    {
        public SyndicationFeedResult(SyndicationFeed feed)
            : base()
        {
            string rssXml = new DIYPodcastRss.Core.SyndicationFeedResult().GenerateRssXml(feed);
            Content = rssXml;
            ContentType = "application/rss+xml";
        }
    }
}