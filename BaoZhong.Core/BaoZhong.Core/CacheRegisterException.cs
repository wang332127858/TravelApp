using System;

namespace BaoZhong.Core
{
	public class CacheRegisterException : BaoZhongException
	{
		public CacheRegisterException()
		{
		}

		public CacheRegisterException(string message) : base(message)
		{
		}

		public CacheRegisterException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
