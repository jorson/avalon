using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class TokenFailedResponse
    {
        public string Error { get; set; }

        public string ErrorDescription { get; set; }

        public Uri ErrorUri { get; set; }
    }
}
