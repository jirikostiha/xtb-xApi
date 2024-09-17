namespace XApi;

public interface ITickRecord : ISymbol, ITick
{
    int? AskVolume { get; }

    int? BidVolume { get; }

    int? Level { get; }

    double? SpreadRaw { get; }

    double? SpreadTable { get; }
}