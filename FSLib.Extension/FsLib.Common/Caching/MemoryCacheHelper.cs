#if !NETSTANDARD2_0
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace System.Caching
{
    /// <summary>
    /// <see cref="MemoryCache"/> 帮助类
    /// </summary>
    public class MemoryCacheHelper
    {
        private static readonly Object _locker = new object();

        /// <summary>
        /// 创建一个缓存的键值，并指定响应的时间范围，如果失效，则自动获取对应的值
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">对象的键</param>
        /// <param name="cachePopulate">获取缓存值的操作</param>
        /// <param name="slidingExpiration">失效的时间范围</param>
        /// <param name="absoluteExpiration">失效的绝对时间</param>
        /// <returns></returns>
        public static T GetCacheItem<T>(String key, Func<T> cachePopulate, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (cachePopulate == null) throw new ArgumentNullException(nameof(cachePopulate));
            if (slidingExpiration == null && absoluteExpiration == null) throw new ArgumentException("Either a sliding expiration or absolute must be provided");

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    if (MemoryCache.Default[key] == null)
                    {
                        var item = new CacheItem(key, cachePopulate());
                        var policy = CreatePolicy(slidingExpiration, absoluteExpiration);

                        MemoryCache.Default.Add(item, policy);
                    }
                }
            }

            return (T)MemoryCache.Default[key];
        }
        /// <summary>
        /// 设置一个缓存的键值，并指定响应的时间范围
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">对象的键</param>
        /// <param name="cachePopulate">获取缓存值的操作</param>
        /// <param name="slidingExpiration">失效的时间范围</param>
        /// <param name="absoluteExpiration">失效的绝对时间</param>
        /// <returns></returns>
        public static void SetCacheItem<T>(String key, Func<T> cachePopulate, TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid cache key");
            if (cachePopulate == null) throw new ArgumentNullException(nameof(cachePopulate));
            if (slidingExpiration == null && absoluteExpiration == null) throw new ArgumentException("Either a sliding expiration or absolute must be provided");

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    var item = new CacheItem(key, cachePopulate());
                    var policy = CreatePolicy(slidingExpiration, absoluteExpiration);
                    if (MemoryCache.Default[key] == null)
                    {
                        MemoryCache.Default.Add(item, policy);
                    }
                    else
                    {
                        MemoryCache.Default.Set(item, policy);
                    }
                }
            }
        }

        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearCache()
        {
            List<string> cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                MemoryCache.Default.Remove(cacheKey);
            }
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cacheKey">对象的键</param>
        public static void RemoveCache(string cacheKey)
        {
            MemoryCache.Default.Remove(cacheKey);
        }
    }
}
#endif
