using Xtb.XApi.Codes;

namespace Xtb.XApi.Records;

public interface ITradeStatusRecord
{
    public string? CustomComment { get; }

    public string? Message { get; }

    public long? OrderId { get; }

    public REQUEST_STATUS? RequestStatus { get; }
}