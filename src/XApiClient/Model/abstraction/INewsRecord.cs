using System;

namespace Xtb.XApiClient.Model;

public interface INewsRecord
{
    string? Title { get; }

    string? Body { get; }

    string? Key { get; }

    DateTimeOffset? Time { get; }
}