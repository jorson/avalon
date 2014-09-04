using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using Avalon.MongoAccess;
using Avalon.Framework;
using Avalon.UserCenter;

namespace Avalon.Repository.UserCenter.Mappinig
{
    public class PicVerifyIpStatMapping : MongoMap<PicVerifyIpStat>
    {
        public PicVerifyIpStatMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
                cm.GetMemberMap(c => c.RecordTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class PicVerifyIpStatDefine : ClassDefine<PicVerifyIpStat>
    {
        public const string TABLENAME = "stat_picverifyip";

        public PicVerifyIpStatDefine()
        {
            Table(TABLENAME);

            Id(o => o.Id);
            Map(o => o.IPAddress);
            Map(o => o.OptType);
            Map(o => o.RecordTime);
            Map(o => o.ExpireTime);

        }
    }

    public class MobileVerifyCodeStatMapping : MongoMap<MobileVerifyCodeStat>
    {
        public MobileVerifyCodeStatMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
                cm.GetMemberMap(c => c.CreateTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class MobileVerifyCodeStatDefine : ClassDefine<MobileVerifyCodeStat>
    {
        public MobileVerifyCodeStatDefine()
        {
            Table("stat_mobileverifycode");

            Id(o => o.Id);
            Map(o => o.Mobile);
            Map(o => o.IPAddress);
            Map(o => o.CreateTime);
            Map(o => o.ExpireTime);
        }
    }

    public class UserPasswordErrorStatMapping : MongoMap<UserPasswordErrorStat>
    {
        public UserPasswordErrorStatMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
                cm.GetMemberMap(c => c.RecordTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class UserPasswordErrorStatDefine : ClassDefine<UserPasswordErrorStat>
    {
        public UserPasswordErrorStatDefine()
        {
            Table("stat_userpassworderror");

            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.RecordTime);
            Map(o => o.ExpireTime);
        }
    }

    public class UserVeriyCodeRepeateSendStatMapping : MongoMap<UserVeriyCodeRepeateSendStat>
    {
        public UserVeriyCodeRepeateSendStatMapping()
        {
            RegisterClassMap(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.Id).SetIdGenerator(GuidGenerator.Instance);
                cm.GetMemberMap(c => c.RecordTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
                cm.GetMemberMap(c => c.ExpireTime).SetSerializationOptions(new DateTimeSerializationOptions(DateTimeKind.Local));
            });
        }
    }

    public class UserVeriyCodeRepeateSendStatDefine : ClassDefine<UserVeriyCodeRepeateSendStat>
    {
        public UserVeriyCodeRepeateSendStatDefine()
        {
            Table("stat_userveriycoderepeatesend");

            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.Type);
            Map(o => o.RecordTime);
            Map(o => o.ExpireTime);
        }
    }
}
