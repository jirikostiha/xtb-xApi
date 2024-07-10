using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}, profit:{Profit}, volume:{Volume}")]
public record TradeRecord : IBaseResponseRecord, ITradeRecord
{
    public double? Close_price { get; set; }

    public long? Close_time { get; set; }

    public bool? Closed { get; set; }

    public long? Cmd { get; set; }

    public string? Comment { get; set; }

    public double? Commission { get; set; }

    public double? Commission_agent { get; set; }

    public string? CustomComment { get; set; }

    public long? Digits { get; set; }

    public long? Expiration { get; set; }

    public string? ExpirationString { get; set; }

    public double? Margin_rate { get; set; }

    public double? Open_price { get; set; }

    public long? Open_time { get; set; }

    public long? Order { get; set; }

    public long? Order2 { get; set; }

    public long? Position { get; set; }

    public double? Profit { get; set; }

    public double? Sl { get; set; }

    public double? Storage { get; set; }

    public string? Symbol { get; set; }

    public long? Timestamp { get; set; }

    public double? Tp { get; set; }

    public long? Value_date { get; set; }

    public double? Volume { get; set; }

    public TRADE_OPERATION_CODE? Cmd2 { get; set; }

    public DateTimeOffset? OpenDateTime => Open_time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Open_time.Value);

    public DateTimeOffset? CloseDateTime => Close_time is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Close_time.Value);

    public DateTimeOffset? ExpirationDateTime => Expiration is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Expiration.Value);

    public void FieldsFromJsonObject(JsonObject value)
    {
        Close_price = (double?)value["close_price"];
        Close_time = (long?)value["close_time"];
        Closed = (bool?)value["closed"];
        Cmd = (long?)value["cmd"];
        Cmd2 = value["cmd"] is not null ? new TRADE_OPERATION_CODE((long)value["cmd"]) : null;
        Comment = (string?)value["comment"];
        Commission = (double?)value["commission"];
        Commission_agent = (double?)value["commission_agent"];
        CustomComment = (string?)value["customComment"];
        Digits = (long?)value["digits"];
        Expiration = (long?)value["expiration"];
        ExpirationString = (string?)value["expirationString"];
        Margin_rate = (double?)value["margin_rate"];
        Open_price = (double?)value["open_price"];
        Open_time = (long?)value["open_time"];
        Order = (long?)value["order"];
        Order2 = (long?)value["order2"];
        Position = (long?)value["position"];
        Profit = (double?)value["profit"];
        Sl = (double?)value["sl"];
        Storage = (double?)value["storage"];
        Symbol = (string?)value["symbol"];
        Timestamp = (long?)value["timestamp"];
        Tp = (double?)value["tp"];
        Value_date = (long?)value["value_date"];
        Volume = (double?)value["volume"];
    }
}