using DIY_PodcastRss.Extensions;
using System;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace DIY_PodcastRss.Utils
{
    /// <summary>
    /// Decorates any MVC route that needs to have client requests limited by time.
    /// </summary>
    /// <remarks>
    /// Uses the current System.Web.Caching.Cache to store each client request to the decorated route.
    /// http://stackoverflow.com/questions/33969/
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ThrottleAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// A unique name for this Throttle.
        /// </summary>
        /// <remarks>
        /// We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1"
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// The number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// A text message that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this. Seconds in the message, e.g. "Wait {n} seconds before trying again".
        /// </summary>
        public string Message { get; set; }

        private const string DefaultMessage = "You may only perform this action every {n} seconds.";
        public override void OnActionExecuting(ActionExecutingContext c)
        {
            var key = string.Concat(Name, "-", c.HttpContext.Request.UserHostAddress);
            var allowExecute = false;

            if (HttpRuntime.Cache[key] == null)
            {
                HttpRuntime.Cache.Add(
                    key: key,
                    value: true,
                    dependencies: null,
                    absoluteExpiration: DateTime.Now.AddSeconds(Seconds),
                    slidingExpiration: Cache.NoSlidingExpiration,
                    priority: CacheItemPriority.Low,
                    onRemoveCallback: null);

                allowExecute = true;
            }

            if (!allowExecute)
            {
                if (this.Message.IsNullOrWhiteSpace())
                {
                    Message = DefaultMessage;
                }

                c.Result = new ContentResult { Content = Message.Replace("{n}", this.Seconds.ToString()) };
                Logger.LogMsg("Throttled user when trying to ", key, CookieHelper.UserUniqueId);

                // see 409 - http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
                c.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
        }
    }
}