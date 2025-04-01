namespace Xtb.XApi.Client.Model;

public sealed class MarginTradeResponse : BaseResponse
{
    public MarginTradeResponse()
        : base()
    { }

    public MarginTradeResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Margin = (double?)ob["margin"];
    }

    public double? Margin { get; init; }
}