using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class SystemTokenRefreshRequest : TokenRefreshRequest
    {
        public override GrantType GrantType
        {
            get { return GrantType.SystemRefreshToken; }
        }

        public override object Token()
        {
            ValidClient();

            AccessGrant accessGrant = OAuthService.GetAccessGrant(RefreshToken);

            if (accessGrant == null)
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "refresh token invalid", 400);

            if (accessGrant.IsRefreshExpire())
            {
                OAuthService.DeleteAccessGrant(accessGrant);
                OAuthError(AccessTokenRequestErrorCodes.InvalidRequest, "refresh token expire", 400);
            }

            var refreshedToken = new AccessGrant(accessGrant.ClientId, accessGrant.ClientCode, accessGrant.UserId)
            {
                Scope = accessGrant.Scope,
                GrantType = accessGrant.GrantType
            };
            OAuthService.CreateAccessGrant(refreshedToken);
            //不删除原来的

            return refreshedToken;
        }
    }
}
