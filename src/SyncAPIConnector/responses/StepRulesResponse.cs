using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public sealed class StepRulesResponse : BaseResponse
{
    public StepRulesResponse() : base() { }

    public StepRulesResponse(string body) : base(body)
    {
        if (ReturnData is not JsonArray arr || arr.Count == 0)
        {
            return;
        }

        int count = arr.Count;
        var records = new StepRuleRecord[count];

        for (int i = 0; i < count; i++)
        {
            if (arr[i] is JsonObject jsonObj)
            {
                var record = new StepRuleRecord();
                record.FieldsFromJsonObject(jsonObj);
                records[i] = record;
            }
        }

        StepRulesRecords = records;
    }

    public StepRuleRecord[] StepRulesRecords { get; init; } = [];
}
