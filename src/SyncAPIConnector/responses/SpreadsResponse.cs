using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;
using System.Linq;

namespace xAPI.Responses
{
    public class SpreadsResponse : BaseResponse
    {
        public SpreadsResponse()
            : base()
        { }

        public SpreadsResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var arr = ReturnData.AsArray();
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                var record = new SpreadRecord();
                record.FieldsFromJsonObject(e);
                SpreadRecords.AddLast(record);
            }
        }

        public LinkedList<SpreadRecord> SpreadRecords { get; init; } = [];
    }
}