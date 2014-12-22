using System;
using System.Web;

namespace DIY_PodcastRss.Utils
{
    public static class CookieHelper
    {

        const string AppName = "DiyPodcastRss";

        public static string UserUniqueId { get { return GetCookie("userUniqueId"); } set { SetCookie("userUniqueId", value); } }

        public static void SetCookie(string key, string value, int cookieExpireDate = 30)
        {
            HttpCookie myCookie = new HttpCookie(AppName);
            myCookie[key] = value;
            myCookie.Expires = DateTime.Now.AddDays(cookieExpireDate);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static string GetCookie(string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[AppName];

            if (cookie != null)
            {
                return cookie.Values[key].ToString();
            }
            return "";
        }
    }
}