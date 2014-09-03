using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.Security.Cryptography.Extensions
{
    /// <summary>
    /// HttpRequestBase扩展
    /// </summary>
    public static class HttpRequestBaseExtend
    {
        /// <summary>
        /// 验证base值是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool ValidAuthorizationBase(this HttpRequestBase value, out AuthorizationUser user)
        {
            var authorization = value.Headers["Authorization"];
            user = null;
            if (!String.IsNullOrEmpty(authorization))
            {
                try
                {
                    if (authorization.IndexOf("Basic", System.StringComparison.Ordinal) == 0)
                    {
                        var auth = Encoding.ASCII.GetString(Convert.FromBase64String(authorization.Substring(6)));
                        var vs = auth.Split(':');
                        user = new AuthorizationUser { UserId = Int32.Parse(vs[0]), Password = vs[1] };
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                
            }
            return false;
        }
    }

    /// <summary>
    /// 授权用户
    /// </summary>
    public class AuthorizationUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
