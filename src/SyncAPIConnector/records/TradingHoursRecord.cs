using System.Collections.Generic;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    public class TradingHoursRecord : BaseResponseRecord
    {
        private string symbol;
        private LinkedList<HoursRecord> quotes = (LinkedList<HoursRecord>)new LinkedList<HoursRecord>();
        private LinkedList<HoursRecord> trading = (LinkedList<HoursRecord>)new LinkedList<HoursRecord>();

        public virtual string Symbol
        {
            get
            {
                return symbol;
            }
        }

        public virtual LinkedList<HoursRecord> Quotes
        {
            get
            {
                return quotes;
            }
        }

        public virtual LinkedList<HoursRecord> Trading
        {
            get
            {
                return trading;
            }
        }

        public void FieldsFromJSONObject(JSONObject value)
        {
            FieldsFromJSONObject(value, null);
        }

        public bool FieldsFromJSONObject(JSONObject value, string str)
        {
            this.symbol = (string)value["symbol"];
            quotes = new LinkedList<HoursRecord>();
            if (value["quotes"] != null)
            {
                JSONArray jsonarray = (JSONArray)value["quotes"];
                foreach (JSONObject i in jsonarray)
                {
                    HoursRecord rec = new HoursRecord();
                    rec.FieldsFromJSONObject(i);
                    quotes.AddLast(rec);
                }
            }
            trading = new LinkedList<HoursRecord>();
            if (value["trading"] != null)
            {
                JSONArray jsonarray = (JSONArray)value["trading"];
                foreach (JSONObject i in jsonarray)
                {
                    HoursRecord rec = new HoursRecord();
                    rec.FieldsFromJSONObject(i);
                    trading.AddLast(rec);
                }
            }
            if ((symbol == null) || (quotes.Count == 0) || (trading.Count == 0)) return false;
            return true;
        }

        public override string ToString()
        {
            return "TradingHoursRecord{" + "symbol=" + symbol + ", quotes=" + quotes.ToString() + ", trading=" + trading.ToString() + '}';
        }
    }
}
