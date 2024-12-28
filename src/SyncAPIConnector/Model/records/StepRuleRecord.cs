using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Model;

public sealed record StepRuleRecord : IBaseResponseRecord
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
            JsonArray jsonarray = value["steps"]?.AsArray() ?? [];
            foreach (JsonObject jsonObj in jsonarray.OfType<JsonObject>())
            {
                StepRecord rec = new();
                rec.FieldsFromJsonObject(jsonObj);
                Steps.AddLast(rec);
            }
        }
    }
}