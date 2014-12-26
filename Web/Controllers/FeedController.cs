using DIY_PodcastRss.Extensions;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.Utils;
using DIY_PodcastRss.ViewModels;
using DIYPodcastRss.Core;
using DIYPodcastRss.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var vm = new UserFeed();
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

        public IEnumerable<string> PullAudioFilesFromTextarea()
        {
            foreach (string file in Request.Form["Files"].ToString().Split(null).Where(x => x.HasValue()))
            {
                string cleanedFile = file.Trim();
                if (cleanedFile.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    cleanedFile = "http://" + cleanedFile;
                }
                yield return cleanedFile;
            }
        }

        [Throttle(Name = "CreateFeedThrottle", Seconds = 5)]
        [HttpPost]
        public ActionResult Create(UserFeed postedUserFeed)
        {
            Logger.LogMsg("Request to create feed.");

            if (ModelState.IsValid)
            {
                if (postedUserFeed.FeedName.IsNullOrWhiteSpace())
                {
                    postedUserFeed.FeedName = TryMakeDateFromJavaScriptDate() ?? DateTime.UtcNow.FriendlyFormat();
                }

                if (postedUserFeed.ImgUrl.IsNullOrWhiteSpace())
                {
                    postedUserFeed.ImgUrl = ConfigurationManager.AppSettings["DefaultFeedArtwork"].ToString();
                }
                postedUserFeed.FeedDesc = ConfigurationManager.AppSettings["DefaultFeedDescription"].ToString();
                postedUserFeed.Files = PullAudioFilesFromTextarea();
                postedUserFeed.CreatedOnUtc = DateTime.UtcNow;
                postedUserFeed.Generator = ConfigurationManager.AppSettings["FeedGenerator"].ToString();
                postedUserFeed.BaseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                postedUserFeed.CreatedFromIpHost = Networking.UserIpHostName(Request.UserHostAddress);
                postedUserFeed.UserUniqueId = CookieHelper.UserUniqueId;
                //ensure the feed token is unique
                var repo = new FeedRepo();
                while (repo.ReserveEmptyFeed(postedUserFeed.FeedToken) == false)
                {
                    postedUserFeed.FeedToken = GuidEncoder.New();
                }
                Logger.LogMsg("Cleaned up new feed ", Environment.NewLine, JsonConvert.SerializeObject(postedUserFeed, Formatting.Indented));

                var rssGenerator = new DIYPodcastRss.Core.RssGenerator();
                var syndicationFeed = rssGenerator.CreateRss(postedUserFeed);
                var feedResult = new SyndicationFeedResult();

                postedUserFeed.FeedDocument = feedResult.GenerateRssXml(syndicationFeed);

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
            Logger.LogMsg("My Feed for User.", CookieHelper.UserUniqueId);
            return RedirectToRoute("UserFeeds", new { userId = CookieHelper.UserUniqueId });
        }

        public ActionResult UserFeeds(string userId)
        {
            Logger.LogMsg("User Feeds for User", CookieHelper.UserUniqueId);
            var repo = new FeedRepo();
            var vm = new UserHistoryViewModel();

            vm.Feeds =
                repo.AllFeeds()
                .Where(x => x.UserUniqueId == userId && x.DeletedOnUtc.HasValue == false)
                .OrderByDescending(x => x.CreatedOnUtc);
            Logger.LogMsg("Number feeds found:", vm.Feeds.Count());

            return View(vm);
        }

        [Throttle(Name = "DeleteFeedThrottle", Seconds = 5)]
        [HttpPost]
        public ActionResult Delete(string feedToken)
        {
            Logger.LogMsg("Deleting feed ", feedToken, ". User Id ", CookieHelper.UserUniqueId, "request from", Networking.UserIpHostName(Request.UserHostAddress));
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
