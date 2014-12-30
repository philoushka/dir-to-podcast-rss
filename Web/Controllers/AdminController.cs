using DiyPodcastRss.Web.Extensions;
using DiyPodcastRss.Web.Repositories;
using DiyPodcastRss.Web.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace DiyPodcastRss.Web.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeedStats()
        {
            var vm = new ViewModels.FeedStatsViewModel();

            var feedRepo = new FeedRepo();
            var chartItems = feedRepo
                .AllFeeds()
                .OrderBy(x => x.CreatedOnUtc)
                .GroupBy(x => x.CreatedOnUtc.Date, (key, g) => new { XAxisVal = key.FriendlyFormatDateOnly(), YAxisVal = g.Count() });
            var chartData = new ChartData();
            chartData.ChartDataPoints = chartItems.Select(x => x.YAxisVal);
            chartData.ChartLabels = chartItems.Select(x => x.XAxisVal);

            vm.ChartTitle = "RSS Feed Creation";
            vm.ChartItems = chartData;
            return View(vm);
        }

    }
}