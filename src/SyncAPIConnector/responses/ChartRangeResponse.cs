using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class ChartRangeResponse : BaseResponse
	{
		private long? digits;
		private LinkedList<RateInfoRecord> rateInfos = (LinkedList<RateInfoRecord>) new LinkedList<RateInfoRecord>();

		public ChartRangeResponse(string body) : base(body)
		{
			JSONObject rd = (JSONObject) this.ReturnData;
			this.digits = (long?) rd["digits"];
			JSONArray arr = (JSONArray) rd["rateInfos"];
            foreach (JSONObject e in arr)
            {
                RateInfoRecord record = new RateInfoRecord();
                record.FieldsFromJSONObject(e);
                this.rateInfos.AddLast(record);
            }
            
		}

		public virtual long? Digits
		{
			get
			{
				return digits;
			}
		}

		public virtual LinkedList<RateInfoRecord> RateInfos
		{
			get
			{
				return rateInfos;
			}
		}

	}

}