using System;
using Xtb.XApi.Codes;

namespace Xtb.XApi;

public interface ITradeRecord : IHasSymbol, IPosition
{
    /// <summary> Close price in symbol currency. </summary>
    double? ClosePrice { get; }

    DateTimeOffset? CloseTime { get; }

    bool? Closed { get; }

    TRADE_OPERATION_TYPE? TradeOperation { get; }

    string? Comment { get; }

    double? Commission { get; }

    string? CustomComment { get; }

    int? Digits { get; }

    DateTimeOffset? ExpirationTime { get; }

    double? MarginRate { get; }

    /// <summary> Open price in symbol currency. </summary>
    double? OpenPrice { get; }

    DateTimeOffset? OpenTime { get; }

    /// <summary> Gross profit in account currency. </summary>
    double? Profit { get; set; }

    /// <summary> Stop loss price in symbol currency. </summary>
    double? Sl { get; }

    /// <summary> Take profit price in symbol currency. </summary>
    double? Tp { get; }

    /// <summary> Fees (swap + rollover) in account currency. </summary>
    double? Storage { get; }

    double? Volume { get; }
}