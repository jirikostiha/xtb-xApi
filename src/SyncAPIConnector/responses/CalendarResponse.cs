using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class CalendarResponse : BaseResponse
    {
        private List<CalendarRecord> calendarRecords = new List<CalendarRecord>();

        public CalendarResponse(string body)
            : base(body)
        {
            JsonArray returnData = this.ReturnData.AsArray();

            foreach (JsonObject e in returnData)
            {
                CalendarRecord record = new CalendarRecord();
                record.FieldsFromJsonObject(e);
                this.calendarRecords.Add(record);
            }
        }

        public List<CalendarRecord> CalendarRecords
        {
            get { return calendarRecords; }
        }
    }
}