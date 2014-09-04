using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Avalon.OAuth
{
    public class AbstractOAuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// accesstoken 仅从query 中获取
        /// </summary>
        public bool QueryStringOnly { get; set; }

        /// <summary>
        /// 授权的范围
        /// </summary>
        public string OAuthScope { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var oauthService = Avalon.Framework.DependencyResolver.Resolve<OAuthService>();
            var accessGrant = OAuthAuthorization.ValidToken(filterContext.HttpContext, QueryStringOnly);
            OnValidSuccess(filterContext, accessGrant);
        }

        protected virtual void OnValidSuccess(AuthorizationContext filterContext, AccessGrant accessGrant)
        {

        }
    }
}
