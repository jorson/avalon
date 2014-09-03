using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;
using Avalon.Framework;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class UserSecurity : ILifecycle
    {
        protected UserSecurity()
        {

        }

        public UserSecurity(long userId):this()
        {
            UserId = userId;
        }

        public virtual long UserId { get; internal protected set; }

        /// <summary>
        /// 密保邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 邮箱是否验证
        /// </summary>
        public virtual bool IsVerifyEmail { get; set; }

        /// <summary>
        /// 密保手机
        /// </summary>
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        void ILifecycle.OnLoaded()
        {
            
        }

        void ILifecycle.OnSaved()
        {
            
        }

        void ILifecycle.OnSaving(bool creating)
        {
            UpdateTime = NetworkTime.Now;
        }

        public virtual UserCode IsBindSecurityEmail()
        {
            return Email.IsNullOrWhiteSpace() ? UserCode.Success : UserCode.SecurityEmailExist;
        }

        public virtual UserCode IsBindSecurityMobile()
        {
            return Mobile.IsNullOrWhiteSpace() ? UserCode.Success : UserCode.SecurityMobileExist;
        }
    }
}
