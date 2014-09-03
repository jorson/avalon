using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    /// <summary>
    /// 新浪微博的第三方客户端实现
    /// </summary>
    public class SinaWeiboClient : ThirdOAuthClient
    {
        public SinaWeiboClient(ThirdOAuthSetting setting)
            : base(setting)
        {
        }

        const string AuthUrl = "https://api.weibo.com/oauth2/authorize";

        const string TokenUrl = "https://api.weibo.com/oauth2/access_token";

        public override string GetForceAuthorizeUrl(string redirect_uri)
        {
            redirect_uri = HttpUtility.UrlEncode(redirect_uri);
            return string.Format("{0}?redirect_uri={1}&client_id={2}&forcelogin=true", AuthUrl, redirect_uri, Setting.AppKey);
        }

        public override string GetAuthorizeUrl(string redirect_uri)
        {
            redirect_uri = HttpUtility.UrlEncode(redirect_uri);
            return string.Format("{0}?redirect_uri={1}&client_id={2}", AuthUrl, redirect_uri, Setting.AppKey);
        }

        public class TokenAndOpenId : IThirdTokenInfo
        {
            [JsonProperty("access_token")]
            public string Token { get; set; }

            [JsonProperty("uid")]
            public string OpenId { get; set; }

        }

        public override IThirdTokenInfo GetThirdToken(string code, string redirect_uri)
        {
            try
            {
                var nvs = new NameValueCollection();
                nvs["client_id"] = Setting.AppKey;
                nvs["client_secret"] = Setting.AppSecret;
                nvs["grant_type"] = "authorization_code";
                nvs["code"] = code;
                nvs["redirect_uri"] = redirect_uri;
                var jsonText = HttpClient.HttpPost(TokenUrl, nvs);
                return JsonConverter.FromJson<TokenAndOpenId>(jsonText);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public override IThirdAccountInfo GeThirdAccountInfo(IThirdTokenInfo tokenAndOpenId)
        {
            var tokenClient = new SinaWeiboTokenClient(tokenAndOpenId.Token,
                new SinaWeiboTokenClient.ThirdAppKeyAndOpenId { AppKey = Setting.AppKey, OpenId = tokenAndOpenId.OpenId });
            return tokenClient.ThirdAccountInfo;
        }
    }

    /// <summary>
    /// 新浪微博的第三方token客户端实现
    /// </summary>
    public class SinaWeiboTokenClient : ThirdTokenClient
    {
        const string TokenInfoUrl = "https://api.weibo.com/oauth2/get_token_info";
        const string AccountInfoUrl = "https://api.weibo.com/2/users/show.json";

        public class ThirdAppKeyAndOpenId : IThirdAppKeyAndOpenId
        {
            [JsonProperty("uid")]
            public string OpenId { get; set; }
            [JsonProperty("appkey")]
            public string AppKey { get; set; }
        }

        public class AccountInfo : IThirdAccountInfo
        {
            private string _nickName;

            [JsonProperty("screen_name")]
            public string NickName
            {
                get { return _nickName != null ? _nickName.Trim() : null; }
                set { _nickName = value; }
            }
        }


        public SinaWeiboTokenClient(string token, IThirdAppKeyAndOpenId appKeyAndOpenId = null)
            : base(token)
        {
            _appKeyAndOpenId = appKeyAndOpenId;
        }

        private IThirdAppKeyAndOpenId _appKeyAndOpenId;

        private IThirdAccountInfo _accountInfo;


        public override IThirdAppKeyAndOpenId AppKeyAndOpenId
        {
            get
            {
                if (_appKeyAndOpenId == null)
                {
                    try
                    {
                        var jsonText = HttpClient.HttpPost(TokenInfoUrl, "access_token", Token);
                        _appKeyAndOpenId = JsonConverter.FromJson<ThirdAppKeyAndOpenId>(jsonText);
                    }
                    catch (Exception)
                    {

                    }
                }
                return _appKeyAndOpenId;
            }
        }

        public override IThirdAccountInfo ThirdAccountInfo
        {
            get
            {
                if (_accountInfo == null)
                {
                    try
                    {
                        var url = string.Format("{0}?access_token={1}&uid={2}", AccountInfoUrl, Token, AppKeyAndOpenId.OpenId);
                        var jsonText = HttpClient.HttpGet(url);
                        _accountInfo = JsonConverter.FromJson<AccountInfo>(jsonText);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }

                return _accountInfo;
            }
        }
    }
}
