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
                name: "UserCreateFeed",
                url: "feed/new",
                defaults: new { controller = "Feed", action = "Create" });

            routes.MapRoute(
           name: "AllFeeds",
           url: "feed/all",
           defaults: new { controller = "Feed", action = "All" });
            routes.MapRoute(
   name: "MyFeeds",
   url: "feeds/mine",
   defaults: new { controller = "Feed", action = "MyFeeds" });

            routes.MapRoute(
   name: "UserFeeds",
   url: "feeds/{userId}",
   defaults: new { controller = "Feed", action = "UserFeeds"    });



            routes.MapRoute(
                name: "ViewFeed",
                url: "{feedToken}",
                defaults: new { controller = "Feed", action = "View" });


            routes.MapRoute(
                name: "DeleteFeed",
                url: "delete/{feedToken}",
                defaults: new { controller = "Feed", action = "Delete" });

            routes.MapRoute(
              name: "Default",
              url: "",
              defaults: new { controller = "Feed", action = "Create" });
        }
    }
}
