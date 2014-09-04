using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using ServiceStack;
using ServiceStack.Common.Utils;
using Avalon.UserCenter;
using Avalon.Framework;
using Avalon.UserCenter.Models;
using Avalon.Framework.Querys;
using Avalon.NHibernateAccess;

namespace Avalon.Repository.UserCenter.Mappinig
{
    public class UserMapping:ClassMap<User>
    {
        public UserMapping()
        {
            Table("u_user");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.LoginName);
            Map(o => o.LoginEmail);
            Map(o => o.LoginMobile);
            Map(o => o.IDCard, "LoginIDCard");
            Map(o => o.Password, "PASSWORD");
            Map(o => o.AvatarObjectId);
            Map(o => o.IsVerifyLoginMobile);
            Map(o => o.IsVerifyLoginEmail);
            Map(o => o.Status).CustomType<UserStatus>();
            Map(o => o.CreateTime).CustomType("timestamp");
            Map(o => o.UpdateTime).CustomType("timestamp");
            Map(o => o.FrozenExpire).CustomType("timestamp");
        }
    }

    public class UserDefine : ClassDefine<User>
    {
        public UserDefine()
        {
            Id(o => o.Id);
            Map(o => o.LoginName);
            Map(o => o.LoginEmail);
            Map(o => o.LoginMobile);
            Map(o => o.IDCard);
            Map(o => o.Password);
            Map(o => o.AvatarObjectId);
            Map(o => o.IsVerifyLoginMobile);
            Map(o => o.IsVerifyLoginEmail);
            Map(o => o.Status);
            Map(o => o.UpdateTime);
            Map(o => o.FrozenExpire);
            Map(o => o.CreateTime);

            Cache();
        }
    }

    public class UserSecurityMapping : ClassMap<UserSecurity>
    {
        public UserSecurityMapping()
        {
            Table("u_usersecurity");
            Id(o => o.UserId).GeneratedBy.Assigned();
            Map(o => o.Mobile);
            Map(o => o.Email);
            Map(o => o.IsVerifyEmail);
            Map(o => o.UpdateTime).CustomType("timestamp");
        }
    }

    public class UserSecurityDefine : ClassDefine<UserSecurity>
    {
        public UserSecurityDefine()
        {
            Id(o => o.UserId);
            Map(o => o.Mobile);
            Map(o => o.Email);
            Map(o => o.IsVerifyEmail);
            Map(o => o.UpdateTime);
        }
    }

    public class IDCardRetrieveView : QueryView
    {
        public IDCardRetrieveView()
        {
            IDCardRetrieve idr = null;
            From(() => idr);

            Define<IDCardRetrieveFilter>()
                .Map(o => o.Id, () => idr.Id)
                .Map(o => o.IdCard, () => idr.IDCard)
                .Map(o => o.UserFullName,() => idr.UserFullName)
                .Map(o => o.ContactEmail, () => idr.ContactEmail)
                .Map(o => o.ContactMobile, () => idr.ContactMobile)
                .Map(o => o.AuditStatus, () => idr.AuditStatus)
                .Map(o => o.CreateTime, () => idr.CreateTime)
                .Map(o => o.AuditTime, () => idr.AuditTime)
                .Map(o => o.RegistAppId, () => idr.RegistAppId);
        }
    }

    public class IDCardRetrieveMapping : ClassMap<IDCardRetrieve>
    {
        public IDCardRetrieveMapping()
        {
            Table("u_idcardretrieve");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.IDCard);
            Map(o => o.IDCardImgObjectId);
            Map(o => o.CreateTime).CustomType("timestamp");
            Map(o => o.AuditRemark);
            Map(o => o.AuditDenyReason);
            Map(o => o.AuditorUserId);
            Map(o => o.AuditorUserName);
            Map(o => o.AuditStatus).CustomType<IDCardRetrieveAuditStatus>();
            Map(o => o.AuditTime).CustomType("timestamp");
            Map(o => o.ContactEmail);
            Map(o => o.ContactMobile);
            Map(o => o.EmailNofiyJobId);
            Map(o => o.MobileNofiyJobId);
            Map(o => o.CreatAppId);
            Map(o => o.UserFullName);
            Map(o => o.UserId);
            Map(o => o.NofiyFlag).CustomType<AuditNotifyStatus>();
            Map(o => o.RegistAppId);
        }
    }

    public class IDCardRetrieveDefine : ClassDefine<IDCardRetrieve>
    {
        public IDCardRetrieveDefine()
        {
            Id(o => o.Id);
            Map(o => o.IDCard);
            Map(o => o.IDCardImgObjectId);
            Map(o => o.CreateTime);
            Map(o => o.AuditRemark);
            Map(o => o.AuditDenyReason);
            Map(o => o.AuditorUserId);
            Map(o => o.AuditorUserName);
            Map(o => o.AuditStatus);
            Map(o => o.AuditTime);
            Map(o => o.ContactEmail);
            Map(o => o.ContactMobile);
            Map(o => o.EmailNofiyJobId);
            Map(o => o.MobileNofiyJobId);
            Map(o => o.CreatAppId);
            Map(o => o.UserFullName);
            Map(o => o.UserId);
            Map(o=>o.NofiyFlag);
            Map(o => o.RegistAppId);
        }
    }
    public class ManulRetrieveView : QueryView
    {
        public ManulRetrieveView()
        {
            ManulRetrieve idr = null;
            From(() => idr);

            Define<ManulRetrieveFilter>()
                .Map(o => o.Id, () => idr.Id)
                .Map(o => o.UserIdentity, () => idr.UserIdentity)
                .Map(o => o.UserFullName, () => idr.UserFullName)
                .Map(o => o.ContactEmail, () => idr.ContactEmail)
                .Map(o => o.ContactMobile, () => idr.ContactMobile)
                .Map(o => o.AuditStatus, () => idr.AuditStatus)
                .Map(o => o.CreateTime, () => idr.CreateTime)
                .Map(o => o.AuditTime, () => idr.AuditTime)
                .Map(o => o.RegistAppId, () => idr.RegistAppId);
        }
    }

    public class ManulRetrieveMapping : ClassMap<ManulRetrieve>
    {
        public ManulRetrieveMapping()
        {
            Table("u_manulretrieve");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.UserId);
            Map(o => o.UserIdentity);
            Map(o => o.CreateTime).CustomType("timestamp");
            Map(o => o.UserFullName);
            Map(o => o.ContactEmail);
            Map(o => o.ContactMobile);
            Map(o => o.AuditStatus).CustomType<IDCardRetrieveAuditStatus>();
            Map(o => o.AuditorUserId);
            Map(o => o.AuditorUserName);
            Map(o => o.AuditTime).CustomType("timestamp");
            Map(o => o.OtherInfo).CustomType<JsonUserType<ManulRetrieveOtherInfo>>();
            Map(o => o.EmailNofiyJobId);
            Map(o => o.MobileNofiyJobId);
            Map(o => o.RegistAppId);
            Map(o => o.CreatAppId);
            Map(o => o.NofiyFlag).CustomType<AuditNotifyStatus>();
            
        }
    }

    public class ManulRetrieveDefine : ClassDefine<ManulRetrieve>
    {
        public ManulRetrieveDefine()
        {
            Id(o => o.Id);
            Map(o => o.UserId);
            Map(o => o.UserIdentity);
            Map(o => o.CreateTime);
            Map(o => o.UserFullName);
            Map(o => o.ContactEmail);
            Map(o => o.ContactMobile);
            Map(o => o.AuditStatus);
            Map(o => o.AuditorUserId);
            Map(o => o.AuditorUserName);
            Map(o => o.AuditTime);
            Map(o => o.OtherInfo);
            Map(o => o.EmailNofiyJobId);
            Map(o => o.MobileNofiyJobId);
            Map(o => o.RegistAppId);
            Map(o => o.CreatAppId);
            Map(o => o.NofiyFlag);
        }
    }
    public class UserAccountMapping : ClassMap<UserAccount>
    {
        public UserAccountMapping()
        {
            Table("u_usermapping");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.SolutionId);
            Map(o => o.AppId);
            Map(o => o.Account);
            Map(o => o.UserId);
            Map(o => o.NickName);
            Map(o => o.Password);
            Map(o => o.Status).CustomType<UserStatus>();
            Map(o => o.CreateTime).CustomType("timestamp");  
            Map(o => o.UpdateTime).CustomType("timestamp");
            Map(o => o.FrozenExpire).CustomType("timestamp");
        }
    }


    public class UserAccountDefine : ClassDefine<UserAccount>
    {
        public UserAccountDefine()
        {
            Id(o => o.Id);
            Map(o => o.SolutionId);
            Map(o => o.AppId);
            Map(o => o.Account);
            Map(o => o.UserId);
            Map(o => o.NickName);
            Map(o => o.Status);
            Map(o => o.CreateTime);
            Map(o => o.UpdateTime);
            Map(o => o.FrozenExpire);
            Map(o=>o.Password);
        }
    }

    public class UserAccountView : QueryView
    {
        public UserAccountView()
        {
            UserAccount userAccount = null;
            From(() => userAccount);
            Define<UserAccountFilter>()
                .Map(o=>o.Id,()=>userAccount.Id)
                .Map(o=>o.AppId,()=>userAccount.AppId)
                .Map(o => o.Account, () => userAccount.Account)
                .Map(o => o.SolutionId, () => userAccount.SolutionId);
        }
    }

    public class SolutionMapping : ClassMap<Solution>
    {
        public SolutionMapping()
        {
            Table("u_solution");
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.Code);
            Map(o => o.Name);
            Map(o => o.AppId);
            Map(o => o.CreateTime).CustomType("timestamp");
            Map(o => o.UpdateTime).CustomType("timestamp");
            Map(o => o.Settings).CustomType<JsonUserType<SolutionSettings>>();
            Map(o => o.Type).CustomType<SolutionType>();
            Map(o => o.Status).CustomType<SolutionStatus>();
        }
    }


    public class SolutionDefine : ClassDefine<Solution>
    {
        public SolutionDefine()
        {
            Id(o => o.Id);
            Map(o => o.Code);
            Map(o => o.Name);
            Map(o => o.AppId);
            Map(o => o.CreateTime);
            Map(o => o.UpdateTime);
            Map(o => o.Settings);
            Map(o => o.Type);
            Map(o => o.Status);

            Cache();
            CacheRegion(Solution.CacheRegionCode, s => s.Code);
        }
    }

    public class SolutionView : QueryView
    {
        public SolutionView()
        {
            Solution idr = null;
            From(() => idr);

            Define<SolutionFilter>()
                .Map(o => o.Id, () => idr.Id)
                .Map(o => o.AppId, () => idr.AppId)
                .Map(o => o.Type, () => idr.Type);
        }
    }

    public class UserOldPasswordMapping : ClassMap<UserOldPassword>
    {
        public UserOldPasswordMapping()
        {
            Table("u_old_password");
            Id(o => o.UserId).GeneratedBy.Assigned();
            Map(o => o.Password);
        }
    }

    public class UserOldPasswordDefine : ClassDefine<UserOldPassword>
    {
        public UserOldPasswordDefine()
        {
            Id(o => o.UserId);
            Map(o => o.Password);
        }
    }
}
