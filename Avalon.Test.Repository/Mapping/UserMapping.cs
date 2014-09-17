using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Avalon.Framework;
using Avalon.Test.Service;
using Avalon.NHibernateAccess;
using Avalon.Framework.Querys;

namespace Avalon.Test.Repository
{
    /// <summary>
    /// 定义单表的映射关系
    /// </summary>
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

    /// <summary>
    /// 定义联合查询的视图
    /// </summary>
    public class UserOrderQueryView : QueryView
    {
        public UserOrderQueryView()
        {
            User ur = null;
            Order od = null;
            From<User>(() => ur)
                .Join<Order>(()=>od, ()=>ur.UserId == od.UserId, JoinType.InnerJoin);

            Define<UserOrderQueryFilter>()
                .Map(o => o.UserName, () => ur.UserName)
                .Map(o => o.OrderNumber, () => od.OrderNumber);
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
