using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public class AuthorizeUserCenterRequest : AuthorizeRequestBase
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string Solution { get; private set; }

        public string SessionId { get; private set; }

        public string VerifyCode { get; private set; }

        public override AccountType AccountType
        {
            get { return AccountType.UserCenter; }
        }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);
            UserName = MessageUtil.GetString(request, Protocal.username);
            Password = MessageUtil.TryGetString(request, Protocal.password);

            Solution = MessageUtil.TryGetString(request, Protocal.solution);
            SessionId = MessageUtil.TryGetString(request, Protocal.sessionid);
            VerifyCode = MessageUtil.TryGetString(request, Protocal.verifycode);

        }

        public override object Authorize()
        {
            base.Authorize();

            var result = OAuthService.ValidPassword(UserName, Password, IpAddressInt, ClientId, TerminalCode, Solution, SessionId, VerifyCode);
            if (result.Code != 0)
                return result;

            return OAuthService.CreateAuthorizationCode(ClientId, result.UserId);
        }
    }
}
