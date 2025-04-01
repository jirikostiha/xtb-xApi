namespace Xtb.XApi.Client.Model;

/// <summary>
/// Represents an entity with position id and orders id.
/// </summary>
public interface IPosition : IHasOrderId, IHasOrder2Id, IHasPositionId
{
}

/// <summary>
/// Represents an entity with a position identifier.
/// </summary>
public interface IHasPositionId
{
    /// <summary>
    /// Order number common to both opened and closed transactions.
    /// </summary>
    long? PositionId { get; }
}

/// <summary>
/// Represents an entity with an order ID for an opened transaction.
/// </summary>
public interface IHasOrderId
{
    /// <summary>
    /// Order number for an opened transaction.
    /// </summary>
    long? OrderId { get; }
}

/// <summary>
/// Represents an entity with an order ID for a closed transaction.
/// </summary>
public interface IHasOrder2Id
{
    /// <summary>
    /// Order number for a closed transaction.
    /// </summary>
    long? Order2Id { get; }
}
