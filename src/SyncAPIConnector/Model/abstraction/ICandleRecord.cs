using System;

namespace Xtb.XApi.Model;

public interface ICandleRecord : ICandle
{
    DateTimeOffset? StartTime { get; }

    double? Volume { get; }
}