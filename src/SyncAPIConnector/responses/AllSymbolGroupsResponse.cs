using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    public class AllSymbolGroupsResponse : BaseResponse
    {
        private LinkedList<SymbolGroupRecord> symbolGroupRecords = (LinkedList<SymbolGroupRecord>)new LinkedList<SymbolGroupRecord>();

        public AllSymbolGroupsResponse(string body) : base(body)
        {

        }

        public virtual LinkedList<SymbolGroupRecord> SymbolGroupRecords
        {
            get
            {
                return symbolGroupRecords;
            }
        }
    }
}