using System;

namespace BaoZhong.Core
{
    /// <summary>
    /// desc:缓存类
    /// author:cgm
    /// </summary>
	public static class Cache
	{
		public delegate void DoSub(object d);

		private static object cacheLocker;

		private static ICache cache;

		public static int TimeOut
		{
			get
			{
				return Cache.cache.TimeOut;
			}
			set
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.TimeOut = value;
				}
			}
		}

		static Cache()
		{
			Cache.cacheLocker = new object();
			Cache.cache = null;
			Cache.Load();
		}

		private static void Load()
		{
			try
			{
				Cache.cache = ObjectContainer.Current.Resolve<ICache>();
			}
			catch (Exception inner)
			{
				throw new CacheRegisterException("注册缓存服务异常", inner);
			}
		}

		public static ICache GetCache()
		{
			return Cache.cache;
		}

		public static object Get(string key)
		{
			object result;
			if (string.IsNullOrWhiteSpace(key))
			{
				result = null;
			}
			else
			{
				result = Cache.cache.Get(key);
			}
			return result;
		}

		public static T Get<T>(string key)
		{
			return Cache.cache.Get<T>(key);
		}

		public static void Insert(string key, object data)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert(key, data);
			}
		}

		public static void Insert<T>(string key, T data)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert<T>(key, data);
			}
		}

		public static void Insert(string key, object data, int cacheTime)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert(key, data, cacheTime);
			}
		}

		public static void Insert<T>(string key, T data, int cacheTime)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert<T>(key, data, cacheTime);
			}
		}

		public static void Insert(string key, object data, DateTime cacheTime)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert(key, data, cacheTime);
			}
		}

		public static void Insert<T>(string key, T data, DateTime cacheTime)
		{
			if (!string.IsNullOrWhiteSpace(key) && data != null)
			{
				Cache.cache.Insert<T>(key, data, cacheTime);
			}
		}

		public static void Remove(string key)
		{
			if (!string.IsNullOrWhiteSpace(key))
			{
				lock (Cache.cacheLocker)
				{
					Cache.cache.Remove(key);
				}
			}
		}

		public static bool Exists(string key)
		{
			return Cache.cache.Exists(key);
		}

		public static void Send(string key, object data)
		{
			Cache.cache.Send(key, data);
		}

		public static void RegisterSubscribe<T>(string key, Cache.DoSub dosub)
		{
			Cache.cache.RegisterSubscribe<T>(key, dosub);
		}

		public static void UnRegisterSubscrib(string key)
		{
			Cache.cache.UnRegisterSubscrib(key);
		}
	}
}
