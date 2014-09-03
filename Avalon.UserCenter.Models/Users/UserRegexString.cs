using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class UserRegexString
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public const string UserName = @"^[a-z][a-z0-9_.]{5,19}$";

        /// <summary>
        /// 邮箱
        /// </summary>
        public const string Email = @"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$";

        /// <summary>
        /// 手机格式
        /// </summary>
        public const string Mobile = @"^(1)\d{10}$";

        /// <summary>
        /// 身份证（18位）
        /// </summary>
        public const string IDCard = @"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|71|(8[12])|91)\d{15}(\d|X|x)$";

        /// <summary>
        /// 旧身份证（15位）
        /// </summary>
        public const string IDCard15Bit = @"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|71|(8[12])|91)\d{13}$";
        /// <summary>
        /// 回乡证
        /// </summary>
        public const string HMIDCard = @"^([MmHh])(\d{8})(\d{2})?$";
        /// <summary>
        /// 军官证
        /// </summary>
        public const string ArmyIDCard = @"^([\u4E00-\u9FA5]|[\u4E00-\u9FA5]{2})\u5B57?\u7B2C?(\d{6,8})\u53F7?$";
    }
}
