using System.Text.Json.Nodes;

namespace xAPI.Records;

using System;

public record RateInfoRecord : IBaseResponseRecord, ICandleRecord
{
    public DateTimeOffset? StartTime { get; set; }

    public double? Open { get; set; }

    public double? High { get; set; }

    public double? Low { get; set; }

    public double? Close { get; set; }

    public double? Volume { get; set; }

    public void FieldsFromJsonObject(JsonObject value)
    {
        Close = (double?)value["close"];
        High = (double?)value["high"];
        Low = (double?)value["low"];
        Open = (double?)value["open"];
        Volume = (double?)value["vol"];

        var ctm = (long?)value["ctm"];
        StartTime = ctm is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(ctm.Value);
    }
}