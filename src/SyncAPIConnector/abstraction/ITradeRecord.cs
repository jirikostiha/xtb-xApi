using System;
using Xtb.XApi.Codes;

namespace Xtb.XApi;

/// <summary>
/// Represents a trade record containing information about the trade's status, prices, volume, and additional details.
/// Inherits from <see cref="IHasSymbol"/> and <see cref="IPosition"/>.
/// </summary>
public interface ITradeRecord : IHasSymbol, IPosition
{
    /// <summary>
    /// Close price in symbol currency.
    /// </summary>
    double? ClosePrice { get; }

    /// <summary>
    /// Close time. Null if the order is not closed.
    /// </summary>
    DateTimeOffset? CloseTime { get; }

    /// <summary>
    /// Indicates if the trade is closed.
    /// </summary>
    bool? Closed { get; }

    /// <summary>
    /// Operation type of the trade.
    /// </summary>
    TRADE_OPERATION_TYPE? TradeOperation { get; }

    /// <summary>
    /// Comment associated with the trade.
    /// </summary>
    string? Comment { get; }

    /// <summary>
    /// Commission in account currency, null if not applicable.
    /// </summary>
    double? Commission { get; }

    /// <summary>
    /// Custom comment for the trade that can be retrieved later.
    /// </summary>
    string? CustomComment { get; }

    /// <summary>
    /// Number of decimal places for the symbol.
    /// </summary>
    int? Digits { get; }

    /// <summary>
    /// Expiration time of the order. Null if the order is not closed.
    /// </summary>
    DateTimeOffset? ExpirationTime { get; }

    /// <summary>
    /// Margin rate applied to the trade.
    /// </summary>
    double? MarginRate { get; }

    /// <summary>
    /// Open price in symbol currency.
    /// </summary>
    double? OpenPrice { get; }

    /// <summary>
    /// Open time of the trade.
    /// </summary>
    DateTimeOffset? OpenTime { get; }

    /// <summary>
    /// Gross profit in account currency. Null unless the trade is closed.
    /// </summary>
    double? Profit { get; set; }

    /// <summary>
    /// Stop loss price in symbol currency. Zero if not set.
    /// </summary>
    double? Sl { get; }

    /// <summary>
    /// Take profit price in symbol currency. Zero if not set.
    /// </summary>
    double? Tp { get; }

    /// <summary>
    /// Fees (swap + rollover) in account currency.
    /// </summary>
    double? Storage { get; }

    /// <summary>
    /// Volume of the trade in lots.
    /// </summary>
    double? Volume { get; }
}
