using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class UserMainHistoryLog
    {

        /// <summary>
        /// 标识
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 旧值
        /// </summary>
        public virtual string OldVal { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public virtual string NewVal { get; set; }
        /// <summary>
        /// 值类型
        /// </summary>
        public virtual UserHistoryValueType ValType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public virtual long IpAddress { get; set; }
        /// <summary>
        /// 注册时的IP所在城市
        /// </summary>
        public virtual int IpCityId { get; set; }
        /// <summary>
        /// 应用标识
        /// </summary>
        public virtual int AppId { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        public virtual int TerminalCode { get; set; }

        public UserMainHistoryLog()
        {
            CreateTime = NetworkTime.Now;
        }
    }



    public enum UserHistoryValueType
    {
        UserName = 1,
        LoginEmail = 2,
        IDCard = 3,
        LoginMobile = 4,
        Password = 5,
        SecurityEmail = 6,
        SecurityMobile = 7,
        Status = 8,
        Account = 9,
        AccountPassword = 10,
        AdminEidtIDCard = 13,
    }
}
