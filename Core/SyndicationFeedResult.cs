using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DiyPodcastRss.Core
{
    public class SyndicationFeedResult
    {
        public string GenerateRssXml(SyndicationFeed feed)
        {
            var formatter = feed.GetRss20Formatter();
            formatter.SerializeExtensionsAsAtom = false;

            using (var memstream = new MemoryStream())
            using (var writer = new XmlTextWriter(memstream, System.Text.UTF8Encoding.UTF8))
            {
                formatter.WriteTo(writer);
                writer.Flush();
                memstream.Position = 0;
                return new StreamReader(memstream).ReadToEnd();
            }
        }
    }
}
