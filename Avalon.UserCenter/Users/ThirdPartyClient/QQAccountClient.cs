using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    /// <summary>
    /// qq帐号的第三方帐号的客户端实现
    /// </summary>
    public class QQAccountClient : ThirdOAuthClient
    {
        public QQAccountClient(ThirdOAuthSetting setting)
            : base(setting)
        {

        }

        const string AuthUrl = "https://graph.qq.com/oauth2.0/authorize";

        const string TokenUrl = "https://graph.qq.com/oauth2.0/token";

        private QQAccountTokenClient tokenClient;

        public override string GetAuthorizeUrl(string redirect_uri)
        {
            redirect_uri = HttpUtility.UrlEncode(redirect_uri);
            var url = string.Format("{0}?response_type=code&redirect_uri={1}&client_id={2}&state={3}", AuthUrl, redirect_uri, Setting.AppKey, Guid.NewGuid().ToString("N"));
            return url;
        }

        public class TokenAndOpenId : IThirdTokenInfo
        {
            public string Token { get; set; }

            [JsonProperty("openid")]
            public string OpenId { get; set; }


        }

        public override IThirdTokenInfo GetThirdToken(string code, string redirect_uri)
        {
            try
            {
                redirect_uri = HttpUtility.UrlEncode(redirect_uri);
                var tokenUrl = string.Format("{0}?grant_type=authorization_code&redirect_uri={1}&client_id={2}&client_secret={3}&code={4}"
                    , TokenUrl, redirect_uri, Setting.AppKey, Setting.AppSecret, code);
                var kvText = HttpClient.HttpGet(tokenUrl);
                var kvs = HttpUtility.ParseQueryString(kvText, Encoding.UTF8);
                var token = kvs["access_token"];
                if (token != null)
                {
                    tokenClient = new QQAccountTokenClient(token);
                    if (tokenClient.AppKeyAndOpenId != null)
                    {
                        return new TokenAndOpenId { Token = token, OpenId = tokenClient.AppKeyAndOpenId.OpenId };
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }



        public override IThirdAccountInfo GeThirdAccountInfo(IThirdTokenInfo tokenAndOpenId)
        {
            if (tokenClient == null)
                tokenClient = new QQAccountTokenClient(tokenAndOpenId.Token, new QQAccountTokenClient.ThirdAppKeyAndOpenId { AppKey = Setting.AppKey, OpenId = tokenAndOpenId.OpenId });

            return tokenClient.ThirdAccountInfo;
        }

        public override string GetForceAuthorizeUrl(string redirect_uri)
        {
            return GetAuthorizeUrl(redirect_uri);
        }
    }

    /// <summary>
    /// qq帐号的第三方帐号的token客户端实现
    /// </summary>
    public class QQAccountTokenClient : ThirdTokenClient
    {
        const string OpenIdUrl = "https://graph.qq.com/oauth2.0/me";
        const string AccountInfoUrl = "https://graph.qq.com/user/get_user_info";

        public class ThirdAppKeyAndOpenId : IThirdAppKeyAndOpenId
        {
            [JsonProperty("openid")]
            public string OpenId { get; set; }
            [JsonProperty("client_id")]
            public string AppKey { get; set; }
        }

        public class AccountInfo : IThirdAccountInfo
        {
            private string _nickName;

            [JsonProperty("ret")]
            public int Code { get; set; }

            [JsonProperty("nickname")]
            public string NickName
            {
                get { return _nickName != null ? _nickName.Trim() : null; }
                set { _nickName = value; }
            }
        }


        public QQAccountTokenClient(string token, IThirdAppKeyAndOpenId appKeyAndOpenId = null)
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
                        var openUrl = string.Format("{0}?access_token={1}", OpenIdUrl, Token);
                        var callbackText = HttpClient.HttpGet(openUrl);
                        if (callbackText.StartsWith("callback("))
                        {
                            var v = callbackText.Substring(9, callbackText.Length - 9 - 3);
                            _appKeyAndOpenId = JsonConverter.FromJson<ThirdAppKeyAndOpenId>(v);
                        }
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
                        var url = string.Format("{0}?access_token={1}&oauth_consumer_key={2}&openid={3}&format=json", AccountInfoUrl,
                        Token, AppKeyAndOpenId.AppKey, AppKeyAndOpenId.OpenId);

                        var jsonText = HttpClient.HttpGet(url);
                        var rt = JsonConverter.FromJson<AccountInfo>(jsonText);
                        if (rt != null && rt.Code == 0)
                        {
                            _accountInfo = rt;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                return _accountInfo;
            }
        }
    }
}
