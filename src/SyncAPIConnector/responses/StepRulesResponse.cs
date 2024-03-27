using System.Collections;
using System.Collections.Generic;
using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

	public class StepRulesResponse : BaseResponse
	{
		private LinkedList<StepRuleRecord> stepRulesRecords = (LinkedList<StepRuleRecord>)new LinkedList<StepRuleRecord>();

        public StepRulesResponse(string body)
            : base(body)
		{
            JSONArray stepRulesRecords = (JSONArray)this.ReturnData;
            foreach (JSONObject e in stepRulesRecords)
            {
                StepRuleRecord stepRulesRecord = new StepRuleRecord();
                stepRulesRecord.FieldsFromJSONObject(e);
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