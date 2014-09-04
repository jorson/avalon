﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    /// <summary>
    /// 出现验证码规则
    /// </summary>
    public enum VerifyCodeType
    {
        /// <summary>
        /// 验证码是必须的
        /// </summary>
        Need = 1,

        /// <summary>
        /// 验证码根据IP的出现
        /// </summary>
        Judge = 2,


        /// <summary>
        /// 不需要验证码
        /// </summary>
        No = 3,
    }

    public static partial class EnumHelper
    {
        public static string ToName(this VerifyCodeType verifyCodeType)
        {
            switch (verifyCodeType)
            {
                case VerifyCodeType.Need:
                    return "验证码是必须的";
                case VerifyCodeType.Judge:
                    return "验证码根据IP的出现";
                case VerifyCodeType.No:
                    return "不需要验证码";
                default:
                    return "";
            }
        }
    }

}
