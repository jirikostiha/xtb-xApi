using System;

namespace xAPI.Errors
{
	public class APICommunicationException : Exception
	{
		public APICommunicationException()
		{
		}

		public APICommunicationException(string msg) : base(msg)
		{
		}
	}
}