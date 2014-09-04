using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avalon.OAuth
{
    public abstract class TokenLoginRequest : TokenRequestBase
    {
        public string IpAddress { get; private set; }

        public long IpAddressInt { get; private set; }

        public override void Parse(HttpRequestBase request)
        {
            base.Parse(request);

            IpAddress = MessageUtil.TryGetString(request, Protocal.ipaddress);

            IpAddressInt = Avalon.Utility.IpAddress.IpToInt(IpAddress);
        }
    }
}
