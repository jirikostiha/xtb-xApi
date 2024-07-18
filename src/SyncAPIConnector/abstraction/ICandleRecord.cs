using System;

namespace xAPI;

public interface ICandleRecord : ICandle
{
    DateTimeOffset? StartTime { get; }

    double? Volume { get; }
}