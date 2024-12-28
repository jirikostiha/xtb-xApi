namespace Xtb.XApi.Model;

public sealed class ConfirmPricedResponse : BaseResponse
{
    public ConfirmPricedResponse()
        : base()
    { }

    public ConfirmPricedResponse(string body) : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        NewRequestId = (long?)ob["requestId"];
    }

    public long? NewRequestId { get; init; }
}