using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class ExamineMessage
    {
        private IDCardRetrieveAuditStatus _AditStatus;
        private AuditNotifyStatus _NotifyStatus;
        public int IdCardRetrieveId { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public IDCardRetrieveAuditStatus AditStatus
        {
            get { return (IDCardRetrieveAuditStatus)this.AditStatus2; }
            set { this._AditStatus = (IDCardRetrieveAuditStatus)(Convert.ToInt32(value)); }
        }
        public int AditStatus2 { get; set; }
        public int NotifyStatus2 { get; set; }
        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string DenyReason { get; set; }

        /// <summary>
        /// 通知类型
        /// </summary>
        public AuditNotifyStatus NotifyStatus {
            get { return (AuditNotifyStatus)this.NotifyStatus2; }
            set { this._NotifyStatus = (AuditNotifyStatus)(Convert.ToInt32(value)); } 
        }

        /// <summary>
        /// 邮件成功发送的消息
        /// </summary>
        public string EmailSuccessMessage { get; set; }

        /// <summary>
        /// 邮件失败发送的消息
        /// </summary>
        public string EmailFailMessage { get; set; }

        /// <summary>
        /// 手机成功发送的消息
        /// </summary>
        public string MobileSuccessMessage { get; set; }

        /// <summary>
        /// 手机失败发送的消息
        /// </summary>
        public string MobileFailMessage { get; set; }

        /// <summary>
        /// 邮件发送的标题
        /// </summary>
        public string EmailTitle { get; set; }

        /// <summary>
        /// 邮件发送者名称
        /// </summary>
        public string EmailSender { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        public long? AuditorUserId { get; set; }

        /// <summary>
        /// 审核人名称
        /// </summary>
        public string AuditorUserName { get; set; }
    }
}
