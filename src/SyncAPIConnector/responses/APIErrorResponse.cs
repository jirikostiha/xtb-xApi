using System;

namespace xAPI.Responses
{
	using ERR_CODE = xAPI.Errors.ERR_CODE;

	public class APIErrorResponse : Exception
	{
		private ERR_CODE code;
		private string errDesc;
		private string msg;

		public APIErrorResponse(ERR_CODE code, string errDesc, string msg) : base(msg)
		{
			this.code = code;
			this.errDesc = errDesc;
			this.msg = msg;
		}

		public override string Message
		{
			get
			{
				return "ERR_CODE = " + code.StringValue + " ERR_DESC = " + errDesc + "\n" + msg + "\n" + base.Message;
			}
		}

        public virtual string Msg
        {
            get
            {
                return msg;
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