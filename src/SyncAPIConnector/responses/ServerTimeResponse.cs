using System;

namespace XApi.Responses;

public class ServerTimeResponse : BaseResponse
{
    public ServerTimeResponse()
        : base()
    { }

    public ServerTimeResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        var time = (long?)ob["time"];
        Time = time.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(time.Value) : null;
        TimeString = (string?)ob["timeString"];
    }

    public DateTimeOffset? Time { get; init; }

    public string? TimeString { get; init; }
}