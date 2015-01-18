using DiyPodcastRss.Web.Extensions;
using DiyPodcastRss.Web.Utils;
using DiyPodcastRss.Web.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace DiyPodcastRss.Web.Controllers
{
    public class LogController : Controller
    {
        public ActionResult ViewLog(string date)
        {
            Logger.LogMsg("Viewing log", date, ". User", CookieHelper.UserUniqueId, "from", Networking.UserIpHostName(Request.UserHostAddress), Request.UserAgent);
            var vm = new LogViewModel { LogDate = date };
            if (vm.IsValidLogDate(expectedFormat: Logger.LogNameDateFormat))
            {
                vm.LogLines = Logger.ReadAllForDay(date);
                return View(vm);
            }
            Response.StatusCode = 404;
            return null;
        }

        public ActionResult Index()
        {
            Logger.LogMsg("Viewing log index.", "User", CookieHelper.UserUniqueId, "from", Networking.UserIpHostName(Request.UserHostAddress), Request.UserAgent);

            var vm = new AllLogsViewModel();
            vm.Logs = Logger
                         .ListLogs()
                         .Select(x => new LogToView
                         {
                             LogDateDisplay = DateTime.ParseExact(x.Key, Logger.LogNameDateFormat, new CultureInfo("en-us")).FriendlyFormatDateOnly(),
                             LogDateName = x.Key,
                             NumLines = x.Value
                         })
                         .OrderByDescending(x => x.LogDateName);

            return View(vm);
        }
    }
}