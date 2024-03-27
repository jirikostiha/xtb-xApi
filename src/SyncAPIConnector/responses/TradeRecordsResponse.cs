using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TradeRecordsResponse : BaseResponse
	{
		private LinkedList<TradeRecord> tradeRecords = (LinkedList<TradeRecord>) new LinkedList<TradeRecord>();

		public TradeRecordsResponse(string body) : base(body)
		{
			JSONArray arr = (JSONArray) this.ReturnData;
            foreach (JSONObject e in arr)
            {
                TradeRecord record = new TradeRecord();
                record.FieldsFromJSONObject(e);
                tradeRecords.AddLast(record);
            }
		}

		public virtual LinkedList<TradeRecord> TradeRecords
		{
			get
			{
				return tradeRecords;
			}
		}
	}
}