using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Avalon.UserCenter.Models;
using Avalon.Utility;
using Avalon.HttpClient;

namespace Avalon.UserCenter
{
    public abstract class UapClient
    {
        protected UapClient(Solution solution)
        {
            BaseUrlPath = solution.Settings.GetValue(BaseUrlPathKey);
            if(BaseUrlPath.IsNullOrEmpty())
                throw new AvalonException("UapClient的帐号方案SvrPath是必须配置项");
            BaseHost = solution.Settings.GetValue(BaseHostKey);
        }

        void UpdateClient()
        {
            if (!string.IsNullOrEmpty(BaseHost))
                apiHttpClient.Host = BaseHost;
        }

        private readonly ApiHttpClient apiHttpClient = new ApiHttpClient();

        private const string BaseUrlPathKey = "SvrPath";
        private const string BaseHostKey = "SvrHost";
        protected const string SessionCheckPath = "/passport/check";
        protected const string LoginPath = "/passport/login1";

        protected string BaseUrlPath { get; private set; }
        private string BaseHost { get; set; }

        public class UapResult
        {
            [JsonProperty("uid")]
            public string Uid { get; set; }
        }

        public class SessionCkeckModel
        {
            [JsonProperty("uap_sid")]
            public string SessionId { get; set; }
        }

        protected TResult PostJsonData<TResult>(string url, object model, Action<string> errorAction)
        {
            var rt = default(TResult);
            try
            {
                UpdateClient();
                rt = apiHttpClient.HttpPostJson<TResult>(url, model);
            }
            catch (Exception e)
            {
                var ex = e.GetBaseException() as WebException;
                if (ex != null && ex.Response != null)
                {
                    var sm = ex.Response.GetResponseStream();
                    if (sm != null)
                    {
                        using (var r = new StreamReader(sm))
                        {
                            var json = r.ReadToEnd();
                            var obj = JsonConverter.FromJson<Dictionary<string, object>>(json);
                            if (obj != null && obj.ContainsKey("msg"))
                            {
                                var msg = obj["msg"].ToString();
                                if (errorAction != null)
                                    errorAction(msg);
                            }

                        }
                    }
                }
            }
            return rt;
        }

        public abstract UapResult GetBySessionId(string sessionId, Action<string> errorAction);

        public abstract UapResult GetByPassword(string account, string password, Action<string> errorAction);
    }

    public class UapForNdClient : UapClient
    {
        public UapForNdClient(Solution solution)
            : base(solution)
        {

        }

        public class LoginModel
        {
            private string _unitCode = "nd";

            /// <summary>
            /// 网龙工号
            /// </summary>
            [JsonProperty("account")]
            public string Account { get; set; }

            /// <summary>
            /// 密码
            /// </summary>
            [JsonProperty("password")]
            public string Password { get; set; }

            /// <summary>
            /// 单位号,默认nd
            /// </summary>
            [JsonProperty("unitcode")]
            public string UnitCode
            {
                get { return _unitCode; }
            }
        }

        /// <summary>
        /// 通过uap会话标识（sid）获取uap用户信息
        /// </summary>
        /// <param name="sessionId">uap会话标识（sid）</param>
        /// <param name="errorAction">错误回调，回调返回错误消息</param>
        /// <returns></returns>
        public override UapResult GetBySessionId(string sessionId, Action<string> errorAction)
        {
            var url = UriPath.Combine(BaseUrlPath, SessionCheckPath);
            var model = new SessionCkeckModel { SessionId = sessionId };
            return PostJsonData<UapResult>(url, model, errorAction);
        }

        /// <summary>
        /// 通过网龙工号和密码获取获取uap用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="errorAction">错误回调，回调返回错误消息</param>
        /// <returns></returns>
        public override UapResult GetByPassword(string account, string password, Action<string> errorAction)
        {
            var url = UriPath.Combine(BaseUrlPath, LoginPath);
            var model = new LoginModel { Account = account, Password = password };
            return PostJsonData<UapResult>(url, model, errorAction);
        }
    }

    public class UapTokenClient : ThirdTokenClient
    {
        private readonly UapClient UapClient;

        private UapClient.UapResult result;

        public UapTokenClient(UapClient client, string token)
            : base(token)
        {
            UapClient = client;
        }

        public class ThirdAppKeyAndOpenId : IThirdAppKeyAndOpenId
        {
            public ThirdAppKeyAndOpenId(string openId)
            {
                OpenId = openId;
            }

            public string OpenId { get; private set; }
            public string AppKey { get; private set; }
        }

        public class UapThirdAccountInfo : IThirdAccountInfo
        {
            public string NickName { get; set; }
        }

        private UapClient.UapResult getUapResult()
        {
            if (result == null)
            {
                var errorMsg = string.Empty;
                result = UapClient.GetBySessionId(Token, msg => { errorMsg = msg; });
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    var ex = new AvalonException(errorMsg) {Code = (int) UserCode.UapSolutionError};
                    throw ex;
                }
            }
            return result;
        }

        public override IThirdAppKeyAndOpenId AppKeyAndOpenId
        {
            get { return new ThirdAppKeyAndOpenId(getUapResult().Uid); }
        }

        public override IThirdAccountInfo ThirdAccountInfo
        {
            get { return new UapThirdAccountInfo {NickName = AppKeyAndOpenId.OpenId}; }
        }
    }
}
