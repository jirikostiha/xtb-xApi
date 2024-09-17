namespace XApi;

public interface ICandle
{
    double? High { get; }

    double? Low { get; }

    double? Open { get; }

    double? Close { get; }
}