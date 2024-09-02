using System.Collections.Generic;

namespace xAPI;

/// <summary>
/// Commodity group values.
/// </summary>
public static class CommodityGroup
{
    /// <summary>Precious Metals</summary>
    public const string PreciousMetals = "Precious Metals";

    /// <summary>Agriculture</summary>
    public const string Agriculture = "Agriculture";

    /// <summary>Energy</summary>
    public const string Energy = "Energy";

    /// <summary>Industrial Metals</summary>
    public const string IndustrialMetals = "Industrial Metals";

    /// <summary>Livestock</summary>
    public const string Livestock = "Livestock";

    /// <summary>
    /// Enumerates all commodity group values.
    /// </summary>
    /// <returns>An enumerable of all commodity group strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return PreciousMetals;
        yield return Agriculture;
        yield return Energy;
        yield return IndustrialMetals;
        yield return Livestock;
    }
}