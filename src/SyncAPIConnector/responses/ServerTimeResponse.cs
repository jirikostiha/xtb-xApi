namespace xAPI.Responses
{
    public class ServerTimeResponse : BaseResponse
    {
        public ServerTimeResponse()
            : base()
        { }

        public ServerTimeResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var ob = ReturnData.AsObject();
            Time = (long?)ob["time"];
            TimeString = (string?)ob["timeString"];
        }

        public long? Time { get; init; }

        public string? TimeString { get; init; }
    }
}