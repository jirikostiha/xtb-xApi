using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradingHoursResponse : BaseResponse
	{
        private LinkedList<TradingHoursRecord> tradingHoursRecords = (LinkedList<TradingHoursRecord>)new LinkedList<TradingHoursRecord>();

        public TradingHoursResponse(string body) : base(body)
        {
            JSONArray ob = (JSONArray)this.ReturnData;
            foreach (JSONObject e in ob)
            {
                TradingHoursRecord record = new TradingHoursRecord();
                record.FieldsFromJSONObject(e);
                tradingHoursRecords.AddLast(record);
            }
        }

        public virtual LinkedList<TradingHoursRecord> TradingHoursRecords
        {
            get
            {
                return tradingHoursRecords;
            }
        }
	}
}