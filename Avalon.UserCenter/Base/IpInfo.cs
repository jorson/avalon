using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    /// <summary>
    /// IP地址所对应省市表
    /// </summary>
    public class IpAddressCity
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual long Id { get; protected set; }

        /// <summary>
        /// 所属城市ID
        /// </summary>
        public virtual int CityId { get; set; }

        /// <summary>
        /// 起始IP
        /// </summary>
        public virtual long StartIp { get; set; }

        /// <summary>
        /// 终止IP
        /// </summary>
        public virtual long EndIp { get; set; }
    }

    /// <summary>
    /// 省份
    /// </summary>
    public class IpProvince
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get;  set; }
    }

    /// <summary>
    /// 城市
    /// </summary>
    public class IpCity
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get;  set; }

        /// <summary>
        /// 所属省份ID
        /// </summary>
        public virtual int ProvinceId { get;  set; }

        /// <summary>
        /// 城市排序
        /// </summary>
        public virtual int CityIndex { get; set; }
    }
}
