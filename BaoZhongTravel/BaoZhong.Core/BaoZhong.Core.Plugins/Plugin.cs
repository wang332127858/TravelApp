using System;

namespace BaoZhong.Core.Plugins
{
	public class Plugin<T> : PluginBase where T : IPlugin
	{
		public T Biz
		{
			get;
			set;
		}
	}
}
