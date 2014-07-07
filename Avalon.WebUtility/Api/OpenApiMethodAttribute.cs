using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OpenApiMethodAttribute : Attribute
    {
        public OpenApiMethodAttribute()
        {
        }

        public OpenApiMethodAttribute(Type requestType)
        {
            RequestTypes = new Type[] { requestType };
        }

        /// <summary>
        /// 支持的HTTP方法
        /// </summary>
        public string Methods { get; set; }

        public Type[] RequestTypes { get; set; }

        public Type[] ResponseTypes { get; set; }

        /// <summary>
        /// 接口摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
