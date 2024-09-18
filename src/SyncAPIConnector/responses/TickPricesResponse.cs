using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("ticks:{Ticks.Count}")]
public class TickPricesResponse : BaseResponse
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
        foreach (JsonObject e in arr.OfType<JsonObject>())
        {
            var record = new TickRecord();
            record.FieldsFromJsonObject(e);
            Ticks.AddLast(record);
        }
    }

    public LinkedList<TickRecord>? Ticks { get; init; } = [];
}