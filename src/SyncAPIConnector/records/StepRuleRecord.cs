namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using System;
    using xAPI.Codes;
    using System.Collections.Generic;

	public class StepRuleRecord : BaseResponseRecord
	{
		private int Id { get; set; }
		private string Name { get; set; }
        private LinkedList<StepRecord> Steps { get; set; }

        public StepRuleRecord()
		{
		}

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.Id = (int)value["id"];
            this.Name = (string)value["name"];

            this.Steps = new LinkedList<StepRecord>();
            if (value["steps"] != null)
            {
                JSONArray jsonarray = (JSONArray)value["steps"];
                foreach (JSONObject i in jsonarray)
                {
                    StepRecord rec = new StepRecord();
                    rec.FieldsFromJSONObject(i);
                    this.Steps.AddLast(rec);
                }
            }
        }
    }
}