namespace Xtb.XApi.Model;

public sealed class TradeTransactionStatusResponse : BaseResponse
{
    public TradeTransactionStatusResponse()
        : base()
    { }

    public TradeTransactionStatusResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        TradeTransactionStatusRecord = new();
        TradeTransactionStatusRecord.FieldsFromJsonObject(ob);
    }

    public TradeTransactionStatusRecord? TradeTransactionStatusRecord { get; init; }
}