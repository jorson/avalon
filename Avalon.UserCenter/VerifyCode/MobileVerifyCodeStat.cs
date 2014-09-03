using Avalon.Utility;
using System;

namespace Avalon.UserCenter
{
    public class MobileVerifyCodeStat
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual Guid Id { get; protected set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 用户IP地址
        /// </summary>
        public virtual long IPAddress { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        public MobileVerifyCodeStat()
        {
            CreateTime = NetworkTime.Now;
            ExpireTime = CreateTime.AddSeconds(AucConfig.KeepMobileVerifyStatSeconds);
        }

        public virtual bool IsExpire()
        {
            return ExpireTime < NetworkTime.Now;
        }

    }
}
