using DIY_PodcastRss.Models;
using DIY_PodcastRss.Repositories;
using DIY_PodcastRss.Utils;
using System.Web.Mvc;
namespace DIY_PodcastRss.Controllers
{
    public class NotificationController : Controller
    {
        [Throttle(Name = "DeleteFeedThrottle", Seconds = 15)]
        [HttpPost]
        public bool Send(string feedToken, string sendTo)
        {
            Logger.LogMsg("Request to send notification feed ", feedToken, sendTo, Request.UserHostName, Request.UserHostAddress);

            var notification = new FeedNotification { SendTo = sendTo };
            var feedRepo = new FeedRepo();
            var feed = feedRepo.GetFeed(feedToken);
            if (feed != null)
            {
                notification.UserFeed = feed;
                var sender = new NotificationSender();

                if (notification.IsEmailAddress)
                {
                    return sender.SendEmailNotification(notification);
                }
                else
                {
                    return sender.SendSmsNotification(notification);
                }
            }
            return true;
        }

    }


}