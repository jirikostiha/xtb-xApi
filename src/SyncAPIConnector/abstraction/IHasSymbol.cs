namespace Xtb.XApi;

/// <summary>
/// Represents an entity that has a symbol associated with it.
/// </summary>
public interface IHasSymbol
{
    /// <summary>
    /// The symbol associated with the entity.
    /// </summary>
    string? Symbol { get; }
}