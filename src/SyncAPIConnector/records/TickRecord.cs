using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, ask:{Ask}, bid:{Bid}")]
    public record TickRecord : IBaseResponseRecord, ITickRecord
    {
        public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

        public double? Ask { get; set; }
        public long? AskVolume { get; set; }
        public double? Bid { get; set; }
        public long? BidVolume { get; set; }
        public double? High { get; set; }
        public long? Level { get; set; }
        public double? Low { get; set; }
        public double? SpreadRaw { get; set; }
        public double? SpreadTable { get; set; }
        public string Symbol { get; set; }
        public long? Timestamp { get; set; }

        public void FieldsFromJsonObject(JsonObject value)
        {
            FieldsFromJsonObject(value, null);
        }

        public bool FieldsFromJsonObject(JsonObject value, string str)
        {
            this.Ask = (double?)value["ask"];
            this.AskVolume = (long?)value["askVolume"];
            this.Bid = (double?)value["bid"];
            this.BidVolume = (long?)value["bidVolume"];
            this.High = (double?)value["high"];
            this.Level = (long?)value["level"];
            this.Low = (double?)value["low"];
            this.SpreadRaw = (double?)value["spreadRaw"];
            this.SpreadTable = (double?)value["spreadTable"];
            this.Symbol = (string)value["symbol"];
            this.Timestamp = (long?)value["timestamp"];

            if ((Ask == null) || (Bid == null) || (Symbol == null) || (Timestamp == null)) return false;
            return true;
        }
    }
}