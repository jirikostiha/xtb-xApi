using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using XApi.Codes;

namespace XApi.Records;

[DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}, profit:{Profit}, volume:{Volume}")]
public record TradeRecord : IBaseResponseRecord, ITradeRecord
{
    public double? ClosePrice { get; set; }

    public bool? Closed { get; set; }

    public string? Comment { get; set; }

    public double? Commission { get; set; }

    public double? CommissionAgent { get; set; }

    public string? CustomComment { get; set; }

    public int? Digits { get; set; }

    public double? MarginRate { get; set; }

    public double? OpenPrice { get; set; }

    public long? Order { get; set; }

    public long? Order2 { get; set; }

    public long? Position { get; set; }

    public double? Profit { get; set; }

    public double? Sl { get; set; }

    public double? Storage { get; set; }

    public string? Symbol { get; set; }

    public double? Tp { get; set; }

    public long? ValueDate { get; set; }

    public double? Volume { get; set; }

    public TRADE_OPERATION_TYPE? TradeOperation { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public DateTimeOffset? OpenTime { get; set; }

    public DateTimeOffset? CloseTime { get; set; }

    public DateTimeOffset? ExpirationTime { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        ClosePrice = (double?)value["close_price"];
        Closed = (bool?)value["closed"];
        Comment = (string?)value["comment"];
        Commission = (double?)value["commission"];
        CommissionAgent = (double?)value["commission_agent"];
        CustomComment = (string?)value["customComment"];
        Digits = (int?)value["digits"];
        MarginRate = (double?)value["margin_rate"];
        OpenPrice = (double?)value["open_price"];
        Order = (long?)value["order"];
        Order2 = (long?)value["order2"];
        Position = (long?)value["position"];
        Profit = (double?)value["profit"];
        Sl = (double?)value["sl"];
        Storage = (double?)value["storage"];
        Symbol = (string?)value["symbol"];
        Tp = (double?)value["tp"];
        ValueDate = (long?)value["value_date"];
        Volume = (double?)value["volume"];

        var tradeOperationCode = (int?)value["cmd"];
        TradeOperation = tradeOperationCode.HasValue ? new TRADE_OPERATION_TYPE(tradeOperationCode.Value) : null;

        var timestamp = (long?)value["timestamp"];
        Timestamp = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : null;
        Debug.Assert(Timestamp?.ToUnixTimeMilliseconds() == timestamp);

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
}