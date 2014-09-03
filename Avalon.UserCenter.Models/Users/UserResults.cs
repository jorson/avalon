using System.Collections;
using System.Collections.Generic;
using Avalon.UserCenter.Models;

namespace Avalon.UserCenter.Models
{
    public class UserSimpleResult
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户头像云存储对象标识
        /// </summary>
        public int AvatarObjectId { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string AvatarUrl { get; set; }
    }

    public class UserResult
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户头像云存储对象标识
        /// </summary>
        public int AvatarObjectId { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 是否有密码
        /// </summary>
        public bool HasPassword { get; set; }
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 证件号，有效的身份证、军官证或回乡证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 密保邮箱
        /// </summary>
        public string SecurityEmail { get; set; }
        /// <summary>
        /// 密保手机号
        /// </summary>
        public string SecurityMobile { get; set; }
        /// <summary>
        /// 登录邮箱是否验证
        /// </summary>
        public bool IsVerifyLoginEmail { get; set; }
        /// <summary>
        /// 密保邮箱是否验证
        /// </summary>
        public bool SecurityEmailIsVerify { get; set; }

        /// <summary>
        /// 用户的帐号列表
        /// </summary>
        public IList<AccountResult> AccountList { get; set; }
    }
}