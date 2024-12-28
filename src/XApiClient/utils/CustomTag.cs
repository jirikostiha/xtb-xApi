using System.Globalization;

namespace Xtb.XApiClient.Utils;

internal abstract class CustomTag
{
    private static int _lastTag;
    private static int _maxTag = 1000000;

    private static readonly object _lock = new();

    /// <summary>
    /// Return next custom tag.
    /// </summary>
    /// <returns>Next custom tag</returns>
    public static string Next()
    {
        lock (_lock)
        {
            _lastTag = ++_lastTag % _maxTag;
            return _lastTag.ToString(CultureInfo.InvariantCulture);
        }
    }
}