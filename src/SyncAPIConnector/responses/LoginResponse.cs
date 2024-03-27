using xAPI.Records;

namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class LoginResponse : BaseResponse
    {
        private string streamSessionId;
        private RedirectRecord redirectRecord;

        public LoginResponse(string body)
            : base(body)
        {
            JSONObject ob = (JSONObject)JSONObject.Parse(body);
            this.streamSessionId = (string)ob["streamSessionId"];

            JSONObject redirectJSON = (JSONObject)ob["redirect"];

            if (redirectJSON != null)
            {
                this.redirectRecord = new RedirectRecord();
                this.redirectRecord.FieldsFromJSONObject(redirectJSON);
            }
        }

        public virtual string StreamSessionId
        {
            get { return streamSessionId; }
        }

        public virtual RedirectRecord RedirectRecord
        {
            get { return redirectRecord; }
        }
    }
}