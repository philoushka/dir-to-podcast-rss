using DIYPodcastRss.Core.Model;
using System;
using System.Linq;
using System.ServiceModel.Syndication;

namespace DIYPodcastRss.Core {
    public class RssGenerator {
        public SyndicationFeed CreateRss(UserFeed userFeed, string[] audioFileNames) {

            var sortedAudioFileNames = audioFileNames.Select(x => new AudioFile { RemoteFileName = x, RemoteUri = new Uri(userFeed.BaseUrl + x), SizeBytes = 100000 })
                                                      .OrderBy(x => x.SortFileName);

            FeedBuilder feedBuilder = new FeedBuilder();
            return feedBuilder.BuildRssForUser(userFeed, sortedAudioFileNames);
        }
    }
}
