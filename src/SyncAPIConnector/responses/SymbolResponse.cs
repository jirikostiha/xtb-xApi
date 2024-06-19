using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class SymbolResponse : BaseResponse
    {
        private SymbolRecord symbol;

        public SymbolResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            symbol = new SymbolRecord();
            symbol.FieldsFromJsonObject(ob);
        }

        public virtual SymbolRecord Symbol
        {
            get
            {
                return symbol;
            }
        }
    }
}