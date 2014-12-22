using DIY_PodcastRss.Models;
using DIY_PodcastRss.Repositories;
using System.Web.Mvc;
namespace DIY_PodcastRss.Controllers
{
    public class NotificationController : Controller
    {
        [HttpPost]
        public bool Send(string feedToken, string sendTo)
        {
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