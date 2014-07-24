using DIYPodcastRss.Core.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace DIYPodcastRss.Core {
    public class FeedBuilder {
        public SyndicationFeed BuildRssForUser(UserFeed feed, IEnumerable<AudioFile> files) {

            SyndicationFeed synFeed = new SyndicationFeed(feed.Name, feed.Name, feed.FeedUri);
            synFeed.Categories.Add(new SyndicationCategory(string.Empty));
            //feed.Copyright = new TextSyndicationContent(string.Empty);
            //feed.Language = "en-us";
            //feed.Copyright = new TextSyndicationContent(DateTime.Now.Year + " " + ownerName);
            synFeed.ImageUrl = new Uri("http://i.imgur.com/xRMwkvZ.png");
            synFeed.LastUpdatedTime = DateTime.Now;
            //feed.Authors.Add(new SyndicationPerson() { Name = ownerName, Email = ownerEmail });

            var feedItems = new List<SyndicationItem>();
            foreach (var file in files) {
                var synItem = new SyndicationItem(file.RemoteFileName, null, null);
                synItem.Summary = new TextSyndicationContent(file.RemoteFileName);
                synItem.Id = file.UniqueId;

                synItem.Links.Add(new SyndicationLink() { Title = file.RemoteFileName, Uri = file.RemoteUri, Length = file.SizeBytes, MediaType = "audio/mpeg" });
                feedItems.Add(synItem);
            }
            synFeed.Items = feedItems;

            return synFeed;

        }
    }
}
