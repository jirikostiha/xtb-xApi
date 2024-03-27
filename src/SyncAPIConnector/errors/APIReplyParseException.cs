using System;

namespace xAPI.Errors
{
	public class APIReplyParseException : Exception
	{
		public APIReplyParseException()
		{
		}

		public APIReplyParseException(string msg) : base(msg)
		{
		}
	}
}