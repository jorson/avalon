using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Avalon.Framework;
using Avalon.Test.Service;
using Avalon.NHibernateAccess;

namespace Avalon.Test.Repository
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Table("demo_user");
            Id(o => o.UserId)
                .GeneratedBy.Native();
            Map(o => o.UserName);
            Map(o => o.EnumDemo).CustomType<EnumField>();
            Map(o => o.ListDemo).CustomType<JsonListUserType<int>>();
            Map(o => o.DateDemo).CustomSqlType("timestamp");
        }
    }

    public class UserDefine : ClassDefine<User>
    {
        public UserDefine()
        {
            Id(o => o.UserId);
            Map(o => o.UserName);
            Map(o => o.EnumDemo);
            Map(o => o.ListDemo);
            Map(o => o.DateDemo);
            //标记为按主键缓存
            Cache();
        }
    }
}
