namespace Avalon.UserCenter.Models
{
    /// <summary>
    /// 帐号方案状态
    /// </summary>
    public enum SolutionStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 
        /// </summary>
        Disable = 10,
    }

    /// <summary>
    /// 帐号方案类型
    /// </summary>
    public enum SolutionType
    {
        /// <summary>
        /// 自定义帐号
        /// </summary>
        Custom=0,
        /// <summary>
        /// 新浪微博
        /// </summary>
        SinaWeibo = 1,
        /// <summary>
        /// QQ帐号
        /// </summary>
        QQAccount = 2,
        /// <summary>
        /// 网龙应软UAP
        /// </summary>
        Uap = 3,
        /// <summary>
        /// 农大一卡通
        /// </summary>
        IdStar = 4,
    }
}