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
                if (body is null)
                {
                    throw new APIReplyParseException("JSON Parse exception: body is null");
                }
                ob = JsonNode.Parse(body);

            }
            catch (Exception x)
            {
                throw new APIReplyParseException("JSON Parse exception: " + body + "\n" + x.Message);
            }

            if (ob == null)
            {
                throw new APIReplyParseException("JSON Parse exception: " + body);
            }
            else
            {
                if (true)
                {
                    Console.WriteLine(ob.ToString());
                }
                this.status = (bool?)ob["status"];
                this.errCode = new ERR_CODE((string)ob["errorCode"]);
                this.errorDescr = ob["errorDescr"]?.GetValue<string>();
                this.returnData = ob["returnData"];
                this.customTag = ob["customTag"]?.GetValue<string>();

                if (this.status == null)
                {
                    Console.Error.WriteLine(body);
                    throw new APIReplyParseException("JSON Parse error: " + "\"status\" is null!");
                }

                if ((this.status == null) || ((bool)!this.status))
                {
                    // If status is false check if redirect exists in given response
                    if (ob["redirect"] == null)
                    {
                        if (this.errorDescr == null)
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

            if (returnData != null)
                obj.Add("returnData", returnData.ToJsonString());

            if (errCode != null)
                obj.Add("errorCode", errCode.StringValue);

            if (errorDescr != null)
                obj.Add("errorDescr", errorDescr);

            if (customTag != null)
                obj.Add("customTag", customTag);

            return obj.ToString();
        }
    }
}