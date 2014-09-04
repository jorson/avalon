using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public class ValidResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public long UserId { get; set; }

        public object Data { get; set; }
    }
}
