using System;

namespace xAPI.Responses
{
    using JSONAware = Newtonsoft.Json.Linq.JContainer;
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    using ERR_CODE = xAPI.Errors.ERR_CODE;
    using APIReplyParseException = xAPI.Errors.APIReplyParseException;
    using xAPI.Records;

    public class BaseResponse
    {
        private bool? status;
        private string errorDescr;
        private ERR_CODE errCode;
        private JSONAware returnData;
        private string customTag;

        public BaseResponse(string body)
        {
            JSONObject ob;
            try
            {
                ob = (JSONObject)JSONObject.Parse(body);

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
                this.status = (bool?)ob["status"];
                this.errCode = new ERR_CODE((string)ob["errorCode"]);
                this.errorDescr = (string)ob["errorDescr"];
                this.returnData = (JSONAware)ob["returnData"];
                this.customTag = (string)ob["customTag"];

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

        public virtual object ReturnData
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
            JSONObject obj = new JSONObject();
            obj.Add("status", status);

            if (returnData != null)
                obj.Add("returnData", returnData.ToString());

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