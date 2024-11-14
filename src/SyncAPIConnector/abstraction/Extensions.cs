using System;
using Xtb.XApi.Codes;

namespace Xtb.XApi;

public static class TradeRecordExtensions
{
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.BUY || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_STOP);

    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.SELL || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_STOP);

    /// <summary>
    /// Indicates if market is cfd stock market.
    /// It is based on symbol name as xtb is using it to distinguish between stocks and cfd stocks.
    /// </summary>
    public static bool IsCfdStock(this ISymbol symbol) =>
        symbol?.Symbol is not null && symbol.Symbol.EndsWith("_4", StringComparison.InvariantCulture);
}