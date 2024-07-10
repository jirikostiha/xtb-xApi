using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class CalendarResponse : BaseResponse
    {
        public CalendarResponse()
            : base()
        { }

        public CalendarResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            foreach (JsonObject e in ReturnData.AsArray().OfType<JsonObject>())
            {
                CalendarRecord record = new();
                record.FieldsFromJsonObject(e);
                CalendarRecords.Add(record);
            }
        }

        public List<CalendarRecord> CalendarRecords { get; init; } = [];
    }
}