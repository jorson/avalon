using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.CloudClient
{
    public class Job
    {
        public virtual int Id { get; set; }
        public virtual string Identity { get; set; }
        public virtual IDictionary<string,string> Datas { get; set; }
        public virtual JobStatus Status { get; set; }
        public virtual int SessionId { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual DateTime CreateTime { get; set; }
        public virtual int Priority { get; set; }
    }

    /// <summary>
    /// 工作状态
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 队列中
        /// </summary>
        Queue = 0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 1,
        /// <summary>
        /// 失败重试中
        /// </summary>
        FailRetry = 2,
        /// <summary>
        /// 已经完成
        /// </summary>
        Done = 8,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 9,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 64
    }
}
