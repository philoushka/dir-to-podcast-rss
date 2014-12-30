using DIY_PodcastRss.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DIY_PodcastRss.ViewModels
{
    public class LogViewModel
    {

        public string LogDate { get; set; }
        public DateTime? ValidatedLogDate { get; set; }
        public bool IsValidLogDate(string expectedFormat)
        {
            DateTime dt;
            LogDate = LogDate.Trim();
            bool isValid = DateTime.TryParseExact(LogDate, expectedFormat, new CultureInfo("en-us"), DateTimeStyles.NoCurrentDateDefault, out dt);
            if (isValid)
            {
                ValidatedLogDate = dt;
            }
            return isValid;
        }
        public IEnumerable<string> LogLines { get; set; }

        public string FriendlyDate
        {
            get
            {
                return ValidatedLogDate.FriendlyFormat();
            }
        }
    }

}
