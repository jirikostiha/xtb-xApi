using System;
using ERR_CODE = xAPI.Errors.ERR_CODE;
using APIReplyParseException = xAPI.Errors.APIReplyParseException;
using System.Text.Json.Nodes;

namespace xAPI.Responses
{
    public class BaseResponse
    {
        private bool? status;
        private string? errorDescr;
        private ERR_CODE errCode;
        private JsonNode? returnData;
        private string? customTag;

        public BaseResponse(string body)
        {
            JsonNode? ob;
            try
            {
                ob = JsonNode.Parse(body);
            }
            catch (ArgumentNullException)
            {
                throw new APIReplyParseException("JSON Parse exception: body is null");
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
                this.status = (bool?)ob["status"];
                if (status == true)
                {
                    returnData = ob["returnData"];
                }
                else
                {
                    errCode = new ERR_CODE(ob["errorCode"]?.GetValue<string>());
                    errorDescr = ob["errorDescr"]?.GetValue<string>();
                }
                this.customTag = ob["customTag"]?.GetValue<string>();

                if (this.status is null)
                {
                    throw new APIReplyParseException("Parsing json error. Status cannot be null.");
                }

                if ((this.status is null) || ((bool)!this.status))
                {
                    // If status is false check if redirect exists in given response
                    if (ob["redirect"] is null)
                    {
                        if (this.errorDescr is null)
                            this.errorDescr = ERR_CODE.getErrorDescription(this.errCode.StringValue);
                        throw new APIErrorResponse(errCode, errorDescr, body);
                    }
                }
            }
        }

        public virtual JsonNode ReturnData
        {
            get
            {
                return returnData;
            }
        }

        public virtual bool? Status
        {
            get
            {
                return status;
            }
        }

        public virtual string ErrorDescr
        {
            get
            {
                return errorDescr;
            }
        }

        public virtual string ErrorCode
        {
            get
            {
                return errCode.StringValue;
            }
        }

        public string CustomTag
        {
            get
            {
                return customTag;
            }
        }

        public string ToJSONString()
        {
            JsonObject obj = new JsonObject();
            obj.Add("status", status);

            if (returnData is not null)
                obj.Add("returnData", returnData.ToJsonString());

            if (errCode is not null)
                obj.Add("errorCode", errCode.StringValue);

            if (errorDescr is not null)
                obj.Add("errorDescr", errorDescr);

            if (customTag is not null)
                obj.Add("customTag", customTag);

            return obj.ToString();
        }
    }
}