using System;

namespace BaoZhong.Core.Plugins
{
	public interface IPlugin
	{
		string WorkDirectory
		{
			set;
		}

		void CheckCanEnable();
	}
}
