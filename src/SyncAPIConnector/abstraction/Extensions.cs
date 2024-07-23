using xAPI.Codes;

namespace xAPI;

public static class TradeRecordExtensions
{
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.BUY || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.BUY_STOP);

    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_TYPE.SELL || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_LIMIT || trade.TradeOperation == TRADE_OPERATION_TYPE.SELL_STOP);
}