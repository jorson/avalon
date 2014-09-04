using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class TokenPasswordUserCenterRequest : TokenLoginRequest
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Solution { get; private set; }

        public string SessionId { get; private set; }

        public string VerifyCode { get; private set; }

        private bool IsShowCode;

        public override GrantType GrantType
        {
            get { return GrantType.Password; }
        }

        public AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtil.GetString(request, Protocal.username);
            Password = MessageUtil.GetString(request, Protocal.password);
            Solution = MessageUtil.TryGetString(request, Protocal.solution);
            SessionId = MessageUtil.TryGetString(request, Protocal.sessionid);
            VerifyCode = MessageUtil.TryGetString(request, Protocal.verifycode);
#if DEBUG
            bool.TryParse(request["__showcode__"], out IsShowCode);
#endif
        }

        public override object Token()
        {
            ValidClient();

            var result = OAuthService.ValidPassword(UserName, Password, IpAddressInt, ClientId, TerminalCode, Solution, SessionId, VerifyCode);
            if (result.Code != 0)
                return result;

            return OAuthService.CreateAccessGrant(ClientId, ClientCode, result.UserId, TerminalCode);
        }
    }
}
