using System;
using System.Diagnostics;
using System.Text.Json.Nodes;

namespace xAPI.Responses
{
    [DebuggerDisplay("status:{Status}, order:{Order}")]
    public class TradeTransactionResponse : BaseResponse
    {
        private long? order;

        public TradeTransactionResponse(string body) : base(body)
        {
            JsonObject ob = this.ReturnData.AsObject();
            this.order = (long?)ob["order"];
        }

        [Obsolete("Use Order instead")]
        public virtual long? RequestId
        {
            get { return Order; }
        }

        public long? Order
        {
            get { return order; }
        }
    }
}