using System.Text.Json.Nodes;

namespace xAPI.Responses
{
    public class VersionResponse : BaseResponse
    {
        private string version;

        public VersionResponse(string body)
            : base(body)
        {
            JsonObject returnData = (JsonObject)this.ReturnData;
            this.version = (string)returnData["version"];
        }

        public virtual string Version
        {
            get { return version; }
        }
    }
}