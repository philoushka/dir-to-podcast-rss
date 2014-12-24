using DIYPodcastRss.Core.Extensions;
using System;
using System.Linq;

namespace DIYPodcastRss.Core.Model
{
    public class AudioFile
    {

        public AudioFile()
        {
            UniqueId = GuidEncoder.NewGuid();
        }
        public string RemoteFileName { get; set; }
        public string SortFileName { get { return ConvertFileNameSortable(this.RemoteFileName); } }
        public Uri RemoteUri { get; set; }
        public string UniqueId { get; private set; }
        private string ConvertFileNameSortable(string input)
        {
            string[] fileNameParts = input.Split("., []():-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string leading = fileNameParts.First();
            if (leading.IsNumeric())
            {
                //pad it large for easy sorting. 5 ought to do.

                string formattedFirst = (int.Parse(leading)).ToString("000000");

                return formattedFirst + input.Substring(leading.Length - 1);

            }
            return input.Trim();
        }

        public long SizeBytes { get; set; }
    }
}
