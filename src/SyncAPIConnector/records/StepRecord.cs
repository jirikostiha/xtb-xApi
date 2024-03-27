using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Records
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class StepRecord : BaseResponseRecord
    {
        private double FromValue;
        private double Step;

        public StepRecord()
        {
        }

        public void FieldsFromJSONObject(JSONObject value)
        {
            this.FromValue = (double)value["fromValue"];
            this.Step = (double)value["step"];
        }
    }
}
