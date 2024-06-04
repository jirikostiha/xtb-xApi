using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class TickPricesResponse : BaseResponse
    {
        private LinkedList<TickRecord> ticks = (LinkedList<TickRecord>)new LinkedList<TickRecord>();

        public TickPricesResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            JsonArray arr = ob["quotations"].AsArray();
            foreach (JsonObject e in arr)
            {
                TickRecord record = new TickRecord();
                record.FieldsFromJsonObject(e);
                ticks.AddLast(record);
            }
        }

        public virtual LinkedList<TickRecord> Ticks
        {
            get
            {
                return ticks;
            }
        }
    }
}