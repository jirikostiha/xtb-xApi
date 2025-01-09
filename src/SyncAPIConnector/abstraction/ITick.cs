using System;

namespace Xtb.XApi;

public interface ITick
{
    /// <summary> Ask price in symbol currency. </summary>
    double? Ask { get; }

    /// <summary> Bid price in symbol currency. </summary>
    double? Bid { get; }

    /// <summary> High price in symbol currency. </summary>
    double? High { get; }

    /// <summary> Low price in symbol currency. </summary>
    double? Low { get; }

    /// <summary> Tick record time. </summary>
    DateTimeOffset? Time { get; }
}