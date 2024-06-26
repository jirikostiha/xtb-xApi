using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class AllSpreadsResponse : BaseResponse
    {
        public AllSpreadsResponse()
            : base()
        { }

        public AllSpreadsResponse(string body) : base(body)
        {
            if (ReturnData is null)
                return;

            var symbolRecords = ReturnData.AsArray();
            foreach (JsonObject e in symbolRecords.OfType<JsonObject>())
            {
                var spreadRecord = new SpreadRecord();
                spreadRecord.FieldsFromJsonObject(e);
                SpreadRecords.AddLast(spreadRecord);
            }
        }

        public LinkedList<SpreadRecord> SpreadRecords { get; init; } = [];
    }
}