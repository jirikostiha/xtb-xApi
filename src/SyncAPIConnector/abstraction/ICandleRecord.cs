using System;

namespace Xtb.XApi;

/// <summary>
/// Represents a candle record containing the start time and trading volume.
/// Inherits from <see cref="ICandle"/>.
/// </summary>
public interface ICandleRecord : ICandle
{
    /// <summary>
    /// The start time of the candle record.
    /// </summary>
    DateTimeOffset? StartTime { get; }

    /// <summary>
    /// The trading volume for the candle record.
    /// </summary>
    double? Volume { get; }
}
