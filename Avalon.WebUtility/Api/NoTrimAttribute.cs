using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.WebUtility
{
    /// <summary>
    /// 表示不会对 string 类型的参数、属性或字段进行的去空格处理（默认都会做去头尾空格处理）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class NoTrimAttribute : Attribute
    {
    }
}
