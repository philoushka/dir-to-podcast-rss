using DIY_PodcastRss.Extensions;
using DIY_PodcastRss.Models;
using SendGrid;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Twilio;
namespace DIY_PodcastRss.Repositories
{
    public class NotificationSender
    {
        public bool SendSmsNotification(FeedNotification notification)
        {
            if (notification.IsEmailAddress)
            {
                return false;
            }

            string twilioAccountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            string twilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            string twilioFromNumber = ConfigurationManager.AppSettings["TwilioFromNumber"];
            var twilio = new TwilioRestClient(twilioAccountSid, twilioAuthToken);
            try
            {
                twilio.SendMessage(twilioFromNumber, notification.SendTo, BuildSmsNotificationMessage(notification));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public string BuildSmsNotificationMessage(FeedNotification notification)
        {
            string smsTextToDeliver = ConfigurationManager.AppSettings["FeedNotificationText"];
            smsTextToDeliver = smsTextToDeliver.Replace("{newline}", "%0a");
            return smsTextToDeliver.FormatWith(notification.UserFeed.FeedName, notification.UserFeed.FeedUri.AbsoluteUri);

        }

        public string BuildEmailNotificationMessage(FeedNotification notification)
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
            email.Text = BuildEmailNotificationMessage(notification);

            var credentials = BuildSendGridCredential();
            var transportWeb = new Web(credentials);
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