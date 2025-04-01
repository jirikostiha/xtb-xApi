namespace Xtb.XApi.Client.Model;

/// <summary>
/// Represents a trade status record containing information about the trade request's status and associated details.
/// </summary>
public interface ITradeStatusRecord
{
    /// <summary>
    /// The custom comment provided by the customer for retrieval later.
    /// </summary>
    public string? CustomComment { get; }

    /// <summary>
    /// Message associated with the trade status. Can be null.
    /// </summary>
    public string? Message { get; }

    /// <summary>
    /// Unique order number.
    /// </summary>
    public long? OrderId { get; }

    /// <summary>
    /// Request status code.
    /// </summary>
    public REQUEST_STATUS? RequestStatus { get; }
}
