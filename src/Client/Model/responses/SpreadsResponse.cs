
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class SpreadsResponse : BaseResponse
{
    public SpreadsResponse() : base() { }

    public SpreadsResponse(string body) : base(body)
    {
        if (ReturnData is not JsonArray arr || arr.Count == 0)
        {
            return;
        }

        int count = arr.Count;
        var records = new SpreadRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (arr[i] is JsonObject jsonObj)
            {
                var record = new SpreadRecord();
                record.FieldsFromJsonObject(jsonObj);
                records[i] = record;
            }
        }

        SpreadRecords = records;
    }

    public SpreadRecord[] SpreadRecords { get; init; } = [];
}
