using System;
using System.Collections.Generic;
using System.Text;

namespace Avalon.Resource
{
    internal class ResourceCollector : IResourceCollector
    {
        private static readonly string JS_HTML_TAG = "<script type=\"text/javascript\" src=\"{0}\"></script>";
        private static readonly string CSS_HTML_TAG = "<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />";

        /// <summary>
        /// 随机数生成器
        /// </summary>
        private static Random random = new Random();

        private List<string> resourceList = new List<string>();

        public void Add(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            resourceList.Add(path);
        }

        public List<string> ResourceList
        {
            get { return resourceList; }
        }

        private bool debug = GlobalConfig.Debug;
        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public string Group { get; set; }

        public string BaseFolder { get; set; }

        private ResourceType resourceType = ResourceType.Undefined;
        public ResourceType ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }

        public string BuildHtmlTag()
        {
            //没有添加资源，返回空
            if (resourceList.Count == 0 && string.IsNullOrEmpty(Group))
                return string.Empty;
            //资源类型未定义，自动识别
            if (resourceType == ResourceType.Undefined)
                resourceType = GetResourceType();
            //无法确认资源类型，返回空
            if (resourceType == ResourceType.Undefined)
                return string.Empty;

            if (Debug)
                return BuildDebugResourcePath();
            else
                return BuildResourcePath();
        }

        private ResourceType GetResourceType()
        {
            ResourceType resourceType = ResourceType.Undefined;
            if (resourceList.Count > 0 && resourceType == ResourceType.Undefined)
            {
                if (System.IO.Path.GetExtension(resourceList[0]).ToLower().Equals(".js"))
                    resourceType = ResourceType.JS;
                else
                    resourceType = ResourceType.CSS;
            }
            return resourceType;
        }

        private string BuildDebugResourcePath()
        {
            int serverIndex;
            string path;
            string resourceVersion;
            var serverCount = GlobalConfig.ServerList.Count;
            var urlBuilder = new StringBuilder();
            if (resourceType == ResourceType.JS)
            {
                foreach (string item in resourceList)
                {
                    serverIndex = random.Next(0, serverCount);
                    path = string.IsNullOrEmpty(BaseFolder) ? item : string.Concat(BaseFolder, "/", item);
                    resourceVersion = VersionManage.GetVersion(path);
                    if (!string.IsNullOrEmpty(resourceVersion))
                    {
                        resourceVersion = "?v=" + resourceVersion;
                    }
                    urlBuilder.AppendFormat(JS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], path, resourceVersion));
                    urlBuilder.Append(Environment.NewLine);
                }
            }
            else
            {
                foreach (string item in resourceList)
                {
                    serverIndex = random.Next(0, serverCount);
                    path = string.IsNullOrEmpty(BaseFolder) ? item : string.Concat(BaseFolder, "/", item);
                    resourceVersion = VersionManage.GetVersion(path);
                    if (!string.IsNullOrEmpty(resourceVersion))
                    {
                        resourceVersion = "?v=" + resourceVersion;
                    }
                    urlBuilder.AppendFormat(CSS_HTML_TAG, string.Concat(GlobalConfig.ServerList[serverIndex], path, resourceVersion));
                    urlBuilder.Append(Environment.NewLine);
                }
            }
            return urlBuilder.ToString();
        }

        private string BuildResourcePath()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            string resourcePath = GetMergeResourcePath();
            if (resourceType == ResourceType.JS)
                htmlBuilder.AppendFormat(JS_HTML_TAG, resourcePath);
            else
                htmlBuilder.AppendFormat(CSS_HTML_TAG, resourcePath);
            return htmlBuilder.ToString();
        }

        private string GetMergeResourcePath()
        {
            string serverUrl = string.Empty;
            if (GlobalConfig.ServerList.Count > 0)
            {
                int serverIndex = random.Next(0, GlobalConfig.ServerList.Count);
                serverUrl = GlobalConfig.ServerList[serverIndex];
            }

            var url = new StringBuilder();
            if (!string.IsNullOrEmpty(Group))
            {
                url.Append(string.Concat(serverUrl, "?g=", Group));
                var groupVersion = VersionManage.GetVersion(Group);
                if (!string.IsNullOrEmpty(groupVersion))
                    url.Append(string.Concat("?v=", groupVersion));
            }
            else
            {
                url.Append(!string.IsNullOrEmpty(BaseFolder)
                    ? string.Concat(serverUrl, "?b=", BaseFolder, "&f=")
                    : string.Concat(serverUrl, "?f="));

                for (var i = 0; i < resourceList.Count; i++)
                {
                    var item = resourceList[i];
                    var itemVersion = VersionManage.GetVersion(string.IsNullOrEmpty(BaseFolder) ? item : string.Concat(BaseFolder, "/", item));
                    url.Append(string.IsNullOrEmpty(itemVersion) ? item : string.Concat(item, "?v=", itemVersion));
                    if (i < resourceList.Count - 1)
                        url.Append(",");
                }
            }
            return url.ToString();
        }
    }
}
