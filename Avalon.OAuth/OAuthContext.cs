using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class OAuthContext
    {
        const string OAuthContextKey = "__oauthcontext__";
        AccessGrant accessGrant = null;

        protected OAuthContext(AccessGrant accessGrant)
        {
            this.accessGrant = accessGrant;
        }

        public static OAuthContext Current
        {
            get
            {
                var oauthContext = (OAuthContext)Workbench.Current.Items[OAuthContextKey];
                if (oauthContext == null)
                {
                    oauthContext = new OAuthContext(null);
                    Workbench.Current.Items[OAuthContextKey] = oauthContext;
                }
                return oauthContext;
            }
        }

        public bool IsAuth
        {
            get { return accessGrant != null; }
        }

        public int OAuthAppId
        {
            get
            {
                EnsourceOAuth();
                return accessGrant.ClientId;
            }
        }

        public int OAuthAppCode
        {
            get
            {
                EnsourceOAuth();
                return accessGrant.ClientCode;
            }
        }

        public long UserId
        {
            get
            {
                EnsourceOAuth();
                return accessGrant.UserId;
            }
        }

        public string AccessToken
        {
            get
            {
                EnsourceOAuth();
                return accessGrant.AccessToken;
            }
        }

        public AccessGrant AccessGrant
        {
            get { return accessGrant; }
        }

        protected void EnsourceOAuth()
        {
            if (accessGrant == null)
                throw new OAuthException("invalid token", "null", 403000);
        }

        public static void OnAuth(AccessGrant accessGrant)
        {
            Arguments.NotNull(accessGrant, "accessGrant");
            Workbench.Current.Items[OAuthContextKey] = new OAuthContext(accessGrant);
        }
    }
}
