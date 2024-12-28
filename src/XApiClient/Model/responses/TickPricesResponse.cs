using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

[DebuggerDisplay("ticks:{Ticks.Count}")]
public sealed class TickPricesResponse : BaseResponse
{
    public TickPricesResponse()
        : base()
    { }

    public TickPricesResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        var arr = ob["quotations"]?.AsArray();
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new TickRecord();
            record.FieldsFromJsonObject(e);
            TickRecords.AddLast(record);
        }
    }

    public LinkedList<TickRecord>? TickRecords { get; init; } = [];
}