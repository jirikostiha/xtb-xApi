namespace Xtb.XApiClient.Model;

public interface ICandle
{
    double? High { get; }

    double? Low { get; }

    double? Open { get; }

    double? Close { get; }

#if NETSTANDARD2_1_OR_GREATER
    public double? Size => High - Low;

    public double? Average => (High + Low) / 2;
#endif
}