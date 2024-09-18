using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public class StepRulesResponse : BaseResponse
{
    public StepRulesResponse()
        : base()
    { }

    public StepRulesResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var arr = ReturnData.AsArray();
        foreach (JsonObject e in arr.OfType<JsonObject>())
        {
            var record = new StepRuleRecord();
            record.FieldsFromJsonObject(e);
            StepRulesRecords.AddLast(record);
        }
    }

    public LinkedList<StepRuleRecord> StepRulesRecords { get; init; } = [];
}