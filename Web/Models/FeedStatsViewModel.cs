using System.Collections.Generic;

namespace DiyPodcastRss.Web.ViewModels
{
    public class ChartData
    {
        //public ChartData()
        //{
        //    ChartData = new List<int>();
        //    ChartLabels = new List<string>();
        //}
        public IEnumerable<int> ChartDataPoints { get; set; }
        public IEnumerable<string> ChartLabels { get; set; }
    }
    public class FeedStatsViewModel
    {
        public string ChartTitle { get; set; }
        public ChartData ChartItems { get; set; }

    }
}
