using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class ChartLastResponse : BaseResponse
    {
        private long? digits;
        private LinkedList<RateInfoRecord> rateInfos = (LinkedList<RateInfoRecord>)new LinkedList<RateInfoRecord>();

        public ChartLastResponse(string body) : base(body)
        {
            JsonObject rd = this.ReturnData.AsObject();
            this.digits = (long?)rd["digits"];
            JsonArray arr = rd["rateInfos"].AsArray();

            foreach (JsonObject e in arr)
            {
                RateInfoRecord record = new RateInfoRecord();
                record.FieldsFromJsonObject(e);
                this.rateInfos.AddLast(record);
            }
        }

        public virtual long? Digits
        {
            get
            {
                return digits;
            }
        }

        public virtual LinkedList<RateInfoRecord> RateInfos
        {
            get
            {
                return rateInfos;
            }
        }
    }
}