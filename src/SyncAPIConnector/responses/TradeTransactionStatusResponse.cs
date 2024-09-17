using System.Diagnostics;
using xAPI.Codes;

namespace XApi.Responses;

[DebuggerDisplay("order:{Order}")]
public class TradeTransactionStatusResponse : BaseResponse
{
    public TradeTransactionStatusResponse()
        : base()
    { }

    public TradeTransactionStatusResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Ask = (double?)ob["ask"];
        Bid = (double?)ob["bid"];
        CustomComment = (string?)ob["customComment"];
        Message = (string?)ob["message"];
        Order = (long?)ob["order"];

        var requestStatusCode = (int?)ob["requestStatus"];
        RequestStatus = requestStatusCode.HasValue ? new REQUEST_STATUS(requestStatusCode.Value) : null;
    }

    public double? Ask { get; init; }

    public double? Bid { get; init; }

    public string? CustomComment { get; init; }

    public string? Message { get; init; }

    public long? Order { get; init; }

    public REQUEST_STATUS? RequestStatus { get; init; }
}