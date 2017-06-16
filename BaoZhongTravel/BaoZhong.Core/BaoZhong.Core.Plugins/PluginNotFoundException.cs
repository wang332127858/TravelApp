using System;

namespace BaoZhong.Core.Plugins
{
	public class PluginNotFoundException : PluginException
	{
		public PluginNotFoundException(string pluginId) : base("未找到插件" + pluginId)
		{
		}
	}
}
