using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System;
using xAPI.Codes;
using xAPI.Records;
using System.Linq;
using System.Text.Json.Nodes;
using System.Net.Http.Headers;

namespace xAPI.Responses
{
    public class SymbolResponse : BaseResponse
    {
        public SymbolResponse()
            : base()
        { }

        public SymbolResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var ob = ReturnData.AsObject();
            Symbol = new SymbolRecord();
            Symbol.FieldsFromJsonObject(ob);
        }

        public SymbolRecord? Symbol { get; init; }
    }
}