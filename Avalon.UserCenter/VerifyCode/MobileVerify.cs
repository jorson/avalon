using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class MobileVerify
    {
        /// <summary>
        /// 键值
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// 验证次数
        /// </summary>
        public virtual int VerifyCount { get; set; }
    }
}
