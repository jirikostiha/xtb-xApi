using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    [DebuggerDisplay("status:{Status}, count:{TradeRecords.Count}")]
    public class TradesResponse : BaseResponse
    {
        private LinkedList<TradeRecord> tradeRecords = (LinkedList<TradeRecord>)new LinkedList<TradeRecord>();

        public TradesResponse(string body) : base(body)
        {
            JsonArray arr = this.ReturnData.AsArray();
            foreach (JsonObject e in arr)
            {
                TradeRecord record = new TradeRecord();
                record.FieldsFromJsonObject(e);
                tradeRecords.AddLast(record);
            }

        }

        public virtual LinkedList<TradeRecord> TradeRecords
        {
            get
            {
                return tradeRecords;
            }
        }
    }
}