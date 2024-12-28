namespace Xtb.XApiClient.Model;

public interface ITickRecord : IHasSymbol, ITick
{
    int? AskVolume { get; }

    int? BidVolume { get; }

    int? Level { get; }

    double? SpreadRaw { get; }

    double? SpreadTable { get; }
}