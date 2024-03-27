using System.Collections.Generic;

namespace xAPI.Records
{
    using System;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StreamingCandleRecord : BaseResponseRecord
    {
        public StreamingCandleRecord()
        {
        }

        public double? Close
        {
            get;
            set;
        }

        public long? Ctm
        {
            get;
            set;
        }

        public string CtmString
        {
            get;
            set;
        }

        public double? High
        {
            get;
            set;
        }

        public double? Low
        {
            get;
            set;
        }

        public double? Open
        {
            get;
            set;
        }

        public long? QuoteId
        {
            get;
            set;
        }

        public string Symbol
        {
            get;
            set;
        }

        public double? Vol
        {
            get;
            set;
        }

        public void FieldsFromJSONObject(JSONObject value)
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

        public override string ToString()
        {
            return "StreamingCandleRecord { " +
                " close: " + Close +
                " ctm: " + Ctm +
                " ctmString: " + CtmString +
                " high: " + High +
                " low: " + Low +
                " open: " + Open +
                " quoteId: " + QuoteId +
                " symbol: " + Symbol +
                " vol: " + Vol +
                " }";
        }
    }
}