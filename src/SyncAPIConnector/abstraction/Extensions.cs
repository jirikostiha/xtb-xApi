using System;
using Xtb.XApi.Codes;

namespace Xtb.XApi;

public static class TradeRecordExtensions
{
    /// <summary> Middle price of ask and bid in base currency. </summary>
    public static double? Mid(this ITick tick) => (tick.Bid - tick.Ask) / 2d;

    /// <summary> Size of the candle. </summary>
    public static double? Size(this ICandle candle) => candle.High - candle.Low;

    /// <summary> Size of the candle as coefficient with open price as base. </summary>
    public static double? SizeCoef(this ICandle candle) => (candle.High - candle.Low) / candle.Open;

    /// <summary> Average price of the candle. </summary>
    public static double? Average(this ICandle candle) => (candle.High + candle.Low) / 2d;

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

    /// <summary>
    /// Net profit in account currency. It is gross profit minus fees for storage (swap + rollover)
    /// </summary>
    public static double? NetProfit(this ITradeRecord trade) =>
        trade.Profit + trade.Storage;
}