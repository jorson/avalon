using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.Framework.Querys;

namespace Avalon.UserCenter.Models
{
    [ODataFilter]
    public class SolutionFilter
    {
        /// <summary>
        /// 标识
        /// </summary>
        public  int Id { get; set; }
        /// <summary>
        /// 应用标识
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 方案类型
        /// </summary>
        public virtual SolutionType Type { get; set; }
    }
}
