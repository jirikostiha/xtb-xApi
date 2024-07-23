using System;
using xAPI.Codes;

namespace xAPI;

public interface ITradeRecord : ISymbol, IPosition
{
    double? Close_price { get; }

    DateTimeOffset? CloseTime { get; }

    bool? Closed { get; }

    TRADE_OPERATION_TYPE? TradeOperation { get; }

    string? Comment { get; }

    double? Commission { get; }

    string? CustomComment { get; }

    int? Digits { get; }

    DateTimeOffset? ExpirationTime { get; }

    double? Margin_rate { get; }

    double? Open_price { get; }

    DateTimeOffset? OpenTime { get; }

    double? Profit { get; }

    double? Sl { get; }

    double? Storage { get; }

    double? Tp { get; }

    double? Volume { get; }
}