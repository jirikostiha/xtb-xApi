using System;

namespace xAPI;

public interface ICandleRecord : ICandle
{
    long? Ctm { get; }

    DateTimeOffset? StartDateTime { get; }

    double? Vol { get; }
}