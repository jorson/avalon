using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Test.Service.Filters
{
    /// <summary>
    /// 用户的筛选条件类
    /// </summary>
    public class UserFilter
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 枚举筛选条件
        /// </summary>
        public int EnumValue { get; set; }
    }
}
