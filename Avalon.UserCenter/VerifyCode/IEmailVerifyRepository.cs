using Avalon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public interface IEmailVerifyRepository : INoShardRepository<EmailVerify>
    {
        void CreateKeyValue(string key, string value, TimeSpan expire);

        void SetKeyValue(string key, bool isVierify);

        void Remove(string key);

        void RemoveByValue(string value);

        string GetValue(string key);
    }
}
