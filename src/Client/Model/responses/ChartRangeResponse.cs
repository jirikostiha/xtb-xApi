using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class ChartRangeResponse : BaseResponse
{
    public ChartRangeResponse() : base() { }

    public ChartRangeResponse(string body) : base(body)
    {
        if (ReturnData is not JsonObject ob)
        {
            return;
        }

        Digits = (int?)ob["digits"];

        if (ob["rateInfos"] is not JsonArray arr || arr.Count == 0)
        {
            return;
        }

        int count = arr.Count;
        var records = new RateInfoRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (arr[i] is JsonObject jsonObj)
            {
                var record = new RateInfoRecord();
                record.FieldsFromJsonObject(jsonObj);
                records[i] = record;
            }
        }

        RateInfoRecords = records;
    }

    public int? Digits { get; init; }

    public RateInfoRecord[] RateInfoRecords { get; init; } = [];
}
