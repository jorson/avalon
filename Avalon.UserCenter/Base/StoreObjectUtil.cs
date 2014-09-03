using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalon.UserCenter.Models;
using Avalon.Utility;
using Avalon.CloudClient;

namespace Avalon.UserCenter
{
    public static class StoreObjectUtil
    {
        /// <summary>
        /// 默认图片MetaData会话键名称
        /// </summary>
        public const string MetaDefaultImageSessionKey = "x-acs-meta-DefaultSession";
        /// <summary>
        /// 头像图片MetaData会话键名称
        /// </summary>
        public const string MetaDataAvatarImageSessionKey = "x-acs-meta-AvatarSession";
        /// <summary>
        /// 证件图片MetaData会话键名称
        /// </summary>
        public const string MetaDataIdCardImageSessionKey = "x-acs-meta-IdCardSession";

        public static void ValidUploadImageObject(this StoreObject storeObject, ImageType imageType, string imageSessionId)
        {
            if(storeObject==null)
                throw new AvalonException("上传的图片对象不是有效的");

            if (storeObject.AppId != AucConfig.AucAppId)
            {
                throw new AvalonException("上传图片对象不属于网站的数据");
            }

            var metaSession = string.Empty;
            switch (imageType)
            {
                  case ImageType.AvatarImage:
                    if (storeObject.BucketName != AucConfig.AvatarImageBucketName)
                    {
                        throw new AvalonException("上传图片对象不属于头像存储桶里面");
                    }
                    storeObject.MetadataDict.TryGetValue(MetaDataAvatarImageSessionKey,out metaSession);
                    break;
                  case ImageType.IdCardImage:
                    if (storeObject.BucketName != AucConfig.IdCardImageBucketName)
                    {
                        throw new AvalonException("上传图片对象不属于证件存储桶里面");
                    }
                    storeObject.MetadataDict.TryGetValue(MetaDataIdCardImageSessionKey, out metaSession);
                    break;
                default:
                    if (storeObject.BucketName != AucConfig.DefualtBucketName)
                    {
                        throw new AvalonException("上传图片对象不属于网站的默认存储桶里面");
                    }
                    storeObject.MetadataDict.TryGetValue(MetaDefaultImageSessionKey, out metaSession);
                    break;
            }

            if (!string.Equals(imageSessionId, metaSession, StringComparison.OrdinalIgnoreCase))
            {
                throw new AvalonException("上传的图片对象与所对应的会话不匹配");
            }
        }
    }
}
