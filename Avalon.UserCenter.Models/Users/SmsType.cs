using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public enum SmsType
    {
        /// <summary>
        /// 注册短信
        /// </summary>
        Register = 0,
        /// <summary>
        /// 登陆短信
        /// </summary>
        Login = 1,
        /// <summary>
        /// 找回密码短信
        /// </summary>
        Forget = 2,
        /// <summary>
        /// 密保短信
        /// </summary>
        Security = 3,
        /// <summary>
        /// 帐号方案找回密码短信
        /// </summary>
        SolutionForget = 4,
        /// <summary>
        /// 帐号方案密保短信
        /// </summary>
        SolutionSecurity = 5
    }
}
