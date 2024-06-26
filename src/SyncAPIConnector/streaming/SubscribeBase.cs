namespace xAPI.Streaming
{
    internal abstract class SubscribeBase
    {
        protected SubscribeBase(string streamSessionId) => StreamSessionId = streamSessionId;

        public string StreamSessionId { get; set; }
    }
}
