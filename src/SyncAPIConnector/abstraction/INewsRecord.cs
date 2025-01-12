using System;

namespace Xtb.XApi;

public interface INewsRecord
{
    /// <summary>
    /// Gets the title of the news record.
    /// </summary>
    string? Title { get; }

    /// <summary>
    /// Gets the body content of the news record.
    /// </summary>
    string? Body { get; }

    /// <summary>
    /// Gets the unique key associated with the news record.
    /// </summary>
    string? Key { get; }

    /// <summary>
    /// Gets the timestamp of when the news record was created or last updated.
    /// </summary>
    DateTimeOffset? Time { get; }
}