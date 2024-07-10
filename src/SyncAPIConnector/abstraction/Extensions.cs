using xAPI.Codes;

namespace xAPI;

public static class TradeRecordExtensions
{
    public static bool IsLongPosition(this ITradeRecord trade) =>
        trade.Cmd2 is not null
        && (trade.Cmd2 == TRADE_OPERATION_CODE.BUY || trade.Cmd2 == TRADE_OPERATION_CODE.BUY_LIMIT || trade.Cmd2 == TRADE_OPERATION_CODE.BUY_STOP);

    public static bool IsShortPosition(this ITradeRecord trade) =>
        trade.Cmd2 is not null
        && (trade.Cmd2 == TRADE_OPERATION_CODE.SELL || trade.Cmd2 == TRADE_OPERATION_CODE.SELL_LIMIT || trade.Cmd2 == TRADE_OPERATION_CODE.SELL_STOP);
}