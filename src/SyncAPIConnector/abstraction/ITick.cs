using System;

namespace xAPI;

public interface ITick
{
    double? Ask { get; }

    double? Bid { get; }

    double? High { get; }

    double? Low { get; }

    DateTimeOffset? Time { get; }
}