using System;
using xAPI.Codes;

namespace xAPI
{
    public interface ITradeRecord : ISymbol, IPosition
    {
        double? Close_price { get; }

        long? Close_time { get; }

        DateTimeOffset? CloseDateTime { get; }

        bool? Closed { get; }

        long? Cmd { get; }

        TRADE_OPERATION_CODE? Cmd2 { get; }

        string Comment { get; }

        double? Commission { get; }

        string CustomComment { get; }

        long? Digits { get; }

        long? Expiration { get; }
        DateTimeOffset? ExpirationDateTime { get; }

        double? Margin_rate { get; }

        double? Open_price { get; }

        long? Open_time { get; }

        DateTimeOffset? OpenDateTime { get; }

        double? Profit { get; }

        double? Sl { get; }

        double? Storage { get; }

        double? Tp { get; }

        double? Volume { get; }
    }
}