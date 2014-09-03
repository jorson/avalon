using Avalon.Utility;
using System;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 用户密码错误统计
    /// </summary>
    public class UserPasswordErrorStat
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual Guid Id { get; protected set; } 

        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime RecordTime { get; protected set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }


        public UserPasswordErrorStat()
        {
            RecordTime = NetworkTime.Now;
        }
    }
}