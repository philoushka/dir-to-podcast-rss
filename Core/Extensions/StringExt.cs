
namespace DiyPodcastRss.Core.Extensions {
    public static class StringExt {
        public static bool IsNumeric(this string input) {

            int number;
            return (int.TryParse(input, out number));
        }
    }
}
