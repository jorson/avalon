using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Avalon.OAuthClient;

namespace Avalon.CloudClient
{
    /// <summary>
    /// 表示 SDK 运行的上下文对象
    /// </summary>
    public class ClientContext
    {
        static ClientContext context;

        static ClientContext()
        {
            var apiPath = ConfigurationManager.AppSettings["Cloud.ApiPath"];
            if (String.IsNullOrWhiteSpace(apiPath))
                throw new ConfigurationErrorsException("缺少 Cloud.ApiPath 配置项。");

            ApiPath = apiPath;

            var apiHost = ConfigurationManager.AppSettings["Cloud.ApiHost"];
            if (!String.IsNullOrWhiteSpace(apiHost))
                ApiHost = apiHost;
        }

        /// <summary>
        /// 建立上下文对象
        /// </summary>
        public static void Setup()
        {
            context = new ClientContext();

            OAuthService.Init();
        }

        /// <summary>
        /// 获取当前的上下文对象
        /// </summary>
        public static ClientContext Current { get { return context; } }

        /// <summary>
        /// Gets or sets the API host.
        /// </summary>
        /// <value>
        /// The API host.
        /// </value>
        public static string ApiPath { get; set; }

        public static string ApiHost { get; set; }
    }
}
