using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, ask:{Ask}, bid:{Bid}")]
public record TickRecord : IBaseResponseRecord, ITickRecord
{
    public double? Ask { get; set; }

    public long? AskVolume { get; set; }

    public double? Bid { get; set; }

    public long? BidVolume { get; set; }

    public double? High { get; set; }

    public long? Level { get; set; }

    public double? Low { get; set; }

    public double? SpreadRaw { get; set; }

    public double? SpreadTable { get; set; }

    public string? Symbol { get; set; }

    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Ask = (double?)value["ask"];
        AskVolume = (long?)value["askVolume"];
        Bid = (double?)value["bid"];
        BidVolume = (long?)value["bidVolume"];
        High = (double?)value["high"];
        Level = (long?)value["level"];
        Low = (double?)value["low"];
        SpreadRaw = (double?)value["spreadRaw"];
        SpreadTable = (double?)value["spreadTable"];
        Symbol = (string?)value["symbol"];

        var timestamp = (long?)value["timestamp"];
        Time = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : null;
    }
}