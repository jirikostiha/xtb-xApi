namespace Xtb.XApi.Client.Model;

public sealed class CommissionDefResponse : BaseResponse
{
    public CommissionDefResponse()
        : base()
    { }

    public CommissionDefResponse(string body) : base(body)
    {
        if (ReturnData is null)
            return;

        var rd = ReturnData.AsObject();
        Commission = (double?)rd["commission"];
        RateOfExchange = (double?)rd["rateOfExchange"];
    }

    public double? Commission { get; init; }

    public double? RateOfExchange { get; init; }
}