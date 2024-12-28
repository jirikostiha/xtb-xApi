using System.Collections.Generic;

namespace Xtb.XApiClient.Model;

/// <summary>
/// Names of streaming commands.
/// </summary>
public static class StreamingCommandName
{
    public const string KeepAlive = "keepAlive";
    public const string Balance = "balance";
    public const string Profit = "profit";
    public const string TickPrices = "tickPrices";
    public const string Candle = "candle";
    public const string TradeStatus = "tradeStatus";
    public const string Trade = "trade";
    public const string News = "news";

    /// <summary>
    /// Enumerates all streaming command name values.
    /// </summary>
    /// <returns>An enumerable of all streaming command name strings.</returns>
    public static IEnumerable<string> Enumerate()
    {
        yield return KeepAlive;
        yield return Balance;
        yield return Profit;
        yield return TickPrices;
        yield return Candle;
        yield return TradeStatus;
        yield return Trade;
        yield return News;
    }
}