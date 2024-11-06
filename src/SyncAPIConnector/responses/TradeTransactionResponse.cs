using System.Diagnostics;

namespace Xtb.XApi.Responses;

[DebuggerDisplay("orderId:{OrderId}")]
public class TradeTransactionResponse : BaseResponse
{
    public TradeTransactionResponse()
        : base()
    { }

    public TradeTransactionResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        OrderId = (long?)ob["order"];
    }

    public long? OrderId { get; init; }
}