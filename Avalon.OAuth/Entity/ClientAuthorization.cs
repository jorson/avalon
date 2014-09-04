using Avalon.Framework;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Avalon.OAuth
{
    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientAuthorization : ILifecycle, IValidatable
    {
        /// <summary>
        /// 客户端标识
        /// </summary>
        public virtual int ClientId { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 客户端描述
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public virtual string Secret { get; set; }

        /// <summary>
        /// 授权范围
        /// </summary>
        public virtual IList<string> Scopes { get; set; }

        /// <summary>
        /// 授权状态
        /// </summary>
        public virtual ClientAuthorizeStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 验证码出现的规则
        /// </summary>
        public virtual VerifyCodeType VerifyCodeType { get; set; }

        /// <summary>
        /// 回调的路径
        /// </summary>
        public virtual string RedirectPath { get; set; }

        public virtual void ValidRedirectUri(Uri redirectUri)
        {
            var rpUri = new Uri(RedirectPath);
            if (!String.Equals(redirectUri.AbsoluteUri, rpUri.AbsoluteUri, StringComparison.InvariantCulture))
                throw new OAuthException(AuthorizationRequestErrorCodes.RedirectUriMismatch, "redirect uri mismatch.", 400);
        }

        void ILifecycle.OnLoaded()
        {

        }

        void ILifecycle.OnSaved()
        {

        }

        void ILifecycle.OnSaving(bool creating)
        {
            if (creating)
            {
                this.Secret = Guid.NewGuid().ToString("N");
                this.CreateTime = NetworkTime.Now;
            }
            this.UpdateTime = NetworkTime.Now;
        }

        void IValidatable.Validate()
        {
            Arguments.NotNullOrWhiteSpace(Name, "Name");
            Name = Name.Trim();
            Arguments.That(Name.Length <= 50, "Name", "名称不能多于{0}个字", 50);

            if (!string.IsNullOrEmpty(Description))
            {
                Description = Description.Trim();
                Arguments.That(Description.Length <= 100, "Description", "描述不能多于{0}个字", 100);
            }

            if (!string.IsNullOrEmpty(Remark))
            {
                Remark = Remark.Trim();
                Arguments.That(Remark.Length <= 200, "Remark", "备注不能多于{0}个字", 200);
            }

            if (!string.IsNullOrEmpty(RedirectPath))
            {
                RedirectPath = RedirectPath.Trim();
                Arguments.That(RedirectPath.Length <= 200, "RedirectPath", "回调地址不能多于{0}个字", 200);

                Arguments.That(Regex.IsMatch(RedirectPath, RegexPattern.Url), "RedirectPath", "回调地址不能多于{0}个字");

            }
        }
    }

    public class AppAdmin : ILifecycle, IValidatable
    {
        protected AppAdmin()
        {

        }

        public AppAdmin(long userId, ISet<CustomAppInfo> apps)
        {
            this.UserId = userId;
            this.AppIdList = apps;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 管理员Id
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 分配应用的列表
        /// </summary>
        public virtual ISet<CustomAppInfo> AppIdList { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Description { get; set; }

        public virtual void Validate()
        {

        }

        public virtual void OnLoaded()
        {

        }

        public virtual void OnSaved()
        {

        }

        public virtual void OnSaving(bool creating)
        {
            if (creating)
                CreateTime = NetworkTime.Now;
        }
    }

    public class CustomAppInfo : ICloneable
    {
        public CustomAppInfo()
        {

        }

        /// <summary>
        /// 自定义字段：应用标识
        /// </summary>
        public int AppId { get; set; }

        public object Clone()
        {
            return new CustomAppInfo()
            {
                AppId = AppId
            };
        }

        public override int GetHashCode()
        {
            return AppId;
        }

        public override bool Equals(object obj)
        {
            var inObj = obj as CustomAppInfo;
            if (inObj != null)
                return inObj.AppId == AppId;

            return false;
        }
    }
}
