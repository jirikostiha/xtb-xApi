using xAPI.Codes;

namespace xAPI
{
    public static class TradeRecordExtensions
    {
        public static bool IsLongPosition(this ITradeRecord trade) =>
            trade.Cmd.HasValue
            && (trade.Cmd == TRADE_OPERATION_CODE.BUY_CODE || trade.Cmd == TRADE_OPERATION_CODE.BUY_LIMIT_CODE || trade.Cmd == TRADE_OPERATION_CODE.BUY_STOP_CODE);

        public static bool IsShortPosition(this ITradeRecord trade) =>
            trade.Cmd.HasValue
            && (trade.Cmd == TRADE_OPERATION_CODE.SELL_CODE || trade.Cmd == TRADE_OPERATION_CODE.SELL_LIMIT_CODE || trade.Cmd == TRADE_OPERATION_CODE.SELL_STOP_CODE);
    }
}