using System.Linq;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public sealed class AllSpreadsResponse : BaseResponse
{
    public AllSpreadsResponse() : base()
    { }

    public AllSpreadsResponse(string body) : base(body)
    {
        if (ReturnData is null)
        {
            return;
        }

        var symbolRecordsArray = ReturnData.AsArray();
        int count = symbolRecordsArray.Count;

        var records = new SpreadRecord[count];
        int index = 0;

        foreach (var jsonObj in symbolRecordsArray.OfType<JsonObject>())
        {
            var spreadRecord = new SpreadRecord();
            spreadRecord.FieldsFromJsonObject(jsonObj);
            records[index++] = spreadRecord;
        }

        SpreadRecords = records;
    }

    public SpreadRecord[] SpreadRecords { get; } = [];
}
