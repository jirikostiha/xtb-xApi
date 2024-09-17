namespace XApi.Responses;

public class ConfirmRequotedResponse : BaseResponse
{
    public ConfirmRequotedResponse()
        : base()
    { }

    public ConfirmRequotedResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        NewRequestId = (long?)ob["requestId"];
    }

    public long? NewRequestId { get; init; }
}