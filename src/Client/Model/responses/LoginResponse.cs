using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class LoginResponse : BaseResponse
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
            throw new APICommunicationException($"Redirection is not supported. message:{redirectJSON.ToString()}");
        }
    }

    public string? StreamSessionId { get; init; }
}