using System.Text.Json.Nodes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class LoginResponse : BaseResponse
    {
        public LoginResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var ob = ReturnData.AsObject();
            StreamSessionId = (string?)ob["streamSessionId"];

            if (ob["redirect"] is JsonObject redirectJSON)
            {
                RedirectRecord = new RedirectRecord();
                RedirectRecord.FieldsFromJsonObject(redirectJSON);
            }
        }

        public string? StreamSessionId { get; init; }

        public RedirectRecord? RedirectRecord { get; init; }
    }
}