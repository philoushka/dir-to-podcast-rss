﻿using DIYPodcastRss.Core.Model;
using System.Collections.Generic;

namespace DIY_PodcastRss.ViewModels
{
    public class UserHistoryViewModel
    {
        public IEnumerable<UserFeed> Feeds { get; set; }

    }
}