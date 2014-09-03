using Avalon.Framework.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    [ODataFilter]
    public class UserAccountFilter
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 帐号名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 帐号方案标识
        /// </summary>
        public int SolutionId { get; set; }

        /// <summary>
        /// 帐号对应应用标识
        /// </summary>
        public int AppId { get; set; }
    }
}
