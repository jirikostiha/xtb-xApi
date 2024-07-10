using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class TradingHoursResponse : BaseResponse
    {
        public TradingHoursResponse()
            : base()
        { }

        public TradingHoursResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var arr = ReturnData.AsArray();
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                var record = new TradingHoursRecord();
                record.FieldsFromJsonObject(e);
                TradingHoursRecords.AddLast(record);
            }
        }

        public LinkedList<TradingHoursRecord> TradingHoursRecords { get; init; } = [];
    }
}