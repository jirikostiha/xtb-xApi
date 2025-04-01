using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class AllSymbolsResponse : BaseResponse
{
    public AllSymbolsResponse() : base() { }

    public AllSymbolsResponse(string body) : base(body)
    {
        if (ReturnData is not JsonArray jsonArray || jsonArray.Count == 0)
        {
            return;
        }

        int count = jsonArray.Count;
        var records = new SymbolRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (jsonArray[i] is JsonObject jsonObj)
            {
                var symbolRecord = new SymbolRecord();
                symbolRecord.FieldsFromJsonObject(jsonObj);
                records[i] = symbolRecord;
            }
        }

        SymbolRecords = records;
    }

    public SymbolRecord[] SymbolRecords { get; init; } = [];
}
