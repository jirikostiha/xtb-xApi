using System;

namespace XApi;

public interface ICandleRecord : ICandle
{
    DateTimeOffset? StartTime { get; }

    double? Volume { get; }
}