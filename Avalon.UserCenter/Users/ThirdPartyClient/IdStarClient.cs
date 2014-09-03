using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Avalon.HttpClient;
using Avalon.Utility;

namespace Avalon.UserCenter
{
    public class IdStarClient
    {

        private static class PwdUtil
        {
            static PwdUtil()
            {
                rsa.FromXmlString(@"
<RSAKeyValue>
<Modulus>0Ro3KGm3Rgq/P8Lsl4CtDot1fyH128zXe6LrMdlNvWdkTpJDbO43qobOJt7LaJrbKXuzinnjfqJOXQZ7QhxcYw==</Modulus>
<Exponent>AQAB</Exponent>
</RSAKeyValue>
");
            }

            private readonly static System.Security.Cryptography.RSACryptoServiceProvider rsa = new System.Security.Cryptography.RSACryptoServiceProvider(512);

            public static string EncryptPassword(string password)
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var dBytes = rsa.Encrypt(bytes, false);
                return System.Convert.ToBase64String(dBytes);
            }
        }

        public IdStarClient(Solution solution)
        {
            BaseUrlPath = solution.Settings.GetValue(BaseUrlPathKey);
            if(BaseUrlPath.IsNullOrEmpty())
                throw new AvalonException("IdStarClient的帐号方案SvrPath是必须配置项");
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

        protected const string LoginPath = "/ProxyUser.ashx";

        protected string BaseUrlPath { get; private set; }
        private string BaseHost { get; set; }


        public bool AuthPassword(string account, string password, Action<string> errorAction)
        {
            var url = UriPath.Combine(BaseUrlPath, LoginPath);
            try
            {
                var data = new NameValueCollection();
                data.Add("user", account);
                var epwd = PwdUtil.EncryptPassword(password);
                data.Add("pwd", epwd);
                UpdateClient();
                var rsStr = apiHttpClient.HttpPost(url, data);
                if (!string.IsNullOrEmpty(rsStr))
                {
                    var idx = rsStr.IndexOf(":", System.StringComparison.Ordinal);
                    if (idx != -1)
                    {
                        var msg = rsStr.Substring(idx+1);
                        errorAction(msg);
                    }
                    else
                    {
                        return rsStr.Equals("1");
                    }
                }
            }
            catch (Exception ex)
            {
                errorAction(ex.Message);
            }

            return false;
        }
    }
}
