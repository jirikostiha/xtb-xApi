using System.Collections.Generic;

namespace xAPI;

public static class Region
{
    /// <summary>Asia-Pacific</summary>
    public const string AsiaPacific = "Asia-Pacific";

    /// <summary>Europe</summary>
    public const string Europe = "Europe";

    /// <summary>Americas</summary>
    public const string Americas = "Americas";

    /// <summary>
    /// Enumerates all region group values.
    /// </summary>
    /// <returns>An enumerable of all region group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return AsiaPacific;
        yield return Europe;
        yield return Americas;
    }
}