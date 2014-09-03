using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class UserLoginLog
    {
        public virtual long Id { get; set; }
        public virtual long UserId { get; set; }
        public virtual UserIdentityType IdentityType { get; set; }
        public virtual int SolutionId { get; set; }
        public virtual DateTime LoginTime { get; set; }
        public virtual int AppId { get; set; }
        public virtual int TerminalCode { get; set; }
        public virtual long IpAddress { get; set; }
        /// <summary>
        /// 登陆时的IP所在城市
        /// </summary>
        public virtual int IpCityId { get; set; }
        public virtual int IpCityIdNum { get; set; }
    }
}
