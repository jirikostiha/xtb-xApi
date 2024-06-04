using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace xAPI.Records
{
    public record StepRuleRecord : BaseResponseRecord
    {
        private int Id { get; set; }
        private string Name { get; set; }
        private LinkedList<StepRecord> Steps { get; set; }

        public StepRuleRecord()
        {
        }

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.Id = (int)value["id"];
            this.Name = (string)value["name"];

            this.Steps = new LinkedList<StepRecord>();
            if (value["steps"] != null)
            {
                JsonArray jsonarray = value["steps"].AsArray();
                foreach (JsonObject i in jsonarray)
                {
                    StepRecord rec = new StepRecord();
                    rec.FieldsFromJsonObject(i);
                    this.Steps.AddLast(rec);
                }
            }
        }
    }
}