using DIY_PodcastRss.Extensions;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.Utils;
using DIY_PodcastRss.ViewModels;
using DIYPodcastRss.Core;
using DIYPodcastRss.Core.Model;
using Newtonsoft.Json;
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
                Logger.LogMsg("New user! ", CookieHelper.UserUniqueId, System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }

        }
        public ActionResult Create()
        {
            var vm = new UserFeed { Files = new[] { "http://localhost/foo.mp3", "http://localhost/bar.mp3", "http://localhost/baz.mp3" } };
            return View(vm);
        }

        private string TryMakeDateFromJavaScriptDate()
        {
            try
            {
                string[] browserDateParts = Request.Form["UserBrowserDateTime"].Split(null);
                return string.Join(" ", browserDateParts.Take(5));
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Throttle(Name = "CreateFeedThrottle", Seconds = 5)]
        [HttpPost]
        public ActionResult Create(UserFeed postedUserFeed)
        {
            Logger.LogMsg("Request to create feed ", Environment.NewLine, postedUserFeed.FeedName, postedUserFeed.ImgUrl, string.Join(",", postedUserFeed.Files));

            if (ModelState.IsValid)
            {
                if (postedUserFeed.FeedName.IsNullOrWhiteSpace())
                {
                    postedUserFeed.FeedName = TryMakeDateFromJavaScriptDate() ?? DateTime.UtcNow.FriendlyFormat();
                }
                postedUserFeed.Files = Request.Form["Files"].ToString().Split(null).Where(x => x.HasValue()).ToList();
                postedUserFeed.CreatedOnUtc = DateTime.UtcNow;
                postedUserFeed.BaseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                postedUserFeed.CreatedFromIpHost = "{0} {1}".FormatWith(Request.UserHostAddress, Request.UserHostName);
                postedUserFeed.UserUniqueId = CookieHelper.UserUniqueId;

                Logger.LogMsg("Cleaned up new feed ", Environment.NewLine, JsonConvert.SerializeObject(postedUserFeed, Formatting.Indented));

                var rssGenerator = new DIYPodcastRss.Core.RssGenerator();
                var syndicationFeed = rssGenerator.CreateRss(postedUserFeed);
                var feedResult = new SyndicationFeedResult();

                postedUserFeed.FeedDocument = feedResult.GenerateRssXml(syndicationFeed);
                var repo = new FeedRepo();

                //ensure the feed token is unique
                while (repo.ReserveEmptyFeed(postedUserFeed.FeedToken) == false)
                {
                    postedUserFeed.FeedToken = GuidEncoder.New();
                }

                repo.SaveFeed(postedUserFeed);
                ViewBag.NewFeedToken = postedUserFeed.FeedToken;
            }
            return View(postedUserFeed);
        }

        public ActionResult All()
        {
            Logger.LogMsg("All Feeds", CookieHelper.UserUniqueId);
            var vm = new AllUserFeedsViewModel();
            var repo = new FeedRepo();
            vm.Feeds = repo.AllFeeds().OrderByDescending(x => x.CreatedOnUtc);
            return View(vm);
        }

        public ActionResult MyFeeds()
        {
            Logger.LogMsg("My Feed for User ", CookieHelper.UserUniqueId);
            return RedirectToRoute("UserFeeds", new { userId = CookieHelper.UserUniqueId });
        }

        public ActionResult UserFeeds(string userId)
        {
            Logger.LogMsg("User Feed for User ", CookieHelper.UserUniqueId);
            var repo = new FeedRepo();
            var vm = new UserHistoryViewModel();
            vm.Feeds = repo.AllFeeds().Where(x => x.UserUniqueId == userId && x.DeletedOnUtc.HasValue == false).OrderByDescending(x => x.CreatedOnUtc);
            Logger.LogMsg("Number feeds found:", vm.Feeds.Count());

            return View(vm);
        }

        [Throttle(Name = "DeleteFeedThrottle", Seconds = 5)]
        [HttpPost]
        public ActionResult Delete(string feedToken)
        {
            Logger.LogMsg("Deleting feed ", feedToken, " User Id ", CookieHelper.UserUniqueId, "at", Request.UserHostAddress);
            var repo = new FeedRepo();
            var callingUserId = CookieHelper.UserUniqueId;
            if (repo.UserCanDeleteFeed(feedToken, callingUserId))
            {
                bool repoWasDeleted = repo.DeleteFeed(feedToken);
                return Json(repoWasDeleted);
            }
            else
            {
                Response.StatusCode = 500;
                return null;
            }
        }


        public string ViewFeed(string feedToken)
        {
            Logger.LogMsg("Viewing feed ", feedToken);
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
