using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class CalendarResponse : BaseResponse
	{
        private List<CalendarRecord> calendarRecords = new List<CalendarRecord>();

        public CalendarResponse(string body)
            : base(body)
		{
            JSONArray returnData = (JSONArray)this.ReturnData;

            foreach (JSONObject e in returnData)
			{
                CalendarRecord record = new CalendarRecord();
				record.FieldsFromJSONObject(e);
                this.calendarRecords.Add(record);
			}
		}

        public List<CalendarRecord> CalendarRecords
        {
            get { return calendarRecords; }
        }
	}
}