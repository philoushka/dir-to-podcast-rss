using DiyPodcastRss.Core;
using DiyPodcastRss.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DiyPodcastRss.Web.Tests
{

    /// <summary>
    /// Can generate a valid RSS XML document, given a list of files.
    /// </summary>
    [TestClass]
    public class RssGeneration
    {
        [TestMethod]
        public void CanGenerate()
        {
            var rssGenerator = new DiyPodcastRss.Core.RssGenerator();

            var userFeed = CreateDummyUserFeed();
            var feed = rssGenerator.CreateRss(userFeed);
            Assert.IsNotNull(feed);
            userFeed.RssDoc = new SyndicationFeedResult().GenerateRssXml(feed);
            Assert.IsNotNull(userFeed.RssDoc);
        }


        private UserFeed CreateDummyUserFeed()
        {
            return new UserFeed { BaseUrl = "http://localhost/", ImgUrl = "http://example.com", FeedName = "My Test Feed", FeedToken = GuidEncoder.New(), Files = GetSomeDummyFilesToServe() };
        }

        private IEnumerable<string> GetSomeDummyFilesToServe()
        {
            return new[] { "http://www.devtxt.com/test.mp3" };
        }

    }
}
