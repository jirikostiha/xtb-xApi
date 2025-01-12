namespace Xtb.XApi;

/// <summary>
/// Represents a candle with high, low, open, and close prices in symbol currency.
/// </summary>
public interface ICandle
{
    /// <summary> High price in symbol currency. </summary>
    double? High { get; }

    /// <summary> Low price in symbol currency. </summary>
    double? Low { get; }

    /// <summary> Open price in symbol currency. </summary>
    double? Open { get; }

    /// <summary> Close price in symbol currency. </summary>
    double? Close { get; }

#if NETSTANDARD2_1_OR_GREATER
    public double? Size => High - Low;

    public double? Average => (High + Low) / 2;
#endif
}