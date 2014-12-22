using DIY_PodcastRss.Extensions;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.Utils;
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
        public FeedController()
        {
            if (CookieHelper.UserUniqueId.IsNullOrWhiteSpace())
            {
                CookieHelper.UserUniqueId = Guid.NewGuid().ToString();
            }

        }
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
                postedUserFeed.BaseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                postedUserFeed.CreatedFromIpHost = "{0} {1}".FormatWith(Request.UserHostAddress, Request.UserHostName);
                postedUserFeed.UserUniqueId = CookieHelper.UserUniqueId;
                var rssGenerator = new DIYPodcastRss.Core.RssGenerator();
                var syndicationFeed = rssGenerator.CreateRss(postedUserFeed);
                var feedResult = new SyndicationFeedResult();

                postedUserFeed.FeedDocument = feedResult.GenerateRssXml(syndicationFeed);
                var repo = new FeedRepo();


                //ensure the feed token is unique
                while (repo.ReserveEmptyFeed(postedUserFeed.FeedToken)==false)
                {
                   postedUserFeed.FeedToken = GuidEncoder.New();
                }

                repo.SaveFeed(postedUserFeed);
                ViewBag.NewFeedToken = postedUserFeed.FeedToken;
                return View(postedUserFeed);
            }
            return View(postedUserFeed);
        }

        public ActionResult All()
        {
            var vm = new AllUserFeedsViewModel();
            var repo = new FeedRepo();
            vm.Feeds = repo.AllFeeds().OrderByDescending(x => x.CreatedOnUtc);
            return View(vm);
        }

        public ActionResult MyFeeds()
        {
            return RedirectToRoute("UserFeeds", new { userId = CookieHelper.UserUniqueId });
        }

        public ActionResult UserFeeds(string userId)
        {
            var repo = new FeedRepo();
            var vm = new UserHistoryViewModel();
            vm.Feeds = repo.AllFeeds().Where(x => x.UserUniqueId == userId && x.DeletedOnUtc.HasValue==false).OrderByDescending(x => x.CreatedOnUtc);
            return View(vm);
        }

        public ActionResult Delete(string feedToken)
        {
            var repo = new FeedRepo();
            bool repoWasDeleted = repo.DeleteFeed(feedToken);
            ViewBag.SuccessfullyDeletedThatFeed = repoWasDeleted;
            return RedirectToRoute("MyFeeds");
        }


        public string ViewFeed(string feedToken)
        {
            var repo = new FeedRepo();
            var feed = repo.GetFeed(feedToken);
            if (feed != null)
            {
                Response.ContentType = "application/rss+xml";
                return feed.FeedDocument.ToString();
            }
            Response.StatusCode = 404;
            return null;
        }

    }
}
