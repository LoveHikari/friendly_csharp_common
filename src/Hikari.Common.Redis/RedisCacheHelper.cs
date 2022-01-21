using ServiceStack.Redis;

namespace Hikari.Common.Redis
{
    /// <summary>
    /// Redis帮助类
    /// </summary>
    public class RedisCacheHelper
    {
        private readonly PooledRedisClientManager? _pool;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisServerSession">连接字符串，例password@ip:port</param>
        /// <param name="maxWritePoolSize">最大写入池大小</param>
        /// <param name="maxReadPoolSize">最大读取池大小</param>
        public RedisCacheHelper(string redisServerSession, int maxWritePoolSize = 3, int maxReadPoolSize = 1)
        {
            var redisHostStr = redisServerSession;

            if (string.IsNullOrWhiteSpace(redisHostStr)) return;
            var redisHosts = redisHostStr.Split(',');

            if (redisHosts.Length > 0)
            {
                _pool = new PooledRedisClientManager(redisHosts, redisHosts,
                    new RedisClientManagerConfig()
                    {
                        MaxWritePoolSize = maxWritePoolSize,
                        MaxReadPoolSize = maxReadPoolSize,
                        AutoStart = true
                    });
            }
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">过期时间</param>
        public void Set<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                Remove(key);

                return;
            }

            using var r = _pool?.GetClient();
            if (r is null) return;
            r.SendTimeout = 1000;
            r.Set(key, value, expiry - DateTime.Now);

        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration">过期时间</param>
        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);

                return;
            }

            using var r = _pool?.GetClient();
            if (r is null) return;
            r.SendTimeout = 1000;
            r.Set(key, value, slidingExpiration);

        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set<T>(string key, T value)
        {
            if (value == null)
            {
                return;
            }

            using var r = _pool?.GetClient();
            if (r == null) return;
            r.SendTimeout = 1000;
            r.Set(key, value);


        }
        /// <summary>
        /// 获得数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T? Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default;
            }

            T? obj = default;

            using var r = _pool?.GetClient();
            if (r is null) return obj;
            r.SendTimeout = 1000;
            obj = r.Get<T>(key);

            return obj;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            using var r = _pool?.GetClient();
            if (r == null) return;
            r.SendTimeout = 1000;
            r.Remove(key);
        }
        /// <summary>
        /// 键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            using var r = _pool?.GetClient();
            if (r is null) return false;
            r.SendTimeout = 1000;
            return r.ContainsKey(key);
        }
        /// <summary>
        /// 改变数据库
        /// </summary>
        /// <param name="db"></param>
        public void ChangeDb(long db)
        {
            using var r = _pool?.GetClient();
            if (r != null) r.Db = db;
        }
    }
}
