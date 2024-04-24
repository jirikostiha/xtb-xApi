using System;

namespace xAPI
{
    public interface ITickRecord
    {
        double? Ask { get; }
        long? AskVolume { get; }
        double? Bid { get; }
        long? BidVolume { get; }
        double? High { get; }
        long? Level { get; }
        double? Low { get; }
        double? SpreadRaw { get; }
        double? SpreadTable { get; }
        string Symbol { get; }
        long? Timestamp { get; }
        DateTimeOffset? Timestamp2 { get; }

        string ToString();
    }
}