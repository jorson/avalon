using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Avalon.UserCenter
{
    public interface IPicVerifyIpStatRepository : INoShardRepository<PicVerifyIpStat>
    {
        void Remove(long ipAddress, PicVerifyIpStatType type);

        int Count(long ipAddress, DateTime dateTime, PicVerifyIpStatType type);
    }

    public interface IMobileVerifyCodeStatRepository : INoShardRepository<MobileVerifyCodeStat>
    {

    }

    public interface IUserVeriyCodeRepeateSendStatRepository : INoShardRepository<UserVeriyCodeRepeateSendStat>
    {

    }
}
