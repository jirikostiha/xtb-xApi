using System;
using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public class BaseResponse
{
    public BaseResponse()
    { }

    public BaseResponse(string body)
    {
        JsonNode? ob;
        try
        {
            ob = JsonNode.Parse(body);
        }
        catch (ArgumentNullException ex)
        {
            throw new APIReplyParseException("Parsing json failed: body is null", ex);
        }
        catch (Exception ex)
        {
            throw new APIReplyParseException($"Parsing json failed. message:'{body.Substring(0, 250)}'", ex);
        }

        if (ob is null)
        {
            throw new APIReplyParseException($"Parsing json returned null object. message:'{body.Substring(0, 250)}'");
        }
        else
        {
            Status = (bool?)ob["status"];
            if (Status == true)
            {
                ReturnData = ob["returnData"];
            }
            else
            {
                var errCodeValue = ob["errorCode"]?.GetValue<string>();
                ErrCode = errCodeValue is not null ? new ERR_CODE(errCodeValue) : null;
                ErrorDescr = ob["errorDescr"]?.GetValue<string>() ?? null;
            }
            CustomTag = ob["customTag"]?.GetValue<string>() ?? null;

            if (Status is null)
            {
                throw new APIReplyParseException("Parsing json error. Status cannot be null.");
            }

            if ((bool)!Status)
            {
                // If status is false check if redirect exists in given response
                if (ob["redirect"] is null)
                {
                    if (ErrorDescr is null && ErrCode != null)
                        ErrorDescr = ERR_CODE.GetErrorDescription(ErrCode.StringValue);

                    throw new APIErrorResponseException(ErrCode!, ErrorDescr!, body);
                }
                else
                {
                    var redirect = ob["redirect"];
                    throw new APICommunicationException($"Redirection is not supported. redirect:{redirect?.ToString()}");
                }
            }
        }
    }

    public bool? Status { get; init; }

    public string? ErrorDescr { get; init; }

    public ERR_CODE? ErrCode { get; init; }

    public JsonNode? ReturnData { get; init; }

    public string? CustomTag { get; init; }

    public string ToJSONString()
    {
        var obj = new JsonObject
        {
            { "status", Status }
        };

        if (ReturnData is not null)
            obj["returnData"] = ReturnData.ToJsonString();

        if (ErrCode is not null)
            obj["errorCode"] = ErrCode.StringValue;

        if (ErrorDescr is not null)
            obj["errorDescr"] = ErrorDescr;

        if (CustomTag is not null)
            obj["customTag"] = CustomTag;

        return obj.ToString();
    }
}