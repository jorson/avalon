using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter;
using Avalon.UserCenter.Models;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 用户帐号关系
    /// </summary>
    public class UserAccount
    {
        public UserAccount(DateTime createTime)
        {
            CreateTime = createTime;
        }

        protected UserAccount()
        {

        }

        private string _password;
        private string _nickName;

        /// <summary>
        /// 标识
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 帐号方案标识
        /// </summary>
        public virtual int SolutionId { get; set; }

        /// <summary>
        /// 帐号所属应用标识
        /// </summary>
        public virtual int AppId { get; set; }

        /// <summary>
        /// 对应帐号方案的帐号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 帐号昵称
        /// </summary>
        public virtual string NickName
        {
            get { return _nickName ?? string.Empty; }
            set { _nickName = value; }
        }

        /// <summary>
        /// 密码(已加密),第三方帐号为空字符串
        /// </summary>
        public virtual string Password
        {
            get { return _password ?? string.Empty; }
            protected set { _password = value; }
        }

        /// <summary>
        /// 绑定的用户标识
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 帐号状态
        /// </summary>
        public virtual AccountStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }
        

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 冻结到期时间
        /// </summary>
        public virtual DateTime FrozenExpire { get; set; }

        public virtual bool EqualsPassword(string notEncryptPassword)
        {
            var encryptPwd = EncryptPassword(notEncryptPassword);
            return string.Equals(Password, encryptPwd);
        }

        public virtual UserCode EqualsPasswordReturnCode(string notEncryptPassword)
        {
            var encryptPwd = EncryptPassword(notEncryptPassword);
            return string.Equals(Password, encryptPwd) ? UserCode.Success : UserCode.WrongPassword;
        }

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="notEncryptPassword">加密的密码</param>
        public virtual void SetPassword(string notEncryptPassword)
        {
            Password = EncryptPassword(notEncryptPassword);
        }

        private string EncryptPassword(string notEncryptPassword)
        {
            return EncryptPassword(notEncryptPassword, this);
        }

        public static string EncryptPassword(string notEncryptPassword, UserAccount userAccount)
        {
            var encryptPwd = AvalonPassportCoder.EncryptPassword(notEncryptPassword + userAccount.CreateTime.ToString("yyyyMMddHHmmss"));
            return encryptPwd;
        }
        
    }

    public class UserAccountCreateInfo
    {
        public virtual long UserAccountId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 注册方式
        /// </summary>
        public virtual RegisterMode RegisterMode { get; set; }

        /// <summary>
        /// 终端编号
        /// </summary>
        public virtual int TerminalCode { get; set; }

        /// <summary>
        /// 注册时的ip地址
        /// </summary>
        public virtual long IpAddress { get; set; }
        /// <summary>
        /// 注册时的IP所在城市
        /// </summary>
        public virtual int IpCityId { get; set; }

        /// <summary>
        /// 绑定的应用标识
        /// </summary>
        public virtual int AppId { get; set; }
    }
}
