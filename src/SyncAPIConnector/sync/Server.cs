namespace xAPI.Sync
{
    public record Server
    {
        public Server(string address, int mainPort, int streamingPort, bool secure, string description)
        {
            Address = address;
            MainPort = mainPort;
            StreamingPort = streamingPort;
            IsSecure = secure;
            Description = description;
        }

        public string Address { get; set; }

        public string Description { get; set; }

        public int MainPort { get; set; }

        public int StreamingPort { get; set; }

        public bool IsSecure { get; set; }

        public override string ToString()
        {
            return Description + " (" + Address + ":" + MainPort + "/" + StreamingPort + ")";
        }
    }
}