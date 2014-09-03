using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class UserLoginLogFilter
    {
        public long UserId { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
