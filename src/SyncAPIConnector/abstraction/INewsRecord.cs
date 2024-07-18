using System;

namespace xAPI;

public interface INewsRecord
{
    string? Title { get; }

    string? Body { get; }

    string? Key { get; }

    DateTimeOffset? Time { get; }
}