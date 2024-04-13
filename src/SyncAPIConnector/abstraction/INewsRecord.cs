namespace xAPI.Records
{
    public interface INewsRecord
    {
        string Body { get; }

        string Key { get; }

        long? Time { get; }

        string Title { get; }
    }
}