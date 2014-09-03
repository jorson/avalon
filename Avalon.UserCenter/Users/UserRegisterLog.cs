using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;

namespace Avalon.UserCenter
{
    public class UserRegisterLog
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public virtual DateTime RegisterTime { get; set; }
        /// <summary>
        /// 注册时的应用标识
        /// </summary>
        public virtual int RegisterAppId { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        public virtual int TerminalCode { get; set; }
        /// <summary>
        /// 注册方式
        /// </summary>
        public virtual RegisterMode RegisterMode { get; set; }
        /// <summary>
        /// 注册时的ip地址
        /// </summary>
        public virtual long IpAddress { get; set; }

        /// <summary>
        /// 注册时的IP所在城市
        /// </summary>
        public virtual int IpCityId { get; set; }
    }
}
