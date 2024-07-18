using xAPI.Codes;

namespace xAPI;

public static class TradeRecordExtensions
{
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_CODE.BUY || trade.TradeOperation == TRADE_OPERATION_CODE.BUY_LIMIT || trade.TradeOperation == TRADE_OPERATION_CODE.BUY_STOP);

    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.TradeOperation is not null
        && (trade.TradeOperation == TRADE_OPERATION_CODE.SELL || trade.TradeOperation == TRADE_OPERATION_CODE.SELL_LIMIT || trade.TradeOperation == TRADE_OPERATION_CODE.SELL_STOP);
}