using System;
using FluentNHibernate.Mapping;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using Avalon.UserCenter;
using Avalon.Framework;
using Avalon.MongoAccess;

namespace Avalon.Repository.UserCenter.Mappinig
{

    public class WebSiteBaseSettingMapping : ClassMap<WebSiteBaseSetting>
    {
        public WebSiteBaseSettingMapping()
        {
            Table("b_websitebasesettings");
            Id(o => o.Key).Column("Name");
            Map(o => o.Value).Column("Val");
        }
    }

    public class WebSiteBaseSettingDefine : ClassDefine<WebSiteBaseSetting>
    {
        public WebSiteBaseSettingDefine()
        {
            Id(o => o.Key);
            Map(o => o.Value);

            Cache();
        }
    }

    public class IpAddressMapping : ClassMap<IpAddressCity>
    {
        public IpAddressMapping()
        {
            Table("ip_address_city");
            Id(o => o.Id);
            Map(o => o.CityId);
            Map(o => o.StartIp);
            Map(o => o.EndIp);
        }
    }

    public class IpAddressDefine : ClassDefine<IpAddressCity>
    {
        public IpAddressDefine()
        {
            Id(o => o.Id);
            Map(o => o.CityId);
            Map(o => o.StartIp);
            Map(o => o.EndIp);
        }
    }

    public class IpProvinceMapping : ClassMap<IpProvince>
    {
        public IpProvinceMapping()
        {
            Table("ip_province");
            Id(o => o.Id);
            Map(o => o.Name);
        }
    }

    public class IpProvinceDefine : ClassDefine<IpProvince>
    {
        public IpProvinceDefine()
        {
            Id(o => o.Id);
            Map(o => o.Name);
            Cache();
        }
    }

    public class IpCityMapping : ClassMap<IpCity>
    {
        public IpCityMapping()
        {
            Table("ip_city");
            Id(o => o.Id);
            Map(o => o.CityIndex);
            Map(o => o.ProvinceId);
            Map(o => o.Name);
        }
    }

    public class IpCityDefine : ClassDefine<IpCity>
    {
        public IpCityDefine()
        {
            Id(o => o.Id);
            Map(o => o.CityIndex);
            Map(o => o.ProvinceId);
            Map(o => o.Name);
            Cache();
        }
    }

    public class EmailVerifyMapping : MongoMap<EmailVerify>
    {
        public EmailVerifyMapping()
        {
            RegisterClassMap(ev =>
            {
                ev.AutoMap();
                ev.MapIdMember(e => e.Key);
                ev.GetMemberMap(e => e.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class EmailVerifyDefine : ClassDefine<EmailVerify>
    {
        public EmailVerifyDefine()
        {
            Table("verifycode_email");
            Id(o => o.Key);
            Map(o => o.Value);
            Map(o => o.ExpireTime);
            Map(o => o.IsVerify);
        }
    }

    public class MoblieVerifyMapping : MongoMap<MobileVerify>
    {
        public MoblieVerifyMapping()
        {
            RegisterClassMap(ev =>
            {
                ev.AutoMap();
                ev.MapIdMember(e => e.Key);
                ev.GetMemberMap(e => e.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class MobileVerifyDefine : ClassDefine<MobileVerify>
    {
        public MobileVerifyDefine()
        {
            Table("verifycode_mobile");
            Id(o => o.Key);
            Map(o => o.Value);
            Map(o => o.ExpireTime);
            Map(o => o.VerifyCount);
        }
    }
}