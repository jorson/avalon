using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public enum ForgetMode
    {
        /// <summary>
        /// 邮箱找回
        /// </summary>
        Email=1,
        /// <summary>
        /// 手机找回
        /// </summary>
        Mobile=2,
        /// <summary>
        /// 证件号找回
        /// </summary>
        IDCard=3,
    }

    public class ForgetModeInfo
    {
        /// <summary>
        /// 找回方式
        /// </summary>
        public ForgetMode Mode { get; set; }

        /// <summary>
        /// 隐去部分信息，邮箱（ko*****t@163.com），手机号（180*****120），证件号（3501***********122）
        /// </summary>
        public string ShowInfo { get; set; }

        /// <summary>
        /// 是否是密保信息
        /// </summary>
        public bool IsSecurity { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}
