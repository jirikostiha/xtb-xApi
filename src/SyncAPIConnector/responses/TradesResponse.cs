using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("trades:{TradeRecords.Count}")]
public sealed class TradesResponse : BaseResponse
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
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new TradeRecord();
            record.FieldsFromJsonObject(e);
            TradeRecords.Add(record);
        }
    }

    public List<TradeRecord> TradeRecords { get; init; } = [];
}