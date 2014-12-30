using System.Net;

namespace DiyPodcastRss.Core
{
    public class FileSizeChecker
    {
        /// <summary>
        /// Determine the size in bytes of a remote file
        /// </summary>        
        /// <returns>The number of bytes that the remote server indicated.</returns>
        public int GetRemoteFileSizeBytes(string url)
        {
            WebRequest req = HttpWebRequest.Create(url);
            req.Method = "HEAD";
            using (WebResponse resp = req.GetResponse())
            {
                int contentLengthBytes;
                if (int.TryParse(resp.Headers.Get("Content-Length"), out contentLengthBytes))
                {
                    return contentLengthBytes;
                }
                return 0;
            }
        }
    }
}
