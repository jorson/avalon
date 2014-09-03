using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Avalon.UserCenter.Models;

namespace Avalon.UserCenter
{
    public class UserValid
    {
        public static readonly Regex UserNameRegex = new Regex(UserRegexString.UserName);
        public static readonly Regex EmailRegex = new Regex(UserRegexString.Email);
        public static readonly Regex MobileRegex = new Regex(UserRegexString.Mobile);
        public static readonly Regex IDCardRegex = new Regex(UserRegexString.IDCard);
        public static readonly Regex IDCard15BitRegex = new Regex(UserRegexString.IDCard15Bit);
        public static readonly Regex HMIDCardRegex = new Regex(UserRegexString.HMIDCard);
        public static readonly Regex ArmyIDCardRegex = new Regex(UserRegexString.ArmyIDCard);

        /// <summary>
        /// 验证用户名格式
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>注册代码</returns>
        public static UserCode ValidUserNameFormat(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                if (userName.Length < 6 || userName.Length > 20)
                {
                    return UserCode.InvalidUserNameLength;
                }

                if (!UserNameRegex.IsMatch(userName))
                {
                    return UserCode.InvalidUserName;
                }
                else if (HMIDCardRegex.IsMatch(userName))
                {
                    return UserCode.InvalidUserName;
                }
            }

            return UserCode.Success;
        }

        /// <summary>
        /// 验证邮箱格式
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns>注册代码</returns>
        public static UserCode ValidEmailFormat(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Length > 60)
                {
                    return UserCode.InvalidEmailLength;
                }

