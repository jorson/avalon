using Avalon.UserCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public class ManulRetrieve
    {
        /// <summary>
        /// 标识
        /// </summary>
        public virtual int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long UserId { get; set; }
        /// <summary>
        /// 申诉帐号
        /// </summary>
        public virtual string UserIdentity { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string UserFullName { get; set; }
        /// <summary>
        /// 客户提交的联系邮箱，用户审核通过后设置用户密保邮箱
        /// </summary>
        public virtual string ContactEmail { get; set; }
        /// <summary>
        /// 客户提交的联系手机，用户审核通过后设置用户密保手机
        /// </summary>
        public virtual string ContactMobile { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual IDCardRetrieveAuditStatus AuditStatus { get; set; }
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
        /// 其他信息
        /// </summary>
        public virtual ManulRetrieveOtherInfo OtherInfo { get; set; }
        /// <summary>
        /// 邮件通知云存储作业标识
        /// </summary>
        public virtual int? EmailNofiyJobId { get; set; }
        /// <summary>
        /// 手机短信通知云存储作业标识
        /// </summary>
        public virtual int? MobileNofiyJobId { get; set; }
        /// <summary>
        /// 应用标识:注册时的appid
        /// </summary>
        public virtual int RegistAppId { get; set; }
        /// <summary>
        /// 添加记录的appid
        /// </summary>
        public virtual int CreatAppId { get; set; }
        /// <summary>
        /// 通知状态：2短信，4邮箱，6短信加邮箱
        /// </summary>
        public virtual AuditNotifyStatus NofiyFlag { get; set; }
    }
}
