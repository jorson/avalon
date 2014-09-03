namespace Avalon.UserCenter.Models
{
    public class LoginOrRegisterResult
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// 验证码显示格式
        /// </summary>
        public PicVerifyCodeDisplayFormat DisplayFormat { get; set; }

        /// <summary>
        /// 必须图片验证码
        /// </summary>
        public bool NeedPicVerifyCode { get; set; }

        /// <summary>
        /// 用户显示名称(带*)
        /// </summary>
        public string UserDisplayNameWithCover { get; set; }

        /// <summary>
        /// 自定义帐号ID
        /// </summary>
        public long AccountId { get; set; }
    }
}