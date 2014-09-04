using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using MongoDB.Bson.Serialization.Options;
using Avalon.UserCenter;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Framework.Querys;

namespace Avalon.Repository.UserCenter.Mappinig
{
    public class UserRegisterMapping : ClassMap<UserRegisterLog>
    {
        public UserRegisterMapping()
        {
            Table("log_userregister");
            Id(o => o.UserId).GeneratedBy.Assigned();
            Map(o => o.RegisterAppId);
            Map(o => o.TerminalCode);
            Map(o => o.RegisterMode).CustomType<RegisterMode>();
            Map(o => o.RegisterTime).CustomType("timestamp");
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
        }
    }

    public class UserRegisterDefine : ClassDefine<UserRegisterLog>
    {
        public UserRegisterDefine()
        {
            Id(o => o.UserId);
            Map(o => o.RegisterAppId);
            Map(o => o.TerminalCode);
            Map(o => o.RegisterMode);
            Map(o => o.RegisterTime);
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
        }
    }

    public class UserLoginLogMapping : ClassMap<UserLoginLog>
    {
        public UserLoginLogMapping()
        {
            Table("log_userlogin");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.UserId);
            Map(o => o.IdentityType).CustomType<UserIdentityType>();
            Map(o => o.SolutionId);
            Map(o => o.AppId);
            Map(o => o.TerminalCode);
            Map(o => o.LoginTime).CustomType("timestamp");
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
        }
    }

    public class UserLoginLogDefine : ClassDefine<UserLoginLog>
    {
        public UserLoginLogDefine()
        {
            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.IdentityType);
            Map(o => o.SolutionId);
            Map(o => o.AppId);
            Map(o => o.TerminalCode);
            Map(o => o.LoginTime);
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
        }
    }

    public class UserLoginLogView : QueryView
    {
        public UserLoginLogView()
        {
            UserLoginLog userLoginLog = null;
            From(() => userLoginLog);

            Define<UserLoginLogFilter>()
                .Map(o=>o.LoginTime,()=>userLoginLog.LoginTime)
                .Map(o => o.UserId, () => userLoginLog.UserId);
        }
    }

    public class UserMainHistoryLogMapping : ClassMap<UserMainHistoryLog>
    {
        public UserMainHistoryLogMapping()
        {
            Table("log_usermainhistory");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.UserId);
            Map(o => o.OldVal);
            Map(o => o.NewVal);
            Map(o => o.ValType).CustomType<UserHistoryValueType>();
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
            Map(o => o.AppId);
            Map(o => o.TerminalCode);
            Map(o => o.CreateTime).CustomType("timestamp");
        }
    }

    public class UserMainHistoryLogDefine : ClassDefine<UserMainHistoryLog>
    {
        public UserMainHistoryLogDefine()
        {
            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.OldVal);
            Map(o => o.NewVal);
            Map(o => o.ValType);
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
            Map(o => o.AppId);
            Map(o => o.TerminalCode);
            Map(o => o.CreateTime);
        }
    }

    public class UserAccountCreateInfoMapping : ClassMap<UserAccountCreateInfo>
    {
        public UserAccountCreateInfoMapping()
        {
            Table("log_usermappingcreate");
            Id(o => o.UserAccountId).Column("UserMappingId").GeneratedBy.Assigned();
            Map(o => o.AppId);
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
            Map(o => o.RegisterMode).CustomType<RegisterMode>();
            Map(o => o.TerminalCode);
            Map(o => o.CreateTime).CustomType("timestamp");
        }
    }

    public class UserAccountCreateInfoDefine : ClassDefine<UserAccountCreateInfo>
    {
        public UserAccountCreateInfoDefine()
        {
            Id(o => o.UserAccountId);
            Map(o => o.AppId);
            Map(o => o.IpAddress);
            Map(o => o.IpCityId);
            Map(o => o.RegisterMode);
            Map(o => o.TerminalCode);
            Map(o => o.CreateTime);
        }
    }

}
