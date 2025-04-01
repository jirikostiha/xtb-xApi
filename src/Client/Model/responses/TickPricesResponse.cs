using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

[DebuggerDisplay("ticks:{TickRecords.Length}")]
public sealed class TickPricesResponse : BaseResponse
{
    public TickPricesResponse() : base()
    { }

    public TickPricesResponse(string body) : base(body)
    {
        if (ReturnData is null)
        {
            return;
        }

        var ob = ReturnData.AsObject();
        var arr = ob["quotations"]?.AsArray();

        if (arr is null)
        {
            return;
        }

        int count = arr.Count;
        var records = new TickRecord[count];
        int index = 0;

        foreach (var jsonObj in arr.OfType<JsonObject>())
        {
            var record = new TickRecord();
            record.FieldsFromJsonObject(jsonObj);
            records[index++] = record;
        }

        TickRecords = records;
    }

    public TickRecord[] TickRecords { get; } = [];
}
