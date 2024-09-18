using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

[DebuggerDisplay("{Symbol}, ask:{Ask}, bid:{Bid}")]
public record StreamingTickRecord : IBaseResponseRecord, ITickRecord
{
    public double? Ask { get; set; }

    public double? Bid { get; set; }

    public int? AskVolume { get; set; }

    public int? BidVolume { get; set; }

    public double? High { get; set; }

    public double? Low { get; set; }

    public string? Symbol { get; set; }

    public double? SpreadRaw { get; set; }

    public double? SpreadTable { get; set; }

    public int? Level { get; set; }

    public int? QuoteId { get; set; }

    public DateTimeOffset? Time { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Ask = (double?)value["ask"];
        Bid = (double?)value["bid"];
        AskVolume = (int?)value["askVolume"];
        BidVolume = (int?)value["bidVolume"];
        High = (double?)value["high"];
        Low = (double?)value["low"];
        Symbol = (string?)value["symbol"];
        Level = (int?)value["level"];
        QuoteId = (int?)value["quoteId"];
        SpreadRaw = (double?)value["spreadRaw"];
        SpreadTable = (double?)value["spreadTable"];

        var timestamp = (long?)value["timestamp"];
        Time = timestamp.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp.Value) : null;
        Debug.Assert(Time?.ToUnixTimeMilliseconds() == timestamp);
    }
}