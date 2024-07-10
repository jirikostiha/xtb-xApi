using System.Globalization;

namespace xAPI.Utils
{
    internal abstract class CustomTag
    {
        private static int lastTag;
        private static int maxTag = 1000000;

        private static readonly object _lock = new();

        /// <summary>
        /// Return next custom tag.
        /// </summary>
        /// <returns>Next custom tag</returns>
        public static string Next()
        {
            lock (_lock)
            {
                lastTag = ++lastTag % maxTag;
                return lastTag.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}