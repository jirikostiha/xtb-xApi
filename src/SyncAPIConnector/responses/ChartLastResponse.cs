using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses;

public class ChartLastResponse : BaseResponse
{
    public ChartLastResponse()
        : base()
    { }

    public ChartLastResponse(string body) : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Digits = (long?)ob["digits"];
        var arr = ob["rateInfos"]?.AsArray();
        if (arr != null)
        {
            foreach (var e in arr.OfType<JsonObject>())
            {
                RateInfoRecord record = new RateInfoRecord();
                record.FieldsFromJsonObject(e);
                RateInfos.AddLast(record);
            }
        }
    }

    public long? Digits { get; init; }

    public LinkedList<RateInfoRecord> RateInfos { get; init; } = [];
}