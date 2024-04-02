namespace xAPI.Codes
{
    public static class TRADE_OPERATION_CODEExtensions
    {
        public static string? ToFriendlyString(this TRADE_OPERATION_CODE tradeOperationCode) =>
            tradeOperationCode.Code switch
            {
                0L => "buy",
                1L => "sell",
                2L => "buy limit",
                3L => "sell limit",
                4L => "buy stop",
                5L => "sell stop",
                6L => "balance",
                _ => null,
            };
    }

    public static class SideExtensions
    {
        public static string? ToFriendlyString(this Side side) =>
            side.Code switch
            {
                0L => "buy",
                1L => "sell",
                _ => null,
            };
    }

    public static class REQUEST_STATUSExtensions
    {
        public static string? ToFriendlyString(this REQUEST_STATUS requestStatus) =>
            requestStatus.Code switch
            {
                0L => "error",
                1L => "pending",
                3L => "accepted",
                4L => "rejected",
                _ => null,
            };
    }

    public static class TRADE_TRANSACTION_TYPEExtensions
    {
        public static string? ToFriendlyString(this TRADE_TRANSACTION_TYPE tradeTransactionType) =>
            tradeTransactionType.Code switch
            {
                0L => "open",
                2L => "close",
                3L => "modify",
                4L => "delete",
                _ => null,
            };
    }

    public static class STREAMING_TRADE_TYPEExtensions
    {
        public static string? ToFriendlyString(this STREAMING_TRADE_TYPE streamingTradeType) =>
            streamingTradeType.Code switch
            {
                0L => "open",
                1L => "pending",
                2L => "close",
                _ => null,
            };
    }

    public static class PERIOD_CODEExtensions
    {
        public static string? ToFriendlyString(this PERIOD_CODE periodCode) =>
            periodCode.Code switch
            {
                1L => "M1",
                5L => "M5",
                15L => "M15",
                30L => "M30",
                60L => "H1",
                240L => "H4",
                1440L => "D1",
                10080L => "W1",
                43200L => "MN1",
                _ => null,
            };
    }

    public static class MARGIN_MODEExtensions
    {
        public static string? ToFriendlyString(this MARGIN_MODE marginMode) =>
            marginMode.Code switch
            {
                101L => "forex",
                102L => "cfd leveraged",
                103L => "cfd",
                _ => null,
            };
    }

    public static class EXECUTION_CODEExtensions
    {
        public static string? ToFriendlyString(this EXECUTION_CODE executionCode) =>
            executionCode.Code switch
            {
                0L => "request",
                1L => "instant",
                2L => "market",
                _ => null,
            };
    }

    public static class PROFIT_MODEExtensions
    {
        public static string? ToFriendlyString(this PROFIT_MODE profitMode) =>
            profitMode.Code switch
            {
                5L => "forex",
                6L => "cfd",
                _ => null,
            };
    }

    public static class SWAP_ROLLOVER_TYPEExtensions
    {
        public static string? ToFriendlyString(this SWAP_ROLLOVER_TYPE swapRolloverType) =>
            swapRolloverType.Code switch
            {
                0L => "monday",
                1L => "tuesday",
                2L => "wednesday",
                3L => "thursday",
                4L => "friday",
                _ => null,
            };
    }

    public static class SWAP_TYPEExtensions
    {
        public static string? ToFriendlyString(this SWAP_TYPE swapType) =>
            swapType.Code switch
            {
                0L => "points",
                1L => "dollars",
                2L => "interest",
                3L => "margin currency",
                _ => null,
            };
    }
}