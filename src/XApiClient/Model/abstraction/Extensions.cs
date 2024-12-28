using System;

namespace Xtb.XApiClient.Model;

public static class TradeRecordExtensions
{
    /// <summary>
    /// Determines whether the specified trade is a long position.
    /// </summary>
    /// <param name="trade">The trade record.</param>
    /// <returns>
    /// <c>true</c> if the trade is a long position (BUY, BUY_LIMIT, or BUY_STOP); otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.BUY || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_STOP);

    /// <summary>
    /// Determines whether the specified trade is a short position.
    /// </summary>
    /// <param name="trade">The trade record.</param>
    /// <returns>
    /// <c>true</c> if the trade is a short position (SELL, SELL_LIMIT, or SELL_STOP); otherwise, <c>false</c>.
    /// </returns>
    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.SELL || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_STOP);

    /// <summary>
    /// Indicates if market is cfd stock market.
    /// It is based on symbol name as xtb is using it to distinguish between stocks and cfd stocks.
    /// </summary>
    public static bool IsCfdStock(this IHasSymbol symbol) =>
        symbol?.Symbol is not null && symbol.Symbol.EndsWith("_4", StringComparison.InvariantCulture);
}