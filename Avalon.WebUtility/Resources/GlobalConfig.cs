using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Avalon.Resource
{
    internal class GlobalConfig
    {
        private static NestedConfig _config;

        static GlobalConfig()
        {
            _config = InitializeConfig();
            VersionManage.StartGetAllVersion();
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version
        {
            get
            {
                return _config.Version;
            }
            set
            {
                _config.Version = value;
            }
        }

        /// <summary>
        /// 是否调试
        /// </summary>
        public static bool Debug
        {
            get
            {
                return _config.Debug;
            }
        }

        /// <summary>
        /// 静态资源服务器列表
        /// </summary>
        public static List<string> ServerList
        {
            get
            {
                return _config.ServerList;
            }
        }

        /// <summary>
        /// 基础平台服务器
        /// </summary>
        public static string CloudServer { get { return _config.CloudServer; } }

        /// <summary>
        /// 获取版本的间隔时间
        /// </summary>
        public static int VersionIntervalSeconds { get { return _config.VersionIntervalSeconds; } }

        /// <summary>
        /// 读取配置
        /// </summary>
        private static NestedConfig InitializeConfig()
        {
            var configs = ConfigurationManager.GetSection("nd.resource") as NameValueCollection;
            if (configs == null)
                throw new Exception("缺少nd.resource配置节");

            var config = new NestedConfig();
            //Debug
            if (!string.IsNullOrEmpty(configs["Debug"]))
                config.Debug = Convert.ToBoolean(configs["Debug"]);
            //Version
            if (!string.IsNullOrEmpty(configs["Version"]))
                config.Version = Convert.ToString(configs["Version"]);
            else
                config.Version = DateTime.Now.ToString("yyyyMM");
            //ServerList
            if (string.IsNullOrEmpty(configs["ServerList"]))
                throw new Exception("配置节ServerList不能为空");
            var servers = configs["ServerList"].Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string url in servers)
            {
                string server = string.Empty;
                if (!url.StartsWith("http"))
                    server = string.Concat("http://", url);
                else
                    server = url;

                if (!url.EndsWith("/"))
                    server += "/";

                config.ServerList.Add(server);
            }
            //CloudServer
            config.CloudServer = configs["CloudServer"];
            if (string.IsNullOrEmpty(config.CloudServer))
                throw new Exception("配置节CloudServer不能为空");
            //VersionIntervalSeconds
            if (!string.IsNullOrEmpty(configs["VersionIntervalSeconds"]))
            {
                var versionIntervalSeconds = Convert.ToInt32(configs["VersionIntervalSeconds"]);
                if (versionIntervalSeconds > 0)
                    config.VersionIntervalSeconds = versionIntervalSeconds;
            }
            return config;
        }

        private class NestedConfig
        {
            public NestedConfig()
            {
                ServerList = new List<string>();
                VersionIntervalSeconds = 300;
            }

            public string Version { get; set; }
            public bool Debug { get; set; }
            public List<string> ServerList { get; set; }
            public string CloudServer { get; set; }
            public int VersionIntervalSeconds { get; set; }
        }
    }
}
