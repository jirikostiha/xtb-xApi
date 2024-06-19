using System.Collections.Generic;
using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class StepRulesResponse : BaseResponse
    {
        private LinkedList<StepRuleRecord> stepRulesRecords = (LinkedList<StepRuleRecord>)new LinkedList<StepRuleRecord>();

        public StepRulesResponse(string body)
            : base(body)
        {
            JsonArray stepRulesRecords = this.ReturnData.AsArray();
            foreach (JsonObject e in stepRulesRecords)
            {
                StepRuleRecord stepRulesRecord = new StepRuleRecord();
                stepRulesRecord.FieldsFromJsonObject(e);
                this.stepRulesRecords.AddLast(stepRulesRecord);
            }
        }

        public virtual LinkedList<StepRuleRecord> StepRulesRecords
        {
            get
            {
                return stepRulesRecords;
            }
        }
    }

}