using Avalon.Framework;
using Avalon.Test.Service;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Repository
{
    /// <summary>
    /// 定义单表的映射关系
    /// </summary>
    public class OrderMapping : ClassMap<Order>
    {
        public OrderMapping()
        {
            Table("demo_order");
            Id(o => o.OrderId)
                .GeneratedBy.Native();
            Map(o => o.OrderNumber);
            Map(o => o.UserId);
            Map(o => o.OrderDate).CustomSqlType("timestamp");
        }
    }

    public class OrderDefine : ClassDefine<Order>
    {
        public OrderDefine()
        {
            Id(o => o.OrderId);
            Map(o => o.OrderNumber);
            Map(o => o.UserId);
            Map(o => o.OrderDate);
            //标记为按主键缓存
            Cache();
        }
    }
}
