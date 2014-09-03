using System;
using System.Collections.Generic;
using Avalon.Framework;

namespace Avalon.UserCenter
{
    public interface IUserRegisterLogRepository : INoShardRepository<UserRegisterLog>
    {
         
    }

    public interface IUserLoginLogRepository : INoShardRepository<UserLoginLog>
    {
        IList<UserLoginLog> GetLoginStaList(long userid, DateTime createTime);
    }

    public interface IUserMainHistoryLogRepository : INoShardRepository<UserMainHistoryLog>
    {

    }

    public interface IUserAccountCreateInfoRepository : INoShardRepository<UserAccountCreateInfo>
    {

    }
    
}