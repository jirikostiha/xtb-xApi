using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Records;
using System.Linq;

namespace xAPI.Responses
{
    [DebuggerDisplay("trades:{TradeRecords.Count}")]
    public class TradeRecordsResponse : BaseResponse
    {
        public TradeRecordsResponse()
            : base()
        { }

        public TradeRecordsResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var arr = ReturnData.AsArray();
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                var record = new TradeRecord();
                record.FieldsFromJsonObject(e);
                TradeRecords.AddLast(record);
            }
        }

        public LinkedList<TradeRecord> TradeRecords { get; init; } = [];
    }
}