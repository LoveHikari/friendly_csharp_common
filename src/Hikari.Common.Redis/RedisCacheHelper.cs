using ServiceStack.Redis;
using System;

namespace Hikari.Common.Redis
{
    public class RedisCacheHelper
    {
        private readonly PooledRedisClientManager _pool;

        RedisCacheHelper(string redisServerSession, int maxWritePoolSize = 3, int maxReadPoolSize = 1)
        {
            var redisHostStr = redisServerSession;

            if (!string.IsNullOrEmpty(redisHostStr))
            {
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
        }
        public void Add<T>(string key, T value, DateTime expiry)
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

            if (_pool != null)
            {
                using (var r = _pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        r.Set(key, value, expiry - DateTime.Now);
                    }
                }
            }

        }
        public void Add<T>(string key, T value, TimeSpan slidingExpiration)
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

            if (_pool != null)
            {
                using (var r = _pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        r.Set(key, value, slidingExpiration);
                    }
                }
            }

        }
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);

            if (_pool != null)
            {
                using (var r = _pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        obj = r.Get<T>(key);
                    }
                }
            }

            return obj;
        }
        public void Remove(string key)
        {
            if (_pool != null)
            {
                using (var r = _pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        r.Remove(key);
                    }
                }
            }
        }
        public bool Exists(string key)
        {
            if (_pool != null)
            {
                using (var r = _pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        return r.ContainsKey(key);
                    }
                }
            }
            return false;
        }

        public void ChangeDb(long db)
        {
            using (var r = _pool.GetClient())
            {
                r.Db = db;
            }
        }
    }
}
