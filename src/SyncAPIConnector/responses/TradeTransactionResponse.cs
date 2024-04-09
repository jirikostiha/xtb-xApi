namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("status:{Status}, order:{Order}")]
    public class TradeTransactionResponse : BaseResponse
    {
        private long? order;

        public TradeTransactionResponse(string body) : base(body)
        {
            JSONObject ob = (JSONObject)this.ReturnData;
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