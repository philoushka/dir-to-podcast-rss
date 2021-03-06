﻿using System;

namespace DiyPodcastRss.Core
{

    public static class GuidEncoder
    {

        private const int UniqueIdLength = 10;
        public static string New()
        {
            return Encode(Guid.NewGuid());
        }

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string Encode(Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "");
            enc = enc.Replace("+", "");
            return enc.Substring(0, UniqueIdLength);
        }

    }
}
