using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;

    [DebuggerDisplay("{Symbol}")]
    public record TradingHoursRecord : BaseResponseRecord, ISymbol
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

        /// <summary>
        /// Determines whether the specified time falls within the quote hours.
        /// </summary>
        /// <param name="time">The <see cref="DateTimeOffset"/> to check.</param>
        /// <returns>
        /// <c>true</c> if the specified time is within the quote hours;
        /// <c>false</c> if it is not;
        /// <c>null</c> if the Quotes collection is <c>null</c>.
        /// </returns>
        public virtual bool? IsInQuotesHours(DateTimeOffset time)
        {
            if (Quotes is null)
                return null;

            foreach (var hoursRecord in Quotes)
            {
                if (hoursRecord.DayOfWeek == time.DayOfWeek
                    && (hoursRecord.IsInTimeInterval(time.TimeOfDay) ?? false))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified time falls within the trading hours.
        /// </summary>
        /// <param name="time">The <see cref="DateTimeOffset"/> to check.</param>
        /// <returns>
        /// <c>true</c> if the specified time is within the trading hours;
        /// <c>false</c> if it is not;
        /// <c>null</c> if the Trading collection is <c>null</c>.
        /// </returns>
        public virtual bool? IsInTradingHours(DateTimeOffset time)
        {
            if (Trading is null)
                return null;

            foreach (var hoursRecord in Trading)
            {
                if (hoursRecord.DayOfWeek == time.DayOfWeek
                    && (hoursRecord.IsInTimeInterval(time.TimeOfDay) ?? false))
                    return true;
            }

            return false;
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
