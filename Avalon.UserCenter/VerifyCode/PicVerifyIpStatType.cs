using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 图片验证码ip统计类型
    /// </summary>
    public enum PicVerifyIpStatType
    {
        /// <summary>
        /// 注册成功
        /// </summary>
        RegisterSuccess= 0,
        /// <summary>
        /// 登录失败
        /// </summary>
        LogonFailure = 1,
    }
}
