using System.Text.Json.Nodes;

namespace xAPI.Records
{

    public record StepRecord : BaseResponseRecord
    {
        private double FromValue;
        private double Step;

        public StepRecord()
        {
        }

        public void FieldsFromJsonObject(JsonObject value)
        {
            this.FromValue = (double)value["fromValue"];
            this.Step = (double)value["step"];
        }
    }
}
