using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 在生成开发文档是将被忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Method)]
    public class DocumentIgnoreAttribute : Attribute
    {
    }
}
