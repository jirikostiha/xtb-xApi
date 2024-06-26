using System;

namespace xAPI.Responses
{
    public class PingResponse : BaseResponse
    {
        public PingResponse()
            : base()
        { }

        public PingResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var ob = ReturnData.AsObject();

            DateTime = (DateTimeOffset?)ob["todo"];
        }

        public DateTimeOffset? DateTime { get; init; }
    }
}