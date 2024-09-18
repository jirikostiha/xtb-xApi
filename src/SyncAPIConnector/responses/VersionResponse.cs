namespace Xtb.XApi.Responses;

public class VersionResponse : BaseResponse
{
    public VersionResponse()
        : base()
    { }

    public VersionResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var returnData = ReturnData.AsObject();
        Version = (string?)returnData["version"];
    }

    public string? Version { get; init; }
}