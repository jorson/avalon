using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 用户验证码重复发送统计
    /// </summary>
    public class UserVeriyCodeRepeateSendStat
    {
         /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 统计类型
        /// </summary>
        public virtual UserVeriyCodeRepeateSendStatType Type { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime RecordTime { get;protected set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; protected set; }

        public UserVeriyCodeRepeateSendStat()
        {
            RecordTime = NetworkTime.Now;
            ExpireTime = RecordTime.AddSeconds(AucConfig.UserVeriyCodeRepeateSendStatExpireSeconds);
        }
    }

    /// <summary>
    /// 用户验证码重复发送统计类型
    /// </summary>
    public enum UserVeriyCodeRepeateSendStatType : byte
    {
        Email=1,
        SecurityEmail=2,
        RecoverEmail=3
    }
}
