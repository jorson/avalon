using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class User : IValidatable, ILifecycle
    {
        /// <summary>
        /// 用户登录名缓存键
        /// </summary>
        public const string CacheRegionLoginName = "user:LoginName";
        /// <summary>
        /// 用户登录邮箱缓存键
        /// </summary>
        public const string CacheRegionLoginEmail = "user:LoginEmail";
        /// <summary>
        /// 用户登录手机缓存键
        /// </summary>
        public const string CacheRegionLoginMobile = "user:LoginMobile";
        /// <summary>
        /// 用户证件缓存键
        /// </summary>
        public const string CacheRegionIDCard = "user:IDCard";

        public User(DateTime createTime)
        {
            CreateTime = createTime;
        }

        protected User()
        {

        }

        private string idCard = null;
        /// <summary>
        /// 标识
        /// </summary>
        public virtual long Id { get; protected set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public virtual string LoginName { get; set; }

        /// <summary>
        /// 是否有登录用户名
        /// </summary>
        public virtual bool HasLoginName {
            get { return !LoginName.IsNullOrEmpty(); }
        }

        /// <summary>
        /// 登录邮箱
        /// </summary>
        public virtual string LoginEmail { get; set; }

        /// <summary>
        /// 是否有登录邮箱
        /// </summary>
        public virtual bool HasLoginEmail
        {
            get { return !LoginEmail.IsNullOrEmpty(); }
        }

        /// <summary>
        /// 登录手机
        /// </summary>
        public virtual string LoginMobile { get; set; }

        /// <summary>
        /// 是否有登录手机
        /// </summary>
        public virtual bool HasLoginMobile
        {
            get { return !LoginMobile.IsNullOrEmpty(); }
        }

        /// <summary>
        /// 存储的证件号
        /// </summary>
        public virtual string IDCard
        {
            get { return idCard; }
            protected set { idCard = value; }
        }

        /// <summary>
        /// 证件字符一致
        /// </summary>
        /// <param name="idCard">输入的证件</param>
        /// <returns></returns>
        public virtual bool IDCardEquals(string idCard)
        {
            var storeIDCard = UserValid.GetIdCardStoreStr(idCard);
            return string.Equals(IDCard, storeIDCard, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 是否有证件号
        /// </summary>
        public virtual bool HasIDCard
        {
            get { return !IDCard.IsNullOrEmpty(); }
        }

        /// <summary>
        /// 显示证件号
        /// </summary>
        /// <returns></returns>
        public virtual string DisplayIDCard
        {
            get
            {
                return UserValid.GetIdCardDisplayStr(IDCard);
            }
            
        }

        /// <summary>
        /// 设置证件号
        /// </summary>
        /// <param name="idCard">证件号</param>
        public virtual void SetIDCard(string idCard)
        {
            this.idCard = UserValid.GetIdCardStoreStr(idCard);
        }

        /// <summary>
        /// 显示名称，顺序 用户名、邮箱、手机、证件号
        /// </summary>
        public virtual string DisplayName
        {
            get { return GetDisplayName(LoginName, LoginEmail, LoginMobile, DisplayIDCard); }
        }

        public virtual string DisplayNameWithCover
        {
            get { return GetDisPlayNameWithCover(LoginName, LoginEmail, LoginMobile, DisplayIDCard); }
        }

        /// <summary>
        /// 获取显示名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public static string GetDisplayName(string name, string email, string mobile, string idcard)
        {
            if (!name.IsNullOrEmpty())
                return name;

            if (!email.IsNullOrEmpty())
                return email;

            if (!mobile.IsNullOrEmpty())
                return mobile;

            if (!idcard.IsNullOrEmpty())
                return idcard;

            return string.Empty;
        }

        //获取显示名称（带*）
        public static string GetDisPlayNameWithCover(string name, string email, string mobile, string idcard)
        {
            if (!name.IsNullOrEmpty())
                return name;

            if (!email.IsNullOrEmpty())
                return ShieldUtil.Email(email);

            if (!mobile.IsNullOrEmpty())
                return ShieldUtil.Mobile(mobile);

            if (!idcard.IsNullOrEmpty())
                return ShieldUtil.IdCard(idcard);

            return string.Empty;
        }

        string password;
        /// <summary>
        /// 密码(已加密)
        /// </summary>
        public virtual string Password
        {
            get
            {
                return password ?? string.Empty;
            }
            protected set
            {
                password = value;
            }
        }

        /// <summary>
        /// 头像对象id
        /// </summary>
        public virtual int AvatarObjectId { get; set; }

        /// <summary>
        /// 是否验证登录手机
        /// </summary>
        public virtual bool IsVerifyLoginMobile { get; set; }

        /// <summary>
        /// 登录邮箱是否验证
        /// </summary>
        public virtual bool IsVerifyLoginEmail { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public virtual UserStatus Status { get; set; }

        /// <summary>
        /// 用户更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; protected set; }

        /// <summary>
        /// 冻结到期时间
        /// </summary>
        public virtual DateTime FrozenExpire { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; protected set; }


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

        public static string EncryptPassword(string notEncryptPassword, User user)
        {
            var encryptPwd = AvalonPassportCoder.EncryptPassword(notEncryptPassword + user.CreateTime.ToString("yyyyMMddHHmmss"));
            return encryptPwd;
        }

        void IValidatable.Validate()
        {

        }

        void ILifecycle.OnLoaded()
        {

        }

        void ILifecycle.OnSaved()
        {

        }

        void ILifecycle.OnSaving(bool creating)
        {
            var now = NetworkTime.Now; ;
            UpdateTime = now;
            if (creating)
                FrozenExpire = NetworkTime.Null;
        }
    }

    public class UserId
    {
        public long Id { get; set; }
        public string Key { get; set; }
    }

    /// <summary>
    ///用户旧密码
    /// </summary>
    public class UserOldPassword
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 用户旧加密的密码
        /// </summary>
        public virtual string Password { get; set; }
    }
}
