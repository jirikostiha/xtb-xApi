using System;

namespace xAPI.Errors
{
	public class APICommandConstructionException : Exception
	{
		public APICommandConstructionException()
		{
		}

		public APICommandConstructionException(string msg) : base(msg)
		{
		}
	}
}