using DIYPodcastRss.Core.Model;
using System;
using System.Linq;
using System.ServiceModel.Syndication;

namespace DIYPodcastRss.Core
{
    public class RssGenerator
    {
        public SyndicationFeed CreateRss(UserFeed userFeed)
        {
            var fileSizeChecker = new FileSizeChecker();
            var sortedAudioFileNames = userFeed.Files
                .Select(x => new AudioFile
                {
                    RemoteFileName = System.IO.Path.GetFileName((new Uri(x)).LocalPath),
                    RemoteUri = new Uri(x),
                    SizeBytes = fileSizeChecker.GetRemoteFileSizeBytes(x)
                });

            var feedBuilder = new FeedBuilder();
            return feedBuilder.BuildRssForUser(userFeed, sortedAudioFileNames);
        }
    }
}
