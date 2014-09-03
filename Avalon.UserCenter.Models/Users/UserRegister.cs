using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class UserRegister
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 证件号，有效的身份证、军官证或回乡证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 密码，必须是密码编码函数编码过的，密码编码函数参见<see cref="md:PwdCoderMultiLangDemo" />
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 短信验证码，手机注册时，该项必须
        /// </summary>
        public string SmsVerifyCode { get; set; }

        /// <summary>
        ///图片验证会话标识
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 图片验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 激活邮件显示名称
        /// </summary>
        public string EmailActiveDisplayName { get; set; }

        /// <summary>
        /// 激活邮件标题
        /// </summary>
        public string EmailActiveSubject { get; set; }

        /// <summary>
        /// 激活邮箱模板，邮箱注册使用，不填不发， 激活码模板参数: $verify$
        /// </summary>
        public string EmailActiveTemplate { get; set; }

        /// <summary>
        /// 注册者ip地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 注册者ip地址
        /// </summary>
        public long IpAddressInt { get; set; }
    }
}
