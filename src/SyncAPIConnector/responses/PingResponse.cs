namespace Xtb.XApi.Responses;

public sealed class PingResponse : BaseResponse
{
    public PingResponse()
        : base()
    { }

    public PingResponse(string body)
        : base(body)
    {
    }
}