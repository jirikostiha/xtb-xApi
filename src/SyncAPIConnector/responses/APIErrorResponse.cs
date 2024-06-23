using System;

namespace xAPI.Responses
{
    using ERR_CODE = xAPI.Errors.ERR_CODE;

    public class APIErrorResponse : Exception
    {
        private ERR_CODE code;
        private string errDesc;
        private string msgBody;

        public APIErrorResponse(ERR_CODE code, string errDesc, string msgBody)
            : base()
        {
            this.code = code;
            this.errDesc = errDesc;
            this.msgBody = msgBody;
        }

        public override string Message
        {
            get
            {
                return $"ERR_CODE '{code.StringValue}' ERR_DESC '{errDesc}'\n{msgBody}";
            }
        }

        public virtual string MsgBody
        {
            get
            {
                return msgBody;
            }
        }

        public virtual ERR_CODE ErrorCode
        {
            get
            {
                return code;
            }
        }

        public virtual string ErrorDescr
        {
            get
            {
                return errDesc;
            }
        }

        public override string ToString()
        {
            return ErrorCode.StringValue + ": " + ErrorDescr;
        }
    }
}