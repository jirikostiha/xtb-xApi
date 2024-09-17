using System.Collections.Generic;

namespace XApi;

public static class CategoryName
{
    /// <summary>
    /// Forex category.
    /// </summary>
    public const string Forex = "FX";

    /// <summary>
    /// Commodity category.
    /// </summary>
    public const string Commodity = "CMD";

    /// <summary>
    /// ETF (Exchange-Traded Fund) category.
    /// </summary>
    public const string Etf = "ETF";

    /// <summary>
    /// Stocks category.
    /// </summary>
    public const string Stocks = "STC";

    /// <summary>
    /// Indices category.
    /// </summary>
    public const string Indices = "IND";

    /// <summary>
    /// Cryptocurrency category.
    /// </summary>
    public const string Crypto = "CRT";

    /// <summary>
    /// Enumerates all category values.
    /// </summary>
    /// <returns>An enumerable of all category strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return Forex;
        yield return Commodity;
        yield return Etf;
        yield return Stocks;
        yield return Indices;
        yield return Crypto;
    }
}
