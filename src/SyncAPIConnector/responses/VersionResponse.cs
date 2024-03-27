namespace xAPI.Responses
{
    using JSONArray = Newtonsoft.Json.Linq.JArray;
    using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class VersionResponse : BaseResponse
    {
        private string version;

        public VersionResponse(string body)
            : base(body)
        {
            JSONObject returnData = (JSONObject)this.ReturnData;
            this.version = (string)returnData["version"];
        }

        public virtual string Version
        {
            get { return version; }
        }
    }
}