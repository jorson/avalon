using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class EmailVerify
    {
        /// <summary>
        /// 键值
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 序列化内容
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        /// <summary>
        /// 是否被验证
        /// </summary>
        public virtual bool IsVerify { get; set; }
    }
}
