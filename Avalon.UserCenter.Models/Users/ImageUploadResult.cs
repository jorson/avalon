namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 参见 http://dev.doc.huayu.nd/doc/method?project=-800341387&amp;group=Nd.Cloud.Api.Areas.V2.Store.ObjectStoreController&amp;method=Upload
    /// </summary>
    public class ImageUploadResult
    {
        /// <summary>
        /// 上传图片的Url，上传操作查看参见
        /// </summary>
        public string Url { get; set; } 
        /// <summary>
        /// 上传图片会话标识
        /// </summary>
        public string SessionId { get; set; }
    }
}