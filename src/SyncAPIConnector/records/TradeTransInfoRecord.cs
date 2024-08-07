using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, order:{Order}, volume:{Volume}")]
public record TradeTransInfoRecord : ISymbol
{
    public TradeTransInfoRecord(
        TRADE_OPERATION_CODE tradeOperation,
        TRADE_TRANSACTION_TYPE transactionType,
        double? price,
        double? sl,
        double? tp,
        string symbol,
        double? volume,
        long? order,
        string customComment,
        long? expiration)
    {
        Price = price;
        Sl = sl;
        Tp = tp;
        Symbol = symbol;
        Volume = volume;
        Order = order;
        CustomComment = customComment;

        TradeOperation = tradeOperation;
        TransactionType = transactionType;
        Expiration = expiration is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(expiration.Value);
    }

    public string CustomComment { get; init; }

    public long? Order { get; init; }

    public double? Price { get; init; }

    public double? Sl { get; init; }

    public string Symbol { get; init; }

    public double? Tp { get; init; }

    public double? Volume { get; init; }

    public TRADE_OPERATION_CODE TradeOperation { get; init; }

    public TRADE_TRANSACTION_TYPE TransactionType { get; init; }

    public DateTimeOffset? Expiration { get; init; }

    public virtual JsonObject ToJsonObject()
    {
        JsonObject obj = new()
        {
            { "cmd", TradeOperation.Code },
            { "type", TransactionType.Code },
            { "price", Price },
            { "sl", Sl },
            { "tp", Tp },
            { "symbol", Symbol },
            { "volume", Volume },
            { "order", Order },
            { "customComment", CustomComment },
            { "expiration", Expiration?.ToUnixTimeMilliseconds() ?? null },
        };

        return obj;
    }
}