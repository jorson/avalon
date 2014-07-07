 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Resource
{
    /// <summary>
    /// 版本配置
    /// </summary>
    public class VersionConfig
    {
        /// <summary>
        /// 路径或完整路径的文件
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 版本类型
        /// </summary>
        public VersionType Type { get; set; }
        /// <summary>
        /// 排序号[小的排前]
        /// </summary>
        public int SortNumber { get; set; }
    }

    /// <summary>
    /// 版本类型
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// 默认版本号
        /// </summary>
        Default = 0,
        /// <summary>
        /// 需要版本号
        /// </summary>
        Need = 1,
        /// <summary>
        /// 不需要版本号
        /// </summary>
        UnNeed = 2
    }
}
