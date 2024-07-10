using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class NewsResponse : BaseResponse
    {
        public NewsResponse()
            : base()
        { }

        public NewsResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var arr = ReturnData.AsArray();
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                var record = new NewsTopicRecord();
                record.FieldsFromJsonObject(e);
                NewsTopicRecords.AddLast(record);
            }
        }

        public LinkedList<NewsTopicRecord> NewsTopicRecords { get; init; } = [];
    }
}