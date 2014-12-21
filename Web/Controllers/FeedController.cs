using DIY_PodcastRss.Extensions;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.ViewModels;
using DIYPodcastRss.Core;
using DIYPodcastRss.Core.Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DIY_PodcastRss.Controllers
{
    public class FeedController : Controller
    {
        public ActionResult Create()
        {
            var vm = new UserFeed { Files = new[] { "http://localhost/foo.mp3", "http://localhost/bar.mp3", "http://localhost/baz.mp3" } };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(UserFeed postedUserFeed)
        {

            if (ModelState.IsValid)
            {
                postedUserFeed.Files = Request.Form["Files"].ToString().Split(null).Where(x => x.HasValue()).ToList();
                postedUserFeed.CreatedOnUtc = DateTime.UtcNow;
                postedUserFeed.FeedToken = GuidEncoder.New();
                postedUserFeed.BaseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                postedUserFeed.CreatedFromIpHost = "{0} {1}".FormatWith(Request.UserHostAddress, Request.UserHostName);
                var rssGenerator = new DIYPodcastRss.Core.RssGenerator();
                var syndicationFeed = rssGenerator.CreateRss(postedUserFeed);
                var feedResult = new SyndicationFeedResult();

                postedUserFeed.FeedDocument = feedResult.GenerateRssXml(syndicationFeed);
                var repo = new FeedRepo();
                repo.SaveFeed(postedUserFeed);
                ViewBag.NewFeedToken = postedUserFeed.FeedToken;
                return RedirectToRoute("AllFeeds");
            }
            return View(postedUserFeed);
        }

        public ActionResult All()
        {
            var vm = new AllUserFeedsViewModel();
            var repo = new FeedRepo();
            vm.Feeds = repo.AllFeeds();
            return View(vm);
        }

        public ActionResult UserHistory() { return View(); }
        public ActionResult Delete(string feedToken) { return View(); }


        public string View(string feedToken)
        {
            var repo = new FeedRepo();
            var feed = repo.GetFeed(feedToken);
            if (feed != null)
            {
                Response.ContentType = "text/plain";// "application/rss+xml";
                return feed.FeedDocument.ToString();
            }
            Response.StatusCode = 404;
            return null;
        }

    }
}
