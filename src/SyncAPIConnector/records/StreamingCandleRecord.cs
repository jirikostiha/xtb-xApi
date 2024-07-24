using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records;

[DebuggerDisplay("{Symbol}, low:{Low}, high:{High}")]
public record StreamingCandleRecord : IBaseResponseRecord, ISymbol, ICandleRecord
{
    public double? Close { get; set; }

    public double? High { get; set; }

    public double? Low { get; set; }

    public double? Open { get; set; }

    public int? QuoteId { get; set; }

    public string? Symbol { get; set; }

    public double? Volume { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Close = (double?)value["close"];
        High = (double?)value["high"];
        Low = (double?)value["low"];
        Open = (double?)value["open"];
        QuoteId = (int?)value["quoteId"];
        Symbol = (string?)value["symbol"];
        Volume = (double?)value["vol"];

        var ctm = (long?)value["ctm"];
        StartTime = ctm.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(ctm.Value) : null;
        //CtmString = (string?)value["ctmString"];
    }
}