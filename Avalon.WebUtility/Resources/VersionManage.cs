using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.Resource
{
    public static class VersionManage
    {
        private static readonly object syncLocker = new object();

        /// <summary>
        /// 上一次获取版本时间
        /// </summary>
        private static DateTime lastGetAllVersion;
        /// <summary>
        /// 是否正在请求中
        /// </summary>
        private static bool isRequesting;
        /// <summary>
        /// 版本号
        /// </summary>
        private static IList<VersionConfig> versionConfigs;

        /// <summary>
        /// 获取版本号 
        /// </summary>
        /// <param name="path">路径[不区分大小写]</param>
        /// <returns></returns>
        public static string GetVersion(string path)
        {
            var now = NetworkTime.Now;
            if (!isRequesting && (now - lastGetAllVersion).TotalSeconds > GlobalConfig.VersionIntervalSeconds)
            {
                StartGetAllVersion();
            }
            Arguments.NotNullOrEmpty(path, "path");

            if (versionConfigs != null && versionConfigs.Count > 0)
            {
                var versionConfig = versionConfigs.FirstOrDefault(t => t.Type != VersionType.Default && path.StartsWith(t.Path, StringComparison.OrdinalIgnoreCase));
                if (versionConfig != null)
                {
                    return versionConfig.Type == VersionType.UnNeed ? string.Empty : versionConfig.Version;
                }
                versionConfig = versionConfigs.FirstOrDefault(t => t.Type == VersionType.Default);
                if (versionConfig != null)
                {
                    return versionConfig.Version;
                }
            }
            return now.ToString("yyyyMMddHHmmss");
        }

        public static void StartGetAllVersion()
        {
            if (isRequesting)
                return;
            Action ac = GetAllVersion;
            ac.BeginInvoke(null, null);
        }

        private static void GetAllVersion()
        {
            if (isRequesting)
                return;
            lock (syncLocker)
            {
                isRequesting = true;
            }
            try
            {
                var httpClient = new OpenApiHttpClient();
                var result = httpClient.HttpGet<IList<VersionConfig>>(UriPath.Combine(GlobalConfig.CloudServer, "v3/general/staticversionconfig/list"));
                lock (syncLocker)
                {
                    versionConfigs = result.OrderBy(t => t.SortNumber).ToList();
                }
                lastGetAllVersion = NetworkTime.Now;
            }
            catch
            {

            }
            finally
            {
                lock (syncLocker)
                    isRequesting = false;
            }
        }
    }
}
