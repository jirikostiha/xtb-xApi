using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class TickPricesResponse : BaseResponse
	{
		private LinkedList<TickRecord> ticks = (LinkedList<TickRecord>) new LinkedList<TickRecord>();

		public TickPricesResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
			JSONArray arr = (JSONArray) ob["quotations"];
			foreach (JSONObject e in arr)
			{
				TickRecord record = new TickRecord();
				record.FieldsFromJSONObject(e);
                ticks.AddLast(record);
			}
		}

		public virtual LinkedList<TickRecord> Ticks
		{
			get
			{
				return ticks;
			}
		}
	}
}