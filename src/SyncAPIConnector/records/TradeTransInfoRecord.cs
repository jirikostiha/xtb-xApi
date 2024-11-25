using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Xtb.XApi.Codes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Symbol}, order:{OrderId}, volume:{Volume}")]
public sealed record TradeTransInfoRecord : IHasSymbol
{
    public TradeTransInfoRecord(
        TRADE_OPERATION_TYPE? tradeOperation,
        TRADE_TRANSACTION_TYPE? transactionType,
        double? price,
        double? sl,
        double? tp,
        string symbol,
        double? volume,
        long? orderId,
        string? customComment,
        DateTimeOffset? expiration)
    {
        Price = price;
        Sl = sl;
        Tp = tp;
        Symbol = symbol;
        Volume = volume;
        OrderId = orderId;
        CustomComment = customComment;

        TradeOperation = tradeOperation;
        TransactionType = transactionType;
        Expiration = expiration;
    }

    public string? CustomComment { get; init; }

    public long? OrderId { get; init; }

    public double? Price { get; init; }

    public double? Sl { get; init; }

    public string? Symbol { get; init; }

    public double? Tp { get; init; }

    public double? Volume { get; init; }

    public TRADE_OPERATION_TYPE? TradeOperation { get; init; }

    public TRADE_TRANSACTION_TYPE? TransactionType { get; init; }

    public DateTimeOffset? Expiration { get; init; }

    public JsonObject ToJsonObject()
    {
        JsonObject obj = new()
        {
            { "cmd", TradeOperation?.Code },
            { "type", TransactionType?.Code },
            { "price", Price },
            { "sl", Sl },
            { "tp", Tp },
            { "symbol", Symbol },
            { "volume", Volume },
            { "order", OrderId },
            { "customComment", CustomComment },
            { "expiration", Expiration?.ToUnixTimeMilliseconds() },
        };

        return obj;
    }
}