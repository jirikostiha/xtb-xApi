using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class AllSpreadsResponse : BaseResponse
    {
        private LinkedList<SpreadRecord> spreadRecords = (LinkedList<SpreadRecord>)new LinkedList<SpreadRecord>();

        public AllSpreadsResponse(string body) : base(body)
        {
            JsonArray symbolRecords = this.ReturnData.AsArray();
            foreach (JsonObject e in symbolRecords)
            {
                SpreadRecord spreadRecord = new SpreadRecord();
                spreadRecord.FieldsFromJsonObject(e);
                this.spreadRecords.AddLast(spreadRecord);
            }
        }

        public virtual LinkedList<SpreadRecord> SpreadRecords
        {
            get
            {
                return spreadRecords;
            }
        }
    }
}
