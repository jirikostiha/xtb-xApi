using System;

namespace Xtb.XApiClient;

public class APIErrorResponseException : Exception
{
    public APIErrorResponseException(ERR_CODE code, string errDesc, string msgBody)
        : base(msgBody)
    {
        ErrorCode = code;
        ErrorDescription = errDesc;
    }

    public ERR_CODE ErrorCode { get; init; }

    public string ErrorDescription { get; init; }

    public override string Message => $"{ErrorCode.StringValue} - {ErrorDescription}\n{base.Message}";

    public override string ToString() => $"{ErrorCode.StringValue}: {ErrorDescription}";
}