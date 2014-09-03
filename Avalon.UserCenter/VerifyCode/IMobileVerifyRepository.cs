using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public interface IMobileVerifyRepository : INoShardRepository<MobileVerify>
    {
        void CreateKeyValue(string key, string value, TimeSpan expire);

        void SetKeyCount(string key);

        void Remove(string key);

        MobileVerify GetValue(string key);
    }
}
