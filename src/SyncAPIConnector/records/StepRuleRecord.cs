using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Linq;

namespace Xtb.XApi.Records;

public record StepRuleRecord : IBaseResponseRecord
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public LinkedList<StepRecord> Steps { get; set; } = [];

    public void FieldsFromJsonObject(JsonObject value)
    {
        Id = (int?)value["id"];
        Name = (string?)value["name"];

        Steps = new LinkedList<StepRecord>();
        if (value["steps"] != null)
        {
            JsonArray jsonarray = value["steps"].AsArray();
            foreach (JsonObject i in jsonarray.OfType<JsonObject>())
            {
                StepRecord rec = new StepRecord();
                rec.FieldsFromJsonObject(i);
                Steps.AddLast(rec);
            }
        }
    }
}