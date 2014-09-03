using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class AccountResult
    {
        public long Id { get; set; }
        public string Account { get; set; }
        public string Solution { get; set; }
    }

    public class ThirdAccountResult
    {
        public long Id { get; set; }
        public string NickName { get; set; }
        public string SolutionCode { get; set; }
        public SolutionType SolutionType { get; set; }
    }
}
