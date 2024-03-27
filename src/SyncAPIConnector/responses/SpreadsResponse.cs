using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class SpreadsResponse : BaseResponse
    {
        private LinkedList<SpreadRecord> spreadRecords = (LinkedList<SpreadRecord>)new LinkedList<SpreadRecord>();

        public SpreadsResponse(string body)
            : base(body)
        {
            JSONArray symbolRecords = (JSONArray)this.ReturnData;
            foreach (JSONObject e in symbolRecords)
            {
                SpreadRecord spreadRecord = new SpreadRecord();
                spreadRecord.FieldsFromJSONObject(e);
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
