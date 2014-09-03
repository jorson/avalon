using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Avalon.Utility
{
    public static class IpAddress
    {
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;
            if (HttpContext.Current == null || !HttpContext.Current.IsAvailable())
                return result;

            result = HttpContext.Current.Request.Headers["X-Real-IP"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result))
                return "0.0.0.0";

            return result;
        }

        public static long IpToInt(string ipAddress)
        {
            if (ipAddress.IsNullOrWhiteSpace())
                ipAddress = IpAddress.GetIP();
            var ip = IPAddress.Parse(ipAddress);
            var bytes = ip.GetAddressBytes().Reverse().ToArray();
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string IpIntoToString(long ipInt)
        {
            var ip1 = (int)(ipInt / Math.Pow(256d, 3d));
            var ip2 = (int)((ipInt - ip1 * Math.Pow(256d, 3d)) / Math.Pow(256d, 2d));
            var ip3 = (int)((ipInt - ip1 * Math.Pow(256d, 3d) - ip2 * Math.Pow(256d, 2d)) / 256);
            var ip4 = (int)((ipInt - ip1 * Math.Pow(256d, 3d) - ip2 * Math.Pow(256d, 2d) - ip3 * 256));
            return ip1 + "." + ip2 + "." + ip3 + "." + ip4;
        }

        public static string IpShow(string ipstr)
        {
            return ipstr.Substring(0, ipstr.LastIndexOf('.')) + ".*";
        }
    }
}
