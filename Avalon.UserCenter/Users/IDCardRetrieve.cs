using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;
namespace Avalon.UserCenter
{
    public class IDCardRetrieve
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// 身份证件
        /// </summary>
        public virtual string IDCard { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 证件图片云存储对象标识
        /// </summary>
        public virtual int IDCardImgObjectId { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public virtual string AuditRemark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual IDCardRetrieveAuditStatus AuditStatus { get; set; }
        /// <summary>
        /// 审核拒绝原因
        /// </summary>
        public virtual string AuditDenyReason { get; set; }
        /// <summary>
        /// 审核操作员用户标识
        /// </summary>
        public virtual long? AuditorUserId { get; set; }
        /// <summary>
        /// 审核操作员用户名称
        /// </summary>
        public virtual string AuditorUserName { get; set; }
        /// <summary>
        /// 审核时间，未审核是值为 NetworkTime.Null
        /// </summary>
        public virtual DateTime AuditTime { get; set; }
        /// <summary>
        /// 客户提交的联系邮箱，用户审核通过后设置用户密保邮箱
        /// </summary>
        public virtual string ContactEmail { get; set; }
        /// <summary>
        /// 客户提交的联系手机，用户审核通过后设置用户密保手机
        /// </summary>
        public virtual string ContactMobile { get; set; }
        /// <summary>
        /// 邮件通知云存储作业标识
        /// </summary>
        public virtual int? EmailNofiyJobId { get; set; }
        /// <summary>
        /// 手机短信通知云存储作业标识
        /// </summary>
        public virtual int? MobileNofiyJobId { get; set; }

        /// <summary>
        /// 创建的应用标识
        /// </summary>
        public virtual int CreatAppId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string UserFullName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// 通知状态：2短信，4邮箱，6短信加邮箱
        /// </summary>
        public virtual AuditNotifyStatus NofiyFlag { get; set; }

        /// <summary>
        /// 注册时的应用标识
        /// </summary>
        public virtual int RegistAppId { get; set; }
    }
}
