using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIYPodcastRss.Core.Model;
using DIYPodcastRss.Core;

namespace DIY_PodcastRss.Tests {
    [TestClass]
    public class RssGeneration {
        [TestMethod]
        public void CanGenerate() {
            var rssGenerator = new DIYPodcastRss.Core.RssGenerator();

            string[] audioFileNames = new[] { "foo.mp3", "bar.mp3", "01.Chapt1.mp3", "02.Chapt2.mp3", "1-Chapter1.wma", "2 - Chapter 2.ogg", "3 Chapter 3.aac" };
            var userFeed = new UserFeed { Name = "My Test Feed", UniqueId = GuidEncoder.New() };
            var feed = rssGenerator.CreateRss(userFeed, audioFileNames);
            Assert.IsNotNull(feed);
            userFeed.RssDoc = new SyndicationFeedResult().GenerateRssXml(feed);
            Assert.IsNotNull(userFeed.RssDoc);
        }
    }
}
