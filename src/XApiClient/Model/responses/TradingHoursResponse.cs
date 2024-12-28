using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
namespace Xtb.XApiClient.Model;

public sealed class TradingHoursResponse : BaseResponse
{
    public TradingHoursResponse()
        : base()
    { }

    public TradingHoursResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var arr = ReturnData.AsArray();
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new TradingHoursRecord();
            record.FieldsFromJsonObject(e);
            TradingHoursRecords.AddLast(record);
        }
    }

    public LinkedList<TradingHoursRecord> TradingHoursRecords { get; init; } = [];
}