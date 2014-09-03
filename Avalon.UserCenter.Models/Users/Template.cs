using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter.Models
{
    public class EmailTemplate
    {
        /// <summary>
        /// Email地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string SenderDispalyName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string BodyTemplate { get; set; }
    }

    public class MobileTemplate
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string BodyTemplate { get; set; }
    }
}
