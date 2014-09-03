using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalon.UserCenter.Models;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class PicVerifyIpStat
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// 用户IP地址
        /// </summary>
        public virtual long IPAddress { get; set; }

        /// <summary>
        /// 操作类型1：注册2：登录
        /// </summary>
        public virtual PicVerifyIpStatType OptType { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime RecordTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public virtual DateTime ExpireTime { get; set; }

        public PicVerifyIpStat()
        {
            RecordTime = NetworkTime.Now;
            ExpireTime = RecordTime.AddSeconds(AucConfig.KeepPicVerifyStatSeconds);
            //Id = DateTime.Now.Ticks;
        }
    }
}
