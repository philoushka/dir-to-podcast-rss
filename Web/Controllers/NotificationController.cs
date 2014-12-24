using DIY_PodcastRss.Models;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.Utils;
using System.Text.RegularExpressions;
using System.Web.Mvc;
namespace DIY_PodcastRss.Controllers
{
    public class NotificationController : Controller
    {
        [Throttle(Name = "DeleteFeedThrottle", Seconds = 15)]
        [HttpPost]
        public ActionResult Send(string feedToken, string sendTo)
        {
            Logger.LogMsg("Request to send notification feed ", feedToken, sendTo, Networking.UserIpHostName(Request.UserHostAddress));
            bool notificationSuccess = false;
            var notification = new FeedNotification { SendTo = sendTo };
            var feedRepo = new FeedRepo();
            var feed = feedRepo.GetFeed(feedToken);
            if (feed != null)
            {
                notification.UserFeed = feed;
                var sender = new NotificationSender();

                if (notification.IsEmailAddress)
                {
                    notificationSuccess = sender.SendEmailNotification(notification);
                }
                else
                {
                    notification.SendTo = CleanSmsNumber(notification.SendTo);
                    notificationSuccess = sender.SendSmsNotification(notification);
                }
            }
            return Json(notificationSuccess);
        }

        public string CleanSmsNumber(string input)
        {
            string cleanInput = Regex.Replace(input, "[^0-9]", "");
            if (input[0] == '+')
            {
                cleanInput = "+" + cleanInput;
            }
            return cleanInput;
        }
    }
}