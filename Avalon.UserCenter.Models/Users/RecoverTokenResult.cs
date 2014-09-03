namespace Avalon.UserCenter.Models
{
    public class RecoverTokenResult
    {
        /// <summary>
        /// 找回事务令牌(RecoverToken)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public long UserId { get; set; }
    }
}