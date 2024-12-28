using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public sealed class StepRulesResponse : BaseResponse
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
        foreach (var e in arr.OfType<JsonObject>())
        {
            var record = new StepRuleRecord();
            record.FieldsFromJsonObject(e);
            StepRulesRecords.AddLast(record);
        }
    }

    public LinkedList<StepRuleRecord> StepRulesRecords { get; init; } = [];
}