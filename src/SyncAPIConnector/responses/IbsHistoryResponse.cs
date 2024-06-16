using System.Collections.Generic;
using xAPI.Records;
using System.Text.Json.Nodes;
using System.Linq;

namespace xAPI.Responses
{
    public class IbsHistoryResponse : BaseResponse
    {
        public IbsHistoryResponse()
            : base()
        { }

        public IbsHistoryResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var arr = ReturnData.AsArray();
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                IbRecord record = new IbRecord();
                record.FieldsFromJsonObject(e);
                IbRecords.AddLast(record);
            }
        }

        public LinkedList<IbRecord> IbRecords { get; init; } = [];
    }
}