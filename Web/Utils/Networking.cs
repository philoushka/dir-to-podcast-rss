using System.Net;

namespace DIY_PodcastRss.Utils
{
    public static class Networking
    {
        public static string UserIpHostName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            return IP + " " + GetIPHost.HostName;
        }
    }
}