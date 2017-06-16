using System;
using System.Collections.Generic;

namespace BaoZhong.Core
{
	internal class DefaultContainerForDictionary : IinjectContainer
	{
		private static IDictionary<Type, object> objectDefine = new Dictionary<Type, object>();

		public void RegisterType<T>()
		{
			if (!DefaultContainerForDictionary.objectDefine.ContainsKey(typeof(T)))
			{
				DefaultContainerForDictionary.objectDefine[typeof(T)] = Activator.CreateInstance(typeof(T));
			}
		}

		public T Resolve<T>()
		{
			if (DefaultContainerForDictionary.objectDefine.ContainsKey(typeof(T)))
			{
				return (T)((object)DefaultContainerForDictionary.objectDefine[typeof(T)]);
			}
			throw new Exception("该服务未在框架中注册");
		}
	}
}
