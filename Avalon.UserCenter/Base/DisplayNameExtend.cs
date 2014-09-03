using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.UserCenter
{
    public static class DisplayNameExtend
    {
        public static string CityDisplayName(this IpCity value)
        {
            return value == null ? "-" : value.Name;
        }

        public static string CityDisplayEmpty(this IpCity value)
        {
            return value == null ? string.Empty : value.Name;
        }

        public static string CityDisplayNameByProvince(this IpCity value,AppBaseService appBaseService)
        {
            if (value == null)
                return "-";
            if (value.ProvinceId > 0)
            {
                var province = appBaseService.GetIpProvinceById(value.ProvinceId);
                if (province != null)
                    return province.Name + " " + value.Name;
            }
            return value.Name;
        }

        public static string ProvinceDisplayName(this IpProvince value)
        {
            return value == null ? "-" : value.Name;
        }

        public static string ProvinceDisplayEmpty(this IpProvince value)
        {
            return value == null ? string.Empty : value.Name;
        }
    }
}
