using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}")]
public record StreamingTradeRecord : IBaseResponseRecord, ITradeRecord
{
    public double? ClosePrice { get; set; }

    public bool? Closed { get; set; }

    public string? Comment { get; set; }

    public double? Commission { get; set; }

    public string? CustomComment { get; set; }

    public double? MarginRate { get; set; }

    public double? OpenPrice { get; set; }

    public long? Order { get; set; }

    public long? Order2 { get; set; }

    public long? Position { get; set; }

    public double? Profit { get; set; }

    public double? Sl { get; set; }

    public string? State { get; set; }

    public double? Storage { get; set; }

    public string? Symbol { get; set; }

    public double? Tp { get; set; }

    public double? Volume { get; set; }

    public int? Digits { get; set; }

    public TRADE_OPERATION_TYPE? TradeOperation { get; set; }

    public STREAMING_TRADE_TYPE? StreamingTradeType { get; set; }

    public DateTimeOffset? OpenTime { get; set; }

    public DateTimeOffset? CloseTime { get; set; }

    public DateTimeOffset? ExpirationTime { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        ClosePrice = (double?)value["close_price"];
        Closed = (bool?)value["closed"];
        Comment = (string?)value["comment"];
        Commission = (double?)value["commision"];
        CustomComment = (string?)value["customComment"];
        MarginRate = (double?)value["margin_rate"];
        OpenPrice = (double?)value["open_price"];
        Order = (long?)value["order"];
        Order2 = (long?)value["order2"];
        Position = (long?)value["position"];
        Profit = (double?)value["profit"];
        Sl = (double?)value["sl"];
        State = (string?)value["state"];
        Storage = (double?)value["storage"];
        Symbol = (string?)value["symbol"];
        Tp = (double?)value["tp"];
        Volume = (double?)value["volume"];
        Digits = (int?)value["digits"];

        var streamingTradeCode = (int?)value["type"];
        StreamingTradeType = streamingTradeCode.HasValue ? new STREAMING_TRADE_TYPE(streamingTradeCode.Value) : null;

        var tradeOperationCode = (int?)value["cmd"];
        TradeOperation = tradeOperationCode.HasValue ? new TRADE_OPERATION_TYPE(tradeOperationCode.Value) : null;

        var openTime = (long?)value["open_time"];
        OpenTime = openTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(openTime.Value) : null;
        Debug.Assert(OpenTime?.ToUnixTimeMilliseconds() == openTime);

        var closeTime = (long?)value["close_time"];
        CloseTime = closeTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(closeTime.Value) : null;
        Debug.Assert(CloseTime?.ToUnixTimeMilliseconds() == closeTime);

        var expiration = (long?)value["expiration"];
        ExpirationTime = expiration.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(expiration.Value) : null;
        Debug.Assert(ExpirationTime?.ToUnixTimeMilliseconds() == expiration);
    }

    public void UpdateBy(ITradeRecord other)
    {
        ClosePrice = other.ClosePrice;
        Closed = other.Closed;
        Comment = other.Comment;
        Commission = other.Commission;
        CustomComment = other.CustomComment;
        MarginRate = other.MarginRate;
        OpenPrice = other.OpenPrice;
        Order = other.Order;
        Order2 = other.Order2;
        Position = other.Position;
        Profit = other.Profit;
        Sl = other.Sl;
        Storage = other.Storage;
        Symbol = other.Symbol;
        Tp = other.Tp;
        Volume = other.Volume;
        TradeOperation = other.TradeOperation;
        OpenTime = other.OpenTime;
        CloseTime = other.CloseTime;
        ExpirationTime = other.ExpirationTime;
    }

    public void UpdateBy(StreamingTradeRecord other)
    {
        ClosePrice = other.ClosePrice;
        CloseTime = other.CloseTime;
        Closed = other.Closed;
        Comment = other.Comment;
        Commission = other.Commission;
        CustomComment = other.CustomComment;
        ExpirationTime = other.ExpirationTime;
        MarginRate = other.MarginRate;
        OpenPrice = other.OpenPrice;
        OpenTime = other.OpenTime;
        Order = other.Order;
        Order2 = other.Order2;
        Position = other.Position;
        Profit = other.Profit;
        Sl = other.Sl;
        State = other.State;
        Storage = other.Storage;
        Symbol = other.Symbol;
        Tp = other.Tp;
        StreamingTradeType = other.StreamingTradeType;
        TradeOperation = other.TradeOperation;
        Volume = other.Volume;
        Digits = other.Digits;
    }

    public void Reset()
    {
        ClosePrice = null;
        CloseTime = null;
        Closed = null;
        Comment = null;
        Commission = null;
        CustomComment = null;
        ExpirationTime = null;
        MarginRate = null;
        OpenPrice = null;
        OpenTime = null;
        Order = null;
        Order2 = null;
        Position = null;
        Profit = null;
        Sl = null;
        State = null;
        Storage = null;
        Symbol = null;
        Tp = null;
        StreamingTradeType = null;
        Volume = null;
        Digits = null;
        StreamingTradeType = null;
        TradeOperation = null;
    }
}