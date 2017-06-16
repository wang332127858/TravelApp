using System;

namespace BaoZhong.Core
{
	public class InstanceCreateException : BaoZhongException
	{
		public InstanceCreateException()
		{
		}

		public InstanceCreateException(string message) : base(message)
		{
		}

		public InstanceCreateException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
