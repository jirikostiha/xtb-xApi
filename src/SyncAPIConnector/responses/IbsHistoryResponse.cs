using System.Collections.Generic;
using xAPI.Records;
using System.Text.Json.Nodes;

namespace xAPI.Responses
{
    public class IbsHistoryResponse : BaseResponse
    {
        /// <summary>
        /// IB records.
        /// </summary>
        public LinkedList<IbRecord> IbRecords { get; set; }

        public IbsHistoryResponse(string body)
            : base(body)
        {
            JsonArray arr = this.ReturnData.AsArray();

            foreach (JsonObject e in arr)
            {
                IbRecord record = new IbRecord(e);
                this.IbRecords.AddLast(record);
            }
        }
    }
}