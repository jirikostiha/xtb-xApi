using System;
using System.Diagnostics;

namespace xAPI.Responses
{
    [DebuggerDisplay("order:{Order}")]
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
            Order = (long?)ob["order"];
        }

        public long? Order { get; init; }
    }
}