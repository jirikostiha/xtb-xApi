using System;

namespace xAPI
{
    public interface INewsRecord
    {
        string Body { get; }

        string Key { get; }

        long? Time { get; }

        DateTimeOffset? Time2 { get; }

        string Title { get; }
    }
}