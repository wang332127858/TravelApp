using System;

namespace BaoZhong.Core
{
	public class Instance
	{
		public static T Get<T>(string classFullName)
		{
			T result;
			try
			{
				Type type = Type.GetType(classFullName);
				result = (T)((object)Activator.CreateInstance(type));
			}
			catch (Exception inner)
			{
				throw new InstanceCreateException("创建实例异常", inner);
			}
			return result;
		}
	}
}
