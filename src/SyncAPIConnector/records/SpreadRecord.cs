using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class SpreadRecord : BaseResponseRecord
    {
        private long? precision;
        private string symbol;
        private long? value;
        private long? quoteId;

        public SpreadRecord()
        {
        }

        public virtual long? Precision
        {
            get
            {
                return precision;
            }
            set
            {
                this.precision = value;
            }
        }

        public virtual string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                this.symbol = value;
            }
        }

        public virtual long? QuoteId
        {
            get 
            { 
                return quoteId; 
            }
            set 
            { 
                quoteId = value; 
            }
        }

        public virtual long? Value
        {
            get 
            { 
                return this.value; 
            }
            set 
            { 
                this.value = value; 
            }
        }

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.Symbol = (string)value["symbol"];
            this.Precision = (long?)value["precision"];
            this.Value = (long?)value["value"];
            this.QuoteId = (long?)value["quoteId"];
        }
    }
}
