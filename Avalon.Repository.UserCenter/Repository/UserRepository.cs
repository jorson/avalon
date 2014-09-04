using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Avalon.Framework;
using Avalon.UserCenter;
using Avalon.NHibernateAccess;

namespace Avalon.Repository.UserCenter
{
    public class UserRepository : AbstractNoShardRepository<User>, IUserRepository
    {

    }

    public class UserSecurityRepository : AbstractNoShardRepository<UserSecurity>, IUserSecurityRepository
    {

    }

    public class IDCardRetrieveRepository : AbstractNoShardRepository<IDCardRetrieve>, IIDCardRetrieveRepository
    {

    }

    public class UserPasswordErrorStatRepository : AbstractNoShardRepository<UserPasswordErrorStat>, IUserPasswordErrorStatRepository
    {
        
    }

    public class SolutionRepository : AbstractNoShardRepository<Solution>, ISolutionRepository
    {
        public IList<int> GetAppHasUserSolutionIds(int appId)
        {
            var ua = this.GetSession().Query<UserAccount>().Where(u => u.AppId == appId)
                .GroupBy(u => u.SolutionId, u => u.SolutionId).Select(g => g.Key).ToList();

            return ua;
        }
    }

    public class UserAccountRepository : AbstractNoShardRepository<UserAccount>, IUserAccountRepository
    {

    }

    public class CustomerSupportRepository : AbstractNoShardRepository<ManulRetrieve>, ICustomerSupportRepository
    {

    }

    public class UserOldPasswordRepository : AbstractNoShardRepository<UserOldPassword>, IUserOldPasswordRepository
    {

    }
}
