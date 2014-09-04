using Avalon.Access;
using Avalon.Framework;
using Avalon.UserCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Repository.UserCenter
{
    public class KeyValueRepository : AbstractNoShardRepository<IVerifyCode>, IKeyValueRepository
    {
        const string ConnectionName = "redisconn";

        void IKeyValueRepository.CreateKeyValue(string key, string value, TimeSpan expire)
        {
            using (var client = RedisManager.Instance.CreateRedisClient(ConnectionName))
            {
                client.SetEntry(key, value, expire);
            }
        }

        void IKeyValueRepository.SetKeyValue<T>(string key, T value, TimeSpan expire)
        {
            using (var client = RedisManager.Instance.CreateRedisClient(ConnectionName))
            {
                client.Set(key, value, expire);
            }
        }

        void IKeyValueRepository.Remove(string key)
        {
            using (var client = RedisManager.Instance.CreateRedisClient(ConnectionName))
            {
                client.Remove(key);
            }
        }

        string IKeyValueRepository.GetStringValue(string key)
        {
            using (var client = RedisManager.Instance.CreateRedisClient(ConnectionName))
            {
                return client.GetValue(key);
            }
        }

        T IKeyValueRepository.GetValue<T>(string key)
        {
            using (var client = RedisManager.Instance.CreateRedisClient(ConnectionName))
            {
                return client.Get<T>(key);
            }
        }

        
    }
}
