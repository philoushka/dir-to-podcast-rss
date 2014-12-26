
using System;
using System.Collections.Generic;


namespace DIY_PodcastRss.ViewModels
{
    public class AllLogsViewModel
    {
        public IEnumerable<LogToView> Logs { get; set; }
    }

    public class LogToView {
        public int NumLines { get; set; }
        public string LogDateDisplay { get; set; }
        public string LogDateName { get; set; }
    }
}
