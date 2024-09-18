using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using XApi.Records;

namespace XApi.Responses;

public class ChartRangeResponse : BaseResponse
{
    public ChartRangeResponse()
        : base()
    { }

    public ChartRangeResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Digits = (int?)ob["digits"];
        var arr = ob["rateInfos"]?.AsArray();
        if (arr != null)
        {
            foreach (JsonObject e in arr.OfType<JsonObject>())
            {
                RateInfoRecord record = new RateInfoRecord();
                record.FieldsFromJsonObject(e);
                RateInfos.AddLast(record);
            }
        }
    }

    public int? Digits { get; init; }

    public LinkedList<RateInfoRecord> RateInfos { get; init; } = [];
}