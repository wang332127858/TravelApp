using BaoZhong.Core;
using BaoZhong.Core.Strategies;
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace BaoZhong.Strategy
{
    /// <summary>
    /// desc:缓存
    /// author:cgm
    /// date:2016/8/23
    /// </summary>
    public class AspNetCache : ICache, IStrategy
    {
        private System.Web.Caching.Cache cache;
        private static object cacheLocker = new object();
        private const int DEFAULT_TMEOUT = 600;
        private int _timeout = 600;
        public int TimeOut
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = ((value > 0) ? value : 600);
            }
        }
        public AspNetCache()
        {
            this.cache = HttpRuntime.Cache;
        }
        public object Get(string key)
        {
            return this.cache.Get(key);
        }
        public T Get<T>(string key)
        {
            return (T)((object)this.cache.Get(key));
        }
        public void Insert(string key, object value)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value);
            }
        }
        public void Insert(string key, object value, int cacheTime)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value, null, DateTime.Now.AddSeconds((double)cacheTime), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }
        public void Insert(string key, object value, DateTime cacheTime)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value, null, cacheTime, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }
        public void Remove(string key)
        {
            this.cache.Remove(key);
        }
        public void Clear()
        {
            IDictionaryEnumerator enumerator = this.cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this.cache.Remove(enumerator.Key.ToString());
            }
        }
        public void Send(string key, object data)
        {
        }
        public void Recieve<T>(string key, BaoZhong.Core.Cache.DoSub dosub)
        {
        }
        public void RegisterSubscribe<T>(string key, BaoZhong.Core.Cache.DoSub dosub)
        {
        }
        public void UnRegisterSubscrib(string key)
        {
        }
        public bool Exists(string key)
        {
            return this.cache.Get(key) != null;
        }
        public void Insert<T>(string key, T value)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value);
            }
        }
        public void Insert<T>(string key, T value, int cacheTime)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value, null, DateTime.Now.AddSeconds((double)cacheTime), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }
        public void Insert<T>(string key, T value, DateTime cacheTime)
        {
            object obj = AspNetCache.cacheLocker;
            lock (obj)
            {
                bool flag2 = this.cache.Get(key) != null;
                if (flag2)
                {
                    this.cache.Remove(key);
                }
                this.cache.Insert(key, value, null, cacheTime, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }
    }
}
