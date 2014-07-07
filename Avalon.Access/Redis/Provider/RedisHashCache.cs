using Avalon.Access;
using Avalon.RedisProvider;
using ServiceStack.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Utility
{
    /// <summary>
    /// 使用 RedisHash 数据结构缓存数据。
    /// </summary>
    /// <remarks>
    /// 分3种情况存储数据
    /// 1、空值：
    ///     key     #{id}:{ValueName}
    ///     value   {NullValue}
    /// 2、简单对象：
    ///     key     #{id}:{ValueName}
    ///     value   {value}
    /// 3、复杂对象
    ///     key     #{id}:{property}...
    ///     value   {value}...
    /// 所有的类型都包含值，该值可用于依赖缓存或被对象缓存的管理
    ///     key     {key}
    ///     value   {timestamp}
    /// </remarks>
    public class RedisHashCache : AbstractCache
    {
        RedisCacheDependProvider dependProvider;
        ILog log = LogManager.GetLogger<RedisHashCache>();

        protected override bool IsLocal
        {
            get { return false; }
        }

        public string ConnectionName
        {
            get;
            set;
        }

        protected override void InitSetting(IEnumerable<SettingNode> settingNodes)
        {
            base.InitSetting(settingNodes);

            this.TrySetSetting(settingNodes, ConfigurationName, "connectionName", o => o.ConnectionName);
        }

        protected override void InitCache()
        {
            base.InitCache();

            if (String.IsNullOrEmpty(ConnectionName))
                throw new ArgumentNullException("ConnectionName");
        }

        protected override void RemoveInner(Type type, string key)
        {
            using (var client = CreateRedisClient())
            {
                client.Remove(key);
            }
        }

        protected override IList<CacheItemResult> GetBatchResultInner(Type type, IEnumerable<string> keys)
        {
            var fieldKeys = ObjectHashMappingUtil.GetKeys(type);

            var clearKeys = new List<string>();
            List<CacheItemResult> results = new List<CacheItemResult>();
            using (var client = CreateRedisClient())
            {
                foreach (var key in keys)
                {
                    var kvs = client.GetAllEntriesFromHash(key);

                    string[] values = new string[fieldKeys.Length];
                    for (var i = 0; i < fieldKeys.Length; i++)
                    {
                        values[i] = kvs.TryGetValue(fieldKeys[i]);
                    }
                    object value = null;
                    try
                    {
                        value = ObjectHashMappingUtil.ToObject(type, values);
                    }
                    catch (Exception ex)
                    {
                        clearKeys.Add(key);
                        log.WarnFormat("反序列化数据 {0} 发生错误 {1}", String.Join("\r\n", values), ex.ToString());
                    }
                    results.Add(new CacheItemResult(key, value));
                }

                //清除无效的数据
                if (clearKeys.Count > 0)
                {
                    client.RemoveAll(clearKeys);
                }
            }
            return results;
        }

        protected override void SetBatchInner(IEnumerable<CacheItem> items, DateTime expiredTime)
        {
            List<KeyValuePair<string, string>> datas = new List<KeyValuePair<string, string>>();
            var tick = NetworkTime.Now.Ticks.ToString();
            using (var client = CreateRedisClient())
            {
                foreach (var item in items)
                {
                    var kvs = ObjectHashMappingUtil.ToHash(item.Value);

                    // add timestamp for depend and manager
                    kvs.Add(ObjectHashMappingUtil.TimeStampName, tick);
                    var key = item.Key;
                    client.SetRangeInHash(key, kvs);
                    // 设置过期时间
                    client.ExpireEntryAt(key, expiredTime);
                }
            }
        }

        protected override bool ContainsInner(Type type, string key)
        {
            return base.ContainsByResult(type, key);
        }

        IRedisClient CreateRedisClient()
        {
            return RedisManager.Instance.CreateRedisClient(ConnectionName);
        }
    }
}
