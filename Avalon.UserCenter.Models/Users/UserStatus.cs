using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    [Flags]
    public enum UserStatus
    {
        /// <summary>
        /// 初始化
        /// </summary>
        Init=0,
        /// <summary>
        /// 正常
        /// </summary>
        Ready=1,
        /// <summary>
        /// 需要修改密码
        /// </summary>
        ChangePassword=2,
        /// <summary>
        /// 冻结
        /// </summary>
        Frozen=4,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled=64
    }

    [Flags]
    public enum AccountStatus 
    {
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Init = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Ready = 1,
        /// <summary>
        /// 需要修改密码
        /// </summary>
        [Description("需要修改密码")]
        ChangePassword = 2,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Frozen = 4,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled = 64
    }
}
