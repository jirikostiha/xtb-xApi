namespace Xtb.XApi.Client.Model;

public interface ITickRecord : IHasSymbol, ITick
{
    /// <summary>
    /// Number of available lots to buy at the given ask price, or null if not applicable.
    /// </summary>
    int? AskVolume { get; }

    /// <summary>
    /// Number of available lots to buy at the given bid price, or null if not applicable.
    /// </summary>
    int? BidVolume { get; }

    /// <summary>
    /// Price level.
    /// </summary>
    int? Level { get; }

    /// <summary>
    /// Raw spread, the difference between ask and bid prices.
    /// </summary>
    double? SpreadRaw { get; }

    /// <summary>
    /// Spread representation.
    /// </summary>
    double? SpreadTable { get; }
}