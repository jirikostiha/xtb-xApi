using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public sealed class TradingHoursResponse : BaseResponse
{
    public TradingHoursResponse() : base() { }

    public TradingHoursResponse(string body) : base(body)
    {
        if (ReturnData is not JsonArray arr || arr.Count == 0)
        {
            return;
        }

        int count = arr.Count;
        var records = new TradingHoursRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (arr[i] is JsonObject jsonObj)
            {
                var record = new TradingHoursRecord();
                record.FieldsFromJsonObject(jsonObj);
                records[i] = record;
            }
        }

        TradingHoursRecords = records;
    }

    public TradingHoursRecord[] TradingHoursRecords { get; init; } = [];
}
