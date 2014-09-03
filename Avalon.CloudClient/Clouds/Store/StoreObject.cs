using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace Avalon.CloudClient
{
    public class StoreObject
    {
        /// <summary>
        ///  对象Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 原文件名
        /// </summary>
        public string SourceFileName { get; set; }
        /// <summary>
        /// 创建该对象的App标识
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 桶Id
        /// </summary>
        public int BucketId { get; set; }
        /// <summary>
        /// 桶名称
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public StoreObjectStatus Status { get; set; }
        /// <summary>
        /// 创建用户的标识
        /// </summary>
        public long CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 上传方式
        /// </summary>
        public UploadMode UploadMode { get; set; }
        /// <summary>
        /// MD5
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 对象的Guid标识
        /// </summary>
        public Guid ResourceGuid { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        public List<KeyValuePair<string,string>> Metadata { get; set; }

        private Dictionary<string, string> _MetadataDict;
        /// <summary>
        /// 元数据字典
        /// </summary>
        public Dictionary<string, string> MetadataDict
        {
            get
            {
                if (_MetadataDict == null)
                {
                    _MetadataDict=new Dictionary<string, string>();
                    if (Metadata != null)
                    {
                        foreach (var kv in Metadata)
                        {
                            _MetadataDict[kv.Key] = kv.Value;
                        }
                    }
                }
                return _MetadataDict;
            }
        }
    }

    public class StoreObjectUrl
    {
        public string Url { get; set; }
    }

    public enum StoreObjectStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create = 0,
        /// <summary>
        /// 上传、续传(uploading)
        /// </summary>
        Resumable = 1,
        /// <summary>
        /// 同步（可选）
        /// </summary>
        Sync = 2,
        /// <summary>
        /// 就绪
        /// </summary>
        Ready = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 错误（如参数错误等等）
        /// </summary>
        Error = 5,
    }

    /// <summary>
    /// 上传文件方式
    /// </summary>
    public enum UploadMode
    {
        /// <summary>
        /// WebSwf
        /// </summary>
        WebSwf = 0,
        /// <summary>
        /// WebHtml5
        /// </summary>
        WebHtml5 = 1,
        /// <summary>
        /// WebActivex
        /// </summary>
        WebActivex = 2,
        /// <summary>
        /// CloudClient
        /// </summary>
        CloudClient = 3,
        /// <summary>
        /// Other
        /// </summary>
        Other = 100,
    }
}
