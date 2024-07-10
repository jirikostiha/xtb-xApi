using System.Text.Json.Nodes;

namespace xAPI.Records
{
    using System;

    public record RateInfoRecord : IBaseResponseRecord, ICandleRecord
    {
        public DateTimeOffset? StartDateTime => Ctm is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Ctm.Value);

        public long? Ctm { get; set; }

        public double? Open { get; set; }

        public double? High { get; set; }

        public double? Low { get; set; }

        public double? Close { get; set; }

        public double? Vol { get; set; }

        public void FieldsFromJsonObject(JsonObject value)
        {
            Close = (double?)value["close"];
            Ctm = (long?)value["ctm"];
            High = (double?)value["high"];
            Low = (double?)value["low"];
            Open = (double?)value["open"];
            Vol = (double?)value["vol"];
        }
    }
}