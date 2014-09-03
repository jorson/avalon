namespace Avalon.UserCenter.Models
{
    public class PicVerifyCodeResult
    {
        /// <summary>
        /// 是否有验证码
        /// </summary>
        public bool HasCode { get; set; }
        /// <summary>
        /// 会话
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }
    }
}