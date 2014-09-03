using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class WebSiteBaseSetting
    {
        protected WebSiteBaseSetting()
        {

        }

        public WebSiteBaseSetting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public virtual string Key { get;protected set; }

        public virtual string Value { get; protected set; }
        
    }

    public enum TemplateType
    {
        /// <summary>
        /// 注册验证
        /// </summary>
        RegisterValid=1,
        /// <summary>
        /// 找回密码
        /// </summary>
        ForgetPassword=2,
        /// <summary>
        /// 修改密码成功
        /// </summary>
        ChangePasswordSuccess=3,
        /// <summary>
        /// 设置/修改登录
        /// </summary>
        ChangeIdentity=4,
        /// <summary>
        /// 设置/修改密保
        /// </summary>
        ChangeSecurity=5,
        /// <summary>
        /// 解绑密保成功
        /// </summary>
        DeleteSecurity=6,
    }

    public class WebSiteEmailTemplate
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string SenderDispalyName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string BodyTemplate { get; set; }
    }

    public class ThirdAccountSetting
    {
        public bool Enable { get; set; }

        public string AppKey { get; set; }

        public string AppSecret { get; set; }
    }

}
