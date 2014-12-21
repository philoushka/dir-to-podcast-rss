using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DIYPodcastRss.Core
{
    public class SyndicationFeedResult
    {
        public string GenerateRssXml(SyndicationFeed feed)
        {
            using (var memstream = new MemoryStream())
            using (var writer = new XmlTextWriter(memstream, System.Text.UTF8Encoding.UTF8))
            {
                feed.SaveAsRss20(writer);
                writer.Flush();
                memstream.Position = 0;
                return new StreamReader(memstream).ReadToEnd();
            }
        }
    }
}
