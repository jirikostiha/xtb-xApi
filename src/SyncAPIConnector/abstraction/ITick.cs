using System;

namespace Xtb.XApi;

public interface ITick
{
    /// <summary> Ask price. </summary>
    double? Ask { get; }

    /// <summary> Bid price. </summary>
    double? Bid { get; }

    /// <summary> High price. </summary>
    double? High { get; }

    /// <summary> Low price. </summary>
    double? Low { get; }

    /// <summary> Tick time. </summary>
    DateTimeOffset? Time { get; }
}