using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Avalon.Framework;
using Avalon.UserCenter;
using Avalon.Utility;
using Avalon.MongoAccess;

namespace Avalon.Repository.UserCenter.Repository
{

    public class WebSiteBaseSettingRepository : AbstractNoShardRepository<WebSiteBaseSetting>, IWebSiteBaseSettingRepository
    {

    }

    public class IpAddressCityRepository : AbstractNoShardRepository<IpAddressCity>, IIpAddressCityRepository
    {

    }

    public class IpCityRepository : AbstractNoShardRepository<IpCity>, IIpCityRepository
    {

    }

    public class IpProvinceRepository : AbstractNoShardRepository<IpProvince>, IIpProvinceRepository
    {

    }

    public class EmailVerifyRepository : AbstractNoShardRepository<EmailVerify>, IEmailVerifyRepository
    {
        private MongoCollection table;
        private MongoCollection Table
        {
            get
            {
                return table ??
                       (table =
                           this.GetMongoSession(ShardParams.Empty).GetCollection<EmailVerify>(ShardParams.Empty));
            }
        }

        public void Remove(string key)
        {
            var q = Query.EQ("_id", key);
            Table.Remove(q);
        }

        public string GetValue(string key)
        {
            var emailverify = Get(ShardParams.Empty, key);
            if (emailverify == null)
                return string.Empty;
            return emailverify.Value;
        }

        public void CreateKeyValue(string key, string value, TimeSpan expire)
        {
            Create(new EmailVerify
            {
                Key = key,
                Value = value,
                ExpireTime = NetworkTime.Now.AddMilliseconds(expire.TotalMilliseconds),
                IsVerify = false
            });
        }

        public void SetKeyValue(string key, bool isVierify)
        {
            var emailverify = Get(ShardParams.Empty, key);
            emailverify.IsVerify = isVierify;
            Update(emailverify);
        }

        public void RemoveByValue(string value)
        {
            var q = Query.EQ("Value", value);
            Table.Remove(q);
        }
    }

    public class MobileVerifyRepository : AbstractNoShardRepository<MobileVerify>, IMobileVerifyRepository
    {

        private MongoCollection table;
        private MongoCollection Table
        {
            get
            {
                return table ??
                       (table =
                           this.GetMongoSession(ShardParams.Empty).GetCollection<MobileVerify>(ShardParams.Empty));
            }
        }

        public void Remove(string key)
        {
            var q = Query.EQ("_id", key);
            Table.Remove(q);
        }

        public MobileVerify GetValue(string key)
        {
            var mobileverify = Get(ShardParams.Empty, key);
            return mobileverify ?? new MobileVerify();
        }

        public void CreateKeyValue(string key, string value, TimeSpan expire)
        {
            Create(new MobileVerify
            {
                Key = key,
                Value = value,
                ExpireTime = NetworkTime.Now.AddMilliseconds(expire.TotalMilliseconds),
                VerifyCount = 0
            });
        }

        public void SetKeyCount(string key)
        {
            var mobileverify = Get(ShardParams.Empty, key);
            if (mobileverify != null)
            {
                mobileverify.VerifyCount++;
                Update(mobileverify);
            }
        }
    }
}