using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class AccountInfo
    {
        /// <summary>
        /// 帐号名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 注册应用
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisterIp { get; set; }

        /// <summary>
        /// 方案名称
        /// </summary>
        public string SolutionName { get; set; }
    }
}
