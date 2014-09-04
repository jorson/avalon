using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth.Mvc
{
    /// <summary>
    /// 声明授权的类型
    /// </summary>
    public enum OAuthType
    {
        /// <summary>
        /// 表示不需要授权
        /// </summary>
        None,
        /// <summary>
        /// 表示应用程序授权
        /// </summary>
        App,
        /// <summary>
        /// 表示用户授权
        /// </summary>
        User
    }
}
