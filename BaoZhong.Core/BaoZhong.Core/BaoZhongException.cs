using System;

namespace BaoZhong.Core
{
	public class BaoZhongException : ApplicationException
    {
		public BaoZhongException()
		{
			Log.Info(this.Message, this);
		}

		public BaoZhongException(string message) : base(message)
		{
			Log.Info(message, this);
		}

		public BaoZhongException(string message, Exception inner) : base(message, inner)
		{
			Log.Info(message, inner);
		}
	}
}
