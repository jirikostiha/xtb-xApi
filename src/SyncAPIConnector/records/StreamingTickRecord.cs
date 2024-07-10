using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, ask:{Ask}, bid:{Bid}")]
    public record StreamingTickRecord : IBaseResponseRecord, ITickRecord
    {
        public double? Ask { get; set; }

        public double? Bid { get; set; }

        public long? AskVolume { get; set; }

        public long? BidVolume { get; set; }

        public double? High { get; set; }

        public double? Low { get; set; }

        public string Symbol { get; set; }

        public double? SpreadRaw { get; set; }

        public double? SpreadTable { get; set; }

        public long? Timestamp { get; set; }

        public long? Level { get; set; }

        public long? QuoteId { get; set; }

        public DateTimeOffset? DateTime => Timestamp is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Timestamp.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.Ask = (double?)value["ask"];
            this.Bid = (double?)value["bid"];
            this.AskVolume = (long?)value["askVolume"];
            this.BidVolume = (long?)value["bidVolume"];
            this.High = (double?)value["high"];
            this.Low = (double?)value["low"];
            this.Symbol = (string)value["symbol"];
            this.Timestamp = (long?)value["timestamp"];
            this.Level = (long?)value["level"];
            this.QuoteId = (long?)value["quoteId"];
            this.SpreadRaw = (double?)value["spreadRaw"];
            this.SpreadTable = (double?)value["spreadTable"];
        }
    }
}