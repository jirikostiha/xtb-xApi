using System;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Records;

public sealed record StepRuleRecord : IBaseResponseRecord
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public StepRecord[] Steps { get; private set; } = [];

    public void FieldsFromJsonObject(JsonObject value)
    {
        Id = (int?)value["id"];
        Name = (string?)value["name"];

        if (!(value["steps"] is JsonArray jsonArray) || jsonArray.Count == 0)
        {
            return;
        }

        int count = jsonArray.Count;
        var records = new StepRecord[count];

        for (int i = 0; i < count; i++)
        {
            var jsonObj = jsonArray[i] as JsonObject;
            if (jsonObj != null)
            {
                var rec = new StepRecord();
                rec.FieldsFromJsonObject(jsonObj);
                records[i] = rec;
            }
        }

        Steps = records;
    }
}
