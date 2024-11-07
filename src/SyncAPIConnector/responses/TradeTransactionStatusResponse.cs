using System.Diagnostics;
using Xtb.XApi.Codes;
using Xtb.XApi.Records;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("orderId:{OrderId}")]
public class TradeTransactionStatusResponse : BaseResponse, ITradeStatusRecord
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
        OrderId = (long?)ob["order"];

        var requestStatusCode = (int?)ob["requestStatus"];
        RequestStatus = requestStatusCode.HasValue ? new REQUEST_STATUS(requestStatusCode.Value) : null;
    }

    public double? Ask { get; init; }

    public double? Bid { get; init; }

    public string? CustomComment { get; init; }

    public string? Message { get; init; }

    public long? OrderId { get; init; }

    public REQUEST_STATUS? RequestStatus { get; init; }
}