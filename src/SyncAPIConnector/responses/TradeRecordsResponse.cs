using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("trades:{TradeRecords.Count}")]
public sealed class TradeRecordsResponse : BaseResponse
{
    public TradeRecordsResponse()
        : base()
    { }

    public TradeRecordsResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var arr = ReturnData.AsArray();
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new TradeRecord();
            record.FieldsFromJsonObject(e);
            TradeRecords.AddLast(record);
        }
    }

    public LinkedList<TradeRecord> TradeRecords { get; init; } = [];
}