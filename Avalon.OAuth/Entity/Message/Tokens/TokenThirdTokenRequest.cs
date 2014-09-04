using Avalon.Framework;
using Avalon.UserCenter;
using Avalon.UserCenter.Models;
using Avalon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
	public class TokenThirdTokenRequest : TokenLoginRequest
    {
		ThirdPartyService thirdPartyService;
		UserService userService;

		public TokenThirdTokenRequest():base()
		{
			this.thirdPartyService = DependencyResolver.Resolve<ThirdPartyService>();
			this.userService = DependencyResolver.Resolve<UserService>();
		}

		public string ThirdToken { get; set; }

		public string Solution { get; set; }

        public override GrantType GrantType
        {
			get { return GrantType.ThirdToken; }
        }			

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            ThirdToken = MessageUtil.GetString(request, "third_token");
            Solution = MessageUtil.GetString(request, "solution");
        }

        public override object Token()
        {
            ValidClient();

			var solutionModel = userService.GetSolutionByCode (Solution);
            ThirdTokenClient thirdTokenClient = null;
            ResultWrapper<UserCode, UserAccount> rt = null;
            var validor = new Validor<UserCode>(UserCode.Success);
            validor.AppendValidAndSet(solutionModel.NeedThirdSolution);
            validor.AppendValidAndSet(() =>
            {
                thirdTokenClient = thirdPartyService.GetThirdTokenClient(solutionModel, ThirdToken);
			    var accountInfo = thirdTokenClient.ThirdAccountInfo;
                return accountInfo == null ? UserCode.InvalidThirdToken : UserCode.Success;
            });

            validor.AppendValidAndSet(() =>
            {
                rt=userService.TryThirdLogin(thirdTokenClient.AppKeyAndOpenId.OpenId, thirdTokenClient.ThirdAccountInfo.NickName, solutionModel, IpAddressInt, ClientId, TerminalCode);
                return rt.Code;
            });

            var code = validor.Valid();

            if (code != UserCode.Success)
                OAuthError(code.ToString(), code.GetDescription(), (int)code);

            return OAuthService.CreateAccessGrant(ClientId, ClientCode, rt.Data.UserId, TerminalCode);
        }
    }
}
