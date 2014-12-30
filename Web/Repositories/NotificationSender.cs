using DiyPodcastRss.Web.Extensions;
using DiyPodcastRss.Web.Models;
using SendGrid;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Twilio;
namespace DiyPodcastRss.Web.Repositories
{
    public class NotificationSender
    {
        public bool SendSmsNotification(FeedNotification notification)
        {
            string twilioAccountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            string twilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            string twilioFromNumber = ConfigurationManager.AppSettings["TwilioFromNumber"];
            var twilio = new TwilioRestClient(twilioAccountSid, twilioAuthToken);
            try
            {
                var smsResult = twilio.SendMessage(twilioFromNumber, notification.SendTo, BuildNotificationMessage(notification));
                return (smsResult.RestException == null);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string BuildNotificationMessage(FeedNotification notification)
        {
            string emailTextToDeliver = ConfigurationManager.AppSettings["FeedNotificationText"];
            emailTextToDeliver = emailTextToDeliver.Replace("{newline}", Environment.NewLine);
            return emailTextToDeliver.FormatWith(notification.UserFeed.FeedName, notification.UserFeed.FeedUri.AbsoluteUri);
        }

        public bool SendEmailNotification(FeedNotification notification)
        {
            var email = new SendGridMessage();
            email.AddTo(notification.SendTo);
            email.From = new MailAddress("info@diyrss.com", "DIY RSS");
            email.Subject = "Your recent new custom RSS feed";
            email.Text = BuildNotificationMessage(notification);

            var credentials = BuildSendGridCredential();
            var transportWeb = new SendGrid.Web(credentials);
            transportWeb.Deliver(email);
            return true;
        }

        public NetworkCredential BuildSendGridCredential()
        {
            return new NetworkCredential(
                ConfigurationManager.AppSettings["SendGridUsername"],
               ConfigurationManager.AppSettings["SendGridPassword"]);
        }
    }


}