using System;

namespace Xtb.XApi.Model;

public interface INewsRecord
{
    string? Title { get; }

    string? Body { get; }

    string? Key { get; }

    DateTimeOffset? Time { get; }
}