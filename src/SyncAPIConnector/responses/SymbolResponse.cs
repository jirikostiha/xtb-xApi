using xAPI.Records;

namespace xAPI.Responses;

public class SymbolResponse : BaseResponse
{
    public SymbolResponse()
        : base()
    { }

    public SymbolResponse(string body)
        : base(body)
    {
        if (ReturnData is null)
            return;

        var ob = ReturnData.AsObject();
        Symbol = new SymbolRecord();
        Symbol.FieldsFromJsonObject(ob);
    }

    public SymbolRecord? Symbol { get; init; }
}