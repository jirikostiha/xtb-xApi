namespace Xtb.XApi.Client.Model;

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