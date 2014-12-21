
namespace DIY_PodcastRss.Extensions
{
    public static class Strings
    {
        public static string FormatWith(this string input, params object[] format)
        {
            return string.Format(input, format);
        }

        public static bool HasValue(this string input)
        {
            return (string.IsNullOrWhiteSpace(input) == false);
        }

    }
}