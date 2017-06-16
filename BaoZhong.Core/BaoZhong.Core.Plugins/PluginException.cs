using System;

namespace BaoZhong.Core.Plugins
{
	public class PluginException : BaoZhongException
	{
		public PluginException()
		{
			Log.Info(this.Message, this);
		}

		public PluginException(string message) : base(message)
		{
			Log.Info(message, this);
		}

		public PluginException(string message, System.Exception inner) : base(message, inner)
		{
			Log.Info(message, inner);
		}
	}
}
