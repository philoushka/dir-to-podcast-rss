using DIYPodcastRss.Core.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace DIYPodcastRss.Core
{
    public class FeedBuilder
    {
        public SyndicationFeed BuildRssForUser(UserFeed feed, IEnumerable<AudioFile> files)
        {
            XNamespace atom = "http://www.w3.org/2005/Atom";

            var synFeed = new SyndicationFeed(feed.FeedName, feed.FeedDesc, feed.FeedUri);
            synFeed.Categories.Add(new SyndicationCategory());
            var link = new SyndicationLink { RelationshipType = "self", Uri = feed.FeedUri };
            synFeed.Links.Add(link);
            synFeed.ImageUrl = new Uri(feed.ImgUrl);
            synFeed.Generator = feed.Generator;
            synFeed.LastUpdatedTime = DateTime.Now;
            synFeed.Items = BuildSyndicationItems(files);
            synFeed.ElementExtensions.Add(new XElement(atom + "link", new XAttribute("href", feed.FeedUri.AbsoluteUri), new XAttribute("rel", "self"), new XAttribute("type", "application/rss+xml")));
            synFeed.AttributeExtensions.Add(new XmlQualifiedName("atom", XNamespace.Xmlns.NamespaceName), atom.NamespaceName);
            return synFeed;
        }

        public IEnumerable<SyndicationItem> BuildSyndicationItems(IEnumerable<AudioFile> audioFiles)
        {
            foreach (AudioFile file in audioFiles)
            {
                var synItem = new SyndicationItem(file.RemoteFileName, null, null);
                synItem.Summary = new TextSyndicationContent(file.RemoteFileName);
                synItem.Id = file.UniqueId;
                synItem.PublishDate = DateTime.UtcNow;
                synItem.Links.Add(new SyndicationLink() { RelationshipType = "enclosure", Title = file.RemoteFileName, Uri = file.RemoteUri, Length = file.SizeBytes, MediaType = "audio/mpeg" });
                yield return synItem;
            }
        }

    }
}
