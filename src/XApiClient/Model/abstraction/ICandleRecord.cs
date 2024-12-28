using System;

namespace Xtb.XApiClient.Model;

public interface ICandleRecord : ICandle
{
    DateTimeOffset? StartTime { get; }

    double? Volume { get; }
}