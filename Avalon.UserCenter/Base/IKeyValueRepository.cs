using System;
using Avalon.UserCenter.Models;
using Avalon.Framework;

namespace Avalon.UserCenter
{
    public interface IKeyValueRepository : IRepository<IVerifyCode>
    {
        void CreateKeyValue(string key, string value, TimeSpan expire);

        void SetKeyValue<T>(string key, T value, TimeSpan expire);

        void Remove(string key);

        string GetStringValue(string key);

        T GetValue<T>(string key);
    }
}