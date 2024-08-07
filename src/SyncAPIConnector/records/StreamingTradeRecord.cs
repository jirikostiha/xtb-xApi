﻿using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Codes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, pos:{Position}, o:{Order}, o2:{Order2}")]
public record StreamingTradeRecord : IBaseResponseRecord, ITradeRecord
{
    public double? Close_price { get; set; }

    public bool? Closed { get; set; }

    public string? Comment { get; set; }

    public double? Commission { get; set; }

    public string? CustomComment { get; set; }

    public double? Margin_rate { get; set; }

    public double? Open_price { get; set; }

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

    //public long? Cmd { get; set; }

    public TRADE_OPERATION_CODE? TradeOperation { get; set; }

    public STREAMING_TRADE_TYPE? TradeType { get; set; }

    public DateTimeOffset? OpenTime { get; set; }

    public DateTimeOffset? CloseTime { get; set; }

    public DateTimeOffset? ExpirationTime { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Close_price = (double?)value["close_price"];
        Closed = (bool?)value["closed"];
        Comment = (string?)value["comment"];
        Commission = (double?)value["commision"];
        CustomComment = (string?)value["customComment"];
        Margin_rate = (double?)value["margin_rate"];
        Open_price = (double?)value["open_price"];
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

        var tradeType = (long?)value["type"];
        TradeType = tradeType.HasValue ? new STREAMING_TRADE_TYPE(tradeType.Value) : null;

        var tradeOperationCode = (long?)value["cmd"];
        TradeOperation = tradeOperationCode.HasValue ? new TRADE_OPERATION_CODE(tradeOperationCode.Value) : null;

        var openTime = (long?)value["open_time"];
        OpenTime = openTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(openTime.Value) : null;

        var closeTime = (long?)value["close_time"];
        CloseTime = closeTime.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(closeTime.Value) : null;

        var expiration = (long?)value["expiration"];
        ExpirationTime = expiration.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(expiration.Value) : null;
    }

    public void UpdateBy(ITradeRecord other)
    {
        Close_price = other.Close_price;
        Closed = other.Closed;
        Comment = other.Comment;
        Commission = other.Commission;
        CustomComment = other.CustomComment;
        Margin_rate = other.Margin_rate;
        Open_price = other.Open_price;
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
        Close_price = other.Close_price;
        CloseTime = other.CloseTime;
        Closed = other.Closed;
        Comment = other.Comment;
        Commission = other.Commission;
        CustomComment = other.CustomComment;
        ExpirationTime = other.ExpirationTime;
        Margin_rate = other.Margin_rate;
        Open_price = other.Open_price;
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
        TradeType = other.TradeType;
        TradeOperation = other.TradeOperation;
        Volume = other.Volume;
        Digits = other.Digits;
    }

    public void Reset()
    {
        Close_price = null;
        CloseTime = null;
        Closed = null;
        Comment = null;
        Commission = null;
        CustomComment = null;
        ExpirationTime = null;
        Margin_rate = null;
        Open_price = null;
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
        TradeType = null;
        Volume = null;
        Digits = null;
        TradeType = null;
        TradeOperation = null;
    }
}