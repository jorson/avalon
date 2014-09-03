using Avalon.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Avalon.UserCenter
{
    public interface IUserRepository : INoShardRepository<User>
    {
         
    }

    public interface IUserSecurityRepository : INoShardRepository<UserSecurity>
    {

    }
    
    public interface IIDCardRetrieveRepository : INoShardRepository<IDCardRetrieve>
    {

    }

    public interface IUserPasswordErrorStatRepository : INoShardRepository<UserPasswordErrorStat>
    {

    }

    public interface ISolutionRepository : IRepository<Solution>
    {
        /// <summary>
        /// 获取应用有帐号注册的帐号方案标识列表
        /// </summary>
        /// <param name="appId">应用</param>
        /// <returns></returns>
        IList<int> GetAppHasUserSolutionIds(int appId);
    }

    public interface IUserAccountRepository : INoShardRepository<UserAccount>
    {

    }

    public interface ICustomerSupportRepository : INoShardRepository<ManulRetrieve>
    {

    }

    public interface IUserOldPasswordRepository : INoShardRepository<UserOldPassword>
    {

    }
}