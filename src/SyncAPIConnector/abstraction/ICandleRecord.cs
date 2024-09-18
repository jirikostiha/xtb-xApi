using System;

namespace Xtb.XApi;

public interface ICandleRecord : ICandle
{
    DateTimeOffset? StartTime { get; }

    double? Volume { get; }
}