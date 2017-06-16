using BaoZhong.Core.Strategies;
using System;

namespace BaoZhong.Core
{
	public interface ICache : IStrategy
	{
		int TimeOut
		{
			get;
			set;
		}

		object Get(string key);

		T Get<T>(string key);

		void Remove(string key);

		void Insert(string key, object data);

		void Insert<T>(string key, T data);

		void Insert(string key, object data, int cacheTime);

		void Insert<T>(string key, T data, int cacheTime);

		void Insert(string key, object data, DateTime cacheTime);

		void Insert<T>(string key, T data, DateTime cacheTime);

		void Send(string key, object data);

		bool Exists(string key);

		void RegisterSubscribe<T>(string key, Cache.DoSub dosub);

		void UnRegisterSubscrib(string key);
	}
}