                if (!EmailRegex.IsMatch(email))
                {
                    return UserCode.InvalidEmail;
                }
            }

            return UserCode.Success;
        }

        private static long IdCardWeight(long w)
        {
            return ((long)Math.Pow(2, w - 1)) % 11;
        }

        /// <summary>
        /// 校验身份证校验码
        /// </summary>
        /// <param name="idCard">身份证</param>
        /// <returns></returns>
        private static bool IdCardLastValEq(string idCard)
        {
            if (idCard.Length != 18)
                return false;
            var date = idCard.Substring(6, 8);
            try
            {
                DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.CurrentCulture);
            }
            catch
            {
                return false;
            }
            var idCard17 = idCard.Substring(0, 17);
            var idCardLast = idCard.Substring(17, 1).ToLower();
            var words = idCard17.Select(a => a.ToString()).Reverse().ToArray();
            long sum = words.Select((t, j) => long.Parse(t) * IdCardWeight(j + 2)).Sum();
            var checkVal = (int)(12 - (sum % 11)) % 11;
            var ckValStr = checkVal == 10 ? "x" : string.Format("{0:d}", checkVal);
            return ckValStr.Equals(idCardLast);
        }

        /// <summary>
        /// 验证身份证件格式
        /// </summary>
        /// <param name="idCard">身份证件</param>
        /// <returns>注册代码</returns>
        public static UserCode ValidIDCardFormat(string idCard)
        {
            if (!string.IsNullOrWhiteSpace(idCard))
            {
                if (!HMIDCardRegex.IsMatch(idCard) && !ArmyIDCardRegex.IsMatch(idCard))
                {
                    if (!IDCardRegex.IsMatch(idCard))
                    {
                        return UserCode.InvalidIDCard;
                    }

                    if (!IdCardLastValEq(idCard))
                    {
                        return UserCode.InvalidIDCard;
                    }
                }
            }

            return UserCode.Success;
        }

        /// <summary>
        /// 根据用户标识返回用户标识类型
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static UserIdentityType GetIdentityType(string identity)
        {
            if (MobileRegex.IsMatch(identity)) return UserIdentityType.Mobile;
            if (EmailRegex.IsMatch(identity)) return UserIdentityType.Email;
            if (IDCardRegex.IsMatch(identity)) return UserIdentityType.IDCard;
            if (IDCard15BitRegex.IsMatch(identity)) return UserIdentityType.IDCard;
            if (HMIDCardRegex.IsMatch(identity)) return UserIdentityType.IDCard;
            if (ArmyIDCardRegex.IsMatch(identity)) return UserIdentityType.IDCard;
            return UserValid.UserNameRegex.IsMatch(identity) ? UserIdentityType.UserName : UserIdentityType.Unknown;
        }

        /// <summary>
        /// 根据证件号返回证件好类型
        /// </summary>
        /// <param name="idCard">证件号</param>
        /// <returns>证件好类型</returns>
        public static IdCardType GetIdCardType(string idCard)
        {
            if (!string.IsNullOrEmpty(idCard))
            {
                if (HMIDCardRegex.IsMatch(idCard)) return IdCardType.Hm;
                if (ArmyIDCardRegex.IsMatch(idCard)) return IdCardType.Army;
                if (IDCardRegex.IsMatch(idCard)) return IdCardType.Id;
                if (IDCard15BitRegex.IsMatch(idCard)) return IdCardType.Id;
            }

            return IdCardType.Unknown;
        }

        /// <summary>
        /// 获取证件号存储的字符串
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string GetIdCardStoreStr(string idCard)
        {
            var rt = idCard;
            var idCardType = GetIdCardType(idCard);
            switch (idCardType)
            {
                  case IdCardType.Id:
                    rt = idCard.ToUpper();
                    break;
                  case IdCardType.Hm:
                    rt=HMIDCardRegex.Replace(idCard, "$1$2").ToUpper();
                    break;
                  case IdCardType.Army:
                    rt = ArmyIDCardRegex.Replace(idCard, "$1$2").ToUpper();
                    break;
                default:
                    break;
            }
            return rt;
        }

        /// <summary>
        /// 获取证件号显示字符串
        /// </summary>
        /// <param name="idCardStoreStr">证件号存储的字符串</param>
        /// <returns></returns>
        public static string GetIdCardDisplayStr(string idCardStoreStr)
        {
            var rt = idCardStoreStr;
            var idCardType = GetIdCardType(idCardStoreStr);
            switch (idCardType)
            {
                case IdCardType.Id:
                    rt = idCardStoreStr.ToUpper();
                    break;
                case IdCardType.Hm:
                    rt = HMIDCardRegex.Replace(idCardStoreStr, "$1$2").ToUpper();
                    break;
                case IdCardType.Army:
                    rt = ArmyIDCardRegex.Replace(idCardStoreStr, "$1字第$2号").ToUpper();
                    break;
                default:
                    break;
            }
            return rt;
        }


        /// <summary>
        /// 验证手机号格式
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>注册代码</returns>
        public static UserCode ValidMobileFormat(string mobile)
        {
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                if (mobile.Length != 11)
                {
                    return UserCode.InvalidMobileLength;
                }

                if (!MobileRegex.IsMatch(mobile))
                {
                    return UserCode.InvalidMobile;
                }
            }

            return UserCode.Success;
        }

        /// <summary>
        /// 验证密码长度
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserCode ValidPasswordFormat(string password)
        {
            if (password.IsNullOrWhiteSpace())
                return UserCode.EmptyPassword;
            if (password.Length >= 6 && password.Length <= 20)
                return UserCode.Success;
            return UserCode.InvalidPasswordLength;
        }

        public static UserCode ValidAccountPasswordFormat(string password, Solution solution)
        {
            if (password.IsNullOrWhiteSpace())
                return UserCode.EmptyPassword;
            if (password.Length >= solution.MinPasswordLength && password.Length <= solution.MaxPasswordLength)
                return UserCode.Success;
            return UserCode.InvalidCustomPasswordLength;
        }

        public static UserCode ValidUserFullNameFormat(string fullName)
        {
            if (fullName.Length < 2 || fullName.Length > 20)
                return UserCode.InvalidUserFullNameLength;

            return UserCode.Success;
        }

        public static UserCode ValidUserExEmailAndMobile(User user, UserSecurity userSecurity)
        {
            if (user != null)
            {
                if (!user.LoginEmail.IsNullOrWhiteSpace() || !user.LoginMobile.IsNullOrWhiteSpace())
                    return UserCode.Success;
            }
            if (userSecurity != null)
            {
                if (!userSecurity.Email.IsNullOrWhiteSpace() || !userSecurity.Mobile.IsNullOrWhiteSpace())
                    return UserCode.Success;
            }
            return UserCode.SecurityLow;
        }
    }

    /// <summary>
    /// 用户标识类型
    /// </summary>
    public enum UserIdentityType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知登录")]
        Unknown = 0,
        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        UserName = 1,
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        Mobile = 2,
        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        Email = 3,
        /// <summary>
        /// 证件号
        /// </summary>
        [Description("证件号")]
        IDCard = 4,
        /// <summary>
        /// 帐号方案
        /// </summary>
        [Description("帐号方案")]
        Solution = 5,
    }

    public enum IdCardType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 身份证
        /// </summary>
        Id = 1 ,
        /// <summary>
        /// 回乡证
        /// </summary>
        Hm = 2 ,
        /// <summary>
        /// 军官证
        /// </summary>
        Army = 3,
    }
}
