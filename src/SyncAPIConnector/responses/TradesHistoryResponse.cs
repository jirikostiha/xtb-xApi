using System.Diagnostics;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("trades:{TradeRecords.Length}")]
public sealed class TradesHistoryResponse : BaseResponse
{
    public TradesHistoryResponse() : base() { }

    public TradesHistoryResponse(string body) : base(body)
    {
        if (ReturnData is not JsonArray arr || arr.Count == 0)
        {
            return;
        }

        int count = arr.Count;
        var records = new TradeRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (arr[i] is JsonObject jsonObj)
            {
                var record = new TradeRecord();
                record.FieldsFromJsonObject(jsonObj);
                records[i] = record;
            }
        }

        TradeRecords = records;
    }

    public TradeRecord[] TradeRecords { get; init; } = [];
}
