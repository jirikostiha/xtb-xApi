using System;

namespace xAPI
{
    public interface ITradeRecord
    {
        double? Close_price { get; }

        long? Close_time { get; }

        DateTimeOffset? Close_time2 { get; }

        bool? Closed { get; }

        long? Cmd { get; }

        string Comment { get; }

        double? Commission { get; }

        string CustomComment { get; }

        long? Digits { get; }

        long? Expiration { get; }

        DateTimeOffset? Expiration2 { get; }

        double? Margin_rate { get; }

        double? Open_price { get; }

        long? Open_time { get; }

        DateTimeOffset? Open_time2 { get; }

        long? Order { get; }

        long? Order2 { get; }

        long? Position { get; }

        double? Profit { get; }

        double? Sl { get; }

        double? Storage { get; }

        string Symbol { get; }

        double? Tp { get; }

        double? Volume { get; }
    }
}