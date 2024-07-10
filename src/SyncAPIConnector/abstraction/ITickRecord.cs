namespace xAPI
{
    public interface ITickRecord : ISymbol, ITick
    {
        long? AskVolume { get; }

        long? BidVolume { get; }

        long? Level { get; }

        double? SpreadRaw { get; }

        double? SpreadTable { get; }
    }
}