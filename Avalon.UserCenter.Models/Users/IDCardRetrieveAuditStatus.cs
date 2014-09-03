using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public enum IDCardRetrieveAuditStatus
    {
        /// <summary>
        /// 提交
        /// </summary>
        Submit=10,
        /// <summary>
        /// 拒绝
        /// </summary>
        Deny= 50,
        /// <summary>
        /// 通过
        /// </summary>
        Pass = 100 
    }
    /// <summary>
    /// 通知状态
    /// </summary>
    public enum AuditNotifyStatus
    {
        /// <summary>
        /// 短信
        /// </summary>
        Sms = 2,
        /// <summary>
        /// 邮箱
        /// </summary>
        Mail = 4,
        /// <summary>
        /// 短信和邮箱
        /// </summary>
        SmsAndMail = 6
    }
}
