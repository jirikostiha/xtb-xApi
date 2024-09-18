using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using XApi.Records;

namespace XApi.Responses;

[DebuggerDisplay("trades:{TradeRecords.Count}")]
public class TradesResponse : BaseResponse
{
    public TradesResponse()
        : base()
    { }

    public TradesResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var arr = ReturnData.AsArray();
        foreach (JsonObject e in arr.OfType<JsonObject>())
        {
            var record = new TradeRecord();
            record.FieldsFromJsonObject(e);
            TradeRecords.Add(record);
        }
    }

    public List<TradeRecord> TradeRecords { get; init; } = [];
}