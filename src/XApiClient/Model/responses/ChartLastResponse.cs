using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
namespace Xtb.XApiClient.Model;

public sealed class ChartLastResponse : BaseResponse
{
    public ChartLastResponse()
        : base()
    { }

    public ChartLastResponse(string body) : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Digits = (int?)ob["digits"];
        var arr = ob["rateInfos"]?.AsArray();
        if (arr != null)
        {
            foreach (var e in arr.OfType<JsonObject>())
            {
                var record = new RateInfoRecord();
                record.FieldsFromJsonObject(e);
                RateInfoRecords.AddLast(record);
            }
        }
    }

    public int? Digits { get; init; }

    public LinkedList<RateInfoRecord> RateInfoRecords { get; init; } = [];
}