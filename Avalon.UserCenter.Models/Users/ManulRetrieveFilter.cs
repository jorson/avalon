using Avalon.Framework.Querys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    [ODataFilter]
    public class ManulRetrieveFilter
    {
        /// <summary>
        /// 标识
        /// </summary>
        public  int Id { get; set; }
        /// <summary>
        /// 申诉帐号
        /// </summary>
        public string UserIdentity { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string UserFullName { get; set; }
        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string ContactMobile { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public IDCardRetrieveAuditStatus AuditStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public int RegistAppId { get; set; }
    }
}
