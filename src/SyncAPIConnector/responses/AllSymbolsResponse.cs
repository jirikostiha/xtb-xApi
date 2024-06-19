using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class AllSymbolsResponse : BaseResponse
    {
        private LinkedList<SymbolRecord> symbolRecords = (LinkedList<SymbolRecord>)new LinkedList<SymbolRecord>();

        public AllSymbolsResponse(string body) : base(body)
        {
            JsonArray symbolRecords = this.ReturnData.AsArray();
            foreach (JsonObject e in symbolRecords)
            {
                SymbolRecord symbolRecord = new SymbolRecord();
                symbolRecord.FieldsFromJsonObject(e);
                this.symbolRecords.AddLast(symbolRecord);
            }
        }

        public virtual LinkedList<SymbolRecord> SymbolRecords
        {
            get
            {
                return symbolRecords;
            }
        }
    }

}