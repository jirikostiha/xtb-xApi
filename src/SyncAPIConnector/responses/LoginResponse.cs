using System.Text.Json.Nodes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

public class LoginResponse : BaseResponse
{
    public LoginResponse(string body)
        : base(body)
    {
        var ob = JsonNode.Parse(body);
        if (ob is null)
            return;

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