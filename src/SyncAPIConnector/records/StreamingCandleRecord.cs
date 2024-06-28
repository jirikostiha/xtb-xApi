using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    [DebuggerDisplay("{Symbol}, low:{Low}, high:{High}")]
    public record StreamingCandleRecord : IBaseResponseRecord, ISymbol, ICandleRecord
    {
        public double? Close { get; set; }

        public long? Ctm { get; set; }

        public string CtmString { get; set; }

        public double? High { get; set; }

        public double? Low { get; set; }

        public double? Open { get; set; }

        public long? QuoteId { get; set; }

        public string Symbol { get; set; }

        public double? Vol { get; set; }

        public DateTimeOffset? StartDateTime => Ctm is null ? null : DateTimeOffset.FromUnixTimeMilliseconds(Ctm.Value);

        public void FieldsFromJsonObject(JsonObject value)
        {
            Close = (double?)value["close"];
            Ctm = (long?)value["ctm"];
            CtmString = (string)value["ctmString"];
            High = (double?)value["high"];
            Low = (double?)value["low"];
            Open = (double?)value["open"];
            QuoteId = (long?)value["quoteId"];
            Symbol = (string)value["symbol"];
            Vol = (double?)value["vol"];
        }
    }
}