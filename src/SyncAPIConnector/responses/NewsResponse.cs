using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class NewsResponse : BaseResponse
	{
		private LinkedList<NewsTopicRecord> newsTopicRecords = (LinkedList<NewsTopicRecord>) new LinkedList<NewsTopicRecord>();

		public NewsResponse(string body) : base(body)
		{
			JSONArray arr = (JSONArray) this.ReturnData;
			foreach (JSONObject e in arr)
			{
				NewsTopicRecord record = new NewsTopicRecord();
				record.FieldsFromJSONObject(e);
                newsTopicRecords.AddLast(record);
			}
		}

		public virtual LinkedList<NewsTopicRecord> NewsTopicRecords
		{
			get
			{
				return newsTopicRecords;
			}
		}
	}
}