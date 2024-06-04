using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{

    public class LoginResponse : BaseResponse
    {
        private string streamSessionId;
        private RedirectRecord redirectRecord;

        public LoginResponse(string body)
            : base(body)
        {
            JsonNode ob = JsonNode.Parse(body);
            this.streamSessionId = (string)ob["streamSessionId"];

            JsonObject redirectJSON = ob["redirect"]?.AsObject();

            if (redirectJSON != null)
            {
                this.redirectRecord = new RedirectRecord();
                this.redirectRecord.FieldsFromJsonObject(redirectJSON);
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