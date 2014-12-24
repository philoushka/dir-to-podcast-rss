using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIYPodcastRss.Core.Model;
using DIYPodcastRss.Core;
using System.Collections.Generic;

namespace DIY_PodcastRss.Tests
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
            var rssGenerator = new DIYPodcastRss.Core.RssGenerator();

            var userFeed = CreateDummyUserFeed();
            var feed = rssGenerator.CreateRss(userFeed );
            Assert.IsNotNull(feed);
            userFeed.RssDoc = new SyndicationFeedResult().GenerateRssXml(feed);
            Assert.IsNotNull(userFeed.RssDoc);
        }


        private UserFeed CreateDummyUserFeed()
        {
            return new UserFeed { BaseUrl = "http://localhost/", ImgUrl="http://example.com", FeedName = "Phil's Test Feed", FeedToken = GuidEncoder.New() , Files=GetSomeDummyFilesToServe()};
        }

        private IEnumerable<string> GetSomeDummyFilesToServe()
        {
            return new[] { "http://www.devtxt.com/test.mp3" };
            return new[] { "http://localhost/foo.mp3", "http://localhost/bar.mp3", "http://localhost/01.Chapt1.mp3", "http://localhost/02.Chapt2.mp3", "http://localhost/1-Chapter1.wma", "http://localhost/2 - Chapter 2.ogg", "http://localhost/3 Chapter 3.aac" };
        }

    }
}
