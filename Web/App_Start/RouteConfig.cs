using System.Web.Mvc;
using System.Web.Routing;

namespace DIY_PodcastRss
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                   name: "About",
                   url: "about",
                   defaults: new { controller = "Home", action = "About" });

            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact" });
            routes.MapRoute(
              name: "SendNotification",
              url: "sendNotification",
              defaults: new { controller = "Notification", action = "Send" });

            routes.MapRoute(
                name: "UserCreateFeed",
                url: "feed/new",
                defaults: new { controller = "Feed", action = "Create" });

            routes.MapRoute(
                name: "AllFeeds",
                url: "feed/all",
                defaults: new { controller = "Feed", action = "All" });

            routes.MapRoute(
               name: "MyFeeds",
               url: "feed/mine",
               defaults: new { controller = "Feed", action = "MyFeeds" });

            routes.MapRoute(
               name: "UserFeeds",
               url: "feed/{userId}",
               defaults: new { controller = "Feed", action = "UserFeeds" });

            routes.MapRoute(
                name: "ViewFeed",
                url: "{feedToken}",
                defaults: new { controller = "Feed", action = "ViewFeed" });

            routes.MapRoute(
                name: "DeleteFeed",
                url: "feed/delete/{feedToken}",
                defaults: new { controller = "Feed", action = "Delete" });

            routes.MapRoute(
              name: "Default",
              url: "",
              defaults: new { controller = "Feed", action = "Create" });



        }
    }
}
