namespace Xtb.XApiClient.Model;

public interface IPosition : IHasOrderId, IHasOrder2Id, IHasPositionId
{
}

public interface IHasPositionId
{
    long? PositionId { get; }
}

public interface IHasOrderId
{
    long? OrderId { get; }
}

public interface IHasOrder2Id
{
    long? Order2Id { get; }
}