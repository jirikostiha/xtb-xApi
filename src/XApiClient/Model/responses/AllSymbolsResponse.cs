using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public sealed class AllSymbolsResponse : BaseResponse
{
    public AllSymbolsResponse()
        : base()
    { }

    public AllSymbolsResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var symbolRecordsArray = ReturnData.AsArray();
        foreach (var e in symbolRecordsArray.OfType<JsonObject>())
        {
            var symbolRecord = new SymbolRecord();
            symbolRecord.FieldsFromJsonObject(e);
            SymbolRecords.AddLast(symbolRecord);
        }
    }

    public LinkedList<SymbolRecord> SymbolRecords { get; init; } = [];
}