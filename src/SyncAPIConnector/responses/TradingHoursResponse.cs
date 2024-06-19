using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class TradingHoursResponse : BaseResponse
    {
        private LinkedList<TradingHoursRecord> tradingHoursRecords = (LinkedList<TradingHoursRecord>)new LinkedList<TradingHoursRecord>();

        public TradingHoursResponse(string body) : base(body)
        {
            JsonArray ob = this.ReturnData.AsArray();
            foreach (JsonObject e in ob)
            {
                TradingHoursRecord record = new TradingHoursRecord();
                record.FieldsFromJsonObject(e);
                tradingHoursRecords.AddLast(record);
            }
        }

        public virtual LinkedList<TradingHoursRecord> TradingHoursRecords
        {
            get
            {
                return tradingHoursRecords;
            }
        }
    }
}